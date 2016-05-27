#pragma once
class PingYingHelper
{
public:
	PingYingHelper();
	~PingYingHelper();
	vector<BYTE> GetPingYin(string str);
private:
	void UsasToGb(BYTE& c1, BYTE& c2);
	int eTerm_00611928(BYTE c1, BYTE c2);
	int eTerm_00611949(BYTE c1, BYTE c2);
};

