
// EtermServerView.cpp : CEtermServerView 类的实现
//

#include "stdafx.h"
// SHARED_HANDLERS 可以在实现预览、缩略图和搜索筛选器句柄的
// ATL 项目中进行定义，并允许与该项目共享文档代码。
#ifndef SHARED_HANDLERS
#include "EtermServer.h"
#endif

#include "EtermServerDoc.h"
#include "CntrItem.h"
#include "resource.h"
#include "EtermServerView.h"
#include "MainFrm.h"
#include "Config.h"
#include "ShowInfo.h"
#include "SocketHelper.h"
#include "Global.h"
#include <algorithm>
#include "EtermConfigData.h"
#include "RedisHelper.h"
#include "EtermCommand.h"
#include "json\json.h"
#include "EmailHelper.h"
#include "eterm3Helper.h"
#include "stringEx.h"
#include <thread>
#include "md5.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//extern unsigned int _stdcall _EtermRequestDispose(void* lparam);

extern void _EtermRequestDispose(CString strConfig);
void SystemInvoke(CString strConfig);
string GetBookInfo(wstring cmd);
void SaveBookInfo(wstring cmd, wstring guid, CString response, CString Config);
extern volatile bool bSystemOver;
// CEtermServerView

IMPLEMENT_DYNCREATE(CEtermServerView, CRichEditView)

BEGIN_MESSAGE_MAP(CEtermServerView, CRichEditView)
	ON_WM_DESTROY()
	ON_WM_CONTEXTMENU()
	ON_WM_RBUTTONUP()
	ON_MESSAGE(WM_CONNECT, ConnectServer)
	ON_MESSAGE(WM_USER, OnVerifyCert)
	ON_MESSAGE(WM_MSG, SystemCommand)
	ON_COMMAND(ID_BACKGROUND, &CEtermServerView::OnBackground)
	ON_COMMAND(ID_FORCOLOR, &CEtermServerView::OnForcolor)
	ON_COMMAND(ID_FONT_SET, &CEtermServerView::OnFontSet)
END_MESSAGE_MAP()

// CEtermServerView 构造/析构

CEtermServerView::CEtermServerView()
{
	m_backcolor = RGB(0, 0, 0);
	m_forecolor = RGB(0, 255, 0);
	m_font.lfCharSet = GB2312_CHARSET;
	m_font.lfStrikeOut = 0;
	m_font.lfUnderline = 0;
	m_font.lfHeight = -19;
	m_fontHeight = 250;
	_tcscpy_s(m_font.lfFaceName, _T("宋体"));
	//m_pEvent = CreateEvent(NULL, TRUE, FALSE, _T("EtermData"));
}

CEtermServerView::~CEtermServerView()
{
	Global::EtermViews.erase(this->GetDocument()->GetTitle());
}

BOOL CEtermServerView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO:  在此处通过修改
	//  CREATESTRUCT cs 来修改窗口类或样式

	return CRichEditView::PreCreateWindow(cs);
}

void CEtermServerView::OnInitialUpdate()
{
	CRichEditView::OnInitialUpdate();


	// 设置打印边距(720 缇 = 1/2 英寸)
	SetMargins(CRect(720, 720, 720, 720));

	CEtermServerDoc* pDoc = this->GetDocument();
	if (pDoc != NULL)
	{
		for (auto it = Global::configlist.begin(); it != Global::configlist.end(); it++)
		{
			auto vit = Global::EtermViews.find(it->first);
			if (vit == Global::EtermViews.end())
			{
				pDoc->SetTitle(it->first);
				Global::EtermViews.insert(make_pair(it->first, this));

				//unsigned int dwThreadId;
				//HANDLE handle = (HANDLE)_beginthreadex(NULL, 0, &_EtermRequestDispose, new CString(it->first), 0, &dwThreadId);

				thread th(_EtermRequestDispose, it->first);
				th.detach();
				thread invoke(SystemInvoke, it->first);
				invoke.detach();

				CString strVal = ::AfxGetApp()->m_lpCmdLine;
				if (strVal.Find(it->first + _T(",")) != -1)
				{
					PostMessage(WM_CONNECT, 0, (LPARAM)(new CString(it->first)));
				}
				break;
			}
		}
	}

	InitControl();
}

