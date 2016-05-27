#pragma once
#include <list>
#include <map>
#include "EtermPacket.h"

enum ETERM_STATE
{
	NONE,							//未知错误
	NO_FIND_CONFIG,					//没有发现可用配置
	CONFIG_INVALID,					//配置不可用
	CONFIG_LOCKED,					//当前配置已锁定
	STRING_FORMAT,					//wstring变量申请错误
	INT_FORMAT,						//int变量申请错误
	FLOAT_FORMAT,					//float变量申请错误
	LIST_FORMAT,					//list变量申请错误
	SET_VALUE,						//设置变量失败
	INVALID_BUSINESS,				//无效的业务名称
	NONE_TYPE,						//未知的类型
	FUN_SUC,						//函数调用成功
	SEND_FAIL,						//发送Eterm命令失败
	SUCCESS							//成功		
};

struct EtermDATA
{
	EtermDATA(wstring _cmd,wstring _rec)
	{
		cmd = _cmd;
		rec = _rec;
		time = CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S");
	}
	wstring time;    //时间  
	wstring cmd;		//发送
	wstring rec;		//接收	
};

struct PARAM
{
	PARAM()
	{
		ival = 0;
		fval = 0;
		type = 0;
	}
	wstring key;
	wstring sval;
	int     ival;
	float   fval;
	vector<wstring> lval;
	int     type;			//1 wstring 2int 3float 4list<wstring>
};

struct FUN
{
	vector<wstring> sources;
	vector<PARAM> params;		
};

struct FOR
{
	FOR()
	{		
		bif = false;		
		ns = 0;
		ne = 0;
	}	
	int ns;
	int ne;
	bool bif;
	wstring step;
	wstring sif;
	wstring sinit;
};

class CEtermCommand
{
public:
	CEtermCommand(void);
	~CEtermCommand(void);
	std::map<wstring,wstring> stringMap;
	std::map<wstring,int>    intMap;
	std::map<wstring,float>  floatMap;	
	std::map<wstring, vector<wstring>> listMap;
	std::map<wstring, FUN> funMap;
	wstring strDATA;
	ETERM_STATE ScanSourceCode(wstring sourceCode);
	bool syntaxcheck(wstring sline,wstring type);
	bool syntaxcheck(wstring& strErr);
	bool analyseIF(wstring sline);
	void InputSourceCode(const TCHAR* szFile);

	wstring m_sret;			//返回数据
	wstring m_config;		//配置
	SOCKET m_sock;			//客户端
private:
	bool Judge(wstring left,wstring right,int bEqual);	//== != > <
	bool InitConfig(TCHAR* szConfig);
	void SetValue(wstring left,wstring right);
	wstring get_system(wstring sline, int& nwait);
	wstring get_return(wstring sline);
	void ret(wstring sline);
	wstring FindStr(wstring sitem);
	wstring Replace(wstring sline);
	wstring Format(wstring sline);
	wstring getparams(wstring szData);
	vector<wstring> getParams(wstring sline);
	vector<wstring> FindStrs(wstring sitem);
	wstring SubStr(wstring sitem);
	FUN InitFun(wstring sline, vector<wstring> codelist, int& i);
	vector<wstring> codelist;	
	vector<EtermDATA> etermData;
	map<wstring,int> steplist;

	bool SetVal(wstring left, wstring right, wstring sline, vector<wstring> codelist, int i);

	int intValue(wstring right);
	float floatValue(wstring right);

	ETERM_STATE function(FUN fun, int type, wstring& sret, int& iret, float& fret, vector<wstring>& listret);
	wstring SetintVal(wstring sline);
	bool Using(wstring sline);
	bool Invoke(wstring sline);
	CMD_TYPE cmdType(wstring& cmd,bool& bEn);
	//int maxStep;
	int curStep;
	map<int,FOR> m_fors;
	wstring GetListValue(wstring sline);
};

