// Global.cpp : 实现文件
//

#include "stdafx.h"
#include "EtermServer.h"
#include "Global.h"
#include "EtermConfig.h"
#include "RedisHelper.h"
#include "EtermConfigData.h"
#include "stringEx.h"
#include "Http.h"
#include "strcoding.h"

#include <Dbghelp.h>
#pragma auto_inline (off)
#pragma comment( lib, "DbgHelp" )

// Global

CString Global::szAppPath = GetAppPath();
CString Global::EtermXSLPath;
CString Global::ServerUrl;
char Global::szMac[20] = { 0 };
char Global::szHostIP[20] = { 0 };

TCHAR Global::szConnectString[1024] = { 0 };
vector<CLog> Global::loglist;
vector<lip> Global::iplist;
map<CString, unique_ptr<CEtermSocket>> Global::configlist;
map<wstring, wstring> Global::scripts;
map<CString, CView*> Global::EtermViews;
CWebServer Global::Server;
CProxyer Global::proxyer;
RedisNode* Global::RedisList=NULL;
CCriticalSection Global::m_log;
CCriticalSection Global::m_cs;
CCriticalSection Global::cs_ip;
::Proxy Global::proxy;
EtermProxy::IProxyPtr Global::pProxy=NULL;
EtermConfig Global::etermconfig;
vector<enums> Global::cmdtypes;
vector<CString> Global::serverlist;
vector<HANDLE> Global::handls;
SMS Global::sms;

CString Global::GetAppPath()
{
	TCHAR pathbuf[MAX_PATH] = { 0 };
	int pathlen = ::GetModuleFileName(NULL, pathbuf, MAX_PATH);

	//替换掉单杠 
	while (TRUE)
	{
		if (pathbuf[pathlen--] == '\\')
			break;
	}
	pathbuf[++pathlen] = 0x0;
	return CString(pathbuf);
}
void Global::WriteLog(CLog log)
{
	Global::m_log.Lock();
	Global::loglist.push_back(log);
	Global::m_log.Unlock();
}

CView* Global::GetView(CString strTitle)
{
	CView* pView = NULL;
	auto it = Global::EtermViews.find(strTitle);
	if (it != Global::EtermViews.end())
	{
		pView = it->second;
		return pView;
	}
	CMDIFrameWnd* MDIframe = (CMDIFrameWnd*)AfxGetMainWnd();

	CEtermServerApp* pApp = (CEtermServerApp*)AfxGetApp();

	POSITION tpos = pApp->GetFirstDocTemplatePosition();

	while (tpos)
	{
		CDocTemplate* pMulDocTemp = pApp->GetNextDocTemplate(tpos);

		if (pMulDocTemp)
		{
			POSITION dpos = pMulDocTemp->GetFirstDocPosition();
			while (dpos)
			{
				CDocument* pDoc = pMulDocTemp->GetNextDoc(dpos);
				if (pDoc)
				{
					POSITION vpos = pDoc->GetFirstViewPosition();
					while (vpos)
					{
						pView = pDoc->GetNextView(vpos);
						if (pView->GetDocument()->GetTitle() == strTitle)
						{
							return pView;
						}
					}
				}
			}
		}
	}
	return NULL;
}

LONG WINAPI Global::MyUnhandledExceptionFilter(struct _EXCEPTION_POINTERS* ExceptionInfo)
{

	HANDLE lhDumpFile = CreateFile(_T("DumpFile.dmp"), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);



	MINIDUMP_EXCEPTION_INFORMATION loExceptionInfo;

	loExceptionInfo.ExceptionPointers = ExceptionInfo;

	loExceptionInfo.ThreadId = GetCurrentThreadId();

	loExceptionInfo.ClientPointers = TRUE;

	MiniDumpWriteDump(GetCurrentProcess(),
		GetCurrentProcessId(),
		lhDumpFile,
		MiniDumpNormal,
		&loExceptionInfo,
		NULL,
		NULL
		);

	CloseHandle(lhDumpFile);
	
	return EXCEPTION_EXECUTE_HANDLER;

}