void CEtermServerView::OnDestroy()
{
	// 析构时停用此项；这在
	// 使用拆分视图时非常重要 
	COleClientItem* pActiveItem = GetDocument()->GetInPlaceActiveItem(this);
	if (pActiveItem != NULL && pActiveItem->GetActiveView() == this)
	{
		pActiveItem->Deactivate();
		ASSERT(GetDocument()->GetInPlaceActiveItem(this) == NULL);
	}
	CRichEditView::OnDestroy();
}


void CEtermServerView::OnRButtonUp(UINT /* nFlags */, CPoint point)
{
	ClientToScreen(&point);
	OnContextMenu(this, point);
}

void CEtermServerView::OnContextMenu(CWnd* /* pWnd */, CPoint point)
{
#ifndef SHARED_HANDLERS
	theApp.GetContextMenuManager()->ShowPopupMenu(IDR_POPUP_EDIT, point.x, point.y, this, TRUE);
#endif
}


// CEtermServerView 诊断

#ifdef _DEBUG
void CEtermServerView::AssertValid() const
{
	CRichEditView::AssertValid();
}

void CEtermServerView::Dump(CDumpContext& dc) const
{
	CRichEditView::Dump(dc);
}

CEtermServerDoc* CEtermServerView::GetDocument() const // 非调试版本是内联的
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CEtermServerDoc)));
	return (CEtermServerDoc*)m_pDocument;
}
#endif //_DEBUG


// CEtermServerView 消息处理程序
void CEtermServerView::InitControl(void)
{
	CHARFORMAT cf;
	ZeroMemory(&cf, sizeof(CHARFORMAT));
	cf.cbSize = sizeof(CHARFORMAT);
	cf.dwMask = CFM_COLOR | CFM_FACE | CFM_CHARSET | CFM_SIZE | CFM_BOLD;
	cf.dwEffects &= ~CFE_BOLD;
	cf.bCharSet = GB2312_CHARSET;
	_tcscpy_s(cf.szFaceName, _T("宋体"));
	cf.crTextColor = RGB(0, 255, 0);
	cf.yHeight = 250;

	GetRichEditCtrl().SetBackgroundColor(FALSE, RGB(0, 0, 0));
	GetRichEditCtrl().SetDefaultCharFormat(cf);

}

void CEtermServerView::OnBackground()
{
	CColorDialog dlg(m_backcolor);
	if (dlg.DoModal() == IDOK)
	{
		m_backcolor = dlg.GetColor();

		//CRichEditCtrl& EditCtrl = GetRichEditCtrl();
		CHARFORMAT cf;
		ZeroMemory(&cf, sizeof(CHARFORMAT));
		cf.cbSize = sizeof(CHARFORMAT);
		cf.dwMask = CFM_COLOR;

		GetRichEditCtrl().SetBackgroundColor(FALSE, m_backcolor);  //获取用户所选颜色
		GetRichEditCtrl().GetDefaultCharFormat(cf);

		cf.crTextColor = m_forecolor;
		cf.dwEffects &= ~CFE_AUTOCOLOR;

		GetRichEditCtrl().SetDefaultCharFormat(cf);
	}
}


void CEtermServerView::OnForcolor()
{
	CColorDialog dlg(m_forecolor);
	if (dlg.DoModal() == IDOK)
	{
		m_forecolor = dlg.GetColor();

		CRichEditCtrl& EditCtrl = GetRichEditCtrl();
		CHARFORMAT cf;
		ZeroMemory(&cf, sizeof(CHARFORMAT));
		cf.cbSize = sizeof(CHARFORMAT);
		cf.crTextColor = m_forecolor;      //获取用户所选颜色
		cf.dwMask = CFM_CHARSET | CFM_COLOR;
		cf.dwEffects = cf.dwEffects  & ~CFE_AUTOCOLOR;  //去除原来的颜色效果，改为新的颜色
		GetRichEditCtrl().SetDefaultCharFormat(cf);        //设置
	}
}


