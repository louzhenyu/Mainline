#pragma once

#ifndef _TCP_SERVER_H
#define _TCP_SERVER_H

#include "TcpSrvException.h"

#include "BaseDefine.h"
#include "tcpreceivecontext.h"
#include "TcpSendContext.h"
#include "AcceptContext.h"


#include <map>
using namespace std;

const int ACCEPT_EVENT_CNT = 2;

class CTcpServer
{
public:
	CTcpServer(void);
	~CTcpServer(void);
public:
	int InitServer(char* ipAddress,unsigned short port) throw(CSrvException);//初始化服务器
	int StartServer(void) throw(CSrvException);//启动服务器
	int StopServer(void) throw(CSrvException);//停止服务器
	int PauseServer(void);
	int CloseClientSocket(SOCKET clientSocket);//关闭异常连接 改为公用
protected:
	int SendFile(SOCKET sock,char* fileName);//通过此函数想远端发送文件
	int SendData(SOCKET sock,const char* pVoid,long dataSize);//通过此函数发送普通数据
	virtual void SaveReceivedData(SOCKET sock,char* pVoid,long dataLen){;}//从CTcpServer继承的类都需要实现此函数
	virtual void ClientConnect(SOCKET sock,char* ip,UINT nPort){;}//用户登录	
private:
	SOCKET m_hListenSocket;//监听套接字句柄
	SA m_struServerAdd; //服务器地址
	HANDLE m_hCompletionPort;
	HANDLE m_hListenThread;
	HANDLE m_hWorkerThread[MAX_PROCESSOR_COUNTER];
	HANDLE m_hPostAcceptEvent[ACCEPT_EVENT_CNT];	
	CRITICAL_SECTION m_struCriSec;
private:
	LPFN_ACCEPTEX m_pfAcceptEx;				//AcceptEx函数地址
	LPFN_GETACCEPTEXSOCKADDRS m_pfGetAddrs;	//GetAcceptExSockaddrs函数的地址
private:
	int m_iMaxPostAcceptNumbs;
	int m_iWorkerThreadNumbers;
	bool m_bServerRunning;
	unsigned long m_iTimerID;
private:
	static UINT WINAPI ListenThread(LPVOID lpParam);//监听client connet事件
	static UINT WINAPI WorkThread(LPVOID lpParma);//整整的work线
	static void PASCAL TimerProc(UINT wTimerID,UINT msg,DWORD dwUser,DWORD dw1,DWORD dw2);
private:
	int PushClientSocket(SOCKET clientSocket);//把新链接的socket保存起来，集中管理	
	bool IsValidSocket(SOCKET socket);//判断传入的socket是否建立的合法的connect
	map<SOCKET,long> m_mapAcceptSockQueue;
	SOCKET m_struTestSockt;
private:	
	void ConnectTimeOutCheck();
	int PostAcceptOperation(int postNumbs);
	int CteateThread();
	int ReleaseResource(void);
	int SetHeartBeatCheck(SOCKET clientSock);
	void ShowNewConnectInf(LPWSAOVERLAPPED pOverlapped);
private:
	  int IssueReceiveOperation(SOCKET socket);
	  int IssueAcceptOperation();
	  int IssueSendOperation(SOCKET cliSock,const char* pData,unsigned long dwTransferBytes);

	  int CompleteReceive(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes);
	  int CompleteAccept(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes);
	  int CompleteSend(LPWSAOVERLAPPED pOverlapped,unsigned long dwTransferBytes);

private: 
	void ErrorReport(unsigned long errorCode,char* description);
	void DebugPrintf(char* szFormat,...);
};
#endif