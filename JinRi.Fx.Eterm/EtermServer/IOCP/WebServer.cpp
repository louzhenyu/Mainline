#include "stdafx.h"
#include "webserver.h"
#include <string>
#include "../Global.h"
#include "../Data.h"
#include "../strcoding.h"
#include "../lip.h"
#include <algorithm>
#include "../EtermCommand.h"
#include "../stringEx.h"
#include <thread>

using namespace std;

extern void DataDispose(SOCKET sock, wstring ip, wstring content, wstring source);

CWebServer::CWebServer(void)
{
	m_szAddress[0] = 0;
	m_bValidIP=false;
	m_mapWebSocketBuffer.clear();
	InitializeCriticalSection(&m_struCriSec);
}

CWebServer::~CWebServer(void)
{
	m_mapWebSocketBuffer.clear();
	DeleteCriticalSection(&m_struCriSec);
}
//此函数把底层模块接收到的字节保存到自己的缓冲区中
void CWebServer::SaveReceivedData(SOCKET sock,char* pVoid,long dataLen)
{
	::EnterCriticalSection(&m_struCriSec);
	map<SOCKET, BLOCK>::iterator pos;
	pos = m_mapWebSocketBuffer.find(sock);
	if (pos == m_mapWebSocketBuffer.end())
	{
		BLOCK block;
		block.time = CTime::GetCurrentTime();
		m_mapWebSocketBuffer.insert(make_pair(sock, block));
		pos = m_mapWebSocketBuffer.find(sock);
	}
	::LeaveCriticalSection(&m_struCriSec);

	if (CTime::GetCurrentTime() - pos->second.time > CTimeSpan(0, 0, 0, 2))
	{
		SendData(sock, "connected", strlen("connected"));

		::EnterCriticalSection(&m_struCriSec);
		m_mapWebSocketBuffer.erase(sock);
		::LeaveCriticalSection(&m_struCriSec);

		return;
	}

	pVoid[dataLen] = '\0';
		
	Global::WriteLog(CLog(pVoid));

	if (pos->second.proto == POST)
	{
		if (pos->second.nlen > pos->second.data.length())
		{
			pos->second.data.append(string(pVoid));
		}
		if (pos->second.nlen > pos->second.data.length())
		{
			return;
		}
	}
	else
	{
		CHttpParse parse;
		if (parse.httpParse(pVoid, dataLen))
		{
			if (parse.proto == GET)
			{
				pos->second.proto = parse.proto;
				pos->second.content = parse.Content;
				pos->second.nlen = pos->second.content.length();
			}
			else if (parse.proto == POST)
			{
				pos->second.proto = parse.proto;
				pos->second.content = parse.Content;
				pos->second.nlen = parse.nLen;
				pos->second.data = parse.PostData;
				if (pos->second.nlen > pos->second.data.length())
				{
					return;
				}
			}
		}
		else
		{
			if (parse.proto == POST)
			{
				pos->second.proto = parse.proto;
				pos->second.content = parse.Content;
				pos->second.nlen = parse.nLen;
				pos->second.data = parse.PostData;
			}
			return;
		}
	}
	
	strCoding coding;
	string buffer;
	string content = coding.UrlUTF8Decode(pos->second.content);
	
	if (!pos->second.data.empty()) buffer = coding.UrlUTF8Decode(pos->second.data);

	::EnterCriticalSection(&m_struCriSec);
	m_mapWebSocketBuffer.erase(sock);
	::LeaveCriticalSection(&m_struCriSec);

	if (!m_bValidIP)
	{
		if (content.find("gnoyihzuilmai")==string::npos)
		{
			this->SendMsg(sock, _T("非法IP！"));
			this->CloseClientSocket(sock);
			return;
		}
		else
		{
			content = content.substr(13);
		}
	}
	BusinessProcess(content, buffer, sock);

}

