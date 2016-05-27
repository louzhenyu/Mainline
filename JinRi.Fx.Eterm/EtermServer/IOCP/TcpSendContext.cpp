#include "stdafx.h"
#include ".\tcpsendcontext.h"

bool CTcpSendContext::m_bInitialized = false;
CContextStack* CTcpSendContext::m_pTcpSendContextStack = NULL;
CContextStack* CTcpSendContext::m_pTcpSendContextManageStack =NULL;
CRITICAL_SECTION CTcpSendContext::m_struCriSec = {0};

CTcpSendContext::CTcpSendContext(SOCKET cliSock,int operateMode/* = SC_WAIT_TRANSMIT*/)
{
	m_iOperateMode = operateMode;
	m_hSocket = cliSock;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	ZeroMemory(&m_struSendBuf,sizeof(WSABUF));
	m_pSendDataBuf = new CMemoryBlock(DEFAULT_TCP_SEND_CONTEXT_SIZE);
}

CTcpSendContext::~CTcpSendContext(void)
{
	m_iOperateMode = 0;
	m_hSocket = 0;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	ZeroMemory(&m_struSendBuf,sizeof(WSABUF));
	delete m_pSendDataBuf;
	m_pSendDataBuf = NULL;

}

//设定发送参数,同时返回剩余的字节数
//返回0表示发送完成，返回传入的字节数表示发送失败
long CTcpSendContext::SetSendParameters(SOCKET clientSock,const char* dataAddr,unsigned long transferByte)
{
	m_hSocket = clientSock;	
	unsigned long tranSize = transferByte;
	if(m_pSendDataBuf)
	{
		if(transferByte !=0 && dataAddr)
		{
			if(transferByte <=DEFAULT_TCP_SEND_CONTEXT_SIZE)
				tranSize = transferByte;
			else
				tranSize = DEFAULT_TCP_SEND_CONTEXT_SIZE;
	        
			memcpy(m_pSendDataBuf->m_pMemoryBlock,dataAddr,tranSize);

			m_struSendBuf.buf = m_pSendDataBuf->m_pMemoryBlock;
			m_struSendBuf.len = transferByte;

            tranSize = transferByte - tranSize;
		}
	}
	return tranSize;
}
void CTcpSendContext::ResetContext()
{
    m_iOperateMode = SC_WAIT_TRANSMIT;
	m_hSocket = NULL;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	m_pSendDataBuf->ResetBlock();
}
void CTcpSendContext::InitContextPool(long poolSize/* = DEFAULT_TCPSEND_CONTEXT_POOL_SIZE*/)
{
	if(m_bInitialized)
		return ;

	m_pTcpSendContextStack = new CContextStack();
	m_pTcpSendContextManageStack  = new CContextStack();

	if(m_pTcpSendContextStack == NULL || m_pTcpSendContextManageStack==NULL)
		return ;

	InitializeCriticalSection(&m_struCriSec);
    CTcpSendContext* pContext = NULL;
	for(int i=0;i<poolSize;i++)
	{
		pContext = new CTcpSendContext(NULL,SC_WAIT_TRANSMIT);
		if(pContext)//传送对象入栈
		{
			m_pTcpSendContextStack->Push(pContext);
			m_pTcpSendContextManageStack->Push(pContext);
		}
	}
    m_bInitialized = true;
}
CTcpSendContext* CTcpSendContext::GetContext()
{
	if(!m_bInitialized)
		return 0;

	CTcpSendContext* pContext = NULL;
	EnterCriticalSection(&m_struCriSec);
	if(!m_pTcpSendContextStack->IsEmpty())
	{
		pContext = (CTcpSendContext*)m_pTcpSendContextStack->Pop();
	}
	else
	{
		pContext = new CTcpSendContext(NULL,SC_WAIT_TRANSMIT);
		m_pTcpSendContextManageStack->Push(pContext);
	}
	LeaveCriticalSection(&m_struCriSec);

	//此处有可能会出现线程同步问题
	pContext->m_iOperateMode = SC_WAIT_TRANSMIT;
	return pContext;
}
void CTcpSendContext::ReleaseContext()
{
	if(this)
	{
		EnterCriticalSection(&m_struCriSec);
		this->ResetContext();
        m_pTcpSendContextStack->Push(this);
		LeaveCriticalSection(&m_struCriSec);
	}
}
void CTcpSendContext::DestroyContextPool()
{
	CTcpSendContext* pContext = NULL;
	int size = m_pTcpSendContextStack->Size();
	for(int i=0;i<size;i++)
	{
		pContext = (CTcpSendContext*)m_pTcpSendContextManageStack->Pop();
		if(pContext)
		{
			delete pContext;
			pContext = NULL;
		}
	}
	DeleteCriticalSection(&m_struCriSec);

	delete m_pTcpSendContextStack;
	m_pTcpSendContextStack = NULL;
	delete m_pTcpSendContextManageStack;
	m_pTcpSendContextManageStack=NULL;
}
long CTcpSendContext::GetContextCounter()
{
	long poolSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolSize = m_pTcpSendContextManageStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolSize;
}
//得到pool中空闲context的数量
long CTcpSendContext::GetIdleContextCounter()
{
	long poolIdleSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolIdleSize = m_pTcpSendContextStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolIdleSize;
}
