#pragma once
#include "afxwin.h"
#include "EtermConfigData.h"
#include "xmlHelper.h"
// CDlgConfigManage 对话框

class CDlgConfigManage : public CDialogEx
{
	DECLARE_DYNAMIC(CDlgConfigManage)

public:
	CDlgConfigManage(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CDlgConfigManage();

// 对话框数据
	enum { IDD = IDD_DLG_UTILITY };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
	void InitControl();
	void GetXmlData();
	void InitUntility(CString types=_T(""));
	void InitAircom(CString strAllow = _T(""), CString strNoAllow = _T(""));
	CComboBox m_cmbEtermLevel;
	EtermConfig* config;
	afx_msg void OnBnClickedButtonOperator();
};
