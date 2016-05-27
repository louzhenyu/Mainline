#include "stdafx.h"
#include ".\udpserver.h"
#include "process.h"
#include <MMSystem.h>
#include <time.h>
#pragma comment(lib,"Winmm.lib")

CUdpServer::CUdpServer(void)
{
	m_hListenSocket = NULL;
	ZeroMemory(&m_struServerAdd,sizeof(m_struServerAdd)); //服务器地址
	m_hCompletionPort = NULL;
	m_hListenThread = NULL;

	for(int i=0;i<MAX_PROCESSOR_COUNTER;i++)
		m_hWorkerThread[i] = NULL;

	for(int i=0;i<RECEIVE_EVENT_CNT;i++)
		m_hPostReceiveEvent[i] = CreateEvent(NULL,FALSE,FALSE,NULL);

	m_iMaxPostReceiveNumbs = 32;
	m_iWorkerThreadNumbers = 2;
	m_bServerRunning = false;

	InitializeCriticalSection(&m_struCriSec);

	WSADATA wsData;
	int errorCode = WSAStartup(MAKEWORD(2, 2), &wsData);//初始化ws2_32.lib库
}

CUdpServer::~CUdpServer(void)
{
	m_hListenSocket = NULL;
	ZeroMemory(&m_struServerAdd,sizeof(m_struServerAdd)); //服务器地址
	m_hCompletionPort = NULL;
	m_hListenThread = NULL;

	for(int i=0;i<MAX_PROCESSOR_COUNTER;i++)
		m_hWorkerThread[i] = NULL;

	for(int i=0;i<RECEIVE_EVENT_CNT;i++)
		m_hPostReceiveEvent[i] = NULL;

	m_iWorkerThreadNumbers = 2;//最佳线程数量是CPU数量的2倍
	m_bServerRunning = false;

	DeleteCriticalSection(&m_struCriSec);

	WSACleanup();
}
int CUdpServer::InitServer(char* ipAddress,unsigned short port)
{
	try
	{
		int errorCode = 1;
		if(!ipAddress)
		{
			throw CSrvException("Invalid Ip address.",-1,(long)(__LINE__));
		}
		//创建套接字
		m_hListenSocket = WSASocket(AF_INET,SOCK_DGRAM,IPPROTO_UDP, NULL, 0, WSA_FLAG_OVERLAPPED);
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

		//设置为地址重用，优点在于服务器关闭后可以立即启用		
		int nOpt = 1;
		errorCode = setsockopt(m_hListenSocket, SOL_SOCKET, SO_REUSEADDR, (char*)&nOpt, sizeof(nOpt));
		if(SOCKET_ERROR == errorCode)
		{
			throw CSrvException("Set listen socket to SO_REUSEADDR mode error.",-1,(long)(__LINE__));
		}

		//关系接收与发送缓冲区
		int nBufferSize = 0;
		setsockopt(m_hListenSocket,SOL_SOCKET,SO_SNDBUF,(char*)&nBufferSize,sizeof(int));
		setsockopt(m_hListenSocket,SOL_SOCKET,SO_RCVBUF,(char*)&nBufferSize,sizeof(int)); 

		unsigned long dwBytesReturned = 0;
		int bNewBehavior = FALSE;

		//下面的函数用于解决远端突然关闭会导致WSARecvFrom放回10054错误导致服务器完成队列中没有reeceive操作而设置
		errorCode  = WSAIoctl(m_hListenSocket, SIO_UDP_CONNRESET,&bNewBehavior, sizeof(bNewBehavior),
			              NULL, 0, &dwBytesReturned,NULL, NULL);
		if (SOCKET_ERROR == errorCode)
		{
			throw CSrvException("Set listen socket to SIO_UDP_CONNRESET mode error.",-1,(long)(__LINE__));
		}
		//填充服务器地址信息
		m_struServerAdd.sin_family = AF_INET;
		m_struServerAdd.sin_addr.s_addr = inet_addr(ipAddress);
		m_struServerAdd.sin_port = htons(port);

		//创建完成端口
		m_hCompletionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE,NULL,0,0);
		if(m_hCompletionPort == NULL)
		{
			throw CSrvException("Invalid completion port handle.",-1,(long)(__LINE__));
		}
		//邦定套接字和服务器地址
		errorCode = bind(m_hListenSocket,(struct sockaddr*)&m_struServerAdd,sizeof(m_struServerAdd));
		if(errorCode)
		{
			throw CSrvException("Error socket bind operation.",-1);
		}

		//创建主线程和工作线程
		if(!this->CteateThread())
		{
			throw CSrvException("Create worker thread error.",-1,(long)(__LINE__));
		}
		//把监听线程和完成端口邦定
		CreateIoCompletionPort((HANDLE)m_hListenSocket,m_hCompletionPort,SC_WAIT_ACCEPT,0);

		CUdpContext::InitContextPool(128);
		//投递reveive操作，等待client的udp数据包到达
		PostReceiveOperation(m_iMaxPostReceiveNumbs);

		//m_iTimerID = timeSetEvent(1000,1000,TimerProc,(DWORD)this,TIME_PERIODIC);//1秒钟定时器
		
		return 1;
	}
	catch(CSrvException& e)
	{
		cout<<e.GetExpDescription()<<"at line:"<<e.GetExpLine()<<endl;
		this->ReleaseResource();//释放分配了的资源
		return e.GetExpCode();
	}
	catch(...)//截获所有异常
	{
		return 0;
	}	
}
int CUdpServer::StartServer(void)
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
				ResumeThread(m_hWorkerThread[i]);
			}
		}
		return 1;
	}
	catch(CSrvException& e)
	{
		cout<<e.GetExpDescription()<<"at line:"<<e.GetExpLine()<<endl;
		return e.GetExpCode();
	}
	catch(...)
	{
		return 0;
	}
}

