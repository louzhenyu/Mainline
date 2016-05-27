#pragma once
#include <time.h>
#include <comutil.h>

class TimeSpan
{
public:
	TimeSpan()
	{
		day = 0;
		hour = 0;
		min = 0;
		sec = 0;
	}
	TimeSpan(int _day = 0, int _hour = 0, int _min = 0, int _sec = 0)
	{
		day = _day;
		hour = _hour;
		min = _min;
		sec = _sec;
	}
	int day;
	int hour;
	int min;
	int sec;
};

class DateTime
{
public:
	DateTime();
	DateTime(_variant_t var);
	DateTime(int nYear, int nMonth, int nDay, int nHour = 0, int nMin = 0, int nSec = 0, int nMil = 0);
	DateTime(const char* szTime);
	DateTime(const TCHAR* szTime);
	~DateTime();
	TCHAR* ToString(const TCHAR* szFormat = _T("%Y-%m-%d"));
	char* ToStringA(const char* szFormat="%Y-%m-%d %H:%M:%S");
	unsigned int GetWeek();
	bool operator>=(const DateTime dt){ return this->m_tt >= dt.m_tt; }
	bool operator<=(const DateTime dt){ return this->m_tt <= dt.m_tt; }
	bool operator<(const DateTime dt){ return this->m_tt < dt.m_tt; }
	bool operator>(const DateTime dt){ return this->m_tt > dt.m_tt; }
	bool operator==(const DateTime dt){ return this->m_tt == dt.m_tt; }
	DateTime operator+(TimeSpan ts) {
		int ntick = (ts.day * 24 * 3600) + (ts.hour * 3600) + (ts.min * 60) + ts.sec;
		this->m_tt += ntick;
		return *this;
	}
	const DateTime operator-(TimeSpan ts) {
		int ntick = (ts.day * 24 * 3600) + (ts.hour * 3600) + (ts.min * 60) + ts.sec;
		this->m_tt -= ntick;
		return *this;
	}
private:
	__time64_t SystemTimeToTimet(SYSTEMTIME st);
	__time64_t m_tt;
	TCHAR m_szTime[50];
	char m_szTimeA[64];
};