bool Global::UpdateEtermState(bool bOn)
{
	if (_tcslen(Global::szConnectString) == 0) return false;

	EtermConfigData ecd;

	EtermConfig config;
	if (Global::ServerUrl.IsEmpty())
	{
		CString strPath;
		strPath.Format(_T("%s\\Config.ini"), Global::szAppPath);

		TCHAR buf[255] = { 0 };
		int nPort = ::GetPrivateProfileInt(_T("SYSTEM"), _T("PORT"), 5252, strPath);
		::GetPrivateProfileString(_T("SYSTEM"), _T("IP"), _T(""), buf, sizeof(buf), strPath);

		Global::ServerUrl.Format(_T("%s:%d"), buf, nPort);
	}

	EtermServer::RedisHelper rh;
	string sret = rh.get("140106_140110_EtermUrl");
	map<CString, EtermConfig> ecs = ecd.GetEtermConfig(sret);
	auto it = ecs.find(Global::ServerUrl);
	if (it != ecs.end())
	{
		it->second.State = bOn ? 0 : 1;
		config = it->second;
		if (!bOn) ecs.erase(it->first);
	}
	else
	{
		EtermConfig* pConfig = ecd.GetConfig(Global::ServerUrl);
			
		if (pConfig == NULL)
		{
			config.AllowAirLine = "*";
			config.ConfigLevel = 5;
			config.ServerUrl = Global::ServerUrl;		
		}
		else
		{			
			config = *pConfig;
			delete pConfig;
		}
		config.State = bOn ? 0 : 1;
		if (config.State == 0) ecs.insert(make_pair(config.ServerUrl, config));
	}

	it = ecs.find(Global::ServerUrl);
	if (it != ecs.end())
	{
		it->second.ConfigList.Empty();
		for (auto cit = Global::configlist.begin(); cit != Global::configlist.end(); cit++)
			it->second.ConfigList.AppendFormat(_T("%s,"), cit->first);
		config.ConfigList = it->second.ConfigList;
	}
	if (!ecd.UpdateConfig(config))
		ecd.AppendConfig(config);
	sret = ecd.toString(ecs);
	rh.set("140106_140110_EtermUrl", sret);			
	Global::etermconfig = config;
	return true;
}
bool Global::SetEtermConfig(CString config, bool bAppend)
{
	EtermConfigData ecd;

	EtermServer::RedisHelper rh;
	string sret = rh.get("140106_140110_EtermUrl");
	map<CString, EtermConfig> ecs = ecd.GetEtermConfig(sret);
	auto it = ecs.find(Global::ServerUrl);
	if (it != ecs.end())
	{
		wstring configlist = it->second.ConfigList;
		string sret;
		stringEx ex;

		if (bAppend)
		{			
			configlist += config;
		}
		else
		{
			configlist = ex.replace(configlist, wstring(config + _T(",")),_T(""));
		}
		it->second.ConfigList = configlist.c_str();

		sret = ecd.toString(ecs);
		return rh.set("140106_140110_EtermUrl", sret);
	}
	return false;
}

// 重启按钮响应函数
void Global::Reboot(CString strConfig)
{
	CString strVal;
	strVal.Format(_T("%s,"), strConfig);
	auto it = Global::configlist.find(strConfig);
	if (it != Global::configlist.end())
		strVal.AppendFormat(_T("%s,"), it->first);

	// TODO: 在此添加控件通知处理程序代码
	::PostMessage(AfxGetMainWnd()->m_hWnd, WM_SYSCOMMAND, SC_CLOSE, NULL);
	//获取exe程序当前路径
	extern CEtermServerApp theApp;
	TCHAR szAppName[MAX_PATH];
	::GetModuleFileName(theApp.m_hInstance, szAppName, MAX_PATH);
	CString strAppFullName;
	strAppFullName.Format(_T("%s %s"), szAppName, strVal);
	//重启程序
	STARTUPINFO StartInfo;
	PROCESS_INFORMATION procStruct;
	memset(&StartInfo, 0, sizeof(STARTUPINFO));
	StartInfo.cb = sizeof(STARTUPINFO);
	::CreateProcess(
		(LPCTSTR)strAppFullName,
		NULL,
		NULL,
		NULL,
		FALSE,
		NORMAL_PRIORITY_CLASS,
		NULL,
		NULL,
		&StartInfo,
		&procStruct);
}

void Global::SendSMS(const TCHAR* szConfig)
{
	__try
	{
		static CTime LastRemindTime;

		CTime curTime = CTime::GetCurrentTime();
		if (curTime - LastRemindTime<CTimeSpan(0, 1, 10, 0))
			return;
		else
			LastRemindTime = curTime;

		strCoding coding;
		for (int i = 0; i<Global::sms.vecMobil.size(); i++)
		{
			CHttp http;
			char szInfo[4086] = { 0 };
			char szObj[4086] = { 0 };
			char szHeader[4086] = { 0 };
			sprintf(szInfo, "服务器:%s 配置:%s 出故障了！", Global::ServerUrl, CT2A(szConfig));
			sprintf(szObj, Global::sms.szObj, Global::sms.vecMobil[i].c_str(), coding.UrlUTF8(szInfo).c_str());
			sprintf(szInfo, ":%d", Global::sms.nPort);
			sprintf(szHeader,
				"GET %s HTTP/1.1\r\n"
				"Host: %s%s\r\n\r\n",
				szObj,
				Global::sms.szSMSserver,
				Global::sms.nPort == 80 ? "" : szInfo);
			string sret;
			http.HttpRequest(Global::sms.szSMSserver, szHeader, sret, Global::sms.nPort);
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		
	}
}