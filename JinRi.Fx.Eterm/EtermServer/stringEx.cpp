#include "StdAfx.h"
#include "stringEx.h"
#include <regex>

using namespace std;

stringEx::stringEx(void)
{
}


stringEx::~stringEx(void)
{
}

vector<wstring> stringEx::FindStrs(wstring str,wstring sf,wstring sr)
{
	vector<wstring> vec;
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	tr1::regex_constants::match_flag_type flag = tr1::regex_constants::match_not_null;
	match_results<std::wstring::const_iterator> result;
	wstring::const_iterator begin, end;
	begin=str.begin();
	end=str.end();
	wregex reg(sf,fl);
	wregex regp;
	
	if (!sr.empty())
		regp.assign(sr,fl);

	wstring sret;

	while (regex_search(begin,end,result,reg,flag))
	{
		wstring stemp=wstring(result[0].first,result[0].second);
		
		if (!sr.empty())
			stemp = regex_replace(stemp, regp, wstring(_T("")));
		
		vec.push_back(stemp);

		begin = result[0].second;		
	}

	return vec;
}

vector<wstring> stringEx::split(wstring str,wstring toke,wstring srep)
{
	vector<wstring> vec;

	//”Ôæ‰Ω· ¯±Í÷æ
	wregex reg(toke);
	
	tr1::wsregex_token_iterator it(str.begin(), str.end(), reg, -1);
	tr1::wsregex_token_iterator end;
	wstring code;
	
	while(it!=end)
	{
		code=*it++;
		if (!srep.empty())
			code = replace(code, srep, _T(""));
		vec.push_back(code);
	}

	return vec;
}

wstring stringEx::replace(wstring str,wstring sf,wstring sr)
{
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	wregex reg(sf,fl);
	return tr1::regex_replace(str,reg,sr);
}

string stringEx::replace(string str, string sf, string sr)
{
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	regex reg(sf, fl);
	return tr1::regex_replace(str, reg, sr);
}

std::wstring stringEx::MBytesToWString(const char* lpcszString)
{
    int len = strlen(lpcszString);
    int unicodeLen = ::MultiByteToWideChar(CP_ACP, 0, lpcszString, -1, NULL, 0);
    wchar_t* pUnicode = new wchar_t[unicodeLen + 1];
    memset(pUnicode, 0, (unicodeLen + 1) * sizeof(wchar_t));
    ::MultiByteToWideChar(CP_ACP, 0, lpcszString, -1, (LPWSTR)pUnicode, unicodeLen);
    wstring wstring = (wchar_t*)pUnicode;
    delete [] pUnicode;
    return wstring;
}

std::string stringEx::WStringToMBytes(const wchar_t* lpwcszWString)
{
    char* pElementText;
    int iTextLen;
    // wide char to multi char
    iTextLen = ::WideCharToMultiByte(CP_ACP, 0, lpwcszWString, -1, NULL, 0, NULL, NULL);
    pElementText = new char[iTextLen + 1];
    memset((void*)pElementText, 0, (iTextLen + 1) * sizeof(char));
    ::WideCharToMultiByte(CP_ACP, 0, lpwcszWString, 0, pElementText, iTextLen, NULL, NULL);
    std::string strReturn(pElementText);
    delete [] pElementText;
    return strReturn;
}

std::wstring stringEx::UTF8ToWString(const char* lpcszString)
{
    int len = strlen(lpcszString);
    int unicodeLen = ::MultiByteToWideChar(CP_UTF8, 0, lpcszString, -1, NULL, 0);
    wchar_t* pUnicode;
    pUnicode = new wchar_t[unicodeLen + 1];
    memset((void*)pUnicode, 0, (unicodeLen + 1) * sizeof(wchar_t));
    ::MultiByteToWideChar(CP_UTF8, 0, lpcszString, -1, (LPWSTR)pUnicode, unicodeLen);
    wstring wstrReturn(pUnicode);
    delete [] pUnicode;
    return wstrReturn;
}

std::string stringEx::WStringToUTF8(const wchar_t* lpwcszWString)
{
    char* pElementText;
    int iTextLen = ::WideCharToMultiByte(CP_UTF8, 0, (LPWSTR)lpwcszWString, -1, NULL, 0, NULL, NULL);
    pElementText = new char[iTextLen + 1];
    memset((void*)pElementText, 0, (iTextLen + 1) * sizeof(char));
    ::WideCharToMultiByte(CP_UTF8, 0, (LPWSTR)lpwcszWString, -1, pElementText, iTextLen, NULL, NULL);
    std::string strReturn(pElementText);
    delete [] pElementText;
    return strReturn;
}

wstring stringEx::FindStr(wstring str, wstring sf, wstring sr, bool blast)
{
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	tr1::regex_constants::match_flag_type flag = tr1::regex_constants::match_not_null;
	match_results<std::wstring::const_iterator> result;
	wstring::const_iterator begin, end;
	begin = str.begin();
	end = str.end();
	tr1::wregex reg(sf, fl);
	tr1::wregex rep;
	if (!sr.empty())
		rep.assign(sr, fl);

	wstring sret;

 	while (regex_search(begin, end, result, reg, flag))
	{
		sret = wstring(result[0].first, result[0].second);
		if (!sr.empty())
			sret = regex_replace(sret, rep, wstring(_T("")));
		if (!blast)
			break;
		begin = result[0].second;
	}

	return sret;
}

string stringEx::FindStr(string str, string sf, string sr, bool blast)
{
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	tr1::regex_constants::match_flag_type flag = tr1::regex_constants::match_not_null;
	match_results<std::string::const_iterator> result;
	string::const_iterator begin, end;
	begin = str.begin();
	end = str.end();
	tr1::regex reg(sf, fl);
	tr1::regex rep;
	if (!sr.empty())
		rep.assign(sr, fl);

	string sret;

	while (regex_search(begin, end, result, reg, flag))
	{
		sret = string(result[0].first, result[0].second);
		if (!sr.empty())
			sret = regex_replace(sret, rep, string(""));
		if (!blast)
			break;
		begin = result[0].second;
	}

	return sret;
}

wstring stringEx::substring(wstring str, wstring first, wstring end)
{
	return replace(str, _T("(.*?") + first + _T("(\\s+|))|(") + end + _T(".*)"), _T(""));
}

bool stringEx::match(wstring str, wstring sf)
{
	tr1::regex_constants::match_flag_type flag = tr1::regex_constants::match_not_null;
	std::regex_constants::syntax_option_type fl = std::regex_constants::icase;
	std::tr1::wregex reg(sf,fl);
	return tr1::regex_match(str, reg, flag);
}

wstring stringEx::GetParam(wstring& url, wstring name, bool breplace)
{	
	wstring sret = FindStr(url, _T("(^|&|/)") + name + _T("=.*?(&|$)"), name + _T("=|&|/"));
	if (breplace) url = replace(url, name + _T("=.*?(&|$|/)"), _T(""));
	return sret;
}

wstring stringEx::trim(wstring str)
{
	return replace(str, _T("^\\s+|\\s+$"), _T(""));
}

string stringEx::trim(string str)
{
	regex reg("^\\s+|\\s+$");
	return tr1::regex_replace(str, reg, "");
}