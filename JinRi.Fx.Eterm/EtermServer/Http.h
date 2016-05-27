#pragma once
#include "HttpSocket.h"

class CHttp:
	public HttpSocket
{
public:
	CHttp(void);
	~CHttp(void);
	bool HttpRequest(char* pServer,char* szHeader,string& sret,int nPort=80);
};

