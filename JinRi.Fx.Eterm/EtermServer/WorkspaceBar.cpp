// WorkspaceBar.cpp : 实现文件
//

#include "stdafx.h"
#include "EtermServer.h"
#include "WorkspaceBar.h"
#include "MainFrm.h"
#include "DlgConfig.h"
#include "ShowInfo.h"
#include "Global.h"
#include "stringEx.h"
// CWorkspaceBar

IMPLEMENT_DYNAMIC(CWorkspaceBar, CDockablePane)

CWorkspaceBar::CWorkspaceBar()
{

}

CWorkspaceBar::~CWorkspaceBar()
{
}


BEGIN_MESSAGE_MAP(CWorkspaceBar, CDockablePane)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_CONTEXTMENU()
	ON_MESSAGE(WM_CONFIG_INFO,ShowConfig)
	ON_COMMAND(ID_CONFIG_ADD, &CWorkspaceBar::OnConfigAdd)
	ON_COMMAND(ID_CONFIG_MODIFY, &CWorkspaceBar::OnConfigModify)
	ON_COMMAND(ID_CONFIG_DELETE, &CWorkspaceBar::OnConfigDelete)
	ON_COMMAND(ID_CONNECT_SERVER, &CWorkspaceBar::OnConnectServer)
	ON_COMMAND(ID_DISCONNECT_SERVER, &CWorkspaceBar::OnDisconnectServer)
	ON_COMMAND(ID_CONNECT_ALL, &CWorkspaceBar::OnConnectAll)
	ON_COMMAND(ID_DISCONNECT_ALL, &CWorkspaceBar::OnDisconnectAll)
END_MESSAGE_MAP()



// CWorkspaceBar 消息处理程序




int CWorkspaceBar::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDockablePane::OnCreate(lpCreateStruct) == -1)
		return -1;

	CBitmap bmWhite;
	CBitmap bmGreen;
	CBitmap bmRed;
	
	bmWhite.LoadBitmap(IDB_WHITE);
	bmRed.LoadBitmap(IDB_RED);
	bmGreen.LoadBitmap(IDB_GREEN);
	
	m_ImageList.Create(16, 16, ILC_COLORDDB | ILC_MASK, 3, 3);
	m_ImageList.Add(&bmWhite, RGB(255, 255, 255));
	m_ImageList.Add(&bmRed, RGB(255, 255, 255));
	m_ImageList.Add(&bmGreen, RGB(255, 255, 255));

	//创建用户列表控件
	CRect rectDummy;
	rectDummy.SetRectEmpty();

	const DWORD dwStyle = WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS | WS_VSCROLL | LVS_REPORT;

	m_ConfigureList.Create(dwStyle, rectDummy, this, ID_CONFIG_LIST);
	m_ConfigureList.SendMessage(LVM_SETEXTENDEDLISTVIEWSTYLE, 0, LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES | LVS_EX_HEADERDRAGDROP);
	m_ConfigureList.SetImageList(&m_ImageList, LVSIL_SMALL);
	m_ConfigureList.InsertColumn(0, _T("帐号"), LVCFMT_LEFT, 120);
	m_ConfigureList.InsertColumn(1, _T("类型"), LVCFMT_LEFT, 80);
	m_ConfigureList.InsertColumn(2, _T("服务器地址"), LVCFMT_LEFT, 120);
	m_ConfigureList.InsertColumn(3, _T("服务器端口"), LVCFMT_LEFT, 80);
	m_ConfigureList.InsertColumn(4, _T("是否加密"), LVCFMT_LEFT, 80);
	m_ConfigureList.InsertColumn(5, _T("连接状态"), LVCFMT_LEFT, 120);
	m_ConfigureList.InsertColumn(6, _T("指令队列"), LVCFMT_LEFT, 100);
	m_ConfigureList.InsertColumn(7, _T("当前指令"), LVCFMT_LEFT, 250);
	m_ConfigureList.InsertColumn(8, _T("指令执行间隔"), LVCFMT_LEFT, 100);
	m_ConfigureList.InsertColumn(9, _T("指令时间"), LVCFMT_LEFT, 150);
	m_ConfigureList.InsertColumn(10, _T("剩余流量"), LVCFMT_LEFT, 150);
	
	CString strPath;
	TCHAR szbuf[1024] = { 0 };
	strPath.Format(_T("%s\\Config.ini"), Global::szAppPath);
	int  nColumnCount = m_ConfigureList.GetHeaderCtrl().GetItemCount();
	
	::GetPrivateProfileString(_T("SYSTEM"), _T("column"), _T(""), szbuf, sizeof(szbuf) / sizeof(szbuf[0]), strPath);
	if (_tcslen(szbuf) > 0)
	{
		stringEx ex;
		vector<wstring> vec = ex.split(wstring(szbuf), _T(","));
		if (vec.size() == nColumnCount)
		{			
			int* pOrders = new int[nColumnCount];
			for (int i = 0; i < nColumnCount; i++)
				pOrders[i] = _ttoi(vec[i].c_str());
			m_ConfigureList.SetColumnOrderArray(nColumnCount, pOrders);
			delete[] pOrders;
		}
	}
	
	vector<Config*> configs = m_cfgdata.LoadConfig();
	int nIndex = 0;
	CString strValue;
	for (auto it = configs.begin(); it != configs.end(); it++, nIndex++)
	{
		PostMessage(WM_COMMAND, ID_FILE_NEW);

		m_ConfigureList.InsertItem(nIndex, (*it)->UserName,0);
		m_ConfigureList.SetItemText(nIndex, 1, (*it)->Type);
		m_ConfigureList.SetItemText(nIndex, 2, (*it)->ServerIP);
		strValue.Format(_T("%d"), (*it)->Port);
		m_ConfigureList.SetItemText(nIndex, 3, strValue);
		m_ConfigureList.SetItemText(nIndex, 4, (*it)->IsSSL ? _T("true") : _T("false"));
		strValue.Format(_T("%d"), (*it)->Interval);
		m_ConfigureList.SetItemText(nIndex, 8, strValue);
		strValue.Format(_T("%d"), (*it)->MaxCount - (*it)->Count);
		m_ConfigureList.SetItemText(nIndex, 10, strValue);
		m_ConfigureList.SetItemData(nIndex, (DWORD_PTR)(*it));

		if (Global::configlist.find((*it)->UserName) == Global::configlist.end())
		{
			CEtermSocket* pSocket = new CEtermSocket;
			pSocket->m_config = **it;
			Global::configlist.insert(make_pair((*it)->UserName, unique_ptr<CEtermSocket>(pSocket)));
		}
	}

	return 0;
}


