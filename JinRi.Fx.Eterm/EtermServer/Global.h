class CWebServer;
#pragma once
#include "log.h"
#include "lip.h"
#include "Proxyer.h"
#include "IOCP/WebServer.h"
#include "Script.h"
#include <atlstr.h>
#include <afxmt.h>
#include "redis\xRedisClient.h"
#include "enums.h"
#include "EtermConfig.h"
#include "Global.h"
#include "Proxy.h"
#include <memory>
#include "EtermSocket.h"
#include "SMS.h"

#import "EtermProxy.tlb" named_guids raw_interfaces_only
using namespace EtermProxy; 

// Global

class Global : public CWinApp
{
public:	
	//取得应用程序当前路径
	static CString szAppPath;
	static CString EtermXSLPath;
	static CString ServerUrl;
	static TCHAR szConnectString[1024];
	static char szMac[20];
	static char szHostIP[20];
				
	static CWebServer Server;
	static vector<CLog> loglist;
	static vector<lip> iplist;
	static RedisNode* RedisList;
	static map<wstring, wstring> scripts;
	static map<CString, CView*> EtermViews;
	static map<CString,unique_ptr<CEtermSocket>> configlist;
	static vector<CString> serverlist;
	static vector<HANDLE> handls;
	static CProxyer proxyer;
	static EtermProxy::IProxyPtr pProxy;
	static EtermConfig etermconfig;
	static CCriticalSection m_log;
	static CCriticalSection m_cs;
	static CCriticalSection cs_ip;
	static ::Proxy proxy;
	static vector<enums> cmdtypes;
	static SMS sms;
	static CView* GetView(CString strTitle);
	static CString GetAppPath();
	static void WriteLog(CLog log);
	static bool UpdateEtermState(bool bOn);
	static bool SetEtermConfig(CString config, bool bAppend);
	static void Reboot(CString strConfig);
	static void SendSMS(const TCHAR* szConfig);
	//容错处理
	static LONG WINAPI MyUnhandledExceptionFilter(struct _EXCEPTION_POINTERS* ExceptionInfo);
};