void CEtermServerView::OnFontSet()
{
	CFontDialog dlg(&m_font);
	if (dlg.DoModal() == IDOK)
	{
		dlg.GetCurrentFont(&m_font);
		m_fontHeight = dlg.m_cf.iPointSize;

		CHARFORMAT cf;
		ZeroMemory(&cf, sizeof(CHARFORMAT));
		cf.cbSize = sizeof(CHARFORMAT);
		cf.dwMask = CFM_FACE | CFM_SIZE;
		::lstrcpy(cf.szFaceName, dlg.GetFaceName());

		cf.yHeight = m_fontHeight;

		GetRichEditCtrl().SetDefaultCharFormat(cf);
	}
}


BOOL CEtermServerView::PreTranslateMessage(MSG* pMsg)
{
	if (pMsg->message == WM_KEYDOWN)
	{
		if (pMsg->wParam == VK_ESCAPE)
		{
			CRichEditCtrl &richEdit = this->GetRichEditCtrl();
			richEdit.SetSel(-1, -1);
			richEdit.ReplaceSel(_T("▶"));

		}
		else if (pMsg->wParam == VK_F12 ||
			(pMsg->wParam == VK_RETURN && pMsg->lParam == 18612225))
		{
			OnSendMsg();
			return TRUE;
		}
	}
	else if (pMsg->message == WM_SYSCOMMAND)
	{
		if (pMsg->wParam == SC_CLOSE)
		{
			return TRUE;
		}
	}
	return CRichEditView::PreTranslateMessage(pMsg);
}

void CEtermServerView::OnSendMsg()
{
	CRichEditCtrl &pRich = GetRichEditCtrl();

	long nStart, nEnd;
	CPoint pStart, pEnd;
	pRich.GetSel(nStart, nEnd);


	FINDTEXTEX ft;
	ft.chrg.cpMin = nStart;
	ft.chrg.cpMax = 0;
	ft.lpstrText = _T("▶");

	long n = pRich.FindText(0, &ft);

	pStart = pRich.GetCharPos(n);
	pEnd = pRich.GetCharPos(nStart);

	//判断是否在同一行
	//if (pStart.y != pEnd.y)	return;

	pRich.SetSel(n + 1, nStart);
	CString strCmd = pRich.GetSelText();


	pRich.SetSel(-1, -1);
	pRich.ReplaceSel(_T("\r\n"));

	if (strCmd.CompareNoCase(_T("CLS")) == 0)
	{
		pRich.SetSel(0, -1);
		pRich.Clear();
		pRich.GetFocus();
	}
	else
	{
		CString strConfig = this->GetDocument()->GetTitle();
		auto it = Global::configlist.find(strConfig);

		if (it != Global::configlist.end())
		{
			if (it->second->m_nState == AVAILABLE)
			{
				it->second->m_strCmd = strCmd;
				it->second->m_strResponse.Empty();
				it->second->m_EtermPacket.m_strResponse.Empty();
				it->second->m_EtermPacket.m_vecRev.clear();
				it->second->m_EtermPacket.m_cmdType = normal;
				it->second->SendMsg((string)CT2A(strCmd));
			}
		}
	}
}