void CWorkspaceBar::OnSize(UINT nType, int cx, int cy)
{
	CDockablePane::OnSize(nType, cx, cy);

	if (CanBeResized())
	{
		CRect rc;
		GetClientRect(rc);

		m_ConfigureList.SetWindowPos(NULL, rc.left, rc.top, rc.Width(), rc.Height(), SWP_NOACTIVATE | SWP_NOZORDER);
	}
}


void CWorkspaceBar::OnContextMenu(CWnd* pWnd, CPoint point)
{
	CMenu menu;
	menu.LoadMenu(IDR_MENU_CONFIG);

	CMenu* pSumMenu = menu.GetSubMenu(0);

	if (AfxGetMainWnd()->IsKindOf(RUNTIME_CLASS(CMDIFrameWndEx)))
	{
		CMFCPopupMenu* pPopupMenu = new CMFCPopupMenu;

		if (!pPopupMenu->Create(this, point.x, point.y, (HMENU)pSumMenu->m_hMenu, FALSE, TRUE))
			return;

		((CMDIFrameWndEx*)AfxGetMainWnd())->OnShowPopupMenu(pPopupMenu);
		UpdateDialogControls(this, FALSE);
	}
}


BOOL CWorkspaceBar::PreTranslateMessage(MSG* pMsg)
{
	__try
	{

		if (pMsg->message == WM_LBUTTONDOWN || pMsg->message == WM_RBUTTONDOWN)
		{
			if (pMsg->hwnd == m_ConfigureList.m_hWnd)
			{
				UINT flags = 0;
				CString strUserName;
				CPoint point = pMsg->pt;
				m_ConfigureList.ScreenToClient(&point);

				int nItem = m_ConfigureList.HitTest(point, &flags);
				if (nItem != -1)
				{
					strUserName = m_ConfigureList.GetItemText(nItem, 0);
					((CMainFrame*)AfxGetMainWnd())->MDIActivate(Global::GetView(strUserName)->GetParentFrame());
				}
			}
		}
		return CDockablePane::PreTranslateMessage(pMsg);
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return CDockablePane::PreTranslateMessage(pMsg);
	}
}


//根据选项卡标题名称激活该选项卡

void CWorkspaceBar::OnConfigAdd()
{
	CDlgConfig dlg;
	dlg.DoModal();
}


void CWorkspaceBar::OnConfigModify()
{
	CDlgConfig dlg;
	dlg.m_bEdit = true;
	int nIndex = m_ConfigureList.GetNextItem(-1, LVNI_ALL | LVNI_SELECTED);
	if (nIndex >= 0) dlg.m_config = (Config*)m_ConfigureList.GetItemData(nIndex);
	dlg.DoModal();
}


