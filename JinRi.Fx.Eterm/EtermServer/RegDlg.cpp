// RegDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "EtermServer.h"
#include "RegDlg.h"
#include "afxdialogex.h"
#include "SocketHelper.h"
#include "md5.h"
#include "Encrypt\Encrypt.h"
#include "Registry.h"
#include "Global.h"

// CRegDlg 对话框

IMPLEMENT_DYNAMIC(CRegDlg, CDialogEx)

CRegDlg::CRegDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CRegDlg::IDD, pParent)
{

}

CRegDlg::~CRegDlg()
{
}

void CRegDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CRegDlg, CDialogEx)
	ON_BN_CLICKED(IDOK, &CRegDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &CRegDlg::OnBnClickedCancel)
END_MESSAGE_MAP()


// CRegDlg 消息处理程序


void CRegDlg::OnBnClickedOk()
{
	CString m_regNo, m_Marchine;
	GetDlgItem(IDC_EDIT_REG)->GetWindowText(m_regNo);
	GetDlgItem(IDC_EDIT_MATCHIN)->GetWindowText(m_Marchine);

	if (m_regNo.IsEmpty())
	{
		MessageBox(_T("请输入注册码!"),_T("EtermServer服务端注册"),MB_OK|MB_ICONEXCLAMATION);
		GetDlgItem(IDC_EDIT_MATCHIN)->SetFocus();
		return;
	}

	USES_CONVERSION;

	Encrypt encrypt;
	unsigned char key[] = "&feghtyj";
	CString strValid = CString(A2T(encrypt.decrypt(key, (char*)T2A(m_regNo))));

	if (m_Marchine == strValid)
	{
		CRegistry reg(HKEY_LOCAL_MACHINE);
		BOOL bRet = reg.Open(_T("SOFTWARE\0"));
		if (bRet)
		{
			BOOL bRet = reg.Open(_T("EtermServer\0"));
			if (!bRet)
				bRet = reg.CreateKey(_T("EtermServer"));
			bRet = reg.Open(_T("Register\0"));
			if (!bRet)
				bRet = reg.CreateKey(_T("Register"));
			bRet = reg.Write(_T(""), m_regNo);

			if (bRet)
			{
				MessageBox(_T("注册成功!"), _T("EtermServer服务端注册"), MB_OK | MB_ICONINFORMATION);
				CDialogEx::OnOK();
			}
		}
		if (!bRet)
		{
			FILE* file = NULL;
			TCHAR szFile[1024] = { 0 };
			_stprintf_s(szFile, _T("%s\\register.dat"), Global::szAppPath);
			errno_t no = _tfopen_s(&file, szFile, _T("w"));
			if (no == 0)
			{
				char szBuf[1024] = { 0 };
				fputs(CT2A(m_regNo), file);
				fclose(file);

				MessageBox(_T("注册成功!"), _T("EtermServer服务端注册"), MB_OK | MB_ICONINFORMATION);
				CDialogEx::OnOK();			
			}
			else
			{
				MessageBox(_T("注册失败!"), _T("EtermServer服务端注册"), MB_OK | MB_ICONEXCLAMATION);
			}
		}
	}
	else
	{
		MessageBox(_T("注册失败!"), _T("EtermServer服务端注册"), MB_OK | MB_ICONEXCLAMATION);
	}
}


BOOL CRegDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	USES_CONVERSION;

	CSocketHelper serialNo;
	char* szSerialNo = serialNo.GetMAC();

	md5 md5(szSerialNo, strlen(szSerialNo));
	
	GetDlgItem(IDC_EDIT_MATCHIN)->SetWindowText(A2T(md5.toString().c_str()));

	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常:  OCX 属性页应返回 FALSE
}


void CRegDlg::OnBnClickedCancel()
{
	exit(0);
	CDialogEx::OnCancel();
}


void CRegDlg::OnCancel()
{
	exit(0);
	CDialogEx::OnCancel();
}
