#pragma once
#include <string>

#import "msado15.dll" rename ("EOF", "adoEOF") rename ("BOF","adoBOF")
#import "msjro.dll" rename("ReplicaTypeEnum", "_ReplicaTypeEnum") 

using namespace ADODB;

class RxADO
{
public:
	RxADO(void);
	~RxADO(void);
public:
	void ExitConnect();	
	bool InitADOConn(TCHAR* strConnection);
	//记录集
	_RecordsetPtr pRecordset;
	// 执行查询指令并返回记录集 
	_RecordsetPtr& GetRecordSet(TCHAR* strConnection, TCHAR* bstrSQL);
	//执行 Command
	_RecordsetPtr& ExecuteCommand(TCHAR* strConnection, TCHAR* bstrSQL);
	// 执行SQL命令 
	BOOL ExecuteSQL(TCHAR* strConnection, TCHAR* bstrSQL);
	//获取连接指针
	_ConnectionPtr GetConnection(){return m_pConnection;}
public:
	//static CString VariantToCString(VARIANT var);
	double VariantToDouble(VARIANT var); 
	std::wstring DateTimeToString(VARIANT var,TCHAR* szFormat);
private:	
	// 指向Connection对象的指针: 
	_ConnectionPtr m_pConnection; 
	// 指向Recordset对象的指针: 
	_RecordsetPtr m_pRecordset; 
	//指向Command对象的指针
	_CommandPtr m_pCommand;
};
