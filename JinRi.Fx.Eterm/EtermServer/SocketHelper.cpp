#include "StdAfx.h"
#include "SocketHelper.h"
#include <winsock.h>
#include "stringEx.h"
#pragma comment ( lib, "Iphlpapi.lib" )

CSocketHelper::CSocketHelper(void)
{
}


CSocketHelper::~CSocketHelper(void)
{
}

char* CSocketHelper::GetMAC()
{
	char szMac[255] = { 0 };

	PIP_ADAPTER_INFO pAdapterInfo;
	PIP_ADAPTER_INFO pAdapter = NULL;
	DWORD dwRetVal = 0;

	pAdapterInfo = (IP_ADAPTER_INFO *)malloc(sizeof(IP_ADAPTER_INFO));
	ULONG ulOutBufLen = sizeof(IP_ADAPTER_INFO);

	if (GetAdaptersInfo(pAdapterInfo, &ulOutBufLen) != ERROR_SUCCESS)
	{
		GlobalFree(pAdapterInfo);
		pAdapterInfo = (IP_ADAPTER_INFO *)malloc(ulOutBufLen);
	}

	if ((dwRetVal = GetAdaptersInfo(pAdapterInfo, &ulOutBufLen)) == NO_ERROR)
	{
		pAdapter = pAdapterInfo;
		while (pAdapter)
		{
			if (
				strstr(pAdapter->Description, "PCI") > 0 // pAdapter->Description中包含"PCI"为：物理网卡
				|| pAdapter->Type == 71                   // pAdapter->Type是71为：无线网卡
				)
			{
				printf("------------------------------------------------------------\n");
				printf("Adapter Name: \t%s\n", pAdapter->AdapterName);
				printf("Adapter Desc: \t%s\n", pAdapter->Description);
				printf("Adapter Addr: \t");
				for (UINT i = 0; i < pAdapter->AddressLength; i++)
				{
					sprintf(szMac,"%s%02x",szMac, pAdapter->Address[i]);
				}
				printf("Adapter Type: \t%d\n", pAdapter->Type);
				printf("IP Address: \t%s\n", pAdapter->IpAddressList.IpAddress.String);
				printf("IP Mask: \t%s\n", pAdapter->IpAddressList.IpMask.String);
			}
			pAdapter = pAdapter->Next;
		}
	}
	else
	{
		printf("Call to GetAdaptersInfo failed.\n");
	}
	return szMac;
}

CString CSocketHelper::GetIP(CString DomailName)
{
	stringEx ex;
	if (!ex.match((wstring)DomailName,_T("^\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}$")))
	{
		return DomailToIP(CT2A(DomailName));
	}
	else
	{
		return DomailName;
	}
}

char* CSocketHelper::GetHostIP()
{
	char szIP[20] = { 0 };
	char Name[255];//定义用于存放获得的主机名的变量 

	PHOSTENT hostinfo;
	if (gethostname(Name, sizeof(Name)) == 0)
	{
		//如果成功地将本地主机名存放入由name参数指定的缓冲区中 
		if ((hostinfo = gethostbyname(Name)) != NULL)
		{
			//这是获取主机名，如果获得主机名成功的话，将返回一个指针，指向hostinfo，hostinfo 
			//为PHOSTENT型的变量，下面即将用到这个结构体
			sprintf(szIP, "%s%s", szIP, inet_ntoa(*(struct in_addr *)*hostinfo->h_addr_list));
		}
	}

	return szIP;
}


CString CSocketHelper::DomailToIP(const char* name)
{
	WSADATA     wsaData; 
	
	CString strIP;
	USES_CONVERSION; 

    HOSTENT   *pHostEnt; 
    int             nAdapter   =   0; 
    struct       sockaddr_in   sAddr; 

    if   (WSAStartup(0x0101,   &wsaData)) 
    { 
          printf( "WSAStartup   failed   %s\n ",   WSAGetLastError()); 
		  return strIP;
    } 

    pHostEnt   =   gethostbyname(name); 

	try
	{
		if (pHostEnt!=NULL)
		{
			while   (   pHostEnt-> h_addr_list[nAdapter]   ) 
			{ 
			  //   pHostEnt-> h_addr_list[nAdapter]   is   the   current   address   in   host 
			  //   order. 

			  //   Copy   the   address   information   from   the   pHostEnt   to   a   sockaddr_in 
			  //   structure. 
				  memcpy   (   &sAddr.sin_addr.s_addr,   pHostEnt-> h_addr_list[nAdapter], 
									pHostEnt-> h_length); 

				  //   Output   the   machines   IP   Address. 
				  //sprintf(szHostname, "%s",   pHostEnt-> h_name, inet_ntoa(sAddr.sin_addr)); 
				  strIP.AppendFormat(_T("%s"),A2T(inet_ntoa(sAddr.sin_addr)));

				  nAdapter++; 
			} 
		}
		WSACleanup();	
	}
	catch(...)
	{
		WSACleanup();
	}

	return strIP;
}