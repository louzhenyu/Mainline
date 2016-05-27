// DlgConfigManage.cpp : 实现文件
//

#include "stdafx.h"
#include "EtermServer.h"
#include "DlgConfigManage.h"
#include "afxdialogex.h"
#include "Global.h"
#include "stringEx.h"
#include "RedisHelper.h"
#include <algorithm>
#include "xmlHelper.h"

// CDlgConfigManage 对话框

IMPLEMENT_DYNAMIC(CDlgConfigManage, CDialogEx)

CDlgConfigManage::CDlgConfigManage(CWnd* pParent /*=NULL*/)
	: CDialogEx(CDlgConfigManage::IDD, pParent)
{
	config = NULL;
}

CDlgConfigManage::~CDlgConfigManage()
{
	if (config) delete config;
}

void CDlgConfigManage::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO_LEVEL, m_cmbEtermLevel);
}


BEGIN_MESSAGE_MAP(CDlgConfigManage, CDialogEx)
	ON_BN_CLICKED(IDC_BUTTON_OPERATOR, &CDlgConfigManage::OnBnClickedButtonOperator)
END_MESSAGE_MAP()


// CDlgConfigManage 消息处理程序


BOOL CDlgConfigManage::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  在此添加额外的初始化
	InitControl();

	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常:  OCX 属性页应返回 FALSE
}

void CDlgConfigManage::InitControl()
{
	TCHAR buf[255] = { 0 };

	if (Global::ServerUrl.IsEmpty())
	{
		CString strPath;
		strPath.Format(_T("%s\\Config.ini"), Global::szAppPath);

		int nPort = ::GetPrivateProfileInt(_T("SYSTEM"), _T("PORT"), 5252, strPath);
		::GetPrivateProfileString(_T("SYSTEM"), _T("IP"), _T(""), buf, sizeof(buf), strPath);

		Global::ServerUrl.Format(_T("%s:%d"), buf, nPort);
	}

	EtermConfigData ecd;
	config = ecd.GetConfig(Global::ServerUrl);
	int npos = Global::ServerUrl.ReverseFind(':');
	if (npos == -1) return;
	GetDlgItem(IDC_IPADDRESS)->SetWindowText(Global::ServerUrl.Left(npos));
	GetDlgItem(IDC_IPADDRESS)->EnableWindow(FALSE);
	GetDlgItem(IDC_EDIT_PORT)->SetWindowText(Global::ServerUrl.Right(Global::ServerUrl.GetLength()-npos-1));
	m_cmbEtermLevel.SetCurSel(0);
	
	if (config)
	{
		((CButton*)GetDlgItem(IDC_CHECK_ON))->SetCheck(config->State == 0);
		GetDlgItem(IDC_EDIT_OFFICE)->SetWindowText(config->OfficeNo);
		_stprintf_s(buf, _T("%d级"), config->ConfigLevel);
		int nPos = m_cmbEtermLevel.FindString(0, buf);
		if (nPos >= 0) m_cmbEtermLevel.SetCurSel(nPos);		
		InitUntility(config->Types);
		InitAircom(config->AllowAirLine,config->DenyAirLine);
		GetDlgItem(IDC_BUTTON_OPERATOR)->SetWindowText(_T("修改(&M)"));
	}
	else
	{
		InitUntility();
		InitAircom();
		GetDlgItem(IDC_BUTTON_OPERATOR)->SetWindowText(_T("添加(&A)"));
	}
}

void CDlgConfigManage::GetXmlData()
{	
	TCHAR szFilePath[MAX_PATH] = { 0 };
	_stprintf_s(szFilePath, _T("%s\\JetermEntity.XML"),Global::szAppPath);

	xmlHelper xml;
	if (xml.LoadFile(CT2A(szFilePath)))
	{
		xml.getnodes("/doc/members/member[contains(@name,'F:JetermEntity.EtermCommand.CmdType')]/summary");
	}
	/*TiXmlDocument *pEtermDoc = NULL;
	pEtermDoc = new TiXmlDocument();
	if (pEtermDoc->LoadFile("E:\\Program\\Fx\\JinRi.Fx.Eterm\\Debug\\EtermProxy.XML"))
	{
		TiXmlElement* rootElement = pEtermDoc->RootElement();
		if (rootElement)
		{
			TiXmlElement* childElement = rootElement->FirstChildElement("members");
			if (childElement)
			{
				TiXmlElement* member = childElement->FirstChildElement();
				while (member)
				{
					const char * name = member->Attribute("name");
					if (strstr(name, "EtermProxy.Entity.EtermCommand.CmdType") != NULL)
					{
						TiXmlElement* summary = member->FirstChildElement("summary");
						if (summary)
						{
							const char* text = summary->GetText();
							if (text)
							vec.push_back((CString)CA2W(text,CP_UTF8));
						}
					}
					member = member->NextSiblingElement();
				}
			}
		}
	}*/
}

