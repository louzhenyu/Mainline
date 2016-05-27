#include "stdafx.h"
#include ".\operatecontext.h"

COperateContext::COperateContext(void)
{
    ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	m_hSocket = NULL;
    m_iOperateMode = -1;
	m_iContextIndex = 0;
}

COperateContext::~COperateContext(void)
{
	ZeroMemory(&m_struOperateOl,sizeof(WSAOVERLAPPED));
	m_hSocket = NULL;
    m_iOperateMode = -1;
	m_iContextIndex = 0;
}
