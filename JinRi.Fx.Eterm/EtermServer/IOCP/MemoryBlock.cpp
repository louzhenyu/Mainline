#include "stdafx.h"
#include ".\memoryblock.h"

CMemoryBlock::CMemoryBlock(long size/*= MIN_MEMORYBLOCK_SIZE*/)
{
	//size建议是内存分配粒度的公倍数或倍数关系，涉及到内存对齐问题
	if(size<MIN_MEMORYBLOCK_SIZE)
		size = MIN_MEMORYBLOCK_SIZE;

	m_iBufferSize = size;
	m_pMemoryBlock = new char[size];
}

CMemoryBlock::~CMemoryBlock(void)
{
  delete []m_pMemoryBlock;
  m_pMemoryBlock = NULL;
}
