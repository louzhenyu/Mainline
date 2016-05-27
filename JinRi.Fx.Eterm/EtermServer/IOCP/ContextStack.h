#pragma once

#include "OperateContext.h"

const long MAX_STACK_CAPACITY = 32768;


class CContextStack
{
	struct STACK_ARRAY
	{
		COperateContext* pContext;
	};
public:
	CContextStack(long stackCapacity=MAX_STACK_CAPACITY);
	~CContextStack(void);
public:
	bool Push(COperateContext* pContext);
	COperateContext* Pop();
	bool IsEmpty();
	bool IsFull();
	long Size();
	void ShowIndex();
private:
   long m_iMaxStackCapacity;//栈最大容量
   long m_iStackSize;//当前的栈大小;
   struct STACK_ARRAY* m_struStackUnit;
};
