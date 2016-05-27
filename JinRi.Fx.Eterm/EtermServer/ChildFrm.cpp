
// ChildFrm.cpp : CChildFrame 类的实现
//

#include "stdafx.h"
#include "EtermServer.h"
#include "MainFrm.h"
#include "ChildFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// CChildFrame

IMPLEMENT_DYNCREATE(CChildFrame, CMDIChildWndEx)

BEGIN_MESSAGE_MAP(CChildFrame, CMDIChildWndEx)
	ON_WM_CLOSE()
END_MESSAGE_MAP()

// CChildFrame 构造/析构

CChildFrame::CChildFrame()
{
	// TODO:  在此添加成员初始化代码
}

CChildFrame::~CChildFrame()
{
}


BOOL CChildFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO:  在此处通过修改 CREATESTRUCT cs 来修改窗口类或样式
	if( !CMDIChildWndEx::PreCreateWindow(cs) )
		return FALSE;

	return TRUE;
}

// CChildFrame 诊断

#ifdef _DEBUG
void CChildFrame::AssertValid() const
{
	CMDIChildWndEx::AssertValid();
}

void CChildFrame::Dump(CDumpContext& dc) const
{
	CMDIChildWndEx::Dump(dc);
}
#endif //_DEBUG

// CChildFrame 消息处理程序


void CChildFrame::OnClose()
{
	CMainFrame *pMain = (CMainFrame *)AfxGetApp()->m_pMainWnd;
	if (pMain)
	{
		LVFINDINFO info;
		info.flags = LVFI_STRING;
		info.psz = GetActiveView()->GetDocument()->GetTitle();
		int nIndex=pMain->m_wndConfigureListBar.m_ConfigureList.FindItem(&info);
		if (nIndex == -1)
		{
			CMDIChildWndEx::OnClose();
		}
		return;
	}

	CMDIChildWndEx::OnClose();
}
