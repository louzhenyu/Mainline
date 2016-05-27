#pragma once
#include "tcpserver.h"
#include "../HttpParse.h"
#include <sstream>

struct BLOCK
{
	BLOCK(){ nlen = 0; proto = GET; }
	int nlen;
	Protocol proto;
	string content;
	string data;
	CTime  time;
};

class CWebServer :public CTcpServer
{
public:
	CWebServer(void);
	~CWebServer(void);
	void SendMsg(SOCKET sock,CString strResponse);
	void SendMsg(SOCKET sock, string strResponse);
protected:
	void SaveReceivedData(SOCKET sock,char* pVoid,long dataLen);
	void ClientConnect(SOCKET sock,char* ip,UINT nPort);
private:
	CRITICAL_SECTION m_struCriSec;
	map<SOCKET,BLOCK> m_mapWebSocketBuffer;
	void BusinessProcess(string content,string buffer,SOCKET sock);
	CString HttpString(CString strResponse);
	//0 1 2
	int ParseCmd(string& cmd);
	bool m_bValidIP;
	char m_szAddress[20];
	stringstream m_ss;	
};
