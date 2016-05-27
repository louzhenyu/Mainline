#pragma once
class ShowInfo
{
public:
	ShowInfo();
	~ShowInfo();
	CString strConfig;
	CString strInfo;
	int     count;
	static void SendShowInfo(int nType,CString strConfig,CString strInfo=_T(""),int count=0);
};