UINT UserLogin(LPVOID lParam)
{
	__try
	{
		CString* strConfig = (CString*)lParam;
		auto it = Global::configlist.find(*strConfig);
		if (it != Global::configlist.end())
		{
			ShowInfo::SendShowInfo(0, it->first, _T("正在登录服务器……"));

			bool bLogin = false;
			if (it->second->UserLogin())
			{
				if (it->second->m_config.AutoSI)
				{
					ShowInfo::SendShowInfo(0, it->first, _T("登录成功，正在签入"));
				}
				int nWait = 0;
				bool bsign = false;
				while (true)
				{
					Sleep(100);
					if (nWait > 150) break;
					if (it->second->m_nState == LOGIN_SUCCESS&&!bsign)
					{
						if (it->second->m_config.AutoSI)
						{
							it->second->SendMsg((string)CT2A(it->second->m_config.SI));
							it->second->m_cs.Lock();							
							it->second->m_config.Count+=2;
							it->second->m_cs.Unlock();
							ShowInfo::SendShowInfo(3, it->first, it->second->m_config.SI, it->second->m_config.Count);
							bsign = true;
						}
					}
					if (it->second->m_nState == AVAILABLE)
					{
						break;
					}					
					nWait++;
				}
				//if (it->second->m_nState != AVAILABLE) it->second->m_nState = AVAILABLE;

				static int nSuccess = 0;
				if (nSuccess == 0)
				{
					for (auto cit = Global::configlist.begin(); cit != Global::configlist.end(); cit++)
					{
						if (cit->second->m_nState == AVAILABLE)
						{
							nSuccess++;
						}
					}
					if (nSuccess >= 1)
					{
						Global::UpdateEtermState(true);
					}
				}

				ShowInfo::SendShowInfo(0, it->first, it->second->m_nState == AVAILABLE ? _T("登录成功") : _T("登录失败"));
				/*if (it->second->m_nState != AVAILABLE)
				{
					CView* pView = Global::GetView(it->first);
					if (pView)	PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));
				}*/
			}
			else
			{
				it->second->m_nState = CONNECT_FAIL;

				ShowInfo::SendShowInfo(0, it->first, _T("登录失败"));
			}
		}
		if (strConfig)
		{
			delete strConfig;
			strConfig = nullptr;
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}
	return 0;
}


LRESULT CEtermServerView::ConnectServer(WPARAM wParam, LPARAM lParam)
{
	__try
	{

		int nType = (int)wParam;
		CString* strConfig = (CString*)lParam;

		if (nType == 0)
		{
			Global::WriteLog(CLog("开始连接"));

			BOOL bRet = FALSE;

			auto it = Global::configlist.find(*strConfig);
			if (it != Global::configlist.end())
			{
				it->second->ReleaseEtermSocket();

				if (it->second->m_config.MaxCount - it->second->m_config.Count >= Threshold)
				{

					it->second->InitEtermSocket(&GetRichEditCtrl(), &m_SslTrustedCertHashList);

					if (it->second->m_config.IsSSL)
						bRet = it->second->AddLayer(it->second->m_pSslLayer);

					if (Global::proxy.type)
					{
						if (Global::proxy.type == 1)
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_SOCKS4, Global::proxy.host.c_str(), Global::proxy.port);
						else if (Global::proxy.type == 2)
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_SOCKS4A, Global::proxy.host.c_str(), Global::proxy.port);
						else if (Global::proxy.type == 3 && (Global::proxy.user.empty() || Global::proxy.pwd.empty()))
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_SOCKS5, Global::proxy.host.c_str(), Global::proxy.port);
						else if (Global::proxy.type == 3 && (!Global::proxy.user.empty() && !Global::proxy.pwd.empty()))
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_SOCKS5, Global::proxy.host.c_str(), Global::proxy.port, Global::proxy.user.c_str(), Global::proxy.pwd.c_str());
						else if (Global::proxy.type == 4 && (Global::proxy.user.empty() || Global::proxy.pwd.empty()))
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_HTTP11, Global::proxy.host.c_str(), Global::proxy.port);
						else if (Global::proxy.type == 4 && (!Global::proxy.user.empty() && !Global::proxy.pwd.empty()))
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_HTTP11, Global::proxy.host.c_str(), Global::proxy.port, Global::proxy.user.c_str(), Global::proxy.pwd.c_str());
						else
							it->second->m_pProxyLayer->SetProxy(PROXYTYPE_NOPROXY);
						VERIFY(it->second->AddLayer(it->second->m_pProxyLayer));
					}

					bRet = it->second->Create();

					if (bRet&&it->second->m_config.KeepAlive)
						it->second->SetKeepAlive();

					if (it->second->m_config.IsSSL)
						bRet = it->second->m_pSslLayer->InitClientSSL();

					ShowInfo::SendShowInfo(0, it->second->m_config.UserName, _T("正在建立Socket……"));

					CSocketHelper socketHelper;

					bRet = it->second->Connect(socketHelper.GetIP(it->second->m_config.ServerIP), it->second->m_config.Port);

					if (!bRet)
					{
						if (GetLastError() != WSAEWOULDBLOCK)
						{
							ShowInfo::SendShowInfo(0, it->second->m_config.UserName, _T("建立Socket失败"));
						}
					}
					Global::WriteLog(CLog("结束连接"));
				}
				else
				{
					ShowInfo::SendShowInfo(0, it->first, _T("已达最大流量"), 0);
				}
			}
		}
		else if (nType == 1)
		{
			AfxBeginThread(UserLogin, (LPVOID)strConfig);
			return 0;
		}
		else if (nType == 2)
		{
			auto it = Global::configlist.find(*strConfig);
			if (it != Global::configlist.end())
			{
				it->second->ReleaseEtermSocket();
				it->second->m_nState = CONFIG_STATE::CLOSE_CONN;
			}
			int nSuccess = 0;
			for (auto cit = Global::configlist.begin(); cit != Global::configlist.end(); cit++)
			{
				if (cit->second->m_nState == AVAILABLE)
				{
					nSuccess++;
				}
			}
			if (nSuccess == 0)
			{
				Global::UpdateEtermState(false);
			}
			ShowInfo::SendShowInfo(0, this->GetDocument()->GetTitle(), _T("连接已关闭"));

		}

		if (strConfig)
		{
			delete strConfig;
			strConfig = nullptr;
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}
	return 0;
}

