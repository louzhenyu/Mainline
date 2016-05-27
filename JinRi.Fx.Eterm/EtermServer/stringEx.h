#pragma once
#include <string>
#include <vector>

using namespace std;

class stringEx
{
public:
	stringEx(void);
	~stringEx(void);
	vector<wstring> FindStrs(wstring str, wstring sf, wstring sr = _T(""));
	vector<wstring> split(wstring str, wstring toke, wstring srep = _T(""));
	wstring replace(wstring str, wstring sf, wstring sr);
	string replace(string str, string sf, string sr);
	std::wstring MBytesToWString(const char* lpcszString);
	std::string WStringToMBytes(const wchar_t* lpwcszWString);
	std::wstring UTF8ToWString(const char* lpcszString);
	std::string WStringToUTF8(const wchar_t* lpwcszWString);

	wstring FindStr(wstring str, wstring sf, wstring sr = _T(""), bool blast = false);
	string FindStr(string str, string sf, string sr = "", bool blast = false);
	wstring substring(wstring str, wstring first, wstring end);
	bool match(wstring str, wstring sf);
	wstring GetParam(wstring& url, wstring name,bool breplace=false);
	wstring trim(wstring str);
	string trim(string str);
};

