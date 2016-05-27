#include "StdAfx.h"
#include "EtermSocket.h"
#include "ParseReceive.h"
#include "../Global.h"
#include "Mstcpip.h"
#include <atlbase.h>
#include <atlconv.h>
#include <regex>
#include "iphlpapi.h"
#include "../Command.h"
#include "Ws2tcpip.h"
#include "../SocketHelper.h"
#pragma comment ( lib, "Iphlpapi.lib" )
#include <sqlite3.h>

#define PRINTDEBUG(a) PrintError(#a,__FILE__,__LINE__,GetLastError()) 
 
inline int PrintError(LPSTR linedesc, LPSTR filename, int lineno, DWORD errnum) 
{ 
	LPSTR lpBuffer; 
	char errbuf[2048]; 
	DWORD numread; 
	 
	__try
	{
	FormatMessage( FORMAT_MESSAGE_ALLOCATE_BUFFER 
		| FORMAT_MESSAGE_FROM_SYSTEM, 
		NULL, 
		errnum, 
		LANG_NEUTRAL, 
		(LPTSTR)&lpBuffer, 
		0, 
		NULL ); 
	 
	sprintf(errbuf,"\nThe following call failed at line %d in %s:\n\n%s\n\nReason: %s\n", lineno, filename, linedesc, lpBuffer); 	 
	Global::WriteLog(errbuf);
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("Global::PrintErro函数错误。");
	}
	return errnum; 
} 
unsigned int _stdcall ReStartEterm(void* lparam);

CEtermSocket::CEtermSocket()
{
	m_nState=0;
	m_Rate=0;
	m_nSleep=0;
	m_bRunXS=false;
	pEvent=nullptr;
	pReEvent=nullptr;
	m_pSslLayer=nullptr;
	m_pProxyLayer=nullptr;
	m_pSslTrustedCertHashList=nullptr;
}

CEtermSocket::CEtermSocket(CRichEditCtrl *pResponse, t_HashList *pSslTrustedCertHashList)
{
	__try
	{
		ASSERT(pResponse);
		ASSERT(pSslTrustedCertHashList);

		m_pResponse = pResponse;
		m_pSslTrustedCertHashList = pSslTrustedCertHashList;

		m_pProxyLayer = new CAsyncProxySocketLayer;
		m_pSslLayer = new CAsyncSslSocketLayer;
		
		m_ActiveTime=CTime::GetCurrentTime();
		pEvent=CreateEvent(NULL,TRUE, FALSE,NULL);
	
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::CEtermSocket函数错误。");
	}
}

void CEtermSocket::InitSocket(CRichEditCtrl *pResponse,t_HashList *pSslTrustedCertHashList)
{	
	__try
	{
		ASSERT(pResponse);
		ASSERT(pSslTrustedCertHashList);

		m_pResponse = pResponse;
		m_pSslTrustedCertHashList = pSslTrustedCertHashList;

		m_pProxyLayer = new CAsyncProxySocketLayer;
		m_pSslLayer = new CAsyncSslSocketLayer;
		
		m_ActiveTime=CTime::GetCurrentTime();
		pEvent=CreateEvent(NULL, TRUE, FALSE, NULL);
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::CEtermSocket函数错误。");
	}
}

CEtermSocket::~CEtermSocket()
{
	__try
	{
		
		Close();
		if (pEvent!=nullptr)
			CloseHandle(pEvent);
		if (pReEvent!=nullptr)
			CloseHandle(pReEvent);
		if (m_pSslLayer!=nullptr)
			delete m_pSslLayer;
		if (m_pProxyLayer!=nullptr)
			delete m_pProxyLayer;
		
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::CEtermSocket函数错误。");
	}
}

void CEtermSocket::ReleaseSocket(void)
{
	__try
	{
		
		Close();
		if (pEvent!=nullptr)
		{
			CloseHandle(pEvent);
			pEvent=nullptr;
		}
		if (pReEvent!=nullptr)
		{
			CloseHandle(pReEvent);
			pReEvent=nullptr;
		}
		if (m_pSslLayer!=nullptr)
		{
			delete m_pSslLayer;
			m_pSslLayer=nullptr;
		}
		if (m_pProxyLayer!=nullptr)
		{
			delete m_pProxyLayer;
			m_pProxyLayer=nullptr;
		}
		
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::CEtermSocket函数错误。");
	}
}

