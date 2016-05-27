#include "StdAfx.h"
#include "Http.h"

CHttp::CHttp(void)
{
	WSADATA wsData;
	WSAStartup(MAKEWORD(2, 2), &wsData);
}


CHttp::~CHttp(void)
{
	WSACleanup();
}

bool CHttp::HttpRequest(char* pServer,char* szHeader,string& sret,int nPort)
{	
	this->Socket();
	this->SetTimeout(10000,0);
	this->SetTimeout(10000,1);
	this->Connect(pServer,nPort);
	this->SendRequest(szHeader,strlen(szHeader));	
		
	int nLineSize = 0;
	char szLine[1024]={0};
	
	while(nLineSize != -1)
	{
		nLineSize = this->GetResponseLine(szLine,sizeof(szLine)-1);
		if(nLineSize > -1)
		{
			szLine[nLineSize] = '\0';			
		}
	}
	char szValue[30];
	this->GetField("Content-Length",szValue,sizeof(szValue)-1);
	int nSvrState = this->GetServerState();
	int nFileSize = atoi(szValue);
	if (nSvrState!=200) return false;
	if (nFileSize==0) return false;

	int nCompletedSize = 0;
	
	char pData[40960];	
	
	sret="";

	int nReceSize = 0;
	
	while(nCompletedSize < nFileSize)
	{
		nReceSize = this->Receive(pData,sizeof(pData)-1);
		if(nReceSize == 0)
		{
			printf("服务器已经关闭连接\n");
			return false;
		}
		if(nReceSize == -1)
		{
			printf("接收数据超时\n");
			return false;
		}
		
		pData[nReceSize]='\0';
		sret.append(pData);

		nCompletedSize += nReceSize;
	}
	
	return true;
}