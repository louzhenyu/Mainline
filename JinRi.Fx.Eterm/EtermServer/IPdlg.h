#pragma once
#include "afxcmn.h"


// CIPdlg 对话框

class CIPdlg : public CDialogEx
{
	DECLARE_DYNAMIC(CIPdlg)

public:
	CIPdlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CIPdlg();

// 对话框数据
	enum { IDD = IDD_DLG_IP };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButtonAdd();
	afx_msg void OnBnClickedButtonDelete();
	afx_msg void OnBnClickedButtonModify();
	CListCtrl m_list;
	virtual BOOL OnInitDialog();
	afx_msg void OnLvnItemchangedList(NMHDR *pNMHDR, LRESULT *pResult);
};
