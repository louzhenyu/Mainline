#include "stdafx.h"
#include "ConfigData.h"
#include "Global.h"

CConfigData::CConfigData()
{
	CString strPath;
	strPath.Format(_T("%s\\eTermAccounts.db"), Global::szAppPath);
	m_pDB.open(strPath);
}


CConfigData::~CConfigData()
{
	m_pDB.close();
}

vector<CString> CConfigData::LoadServer(void)
{
	vector<CString> Servers;

	CppSQLite3Query query = m_pDB.execQuery(_T("SELECT server FROM Config GROUP BY server"));

	while (!query.eof())
	{
		Servers.push_back(query.getStringField(_T("server")));
		query.nextRow();
	}
	
	return Servers;
}

vector<Config*> CConfigData::LoadConfig(void)
{
	vector<Config*> configs;

	CppSQLite3Query query = m_pDB.execQuery(_T("SELECT username,password,type,server,port,ssl,sitext,autosi,interval,maxcount,count,KeepAlive FROM Config"));

	while (!query.eof())
	{
		Config* cfg = new Config;

		cfg->UserName = query.getStringField(_T("username"));
		cfg->PassWord = query.getStringField(_T("password"));
		cfg->Type = query.getStringField(_T("type"));
		cfg->ServerIP = query.getStringField(_T("server"));
		cfg->Port = query.getIntField(_T("port"));
		cfg->IsSSL = query.getIntField(_T("ssl"));
		cfg->SI = query.getStringField(_T("sitext"));
		cfg->AutoSI = query.getIntField(_T("autosi"));
		cfg->Interval = query.getIntField(_T("interval"));
		cfg->MaxCount = query.getIntField(_T("maxcount"));
		cfg->Count = query.getIntField(_T("count"));
		cfg->KeepAlive = query.getIntField(_T("KeepAlive"));

		configs.push_back(cfg);

		query.nextRow();
	}

	return configs;
}

bool CConfigData::AppendConfig(Config cfg)
{
	CString szSql;
	szSql.Format(
		_T("INSERT INTO Config(username,password,server,port,ssl,type,sitext,autosi,interval,maxcount,count,KeepAlive) VALUES('%s','%s','%s',%d,%d,'%s','%s',%d,%d,%d,%d,%d)"),
		cfg.UserName,
		cfg.PassWord,
		cfg.ServerIP,
		cfg.Port,
		cfg.IsSSL,
		cfg.Type,
		cfg.SI,
		cfg.AutoSI,
		cfg.Interval,
		cfg.MaxCount,
		cfg.Count,
		cfg.KeepAlive
		);

	return m_pDB.execDML(szSql) == 0;

}

bool CConfigData::UpdateConfig(Config cfg)
{
	CString szSql;
	szSql.Format(
		_T("UPDATE Config SET password='%s',server='%s',port=%d,ssl=%d,type='%s',sitext='%s',autosi=%d,interval=%d,maxcount=%d,count=%d,KeepAlive=%d WHERE username='%s'"),
		cfg.PassWord,
		cfg.ServerIP,
		cfg.Port,
		cfg.IsSSL,
		cfg.Type,
		cfg.SI,
		cfg.AutoSI,
		cfg.Interval,
		cfg.MaxCount,
		cfg.Count,
		cfg.KeepAlive,
		cfg.UserName);

	return m_pDB.execDML(szSql) == 0;
}

bool CConfigData::DeleteConfig(CString szConfig)
{
	CString szSql;
	szSql.Format(_T("DELETE FROM Config WHERE username='%s'"), szConfig);

	return m_pDB.execDML(szSql) == 0;
}

bool CConfigData::UpdateUseCount(CString szConfig, int count)
{
	CString szSql;
	szSql.Format(_T("UPDATE Config SET count=%d WHERE username='%s'"), count, szConfig);

	return m_pDB.execDML(szSql) == 0;
}

bool CConfigData::CreateConfig()
{
	CString sql =
		_T("CREATE TABLE Config(")
		_T("username VARCHAR(50) PRIMARY KEY,")
		_T("password VARCHAR(50),")
		_T("type VARCHAR(10),")
		_T("server VARCHAR(20),")
		_T("port INT,")
		_T("ssl BIT,")
		_T("sitext VARCHAR(50),")
		_T("autosi BIT,")
		_T("interval INT,")
		_T("maxcount INT,")
		_T("count INT,")
		_T("KeepAlive BIT")
		_T(");");

	return m_pDB.execDML(sql) == 0;
}