#pragma once
class CSocketHelper
{
public:
	CSocketHelper(void);
	~CSocketHelper(void);
	char* GetMAC();
	CString GetIP(CString DomailName);
	char* GetHostIP();
	bool IsDomailName(const char* szName);
private:
	CString DomailToIP(const char* name);
};

