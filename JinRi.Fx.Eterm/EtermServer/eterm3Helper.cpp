#include "stdafx.h"
#include "eterm3Helper.h"
#include "DateTime.h"


bool eterm3Helper::StanDate(CString str, CString stime, CString etime)
{
	if (str.GetLength() != 7 || stime.GetLength() != 4 || etime.GetLength()< 4) return false;

	CString month = str.Right(3);

	if (month == _T("JAN"))
		month = _T("01");
	else if (month == _T("FEB"))
		month = _T("02");
	else if (month == _T("MAR"))
		month = _T("03");
	else if (month == _T("APR"))
		month = _T("04");
	else if (month == _T("MAY"))
		month = _T("05");
	else if (month == _T("JUN"))
		month = _T("06");
	else if (month == _T("JUL"))
		month = _T("07");
	else if (month == _T("AUG"))
		month = _T("08");
	else if (month == _T("SEP"))
		month = _T("09");
	else if (month == _T("OCT"))
		month = _T("10");
	else if (month == _T("NOV"))
		month = _T("11");
	else if (month == _T("DEC"))
		month = _T("12");

	CTime cur = CTime::GetCurrentTime();
	sdate.Format(_T("%04d-%s-%s %s:%s:00"), cur.GetYear(), month, str.Mid(2, 2), stime.Left(2), stime.Right(2));
	edate.Format(_T("%04d-%s-%s %s:%s:00"), cur.GetYear(), month, str.Mid(2, 2), etime.Left(2), etime.Right(2));
	int n = etime.Find(_T("+"));
	if (n != -1)
	{
		DateTime dt(edate);
		dt = dt + TimeSpan(_wtoi(etime.Mid(n + 1)));
		edate = dt.ToString(_T("%Y-%m-%d %H:%M:%S"));
	}
	return true;
}