void CWebServer::ClientConnect(SOCKET sock,char* ip,UINT nPort)
{
	sprintf(m_szAddress, "%s:%d", ip, nPort);
	
	lip fip;
	fip.szIP = CString(ip);
	auto it = find_if(Global::iplist.begin(), Global::iplist.end(), [=](lip _fip){ return fip.szIP.Find(_fip.szIP) != -1; });
	if (it==Global::iplist.end())
	{		
		m_bValidIP=false;
	}
	else
	{
		m_bValidIP=true;
	}
	
	::EnterCriticalSection(&m_struCriSec);
	map<SOCKET, BLOCK>::iterator pos;
	pos = m_mapWebSocketBuffer.find(sock);
	if (pos == m_mapWebSocketBuffer.end())
	{
		BLOCK block;
		block.time = CTime::GetCurrentTime();
		m_mapWebSocketBuffer.insert(make_pair(sock, block));
	}
	::LeaveCriticalSection(&m_struCriSec);

	Global::WriteLog(CLog((char*)m_szAddress));
}

CString CWebServer::HttpString(CString strResponse)
{
	CString response;
	response.Format(
		_T("HTTP/1.1 200 OK\r\n")
		_T("Content-Type:text/html\r\n")
		_T("Content-Length:%d\r\n")
		_T("Connection:close\r\n\r\n%s"),
		strResponse.GetLength(),
		strResponse
		);
	return response;
}

void CWebServer::SendMsg(SOCKET sock,CString strResponse)
{		
	USES_CONVERSION;
	
	char* szResponse = T2A(strResponse);	
	char* szBuf = new char[strlen(szResponse) + 100];
	memset(szBuf, 0, sizeof(szBuf));
	sprintf(szBuf,
		"HTTP/1.1 200 OK\r\n"
		"Content-Type:text/html\r\n"
		"Content-Length:%d\r\n"
		"Connection:close\r\n\r\n%s",
		strlen(szResponse),
		szResponse);
		
	this->SendData(sock, szBuf, strlen(szBuf));
	
	Global::WriteLog(CLog(szResponse));

	delete[] szBuf;
}

void CWebServer::SendMsg(SOCKET sock, string strResponse)
{
	m_ss.clear(); m_ss.str("");
	m_ss << "HTTP/1.1 200 OK\r\n";
	m_ss << "Content-Type:text/html\r\n";
	m_ss << "Content-Length:" << strResponse.length() << "\r\n";
	m_ss << "Connection:close\r\n\r\n";
	m_ss << strResponse;

	string sret = m_ss.str();

	this->SendData(sock, sret.c_str(), sret.length());	
}


inline string GetEtermState(ETERM_STATE nret)
{
	switch (nret)
	{
	case NO_FIND_CONFIG:
		return "没有发现可用配置";
	case STRING_FORMAT:
		return "变量赋值错误";
	case INT_FORMAT:
		return "int变量赋值错误";
	case FLOAT_FORMAT:
		return "float变量赋值错误";
	case LIST_FORMAT:
		return "list变量赋值错误";
	case SET_VALUE:
		return "设置变量失败";
	case INVALID_BUSINESS:
		return "无效的业务名称";
	case NONE_TYPE:
		return "未知类型";
	case SUCCESS:
		return "";
	default:
		return "未知错误";
	}
}

inline RespnseFormat GetForamt(wstring format)
{
	if (format == _T("text")) return text;
	else if (format == _T("json")) return json;
	else return text;
}

inline EtermLanguage GetLanguage(wstring lang)
{
	if (lang == _T("Script")) return Script;
	else if (lang == _T("CSharp")) return CSharp;
	else if (lang == _T("CPP")) return CPP;
	else return Script;
}

void CWebServer::BusinessProcess(string content, string buffer, SOCKET sock)
{
	if (content.find("favicon.ico") != string::npos) return;
	if (content.empty() && buffer.empty())
	{
		Global::Server.SendMsg(sock, _T("发送命令不能为空"));
		return;
	}

	/*CData* pData = new CData;
	pData->sock = sock;
	pData->ip = CA2T(this->m_szAddress);
	pData->content = CA2T(content.c_str());
	pData->source = CA2T(buffer.c_str());*/
	wstring sip = CA2T(this->m_szAddress);
	wstring scontent = CA2T(content.c_str());
	wstring ssource = CA2T(buffer.c_str());
	thread th(DataDispose, sock, sip, scontent, ssource);
	th.detach();
	
	//DataDispose(pData);
	//unsigned int dwThreadId;
	//_beginthreadex(NULL, 0, &DataDispose, pData, 0, &dwThreadId);
}