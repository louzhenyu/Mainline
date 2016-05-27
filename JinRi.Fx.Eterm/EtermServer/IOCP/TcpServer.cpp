#include "stdafx.h"
#include ".\tcpserver.h"

#include <stdio.h>
#include <process.h>
#include <MMSystem.h>
#pragma comment(lib,"Winmm.lib") //timeSetEvent()

#include <time.h>

CTcpServer::CTcpServer(void)
{
	m_hListenSocket = NULL;
	ZeroMemory(&m_struServerAdd,sizeof(m_struServerAdd)); //服务器地址
	m_hCompletionPort = NULL;
	m_hListenThread = NULL;

	for(int i=0;i<MAX_PROCESSOR_COUNTER;i++)
		m_hWorkerThread[i] = NULL;

	m_iMaxPostAcceptNumbs = 64;//一次最多投递64个accept
	m_hPostAcceptEvent[0] = CreateEvent(NULL,FALSE,FALSE,NULL);
	m_hPostAcceptEvent[1] = ::CreateSemaphore(NULL,0,m_iMaxPostAcceptNumbs,NULL);

	InitializeCriticalSection(&m_struCriSec);
	
	m_iWorkerThreadNumbers = 2;//最佳线程数量是CPU数量的2倍
	m_bServerRunning = false;

	m_pfAcceptEx = NULL;				//AcceptEx函数地址
	m_pfGetAddrs = NULL;	//GetAcceptExSockaddrs函数的地址

	m_mapAcceptSockQueue.clear();

	//初始化winsocket2库
	WSADATA wsData;
	int errorCode = WSAStartup(MAKEWORD(2, 2), &wsData);//初始化ws2_32.lib库
}

