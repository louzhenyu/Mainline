#pragma once
#ifndef _UDP_CONTEXT_
#define _UDP_CONTEXT_

#include "operatecontext.h"
#include "BaseDefine.h"
#include "ContextStack.h"

const int MAX_UDP_CONTEXT_BUFSIZE = 4*1024; //4K
const int DEFAULT_UDP_POOL_SIZE = 256;

class CUdpContext :public COperateContext
{
private:
	CUdpContext();
	~CUdpContext(void);
public:
	long SetSendParameters(struct sockaddr_in& remoteAddr,char* dataAddr,unsigned long transferByte);
	virtual void ResetContext();
	LPWSABUF GetSendBuf(){return &m_struRevBuf;}
public:
	struct sockaddr_in m_struRemoteAddr;
	WSABUF m_struRevBuf;

public:
	static long GetContextCounter();
	static long GetIdleContextCounter();

public://下面的函数用于进行context pool的管理
	static void InitContextPool(long poolSize = DEFAULT_UDP_POOL_SIZE);
	static CUdpContext* GetContext(SOCKET socket,int operateMode); //替代new方案
	void ReleaseContext();//替代delete方案
	static void DestroyContextPool();
	static void ShowContextIndex();
private:
	static bool m_bInitialized;
	static CContextStack* m_pUdpContextStack; //实际个噢能做
	static CContextStack* m_pUdpManageContextStack;//用于资源管理
	static CRITICAL_SECTION m_struCriSec;	
};
#endif