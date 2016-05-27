#pragma once


// CDlgReckon 对话框

class CDlgReckon : public CDialogEx
{
	DECLARE_DYNAMIC(CDlgReckon)

public:
	CDlgReckon(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CDlgReckon();

// 对话框数据
	enum { IDD = IDD_DLG_RECKON };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	virtual BOOL OnInitDialog();
};
