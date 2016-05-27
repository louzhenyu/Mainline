#pragma once
class Config
{
public:
	Config();
	~Config();
	bool			IsSSL;
	bool			AutoSI;
	bool			bPause;
	bool            KeepAlive;
	unsigned int	Port;
	unsigned int	Interval;
	int				MaxCount;
	int				Count;
	CString			Type;
	CString			UserName;
	CString			PassWord;
	CString			ServerIP;
	CString			SI;
};

