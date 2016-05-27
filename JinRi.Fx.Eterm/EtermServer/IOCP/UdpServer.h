#pragma once
#ifndef _UDP_SERVER_H
#define _UDP_SERVER_H

#include "BaseDefine.h"
#include "TcpSrvException.h"
#include "udpcontext.h"

const int RECEIVE_EVENT_CNT = 2;


class CUdpServer
{
public:
	CUdpServer(void);
	~CUdpServer(void);
public:
	int InitServer(char* ipAddress,unsigned short port) throw(CSrvException);//初始化服务器
	int StartServer(void) throw(CSrvException);//启动服务器
	int StopServer(void) throw(CSrvException);//停止服务器
	int PauseServer(void);
	int RestartServer(void) throw(CSrvException);//重启服务器
	int GetRevData(void);//接收数据
	int SendData(char* pVoid,long dataSize);//发送数据
private:
	SOCKET m_hListenSocket;//监听套接字句柄
	SA m_struServerAdd; //服务器地址
	HANDLE m_hCompletionPort;
	HANDLE m_hListenThread;
	HANDLE m_hWorkerThread[MAX_PROCESSOR_COUNTER];
	HANDLE m_hPostReceiveEvent[RECEIVE_EVENT_CNT];
	CRITICAL_SECTION m_struCriSec;
private:
	int m_iMaxPostReceiveNumbs;
	int m_iWorkerThreadNumbers;
	bool m_bServerRunning;
	unsigned long m_iTimerID;
private:
	static UINT WINAPI ListenThread(LPVOID lpParam);//监听client connet事件
	static UINT WINAPI WorkThread(LPVOID lpParma);//整整的work线
	static void PASCAL TimerProc(UINT wTimerID,UINT msg,DWORD dwUser,DWORD dw1,DWORD dw2);
private:
	int PostReceiveOperation(int postNumbs);
	int CteateThread();
	int ReleaseResource(void);
	void ShowNewConnectInf(LPWSAOVERLAPPED pOverlapped);
private:
	  int IssueReceiveOperation();	 
	  int CompleteReceive(LPWSAOVERLAPPED pOverlapped,DWORD dwTransferBytes);
	  int IssueSendOperation(struct sockaddr_in& remoteAddr,char* pData,unsigned long dwTransferBytes);
	  int CompleteSend(LPWSAOVERLAPPED pOverlapped,DWORD dwTransferBytes);
private:
	void ErrorReport(unsigned long errorCode,char* description);
	void DebugPrintf(char* szFormat,...);
};

#endif