int CUdpServer::StopServer(void)
{
	if(!m_bServerRunning)
	{
		StartServer();
	}

	//设置下面的语句的目的是把socke阻塞运行修改为非阻塞模式
	unsigned long ul = 1;//要改为非阻赛模式，此值必须非零
	ioctlsocket(m_hListenSocket, FIONBIO, &ul);//设置socket 的I/O模式，为同步还是异步

	m_bServerRunning = false;//结束服务器线程
	PostQueuedCompletionStatus(m_hCompletionPort, COMPLETION_KEY_SHUTDOWN, 0, NULL);
	for(int i=0;i<RECEIVE_EVENT_CNT;i++)
	{
		if(m_hPostReceiveEvent[i])
			SetEvent(m_hPostReceiveEvent[i]);
	}
	if(m_hListenThread)
	{
		WaitForSingleObject(m_hListenThread,INFINITE);
	}
	for(int i=0;i<m_iWorkerThreadNumbers;i++)
	{
		PostQueuedCompletionStatus(m_hCompletionPort, COMPLETION_KEY_SHUTDOWN, 0, NULL);
	}
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

int CUdpServer::RestartServer(void)
{
	return 0;
}

int CUdpServer::GetRevData(void)
{
	return 0;
}

int CUdpServer::SendData(char* pVoid,long dataSize)
{
	return 0;
}
int CUdpServer::PostReceiveOperation(int postNumbs)
{
	int errorCode = 1;
	unsigned long dwPostReceiveNumbs = 0;
	for(int i=0;i<postNumbs;i++)
	{
		if(IssueReceiveOperation())
			dwPostReceiveNumbs++;
	}
	return dwPostReceiveNumbs;
}
// 创建工作线程和监听线程
int CUdpServer::CteateThread()
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
int CUdpServer::ReleaseResource(void)
{
	if(m_hListenSocket)
	{
		closesocket(m_hListenSocket);
		m_hListenSocket = NULL;
	}
	timeKillEvent(m_iTimerID);
	CUdpContext::DestroyContextPool();
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
	for(int i=0;i<RECEIVE_EVENT_CNT;i++)
	{
		if(m_hPostReceiveEvent[i])
		{
			CloseHandle(m_hPostReceiveEvent[i]);
			m_hPostReceiveEvent[i] = NULL;
		}

	}
	return 1;
}
//服务器暂停后会导致的后果暂未考虑
int CUdpServer::PauseServer(void)
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
		cout<<e.GetExpDescription()<<"expCode:"<<e.GetExpCode()<<endl;
		return 0;
	}
	catch(...)
	{
		return 0;
	}
}
UINT WINAPI CUdpServer::ListenThread(LPVOID lpParam)
{
	CUdpServer* pThis = (CUdpServer*)lpParam;
	if(!pThis)
		return 0; 


	//	int cleintAddrSize = sizeof(SA);
	unsigned long rc = 0;
	unsigned long postReceiveCnt = 1;
	while(pThis->m_bServerRunning)
	{

		rc = WSAWaitForMultipleEvents(RECEIVE_EVENT_CNT, pThis->m_hPostReceiveEvent, FALSE,WSA_INFINITE, FALSE);		
		if(!pThis->m_bServerRunning)
			break;

		if(WSA_WAIT_FAILED == rc)
		{
			pThis->ErrorReport(WSAGetLastError(),"WSAWaitForMultipleEvents");
			continue;
		}
		else if(rc ==WSA_WAIT_TIMEOUT)
		{
			continue;
		}
		else  //超时
		{
			rc = rc - WSA_WAIT_EVENT_0;
			if(rc == 0)
				postReceiveCnt = pThis->m_iMaxPostReceiveNumbs;
			else
				postReceiveCnt = 1;
			pThis->PostReceiveOperation(postReceiveCnt);
		}
	}
	return 0;
}
UINT WINAPI CUdpServer::WorkThread(LPVOID lpParam)
{
	CUdpServer* pThis = (CUdpServer*)lpParam;
	if(!pThis)
		return 0;	


	static long counter = 0;

	BOOL    bResult;
	unsigned long   NumTransferred;
	ULONG_PTR contextKey = COMPLETION_KEY_SHUTDOWN ;
	LPOVERLAPPED pOverlapped = NULL;

	while(true)
	{
		bResult = FALSE;
		NumTransferred = 0;
		contextKey = COMPLETION_KEY_SHUTDOWN;
		pOverlapped = NULL;

		bResult = GetQueuedCompletionStatus(pThis->m_hCompletionPort,&NumTransferred,&contextKey,&pOverlapped,INFINITE);
		CUdpContext* pContext = CONTAINING_RECORD(pOverlapped,CUdpContext,m_struOperateOl);
		if ((bResult == FALSE) && (pOverlapped != NULL))//客户端非正常退出(包括机器重启)会在此检测到，在I/O系统排队的操作会返回,只有设置了heartbeat机制才会有此作用
		{
			unsigned long errorCode = GetLastError();
		
			if(ERROR_MORE_DATA == errorCode)
			{
				pThis->DebugPrintf("过大的UDP包.\n");
			}
			else if(ERROR_PORT_UNREACHABLE == errorCode)//远程系统异常终止
			{
			   pThis->DebugPrintf("客户端终止数据发送,错误代码:%d.\n",errorCode);
			}	
			
			pContext->ReleaseContext();
			pThis->IssueReceiveOperation();
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
				case SC_WAIT_RECEIVE://client gracefulclose handle here 正常退出也在此例程中处理
					{
						if(NumTransferred == 0)
						{
							pThis->DebugPrintf("客户端:%d 正常退出\n",pContext->m_hSocket);
							pContext->ReleaseContext();
							pThis->IssueReceiveOperation();
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
void PASCAL CUdpServer::TimerProc(UINT wTimerID,UINT msg,DWORD dwUser,DWORD dw1,DWORD dw2)
{
	CUdpServer* pThis = (CUdpServer*)dwUser;
	pThis->DebugPrintf("UdpContext pool:%ld,Idle:%ld,time:%ld\n",CUdpContext::GetContextCounter(),CUdpContext::GetIdleContextCounter(),(unsigned long)time(NULL));
}
//发送异步接收请求
int CUdpServer::IssueReceiveOperation()
{
	try
	{
		int errorCode = 1;
		unsigned long dwPostReceiveNumbs = 0;

		//创建连接context
		CUdpContext* pUdpContext = CUdpContext::GetContext(this->m_hListenSocket,SC_WAIT_RECEIVE);
		if(pUdpContext==NULL)
		{
			throw CSrvException("IssueReceiveOperation->CUdpContext::GetContext() error.",-1,(long)(__LINE__));
		}	

		unsigned long dwFlag = 0;
		unsigned long dwBytes = 0;
		int nAddrLen = sizeof(struct sockaddr);

		errorCode = WSARecvFrom(pUdpContext->m_hSocket,&pUdpContext->m_struRevBuf,1,&dwBytes,&dwFlag,(struct sockaddr*)&pUdpContext->m_struRemoteAddr,&nAddrLen,&pUdpContext->m_struOperateOl, NULL);
		if (SOCKET_ERROR == errorCode && ERROR_IO_PENDING != WSAGetLastError())		
		{
			ErrorReport(WSAGetLastError(),"PostReceiveOperation()->WSARecvFrom()");
			if(WSAECONNRESET !=WSAGetLastError())//当远方关闭后，因为ICMP关系，会出现此错误
			    pUdpContext->ReleaseContext();
			throw CSrvException("IssueReceiveOperation->WSARecvFrom() error.",-1,(long)(__LINE__));
		}
		return 1;
	}
	catch(CSrvException& e)
	{
		//DebugPrintf("%s at line:%d, errorCode: %d\n",e.GetExpDescription(),e.GetExpLine(),e.GetExpCode());
		return 0;
	}
}
////接收完成例程
static long revConuter = 0;
int CUdpServer::CompleteReceive(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes)
{
	if(pOverlapped)
	{
		CUdpContext* pContext = CONTAINING_RECORD(pOverlapped,CUdpContext,m_struOperateOl);
		//在此处需要判断接受数据是否完成	
        IssueReceiveOperation();

		//ShowNewConnectInf(pOverlapped);//此函数打开在release模式下程序会崩溃，分析原因是printf函数在release高速模式下有问题
		IssueSendOperation(pContext->m_struRemoteAddr,pContext->m_struRevBuf.buf,dwTransferBytes);
		pContext->ReleaseContext();
	}
	return 0;
}
void CUdpServer::ShowNewConnectInf(LPWSAOVERLAPPED pOverlapped)
{
	if(pOverlapped)
	{
		CUdpContext* pContext = CONTAINING_RECORD(pOverlapped,CUdpContext,m_struOperateOl);
		DebugPrintf("客户端:%s :%u 连接服务器成功.\n",inet_ntoa(pContext->m_struRemoteAddr.sin_addr),ntohs(pContext->m_struRemoteAddr.sin_port));
	}
}
//发送异步文件传送请求
int CUdpServer::IssueSendOperation(struct sockaddr_in& remoteAddr,char* pData,unsigned long dwTransferBytes)
{
	try
	{
		unsigned long dwBytes = 0,err = 0;
		unsigned long leavingBytes = dwTransferBytes; 
		if(leavingBytes!=0)
		{		
			CUdpContext* pContext = NULL;
			do
			{
				pContext = CUdpContext::GetContext(this->m_hListenSocket,SC_WAIT_TRANSMIT);
				leavingBytes = pContext->SetSendParameters(remoteAddr,pData,leavingBytes);
				err = WSASendTo(pContext->m_hSocket,pContext->GetSendBuf(),1,&dwBytes,0,
					(struct sockaddr*)&pContext->m_struRemoteAddr,sizeof(struct sockaddr),&pContext->m_struOperateOl,NULL);
				if(SOCKET_ERROR == err && WSA_IO_PENDING != WSAGetLastError())
				{
					pContext->ReleaseContext();
					DebugPrintf("IssueSendOperation->WSASend() occured a error.错误代码:%ld.",WSAGetLastError());
					throw CSrvException("IssueSendOperation->WSASend occured an error.",-1,(long)(__LINE__));
				}
			}while(leavingBytes!=0);
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
	return 0;
}
////文件传送请求完成例程
int CUdpServer::CompleteSend(LPWSAOVERLAPPED pOverlapped,DWORD dwTransferBytes)
{
	if(pOverlapped)
	{
		CUdpContext* pContext = CONTAINING_RECORD(pOverlapped,CUdpContext,m_struOperateOl);
		//DebugPrintf("向客户端:%s :%u 发送字节数为%ld.\n",inet_ntoa(pContext->m_struRemoteAddr.sin_addr),ntohs(pContext->m_struRemoteAddr.sin_port),dwTransferBytes);
		pContext->ReleaseContext();
	}
	return 0;
}
void CUdpServer::ErrorReport(unsigned long errorCode,char* description)
{
	EnterCriticalSection(&m_struCriSec);
	cout<<"函数 "<<description<<"发生错误,错误代码: "<<errorCode<<endl;
	LeaveCriticalSection(&m_struCriSec);

}
void CUdpServer::DebugPrintf(char* szFormat,...)
{
	EnterCriticalSection(&m_struCriSec);

	va_list args;
	va_start(args, szFormat);

	vprintf(szFormat, args );

	va_end(args);

	LeaveCriticalSection(&m_struCriSec);
}