#pragma once

class CPrinterDecrypt
{
public:
	CPrinterDecrypt(void);
	~CPrinterDecrypt(void);
	CString Decrypt(CString strInput);
	bool ConvertXmlToHtml(CString xml,CString fileXSLName,CString& strRet);
	void AppendInfo(wstring& strData);
private:
	BYTE* sub_00401920(UINT iHi, UINT iLow);
	UINT sub_00401B00(UINT idata);
	UINT sub_0042e720(UINT idata, int ibits);
	int ByteArrayToInt(BYTE* buffer, int nStartIndex);
	BYTE* IntToByteArray(int iValue);
	long ByteArrayToLong(BYTE* buffer, int nStartIndex);
	BYTE* LongToByteArray(long lValue);
	void LongToIntArray(long lValue, int* buffer, int nStartIndex);
	long MakeLong(int nLo, int nHi);
};