LRESULT CEtermServerView::OnVerifyCert(WPARAM wParam, LPARAM lParam)
{
	__try
	{
		t_SslCertData *pData = reinterpret_cast<t_SslCertData*>(wParam);
		CString* strConfig = (CString*)lParam;

		if (pData&&strConfig)
		{
			auto it = Global::configlist.find(*strConfig);
			if (it != Global::configlist.end())
			{
				it->second->AddSslCertHashToTrustedList(pData->hash);
				it->second->m_pSslLayer->SetNotifyReply(pData->priv_data, SSL_VERIFY_CERT, 1);
			}
			delete strConfig;
			strConfig = nullptr;
			delete pData;
			pData = nullptr;
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}

	return 0;
}


LRESULT CEtermServerView::SystemCommand(WPARAM wParam, LPARAM lParam)
{
	CString strConfig = this->GetDocument()->GetTitle();
	auto it = Global::configlist.find(strConfig);
	if (it != Global::configlist.end()){
		CScript script;
		script.method = strConfig.GetBuffer();
		wstring data = wstring((LPCTSTR)lParam);
		if (data.length() > 32){
			script.guid = data.substr(0, 32);
			script.script = data.substr(32);
			script.timeout = wParam;
			it->second->cmdlist.push_back(script);
		}
		strConfig.ReleaseBuffer();
	}
	return 0;
}

CMD_TYPE cmdType(wstring& cmd)
{
	if (cmd.find(_T("PRINT:")) == 0)
		return print;
	else if (cmd.find(_T("PNR:")) == 0)
		return pnr;
	else
	{
		stringEx ex;
		cmd = ex.replace(cmd, _T("\\[RN\\]"), _T("\r"));
		return normal;
	}
}

void SystemInvoke(CString strConfig)
{
	CEtermServerView* pView = (CEtermServerView*)Global::GetView(strConfig);
	auto it = Global::configlist.find(strConfig);

	if (pView &&
		it != Global::configlist.end())
	{
		while (!bSystemOver)
		{
			Sleep(100);
			__try
			{
				if (it->second->cmdlist.size() == 0) continue;

				it->second->m_cmdCS.Lock();
				CScript script = it->second->cmdlist.front();
				it->second->m_cmdCS.Unlock();

				__try
				{
					int nWait = 0;
					wstring cmd = script.script;

					//检查Redis,是否有重复订位情况
					string rdsData = GetBookInfo(cmd);
					if (!rdsData.empty()){
						it->second->m_cs.Lock();
						if (_tcslen(Global::szConnectString) > 0)
						{
							Global::pProxy->SetEtermData(_bstr_t(it->first.AllocSysString()), _bstr_t(script.guid.c_str()), _bstr_t(script.script.c_str()), _bstr_t(rdsData.c_str()));
							SetEvent(it->second->m_hNotify);
						}
						it->second->m_cs.Unlock();

						it->second->m_cmdCS.Lock();
						it->second->cmdlist.pop_front();
						it->second->m_cmdCS.Unlock();
						continue;
					}

					if (it->second->m_config.Interval > 0)
					{
						CTimeSpan ts = CTime::GetCurrentTime() - it->second->m_ActiveTime;
						int ntime = ts.GetTotalSeconds() * 1000;
						ntime = min(ntime, it->second->m_config.Interval);
						if (ntime>0)
							Sleep(ntime);
					}

					int nresendcount = 0;
				resend:

					it->second->m_cs.Lock();
					it->second->m_EtermPacket.m_cmdType = cmdType(cmd);
					it->second->m_EtermPacket.m_cmd = cmd.c_str();
					it->second->m_EtermPacket.m_strResponse.Empty();
					it->second->m_EtermPacket.m_vecRev.clear();
					it->second->m_strResponse.Empty();
					it->second->m_config.Count++;
					it->second->m_cs.Unlock();

					ShowInfo::SendShowInfo(2, it->second->m_config.UserName, cmd.c_str(), it->second->m_config.Count);
										

					Global::WriteLog(CLog(cmd.c_str(), it->first));

					if (it->second->m_EtermPacket.m_cmdType == print)
						it->second->SendPrintMsg((string)CT2A(cmd.c_str()));
					else
						it->second->SendMsg((string)CT2A(cmd.c_str()));

					ResetEvent(it->second->pEvent);
					DWORD dwOutTime = ::WaitForSingleObject(it->second->pEvent, script.timeout);

					switch (dwOutTime)
					{
					case WAIT_OBJECT_0:
					{
										  SetEvent(it->second->pEvent);
										  it->second->m_cs.Lock();
										  it->second->m_config.Count++;
										  it->second->m_cs.Unlock();

										  if (it->second->m_config.IsSSL)
										  {
											  if (it->second->m_strResponse == _T("S") ||
												  it->second->m_strResponse.Find(_T("SIGN IN FIRST")) != -1)
											  {
												  it->second->SendMsg((string)CT2A(it->second->m_config.SI));
												  it->second->m_cs.Lock();
												  it->second->m_config.Count+=2;
												  it->second->m_cs.Unlock();
												  ShowInfo::SendShowInfo(3, it->first, it->second->m_config.SI, it->second->m_config.Count);
												  Sleep(500);
												  goto resend;
											  }
										  }
										  ShowInfo::SendShowInfo(3, it->first, cmd.c_str(), it->second->m_config.Count);

					}
						break;
					case WAIT_TIMEOUT:
					case WAIT_FAILED:
					{
										SetEvent(it->second->pEvent);

										/*it->second->m_cs.Lock();
										it->second->m_nState = CONNECT_FAIL;
										it->second->m_cs.Unlock();
										auto vit = Global::EtermViews.find(it->first);
										if (vit != Global::EtermViews.end())
										{
											::PostMessage(vit->second->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));

											int ncount = 0;
											while (true)
											{
												Sleep(500);
												if (it->second->m_nState == CONFIG_STATE::AVAILABLE)
												{
													break;
												}
												ncount++;
												if (ncount > 60) break;
											}
											nresendcount++;
											if (nresendcount<3)
												goto resend;
										}*/
					}
						break;
					default:
						SetEvent(it->second->pEvent);
						break;
					}

					it->second->m_cs.Lock();
					if (_tcslen(Global::szConnectString) > 0)
					{
						Global::pProxy->SetEtermData(_bstr_t(it->first.AllocSysString()), _bstr_t(script.guid.c_str()), _bstr_t(script.script.c_str()), _bstr_t(it->second->m_strResponse.AllocSysString()));
						SetEvent(it->second->m_hNotify);
					}
					it->second->m_cs.Unlock();

					thread th(SaveBookInfo, script.script, script.guid, it->second->m_strResponse, it->first);
					th.detach();

					//----------流量控制-----------------------------------------------------------------------------
					if (it->second->m_config.MaxCount - it->second->m_config.Count < Threshold)
					{
						CView* pView = Global::GetView(it->first);
						if (pView)
							::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(it->first)));
						CString strInfo;
						strInfo.Format(_T("配置[%s] 设定最大流量：%d 当前流量：%d 剩余流量：%d"), it->first, it->second->m_config.MaxCount, it->second->m_config.Count, it->second->m_config.MaxCount - it->second->m_config.Count);
						EmailHelper::SendEmail(_T("Eterm流量超限"), strInfo);
					}
				}
				__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation())){}

				it->second->m_cmdCS.Lock();
				it->second->cmdlist.pop_front();
				it->second->m_cmdCS.Unlock();

			}
			__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
			{
				continue;
			}
		}
	}
}

