// DlgConfig.cpp : 实现文件
//

#include "stdafx.h"
#include "EtermServer.h"
#include "DlgConfig.h"
#include "afxdialogex.h"
#include "ConfigData.h"
#include "MainFrm.h"
#include "Global.h"
#include "EtermServerView.h"
// CDlgConfig 对话框

IMPLEMENT_DYNAMIC(CDlgConfig, CDialogEx)

CDlgConfig::CDlgConfig(CWnd* pParent /*=NULL*/)
	: CDialogEx(CDlgConfig::IDD, pParent)
{
	m_bEdit = false;
	m_config = NULL;
}

CDlgConfig::~CDlgConfig()
{
}

void CDlgConfig::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO_SERVER, m_cmbServer);
	DDX_Control(pDX, IDC_COMBO_TYPE, m_cmbType);
}


BEGIN_MESSAGE_MAP(CDlgConfig, CDialogEx)
	ON_BN_CLICKED(IDC_SECRET, &CDlgConfig::OnBnClickedSecret)
	ON_BN_CLICKED(IDOK, &CDlgConfig::OnBnClickedOk)
END_MESSAGE_MAP()


// CDlgConfig 消息处理程序


BOOL CDlgConfig::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	InitControl();

	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常:  OCX 属性页应返回 FALSE
}

void CDlgConfig::InitControl()
{
	CConfigData cfd;
	vector<CString> vec = cfd.LoadServer();
	for (int i = 0; i < vec.size(); i++)
	{
		m_cmbServer.AddString(vec[i]);
	}
	if (m_bEdit)
	{
		this->SetWindowText(_T("Eterm配置修改"));
		GetDlgItem(IDOK)->SetWindowText(_T("修改(&M)"));
		if (m_config)
		{
			int nIndex = m_cmbType.FindString(-1, m_config->Type);
			if (nIndex == -1) nIndex == 0;
			m_cmbType.SetCurSel(nIndex);
			CString strVal;
			strVal.Format(_T("%d"), m_config->Interval);
			GetDlgItem(IDC_EDIT_INTERVAL)->SetWindowText(strVal);
			strVal.Format(_T("%d"), m_config->Port);
			GetDlgItem(IDC_EDIT_PORT)->SetWindowText(strVal);
			strVal.Format(_T("%d"), m_config->MaxCount);
			GetDlgItem(IDC_EDIT_MAX_COUNT)->SetWindowText(strVal);
			strVal.Format(_T("%d"), m_config->Count);
			GetDlgItem(IDC_EDIT_CURRENT_COUNT)->SetWindowText(strVal);
			GetDlgItem(IDC_EDIT_ACCOUNT)->SetWindowText(m_config->UserName);
			GetDlgItem(IDC_EDIT_PASSWORD)->SetWindowText(m_config->PassWord);
			GetDlgItem(IDC_EDIT_SI)->SetWindowText(m_config->SI);
			((CButton*)GetDlgItem(IDC_AUTO_SI))->SetCheck(m_config->AutoSI);
			nIndex = m_cmbServer.FindString(-1, m_config->ServerIP);
			if (nIndex >= 0) m_cmbServer.SetCurSel(nIndex);
			((CButton*)GetDlgItem(IDC_SECRET))->SetCheck(m_config->IsSSL);
			((CButton*)GetDlgItem(IDC_KEEP_ALIVE))->SetCheck(m_config->KeepAlive);
		}
	}
	else
	{
		this->SetWindowText(_T("Eterm配置添加"));
		GetDlgItem(IDOK)->SetWindowText(_T("添加(&A)"));
		m_cmbType.SetCurSel(0);
		GetDlgItem(IDC_EDIT_INTERVAL)->SetWindowText(_T("0"));
		GetDlgItem(IDC_EDIT_PORT)->SetWindowText(_T("350"));
		GetDlgItem(IDC_EDIT_MAX_COUNT)->SetWindowText(_T("130000"));
		GetDlgItem(IDC_EDIT_CURRENT_COUNT)->SetWindowText(_T("0"));
		((CButton*)GetDlgItem(IDC_KEEP_ALIVE))->SetCheck(TRUE);
		if (m_cmbServer.GetCount() > 0) m_cmbServer.SetCurSel(0);
	}
}

void CDlgConfig::OnBnClickedSecret()
{
	GetDlgItem(IDC_EDIT_PORT)->SetWindowText(((CButton*)GetDlgItem(IDC_SECRET))->GetCheck() == BST_CHECKED ? _T("443") : _T("350"));
}


