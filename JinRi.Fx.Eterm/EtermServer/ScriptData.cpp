#include "stdafx.h"
#include "ScriptData.h"
#include "RxADO.h"
#include "Global.h"
#include "Script.h"

ScriptData::ScriptData()
{
}


ScriptData::~ScriptData()
{
}

void ScriptData::LoadScripts(void)
{
	try
	{
		Global::scripts.clear();

		TCHAR szSql[] = _T("SELECT method,script FROM dbo.EtermScripts WITH(NOLOCK)");

		RxADO ado;
		_RecordsetPtr rst = ado.GetRecordSet(Global::szConnectString, szSql);
		while (!rst->adoEOF)
		{
			_variant_t varMethod = rst->GetCollect("method");
			_variant_t varScript = rst->GetCollect("script");

			CScript script;
			script.method = varMethod.bstrVal;
			script.script = varScript.bstrVal;

			Global::scripts.insert(make_pair(script.method, script.script));

			rst->MoveNext();
		}
	}
	catch (_com_error err)
	{
		Global::WriteLog(CLog(err.ErrorMessage()));
	}
}