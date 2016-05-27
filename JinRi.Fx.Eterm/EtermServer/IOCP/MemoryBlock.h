#pragma once
#ifndef _CMEMEORY_BLOCK_H
#define _CMEMEORY_BLOCK_H

#include "BaseDefine.h"

const int MIN_MEMORYBLOCK_SIZE = 4096;
class CMemoryBlock
{
public:
	CMemoryBlock(long size = MIN_MEMORYBLOCK_SIZE);
	~CMemoryBlock(void);
public:
	inline long GetSize(){return m_iBufferSize;}
	inline void ResetBlock(){ZeroMemory(m_pMemoryBlock,m_iBufferSize);}
public:
	long m_iBufferSize;  
	char* m_pMemoryBlock;
};
#endif
