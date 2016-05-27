#pragma once
#include "afxwin.h"
#include "Config.h"

// CDlgConfig 对话框

class CDlgConfig : public CDialogEx
{
	DECLARE_DYNAMIC(CDlgConfig)

public:
	CDlgConfig(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CDlgConfig();

// 对话框数据
	enum { IDD = IDD_DLG_CONFIG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	bool m_bEdit;
	void InitControl();
	virtual BOOL OnInitDialog();
	CComboBox m_cmbServer;
	CComboBox m_cmbType;
	Config* m_config;
	afx_msg void OnBnClickedSecret();
	afx_msg void OnBnClickedOk();
};