void CEtermSocket::OnReceive(int nErrorCode)
{
	if (nErrorCode)
	{
		Global::WriteLog("Fatal error! Network subsystem failed!");
		Close();
		return;
	}	

	BYTE btData[2048];
	UINT nSize=0;
	nSize=Receive(btData,2048);
	
	if (!this->m_EtermPacket.ValidatePakcet(btData,nSize))
		return;
	
	
	Global::WriteFlow(this->m_config.UserName);

	CString strTitle,strResponse,strKey,strData;
		
	if (nSize!=SOCKET_ERROR)
	{
		__try
		{			

			//CParseReceive parseRec;	
			ClientData data;
			
			auto it=Global::CSocketAssemble.find(this->m_config.UserName);
			if (it!=Global::CSocketAssemble.end()) 
			{				
				it->second->m_cs.Lock();
				it->second->m_ActiveTime=CTime::GetCurrentTime();				
				if (it->second->ClientSocket.size()>0)
				{
					data = it->second->ClientSocket.front();					
					this->m_EtermPacket.ParseData(btData,nSize,data);
					it->second->ClientSocket.front()=data;
					if (this->m_EtermPacket.m_pt==PACKET_TYPE::COMPLETE)
						SetEvent(it->second->pEvent);
				}
				else
				{
					this->m_EtermPacket.ParseData(btData,nSize,data);
					if (this->m_EtermPacket.m_pt==PACKET_TYPE::LOGIN)
					{
						SetEvent(it->second->pEvent);
					}
				}				
				it->second->m_cs.Unlock();				
			}			
			
			if (this->m_EtermPacket.m_pt==PACKET_TYPE::COMPLETE)
				strResponse=data.strResponse;

			if (Global::bHandCmd)
			{
				strResponse.Append("\r\n\r\n■");		
				Global::bHandCmd=false;
			}
			else
				strResponse.Append("\r\n");	
			
		}
		__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
		{
			Global::WriteLog("CEtermSocket::OnReceive函数错误。");
		}
		

	}
	
	if (strResponse.GetLength()>10)
	{
		AddStringToLog(strResponse);		
	}
}

void CEtermSocket::OnConnect(int nErrorCode)
{
	if (0==nErrorCode)
	{			
		this->m_nState=5;		//已连接
		if (!m_config.IsSSL)
			Global::GetEtermView(this)->PostMessage(WM_CONNECTED,1,0);		
	}
	else
	{
		Close();		
		Global::GetEtermView(this)->PostMessage(WM_CONNECT,(WPARAM)(char*)this->m_config.UserName,0);
		Global::SendSMS(this->m_config.UserName);
	}	
		
	//AddStringToLog("Status: Connected to server.");
}

void CEtermSocket::OnClose(int nErrorCode)
{
	if (nErrorCode==0)
	{
		
	}

	CTimeSpan span(0,0,5,0);
	CTimeSpan difSpan=CTime::GetCurrentTime() - this->m_ActiveTime;

	this->m_cs.Lock();
	this->m_Rate--;		
	if (difSpan>span)
	{
		this->m_config.HeartWrap=difSpan.GetTotalMinutes()-1;
		UpdateHeartWrap();
	}
	this->m_cs.Unlock();

	Global::GetEtermView(this)->PostMessage(WM_CONNECT,(WPARAM)(char*)this->m_config.UserName,0);
}

bool CEtermSocket::UpdateHeartWrap()
{
	char szSql[1024]={0};
	sprintf(
		szSql,
		"UPDATE Config SET heartwrap=%d WHERE username='%s'",
		this->m_config.HeartWrap,
		this->m_config.UserName);

	char* errMsg=NULL;
	int ret=sqlite3_exec(Global::m_pDB,szSql,0,0,&errMsg);

	return ret==SQLITE_OK;
}