CTcpServer::~CTcpServer(void)
{
	m_hListenSocket = NULL;
	ZeroMemory(&m_struServerAdd,sizeof(m_struServerAdd)); //服务器地址
	m_hCompletionPort = NULL;
	m_hListenThread = NULL;

	for(int i=0;i<MAX_PROCESSOR_COUNTER;i++)
		m_hWorkerThread[i] = NULL;

	for(int i=0;i<ACCEPT_EVENT_CNT;i++)
	    m_hPostAcceptEvent[i] = NULL;

	m_iWorkerThreadNumbers = 2;//最佳线程数量是CPU数量的2倍
	m_bServerRunning = false;

    m_pfAcceptEx = NULL;				//AcceptEx函数地址
	m_pfGetAddrs = NULL;	//GetAcceptExSockaddrs函数的地址

	DeleteCriticalSection(&m_struCriSec);

	WSACleanup();
}
int CTcpServer::InitServer(char* ipAddress,unsigned short port)
{
	try
	{
		int errorCode = 1;
		if(!ipAddress)
		{
			throw CSrvException("Invalid Ip address.",-1,(long)(__LINE__));
		}
		//创建套接字
		m_hListenSocket = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, NULL, 0, WSA_FLAG_OVERLAPPED);
		if(m_hListenSocket == INVALID_SOCKET)
		{
			throw CSrvException("Invalid socket handle.",-1,(long)(__LINE__));
		}
		//把accetpex设置为异步非阻塞模式
		ULONG ul = 1;
		errorCode = ioctlsocket(m_hListenSocket, FIONBIO, &ul);
		if(SOCKET_ERROR == errorCode)
		{
			throw CSrvException("Set listen socket to FIONBIO mode error.",-1,(long)(__LINE__));
		}

		//设置为socket重用,防止服务器崩溃后端口能够尽快再次使用或共其他的进程使用
		int nOpt = 1;
		errorCode = setsockopt(m_hListenSocket, SOL_SOCKET, SO_REUSEADDR, (char*)&nOpt, sizeof(nOpt));
		if(SOCKET_ERROR == errorCode)
		{
			throw CSrvException("Set listen socket to SO_REUSEADDR mode error.",-1,(long)(__LINE__));
		}
        
		//关闭系统的接收与发送缓冲区
		int nBufferSize = 0;
		setsockopt(m_hListenSocket,SOL_SOCKET,SO_SNDBUF,(char*)&nBufferSize,sizeof(int));
		setsockopt(m_hListenSocket,SOL_SOCKET,SO_RCVBUF,(char*)&nBufferSize,sizeof(int)); 

		unsigned long dwBytes = 0;
		GUID guidAcceptEx = WSAID_ACCEPTEX;
		if (NULL == m_pfAcceptEx)
		{
			errorCode = WSAIoctl(m_hListenSocket,SIO_GET_EXTENSION_FUNCTION_POINTER, &guidAcceptEx,sizeof(guidAcceptEx)
				, &m_pfAcceptEx, sizeof(m_pfAcceptEx), &dwBytes, NULL, NULL);
		}
		if (NULL == m_pfAcceptEx || SOCKET_ERROR == errorCode)
		{
			throw CSrvException("Invalid fuction pointer.",-1,(long)(__LINE__));
		}

		//加载GetAcceptExSockaddrs函数
		GUID guidGetAddr = WSAID_GETACCEPTEXSOCKADDRS;
		if (NULL == m_pfGetAddrs)
		{
			errorCode = WSAIoctl(m_hListenSocket, SIO_GET_EXTENSION_FUNCTION_POINTER, &guidGetAddr, sizeof(guidGetAddr)
				, &m_pfGetAddrs, sizeof(m_pfGetAddrs), &dwBytes, NULL, NULL);
		}
		if (NULL == m_pfGetAddrs || SOCKET_ERROR == errorCode)
		{
			throw CSrvException("Invalid fuction pointer.",-1,(long)(__LINE__));
		}

		//填充服务器地址信息
		m_struServerAdd.sin_family = AF_INET;
		m_struServerAdd.sin_addr.s_addr = strlen(ipAddress) == 0 ? htonl(INADDR_ANY) : inet_addr(ipAddress);
		m_struServerAdd.sin_port = htons(port);

		//创建完成端口
		m_hCompletionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE,NULL,0,0);
		if(m_hCompletionPort == NULL)
		{
			throw CSrvException("Invalid completion port handle.",-1,(long)(__LINE__));
		}
		//邦定套接字和服务器地址
		errorCode = bind(m_hListenSocket,(struct sockaddr*)&m_struServerAdd,sizeof(m_struServerAdd));
		if(errorCode == SOCKET_ERROR)
		{
			throw CSrvException("Error socket bind operation.",WSAGetLastError(),(long)(__LINE__));
		}

		//创建主线程和工作线程
		if(!this->CteateThread())
		{
			throw CSrvException("Create worker thread error.",-1,(long)(__LINE__));
		}
        //把监听线程和完成端口邦定
		CreateIoCompletionPort((HANDLE)m_hListenSocket,m_hCompletionPort,SC_WAIT_ACCEPT,0);
		//当连接请求不足时，os将激发此事件
		WSAEventSelect(m_hListenSocket,m_hPostAcceptEvent[0],FD_ACCEPT);
		//设置监听等待句柄
		errorCode = listen(m_hListenSocket,m_iMaxPostAcceptNumbs);//第二个参数设置在连接队列里面的等待句柄数目.并发量大的server需要调大此值
		if(errorCode)
		{
			throw CSrvException("Error socket lister operation.",-1,(long)(__LINE__));
		}
        
		CAcceptContext::InitContextPool(m_iMaxPostAcceptNumbs);//默认链接池中有64个链接context待命
		CTcpReceiveContext::InitContextPool(m_iMaxPostAcceptNumbs);
		CTcpSendContext::InitContextPool(m_iMaxPostAcceptNumbs*10);
		PostAcceptOperation(m_iMaxPostAcceptNumbs/2);//投递链接操作到完成端口,应付高并发量
	
		//m_iTimerID = timeSetEvent(1000,0,TimerProc,(DWORD)this,TIME_PERIODIC);//1秒钟定时器
		
		return 1;
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d, errorCode: %d\n",e.GetExpDescription(),e.GetExpLine(),e.GetExpCode());
		this->ReleaseResource();//释放分配了的资源
		return e.GetExpCode();
	}	
}
int CTcpServer::StartServer(void)
{
	try
	{
		if(m_bServerRunning)
		{
			throw CSrvException("Server is running..",-1,(long)(__LINE__));
		}
		m_bServerRunning = true; //启动服务器线程
		if(m_hListenThread)
		{
			ResumeThread(m_hListenThread);
		}

		for (int i=0; i<m_iWorkerThreadNumbers; i++)
		{
			if(m_hWorkerThread[i])
			{
				SetThreadPriority(m_hWorkerThread[i],THREAD_PRIORITY_TIME_CRITICAL);
				ResumeThread(m_hWorkerThread[i]);
			}
		}
		return 1;
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return e.GetExpCode();
	}
	catch(...)
	{
		return 0;
	}
}

