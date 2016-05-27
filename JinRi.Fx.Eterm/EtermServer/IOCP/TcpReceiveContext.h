#pragma once
#ifndef _CTCPCONTEX_H_
#define _CTCPCONTEX_H_

#include "OperateContext.h"
#include "MemoryBlock.h"
#include "ContextStack.h"
#include "BaseDefine.h"


const int DEFAULT_TCP_RECEIVE_CONTEXT_SIZE = 1024; //4 k
const int DEFAULT_TCP_RECEIVE_POOL_SIZE = 256;

//每一个client到server的TCP连接都有一个CTcp对象和他关联，主要负责数据的接收
class CTcpReceiveContext : public COperateContext
{
private:
	CTcpReceiveContext(SOCKET cliSock,long memoryBlockSize = DEFAULT_TCP_RECEIVE_CONTEXT_SIZE);
	~CTcpReceiveContext(void);
public:
    static long GetContextCounter();
	static long GetIdleContextCounter();
public:
	LPWSABUF GetBufAddr(){return &m_pOperateBuffer;}
	void ResetContext();
	static void InitContextPool(long poolSize = DEFAULT_TCP_RECEIVE_POOL_SIZE);
	static CTcpReceiveContext* GetContext(SOCKET clientSock); 
	void ReleaseContext();
	static void DestroyContextPool();
	static void ShowContextIndex();
private:
	WSABUF m_pOperateBuffer; //每次的操作缓冲区
	long m_iMemoryBlockSize;
	CMemoryBlock* m_pDataBuf;
private:
	static bool m_bInitialized;
	static CContextStack* m_pTcpReceiveContextStack;
	static CContextStack* m_pTcpReceiveContextManageStack;
	static CRITICAL_SECTION m_struCriSec;	
};

#endif
