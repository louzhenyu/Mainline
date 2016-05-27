#pragma once
class CLockEterm
{
public:
	CLockEterm();
	~CLockEterm();
	long lockcount;
	LARGE_INTEGER locktime;
	wstring SessionId;
};

