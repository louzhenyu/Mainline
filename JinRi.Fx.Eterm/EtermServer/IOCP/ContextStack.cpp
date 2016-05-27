#include "stdafx.h"
#include ".\contextstack.h"

CContextStack::CContextStack(long stackCapacity)
{
   m_iMaxStackCapacity = stackCapacity;
   m_iStackSize = -1;
   m_struStackUnit = new STACK_ARRAY[m_iMaxStackCapacity];
}

CContextStack::~CContextStack(void)
{
	delete []m_struStackUnit;
    m_struStackUnit = NULL;
	m_iStackSize = -1;
}
bool CContextStack::Push(COperateContext* pContext)
{
	if(!pContext || (unsigned long)pContext==0xffffffff)
		return false;

	if(m_iStackSize+1>=m_iMaxStackCapacity)//Õ»Âú
		return false;

	m_struStackUnit[++m_iStackSize].pContext = pContext;
	return true;
}
COperateContext* CContextStack::Pop()
{
	COperateContext* rc=NULL;
	if(m_iStackSize>=0)//Õ»·Ç¿Õ
	{  
		rc = m_struStackUnit[m_iStackSize--].pContext;
	}
	return rc;
}
bool CContextStack::IsEmpty()
{
	return m_iStackSize<0?true:false;
}
bool CContextStack::IsFull()
{
	if(m_iStackSize+1==m_iMaxStackCapacity)
		 return true;
	else
		 return false;
}
long CContextStack::Size()
{
    return m_iStackSize+1;
}
void CContextStack::ShowIndex()
{
	for(int i=0;i<Size();i++)
		cout<<" "<<m_struStackUnit[i].pContext->m_iContextIndex;
}