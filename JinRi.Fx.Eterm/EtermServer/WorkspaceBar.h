#pragma once
#include "ConfigData.h"

// CWorkspaceBar

class CWorkspaceBar : public CDockablePane
{
	DECLARE_DYNAMIC(CWorkspaceBar)

public:
	CWorkspaceBar();
	virtual ~CWorkspaceBar();
	CMFCListCtrl m_ConfigureList;
	CImageList m_ImageList;
	CConfigData  m_cfgdata;
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint point);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	afx_msg void OnConfigAdd();
	afx_msg void OnConfigModify();
	afx_msg void OnConfigDelete();
	afx_msg void OnConnectServer();
	afx_msg void OnDisconnectServer();
	afx_msg void OnConnectAll();
	afx_msg void OnDisconnectAll();
	afx_msg LRESULT ShowConfig(WPARAM wParam, LPARAM lParam);
};


