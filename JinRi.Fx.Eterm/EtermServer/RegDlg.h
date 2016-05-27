#pragma once


// CRegDlg 对话框

class CRegDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CRegDlg)

public:
	CRegDlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CRegDlg();

// 对话框数据
	enum { IDD = IDD_DIALOG_REG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedCancel();
	virtual void OnCancel();
};
