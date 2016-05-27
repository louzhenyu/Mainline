#pragma once

#include "operatecontext.h"
#include "MemoryBlock.h"
#include "ContextStack.h"

const long DEFAULT_TCPSEND_CONTEXT_POOL_SIZE = 256;//默认发送context pool的最大数，视并发量及主机性能调整次参数
const long DEFAULT_TCP_SEND_CONTEXT_SIZE = 10240*4;//默认的每一次传输最大字节数

class CTcpSendContext :public COperateContext
{
private:
	CTcpSendContext(SOCKET cliSock,int operateMode = SC_WAIT_TRANSMIT);
	~CTcpSendContext(void);
private:
	CMemoryBlock* m_pSendDataBuf;
public:
//	TRANSMIT_FILE_BUFFERS m_struTransmitFileBuf;
	WSABUF m_struSendBuf;
public:
	long SetSendParameters(SOCKET cliScok,const char* dataAddr,unsigned long transferByte);
	void ResetContext();
	static long GetContextCounter();
	static long GetIdleContextCounter();
	long GetContextSize(){return DEFAULT_TCP_SEND_CONTEXT_SIZE;}

public://下面的函数用于进行context pool的管理
	static void InitContextPool(long poolSize = DEFAULT_TCPSEND_CONTEXT_POOL_SIZE);
	static CTcpSendContext* GetContext(); 
	void ReleaseContext();
	static void DestroyContextPool();
private:
	static bool m_bInitialized;
	static CContextStack* m_pTcpSendContextStack;
	static CContextStack* m_pTcpSendContextManageStack;
	static CRITICAL_SECTION m_struCriSec;	
};
