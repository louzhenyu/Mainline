#pragma once

#ifndef _TCPSRVEXCEPTION_H
#define _TCPSRVEXCEPTION_H

#pragma warning( disable : 4290 )  //vc++编译器还不支持异常规范，所以忽略此警告


class CSrvException
{
public:
	CSrvException(void);
	CSrvException(const char* expDescription,int expCode,long expLine = 0);
	~CSrvException(void);
public:
	char* GetExpDescription(){return this->m_strExpDescription;};
	int GetExpCode(){return this->m_iExpCode;};
	long GetExpLine(){return this->m_iExpLineNumber;};
private:
	char m_strExpDescription[512];//异常的文本描述
	int m_iExpCode;//异常代码
	long m_iExpLineNumber;//异常源代码的行数，对调试版本有效
};

#endif
