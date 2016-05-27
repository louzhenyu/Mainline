
// EtermServer.cpp : 定义应用程序的类行为。
//

#include "stdafx.h"
#include "afxwinappex.h"
#include "afxdialogex.h"
#include "EtermServer.h"
#include "MainFrm.h"
#include "ChildFrm.h"
#include "EtermServerDoc.h"
#include "EtermServerView.h"
#include "Global.h"
#include "threadpools.h"
#include "Registry.h"
#include "SocketHelper.h"
#include "md5.h"
#include "Encrypt\Encrypt.h"
#include "RegDlg.h"
#include "limitIP.h"
#include "ScriptData.h"
#include "xmlHelper.h"
#include "DlgConfigManage.h"
#include "EmailHelper.h"
#include "DlgReckon.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

extern UINT StartWebServer(LPVOID lParam);
extern UINT LogThread(LPVOID lparam);
extern UINT CheckEtermState(LPVOID lParam);
extern volatile bool bSystemOver;
// CEtermServerApp

BEGIN_MESSAGE_MAP(CEtermServerApp, CWinAppEx)
	ON_COMMAND(ID_APP_ABOUT, &CEtermServerApp::OnAppAbout)
	// 基于文件的标准文档命令
	ON_COMMAND(ID_FILE_NEW, &CWinAppEx::OnFileNew)
	ON_COMMAND(ID_FILE_OPEN, &CWinAppEx::OnFileOpen)
END_MESSAGE_MAP()


// CEtermServerApp 构造

CEtermServerApp::CEtermServerApp()
{
	m_bHiColorIcons = TRUE;

	// 支持重新启动管理器
	m_dwRestartManagerSupportFlags = AFX_RESTART_MANAGER_SUPPORT_ALL_ASPECTS;
#ifdef _MANAGED
	// 如果应用程序是利用公共语言运行时支持(/clr)构建的，则: 
	//     1) 必须有此附加设置，“重新启动管理器”支持才能正常工作。
	//     2) 在您的项目中，您必须按照生成顺序向 System.Windows.Forms 添加引用。
	System::Windows::Forms::Application::SetUnhandledExceptionMode(System::Windows::Forms::UnhandledExceptionMode::ThrowException);
#endif

	// TODO:  将以下应用程序 ID 字符串替换为唯一的 ID 字符串；建议的字符串格式
	//为 CompanyName.ProductName.SubProduct.VersionInformation
	SetAppID(_T("EtermServer.AppID.NoVersion"));

	// TODO:  在此处添加构造代码，
	// 将所有重要的初始化放置在 InitInstance 中
}

// 唯一的一个 CEtermServerApp 对象

CEtermServerApp theApp;


// CEtermServerApp 初始化

