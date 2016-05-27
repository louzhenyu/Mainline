#include "stdafx.h"
#include ".\tcpreceivecontext.h"

bool CTcpReceiveContext::m_bInitialized=false;
CContextStack* CTcpReceiveContext::m_pTcpReceiveContextStack=NULL;
CContextStack* CTcpReceiveContext::m_pTcpReceiveContextManageStack=NULL;
CRITICAL_SECTION CTcpReceiveContext::m_struCriSec={0};

CTcpReceiveContext::CTcpReceiveContext(SOCKET cliSock,long memoryBlockSize/* = DEFAULT_TCP_RECEIVE_CONTEXT_SIZE*/)
{
	this->m_hSocket = cliSock;
	this->m_iOperateMode = SC_WAIT_RECEIVE;
	ZeroMemory(&m_pOperateBuffer,sizeof(WSABUF));
	m_iMemoryBlockSize = memoryBlockSize<DEFAULT_TCP_RECEIVE_CONTEXT_SIZE?DEFAULT_TCP_RECEIVE_CONTEXT_SIZE:memoryBlockSize;
	m_pDataBuf = new CMemoryBlock(m_iMemoryBlockSize);//
	if(m_pDataBuf)
	{
		m_pOperateBuffer.buf = m_pDataBuf->m_pMemoryBlock;
		m_pOperateBuffer.len = m_iMemoryBlockSize;
	}
}

CTcpReceiveContext::~CTcpReceiveContext(void)
{
    this->m_hSocket = NULL;
	this->m_iOperateMode = SC_WAIT_RECEIVE;
	ZeroMemory(&m_pOperateBuffer,sizeof(WSABUF));
	m_iMemoryBlockSize = 0;

    delete m_pDataBuf;
	m_pDataBuf = NULL;
}
void CTcpReceiveContext::InitContextPool(long poolSize/* = DEFAULT_TCP_RECEIVE_POOL_SIZE*/)
{
	if(m_bInitialized)
		return ;

	m_pTcpReceiveContextStack = new CContextStack();
	m_pTcpReceiveContextManageStack  = new CContextStack();

	if(m_pTcpReceiveContextStack == NULL || m_pTcpReceiveContextManageStack==NULL)
		return ;

	InitializeCriticalSection(&m_struCriSec);
    CTcpReceiveContext* pContext = NULL;
	for(int i=0;i<poolSize;i++)
	{
		pContext = new CTcpReceiveContext(NULL);
		if(pContext)
		{
			m_pTcpReceiveContextStack->Push(pContext);
			m_pTcpReceiveContextManageStack->Push(pContext);

			pContext->m_iContextIndex = m_pTcpReceiveContextManageStack->Size();
		}
	}
    m_bInitialized = true;
}
//此函数的性能将直接关系到整个服务器的性能
CTcpReceiveContext* CTcpReceiveContext::GetContext(SOCKET clientSock)
{
	if(!m_bInitialized)
		return 0;

	CTcpReceiveContext* pContext = NULL;
	EnterCriticalSection(&m_struCriSec);
	if(!m_pTcpReceiveContextStack->IsEmpty())
	{
		pContext = (CTcpReceiveContext*)m_pTcpReceiveContextStack->Pop();
	}
	else
	{
		pContext = new CTcpReceiveContext(NULL);

		pContext->m_iContextIndex = m_pTcpReceiveContextManageStack->Size();
		m_pTcpReceiveContextManageStack->Push(pContext);
	}
	LeaveCriticalSection(&m_struCriSec);

	//此处有可能会出现线程同步问题
	pContext->m_hSocket = clientSock;
	return pContext;
}
void CTcpReceiveContext::ReleaseContext()
{
	if(!m_bInitialized)
		return ;

	EnterCriticalSection(&m_struCriSec);
	m_pTcpReceiveContextStack->Push(this);
	LeaveCriticalSection(&m_struCriSec);
}
void CTcpReceiveContext::ResetContext()
{
	m_pDataBuf->ResetBlock();
	if(m_pDataBuf)
	{
		m_pOperateBuffer.buf = m_pDataBuf->m_pMemoryBlock;
		m_pOperateBuffer.len = m_iMemoryBlockSize;
	}
}
void CTcpReceiveContext::DestroyContextPool()
{
	if(!m_bInitialized)
		return ;

	CTcpReceiveContext* pContext = NULL;
	int size = m_pTcpReceiveContextStack->Size();
	for(int i=0;i<size;i++)
	{
		pContext = (CTcpReceiveContext*)m_pTcpReceiveContextManageStack->Pop();
		if(pContext)
		{
			delete pContext;
			pContext = NULL;
		}
	}
	DeleteCriticalSection(&m_struCriSec);

	delete m_pTcpReceiveContextStack;
	m_pTcpReceiveContextStack = NULL;
	delete m_pTcpReceiveContextManageStack;
	m_pTcpReceiveContextManageStack=NULL;
}
//得到pool 中context总数
long CTcpReceiveContext::GetContextCounter()
{
	if(!m_bInitialized)
		return 0;

	long poolSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolSize = m_pTcpReceiveContextManageStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolSize;
}
//得到pool中空闲context的数量
long CTcpReceiveContext::GetIdleContextCounter()
{
	if(!m_bInitialized)
		return 0;

	long poolIdleSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolIdleSize = m_pTcpReceiveContextStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolIdleSize;
}
void CTcpReceiveContext::ShowContextIndex()
{
	EnterCriticalSection(&m_struCriSec);
	m_pTcpReceiveContextManageStack->ShowIndex();
	cout<<endl<<endl;
    m_pTcpReceiveContextStack->ShowIndex();
	cout<<endl;
	LeaveCriticalSection(&m_struCriSec);
}