void CWorkspaceBar::OnConfigDelete()
{
	int nIndex = m_ConfigureList.GetNextItem(-1, LVNI_ALL | LVNI_SELECTED);
	if (nIndex >= 0)
	{
		CString strInfo;
		CString strConfig = m_ConfigureList.GetItemText(nIndex, 0);
		strInfo.Format(_T("是否需要删除这个配置[%s]吗？"), strConfig);
		if (MessageBox(strInfo, _T("Eterm配置"), MB_YESNO | MB_ICONINFORMATION) == IDNO) return;
		CConfigData cfd;
		if (cfd.DeleteConfig(strConfig))
		{
			m_ConfigureList.DeleteItem(nIndex);
			CView* pView = Global::GetView(strConfig);
			if (pView)
			{
				::SendMessage(pView->GetSafeHwnd(), WM_COMMAND, ID_FILE_CLOSE, NULL);
				auto it = Global::configlist.find(strConfig);
				if (it != Global::configlist.end())
				{
					it->second->ReleaseEtermSocket();
					it->second->m_nState = ETERM_OVER;
				}
			}
		}
	}
}


void CWorkspaceBar::OnConnectServer()
{
	int nIndex = m_ConfigureList.GetNextItem(-1, LVNI_ALL | LVNI_SELECTED);
	if (nIndex >= 0)
	{
		CString strConfig = m_ConfigureList.GetItemText(nIndex, 0);
		CView* pView = Global::GetView(strConfig);
		if (pView)
			::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(strConfig)));
	}
}


void CWorkspaceBar::OnDisconnectServer()
{
	int nIndex = m_ConfigureList.GetNextItem(-1, LVNI_ALL | LVNI_SELECTED);
	if (nIndex >= 0)
	{
		CString strConfig = m_ConfigureList.GetItemText(nIndex, 0);
		CView* pView = Global::GetView(strConfig);
		if (pView)
			::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(strConfig)));
	}
}


void CWorkspaceBar::OnConnectAll()
{
	int nCount = m_ConfigureList.GetItemCount();
	for (int i = 0; i < nCount; i++)
	{
		CString strConfig = m_ConfigureList.GetItemText(i, 0);
		CView* pView = Global::GetView(strConfig);
		if (pView)
		{
			::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(strConfig)));
		}
	}
}


void CWorkspaceBar::OnDisconnectAll()
{
	int nCount = m_ConfigureList.GetItemCount();
	for (int i = 0; i < nCount; i++)
	{
		CString strConfig = m_ConfigureList.GetItemText(i, 0);
		CView* pView = Global::GetView(strConfig);
		if (pView)
		{
			::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(strConfig)));
		}
	}
}

LRESULT CWorkspaceBar::ShowConfig(WPARAM wParam, LPARAM lParam)
{
	__try
	{

		int nType = (int)wParam;
		ShowInfo* si = (ShowInfo*)lParam;
		LVFINDINFO lv;
		lv.flags = LVFI_STRING;
		lv.psz = si->strConfig;
		int nIndex = m_ConfigureList.FindItem(&lv);
		CString strInfo;

		if (nIndex >= 0)
		{
			LVITEM lvItem;
			memset(&lvItem, 0, sizeof(LVITEM));
			lvItem.mask = LVIF_IMAGE;
			lvItem.iItem = nIndex;
			lvItem.iSubItem = 0;
			int ncount = _ttoi(m_ConfigureList.GetItemText(nIndex, 6));
			Config* pConfig = (Config*)m_ConfigureList.GetItemData(nIndex);

			if (nType == 0)
			{
				m_ConfigureList.SetItemText(nIndex, 5, si->strInfo);
				m_ConfigureList.SetItemText(nIndex, 9, CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S"));
			}
			else if (nType == 1)
			{
				lvItem.iImage = si->count > 0 ? 1 : 2;
				m_ConfigureList.EnsureVisible(nIndex, FALSE);
				m_ConfigureList.SetItem(&lvItem);

				strInfo.Format(_T("%d"), si->count);
				m_ConfigureList.SetItemText(nIndex, 6, strInfo);
				m_ConfigureList.SetItemText(nIndex, 9, CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S"));
			}
			else if (nType == 2)
			{
				m_ConfigureList.SetItemText(nIndex, 7, si->strInfo);
				m_ConfigureList.SetItemText(nIndex, 9, CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S"));

				if (si->count > 0)
				{
					pConfig->Count = si->count;
					strInfo.Format(_T("%d"), pConfig->MaxCount - si->count);
					m_ConfigureList.SetItemText(nIndex, 10, strInfo);
				}
			}
			else if (nType == 3)
			{				
				m_ConfigureList.SetItemText(nIndex, 7, si->strInfo);
				m_ConfigureList.SetItemText(nIndex, 9, CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S"));

				if (si->count>0)
				{
					pConfig->Count = si->count;
					strInfo.Format(_T("%d"), pConfig->MaxCount - si->count);
					m_ConfigureList.SetItemText(nIndex, 10, strInfo);
					m_cfgdata.UpdateUseCount(si->strConfig, si->count);
				}
			}
			else if (nType == 4)
			{
				m_ConfigureList.SetItemText(nIndex, 2, si->strInfo);
			}			
		}

		delete si;
	
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}
	return 0;
}