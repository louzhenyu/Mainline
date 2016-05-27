#pragma once
#include "EtermConfig.h"
#include "BookData.h"

class EtermConfigData
{
public:
	EtermConfigData();
	~EtermConfigData();
	EtermConfig* GetConfig(CString ServerUrl);
	bool AppendConfig(EtermConfig config);
	bool UpdateConfig(EtermConfig config);
	bool UpdateConfig(CString ServerUrl,int State);
	bool AppendBooking(BookData book);
	map<CString, EtermConfig> GetEtermConfig(std::string str);
	std::string toString(map<CString, EtermConfig> ecs);
};