int CEtermSocket::OnLayerCallback(const CAsyncSocketExLayer *pLayer, int nType, int nParam1, int nParam2)
{
	__try
	{
	ASSERT(pLayer);
	
	if (nType==LAYERCALLBACK_STATECHANGE)
	{
		CString str;
		if (pLayer==m_pProxyLayer)
			str.Format(_T("Layer Status: m_pProxyLayer changed state from %d to %d\r\n"), nParam2, nParam1);
		else if (pLayer==m_pSslLayer)
			str.Format(_T("Layer Status: m_pSslLayer changed state from %d to %d\r\n"), nParam2, nParam1);
		else
			str.Format(_T("Layer Status: Layer @ %d changed state from %d to %d\r\n"), pLayer, nParam2, nParam1);	
		
		AddStringToLog(str);
		return 1;
	}
	else if (nType == LAYERCALLBACK_LAYERSPECIFIC)
	{
		if (pLayer == m_pProxyLayer)
		{
			switch (nParam1)
			{
			case PROXYERROR_NOCONN:
				AddStringToLog(_T("Proxy error: Can't connect to proxy server.\r\n"));
				break;
			case PROXYERROR_REQUESTFAILED:
				AddStringToLog(_T("Proxy error: Proxy request failed, can't connect through proxy server.\r\n"));
				if (nParam2)
					AddStringToLog((LPCTSTR)nParam2);
				break;
			case PROXYERROR_AUTHTYPEUNKNOWN:
				AddStringToLog(_T("Proxy error: Required authtype reported by proxy server is unknown or not supported.\r\n"));
				break;
			case PROXYERROR_AUTHFAILED:
				AddStringToLog(_T("Proxy error: Authentication failed\r\n"));
				break;
			case PROXYERROR_AUTHNOLOGON:
				AddStringToLog(_T("Proxy error: Proxy requires authentication\r\n"));
				break;
			case PROXYERROR_CANTRESOLVEHOST:
				AddStringToLog(_T("Proxy error: Can't resolve host of proxy server.\r\n"));
				break;
			default:
				AddStringToLog(_T("Proxy error: Unknown proxy error\r\n") );
			}
		}
		else if (pLayer == m_pSslLayer)
		{
			switch (nParam1)
			{
			case SSL_INFO:
				switch (nParam2)
				{
				case SSL_INFO_ESTABLISHED:
					AddStringToLog(_T("SSL connection established\r\n"));	
					Global::GetEtermView(this)->PostMessage(WM_CONNECTED,1,0);	
					break;
				}
				break;
			case SSL_FAILURE:
				switch (nParam2)
				{
				case SSL_FAILURE_UNKNOWN:
					AddStringToLog(_T("Unknown error in SSL layer\r\n"));
					break;
				case SSL_FAILURE_ESTABLISH:
					AddStringToLog(_T("Could not establish SSL connection\r\n"));
					break;
				case SSL_FAILURE_LOADDLLS:
					AddStringToLog(_T("Failed to load OpenSSL DLLs\r\n"));
					break;
				case SSL_FAILURE_INITSSL:
					AddStringToLog(_T("Failed to initialize SSL\r\n"));
					break;
				case SSL_FAILURE_VERIFYCERT:
					AddStringToLog(_T("Could not verify SSL certificate\r\n"));
					break;
				}				
				break;
			case SSL_VERBOSE_INFO:
			case SSL_VERBOSE_WARNING:						
				AddStringToLog(CString((char *)nParam2)+"\r\n");				
				break;
			case SSL_VERIFY_CERT:
				t_SslCertData *pData = new t_SslCertData;				
				if (m_pSslLayer->GetPeerCertificateData(*pData))
				{
					/* Do a first validation of the certificate here, return 1 if it's valid
					 * and 0 if not.
					 * If still unsure, let the user decide and return 2;
					 */
					for (t_HashList::iterator iter = m_pSslTrustedCertHashList->begin(); iter != m_pSslTrustedCertHashList->end(); iter++)
						if (!memcmp(pData->hash, *iter, 20))
							return 1;					
								
					Global::GetEtermView(this)->PostMessage(WM_USER, (WPARAM)pData, 0);
					return 2;
				}
				else
					return 1;				
			}
		}
	}
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::OnLayerCallback函数错误。");
	}

	return 1;
}


BOOL CEtermSocket::AddSslCertHashToTrustedList(unsigned char * pHash)
{
	__try
	{
	if (!pHash)
		return FALSE;
	for (t_HashList::iterator iter = m_pSslTrustedCertHashList->begin(); iter != m_pSslTrustedCertHashList->end(); iter++)
		if (!memcmp(pHash, *iter, 20))
			return FALSE;

	unsigned char* pData = new unsigned char[20];
	memcpy(pData, pHash, 20);
	m_pSslTrustedCertHashList->push_back(pData);
	
	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::OnLayerCallback函数错误。");
	}
	return TRUE;


}


void CEtermSocket::AddStringToLog(LPCTSTR pszString)
{			

	__try
	{
		if (m_pResponse==nullptr) return;
		if (!::IsWindow(m_pResponse->m_hWnd)) return;
		CHARFORMAT2 cf;
		ZeroMemory(&cf, sizeof(CHARFORMAT2));
		cf.cbSize = sizeof(CHARFORMAT2);
	
		cf.dwMask=CFM_CHARSET;
		cf.bCharSet=GB2312_CHARSET;
		m_pResponse->SetSelectionCharFormat(cf);
	
		cf.dwMask=CFM_FACE;
		strcpy(cf.szFaceName,"宋体");
		m_pResponse->SetSelectionCharFormat(cf); 

		cf.dwMask = CFM_COLOR;
		//cf.dwEffects = 0;
		cf.crTextColor = RGB(0, 255, 0); //自行修改
		m_pResponse->SetSelectionCharFormat(cf);

		cf.dwMask=CFM_BOLD;
		cf.dwEffects &=~CFE_BOLD;
		m_pResponse->SetSelectionCharFormat(cf);
		
		m_pResponse->SetSel(-1,-1);
		m_pResponse->ReplaceSel("\r\n");

		m_pResponse->SetSel(-1,-1);
		m_pResponse->ReplaceSel(pszString);
	
	
		/*CTime time=CTime::GetCurrentTime();
		CString strPath;
		strPath.Format("%s\\log\\%s.rtf",Global::GetAppPath(),time.Format("%Y%m%d"));
		CFileFind find;
		BOOL bFind = find.FindFile(strPath);
		find.Close();

		if (!bFind)
		{
			CView* pView=NULL;
			auto it=Global::CSocketAssemble.begin();
			for (;
				it!=Global::CSocketAssemble.end();
				++it)
			{
				if (it->second.get()==this)
				{
					pView=Global::GetCurView(it->first);
					break;
				}
			}
	
			if (pView!=NULL)
			{			
				pView->GetDocument()->DoSave(strPath,FALSE);
			
				m_pResponse->SetSel(0,-1);
				m_pResponse->Clear();					
			}
	}*/

	}
	__except(Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::WriteLog("CEtermSocket::AddStringToLog函数错误。");
	}
}


