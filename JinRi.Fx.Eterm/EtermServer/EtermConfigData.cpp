#include "stdafx.h"
#include "EtermConfigData.h"
#include "RxADO.h"
#include "Global.h"
#include "json\json.h"
#include <algorithm>
#include "stringEx.h"

EtermConfigData::EtermConfigData()
{
}


EtermConfigData::~EtermConfigData()
{
}

EtermConfig* EtermConfigData::GetConfig(CString ServerUrl)
{
	EtermConfig* config=NULL;

	try
	{
		RxADO ado;
		TCHAR szSql[1024] = { 0 };
		_stprintf_s(szSql, _T("SELECT ServerUrl,OfficeNo,ConfigType,ConfigState,AllowAirLine,DenyAirLine,ConfigLevel,ConfigList FROM dbo.EtermConfig WITH(NOLOCK) WHERE ServerUrl='%s'"), ServerUrl);
		_RecordsetPtr rst = ado.GetRecordSet(Global::szConnectString, szSql);
		
		_variant_t ServerUrl = rst->GetCollect("ServerUrl");
		_variant_t OfficeNo = rst->GetCollect("OfficeNo");
		_variant_t Types = rst->GetCollect("ConfigType");
		_variant_t State = rst->GetCollect("ConfigState");
		_variant_t AllowAirLine = rst->GetCollect("AllowAirLine");
		_variant_t DenyAirLine = rst->GetCollect("DenyAirLine");
		_variant_t ConfigLevel = rst->GetCollect("ConfigLevel");
		_variant_t ConfigList = rst->GetCollect("ConfigList");

		config = new EtermConfig;
		config->ServerUrl = ServerUrl.bstrVal;
		config->OfficeNo = OfficeNo.bstrVal;
		config->Types = Types.bstrVal;
		config->State = State.intVal;
		config->AllowAirLine = AllowAirLine.bstrVal;
		config->DenyAirLine = DenyAirLine.bstrVal;
		config->ConfigLevel = ConfigLevel.intVal;
		config->ConfigList = ConfigList.bstrVal;
	}
	catch (_com_error err)
	{
		Global::WriteLog(CLog(err.ErrorMessage()));
	}
	return config;
}

bool EtermConfigData::AppendConfig(EtermConfig config)
{
	try
	{
		RxADO ado;
		TCHAR szSql[4096] = { 0 };
		_stprintf_s(szSql,
			_T("INSERT INTO dbo.EtermConfig (ServerUrl,OfficeNo,ConfigType,ConfigState,AllowAirLine,DenyAirLine,ConfigLevel,OperDate,ConfigList) VALUES")
			_T("('%s','%s','%s',%d,'%s','%s',%d,GETDATE(),'%s')"),
			config.ServerUrl, config.OfficeNo, config.Types, config.State, config.AllowAirLine, config.DenyAirLine, config.ConfigLevel,config.ConfigList);

		return ado.ExecuteSQL(Global::szConnectString, szSql)>0;
	}
	catch (_com_error err)
	{
		Global::WriteLog(CLog(err.ErrorMessage()));
	}
}

bool EtermConfigData::AppendBooking(BookData book)
{
	try
	{
		RxADO ado;
		TCHAR szSql[80960] = { 0 };
		_stprintf_s(szSql,
			_T("INSERT INTO dbo.Booking (aircom,flightno,pnr,request,response,[guid],md5,scity,ecity,cabin,sdate,edate,config,officeno,serverip,booktime) VALUES")
			_T("('%s','%s','%s','%s','%s','%s','%s','%s','%s','%s','%s','%s','%s','%s','%s',GETDATE())"),
			book.aircom,book.flightno,book.pnr,book.request,book.response,book.guid,book.md5,book.scity,book.ecity,book.cabin,book.sdate,book.edate,book.config,book.officeno,book.serverip);

		return ado.ExecuteSQL(Global::szConnectString, szSql)>0;
	}
	catch (_com_error err)
	{
		Global::WriteLog(CLog(err.ErrorMessage()));
	}
}

bool EtermConfigData::UpdateConfig(EtermConfig config)
{
	try
	{
		RxADO ado;
		TCHAR szSql[4096] = { 0 };
		_stprintf_s(szSql,
			_T("UPDATE dbo.EtermConfig SET ")			
			_T("OfficeNo='%s',")
			_T("ConfigType='%s',")
			_T("ConfigState=%d,")
			_T("AllowAirLine='%s',")
			_T("DenyAirLine='%s',")
			_T("ConfigLevel=%d,")
			_T("ConfigList='%s',")
			_T("OperDate=GETDATE() WHERE ServerUrl='%s'"),
			config.OfficeNo, config.Types, config.State, config.AllowAirLine, config.DenyAirLine, config.ConfigLevel,config.ConfigList, config.ServerUrl);

		return ado.ExecuteSQL(Global::szConnectString, szSql)>0;
	}
	catch (_com_error err)
	{
		Global::WriteLog(CLog(err.ErrorMessage()));
	}
}

bool EtermConfigData::UpdateConfig(CString ServerUrl, int State)
{
	try
	{
		RxADO ado;
		TCHAR szSql[1024] = { 0 };
		_stprintf_s(szSql,
			_T("UPDATE dbo.EtermConfig SET ")
			_T("State=%d,")			
			_T("OperDate=GETDATE() WHERE ServerUrl='%s'"),
			State,ServerUrl);

		return ado.ExecuteSQL(Global::szConnectString, szSql)>0;
	}
	catch (_com_error err)
	{
		Global::WriteLog(CLog(err.ErrorMessage()));
	}
}

