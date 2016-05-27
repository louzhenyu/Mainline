#pragma once
class Proxy
{
public:
	Proxy();
	~Proxy();

	//代理类型 0.不使用代理 1.SOCKS4 2.SOCKS4A 3.SOCKS5 4.HTTP11
	int type;
	wstring host;
	unsigned int port;
	wstring user;
	wstring pwd;
};

