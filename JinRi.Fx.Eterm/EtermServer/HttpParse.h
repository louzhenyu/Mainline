#pragma once

enum Protocol
{
	GET,
	POST
};

class CHttpParse
{
public:
	CHttpParse();
	~CHttpParse();
	Protocol proto;				//协议	
	string Content;			//http内容
	int   nLen;					//post数据长度
	string PostData;			//post数据	
	bool httpParse(char* pVoid,long len);
};