void CDlgConfig::OnBnClickedOk()
{	
	__try
	{

		Config config;
		CString strVal;

		GetDlgItem(IDC_EDIT_ACCOUNT)->GetWindowText(strVal);
		strVal = strVal.Trim();
		if (strVal.IsEmpty())
		{
			MessageBox(_T("请输入Eterm配置帐号"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			GetDlgItem(IDC_EDIT_ACCOUNT)->SetFocus();
			return;
		}
		config.UserName = strVal;

		GetDlgItem(IDC_EDIT_PASSWORD)->GetWindowText(strVal);
		if (strVal.IsEmpty())
		{
			MessageBox(_T("请输入Eterm配置密码"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			GetDlgItem(IDC_EDIT_PASSWORD)->SetFocus();
			return;
		}
		config.PassWord = strVal;

		config.AutoSI = ((CButton*)GetDlgItem(IDC_AUTO_SI))->GetCheck();

		GetDlgItem(IDC_EDIT_SI)->GetWindowText(strVal);
		if (config.AutoSI && strVal.GetLength() < 2)
		{
			MessageBox(_T("请输入SI密码"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			GetDlgItem(IDC_EDIT_SI)->SetFocus();
			return;
		}
		config.SI = strVal.Left(2).CompareNoCase(_T("SI")) == 0 ? strVal : _T("SI:") + strVal;

		GetDlgItem(IDC_EDIT_INTERVAL)->GetWindowText(strVal);
		config.Interval = _ttoi(strVal);

		m_cmbServer.GetWindowText(strVal);
		if (strVal.IsEmpty())
		{
			MessageBox(_T("请选择或输入Eterm服务器地址"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			m_cmbServer.SetFocus();
			return;
		}
		config.ServerIP = strVal;

		GetDlgItem(IDC_EDIT_PORT)->GetWindowText(strVal);
		if (strVal.IsEmpty())
		{
			MessageBox(_T("请输入Eterm服务器端口"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			GetDlgItem(IDC_EDIT_PORT)->SetFocus();
			return;
		}
		config.Port = _ttoi(strVal);

		config.IsSSL = ((CButton*)GetDlgItem(IDC_SECRET))->GetCheck();
		config.KeepAlive = ((CButton*)GetDlgItem(IDC_KEEP_ALIVE))->GetCheck();
		GetDlgItem(IDC_EDIT_MAX_COUNT)->GetWindowText(strVal);
		if (strVal.IsEmpty())
		{
			MessageBox(_T("请输入Eterm配置最大流量"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			GetDlgItem(IDC_EDIT_MAX_COUNT)->SetFocus();
			return;
		}
		config.MaxCount = _ttoi(strVal);

		GetDlgItem(IDC_EDIT_CURRENT_COUNT)->GetWindowText(strVal);
		if (strVal.IsEmpty())
		{
			MessageBox(_T("请输入Eterm配置当前流量"), _T("Eterm配置"), MB_OK | MB_ICONEXCLAMATION);
			GetDlgItem(IDC_EDIT_CURRENT_COUNT)->SetFocus();
			return;
		}
		config.Count = _ttoi(strVal);
		CConfigData cfd;

		bool bOpert = false;

		if (m_bEdit)
		{
			if (cfd.UpdateConfig(config))
			{
				bOpert = true;
			}
			else
			{
				MessageBox(_T("更新Eterm配置失败"), _T("Eterm配置"), MB_OK | MB_ICONINFORMATION);
				return;
			}
		}
		else
		{
			if (cfd.AppendConfig(config))
			{
				m_config = new Config;
				bOpert = true;
			}
			else
			{
				MessageBox(_T("更新Eterm配置失败"), _T("Eterm配置"), MB_OK | MB_ICONINFORMATION);
				return;
			}
		}

		if (bOpert)
		{
			m_config->UserName = config.UserName;
			m_config->PassWord = config.PassWord;
			m_config->ServerIP = config.ServerIP;
			m_config->SI = config.SI;
			m_config->Type = config.Type;
			m_config->AutoSI = config.AutoSI;
			m_config->Count = config.Count;
			m_config->Interval = config.Interval;
			m_config->MaxCount = config.MaxCount;
			m_config->Port = config.Port;
			m_config->IsSSL = config.IsSSL;
			m_config->KeepAlive = config.KeepAlive;

			CMainFrame *pMain = (CMainFrame *)AfxGetApp()->m_pMainWnd;
			if (pMain)
			{
				LVFINDINFO lvf;
				lvf.flags = LVFI_STRING;
				lvf.psz = m_config->UserName;
				int nIndex = pMain->m_wndConfigureListBar.m_ConfigureList.FindItem(&lvf);
				if (nIndex == -1)
				{
					nIndex = pMain->m_wndConfigureListBar.m_ConfigureList.GetItemCount();
					pMain->m_wndConfigureListBar.m_ConfigureList.InsertItem(nIndex, m_config->UserName, 0);

				}

				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 1, m_config->Type);
				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 2, m_config->ServerIP);
				strVal.Format(_T("%d"), m_config->Port);
				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 3, strVal);
				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 4, m_config->IsSSL ? _T("true") : _T("false"));
				strVal.Format(_T("%d"), m_config->Interval);
				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 8, strVal);
				strVal.Format(_T("%d"), m_config->MaxCount - m_config->Count);
				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 10, strVal);
				pMain->m_wndConfigureListBar.m_ConfigureList.SetItemData(nIndex, (DWORD_PTR)m_config);
				auto it = Global::configlist.find(m_config->UserName);
				if (it != Global::configlist.end())
				{
					it->second->m_config = *m_config;
					if (it->second->m_nState == AVAILABLE)
					{
						CView* pView = Global::GetView(m_config->UserName);
						if (pView) ::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));
					}
				}
				else
				{
					CEtermSocket* pSocket = new CEtermSocket;
					pSocket->m_config = *m_config;
					Global::configlist.insert(make_pair(m_config->UserName, unique_ptr<CEtermSocket>(pSocket)));
				}

			}
			if (m_bEdit)
			{
				CDialogEx::OnOK();
			}
			else
			{
				PostMessage(WM_COMMAND, ID_FILE_NEW);
			}
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		
	}
}
