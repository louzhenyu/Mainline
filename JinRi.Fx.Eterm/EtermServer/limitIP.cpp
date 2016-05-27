#include "stdafx.h"
#include "limitIP.h"
#include "Global.h"

limitIP::limitIP()
{
	CString strPath;
	strPath.Format(_T("%s\\eTermAccounts.db"), Global::szAppPath);
	m_pDB.open(strPath);
}


limitIP::~limitIP()
{
	m_pDB.close();
}

bool limitIP::AppendIP(CString szIP, CString szRemark)
{
	CString szSql;
	szSql.Format(_T("INSERT INTO IPfliter(IP,Remark) VALUES('%s','%s')"),
		szIP,
		szRemark);

	return m_pDB.execDML(szSql) == 0;
}

bool limitIP::DeleteIP(CString szIP)
{
	CString szSql;
	szSql.Format(_T("DELETE FROM IPfliter WHERE IP='%s'"),szIP);

	return m_pDB.execDML(szSql) == 0;
}

bool limitIP::UpdateIP(CString szIP, CString szRemark)
{
	CString szSql;
	szSql.Format(_T("UPDATE IPfliter SET Remark='%s' WHERE IP='%s'"),szRemark, szIP);
	return m_pDB.execDML(szSql) == 0;
}

vector<lip> limitIP::GetIPList()
{
	vector<lip> lips;
	__try
	{

	CppSQLite3Query query = m_pDB.execQuery(_T("SELECT IP,Remark FROM IPfliter"));

	while (!query.eof())
	{
		lip l;
		l.szIP = query.getStringField(_T("IP"));
		l.remark = query.getStringField(_T("Remark"));
		lips.push_back(l);

		query.nextRow();
	}

	return lips;
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return lips;
	}
}