void CDlgConfigManage::InitUntility(CString types)
{
	stringEx ex;	
	CRect rt,rect,rtwin;

	if (Global::cmdtypes.size() == 0) GetXmlData();

	GetDlgItem(IDC_UTILITY)->GetWindowRect(rt);
	ScreenToClient(&rt);
	rect = rt;
	rect.bottom = rect.top + ((int)((Global::cmdtypes.size()/2.0)+0.5) * 30);
	GetDlgItem(IDC_UTILITY)->MoveWindow(rect);
	rect.top=rect.bottom + 10;
	rect.bottom = rect.top + 85;
	GetDlgItem(IDC_ALLOW_AIRCOM)->MoveWindow(rect);
	rect.top = rect.bottom + 10;
	rect.bottom = rect.top + 85;
	GetDlgItem(IDC_NOALLOW_AIRCOM)->MoveWindow(rect);
	GetWindowRect(rtwin);
	rtwin.bottom += rect.bottom - rt.bottom - 190;
	MoveWindow(rtwin);

	rt.DeflateRect(10, 20,10,10);
	int il = 0, ir = 0;	
	CString strKey;
	

	for (int i = 0; i < Global::cmdtypes.size(); i++)
	{
		int x = i % 2 == 0 ? rt.left : (rt.left + rt.Width() / 2);
		int y = ((i / 2) * 25) + rt.top;
		CButton *pButton = new CButton();
		pButton->Create(Global::cmdtypes[i].des, WS_CHILD | WS_VISIBLE | BS_AUTOCHECKBOX, CRect(x, y, x + 250, y + 22), this, ID_UTILITY + i);
		pButton->SetFont(this->GetFont());
		strKey.Format(_T("%d,"), Global::cmdtypes[i].index);
		pButton->SetCheck(types.Find(strKey) != -1);
		pButton->ShowWindow(SW_SHOW);
	}
}

void CDlgConfigManage::InitAircom(CString strAllow,CString strNoAllow)
{
	CString strBuf;
	CString strPath;
	strPath.Format(_T("%s\\aircom.ini"), Global::szAppPath);
	 
	CRect rt,rtn;
	GetDlgItem(IDC_ALLOW_AIRCOM)->GetWindowRect(rt);
	GetDlgItem(IDC_NOALLOW_AIRCOM)->GetWindowRect(rtn);
	ScreenToClient(&rt);
	rt.DeflateRect(10, 15, 10, 10);
	ScreenToClient(&rtn);
	rtn.DeflateRect(10, 15, 10, 10);
	unsigned char buf[1024] = { 0 };
	DWORD dw = ::GetPrivateProfileSectionA("Aircom", (LPSTR)buf, sizeof(buf), CW2A(strPath));
	char szbuf[256] = { 0 };
	int count = 0;
	for (int i = 0;i<dw;)
	{
		memset(szbuf, 0, sizeof(szbuf));
		memcpy(szbuf, &buf[i], strlen((const char*)&buf[i]));
		if (szbuf)
		{
			int x = count % 15 == 0 ? rt.left : (rt.left + ((count%15)*(rt.Width() / 15)));
			int y = ((count / 15) * 22) + rt.top;

			char szName[3] = { 0 };
			strncpy(szName, szbuf, 2);

			CButton *pButton = new CButton();
			pButton->Create(CA2W(szName), WS_CHILD | WS_VISIBLE | BS_AUTOCHECKBOX, CRect(x, y, x + 35, y + 22), this, ID_ALLOW_AIRCOM + count);
			pButton->SetFont(this->GetFont());			
			pButton->SetCheck(strAllow.IsEmpty() || strAllow == _T("*") || strAllow.Find(CA2W(szName)) != -1);
			pButton->ShowWindow(SW_SHOW);

			y = ((count / 15) * 22) + rtn.top;
			CButton *pButtonNo = new CButton();
			pButtonNo->Create(CA2W(szName), WS_CHILD | WS_VISIBLE | BS_AUTOCHECKBOX, CRect(x, y, x + 35, y + 22), this, ID_NOALLOW_AIRCOM + count);
			pButtonNo->SetFont(this->GetFont());
			pButtonNo->SetCheck(strNoAllow==_T("*") || strNoAllow.Find(CA2W(szName)) != -1);
			pButtonNo->ShowWindow(SW_SHOW);

			i += strlen(szbuf)+1;
			count++;
		}
	}
	
}

