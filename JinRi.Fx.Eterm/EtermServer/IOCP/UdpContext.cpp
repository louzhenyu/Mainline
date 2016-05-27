#include "stdafx.h"
#include ".\udpcontext.h"

bool CUdpContext::m_bInitialized = false;
CRITICAL_SECTION CUdpContext::m_struCriSec = {0};
CContextStack* CUdpContext::m_pUdpContextStack = NULL;
CContextStack* CUdpContext::m_pUdpManageContextStack = NULL;
CUdpContext::CUdpContext()
{
   m_iOperateMode  = SC_WAIT_RECEIVE;
   m_hSocket = NULL;
   m_struOperateOl.hEvent = NULL; 
   ZeroMemory(&m_struRemoteAddr,sizeof(struct sockaddr));
   m_struRevBuf.buf = new char[MAX_UDP_CONTEXT_BUFSIZE];
   m_struRevBuf.len = MAX_UDP_CONTEXT_BUFSIZE;
}

CUdpContext::~CUdpContext(void)
{
   m_iOperateMode  = SC_WAIT_RECEIVE;
   m_hSocket = NULL;
   ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
   ZeroMemory(&m_struRemoteAddr,sizeof(struct sockaddr));
   if(m_struRevBuf.buf)
   {
       delete []m_struRevBuf.buf;
   }
   m_struRevBuf.buf = NULL;
   m_struRevBuf.len = 0;
}
//设定发送参数,同时返回剩余的字节数
//返回0表示发送完成，返回传入的字节数表示发送失败
long CUdpContext::SetSendParameters(struct sockaddr_in& addr,char* dataAddr,unsigned long transferByte)
{
	m_struRemoteAddr = addr;
	unsigned long tranSize = transferByte;
	if(m_struRevBuf.buf)
	{
		if(transferByte !=0 && dataAddr)
		{
			if(transferByte <=m_struRevBuf.len)
				tranSize = transferByte;
			else
				tranSize = m_struRevBuf.len;

			m_struRevBuf.len = tranSize;
			memcpy(m_struRevBuf.buf,dataAddr,tranSize);
			tranSize = transferByte - tranSize;
		}
	}
	return tranSize;
}
void CUdpContext::ResetContext()
{
	m_iOperateMode  = SC_WAIT_RECEIVE;
	m_hSocket = NULL;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	ZeroMemory(&m_struRemoteAddr,sizeof(struct sockaddr));

	ZeroMemory(m_struRevBuf.buf,sizeof(char)*MAX_UDP_CONTEXT_BUFSIZE);
	m_struRevBuf.len = MAX_UDP_CONTEXT_BUFSIZE;
}
void CUdpContext::InitContextPool(long poolSize/* = DEFAULT_ACCEPT_CONTEXT_POOL*/)
{
	if(m_bInitialized)
		return ;

	m_pUdpContextStack = new CContextStack();
	m_pUdpManageContextStack  = new CContextStack();

	if(m_pUdpContextStack == NULL)
		return ;

	InitializeCriticalSection(&m_struCriSec);
    CUdpContext* pContext = NULL;
	for(int i=0;i<poolSize;i++)
	{
		pContext = new CUdpContext();
		if(pContext)
		{
			m_pUdpContextStack->Push(pContext);//链接对象入栈
			m_pUdpManageContextStack->Push(pContext);	
			pContext->m_iContextIndex = m_pUdpManageContextStack->Size();
		}
	}
    m_bInitialized = true;
}
CUdpContext* CUdpContext::GetContext(SOCKET clientSocket,int operateMode)
{
	if(!m_bInitialized)
		return 0;

	CUdpContext* pContext = NULL;
	EnterCriticalSection(&m_struCriSec);
	if(!m_pUdpContextStack->IsEmpty())
	{
		pContext = (CUdpContext*)m_pUdpContextStack->Pop();
	}
	else
	{
					
		pContext = new CUdpContext();
		
		pContext->m_iContextIndex = m_pUdpManageContextStack->Size();
		m_pUdpManageContextStack->Push(pContext);
	}
	pContext->m_iOperateMode = operateMode;
	pContext->m_hSocket = clientSocket;
	LeaveCriticalSection(&m_struCriSec);

	//此处有可能会出现线程同步问题
	return pContext;
}
void CUdpContext::ReleaseContext()
{
	if(this)
	{
		EnterCriticalSection(&m_struCriSec);
        m_pUdpContextStack->Push(this);
		LeaveCriticalSection(&m_struCriSec);
	}
}
//销毁整个链接池context
void CUdpContext::DestroyContextPool()
{
	CUdpContext* pContext = NULL;
	int size = m_pUdpContextStack->Size();
	for(int i=0;i<size;i++)
	{
		pContext = (CUdpContext*)m_pUdpManageContextStack->Pop();
		if(pContext)
		{
			delete pContext;
			pContext = NULL;
		}
	}
	DeleteCriticalSection(&m_struCriSec);

	delete m_pUdpContextStack;
	m_pUdpContextStack = NULL;
	delete m_pUdpManageContextStack;
	m_pUdpManageContextStack=NULL;
}
//得到当前accept用到的context总数量
long CUdpContext::GetContextCounter()
{
	long poolSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolSize = m_pUdpManageContextStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolSize;
}
//得到当前处于空闲状态的accept context数量
long CUdpContext::GetIdleContextCounter()
{
	long poolIdleSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolIdleSize = m_pUdpContextStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolIdleSize;
}
void CUdpContext::ShowContextIndex()
{
	EnterCriticalSection(&m_struCriSec);
    m_pUdpContextStack->ShowIndex();
	cout<<endl;
	LeaveCriticalSection(&m_struCriSec);
}