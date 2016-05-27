#include "stdafx.h"
#include "log.h"


CLog::CLog()
{
	strConfig = "log";
	GetLocalTime(&logTime);
}


CLog::~CLog()
{
}


CLog::CLog(string _strLog, string _strConfig)
{
	strLog = _strLog;
	strConfig = _strConfig.empty()?"log":_strConfig;
	GetLocalTime(&logTime);
}

CLog::CLog(CString _strLog, CString _strConfig)
{
	strLog = string(CT2A(_strLog));
	strConfig = _strConfig.IsEmpty() ? "log" : string(CT2A(_strConfig));
	GetLocalTime(&logTime);
}