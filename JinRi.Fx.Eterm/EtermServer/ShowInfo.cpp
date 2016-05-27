#include "stdafx.h"
#include "ShowInfo.h"
#include "MainFrm.h"
#include "Global.h"

ShowInfo::ShowInfo()
{
	count = 0;
}


ShowInfo::~ShowInfo()
{
}

void ShowInfo::SendShowInfo(int nType, CString strConfig, CString strInfo, int count)
{
	__try
	{
		ShowInfo* si = new ShowInfo;
		si->strConfig = strConfig;
		si->strInfo = strInfo;
		si->count = count;
		CMainFrame* pMainFrame = (CMainFrame *)AfxGetApp()->m_pMainWnd;
		if (pMainFrame)
		{
			::PostMessage(pMainFrame->m_wndConfigureListBar.GetSafeHwnd(), WM_CONFIG_INFO, nType, (LPARAM)si);
		}		
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return;
	}
}