BOOL CEtermServerApp::InitInstance()
{	
	// 如果一个运行在 Windows XP 上的应用程序清单指定要
	// 使用 ComCtl32.dll 版本 6 或更高版本来启用可视化方式，
	//则需要 InitCommonControlsEx()。  否则，将无法创建窗口。
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	// 将它设置为包括所有要在应用程序中使用的
	// 公共控件类。
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinAppEx::InitInstance();

	CheckMulSoft();

	// 初始化 OLE 库
	if (!AfxOleInit())
	{
		AfxMessageBox(IDP_OLE_INIT_FAILED);
		return FALSE;
	}

	EnableTaskbarInteraction();

	// 使用 RichEdit 控件需要  AfxInitRichEdit2()	
	AfxInitRichEdit();

	// 标准初始化
	// 如果未使用这些功能并希望减小
	// 最终可执行文件的大小，则应移除下列
	// 不需要的特定初始化例程
	// 更改用于存储设置的注册表项
	// TODO:  应适当修改该字符串，
	// 例如修改为公司或组织名
	SetRegistryKey(_T("应用程序向导生成的本地应用程序"));
	LoadStdProfileSettings(4);  // 加载标准 INI 文件选项(包括 MRU)


	InitContextMenuManager();

	InitKeyboardManager();

	InitTooltipManager();
	CMFCToolTipInfo ttParams;
	ttParams.m_bVislManagerTheme = TRUE;
	theApp.GetTooltipManager()->SetTooltipParams(AFX_TOOLTIP_TYPE_ALL,
		RUNTIME_CLASS(CMFCToolTipCtrl), &ttParams);

	// 注册应用程序的文档模板。  文档模板
	// 将用作文档、框架窗口和视图之间的连接
	CMultiDocTemplate* pDocTemplate;
	pDocTemplate = new CMultiDocTemplate(IDR_EtermServerTYPE,
		RUNTIME_CLASS(CEtermServerDoc),
		RUNTIME_CLASS(CChildFrame), // 自定义 MDI 子框架
		RUNTIME_CLASS(CEtermServerView));
	if (!pDocTemplate)
		return FALSE;
	pDocTemplate->SetContainerInfo(IDR_EtermServerTYPE_CNTR_IP);
	AddDocTemplate(pDocTemplate);

	// 创建主 MDI 框架窗口
	CMainFrame* pMainFrame = new CMainFrame;
	if (!pMainFrame || !pMainFrame->LoadFrame(IDR_MAINFRAME))
	{
		delete pMainFrame;
		return FALSE;
	}
	m_pMainWnd = pMainFrame;


	// 分析标准 shell 命令、DDE、打开文件操作的命令行
	CCommandLineInfo cmdInfo;
	ParseCommandLine(cmdInfo);

	cmdInfo.m_nShellCommand = CCommandLineInfo::FileNothing;

	// 调度在命令行中指定的命令。  如果
	// 用 /RegServer、/Register、/Unregserver 或 /Unregister 启动应用程序，则返回 FALSE。
	if (!ProcessShellCommand(cmdInfo))
		return FALSE;
	// 主窗口已初始化，因此显示它并对其进行更新
	pMainFrame->ShowWindow(m_nCmdShow);
	pMainFrame->UpdateWindow();

	ValidityCheck();
	InitCommon();

	HANDLE handleSS = ::CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)StartWebServer, NULL, 0, 0);
	HANDLE handleLT = ::CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)LogThread, NULL, 0, 0);
	HANDLE handleCE = ::CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)CheckEtermState, NULL, 0, 0);

	Global::handls.push_back(handleSS);
	Global::handls.push_back(handleLT);
	Global::handls.push_back(handleCE);

	return TRUE;
}

int CEtermServerApp::ExitInstance()
{
	//TODO:  处理可能已添加的附加资源
	AfxOleTerm(FALSE);

	bSystemOver = true;

	Global::UpdateEtermState(false);

	int nCount = Global::handls.size();

	
	for (int i = 0; i < nCount; i++)
	{
		TerminateThread(Global::handls[i], 0);
	}


	//while (true)
	{
		DWORD dwRet = ::WaitForMultipleObjects(nCount, &Global::handls[0], TRUE, 3000);
		/*if (dwRet == WAIT_TIMEOUT || dwRet == WAIT_FAILED)
			break;
		if (dwRet >= WAIT_OBJECT_0 + nCount - 1 && dwRet <= WAIT_OBJECT_0)
			break;
		if (WAIT_OBJECT_0 + nCount - 1 <= dwRet <= WAIT_OBJECT_0)
			break;*/
	}
	if (Global::pProxy)
		Global::pProxy.Release();
	if (EmailHelper::pComm)
		EmailHelper::pComm.Release();

	for (DWORD i = 0; i<nCount; i++)
	{
		CloseHandle(Global::handls[i]);
	}

	return CWinAppEx::ExitInstance();
}

// CEtermServerApp 消息处理程序


// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

	// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()

// 用于运行对话框的应用程序命令
void CEtermServerApp::OnAppAbout()
{
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();
}

