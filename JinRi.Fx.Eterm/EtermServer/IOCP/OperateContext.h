#pragma once

#ifndef _OPERATE_CONTEXT_H
#define _OPERATE_CONTEXT_H

#include <winsock2.h>
#include "BaseDefine.h"

const int DESTROY_ALL_CONTEXT = 0xffffffff;

class COperateContext
{
public:
	COperateContext(void);
	~COperateContext(void);
//真正运行数据
public:
	WSAOVERLAPPED m_struOperateOl;//peer handle overlapped struct
	SOCKET m_hSocket; //client socket
	int m_iOperateMode ;//current context operation mode
    long m_iContextIndex;
//下面的代码都用于资源管理及提高利用率
public:
	virtual void ResetContext(){;}
	virtual void ReleaseContext(){;}
	//下面的函数在继承类中独立实现
	//static void InitContextPool(long poolSize)
	//static COperateContext* GetContext();
	//static void DestroyContextPool();
	//static long GetContextCounter();
};

#endif