//用户登录
bool CEtermSocket::UserLogin(CConfigure cfg)
{	
	int ret;	
	
	BYTE Space[162];//={0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
	
	BYTE str0[162];
	BYTE str1[]={0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
	BYTE str2[]={0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x0C,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x29,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x20,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
	BYTE str3[]={0x01,0xFE,0x00,0x11,0x14,0x10,0x00,0x02,0x20,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};				  
	BYTE bVer[]={0x33,0x38,0x34,0x39,0x30,0x31,0x30,0x00,0x30,0x30,0x30,0x30,0x30,0x30};
	//BYTE bVer[]={0x33,0x38,0x34,0x33,0x30,0x31,0x30,0x00,0x30,0x30,0x30,0x30,0x30,0x30};
	//..txt100..........01..............................001c23fad023192.168.1.102  3730310.000000.......................................................................
	//01 A2 
	//74 78 74 31 30 30 00 00 00 00 00 00 00 00 00 00 
	//30 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
	//00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
	//30 30 31 63 32 33 66 61 64 30 32 33 
	//31 39 32 2E 31 36 38 2E 31 2E 31 30 32 20 20 
	//33 37 33 30 33 31 30 00 30 30 30 30 30 30 
	//00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 
	//01 FE 00 11 14 10 00 02 00 00 00 00 00 00 00 00 00 
	//01 FE 00 11 14 10 00 02 0C 00 00 00 00 00 00 00 00 01 FE 00 11 14 10 00 02 29 00 00 00 00 00 00 00 00 
	
	CSocketHelper socketHelper;
	
	int nlen=0;

	memset(Space,0x00,sizeof(Space));

	//两位魔术数据
	BYTE magic[]={0x01,0xA2};
	memcpy(str0,(void*)magic,_countof(magic));	
	nlen+=2;

	//16位用户名
	memcpy(&str0[nlen],(void*)cfg.UserName,strlen(cfg.UserName));
	nlen+=strlen(cfg.UserName);	
	memcpy(&str0[nlen],(void*)Space,16-strlen(cfg.UserName));
	nlen+=16-strlen(cfg.UserName);

	//16位密码
	memcpy(&str0[nlen],(void*)cfg.PassWord,strlen(cfg.PassWord));
	nlen+=strlen(cfg.PassWord);
	memcpy(&str0[nlen],(void*)Space,16-strlen(cfg.PassWord));
	nlen+=16-strlen(cfg.PassWord);

	//16位空位
	memcpy(&str0[nlen],(void*)Space,16);
	nlen+=16;

	//网卡地址
	char* szMac = socketHelper.GetMAC();
	memcpy(&str0[nlen],(void*)szMac,strlen(szMac));
	nlen+=strlen(szMac);
	memcpy(&str0[nlen],(void*)Space,12-strlen(szMac));
	nlen+=12-strlen(szMac);

	//IP地址
	char* szIP	= socketHelper.GetHostIP();
	memcpy(&str0[nlen],(void*)szIP,strlen(szIP));
	nlen+=strlen(szIP);
	memcpy(&str0[nlen],(void*)Space,15-strlen(szIP));
	nlen+=15-strlen(szIP);

	//版本号
	memcpy(&str0[nlen],(void*)bVer,_countof(bVer));
	nlen+=_countof(bVer);
	memcpy(&str0[nlen],(void*)Space,162-nlen);
	
	//int nlen=2;
	//BYTE btUserName[255];
	//memcpy(btUserName,(char*)cfg.UserName.c_str(),cfg.UserName.length());
	//memcpy(&str0[nlen],btUserName,cfg.UserName.length());
	//nlen+= cfg.UserName.length();
	//memcpy(&str0[nlen],Space,16- cfg.UserName.length());
	//nlen+=16-cfg.UserName.length();
	////16位密码
	//BYTE btPassWord[255];
	//memcpy(btPassWord,(char*)(LPCTSTR)cfg.PassWord,cfg.PassWord.length());
	//memcpy(&str0[nlen],btPassWord,cfg.PassWord.length());
	//nlen+=cfg.PassWord.length();
	//memcpy(&str0[nlen],Space,16-cfg.PassWord.length());
	//nlen+=16-cfg.PassWord.GetLength();
	////16位空位	
	//memcpy(&str0[nlen],Space,16);
	//nlen+=16;
	////12位校验码
	//BYTE verify[12];
	//CString strMAC=socketHelper.GetMAC();
	//memcpy(verify,(char*)(LPCTSTR)strMAC,strMAC.length());
	//memcpy(&str0[nlen],verify,strMAC.length());	
	//nlen+=12;
	////15位IP地址
	//char ip[255]={0};
	//strcpy(ip,socketHelper.GetHostIP());
	//int nIP=15-strlen(ip);
	//while(nIP)
	//{
	//	strcat(ip," ");
	//	nIP--;
	//}
	//BYTE btIP[255];
	//memcpy(btIP,ip,15);
	//memcpy(&str0[nlen],btIP,15);
	//nlen+=15;

	////14位版本号
	//BYTE btVer[255];
	//memcpy(btVer,"3843010",7);
	//memcpy(&str0[nlen],btVer,7);
	//nlen+=7;
	//
	////填充
	//BYTE btSpace[]={0x00,0x30,0x30,0x30,0x30,0x30,0x30};	
	//memcpy(&str0[nlen],btSpace,7);	
	//nlen+=7;
	//memcpy(&str0[nlen],Space,162-nlen);
	//
	
	Global::WriteFlow(cfg.UserName);	

	ret =Send(str0,162);	
	if (ret==-1) return false;
	ResetEvent(pEvent);
	DWORD dwOutTime=::WaitForSingleObject(pEvent,30*1000);
	if (dwOutTime!=WAIT_OBJECT_0)
	{
		SetEvent(pEvent);
		return false;	
	}
	ret= Send(str1,17);
	if (ret==-1) return false;
	Sleep(200);
	ret= Send(str2,34);
	if (ret==-1) return false;
	Sleep(200);	

	return true;	
}

inline vector<string> Split(string cmd)
{
	vector<string> cmdList;
	//语句结束标志
	tr1::regex reg("\\[(rn|RN)\\]");
	tr1::sregex_token_iterator it(cmd.begin(),cmd.end(),reg,-1);
	tr1::sregex_token_iterator end;
	string code;
	
	while(it!=end)
	{
		code=*it++;
		if (!code.empty())
			cmdList.push_back(code);
	}
	return cmdList;
}

inline int GetIndex(string cmd)
{
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase; 
	tr1::regex_constants::match_flag_type flag=std::tr1::regex_constants::match_default;	
	tr1::match_results<std::string::const_iterator> result;
	string::const_iterator begin,end;
	
	regex regXSPN("^XS\\s{1,}PN",fl);
	regex regPN("^PN$",fl);	
		
	begin=cmd.begin();
	end=cmd.end();	

	if (tr1::regex_search(begin,end,result,regPN,flag))	
		return 2;	
	else if (tr1::regex_search(begin,end,result,regXSPN,flag))
		return 1;
	else 
		return 0;
}

//发送数据
bool CEtermSocket::SendMsg(string sMsg)
{		
	//前缀1
	BYTE prefix1[]={0x01,0x00};
	//前缀2
	BYTE prefix2[]={0x00,0x00,0x00,0x01};
	//前缀3
	BYTE prefix[3][10]={	
		{0x51,0x70,0x02,0x1B,0x0B,0x21,0x20,0x00,0x0F,0x1E},				//正常
		{0x51,0x70,0x02,0x1B,0x0B,0x37,0x20,0x00,0x0F,0x1E},				//XS PN
		{0x51,0x70,0x02,0x1B,0x0B,0x2C,0x20,0x00,0x0F,0x1E}					//PN		
	};
	//后缀
	BYTE suffix[]={0x20,0x03};
		
	if (sMsg.empty())
		return false;

	std::regex_constants::syntax_option_type fl = std::regex_constants::icase; 
	sMsg=tr1::regex_replace(sMsg,regex("\\[(RN)\\]",fl),string("\r"));
	
	BYTE cmd[4096];

	//前缀1
	int nlen=_countof(prefix1);
	memcpy(cmd,prefix1,nlen);
		
	//数据包长度
	byte cmdLen[4]={0};
	byte cmdL[4]={0};
	*(int*)cmdLen = sMsg.length()+21;
	cmdL[0]=cmdLen[1];
	cmdL[1]=cmdLen[0];
	memcpy(&cmd[nlen],cmdL,2);
	nlen+=2;

	//前缀2
	memcpy(&cmd[nlen],prefix2,_countof(prefix2));
	nlen+=_countof(prefix2);

	//SID
	memcpy(&cmd[nlen],&this->m_EtermPacket.SID,1);
	nlen+=1;

	//前缀3
	int nIndex=GetIndex(sMsg);
	memcpy(&cmd[nlen],prefix[nIndex],_countof(prefix[nIndex]));
	nlen+=_countof(prefix[nIndex]);

	//命令
	memcpy(&cmd[nlen],(void*)sMsg.c_str(),sMsg.length());
	nlen+=sMsg.length();

	//后缀	
	memcpy(&cmd[nlen],suffix,_countof(suffix));
	nlen+=_countof(suffix);

	//	nlen+=strMsg.length();
	//}
	////后缀
	//BYTE suffix[]={0x20,0x03};
	//memcpy(&cmd[nlen],suffix,2);
	//nlen+=2;

	//	
	//string strcmd=strMsg.GetBuffer();
	//strMsg.ReleaseBuffer();
	//vector<string> cmdList = Split(strcmd);
	//	
	//BYTE cmd[1024];
	//
	////前缀1
	//BYTE prefix1[]={0x01,0x00,0x00};
	//int nlen=_countof(prefix1);
	//memcpy(cmd,prefix1,nlen);
	////数据包长度
	//int ncmdLen=strMsg.GetLength();
	//if (cmdList.size()>0)
	//{
	//	ncmdLen=0;
	//	for(auto it=cmdList.begin();it!=cmdList.end();it++)
	//		ncmdLen+=it->length();
	//	ncmdLen+=cmdList.size()-1;
	//}
	//char cmdLen[5];
	//sprintf(cmdLen,"%c",ncmdLen+21);
	//memcpy(&cmd[nlen],cmdLen,1);
	//nlen+=1;
	////前缀2
	//BYTE prefix2[]={0x00,0x00,0x00,0x01};
	//memcpy(&cmd[nlen],prefix2,_countof(prefix2));
	//nlen+=_countof(prefix2);
	////SessionID	
	//memcpy(&cmd[nlen],&this->m_EtermPacket.SID,1);
	//nlen+=1;
	////前缀3
	////BYTE prefix3[]={0x51,0x70,0x02,0x1B,0x0B,0x37,0x20,0x00,0x0F,0x1E};
	//BYTE prefix[3][10]={
	//	//{0x51,0x70,0x02,0x1B,0x0B,0x2D,0x20,0x00,0x0F,0x1E},		
	//	{0x51,0x70,0x02,0x1B,0x0B,0x21,0x20,0x00,0x0F,0x1E},				//正常
	//	{0x51,0x70,0x02,0x1B,0x0B,0x37,0x20,0x00,0x0F,0x1E},				//XS PN
	//	{0x51,0x70,0x02,0x1B,0x0B,0x2C,0x20,0x00,0x0F,0x1E}					//PN		
	//};

	//int nIndex=GetIndex(strMsg);
	//
	//memcpy(&cmd[nlen],prefix[nIndex],_countof(prefix[nIndex]));
	//nlen+=_countof(prefix[nIndex]);
	////命令
	//if (cmdList.size()>0)
	//{
	//	int i=1;
	//	BYTE rn[]={0x0D};
	//	for(auto it=cmdList.begin();it!=cmdList.end();it++,i++)
	//	{
	//		memcpy(&cmd[nlen],it->c_str(),it->length());
	//		nlen+=it->length();
	//		if (i<cmdList.size())
	//		{
	//			memcpy(&cmd[nlen],rn,1);
	//			nlen++;
	//		}
	//	}
	//}
	//else
	//{
	//	memcpy(&cmd[nlen],(char*)(LPCTSTR)strMsg,strMsg.length());
	//	nlen+=strMsg.length();
	//}
	////后缀
	//BYTE suffix[]={0x20,0x03};
	//memcpy(&cmd[nlen],suffix,2);
	//nlen+=2;

#ifdef DEBUG
	CString s;
	for(int i=0;i<nlen;i++)
	{
		s.AppendFormat("%02X ",cmd[i]);
	}
	Global::WriteLog(s);
#endif

	int ret=Send(cmd,nlen);		
	if (ret==-1)
		return false;
	else
		return true;
}

//发送数据(打印)
bool CEtermSocket::SendPrintMsg(string sMsg)
{
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	regex_constants::match_flag_type flag=regex_constants::match_default;
	regex reg("(PRINT:)|(EN)|(CN)|(VV)|(-)",fl);

	bool bEn=tr1::regex_search(sMsg,regex("EN",fl),flag);
	sMsg=tr1::regex_replace(sMsg,reg,string(""));

	//前缀1
	BYTE prefix1[]={0x01,0x00,0x00};
	//前缀2
	BYTE prefix2[]={0x0C,0x00,0x00,0x01,0x8C,0x0C,0x00,0x02,0x49,0x54,0x49,0x4E,0x3A,0x54,0x4E,0x2F};
	//前缀3
	BYTE prefix3[]={0x0C,0x00,0x02,0x49,0x54,0x49,0x4E,0x3A,0x54,0x4E,0x2F};
	//后缀           
	BYTE suffixCN[]={0x2C,0x7B,0x73,0x79,0x73,0x74,0x65,0x6D,0x3D,0x74,0x69,0x70,0x42,0x3B,0x6C,0x61,0x6E,0x67,0x75,0x61,0x67,0x65,0x3D,0x43,0x4E,0x3B,0x45,0x54,0x3D,0x59,0x7D,0x03};
	BYTE suffixEN[]={0x2C,0x7B,0x73,0x79,0x73,0x74,0x65,0x6D,0x3D,0x74,0x69,0x70,0x42,0x3B,0x6C,0x61,0x6E,0x67,0x75,0x61,0x67,0x65,0x3D,0x45,0x4E,0x3B,0x45,0x54,0x3D,0x59,0x7D,0x03};	

	BYTE cmd[1024];

	//前缀1	
	int nlen=_countof(prefix1);
	memcpy(cmd,prefix1,nlen);

	//数据包长度	
	char cmdLen[5];
	sprintf(cmdLen,"%c",sMsg.length()+52);
	memcpy(&cmd[nlen],cmdLen,1);
	nlen+=1;

	//前缀2
	memcpy(&cmd[nlen],prefix2,_countof(prefix2));
	nlen+=_countof(prefix2);

	//命令	
	memcpy(&cmd[nlen],(char*)sMsg.c_str(),sMsg.length());
	nlen+=sMsg.length();

	//后缀
	if (bEn)
	{
		memcpy(&cmd[nlen],suffixEN,_countof(suffixEN));
		nlen+=_countof(suffixEN);
	}
	else
	{
		memcpy(&cmd[nlen],suffixCN,_countof(suffixCN));
		nlen+=_countof(suffixCN);
	}	


	//strMsg.Replace("PRINT:","");
	//BOOL bEN = strMsg.Replace("EN","");	
	//strMsg.Replace("VV","");
	//strMsg.Replace("CN","");

	////...:........ITIN:CN/JRFW6S,{system=tipB;language=CN;ET=N}.
	////..A........ITIN:TN/7843546767345,{system=tipB;language=CN;ET=N}.
	////01 00 00 
	////3A 
	////0C 00 00 01 8C 
	////0C 00 02 49 54 49 4E 3A 43 4E 2F 4A 52 46 57 36 53 2C 7B 73 79 73 74 65 6D 3D 74 69 70 42 3B 6C 61 6E 67 75 61 67 65 3D 43 4E 3B 45 54 3D 4E 7D 03 
	//
	////01 00 00 
	////41 
	////0C 00 00 01 8C 0C 00 02 49 54 49 4E 3A 54 4E 2F 
	////37 38 34 33 35 34 36 37 36 37 33 34 35 
	////2C 7B 73 79 73 74 65 6D 3D 74 69 70 42 3B 6C 61 6E 67 75 61 67 65 3D 43 4E 3B 45 54 3D 4E 7D 03 
	//BYTE cmd[255];
	//
	////前缀1
	//BYTE prefix1[]={0x01,0x00,0x00};
	//int nlen=_countof(prefix1);
	//memcpy(cmd,prefix1,nlen);
	////数据包长度	
	//char cmdLen[5];
	//sprintf(cmdLen,"%c",strMsg.length()+52);
	//memcpy(&cmd[nlen],cmdLen,1);
	//nlen+=1;
	////前缀2
	//BYTE prefix2[]={0x0C,0x00,0x00,0x01,0x8C,0x0C,0x00,0x02,0x49,0x54,0x49,0x4E,0x3A,0x54,0x4E,0x2F};
	////BYTE prefix2[]={0x0C,0x00,0x00,0x01,0x8C};
	//memcpy(&cmd[nlen],prefix2,_countof(prefix2));
	//nlen+=_countof(prefix2);
	////SessionID	
	////memcpy(&cmd[nlen],m_SessionID,1);
	////nlen+=1;
	////前缀3
	////BYTE prefix3[]={0x0C,0x00,0x02,0x49,0x54,0x49,0x4E,0x3A,0x54,0x4E,0x2F};
	////memcpy(&cmd[nlen],prefix3,_countof(prefix3));
	////nlen+=_countof(prefix3);
	////命令	
	//memcpy(&cmd[nlen],(char*)(LPCTSTR)strMsg,strMsg.length());
	//nlen+=strMsg.length();
	////后缀           
	//BYTE suffixCN[]={0x2C,0x7B,0x73,0x79,0x73,0x74,0x65,0x6D,0x3D,0x74,0x69,0x70,0x42,0x3B,0x6C,0x61,0x6E,0x67,0x75,0x61,0x67,0x65,0x3D,0x43,0x4E,0x3B,0x45,0x54,0x3D,0x59,0x7D,0x03};
	//BYTE suffixEN[]={0x2C,0x7B,0x73,0x79,0x73,0x74,0x65,0x6D,0x3D,0x74,0x69,0x70,0x42,0x3B,0x6C,0x61,0x6E,0x67,0x75,0x61,0x67,0x65,0x3D,0x45,0x4E,0x3B,0x45,0x54,0x3D,0x59,0x7D,0x03};	
	//if (bEN)
	//{
	//	memcpy(&cmd[nlen],suffixEN,_countof(suffixEN));
	//	nlen+=_countof(suffixEN);
	//}
	//else
	//{
	//	memcpy(&cmd[nlen],suffixCN,_countof(suffixCN));
	//	nlen+=_countof(suffixCN);
	//}	

	int ret=Send((char*)cmd,nlen);		
	if (ret==-1)
		return false;
	else
		return true;
}

//发送数据(PNR)
bool CEtermSocket::SendPNRMsg(string sMsg)
{

	//strMsg.Replace("PNR:","");
		
	//...:........ITIN:CN/JRFW6S,{system=tipB;language=CN;ET=N}.
	//..A........ITIN:TN/7843546767345,{system=tipB;language=CN;ET=N}.
	//...;........ITIN:CN/HZPQ9E,{system=tipC3;language=EN;ET=N}.

   //01 00 00 
	//3A 
	//0C 00 00 01 8C 
	//0C 00 02 49 54 49 4E 3A 43 4E 2F 4A 52 46 57 36 53 2C 7B 73 79 73 74 65 6D 3D 74 69 70 42 3B 6C 61 6E 67 75 61 67 65 3D 43 4E 3B 45 54 3D 4E 7D 03 
	
	//01 00 00 
	//41 
	//0C 00 00 01 8C 0C 00 02 49 54 49 4E 3A 54 4E 2F 
	//37 38 34 33 35 34 36 37 36 37 33 34 35 
	//2C 7B 73 79 73 74 65 6D 3D 74 69 70 42 3B 6C 61 6E 67 75 61 67 65 3D 43 4E 3B 45 54 3D 4E 7D 03 
	
	//前缀1
	BYTE prefix1[]={0x01,0x00,0x00};
	//前缀2
	BYTE prefix2[]={0x0C,0x00,0x00,0x01,0x8C,0x0C,0x00,0x02,0x49,0x54,0x49,0x4E,0x3A,0x54,0x4E,0x2F};
	//后缀
	BYTE suffix[]={0x2C,0x7B,0x73,0x79,0x73,0x74,0x65,0x6D,0x3D,0x74,0x69,0x70,0x43,0x33,0x3B,0x6C,0x61,0x6E,0x67,0x75,0x61,0x67,0x65,0x3D,0x45,0x4E,0x3B,0x45,0x54,0x3D,0x4E,0x7D,0x03};

	BYTE cmd[255];
	
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	sMsg=tr1::regex_replace(sMsg,regex("PNR:",fl),string(""));

	//前缀1	
	int nlen=_countof(prefix1);
	memcpy(cmd,prefix1,nlen);

	//数据包长度	
	char cmdLen[5];
	sprintf(cmdLen,"%c",sMsg.length()+53);
	memcpy(&cmd[nlen],cmdLen,1);
	nlen+=1;
	
	//前缀2
	memcpy(&cmd[nlen],prefix2,_countof(prefix2));
	nlen+=_countof(prefix2);

	//命令	
	memcpy(&cmd[nlen],(char*)sMsg.c_str(),sMsg.length());
	nlen+=sMsg.length();

	//后缀
	memcpy(&cmd[nlen],suffix,_countof(suffix));
	nlen+=_countof(suffix);

	//SessionID	
	//memcpy(&cmd[nlen],m_SessionID,1);
	//nlen+=1;
	//前缀3
	//BYTE prefix3[]={0x0C,0x00,0x02,0x49,0x54,0x49,0x4E,0x3A,0x54,0x4E,0x2F};
	//memcpy(&cmd[nlen],prefix3,_countof(prefix3));
	//nlen+=_countof(prefix3);
	//命令	
	//memcpy(&cmd[nlen],(char*)(LPCTSTR)strMsg,strMsg.length());
	//nlen+=strMsg.length();
	////后缀           01 00 00 3B 0C 00 00 01 8C 0C 00 02 49 54 49 4E 3A 43 4E 2F 48 5A 50 51 39 45 
	//
	//BYTE suffix[]={0x2C,0x7B,0x73,0x79,0x73,0x74,0x65,0x6D,0x3D,0x74,0x69,0x70,0x43,0x33,0x3B,0x6C,0x61,0x6E,0x67,0x75,0x61,0x67,0x65,0x3D,0x45,0x4E,0x3B,0x45,0x54,0x3D,0x4E,0x7D,0x03};
	//memcpy(&cmd[nlen],suffix,_countof(suffix));
	//nlen+=_countof(suffix);
	//	
	int ret=Send((char*)cmd,nlen);		
	if (ret==-1)
		return false;
	else
		return true;
}

BOOL CEtermSocket::SetKeepAlive()
{		
	// 开启KeepAlive
	BOOL bKeepAlive = TRUE;
	int nRet = ::setsockopt(this->GetSocketHandle(), SOL_SOCKET, SO_KEEPALIVE, (char*)&bKeepAlive, sizeof(bKeepAlive));
	if (nRet == SOCKET_ERROR)
	{
		PRINTDEBUG(WSAGetLastError()); 
		return FALSE;
	}
	
	// 设置KeepAlive参数
	tcp_keepalive alive_in = {0};
	tcp_keepalive alive_out = {0};
	alive_in.keepalivetime = 10000;                // 开始首次KeepAlive探测前的TCP空闭时间
	alive_in.keepaliveinterval= 10000;              // 两次KeepAlive探测间的时间间隔
	alive_in.onoff= TRUE;

	unsigned long ulBytesReturn = 0;
	nRet = WSAIoctl(this->GetSocketHandle(), SIO_KEEPALIVE_VALS, &alive_in, sizeof(alive_in),	&alive_out, sizeof(alive_out), &ulBytesReturn, NULL, NULL);
	if (nRet == SOCKET_ERROR)
	{
		PRINTDEBUG(WSAGetLastError());
		return FALSE;
	}

	return TRUE;
}

unsigned int _stdcall ReStartEterm(void* lparam)
{
	CEtermSocket* eTerm=(CEtermSocket*)lparam; 
	if (eTerm->m_nState!=0)
	{
		Sleep(10*1000);
		Global::GetEtermView(eTerm)->PostMessage(WM_CONNECT,(WPARAM)(char*)eTerm->m_config.UserName,0);
	}
	return 0;
}