#include "stdafx.h"
#include "DateTime.h"


DateTime::DateTime()
{
	m_szTime[0] = 0;
	m_szTimeA[0] = 0;
	SYSTEMTIME st;
	GetLocalTime(&st);
	m_tt = SystemTimeToTimet(st);
}


DateTime::~DateTime()
{
}

DateTime::DateTime(_variant_t var)
{
	m_szTime[0] = 0;
	m_szTimeA[0] = 0;
	if (var.vt == VT_DATE)
	{
		SYSTEMTIME st;
		if (VariantTimeToSystemTime(var.date, &st))
		{
			m_tt = SystemTimeToTimet(st);
		}
	}

}

DateTime::DateTime(int nYear, int nMonth, int nDay, int nHour, int nMin, int nSec, int nMil)
{
	m_szTime[0] = 0;
	m_szTimeA[0] = 0;
	SYSTEMTIME st;
	st.wYear = nYear;
	st.wMonth = nMonth;
	st.wDay = nDay;
	st.wHour = nHour;
	st.wMinute = nMin;
	st.wSecond = nSec;
	st.wMilliseconds = nMil;
	m_tt = SystemTimeToTimet(st);
}

DateTime::DateTime(const TCHAR* szTime)
{
	int nYear = 0;
	int nMonth = 0;
	int nDay = 0;
	int nHour = 0;
	int nMin = 0;
	int nSec = 0;
	swscanf_s(szTime, _T("%04d-%02d-%02d %02d:%02d:%02d"), &nYear, &nMonth, &nDay, &nHour, &nMin, &nSec);
	m_szTime[0] = 0;
	m_szTimeA[0] = 0;
	SYSTEMTIME st;
	st.wYear = nYear;
	st.wMonth = nMonth;
	st.wDay = nDay;
	st.wHour = nHour;
	st.wMinute = nMin;
	st.wSecond = nSec;
	st.wMilliseconds = 0;
	m_tt = SystemTimeToTimet(st);
}

DateTime::DateTime(const char* szTime)
{
	int nYear = 0;
	int nMonth = 0;
	int nDay = 0;
	int nHour = 0;
	int nMin = 0;
	int nSec = 0;
	sscanf_s(szTime, "%04d-%02d-%02d %02d:%02d:%02d", &nYear, &nMonth, &nDay, &nHour, &nMin, &nSec);
	m_szTime[0] = 0;
	m_szTimeA[0] = 0;
	SYSTEMTIME st;
	st.wYear = nYear;
	st.wMonth = nMonth;
	st.wDay = nDay;
	st.wHour = nHour;
	st.wMinute = nMin;
	st.wSecond = nSec;
	st.wMilliseconds = 0;
	m_tt = SystemTimeToTimet(st);
}

/*
**SYSTEMTIMEתtime_t
*/
__time64_t DateTime::SystemTimeToTimet(SYSTEMTIME st)
{
	FILETIME ft;
	SystemTimeToFileTime(&st, &ft);
	LONGLONG nLL;
	ULARGE_INTEGER ui;
	ui.LowPart = ft.dwLowDateTime;
	ui.HighPart = ft.dwHighDateTime;
	nLL = (ft.dwHighDateTime << 32) + ft.dwLowDateTime;
	__time64_t pt = ((LONGLONG)(ui.QuadPart - 116444736000000000) / 10000000);
	return pt;
}

TCHAR* DateTime::ToString(const TCHAR* szFormat)
{
	struct tm _tm;
	errno_t no = _gmtime64_s(&_tm, &m_tt);
	if (!no)
	{
		_tcsftime(m_szTime, sizeof(m_szTime) / sizeof(m_szTime[0]), szFormat, &_tm);
	}
	return m_szTime;
}

char* DateTime::ToStringA(const char* szFormat)
{
	struct tm _tm;
	errno_t no = _gmtime64_s(&_tm, &m_tt);
	if (!no)
	{
	   
		strftime(m_szTimeA, sizeof(m_szTimeA) / sizeof(m_szTimeA[0]), szFormat, &_tm);
	}
	return m_szTimeA;
}

unsigned int DateTime::GetWeek()
{
	struct tm _tm;
	errno_t no = _gmtime64_s(&_tm, &m_tt);
	if (!no)
	{
		return _tm.tm_wday == 0 ? 7 : _tm.tm_wday;
	}
	return 0;
}