// CEtermServerApp 自定义加载/保存方法

void CEtermServerApp::PreLoadState()
{
	BOOL bNameValid;
	CString strName;
	bNameValid = strName.LoadString(IDS_EDIT_MENU);
	ASSERT(bNameValid);
	GetContextMenuManager()->AddMenu(strName, IDR_POPUP_EDIT);
}

void CEtermServerApp::LoadCustomState()
{
}

void CEtermServerApp::SaveCustomState()
{
}

// CEtermServerApp 消息处理程序

void CEtermServerApp::ValidityCheck()
{
	__try
	{
		USES_CONVERSION;

		bool bReg = false;

		CString strValue;

		CRegistry reg(HKEY_LOCAL_MACHINE);
		BOOL bRet = reg.Open(_T("SOFTWARE\0"));
		if (bRet)
		{
			BOOL bRet = reg.Open(_T("EtermServer\0"));
			if (bRet)
			{
				bRet = reg.Open(_T("Register\0"));
				if (bRet)
				{
					bRet = reg.Read(_T(""), &strValue);
					if (!strValue.IsEmpty())
					{
						CSocketHelper sockHelper;
						char* szRegNo = sockHelper.GetMAC();

						md5 md5(szRegNo, strlen(szRegNo));

						Encrypt encrypt;
						unsigned char key[] = "&feghtyj";
						char* szDec = encrypt.decrypt(key, (char*)T2A(strValue));
						bReg = strcmp(szDec, md5.toString().c_str()) == 0;
					}
				}
			}
		}
		if (!bReg)
		{
			FILE* file = NULL;
			TCHAR szFile[1024] = { 0 };
			_stprintf_s(szFile, _T("%s\\register.dat"), Global::szAppPath);
			errno_t no = _tfopen_s(&file, szFile, _T("r"));
			if (no == 0)
			{
				char szBuf[1024] = { 0 };
				if (fgets(szBuf, 1024, file))
				{
					Encrypt encrypt;
					unsigned char key[] = "&feghtyj";
					char* szDec = encrypt.decrypt(key, szBuf);
					CSocketHelper sockHelper2;
					char* szRegNo2 = sockHelper2.GetMAC();
					md5 md5(szRegNo2, strlen(szRegNo2));
					bReg = strcmp(szDec, md5.toString().c_str()) == 0;
				}
				fclose(file);
			}

			if (!bReg)
			{
				CRegDlg dlg;
				dlg.DoModal();
			}
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		exit(0);
	}
}

void CEtermServerApp::InitCommon()
{
	limitIP lipd;
	Global::iplist = lipd.GetIPList();

	char buf[1024] = { 0 };
	char key[25] = { 0 };
	char strPath[1024] = { 0 };
	sprintf_s(strPath, "%s\\Config.ini", CT2A(Global::szAppPath));
	::GetPrivateProfileStringA("SQL", "ConnectString", "", buf, sizeof(buf), strPath);
	if (strlen(buf) > 0)
	{
		Encrypt encrypt;
		unsigned char key[] = "pyioi$9k";
		_tcscpy_s(Global::szConnectString, CA2T(encrypt.decrypt(key, buf)));

		Global::pProxy = EtermProxy::IProxyPtr(__uuidof(EtermProxy::Proxy));
		EmailHelper::pComm = JetermUntility::ICommunicationPtr(__uuidof(JetermUntility::Communication));

		ScriptData sd;
		sd.LoadScripts();
	}
	//初始化Redis	
	::GetPrivateProfileStringA("Redis", "IP", "", buf, sizeof(buf), strPath);
	if (strlen(buf) > 0)
	{
		int nPort = ::GetPrivateProfileIntA("Redis", "Port", 6379, strPath);
		char* host = new char[strlen(buf)];
		strcpy(host, buf);
		Global::RedisList = new RedisNode{ 0, host, nPort, "", 2, 5 };
	}
	//初始化JetermEntity
	sprintf_s(buf, "%s\\JetermEntity.XML", CT2A(Global::szAppPath));
	xmlHelper xml;
	if (xml.LoadFile(buf))
	{
		xml.getnodes("/doc/members/member[contains(@name,'F:JetermEntity.EtermCommand.CmdType')]/summary");
	}
	TCHAR szPath[1024] = { 0 };
	TCHAR szbuf[1024] = { 0 };
	_stprintf_s(szPath, _T("%s\\Config.ini"), Global::szAppPath);
	//初始化Email信息
	if (EmailHelper::pComm)
	{
		TCHAR sztemp[1024] = { 0 };
		::GetPrivateProfileString(_T("Email"), _T("FormEmail"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
		EmailHelper::FromEamil = CString(szbuf);
		::GetPrivateProfileString(_T("Email"), _T("FormPwd"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
		EmailHelper::FromPwd = CString(szbuf);
		::GetPrivateProfileString(_T("Email"), _T("FormServer"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
		EmailHelper::FormServer = CString(szbuf);
		for (int i = 0;; i++)
		{
			_stprintf_s(sztemp, _T("To%d"), i);
			::GetPrivateProfileString(_T("Email"), sztemp, _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
			if (_tcslen(szbuf) == 0) break;
			EmailHelper::Address.push_back(CString(szbuf));
		}
	}
	//初始化代理信息
	Global::proxy.type = ::GetPrivateProfileInt(_T("Proxy"), _T("type"), 0, szPath);
	if (strlen(buf) > 0)
	{
		::GetPrivateProfileString(_T("Proxy"), _T("host"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
		Global::proxy.host = wstring(szbuf);
		Global::proxy.port = ::GetPrivateProfileInt(_T("Proxy"), _T("port"), 0, szPath);
		::GetPrivateProfileString(_T("Proxy"), _T("user"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
		Global::proxy.user = wstring(szbuf);
		::GetPrivateProfileString(_T("Proxy"), _T("pwd"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), szPath);
		Global::proxy.pwd = wstring(szbuf);
	}
	//打印参数
	Global::EtermXSLPath.Format(_T("%s\\3in1\\ItinBSP"), Global::szAppPath);

	//短信发送
	::GetPrivateProfileStringA("SMS", "server", "114.80.85.137", buf, sizeof(buf), strPath);
	strcpy(Global::sms.szSMSserver, buf);
	::GetPrivateProfileStringA("SMS", "obj", "/SendMessage.ashx?mobile=%s&comment=%s", buf, sizeof(buf), strPath);
	strcpy(Global::sms.szObj, buf);
	Global::sms.nPort = ::GetPrivateProfileIntA("SMS", "port", 18188, strPath);
	for (int i = 0;; i++)
	{
		char szkey[50] = { 0 };
		sprintf(szkey, "mobil%d", i);
		::GetPrivateProfileStringA("SMS", szkey, "", buf, sizeof(buf), strPath);
		if (strlen(buf) == 0) break;
		Global::sms.vecMobil.push_back(buf);
	}

}

void CEtermServerApp::CheckMulSoft()
{
	CString strPath;
	strPath.Format(_T("%s\\Config.ini"), Global::szAppPath);

	TCHAR buf[255] = { 0 };
	int nPort = ::GetPrivateProfileInt(_T("SYSTEM"), _T("PORT"), 5252, strPath);
	::GetPrivateProfileString(_T("SYSTEM"), _T("IP"), _T(""), buf, sizeof(buf), strPath);

	Global::ServerUrl.Format(_T("%s:%d"), buf, nPort);

	if (!Global::ServerUrl.IsEmpty())
	{
		HANDLE m_hOneInstance = ::CreateMutex(NULL, FALSE, Global::ServerUrl);
		if (GetLastError() == ERROR_ALREADY_EXISTS)
		{
			CDlgReckon dlg;
			dlg.DoModal();
			exit(0);
			return;
		}
	}
}