int CTcpServer::StopServer(void)
{
	if(!m_bServerRunning)
	{
		StartServer();
	}
    
	//设置下面的语句的目的是把socke阻塞运行修改为非阻塞模式
	unsigned long ul = 1;//要改为非阻赛模式，此值必须非零
	ioctlsocket(m_hListenSocket, FIONBIO, &ul);//设置socket 的I/O模式，为同步还是异步

	m_bServerRunning = false;//结束服务器线程
	if(m_hListenThread)
	{
		WaitForSingleObject(m_hListenThread,INFINITE);
	}
	for(int i=0;i<m_iWorkerThreadNumbers;i++)
		PostQueuedCompletionStatus(m_hCompletionPort, COMPLETION_KEY_SHUTDOWN, 0, NULL);
	for(int i=0;i<m_iWorkerThreadNumbers;i++)
	{
		if(m_hWorkerThread[i])
		{
			WaitForSingleObject(m_hWorkerThread[i],INFINITE);
		}
	}
	this->ReleaseResource();
	return 0;
}
//通过此函数向client发送文件
//fileName传入的是绝对路径
int CTcpServer::SendFile(SOCKET sock,char* fileName)//通过此函数想远端发送文件
{
	if(IsValidSocket(sock) && fileName)
	{
		HANDLE file = CreateFileA(fileName,GENERIC_READ,FILE_SHARE_READ,0,OPEN_EXISTING, 
			                     FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN,0);
		if(INVALID_HANDLE_VALUE != file)
		{
			BY_HANDLE_FILE_INFORMATION Info={0};
			GetFileInformationByHandle( file, &Info );//通过文件句柄得到文件信息

			CTcpSendContext* pContext = CTcpSendContext::GetContext();
			if(pContext)
			{
				pContext->SetSendParameters(sock,NULL,0);
				int errorCode = TransmitFile(sock, file, Info.nFileSizeLow, 0,&pContext->m_struOperateOl,NULL, 0 );
				if(errorCode || (!errorCode && WSAGetLastError() == ERROR_IO_PENDING))
				{
					return 1;
				}
				else
				{
					pContext->ReleaseContext();
				}
			}
		}
		CloseHandle(file);
	}
}
//通过此函数server向服务器发送数据
int CTcpServer::SendData(SOCKET sock,const char* pVoid,long dataSize)
{
	if(IsValidSocket(sock))
	{
		if(pVoid)
		{
			return IssueSendOperation(sock,pVoid,dataSize);
		}
	}
	return 0;
}
int CTcpServer::PushClientSocket(SOCKET clientSock)
{
	int rc = 0;
	::EnterCriticalSection(&m_struCriSec);

	rc = (int)m_mapAcceptSockQueue.size();
	if(m_mapAcceptSockQueue.insert(std::make_pair(clientSock,rc++)).second)
		rc = 1;
	else
		rc = 0;
	LeaveCriticalSection(&m_struCriSec);
	return rc;
}
//此函数主要用于关闭client发起的连接，在运行过程中出现异常，将由服务器主动关闭
//次函数需要考虑线程同步问题
int CTcpServer::CloseClientSocket(SOCKET clientSocket)
{ 
	if (!m_struCriSec.OwningThread) return 1;
	::EnterCriticalSection(&this->m_struCriSec);
	SOCKET sock = NULL;
	map<SOCKET,long>::iterator pos;
	pos = m_mapAcceptSockQueue.find(clientSocket);
	if(pos!=m_mapAcceptSockQueue.end())
	{
		sock = pos->first;
		m_mapAcceptSockQueue.erase(pos);
	}
	::LeaveCriticalSection(&this->m_struCriSec);
	if(sock)
	{
		shutdown(sock,SD_SEND); 
		closesocket(sock);//SO_UPDATE_ACCEPT_CONTEXT 参数会导致出现TIME_WAIT,即使设置了DONT
	}
	return 1;
}
bool CTcpServer::IsValidSocket(SOCKET socket)
{
	bool validSocket = false;
	::EnterCriticalSection(&this->m_struCriSec);
	SOCKET sock = NULL;
	map<SOCKET,long>::iterator pos;
	pos = m_mapAcceptSockQueue.find(socket);
	if(pos!=m_mapAcceptSockQueue.end())
	{
		validSocket = true;
	}
	::LeaveCriticalSection(&this->m_struCriSec);
	return validSocket;
}
void CTcpServer::ConnectTimeOutCheck()
{
	EnterCriticalSection(&m_struCriSec);

	map<SOCKET,long>::iterator pos;
	int iIdleTime = 0;
	int nTimeLen = 0;
	for(pos = m_mapAcceptSockQueue.begin();pos!=m_mapAcceptSockQueue.end();)
	{
		nTimeLen = sizeof(iIdleTime);
		//getsockopt(pos->first,SOL_SOCKET,SO_CONNECT_TIME,(char *)&iConnectTime, &nTimeLen);//此只能检测从开始链接到现在的时间，如果此socket在中间有数据接收或发送，仍然会断开连接
		getsockopt(pos->first,SOL_SOCKET, SO_GROUP_PRIORITY, (char *)&iIdleTime, &nTimeLen);
						
		if((int)time(NULL) - iIdleTime >120) //timeout value is 2 minutes.
		{
			cout<<"客户端: "<<pos->first<<"超时,系统将关闭此连接."<<endl;
			shutdown(pos->first,SD_SEND); 
			closesocket(pos->first);
			m_mapAcceptSockQueue.erase(pos++);
		}
		else
		{
           ++pos;
		}
	}
	LeaveCriticalSection(&m_struCriSec);
}
int CTcpServer::PostAcceptOperation(int postNumbs)
{
	unsigned long dwAcceptNumbs = 0;
	for(int i=0;i<postNumbs;i++)
	{
		if(IssueAcceptOperation())
		{
			dwAcceptNumbs++;
		}
	}
	return dwAcceptNumbs;
}
// 创建工作线程和监听线程
int CTcpServer::CteateThread()
{
	SYSTEM_INFO  sysinfo;
	GetSystemInfo(&sysinfo);
    m_iWorkerThreadNumbers = sysinfo.dwNumberOfProcessors*2+2;//最佳线程数量是CPU数量的2倍
	if(m_iWorkerThreadNumbers>=MAX_PROCESSOR_COUNTER) //注意此处很可能会引起性能的剧烈下降
		m_iWorkerThreadNumbers = MAX_PROCESSOR_COUNTER-1;


	m_hListenThread = (HANDLE)_beginthreadex(NULL,0,ListenThread,this,CREATE_SUSPENDED,NULL);
	if(!m_hListenThread)
		return 0;

    int counter = 0;
	for (int i=0; i<m_iWorkerThreadNumbers; i++)
	{
		m_hWorkerThread[i] = (HANDLE)_beginthreadex(NULL,0,WorkThread,this,CREATE_SUSPENDED,NULL);
		if(!m_hWorkerThread[i]) //将少创建一个线程
		{
			m_hWorkerThread[i] = NULL;
			continue;
		}	
		counter++;
	}
    return counter;
}
//释放资源
int CTcpServer::ReleaseResource(void)
{
	if(m_hListenSocket)
	{
		closesocket(m_hListenSocket);
		m_hListenSocket = NULL;
	}
	map<SOCKET,long>::iterator pos;
	for(pos = m_mapAcceptSockQueue.begin();pos!=m_mapAcceptSockQueue.end();++pos)
	{
		shutdown(pos->first,SD_SEND);
		closesocket(pos->first);
	}
	m_mapAcceptSockQueue.clear();

	timeKillEvent(m_iTimerID);
	CAcceptContext::DestroyContextPool();//销毁所有的AcceptContext对象
	CTcpReceiveContext::DestroyContextPool();
	CTcpSendContext::DestroyContextPool();

	ZeroMemory(&m_struServerAdd,sizeof(m_struServerAdd)); //服务器地址
	if(m_hCompletionPort)
	{
		CloseHandle(m_hCompletionPort);
		m_hCompletionPort = NULL;
	}
	if(m_hListenThread)
	{
		CloseHandle(m_hListenThread);
		m_hListenThread = NULL;
	}
	for(int i=0;i<m_iWorkerThreadNumbers;i++)
	{
		if(m_hWorkerThread[i])
		{
			CloseHandle(m_hWorkerThread[i]);
			m_hWorkerThread[i] = NULL;
		}
	}
    for(int i=0;i<ACCEPT_EVENT_CNT;i++)
	{
		if(m_hPostAcceptEvent[i])
		{
			CloseHandle(m_hPostAcceptEvent[i]);
			m_hPostAcceptEvent[i] = NULL;
		}

	}
	return 1;
}
//用于检测突然断线,只适用于windows 2000后平台
//即客户端也需要win2000以上平台
int CTcpServer::SetHeartBeatCheck(SOCKET clientSock)
{
	DWORD dwError = 0L,dwBytes = 0;
	TCP_KEEPALIVE_V sKA_Settings = {0},sReturned = {0};
	sKA_Settings.onoff = 1 ;
	sKA_Settings.keepalivetime = 5000 ; // Keep Alive in 5.5 sec.
	sKA_Settings.keepaliveinterval = 1000 ; // Resend if No-Reply

	dwError = WSAIoctl(clientSock, SIO_KEEPALIVE_VALS, &sKA_Settings,sizeof(sKA_Settings), &sReturned, sizeof(sReturned),&dwBytes,NULL, NULL);
	if(dwError == SOCKET_ERROR )
	{
		dwError = WSAGetLastError();
		DebugPrintf("SetHeartBeatCheck->WSAIoctl()发生错误,错误代码: %ld",dwError);	
		return 0;
	}
    return 1;
}
//显示新链接的客户端信息
void CTcpServer::ShowNewConnectInf(LPWSAOVERLAPPED pOverlapped)
{
	if(pOverlapped)
	{
		CAcceptContext* pContext = CONTAINING_RECORD(pOverlapped,CAcceptContext,m_struOperateOl);

		sockaddr_in* pClientAddr = NULL;
		sockaddr_in* pLocalAddr = NULL;
		int nClientAddrLen = 0;
		int nLocalAddrLen = 0;

		m_pfGetAddrs(pContext->m_ucAddressbuf, 0,ACCEPT_ADDRESS_LENGTH,ACCEPT_ADDRESS_LENGTH,
			(LPSOCKADDR*)&pLocalAddr, &nLocalAddrLen, (LPSOCKADDR*)&pClientAddr, &nClientAddrLen);

		ClientConnect(pContext->m_hSocket,inet_ntoa(pClientAddr->sin_addr),ntohs(pClientAddr->sin_port));
		
		//DebugPrintf("客户端:%s :%u 连接服务器成功.socket:%d\n",inet_ntoa(pClientAddr->sin_addr),ntohs(pClientAddr->sin_port),pContext->m_hSocket);
	}
}
//服务器暂停后会导致的后果暂未考虑
int CTcpServer::PauseServer(void)
{
try
	{
		if(m_bServerRunning)
		{
			throw CSrvException("Server have paused",-1);
		}
		if(m_hListenThread)
		{
			SuspendThread(m_hListenThread);
		}

		for (int i=0; i<m_iWorkerThreadNumbers; i++)
		{
			if(!m_hWorkerThread[i])
			{
				SuspendThread(m_hWorkerThread[i]);
			}
		}
		return 1;
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return 0;
	}
	catch(...)
	{
		return 0;
	}
}
UINT WINAPI CTcpServer::ListenThread(LPVOID lpParam)
{
	CTcpServer* pThis = (CTcpServer*)lpParam;
	if(!pThis)
		return 0; 

	unsigned long rc = 0;
	unsigned long postAcceptCnt = 1;
	while(pThis->m_bServerRunning)
	{

		rc = WSAWaitForMultipleEvents(ACCEPT_EVENT_CNT, pThis->m_hPostAcceptEvent, FALSE,1000, FALSE);		
		if(!pThis->m_bServerRunning)
			break;

		if(WSA_WAIT_FAILED == rc)
		{
			pThis->ErrorReport(WSAGetLastError(),"WSAWaitForMultipleEvents");
			continue;
		}
		else if(rc ==WSA_WAIT_TIMEOUT)//超时
		{
			continue;
		}
		else  
		{
			rc = rc - WSA_WAIT_EVENT_0;
			if(rc == 0)
				postAcceptCnt = pThis->m_iMaxPostAcceptNumbs;
			else
				postAcceptCnt = 1;

			pThis->PostAcceptOperation(postAcceptCnt);
		}
	}
	return 0;
}
UINT WINAPI CTcpServer::WorkThread(LPVOID lpParam)
{
	CTcpServer* pThis = (CTcpServer*)lpParam;
	if(!pThis)
		return 0;	

	static long byteCount = 0;
	BOOL    bResult;
	unsigned long   NumTransferred;
	ULONG_PTR contextKey = COMPLETION_KEY_SHUTDOWN ;
	LPOVERLAPPED pOverlapped = NULL;

	while(pThis->m_bServerRunning)
	{
		bResult = GetQueuedCompletionStatus(pThis->m_hCompletionPort,&NumTransferred,&contextKey,&pOverlapped,INFINITE);
		COperateContext* pContext = CONTAINING_RECORD(pOverlapped,COperateContext,m_struOperateOl);
		if ((bResult == FALSE) && (pOverlapped != NULL))//客户端非正常退出(包括机器重启)会在此检测到，在I/O系统排队的操作会返回,只有设置了heartbeat机制才会有此作用
		{
			pThis->DebugPrintf("监测到客户端:%d 非正常退出,错误代码:%d,操作码: %d\n",pContext->m_hSocket,GetLastError(),pContext->m_iOperateMode);
			pThis->CloseClientSocket(pContext->m_hSocket);
			pContext->ReleaseContext();
			continue;
		}
		else if((bResult == TRUE) && (pOverlapped == NULL))
		{
          pThis->DebugPrintf("用户操作服务器正常退出\n");	
		  break;
		}
		else if((bResult == TRUE)&& (pOverlapped != NULL))//正常操作
		{
			if(contextKey == COMPLETION_KEY_IO)
			{
				switch(pContext->m_iOperateMode)
				{
				case SC_WAIT_ACCEPT:
					{
						pThis->CompleteAccept(pOverlapped,NumTransferred);
						break;
					}
				case SC_WAIT_RECEIVE://client gracefulclose handle here 正常退出也在此例程中处理
					{
						if(NumTransferred == 0)
						{
							pThis->DebugPrintf("客户端:%d 正常退出\n",pContext->m_hSocket);
							pThis->CloseClientSocket(pContext->m_hSocket);
							pContext->ReleaseContext();
						}
						else
						   pThis->CompleteReceive(pOverlapped,NumTransferred);
						break;
					}
				case SC_WAIT_TRANSMIT:
					{
						pThis->CompleteSend(pOverlapped,NumTransferred);
						break;
					}
				default:
					break;
				}
			}
			else if(contextKey == COMPLETION_KEY_SHUTDOWN)
			{
				pContext->ReleaseContext();
				break;
			}
		}
	}
	return 0;
}
//定时器回调函数,功能同线程函数完全相同

