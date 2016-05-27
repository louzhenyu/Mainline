#pragma once
class SMS
{
public:
	SMS();
	~SMS();

	unsigned int  nPort;
	char szObj[1024];
	char szSMSserver[32];
	vector<string> vecMobil;
};