void CDlgConfigManage::OnBnClickedButtonOperator()
{
	if (!config)
	{
		config = new EtermConfig;
	}

	EtermConfigData ecd;

	CString strVal,strTemp;

	GetDlgItem(IDC_IPADDRESS)->GetWindowText(strVal);
	config->ServerUrl = strVal;
	if (config->ServerUrl.IsEmpty())
	{
		MessageBox(_T("请输入服务器IP地址"), _T("Eterm服务端"), MB_OK | MB_ICONEXCLAMATION);
		GetDlgItem(IDC_IPADDRESS)->SetFocus();
		return;
	}

	GetDlgItem(IDC_EDIT_PORT)->GetWindowText(strVal);
	if (strVal.IsEmpty())
	{
		MessageBox(_T("请输入服务器端口"), _T("Eterm服务端"), MB_OK | MB_ICONEXCLAMATION);
		GetDlgItem(IDC_IPADDRESS)->SetFocus();
		return;
	}
	config->ServerUrl.AppendFormat(_T(":%s"), strVal);
	config->State = ((CButton*)GetDlgItem(IDC_CHECK_ON))->GetCheck() == 1 ? 0 : 1;
	GetDlgItem(IDC_EDIT_OFFICE)->GetWindowText(strVal);
	config->OfficeNo = strVal;
	m_cmbEtermLevel.GetLBText(m_cmbEtermLevel.GetCurSel(), strVal);
	strVal.Replace(_T("级"), _T(""));
	config->ConfigLevel = _ttoi(strVal);
	strVal.Empty();
	for (int i = ID_UTILITY; i < ID_UTILITY + 100; i++)
	{
		CButton* pButton = (CButton*)GetDlgItem(i);
		if (!pButton) break;
		if (pButton->GetCheck())
		{
			pButton->GetWindowText(strTemp);
			enums type;
			type.des = strTemp;
			auto it = find_if(Global::cmdtypes.begin(), Global::cmdtypes.end(), [=](enums _type){ return _type.des == type.des; });
			if (it != Global::cmdtypes.end())
				strVal.AppendFormat(_T("%d,"), it->index);
		}
	}
	config->Types = strVal;
	strVal.Empty();
	for (int i = ID_ALLOW_AIRCOM; i < ID_ALLOW_AIRCOM + 50; i++)
	{
		CButton* pButton = (CButton*)GetDlgItem(i);
		if (!pButton) break;
		if (pButton->GetCheck())
		{
			pButton->GetWindowText(strTemp);
			strVal.AppendFormat(_T("%s,"), strTemp.Left(2));
		}
	}
	config->AllowAirLine = strVal;
	strVal.Empty();
	for (int i = ID_NOALLOW_AIRCOM; i < ID_NOALLOW_AIRCOM + 50; i++)
	{
		CButton* pButton = (CButton*)GetDlgItem(i);
		if (!pButton) break;
		if (pButton->GetCheck())
		{
			pButton->GetWindowText(strTemp);
			strVal.AppendFormat(_T("%s,"), strTemp.Left(2));
		}
	}
	config->DenyAirLine = strVal;
	
	config->ConfigList.Empty();
	for (auto cit = Global::configlist.begin(); cit != Global::configlist.end(); cit++)
		config->ConfigList.AppendFormat(_T("%s,"), cit->first);

	bool bret = false;
	if (!(bret = ecd.UpdateConfig(*config))) bret = ecd.AppendConfig(*config);
	GetDlgItem(IDC_BUTTON_OPERATOR)->GetWindowText(strVal);
	if (bret)
	{
		EtermServer::RedisHelper rh;
		string sret = rh.get("140106_140110_EtermUrl");
		map<CString, EtermConfig> ecs = ecd.GetEtermConfig(sret);
		auto it = ecs.find(config->ServerUrl);
		if (it != ecs.end())
		{
			if (config->State == 0)
				it->second = *config;
			else
				ecs.erase(config->ServerUrl);
		}
		else
		{
			if (config->State == 0)
				ecs.insert(make_pair(config->ServerUrl, *config));
		}
		sret = ecd.toString(ecs);
		rh.set("140106_140110_EtermUrl", sret);

		MessageBox(strVal.Left(2) + _T("配置成功！"), _T("Eterm服务端"), MB_OK | MB_ICONINFORMATION);
		OnOK();
	}
	else
	{
		MessageBox(strVal.Left(2) + _T("配置失败！"), _T("Eterm服务端"), MB_OK | MB_ICONINFORMATION);
	}
		
}
