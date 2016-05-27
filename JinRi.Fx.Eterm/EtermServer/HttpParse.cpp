#include "stdafx.h"
#include "HttpParse.h"

CHttpParse::CHttpParse()
{
	
}

CHttpParse::~CHttpParse()
{
	/*if (szContent)
	{
		delete[] szContent;
		szContent = NULL;
	}
	if (szPostData)
	{
		delete[] szPostData;
		szPostData = NULL;
	}*/
}

bool CHttpParse::httpParse(char* pVoid, long len)
{
	char* szContent=new char[len];

	if (strstr(pVoid, "GET /") != NULL)
	{
		proto = GET;
		sscanf_s(pVoid, "GET /%s HTTP/1.1", szContent, len);
		if (strcmp(szContent, "HTTP/1.1") == 0) szContent[0] = 0;
		Content = string(szContent);
		delete[] szContent;
		return true;
	}
	else if (strstr(pVoid, "POST /") != NULL)
	{
		proto = POST;
		sscanf_s(pVoid, "POST /%s HTTP/1.1", szContent, len);
		if (strcmp(szContent, "HTTP/1.1") == 0) szContent[0] = 0;
		pVoid = strstr(pVoid, "Content-Length:");
		sscanf_s(pVoid, "Content-Length: %d", &nLen);		
		pVoid = strstr(pVoid, "\r\n\r\n");

		Content = string(szContent);
		delete[] szContent;

		if (pVoid)
		{
			PostData=string(&pVoid[4]);
			if (PostData.length() == nLen)
			{
				return true;
			}
		}
	}
	return false;
}