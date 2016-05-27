#include "stdafx.h"
#include "Encrypt.h"
#include "DES.h"
#include <string.h>

Encrypt::Encrypt()
{
	pCip=NULL;
}

Encrypt::~Encrypt()
{
	if (pCip)
	{
		delete[] pCip;
		pCip=NULL;
	}
}

char* Encrypt::encrypt(unsigned char* key, char* data)
{
	int nlen=(strlen(data) / 8 + 1) * 8;

	unsigned char *cip = new unsigned char[nlen];

	BYTE *mcip = new BYTE[nlen];
	memset(mcip,0,nlen);

	DES des;
	bool se = des.CDesEnter((unsigned char*)data,mcip,nlen,key,0);

	base.Encode(mcip,nlen);
	delete[] mcip;

    return (char*)base.EncodedMessage();
};

char* Encrypt::decrypt(unsigned char* key, char* data)
{
	base.Decode(data);
    unsigned char* datares=(unsigned char *)base.DecodedMessage();

	int nlen=(base.m_nDDataLen / 8 + 1) * 8;

	unsigned char *cip = new unsigned char[nlen];
	memset(cip,0x00,nlen);
	memcpy(cip,datares,nlen);

	pCip = new BYTE[nlen];
	memset(pCip,0x00,nlen);
	
	DES des;	
	bool se = des.CDesEnter(cip,pCip,nlen,key,1);
	
	pCip[base.m_nDDataLen]=0x00;

	delete[] cip;

	return (char*)pCip;
};