void PASCAL CTcpServer::TimerProc(UINT wTimerID,UINT msg,DWORD dwUser,DWORD dw1,DWORD dw2)
{
	CTcpServer* pThis = (CTcpServer*)dwUser;
	pThis->DebugPrintf("RevPool:%ld,Idle:%ld,AccPool: %d,Idle:%ld,SendPool: %ld,Idle: %ld. connect:%ld \n",CTcpReceiveContext::GetContextCounter(),CTcpReceiveContext::GetIdleContextCounter(),CAcceptContext::GetContextCounter(),CAcceptContext::GetIdleContextCounter(),CTcpSendContext::GetContextCounter(),CTcpSendContext::GetIdleContextCounter(),pThis->m_mapAcceptSockQueue.size());
    
	pThis->ConnectTimeOutCheck();

	//CTcpReceiveContext::ShowContextIndex();

			
}
int CTcpServer::IssueAcceptOperation()
{
	try
	{
		int errorCode = 1;
		unsigned long dwBytes = 0;
		unsigned long dwAcceptNumbs = 0;

		SOCKET hClientSocket = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, NULL, 0, WSA_FLAG_OVERLAPPED);
		if (INVALID_SOCKET == hClientSocket)
		{
			CloseClientSocket(hClientSocket);
			throw CSrvException("PostAcceptOperation()->WSASocket(),Invalid socket handle.",-1,(long)(__LINE__));
		}
		//设置为异步模式
		ULONG ul = 1;
		errorCode =ioctlsocket(hClientSocket, FIONBIO, &ul);
		if(SOCKET_ERROR == errorCode)
		{
			CloseClientSocket(hClientSocket);
			throw CSrvException("PostAcceptOperation()->ioctlsocket(),incorrect ioctlsocket operation.",-1,(long)(__LINE__));
		}
		CAcceptContext* pAccContext = CAcceptContext::GetContext();
		if (NULL == pAccContext)
		{
			CloseClientSocket(hClientSocket);
			throw CSrvException("PostAcceptOperation()->CAcceptContext::GetContext(),Invalid AcceptContext handle.",-1,(long)(__LINE__));
		}
		pAccContext->SetAcceptParameters(this->m_hListenSocket,hClientSocket);

		errorCode = m_pfAcceptEx(pAccContext->m_struListenSocket,pAccContext->m_hSocket, pAccContext->m_ucAddressbuf,0,ACCEPT_ADDRESS_LENGTH, ACCEPT_ADDRESS_LENGTH, &dwBytes, &(pAccContext->m_struOperateOl));
		if(FALSE == errorCode && ERROR_IO_PENDING != WSAGetLastError())
		{
			CloseClientSocket(hClientSocket);
			pAccContext->ReleaseContext();
			throw CSrvException("PostAcceptOperation()->AcceptEx,incorrect AcceptEx operation.",-1,(long)(__LINE__));
		}
		return 1;
	}
	catch(CSrvException& e)
	{	
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return 0;
	}
}
//连接请求完成例程
//在此函数中主要设置net conncet sockt的参数
//投递下一个accept请求及在新socket上投递receive请求
int CTcpServer::CompleteAccept(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes)
{
	try
	{
		if(pOverlapped)
		{
			CAcceptContext* pContext = CONTAINING_RECORD(pOverlapped,CAcceptContext,m_struOperateOl);

			m_struTestSockt = pContext->m_hSocket;  //测试用
			PushClientSocket(pContext->m_hSocket);//把新联入的socket入栈,集中管理

			int errorCode = 0;
			//int nZero=1024*1000;//关闭发送缓冲区
			//errorCode = setsockopt(pContext->m_hSocket,SOL_SOCKET,SO_SNDBUF,(char *)&nZero,sizeof(nZero));
			//errorCode = setsockopt(pContext->m_hSocket,SOL_SOCKET,SO_RCVBUF,(char *)&nZero,sizeof(nZero));

			int nOpt = 1;//socket重用，防止当出现大量TIME_WAIT状态的时候重用socket
			errorCode = setsockopt(pContext->m_hSocket,SOL_SOCKET ,SO_REUSEADDR,(const char*)&nOpt,sizeof(nOpt));
			if(errorCode == SOCKET_ERROR)
			{
				CloseClientSocket(pContext->m_hSocket);
				pContext->ReleaseContext();
				throw CSrvException("CompleteAccept->setsockopt() error..",-1,(long)(__LINE__));
			}
			int dontLinget = 1;//服务器关闭socket的时候,不执行正常的四步握手关闭，而是执行RESET
			errorCode = setsockopt(pContext->m_hSocket,SOL_SOCKET,SO_DONTLINGER,(char *)&dontLinget,sizeof(dontLinget)); 
			if(errorCode == SOCKET_ERROR)
			{
				CloseClientSocket(pContext->m_hSocket);
				pContext->ReleaseContext();
				throw CSrvException("CompleteAccept->setsockopt() error..",-1,(long)(__LINE__));
			}
			//char chOpt=1; //发送的时候关闭Nagle算法,关闭nagel算法有可能会引起断流
			//errorCode = setsockopt(pContext->m_hSocket,IPPROTO_TCP,TCP_NODELAY,&chOpt,sizeof(char));   
			//if(errorCode == SOCKET_ERROR)
			//{
			//	CloseClientSocket(pContext->m_hSocket);
			//	pContext->ReleaseContext();
			//	throw CSrvException("CompleteAccept->setsockopt() error..",-1,(long)(__LINE__));
			//}
			int accTime = (int)time(NULL);//更新当前socket活动的时间,用于超时检测
			errorCode = setsockopt(pContext->m_hSocket, SOL_SOCKET, SO_GROUP_PRIORITY, (char *)&accTime, sizeof(accTime));
			if(errorCode == SOCKET_ERROR)
			{
				CloseClientSocket(pContext->m_hSocket);
				pContext->ReleaseContext();
				throw CSrvException("CompleteAccept->setsockopt() error..",-1,(long)(__LINE__));
			}
			if(!this->SetHeartBeatCheck(pContext->m_hSocket))//设置心跳包参数
			{
				CloseClientSocket(pContext->m_hSocket);
				pContext->ReleaseContext();
				throw CSrvException("CompleteAccept->setsockopt() error..",-1,(long)(__LINE__));
			}

			//在客户端发起高速连接时候,如果开启该功能，会出现后面的IssueReceiveOperation中报10057错误,而此函数输出可看到没有正确解析出客户端信息
			this->ShowNewConnectInf(pOverlapped);

			if(NULL == CreateIoCompletionPort((HANDLE)pContext->m_hSocket,this->m_hCompletionPort,(ULONG_PTR)0,0))
			{
				CloseClientSocket(pContext->m_hSocket);
				pContext->ReleaseContext();
				throw CSrvException("CompleteAccept->setsockopt() error..",-1,(long)(__LINE__));
			}
			else
			{   
				if(!IssueReceiveOperation(pContext->m_hSocket))//发送接受数据请求
				{
					CloseClientSocket(pContext->m_hSocket);
					pContext->ReleaseContext();
					throw CSrvException("CompleteAccept->IssueReceiveOperation() occured an error.",-1,(long)(__LINE__));
				}
			}
			if(!IssueAcceptOperation())
			{
				ReleaseSemaphore(m_hPostAcceptEvent[1],1,NULL);
			}
			pContext->ReleaseContext();
			return 1;
		}
		else
		{
			throw CSrvException("CompleteAccept->pOverlapped is null..",-1,(long)(__LINE__));
		}
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return 0;
	}
}
//发送异步接收请求
int CTcpServer::IssueReceiveOperation(SOCKET socket)
{
	try
	{
		CTcpReceiveContext* pContext = CTcpReceiveContext::GetContext(socket);
		if(pContext)
		{
			//在新连接的socket上投递receive操作，等待remote client发送数据过来.
			unsigned long bytes = 0;
			unsigned long flag = 0;
			int err = WSARecv(pContext->m_hSocket,pContext->GetBufAddr(),1,&bytes,&flag, &(pContext->m_struOperateOl), NULL);
			if (SOCKET_ERROR == err && WSA_IO_PENDING != WSAGetLastError())
			{
				pContext->ReleaseContext();
				ErrorReport(WSAGetLastError(),"IssueReceiveOperation()->WSARecv()");
				throw CSrvException("IssueReceiveOperation->WSARecv() error.",-1,(long)(__LINE__));
			}
			return 1;
		}
		else
		{
			throw CSrvException("IssueReceiveOperation->CTcpReceiveContext::GetContext() occured an error.",-1,(long)(__LINE__));
		}
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return 0;
	}
}
////接收完成例程
//要完成的工作有
//1  保存数据
//2  投递下一个Receive工作
//3  在2的基础上重用上一次receive的context
int CTcpServer::CompleteReceive(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes)
{
	try
	{
		if(pOverlapped)
		{
			CTcpReceiveContext* pContext = CONTAINING_RECORD(pOverlapped,CTcpReceiveContext,m_struOperateOl);
			if(dwTransferBytes && pContext)
			{
				//更新当前socket活动的时间,用于超时检测
				//此功能放到complete中可能使系统的性能更好
				int revTime = (int)time(NULL);
				int errorCode = setsockopt(pContext->m_hSocket, SOL_SOCKET, SO_GROUP_PRIORITY, (char *)&revTime, sizeof(revTime));
				if(SOCKET_ERROR == errorCode)
				{
					pContext->ReleaseContext();
					CloseClientSocket(pContext->m_hSocket);
					throw CSrvException("CompleteReceive->setsockopt() error..",-1,(long)(__LINE__));
				}
				//在此处需要判断接受数据是否完成及保存数据
			    SaveReceivedData(pContext->m_hSocket,pContext->GetBufAddr()->buf,dwTransferBytes);
				
				//IssueSendOperation(pContext->m_hSocket,pContext->GetBufAddr()->buf,dwTransferBytes);//把接收到的数据原封不动地返回给client
				
				if(!IssueReceiveOperation(pContext->m_hSocket))
				{
					pContext->ReleaseContext();
					CloseClientSocket(pContext->m_hSocket);
					throw CSrvException("CompleteReceive->IssueReceiveOperation() error..",-1,(long)(__LINE__));
				}
			}	
			else if(dwTransferBytes==0)//client 主动断开了链接
			{
				CloseClientSocket(pContext->m_hSocket);
			}
			pContext->ReleaseContext();
			return 1;
		}
		else
		{
			throw CSrvException("CompleteReceive->pOverlapped is null.",-1,(long)(__LINE__));
		}
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return 0;
	}
}
////发送异步文件传送请求
int CTcpServer::IssueSendOperation(SOCKET cliSock,const char* pData,unsigned long dwTransferBytes)
{
	try
	{
		unsigned long dwBytes = 0,err = 0;
		unsigned long leavingBytes = dwTransferBytes; 
		if(leavingBytes!=0)
		{		
			CTcpSendContext* pContext = NULL;
			do
			{
				pContext = CTcpSendContext::GetContext();
				leavingBytes = pContext->SetSendParameters(cliSock,pData,leavingBytes);
				err = WSASend(pContext->m_hSocket,&pContext->m_struSendBuf,1,&dwBytes,0,&pContext->m_struOperateOl,NULL);
				if(SOCKET_ERROR == err && WSA_IO_PENDING != WSAGetLastError())
				{
					pContext->ReleaseContext();
					DebugPrintf("IssueSendOperation->WSASend() occured a error.错误代码:%ld.",WSAGetLastError());
				    throw CSrvException("IssueSendOperation->WSASend occured an error.",-1,(long)(__LINE__));
				}
			}while(leavingBytes!=0);
			return 1;
		}
		else
		{
			throw CSrvException("IssueSendOperation->WSASend occured an error.",-1,(long)(__LINE__));
		}
	}
	catch(CSrvException& e)
	{
		DebugPrintf("%s at line:%d\n",e.GetExpDescription(),e.GetExpLine());
		return 0;
	}
}
////文件传送请求完成例程
int CTcpServer::CompleteSend(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes)
{
	if(pOverlapped)
	{
		CTcpSendContext* pContext = CONTAINING_RECORD(pOverlapped,CTcpSendContext,m_struOperateOl);
		if(pContext->m_struSendBuf.len != dwTransferBytes)
			DebugPrintf("投递的send 操作数据没有发送完成.\n");

		/*char buf[512] = "\n";
		IssueSendOperation(pContext->m_hSocket,buf,512);*/	
		pContext->ReleaseContext();	
		//DebugPrintf("成功发送字节数为:%ld\n",dwTransferBytes);
		return 1;
	}
	return 0;
}

void CTcpServer::ErrorReport(unsigned long errorCode,char* description)
{
	DebugPrintf("函数: %s 发生错误,错误代码为: %d\n",description,errorCode);
}
void CTcpServer::DebugPrintf(char* szFormat,...)
{
	if (!m_struCriSec.OwningThread) return;

	 EnterCriticalSection(&m_struCriSec);
     
     va_list args;
     va_start(args, szFormat);
     
     vprintf(szFormat, args );
     
     va_end(args);
     
     LeaveCriticalSection(&m_struCriSec);
}