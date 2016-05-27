#include "stdafx.h"
#include ".\acceptcontext.h"
#include "BaseDefine.h"

bool CAcceptContext::m_bInitialized = false;
CRITICAL_SECTION CAcceptContext::m_struCriSec = {0};
CContextStack* CAcceptContext::m_pAcceptContextStack = NULL;
CContextStack* CAcceptContext::m_pAcceptContextManageStack = NULL;
CAcceptContext::CAcceptContext(int opMode,SOCKET listenSocket,SOCKET clientSocket)
{
	m_iOperateMode = opMode;
	m_struListenSocket = listenSocket;
	m_hSocket = clientSocket;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	ZeroMemory(m_ucAddressbuf,ACCEPT_ADDRESS_LENGTH*2);

}
CAcceptContext::~CAcceptContext(void)
{
    m_iOperateMode = SC_WAIT_ACCEPT;
	m_struListenSocket = NULL;
//	closesocket(m_hSocket);
	m_hSocket = NULL;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	ZeroMemory(m_ucAddressbuf,ACCEPT_ADDRESS_LENGTH*2);
}
void CAcceptContext::SetAcceptParameters(SOCKET listenSocket,SOCKET clientSocket)
{
	m_struListenSocket = listenSocket;
	m_hSocket = clientSocket;
}
void CAcceptContext::ResetContext()
{
    //m_iOperateMode = SC_WAIT_ACCEPT;
	m_struListenSocket = NULL;
	m_hSocket = NULL;
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	ZeroMemory(m_ucAddressbuf,ACCEPT_ADDRESS_LENGTH*2);
}
void CAcceptContext::InitContextPool(long poolSize/* = DEFAULT_ACCEPT_CONTEXT_POOL*/)
{
	if(m_bInitialized)
		return ;

	m_pAcceptContextStack = new CContextStack();
	m_pAcceptContextManageStack  = new CContextStack();

	if(m_pAcceptContextStack == NULL)
		return ;

	InitializeCriticalSection(&m_struCriSec);
    CAcceptContext* pContext = NULL;
	for(int i=0;i<poolSize;i++)
	{
		pContext = new CAcceptContext(SC_WAIT_ACCEPT,NULL,NULL);
		if(pContext)
		{
			m_pAcceptContextStack->Push(pContext);//链接对象入栈
			m_pAcceptContextManageStack->Push(pContext);	
			pContext->m_iContextIndex = m_pAcceptContextManageStack->Size();
		}
	}
    m_bInitialized = true;
}
CAcceptContext* CAcceptContext::GetContext()
{
	if(!m_bInitialized)
		return 0;

	CAcceptContext* pContext = NULL;
	EnterCriticalSection(&m_struCriSec);
	if(!m_pAcceptContextStack->IsEmpty())
	{
		pContext = (CAcceptContext*)m_pAcceptContextStack->Pop();
	}
	else
	{
					
		pContext = new CAcceptContext(SC_WAIT_ACCEPT,NULL,NULL);
		
		pContext->m_iContextIndex = m_pAcceptContextManageStack->Size();
		m_pAcceptContextManageStack->Push(pContext);
	}
	LeaveCriticalSection(&m_struCriSec);

	//此处有可能会出现线程同步问题
	return pContext;
}
void CAcceptContext::ReleaseContext()
{
	if(this)
	{
		EnterCriticalSection(&m_struCriSec);
        m_pAcceptContextStack->Push(this);
		LeaveCriticalSection(&m_struCriSec);
	}
}
//销毁整个链接池context
void CAcceptContext::DestroyContextPool()
{
	CAcceptContext* pContext = NULL;
	int size = m_pAcceptContextStack->Size();
	for(int i=0;i<size;i++)
	{
		pContext = (CAcceptContext*)m_pAcceptContextManageStack->Pop();
		if(pContext)
		{
			delete pContext;
			pContext = NULL;
		}
	}
	DeleteCriticalSection(&m_struCriSec);

	delete m_pAcceptContextStack;
	m_pAcceptContextStack = NULL;
	delete m_pAcceptContextManageStack;
	m_pAcceptContextManageStack=NULL;
}
//得到当前accept用到的context总数量
long CAcceptContext::GetContextCounter()
{
	long poolSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolSize = m_pAcceptContextManageStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolSize;
}
//得到当前处于空闲状态的accept context数量
long CAcceptContext::GetIdleContextCounter()
{
	long poolIdleSize = 0;
	EnterCriticalSection(&m_struCriSec);
	poolIdleSize = m_pAcceptContextStack->Size();
	LeaveCriticalSection(&m_struCriSec);

	return poolIdleSize;
}
void CAcceptContext::ShowContextIndex()
{
	EnterCriticalSection(&m_struCriSec);
    m_pAcceptContextStack->ShowIndex();
	cout<<endl;
	LeaveCriticalSection(&m_struCriSec);
}