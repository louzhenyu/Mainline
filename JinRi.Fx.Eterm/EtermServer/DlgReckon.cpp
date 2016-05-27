// DlgReckon.cpp : 实现文件
//

#include "stdafx.h"
#include "EtermServer.h"
#include "DlgReckon.h"
#include "afxdialogex.h"


// CDlgReckon 对话框

IMPLEMENT_DYNAMIC(CDlgReckon, CDialogEx)

CDlgReckon::CDlgReckon(CWnd* pParent /*=NULL*/)
	: CDialogEx(CDlgReckon::IDD, pParent)
{

}

CDlgReckon::~CDlgReckon()
{
}

void CDlgReckon::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CDlgReckon, CDialogEx)
	ON_WM_TIMER()
END_MESSAGE_MAP()


// CDlgReckon 消息处理程序


void CDlgReckon::OnTimer(UINT_PTR nIDEvent)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	static unsigned short inum = 10;

	CString strInfo;
	strInfo.Format(_T("检测到相同端口的软件正在运行，软件将在%d秒内关闭！"), inum);
	GetDlgItem(IDC_INFO)->SetWindowTextW(strInfo);	
	if (inum <= 0)
	{
		OnCancel();
	}
	inum--;

	CDialogEx::OnTimer(nIDEvent);
}


BOOL CDlgReckon::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  在此添加额外的初始化
	SetTimer(1, 1000, NULL);
	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常:  OCX 属性页应返回 FALSE
}
