#pragma once

class CLog
{
public:
	CLog();
	CLog(string _strLog, string _strConfig="log");
	CLog(CString _strLog, CString _strConfig = _T("log"));
	~CLog();
	string strLog;
	string strConfig;
	SYSTEMTIME logTime;
};

