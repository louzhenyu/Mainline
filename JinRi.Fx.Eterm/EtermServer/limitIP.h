#pragma once
#include "lip.h"
#include "CppSQLite3U.h"

class limitIP
{
public:
	limitIP();
	~limitIP();
	bool AppendIP(CString szIP, CString szRemark);
	bool DeleteIP(CString szIP);
	bool UpdateIP(CString szIP, CString szRemark);
	vector<lip> GetIPList();
private:
	CppSQLite3DB m_pDB;
};

