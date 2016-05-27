#pragma once
#include "Config.h"
#include "CppSQLite3U.h"

class CConfigData
{
public:
	CConfigData();
	~CConfigData();
	
	vector<CString> LoadServer(void);
	vector<Config*> LoadConfig(void);
	
	bool AppendConfig(Config cfg);
	bool UpdateConfig(Config cfg);
	bool DeleteConfig(CString szConfig);
	bool UpdateUseCount(CString szConfig, int count);
	bool CreateConfig();
private:
	CppSQLite3DB m_pDB;
};

