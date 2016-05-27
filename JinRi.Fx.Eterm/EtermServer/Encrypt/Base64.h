// Base64.h

#include <memory.h>

class CBase64 
{
    // Internal bucket class.
    class TempBucket
    {
    public:
		unsigned char         nData[4];
		unsigned char         nSize;
		void         Clear() { memset(nData,0,4); nSize = 0; };
    };
public:
	unsigned char*                       m_pDBuffer;
	unsigned char*                       m_pEBuffer;
    long                       m_nDBufLen;
	long                       m_nEBufLen;
	long                       m_nDDataLen;
	long                       m_nEDataLen;

public:
    CBase64();
    virtual ~CBase64();

public:
	virtual void         Encode(const unsigned char*, long);
	virtual void         Decode(const unsigned char*, long);
	virtual void         Encode(const char* sMessage);
	virtual void         Decode(const char* sMessage);

	virtual const char*     DecodedMessage() const;
	virtual const char*     EncodedMessage() const;

	virtual void         AllocEncode(long);
	virtual void         AllocDecode(long);
	virtual void         SetEncodeBuffer(const unsigned char* pBuffer, long nBufLen);
	virtual void         SetDecodeBuffer(const unsigned char* pBuffer, long nBufLen);

protected:
	virtual void         _EncodeToBuffer(const TempBucket &Decode, unsigned char* pBuffer);
	virtual long         _DecodeToBuffer(const TempBucket &Decode, unsigned char* pBuffer);
    virtual void         _EncodeRaw(TempBucket &, const TempBucket &);
    virtual void         _DecodeRaw(TempBucket &, const TempBucket &);
	virtual int         _IsBadMimeChar(unsigned char);

    static char         m_DecodeTable[256];
	static int          m_Init;
    void                _Init();
};