#pragma once
#ifndef _ACCEP_CONTEXT_H
#define _ACCEP_CONTEXT_H

#include "ContextStack.h"

#include "OperateContext.h"

const long DEFAULT_ACCEPT_CONTEXT_POOL = 256;

//tcp accept operation context
const int ACCEPT_ADDRESS_LENGTH = ((sizeof( struct sockaddr_in) + 16));

class CAcceptContext :public COperateContext
{
private:
	CAcceptContext(int opMode,SOCKET listenSocket,SOCKET clientSocket);
	~CAcceptContext(void);
public:
	SOCKET m_struListenSocket;
	unsigned char m_ucAddressbuf[ACCEPT_ADDRESS_LENGTH*2];

public:
	void SetAcceptParameters(SOCKET listenSocket,SOCKET clientSocket);
	void ResetContext();
	static long GetContextCounter();
	static long GetIdleContextCounter();

public://下面的函数用于进行context pool的管理
	static void InitContextPool(long poolSize = DEFAULT_ACCEPT_CONTEXT_POOL);
	static CAcceptContext* GetContext(); //替代new方案
	void ReleaseContext();//替代delete方案
	static void DestroyContextPool();
	static void ShowContextIndex();
private:
	static bool m_bInitialized;
	static CContextStack* m_pAcceptContextStack; //实际个噢能做
	static CContextStack* m_pAcceptContextManageStack;//用于资源管理
	static CRITICAL_SECTION m_struCriSec;	

};

#endif
