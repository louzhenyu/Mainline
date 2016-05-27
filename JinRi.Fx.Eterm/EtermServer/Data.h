#pragma once

//返回格式
enum RespnseFormat
{
	//文本
	text,
	//json格式
	json
};

//请求语言
enum EtermLanguage
{
	//脚本
	Script,
	//c#语言
	CSharp,
	//c++
	CPP
};

class CData
{
public:
	CData();
	~CData();
	wstring ip;
	SOCKET sock;
	RespnseFormat format;
	EtermLanguage language;
	wstring config;
	wstring content;
	wstring source;
};