string GetBookInfo(wstring cmd)
{
	__try
	{
		if (cmd.find(_T("SS:")) == 0)
		{
			stringEx ex;
			EtermServer::RedisHelper rh;
			char szkey[1024] = { 0 };
			cmd = ex.replace(cmd, _T("TKTL\\d{4}/\\d{2}[A-Z]{3}"), _T(""));
			string temp = CW2A(cmd.c_str());
			md5 md5(temp.c_str(), temp.length());
			sprintf(szkey, "140106_140110_%s", md5.toString().c_str());
			string sret = rh.get(szkey);
			if (!sret.empty())
			{
				if (sret.find("SUCCESSFUL") != string::npos)
					return sret;
			}
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}
	return "";
}
void SaveBookInfo(wstring cmd, wstring guid, CString response, CString Config)
{
	__try
	{
		if (response.Find(_T("SUCCESSFUL")) != -1)
		{
			stringEx ex;
			EtermServer::RedisHelper rh;
			char szkey[1024] = { 0 };
			cmd = ex.replace(cmd, _T("TKTL\\d{4}/\\d{2}[A-Z]{3}"), _T(""));
			string temp = CW2A(cmd.c_str());
			md5 md5(temp.c_str(), temp.length());
			sprintf(szkey, "140106_140110_%s", md5.toString().c_str());
			bool bret = rh.set(szkey, (string)CT2A(response), 1200);
			if (!bret){
				Global::WriteLog(CLog("写入Redis失败:" + string(szkey)));
			}
			wstring sline = ex.FindStr(wstring(response), _T("[A-Z0-9]{2}\\d{3,5}\\s+[A-Z](\\d|)\\s+[A-Z]{2}\\d{2}[A-Z]{3}\\s+[A-Z]{6}\\s+[A-Z]{2}\\d\\s+\\d{4}\\s+\\d{4}(\\+\\d|)"));
			vector<wstring> vec = ex.split(sline, _T("\\s+"));
			BookData book;
			if (vec.size() >= 7)
			{
				book.flightno = vec[0].c_str();
				book.aircom = book.flightno.GetLength() > 2 ? book.flightno.Left(2) : _T("");
				eterm3Helper eh;
				eh.StanDate(vec[2].c_str(), vec[5].c_str(), vec[6].c_str());
				book.sdate = eh.sdate;
				book.edate = eh.edate;
				book.cabin = vec[1].c_str();
				book.ecity = vec[3].substr(3).c_str();
				book.scity = vec[3].substr(0, 3).c_str();
			}
			book.guid = guid.c_str();
			wstring pnr = ex.FindStr((wstring)response, _T("[A-Z0-9]{5,6}\\s+-EOT\\s+SUCCESSFUL"), _T("-EOT\\s+SUCCESSFUL|\\s"));
			book.pnr = pnr.c_str();
			book.request = cmd.c_str();
			book.response = response;
			book.md5 = CA2W(szkey);
			book.config = Config;
			book.officeno = Global::etermconfig.OfficeNo;
			book.serverip = Global::ServerUrl;

			EtermConfigData etermData;
			etermData.AppendBooking(book);
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}
}