map<CString, EtermConfig> EtermConfigData::GetEtermConfig(std::string str)
{
	map<CString, EtermConfig> vec;

	if (str.empty())
	{
		RxADO ado;
		TCHAR szSql[] = _T("SELECT ServerUrl,OfficeNo,ConfigType,ConfigState,AllowAirLine,DenyAirLine,ConfigLevel,ConfigList FROM dbo.EtermConfig WITH(NOLOCK) WHERE ConfigState=0");
		_RecordsetPtr rst = ado.GetRecordSet(Global::szConnectString, szSql);

		while (!rst->adoEOF)
		{

			_variant_t ServerUrl = rst->GetCollect("ServerUrl");
			_variant_t OfficeNo = rst->GetCollect("OfficeNo");
			_variant_t Types = rst->GetCollect("ConfigType");
			_variant_t State = rst->GetCollect("ConfigState");
			_variant_t AllowAirLine = rst->GetCollect("AllowAirLine");
			_variant_t DenyAirLine = rst->GetCollect("DenyAirLine");
			_variant_t ConfigLevel = rst->GetCollect("ConfigLevel");
			_variant_t ConfigList = rst->GetCollect("ConfigList");

			EtermConfig config;
			config.ServerUrl = ServerUrl.bstrVal;
			config.OfficeNo = OfficeNo.bstrVal;
			config.Types = Types.bstrVal;
			config.State = State.intVal;
			config.AllowAirLine = AllowAirLine.bstrVal;
			config.DenyAirLine = DenyAirLine.bstrVal;
			config.ConfigLevel = ConfigLevel.intVal;
			config.ConfigList = ConfigList.bstrVal;
			
			vec.insert(make_pair(config.ServerUrl, config));

			rst->MoveNext();
		}
	}
	else
	{
		Json::Value root;
		Json::Reader reader;
		if (reader.parse(str, root, false))
		{
			for (auto it = root.begin(); it != root.end(); it++)
			{
				EtermConfig ec;

				ec.State = (*it)["State"].asString() == "connect" ? 0 : 1;
				ec.ConfigLevel = (*it)["ConfigLevel"].asInt();
				ec.OfficeNo = CA2T((*it)["OfficeNo"].asCString());
				ec.ServerUrl = CA2T((*it)["ServerUrl"].asCString());
				Json::Value cmdType = (*it)["cmdType"];
				for (auto ait = cmdType.begin(); ait != cmdType.end(); ait++)
				{
					enums type;
					type.name = CA2T((*ait).asCString());
					auto it = find_if(Global::cmdtypes.begin(), Global::cmdtypes.end(), [=](enums _type){ return _type.name == type.name; });
					if (it != Global::cmdtypes.end())
						ec.Types.AppendFormat(_T("%d,"), it->index);
				}
				Json::Value AllowAirLine = (*it)["AllowAirLine"];
				for (auto ait = AllowAirLine.begin(); ait != AllowAirLine.end(); ait++)
					ec.AllowAirLine.AppendFormat(_T("%s,"), CA2T((*ait).asCString()));
				Json::Value DenyAirLine = (*it)["DenyAirLine"];
				for (auto ait = DenyAirLine.begin(); ait != DenyAirLine.end(); ait++)
					ec.DenyAirLine.AppendFormat(_T("%s,"), CA2T((*ait).asCString()));
				Json::Value ConfigList = (*it)["ConfigList"];
				for (auto ait = ConfigList.begin(); ait != ConfigList.end(); ait++)
					ec.ConfigList.AppendFormat(_T("%s,"), CA2T((*ait).asCString()));

				vec.insert(make_pair(ec.ServerUrl, ec));
			}
		}
	}
	return vec;
}

std::string EtermConfigData::toString(map<CString, EtermConfig> ecs)
{
	Json::Value root;
	stringEx ex;

	for (auto it = ecs.begin(); it != ecs.end(); it++)
	{
		Json::Value ec;

		ec["ServerUrl"] = (const char*)CT2A(it->first);
		ec["OfficeNo"] = (const char*)CT2A(it->second.OfficeNo);
		ec["cmdType"] = Json::Value(Json::arrayValue);
		wstring types = it->second.Types;
		vector<wstring> vec = ex.split(types, _T(","));
		for (int i = 0; i < vec.size(); i++)
		{
			enums type;
			type.index = _ttoi(vec[i].c_str());
			auto it = find_if(Global::cmdtypes.begin(), Global::cmdtypes.end(), [=](enums _type){ return _type.index == type.index; });
			if (it != Global::cmdtypes.end())
			{
				ec["cmdType"].append(Json::Value(CT2A(it->name)));
			}
		}
		ec["State"] = it->second.State == 0 ? "connect" : "suspend";
		ec["ConfigLevel"] = it->second.ConfigLevel;
		ec["AllowAirLine"] = Json::Value(Json::arrayValue);
		wstring AllowAirLine = it->second.AllowAirLine;
		vec = ex.split(AllowAirLine, _T(","));
		for (int i = 0; i < vec.size(); i++)
		{
			if (!vec[i].empty())
				ec["AllowAirLine"].append(Json::Value(CT2A(vec[i].c_str())));
		}
		ec["DenyAirLine"] = Json::Value(Json::arrayValue);
		wstring DenyAirLine = it->second.DenyAirLine;
		vec = ex.split(DenyAirLine, _T(","));
		for (int i = 0; i < vec.size(); i++)
		{
			if (!vec[i].empty())
				ec["DenyAirLine"].append(Json::Value(CT2A(vec[i].c_str())));
		}
		wstring ConfigList = it->second.ConfigList;
		vec = ex.split(ConfigList, _T(","));
		for (int i = 0; i < vec.size(); i++)
		{
			if (!vec[i].empty())
				ec["ConfigList"].append(Json::Value(CT2A(vec[i].c_str())));
		}
		root.append(ec);
	}

	return root.toStyledString();
}