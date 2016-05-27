#pragma once
#include <map>

class EtermConfig
{
public:	
	EtermConfig();
	~EtermConfig();
	CString ServerUrl;
	CString OfficeNo;
	CString Types;
	int State;
	CString AllowAirLine;
	CString DenyAirLine;
	int ConfigLevel;
	CString Remark;
	CString ConfigList;
};

