////////////////////////////////////

//CBase64.cpp
// CBase64.cpp: implementation of the CBase64 class.
//
//////////////////////////////////////////////////////////////////////
#include "stdafx.h"
#include "Base64.h"
#include <string.h>

// Digits...
static char Base64Digits[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

int CBase64::m_Init   = 0;
char CBase64::m_DecodeTable[256];

#ifndef PAGESIZE
#define PAGESIZE       4096
#endif

#ifndef ROUNDTOPAGE
#define ROUNDTOPAGE(a)     (((a/4096)+1)*4096)
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CBase64::CBase64()
: m_pDBuffer(NULL),
m_pEBuffer(NULL),
m_nDBufLen(0),
m_nEBufLen(0)
{

}

CBase64::~CBase64()
{
if(m_pDBuffer != NULL)
  delete [] m_pDBuffer;

if(m_pEBuffer != NULL)
  delete [] m_pEBuffer;
}

const char * CBase64::DecodedMessage() const
{ 
	return (const char *)m_pDBuffer;
}

const char * CBase64::EncodedMessage() const
{ 
	return (const char *)m_pEBuffer;
}

void CBase64::AllocEncode(long nSize)
{
if(m_nEBufLen < nSize)
{
  if(m_pEBuffer != NULL)
    delete [] m_pEBuffer;
  
  m_nEBufLen = ROUNDTOPAGE(nSize);
  m_pEBuffer = new unsigned char[m_nEBufLen];
}

  memset(m_pEBuffer,0, m_nEBufLen);
m_nEDataLen = 0;
}

void CBase64::AllocDecode(long nSize)
{
if(m_nDBufLen < nSize)
{
  if(m_pDBuffer != NULL)
    delete [] m_pDBuffer;
  
  m_nDBufLen = ROUNDTOPAGE(nSize);
  m_pDBuffer = new unsigned char[m_nDBufLen];
}

memset(m_pDBuffer,0, m_nDBufLen);
m_nDDataLen = 0;
}

void CBase64::SetEncodeBuffer(const unsigned char* pBuffer, long nBufLen)
{
long ii = 0;

AllocEncode(nBufLen);
while(ii < nBufLen)
{
  if(!_IsBadMimeChar(pBuffer[ii]))
  {
    m_pEBuffer[m_nEDataLen] = pBuffer[ii];
    m_nEDataLen++;
  }
  
  ii++;
}
}

void CBase64::SetDecodeBuffer(const unsigned char* pBuffer, long nBufLen)
{
AllocDecode(nBufLen);
memcpy(m_pDBuffer, pBuffer, nBufLen);
m_nDDataLen = nBufLen;
}

void CBase64::Encode(const unsigned char* pBuffer, long nBufLen)
{
SetDecodeBuffer(pBuffer, nBufLen);
AllocEncode(nBufLen * 2);

TempBucket     Raw;
long       nIndex = 0;

while((nIndex + 3) <= nBufLen)
{
  Raw.Clear();
  memcpy(&Raw, m_pDBuffer + nIndex, 3);
  Raw.nSize = 3;
  _EncodeToBuffer(Raw, m_pEBuffer + m_nEDataLen);
  nIndex   += 3;
  m_nEDataLen += 4;
}

if(nBufLen > nIndex)
{
  Raw.Clear();
  Raw.nSize = (unsigned char)(nBufLen - nIndex);
  memcpy(&Raw, m_pDBuffer + nIndex, nBufLen - nIndex);
  _EncodeToBuffer(Raw, m_pEBuffer + m_nEDataLen);
  m_nEDataLen += 4;
}
}

void CBase64::Encode(const char * szMessage)
{
if(szMessage != NULL)
CBase64::Encode((const unsigned char*)szMessage, strlen(szMessage));
}

void CBase64::Decode(const unsigned char* pBuffer, long dwBufLen)
{
if(!CBase64::m_Init)
  _Init();

SetEncodeBuffer(pBuffer, dwBufLen);

AllocDecode(dwBufLen);

TempBucket     Raw;

long   nIndex = 0;

while((nIndex + 4) <= m_nEDataLen)
{
  Raw.Clear();
  Raw.nData[0] = CBase64::m_DecodeTable[m_pEBuffer[nIndex]];
  Raw.nData[1] = CBase64::m_DecodeTable[m_pEBuffer[nIndex + 1]];
  Raw.nData[2] = CBase64::m_DecodeTable[m_pEBuffer[nIndex + 2]];
  Raw.nData[3] = CBase64::m_DecodeTable[m_pEBuffer[nIndex + 3]];
  
  if(Raw.nData[2] == 255)
    Raw.nData[2] = 0;
  if(Raw.nData[3] == 255)
    Raw.nData[3] = 0;
  
  Raw.nSize = 4;
  _DecodeToBuffer(Raw, m_pDBuffer + m_nDDataLen);
  nIndex += 4;
  m_nDDataLen += 3;
}

// If nIndex < m_nEDataLen, then we got a decode message without padding.
// We may want to throw some kind of warning here, but we are still required
// to handle the decoding as if it was properly padded.
if(nIndex < m_nEDataLen)
{
  Raw.Clear();
  for(long ii = nIndex; ii < m_nEDataLen; ii++)
  {
    Raw.nData[ii - nIndex] = CBase64::m_DecodeTable[m_pEBuffer[ii]];
    Raw.nSize++;
    if(Raw.nData[ii - nIndex] == 255)
    Raw.nData[ii - nIndex] = 0;
  }
  
  _DecodeToBuffer(Raw, m_pDBuffer + m_nDDataLen);
  m_nDDataLen += (m_nEDataLen - nIndex);
}
}

void CBase64::Decode(const char * szMessage)
{
if(szMessage != NULL)
CBase64::Decode((const unsigned char*)szMessage, strlen(szMessage));
}

long CBase64::_DecodeToBuffer(const TempBucket &Decode, unsigned char* pBuffer)
{
TempBucket Data;
long   nCount = 0;

_DecodeRaw(Data, Decode);

for(int ii = 0; ii < 3; ii++)
{
  pBuffer[ii] = Data.nData[ii];
  if(pBuffer[ii] != 255)
    nCount++;
}

return nCount;
}


void CBase64::_EncodeToBuffer(const TempBucket &Decode, unsigned char* pBuffer)
{
TempBucket Data;

_EncodeRaw(Data, Decode);

for(int ii = 0; ii < 4; ii++)
  pBuffer[ii] = Base64Digits[Data.nData[ii]];

switch(Decode.nSize)
{
case 1:
  pBuffer[2] = '=';
case 2:
  pBuffer[3] = '=';
}
}

void CBase64::_DecodeRaw(TempBucket &Data, const TempBucket &Decode)
{
	unsigned char   nTemp;

	Data.nData[0] = Decode.nData[0];
	Data.nData[0] <<= 2;

	nTemp = Decode.nData[1];
	nTemp >>= 4;
	nTemp &= 0x03;
	Data.nData[0] |= nTemp;

	Data.nData[1] = Decode.nData[1];
	Data.nData[1] <<= 4;

	nTemp = Decode.nData[2];
	nTemp >>= 2;
	nTemp &= 0x0F;
	Data.nData[1] |= nTemp;

	Data.nData[2] = Decode.nData[2];
	Data.nData[2] <<= 6;
	nTemp = Decode.nData[3];
	nTemp &= 0x3F;
	Data.nData[2] |= nTemp;
}

void CBase64::_EncodeRaw(TempBucket &Data, const TempBucket &Decode)
{
	unsigned char   nTemp;

	Data.nData[0] = Decode.nData[0];
	Data.nData[0] >>= 2;

	Data.nData[1] = Decode.nData[0];
	Data.nData[1] <<= 4;
	nTemp = Decode.nData[1];
	nTemp >>= 4;
	Data.nData[1] |= nTemp;
	Data.nData[1] &= 0x3F;

	Data.nData[2] = Decode.nData[1];
	Data.nData[2] <<= 2;

	nTemp = Decode.nData[2];
	nTemp >>= 6;

	Data.nData[2] |= nTemp;
	Data.nData[2] &= 0x3F;

	Data.nData[3] = Decode.nData[2];
	Data.nData[3] &= 0x3F;
}

int CBase64::_IsBadMimeChar(unsigned char nData)
{
	switch(nData)
	{
	case '\r': case '\n': case '\t': case ' ' :
	case '\b': case '\a': case '\f': case '\v':
	  return 1;
	default:
	  return 0;
}
}

void CBase64::_Init()
{ // Initialize Decoding table.

int ii;

for(ii = 0; ii < 256; ii++)
  CBase64::m_DecodeTable[ii] = -2;

for(ii = 0; ii < 64; ii++)
{
  CBase64::m_DecodeTable[Base64Digits[ii]]   = (char)ii;
  CBase64::m_DecodeTable[Base64Digits[ii]|0x80] = (char)ii;
}

CBase64::m_DecodeTable['=']     = -1;
CBase64::m_DecodeTable['='|0x80]   = -1;

CBase64::m_Init = 1;
}
