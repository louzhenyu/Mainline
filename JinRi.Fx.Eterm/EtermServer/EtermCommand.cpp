#include "StdAfx.h"
#include "EtermCommand.h"
#include "stringEx.h"
#include <sstream>
#include <iomanip>
#include "Encrypt/Encrypt.h"
#include "Global.h"
#include <algorithm>
#include "ShowInfo.h"
#include "MainFrm.h"
#include "EtermServerView.h"
#include "EmailHelper.h"

using namespace std;



CEtermCommand::CEtermCommand(void)
{
}



CEtermCommand::~CEtermCommand(void)
{
}

ETERM_STATE CEtermCommand::ScanSourceCode(wstring sourceCode)
{
	__try
	{
		stringEx ex;
		codelist = ex.split(sourceCode, _T("\r\n|\n"));
		list<bool> bElses;
		int fi = 0;

		for (int i = 0; i < codelist.size(); i++)
		{
			wstring sline = ex.replace(codelist[i], _T("^\\s+|\\s+$"), _T(""));
			if (sline.empty()) continue;

			#ifdef DEBUG
			CString strLog;
			strLog.Format(_T("行号：%d 源程序：%s"), i, sline.c_str());	
			Global::WriteLog(CLog(strLog));
			#endif
			if (sline.find(_T("USING:")) == 0)
			{
				if (!Using(sline)) return NO_FIND_CONFIG;
			}
			else if (sline.find(_T("string")) == 0)
			{
				if (!syntaxcheck(sline, _T("string")))
				{
					return STRING_FORMAT;
				}
			}
			else if (sline.find(_T("int")) == 0)
			{
				if (!syntaxcheck(sline, _T("int")))
				{
					return INT_FORMAT;
				}
			}
			else if (sline.find(_T("float")) == 0)
			{
				if (!syntaxcheck(sline, _T("float")))
				{
					return FLOAT_FORMAT;
				}
			}
			else if (sline.find(_T("list<string>")) == 0)
			{
				if (!syntaxcheck(sline, _T("list<string>")))
				{
					return LIST_FORMAT;
				}
			}
			else if (sline.find(_T("config=")) == 0)
			{
				/*if (!InitConfig(sock,(TCHAR*)sline.c_str()))
				{
				Global::webServer.SendMsg(sock, _T("解析配置或登录时出错"));
				return i;
				}*/
			}
			else if (sline.find(_T("if")) == 0)
			{
				if (!analyseIF(sline))
				{
					int c1 = 0, c2 = 0;
					while (i<codelist.size())
					{
						sline = ex.replace(codelist[i++], _T("^\\s+|\\s+$"), _T(""));
						if (sline == _T("{"))
							c1++;
						else if (sline == _T("}"))
							c2++;
						if (c1>0 && c2 == c1)
							break;
					}
					int jj = i; i--;
					while (jj < codelist.size())
					{
						wstring key = ex.replace(codelist[jj], _T("^\\s+|\\s+$"), _T(""));
						if (key == _T("else"))
						{
							bElses.push_back(true);
							break;
						}
						else if (key.empty())
							continue;
						else if (ex.match(key, _T("//.*")))
							continue;
						else
							break;
						jj++;
					}
				}
				else
				{
					int jj = i;

					int c1 = 0, c2 = 0;
					while (jj<codelist.size())
					{
						sline = ex.replace(codelist[jj++], _T("^\\s+|\\s+$"), _T(""));
						if (sline == _T("{"))
							c1++;
						else if (sline == _T("}"))
							c2++;
						if (c1>0 && c2 == c1)
							break;

					}
					while (jj < codelist.size())
					{
						wstring key = ex.replace(codelist[jj], _T("^\\s+|\\s+$"), _T(""));
						if (key == _T("else"))
						{
							bElses.push_back(false);
							break;
						}
						else if (key.empty())
							continue;
						else if (ex.match(key, _T("//.*")))
							continue;
						else
							break;
						jj++;
					}
				}
			}
			else if (sline.find(_T("system")) == 0)
			{
				Invoke(sline);
			}
			else if (sline.find(_T("goto")) == 0)
			{
				wstring key = ex.FindStr(sline, _T("goto.*?;"), _T("goto|;|\\s"), true);
				auto step = steplist.find(key);
				if (step != steplist.end())
				{
					i = step->second;
				}
				else
				{
					bool bfind = false;
					int ii = i;
					while (i < codelist.size())
					{
						sline = ex.replace(codelist[i++], _T("^\\s+|\\s+$"), _T(""));
						if (sline == key + _T(":"))
						{
							if (steplist.find(key) == steplist.end())
								steplist.insert(make_pair(key, i--));
							bfind = true;
							break;
						}
					}
					if (!bfind)
					{
						i = ii;
						while (i > 0)
						{
							sline = ex.replace(codelist[i--], _T("^\\s+|\\s+$"), _T(""));
							if (sline == key + _T(":"))
							{
								if (steplist.find(key) == steplist.end())
									steplist.insert(make_pair(key, i++));
								bfind = true;
								break;
							}
						}
					}
				}
			}
			else if (sline.find(_T("return")) == 0)
			{
				m_sret = get_return(sline);
				return SUCCESS;
			}
			else if (sline.find(_T("ret")) == 0)
			{
				ret(sline);				
			}
			else if (sline.find(_T("else")) == 0 && bElses.size() > 0)
			{
				if (!bElses.back())
				{
					int c1 = 0, c2 = 0;
					while (i<codelist.size())
					{
						sline = ex.replace(codelist[i++], _T("^\\s+|\\s+$"), _T(""));
						if (sline == _T("{"))
							c1++;
						else if (sline == _T("}"))
							c2++;
						if (c1>0 && c2 == c1)
							break;

					}
					i--;
				}
				bElses.pop_back();
			}
			else if (ex.match(sline, _T("^(\\s+|)[A-Za-z][A-Za-z0-9]+?:(\\s+|)$")))
			{
				wstring key = ex.replace(sline, _T("\\s|:"), _T(""));
				if (steplist.find(key) == steplist.end())
					steplist.insert(make_pair(key, i));
			}
			else if (sline.find(_T("Sleep")) == 0)
			{
				int interal = _ttoi(ex.FindStr(sline, _T("\\d+")).c_str());
				Sleep(interal);
			}
			else if (sline.find(_T("for")) == 0)
			{
				auto it = m_fors.find(i);
				if (it == m_fors.end())
				{
					wstring sfor = ex.FindStr(sline, _T("\\(.*?\\)"), _T("\\(|\\)"));
					vector<wstring> fors = ex.split(sfor, _T(";"));
					if (fors.size() == 3)
					{
						FOR _for;
						_for.sinit = SetintVal(fors[0] + _T(";"));
						_for.sif = fors[1];
						_for.step = ex.replace(fors[2], _for.sinit + _T("="), _T(""));
						_for.ns = i;
						int c1 = 0, c2 = 0;
						int ii = i;
						while (i<codelist.size())
						{
							sline = ex.replace(codelist[ii++], _T("^\\s+|\\s+$"), _T(""));
							if (sline == _T("{"))
								c1++;
							else if (sline == _T("}"))
								c2++;
							if (c1>0 && c2 == c1)
								break;
						}
						_for.ne = ii - 1;
						_for.bif = analyseIF(_for.sif);
						if (!_for.bif)
						{
							i = _for.ne;
						}
						else
						{
							m_fors.insert(make_pair(i, _for));
						}						
					}
				}
			}
			else if (sline == _T("}"))
			{
				if (m_fors.size() > 0)
				{
					FOR _for;
					bool bfind = false;
					for (auto it = m_fors.begin(); it != m_fors.end(); it++)
					{
						if (it->second.ne == i)
						{
							_for = it->second;
							bfind = true;
							break;
						}
					}
					if (bfind)
					{
						auto it = intMap.find(_for.sinit);
						if (it != intMap.end())
						{
							it->second = intValue(_for.step);
							_for.bif = analyseIF(_for.sif);
							if (_for.bif)
							{
								i = _for.ns;
							}							
						}
					}
				}
			}
			else if (sline.find(_T("break")) == 0)
			{
				for (auto it = m_fors.begin(); it != m_fors.end(); it++)
				{
					if (i > it->second.ns && it->second.ne > i)
					{
						i = it->second.ne;
						break;
					}
				}
			}
			else if (sline.find(_T("fun_")) == 0)
			{
				wstring skey = ex.FindStr(sline, _T("fun_.*?\\("), _T("\\("));
				auto it = funMap.find(skey);
				if (it == funMap.end())
				{
					funMap.insert(make_pair(skey, InitFun(sline, codelist, i)));
				}
			}
			else
			{
				int isplit = sline.find(_T("="));
				if (isplit > 0)
				{
					wstring left = ex.replace(sline.substr(0, isplit), _T("^\\s+|\\s+$"), _T(""));
					wstring right = ex.replace(sline.substr(isplit + 1), _T("^\\s+|\\s+$|;"), _T(""));

					if (!SetVal(left, right, sline, codelist, i)) return SET_VALUE;
				}
				else if (sline.find(_T("++")) != wstring::npos)
				{
					wstring left = ex.FindStr(sline, _T("^.*?\\+\\+"), _T("\\+\\+|\\s"));
					wstring right = ex.FindStr(sline, _T("\\+\\+.*"), _T("\\s|;"));
					if (!SetVal(left, right, sline, codelist, i)) return SET_VALUE;
				}
			}
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}
	return NONE;
}

wstring CEtermCommand::FindStr(wstring sitem)
{
	__try
	{

		wstring str;
		stringEx ex;
		sitem = ex.substring(sitem, _T("FindStr(\\s+|)\\("), _T("\\)$"));
		vector<wstring> items = getParams(sitem);
		wstring strfind, strsear, strrep;
		bool brep = false;
		bool blast = false;
		for (int i = 0; i < items.size(); i++)
		{
			if (i == 0)
			{
				strfind = items[i];
			}
			else if (i == 1)
			{
				strsear = items[i];
			}
			else if (i == 2)
			{
				strrep = items[i];
			}
			else if (i == 3)
			{
				blast = items[i] == _T("true");
			}
		}
		return ex.FindStr(strfind, strsear, strrep, blast);
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return _T("");
	}
}

vector<wstring> CEtermCommand::FindStrs(wstring sitem)
{
	wstring str;
	stringEx ex;
	sitem = ex.substring(sitem, _T("FindStrs(\\s+|)\\("), _T("\\)$"));
	vector<wstring> items = getParams(sitem);
	wstring strfind, strsear, strrep;
	bool brep = false;
	bool blast = false;
	for (int i = 0; i<items.size(); i++)
	{
		if (i == 0)
		{
			strfind = items[i];
		}
		else if (i == 1)
		{
			strsear = items[i];
		}
		else if (i == 2)
		{
			strrep = items[i];
		}		
	}
	return ex.FindStrs(strfind, strsear, strrep);
}

wstring CEtermCommand::SubStr(wstring sitem)
{
	wstring str;
	stringEx ex;
	sitem=ex.substring(sitem,_T("SubStr(\\s+|)\\("),_T("\\)$"));
	vector<wstring> items=ex.split(sitem,_T(","));
	wstring strfind;
	int nf=0;
	int nlen=0;
	for(int i=0;i<items.size();i++)
	{
		wstring sstr = ex.replace(items[i],_T("^\\s|\\s$"),_T(""));
		if (i==0)
		{
			if (sstr==_T("DATA"))
			{
				if (etermData.size()>0)
				{
					strfind=etermData[etermData.size()-1].rec;
				}
			}
			else if (ex.match(sstr,_T("^\".*?\"$")))
			    strfind=ex.replace(sstr,_T("^\"|\"$"),_T(""));
			else 
			{
				auto sfit=stringMap.find(sstr);
				if (sfit!=stringMap.end())
					strfind=sfit->second;
			}
		}
		else if (i==1)
		{
			if (ex.match(sstr,_T("\\d+")))
			    nf=_ttoi(sstr.c_str());
			else
			{
				wstring skey=ex.replace(sstr,_T("^\\s+|\\s+$"),_T(""));
				auto iit=intMap.find(skey);
				if (iit!=intMap.end())
				{
					nf=iit->second;
				}
				
			}
		}
		else if (i==2)
		{
			if (ex.match(sstr,_T("\\d+")))
			    nlen=_ttoi(sstr.c_str());
			else
			{
				wstring skey=ex.replace(sstr,_T("^\\s+|\\s+$"),_T(""));
				auto iit=intMap.find(skey);
				if (iit!=intMap.end())
				{
					nlen=iit->second;
				}
				
			}
		}
	}

	if (!strfind.empty())
	{
		if (nf>=0&&nf<strfind.length()&&nf+nlen<=strfind.length())
		{
			return strfind.substr(nf,nlen);
		}
	}

	return _T("");
}

void CEtermCommand::SetValue(wstring left,wstring right)
{
	stringEx ex;
	auto sit = stringMap.find(left);
	auto iit = intMap.find(left);
	auto fit = floatMap.find(left);
	auto lit = listMap.find(left);

	CString strLog(_T("结果："));

	if (sit!=stringMap.end())
	{		
		sit->second = getparams(right.c_str());

		strLog.AppendFormat(_T("%s"), sit->second.c_str());
	}
	else if (iit!=intMap.end())
	{
		if (ex.match(right, _T("^#[A-Za-z]+(\\d+|)$")))
		{
			wstring key = ex.replace(right, _T("#|\\s"), _T(""));
			auto llit = listMap.find(key);
			if (llit != listMap.end())
			{
				iit->second = llit->second.size();
			}
		}
		else if (ex.match(right, _T("^atoi\\([A-Za-z][A-Za-z0-9_]+?\\)$")))
		{
			wstring key = ex.substring(right, _T("\\("), _T("\\)"));
			auto siit = stringMap.find(key);
			if (siit != stringMap.end())
			{
				iit->second = _ttoi(siit->second.c_str());
			}
		}
		else if (right.find(_T("system(")) == 0)
		{			
			iit->second = Invoke(right) ? etermData.size() > 0 ? etermData[etermData.size() - 1].rec.length() : -1 : -1;
		}
		else
		{
			if (right == _T("++"))
				iit->second ++;
			else
				iit->second = intValue(right);
		}
		strLog.AppendFormat(_T("%d"), iit->second);
	}
	else if (fit!=floatMap.end())
	{
		
		if (ex.match(right, _T("^atof\\([A-Za-z][A-Za-z0-9_]+?\\)$")))
		{
			wstring key = ex.substring(right, _T("\\("), _T("\\)"));
			auto siit = stringMap.find(key);
			if (siit != stringMap.end())
			{
				fit->second = _ttof(siit->second.c_str());
			}
		}
		else
		{
			fit->second = floatValue(right);
		}		
		strLog.AppendFormat(_T("%0.2f"), fit->second);
	}
	else if (lit != listMap.end())
	{
		vector<wstring> vec;
		if (ex.match(right, _T("^(\\s*|)\\{.*?\\}(\\s*|)$")))
		{
			right = ex.replace(right, _T("^(\\s*|)\\{|\\}(\\s*|)$"), _T(""));
			vec = ex.split(right, _T(","), _T("^\"|\"$"));
		}
		else if (ex.match(right, _T("^(\\s*|)FindStrs\\(.*?\\)(\\s*|)$")))
		{
			vec=FindStrs(right);
		}
		else
		{
			right = ex.replace(right, _T("^\\s+|\\s+$"),_T(""));
			auto rlit = listMap.find(right);
			if (rlit != listMap.end())
			{
				vec = rlit->second;
			}
		}
		lit->second = vec;

		for (int i = 0; i < vec.size();i++)
		strLog.AppendFormat(_T("%s\r\n"), vec[i].c_str());
	}
	//strLog.AppendFormat(_T("赋值 左值:%s\r\n右值:%s"), left.c_str(), right.c_str());
	//Global::WriteLog(CLog(strLog));
}

int CEtermCommand::intValue(wstring right)
{
	int ival = 0;
	wstring stemp;
	int sign = 0;
	for (int i = 0; i < right.size(); i++)
	{
		TCHAR szVal = right.at(i);
		switch (szVal)
		{		
		case '+':
			ival += intMap.find(stemp) == intMap.end() ? _ttoi(stemp.c_str()) : intMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		case '-':
			ival += intMap.find(stemp) == intMap.end() ? _ttoi(stemp.c_str()) : intMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		case '*':
			ival += intMap.find(stemp) == intMap.end() ? _ttoi(stemp.c_str()) : intMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		case '/':
			ival += intMap.find(stemp) == intMap.end() ? _ttoi(stemp.c_str()) : intMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		default:
			stemp+=szVal;
			if (i == right.size() - 1 ||
				wstring(_T("+-*/#")).find(right.substr(i + 1, 1)) != wstring::npos)
			{
				if (sign > 0)
				{
					int itemp = 0;
					auto it = intMap.find(stemp);
					if (it != intMap.end())
						itemp = it->second;
					else
						itemp = _ttoi(stemp.c_str());

					switch (sign)
					{
					case '+':
						ival += itemp;
						break;
					case '-':
						ival -= itemp;
						break;
					case '*':
						ival *= itemp;
						break;
					case '/':
						ival /= itemp;
						break;
					}
					sign = 0;
					stemp = _T("");
				}
				else
				{					
					auto it = intMap.find(stemp);
					if (it != intMap.end())
					{
						ival = it->second;
						if (right.substr(i + 1).find(_T("++")) == 0)
						{
							ival++;
							i+=2;
						}
					}
					else if (stemp.find(_T("#"))==0)
					{
						auto lit = listMap.find(stemp.substr(1));
						if (lit != listMap.end())
						{
							ival = lit->second.size();
						}
					}
					else
					{
						ival = _ttoi(stemp.c_str());
					}
					stemp = _T("");
				}
			}
			break;
		}
	}
	return ival;
}
float CEtermCommand::floatValue(wstring right)
{
	float fval = 0;
	wstring stemp;
	int sign = 0;
	for (int i = 0; i < right.size(); i++)
	{
		TCHAR szVal = right.at(i);
		switch (szVal)
		{
		case '+':
			fval += floatMap.find(stemp) == floatMap.end() ? _ttoi(stemp.c_str()) : floatMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		case '-':
			fval += floatMap.find(stemp) == floatMap.end() ? _ttoi(stemp.c_str()) : floatMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		case '*':
			fval += floatMap.find(stemp) == floatMap.end() ? _ttoi(stemp.c_str()) : floatMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		case '/':
			fval += floatMap.find(stemp) == floatMap.end() ? _ttoi(stemp.c_str()) : floatMap.find(stemp)->second;
			stemp = _T("");
			sign = szVal;
			break;
		default:
			stemp += szVal;
			if (i == right.size() - 1 ||
				wstring(_T("+-*/")).find(right.substr(i + 1, 1)) != wstring::npos)
			{
				if (sign > 0)
				{
					float itemp = 0;
					auto it = floatMap.find(stemp);
					if (it != floatMap.end())
						itemp = it->second;
					else
						itemp = _ttof(stemp.c_str());

					switch (sign)
					{
					case '+':
						fval += itemp;
						break;
					case '-':
						fval -= itemp;
						break;
					case '*':
						fval *= itemp;
						break;
					case '/':
						fval /= itemp;
						break;
					}
					sign = 0;
					stemp = _T("");
				}
				else
				{
					auto it = floatMap.find(stemp);
					if (it != floatMap.end())
						fval = it->second;
					else
						fval = _ttof(stemp.c_str());
					stemp = _T("");
				}
			}
			break;
		}
	}
	return fval;
}
bool CEtermCommand::syntaxcheck(wstring sline,wstring type)
{
	stringEx ex;
	wstring key = ex.FindStr(sline, type + _T(".*?(=|;)"), _T("^") + type + _T("|=$|\\s|;"));
	wstring val = ex.FindStr(sline, _T("=.*?;"), _T("^=|;$"));

	if (type==_T("string"))
	{
		auto it=stringMap.find(key);
		if (it==stringMap.end())
			stringMap.insert(make_pair(key, _T("")));
		it = stringMap.find(key);
		it->second = getparams(val);
	}
	else if (type==_T("int"))
	{
		auto it = intMap.find(key);
		if (it == intMap.end())
			intMap.insert(make_pair(key, 0));
		it = intMap.find(key);
		it->second = intValue(val);
	}
	else if (type==_T("float"))
	{
		auto it = floatMap.find(key);
		if (it == floatMap.end())
			floatMap.insert(make_pair(key, 0));
		it = floatMap.find(key);
		it->second = floatValue(val);
	}
	else if (type == _T("list<string>"))
	{
		auto it = listMap.find(key);
		vector<wstring> vec;
		if (it == listMap.end())
			listMap.insert(make_pair(key, vec));
		SetValue(key,val);
	}
	return true;
}

bool CEtermCommand::Judge(wstring left,wstring right,int bEqual)
{
	stringEx ex;
	auto sit = stringMap.find(left);
	auto iit = intMap.find(left);
	auto fit = floatMap.find(left);

	if (sit!=stringMap.end())
	{
		wstring sl,sr;
		sl=sit->second;
		auto rit = stringMap.find(right);
		if (rit!=stringMap.end())
			sr=rit->second;
		else
			sr=ex.replace(right,_T("(^\")|(\"$)"),_T(""));	
		switch (bEqual)
		{
		case 0:
			return sl==sr;
		case 1:
			return sl!=sr;	
		}
	}
	else if (iit!=intMap.end())
	{
		int il,ir;
		il=iit->second;
		auto rit = intMap.find(right);
		if (rit!=intMap.end())		
			ir=rit->second;
		else
			ir=_ttoi(right.c_str());
		switch (bEqual)
		{
		case 0:
			return il==ir;
		case 1:
			return il!=ir;	
		case 2:
			return il>ir;
		case 3:
			return il<ir;
		case 4:
			return il >= ir;
		case 5:
			return il <= ir;
		}
	}
	else if (fit!=floatMap.end())
	{
		int fl,fr;
		fl=fit->second;
		auto rft = floatMap.find(right);
		if (rft!=floatMap.end())		
			fr=rft->second;
		else
			fr=_ttof(right.c_str());
		switch (bEqual)
		{
		case 0:
			return fl==fr;
		case 1:
			return fl!=fr;	
		case 2:
			return fl>fr;
		case 3:
			return fl<fr;
		case 4:
			return fl >= fr;
		case 5:
			return fl <= fr;
		}
	}
	return false;
}

bool CEtermCommand::analyseIF(wstring sline)
{
	stringEx ex;
	wstring term = ex.substring(sline,_T("\\("),_T("\\)"));
	
	bool bret = false;
	wstring sub;				//临时子字符串
	wstring left,right;			//左值、右值
	int sign = -1;				//符号
	int nog = 0;				//逻辑符号
	int nJudge = 0;				//第N次逻辑判断
	for (int i = 0; i < term.size(); i++)
	{
		if (i+1<term.size() && term.substr(i, 2) == _T("&&"))
		{
			right = sub;
			if (nJudge == 0)
			{
				bret = Judge(left, right, sign);
			}
			else
			{
				if (nog == 1)
				{
					bret &= Judge(left, right, sign);
				}
				else if (nog == 2)
				{
					bret |= Judge(left, right, sign);
				}				
			}
			nog = 1;
			i += 1;
			nJudge++;
			sign = -1;
			sub = _T("");
			left = _T("");
			right = _T("");
		}
		else if (i + 1<term.size() && term.substr(i, 2) == _T("||"))
		{
			right = sub;
			if (nJudge == 0)
			{
				bret = Judge(left, right, sign);
			}
			else
			{
				if (nog == 1)
				{
					bret &= Judge(left, right, sign);
				}
				else if (nog == 2)
				{
					bret |= Judge(left, right, sign);
				}
			}					
			nJudge++;
			nog = 2;
			i += 1;
			sign = -1;
			sub = _T("");
			left = _T("");
			right = _T("");
		}
		else if (i + 1<term.size() && term.substr(i, 2) == _T("=="))
		{
			sign = 0;
			i += 1;
			left = sub;
			sub = _T("");
			right = _T("");
		}
		else if (i + 1<term.size() && term.substr(i, 2) == _T("!="))
		{
			sign = 1;
			i += 1;
			left = sub;
			sub = _T("");
			right = _T("");
		}
		else if (i + 1<term.size() && term.substr(i, 2) == _T(">="))
		{
			sign = 4;
			i += 1;
			left = sub;
			sub = _T("");
			right = _T("");
		}
		else if (i + 1<term.size() && term.substr(i, 2) == _T("<="))
		{
			sign = 5;
			i += 1;
			left = sub;
			sub = _T("");
			right = _T("");
		}
		else if (term.substr(i, 1) == _T(">"))
		{
			sign = 2;			
			left = sub;
			sub = _T("");
			right = _T("");
		}
		else if (term.substr(i,1) == _T("<"))
		{
			sign = 3;
			left = sub;
			sub = _T("");
			right = _T("");
		}
		else
		{
			sub.append(term.substr(i, 1));
			if (i == term.size() - 1)
			{
				right = sub;								
				if (nJudge == 0)
				{
					bret = Judge(left, right, sign);
				}
				else
				{
					if (nog == 1)
					{
						bret &= Judge(left, right, sign);
					}
					else if (nog == 2)
					{
						bret |= Judge(left, right, sign);
					}
				}
				nog = 0;
				i += 1;
				nJudge++;
				sign = -1;
				sub = _T("");
				left = _T("");
				right = _T("");
			}
		}		
	}
	return bret;
}

wstring CEtermCommand::get_system(wstring sline,int& nwait)
{
	stringEx ex;
	sline=ex.substring(sline,_T("\\("),_T("\\)"));
	wstring snum = ex.FindStr(sline, _T(",\\d+$"),_T(","));
	if (!snum.empty())
	{
		sline = ex.FindStr(sline, _T("^.*,"), _T(",$"));
		nwait = _ttoi(snum.c_str());
	}
	return getparams(sline.c_str());
}

wstring CEtermCommand::get_return(wstring sline)
{
	wstring str;
	stringEx ex;
	sline=ex.replace(sline,_T("((^\\s+|)return\\s+)|(;.*?)"),_T(""));
	 
	return getparams(sline);	
}

void CEtermCommand::ret(wstring sline)
{
	wstring str;
	stringEx ex;
	sline = ex.replace(sline, _T("((^\\s+|)ret\\s+)|(;.*?)"), _T(""));
	wstring rets = getparams(sline);
	Global::Server.SendMsg(this->m_sock, rets.c_str());
}

bool CEtermCommand::InitConfig(TCHAR* szConfig)
{
	/*stringEx ex;
	wstring sconfig = ex.replace(szConfig, _T("config=|;"),_T(""));
	
	if (_tcslen(szConfig) < 10) return false;

	USES_CONVERSION;

	Encrypt encrypt;
	unsigned char key[] = "a3%fej&4";
	sconfig = A2T(encrypt.decrypt(key, (char*)T2A(sconfig.c_str())));
	
	CONFIG config;
	config.UserName = ex.FindStr(sconfig, _T("UserName=.*?(&|$)"), _T("UserName=|&"), true);
	config.Company = ex.FindStr(sconfig, _T("Company=.*?(&|$)"), _T("Company=|&"), true);
	config.AutoSI = ex.FindStr(sconfig, _T("AutoSI=.*?(&|$)"), _T("AutoSI=|&"), true) == _T("true");
	config.PassWord = ex.FindStr(sconfig, _T("PassWord=.*?(&|$)"), _T("PassWord=|&"), true);
	config.Port = _ttoi(ex.FindStr(sconfig, _T("Port=.*?(&|$)"), _T("Port=|&"), true).c_str());
	config.Server = ex.FindStr(sconfig, _T("Server=.*?(&|$)"), _T("Server=|&"), true);
	config.SI = ex.FindStr(sconfig, _T("SI=.*?(&|$)"), _T("SI=|&"), true);
	config.SSL = ex.FindStr(sconfig, _T("SSL=.*?(&|$)"), _T("SSL=|&"), true) == _T("true");
	if (config.UserName.empty()) return false;

	CString strMd5;
	strMd5.Format(_T("%s%s%s%d"), config.UserName.c_str(), config.PassWord.c_str(), config.Server.c_str(), config.Port);	 
	BYTE* pBuffer = new BYTE[strMd5.GetLength()];
	memcpy(pBuffer, strMd5, strMd5.GetLength());
	CString MD5ret = CMD5Checksum::GetMD5(pBuffer, strMd5.GetLength());
	delete[] pBuffer;	
	config.sid = MD5ret.GetBuffer();
	MD5ret.ReleaseBuffer();

	CString strLog;
	strLog.Format(_T("用户:%s开始 MD5:%s"),config.UserName.c_str(),MD5ret);
	Global::WriteLog(strLog, config.sid);

	this->config = config.sid;
	SendMessage(AfxGetApp()->m_pMainWnd->GetSafeHwnd(), WM_CONFIG, (WPARAM)sock,(LPARAM)&config);	
	auto it = Global::CSocketAssemble.find(this->config);
	if (it == Global::CSocketAssemble.end())
	{
		return false;
	}
	int timeout = 0;
	while (it->second->m_nState!=2)
	{
		Sleep(100);
		if (it->second->m_nState > 0)
		{
			Sleep(1000);
			break;
		}		
		if (timeout>50)
		timeout++;
	}	
	return it->second->m_nState == 2;*/
	return true;
}

wstring CEtermCommand::Replace(wstring sline)
{
	wstring str;
	stringEx ex;
	sline = ex.substring(sline, _T("Replace(\\s+|)\\("), _T("\\)$"));
	vector<wstring> items = ex.split(sline, _T(","));
	wstring strfind, strsear, strrep;
	for (int i = 0; i<items.size(); i++)
	{
		wstring sstr = ex.replace(items[i], _T("^\\s|\\s$"), _T(""));
		if (i == 0)
		{
			if (sstr == _T("DATA"))
			{
				if (etermData.size()>0)
				{
					strfind = etermData[etermData.size() - 1].rec;
				}
			}
			else
			{
				auto sfit = stringMap.find(sstr);
				if (sfit != stringMap.end())
					strfind = sfit->second;
			}
		}
		else if (i == 1)
		{
			if (ex.match(sstr, _T("^\".*?\"$")))
				strsear = ex.replace(sstr, _T("^\"|\"$"), _T(""));
			else
			{
				wstring skey = ex.replace(sstr, _T("^\\s+|\\s+$"), _T(""));
				auto sit = stringMap.find(skey);
				if (sit != stringMap.end())
				{
					strsear = sit->second;
				}
				else
				{
					strsear = getparams(skey.c_str());
				}
			}
		}
		else if (i == 2)
		{
			if (ex.match(sstr, _T("^\".*?\"$")))
				strrep = ex.replace(sstr, _T("^\"|\"$"), _T(""));
			else
			{
				wstring skey = ex.replace(sstr, _T("^\\s+|\\s+$"), _T(""));
				auto sit = stringMap.find(skey);
				if (sit != stringMap.end())
				{
					strrep = sit->second;
				}
				else
				{
					strrep = getparams(skey.c_str());
				}
			}
		}		
	}
	return ex.replace(strfind, strsear, strrep);
}

wstring CEtermCommand::Format(wstring sline)
{
	stringEx ex;
	sline = ex.substring(sline, _T("Format(\\s+|)\\("), _T("\\)$"));
	vector<wstring> items = ex.split(sline, _T("\\<\\<"));
	wstringstream ss;
	wstring str;
	for (int i = 0; i<items.size(); i++)
	{
		wstring sstr = ex.replace(items[i], _T("^\\s+|\\s+$"), _T(""));
		if (ex.match(sstr, _T("^\".*?\"$")))
			ss << ex.replace(sstr, _T("^\"|\"$"), _T(""));
		else if (ex.match(sstr, _T("\\d+(\.\\d|)")))
			ss << sstr;
		auto sfit = stringMap.find(sstr);
		auto ifit = intMap.find(sstr);
		auto ffit = floatMap.find(sstr);
		if (sfit != stringMap.end())
			ss<<sfit->second;
		if (ifit != intMap.end())
			ss<<ifit->second;
		if (ffit != floatMap.end())
			ss << setiosflags(ios::fixed) << setprecision(2)<<ffit->second;
	}
	str = ss.str();
	return str;
}

wstring CEtermCommand::getparams(wstring szData)
{
	wstring sret;
	stringEx ex;
	
	wstring sitem(szData);
	if (ex.match(sitem, _T("^(\\s+|)FindStr\\(.*?\\)(\\s+|)$")))
	{
		sret += FindStr(sitem);
	}
	else if (ex.match(sitem, _T("^(\\s+|)Replace\\(.*?\\)(\\s+|)$")))
	{
		sret += Replace(sitem);
	}
	else if (ex.match(sitem, _T("^(\\s+|)Format\\(.*?\\)(\\s+|)$")))
	{
		sret += Format(sitem);
	}	
	else if (ex.match(sitem, _T("^(\\s+|)SubStr\\(.*?\\)(\\s+|)$")))
	{
		sret += SubStr(sitem);
	}
	else if (ex.match(sitem, _T("^(\\s+|)[A-Z]([A-Z0-9]+|)\\[.*?\\](\\s+|)$")))
	{
		int id = -1;
		wstring key = ex.FindStr(sitem, _T("^(\\s+|)[A-Z]([A-Z0-9]+|)"),_T("\\s"));
		wstring sid = ex.FindStr(sitem, _T("\\[.*?\\]"), _T("\\[|\\]|\\s"));
		if (ex.match(sid, _T("\\d+")))
		{
			id = _ttoi(sid.c_str());
		}
		else
		{
			auto iit = intMap.find(sid);
			if (iit != intMap.end())
			{
				id = iit->second;
			}
		}
		auto llit = listMap.find(key);
		if (llit != listMap.end())
		{
			if (id >= 0 && id < llit->second.size())
			{
				sret += llit->second[id];
			}
		}
		else if (key == _T("DATAS"))
		{			
			if (id >= 0 && id<etermData.size())
				sret += etermData[id].rec;
		}
	}
	else
	{
		bool bstart = false;
		wstring stemp;

		int n = 0;

		for (int i = 0; i < szData.size(); i++)
		{
			if (szData[i] == '"')
			{
				if (bstart && (i == szData.size() - 1 || (i + 1<szData.size() && szData[i + 1] == '+')))
				{					
					sret += stemp;
					stemp = _T("");
					n = 0;
					bstart = false;
				}
				else if (!bstart)
				{
					bstart = true;
				}
				else
				{
					stemp += szData[i];
				}
			}			
			else if (szData[i] == '+')
			{
				if (bstart)
				{
					stemp += szData[i];
				}
				else
				{
					if (!stemp.empty())
					{	
						auto sit = stringMap.find(stemp);
						if (sit != stringMap.end())
						{
							sret += sit->second;
						}
						else if (stemp == _T("DATA"))
						{
							if (etermData.size() > 0)
							{
								sret += etermData[etermData.size() - 1].rec;
							}							
						}
						else if (stemp == _T("DATAS"))
						{
							for (int i = 0; i<etermData.size();i++)
							{
								sret += etermData[i].rec + _T("\r");
							}							
						}
						stemp = _T("");
					}
				}
			}
			else
			{
				stemp += szData[i];
				if (i == szData.size() - 1)
				{					
					auto sit = stringMap.find(stemp);
					if (sit != stringMap.end())
					{
						sret += sit->second;
					}
					else if (stemp == _T("DATA"))
					{
						if (etermData.size() > 0)
						{
							sret += etermData[etermData.size() - 1].rec;
						}						
					}
					else if (stemp == _T("DATAS"))
					{
						for (int ii = 0; ii< etermData.size();ii++)
						{
							sret += etermData[ii].rec + _T("\r");
						}						
					}
					else if (ex.match(stemp, _T("^(\\s+|)[A-Z]([A-Z0-9]+|)\\[.*?\\](\\s+|)$")))
					{
						sret += GetListValue(stemp);
					}
				}
			}
		}		
	}
	return sret;
}

vector<wstring> CEtermCommand::getParams(wstring sline)
{
	vector<wstring> vec;
	wstring sret;
	stringEx ex;

	bool bstart = false;
	wstring stemp;

	int n = 0;

	for (int i = 0; i < sline.size(); i++)
	{
		if (sline[i] == '"')
		{
			if (bstart && (i == sline.size() - 1 || (i + 1<sline.size() && (sline[i + 1] == '+' || sline[i + 1] == ','))))
			{
				vec.push_back(stemp);
				stemp = _T("");
				n = 0;
				bstart = false;
			}
			else if (!bstart)
			{
				bstart = true;
			}
			else
			{
				stemp += sline[i];
			}
		}
		else if (sline[i] == ',')
		{
			if (bstart)
			{
				stemp += sline[i];
			}
			else
			{
				if (!stemp.empty())
				{
					auto sit = stringMap.find(stemp);
					if (sit != stringMap.end())
					{
						vec.push_back(sit->second);
					}
					else if (stemp == _T("DATA"))
					{
						if (etermData.size() > 0)
						{
							vec.push_back(etermData[etermData.size() - 1].rec);
						}
					}
					else if (stemp == _T("DATAS"))
					{
						sret = _T("");
						for (int i = 0; i<etermData.size(); i++)
						{
							sret += etermData[i].rec + _T("\r");
						}
						vec.push_back(sret);
						sret = _T("");
					}
					stemp = _T("");
				}
			}
		}
		else
		{
			stemp += sline[i];
			if (i == sline.size() - 1)
			{
				auto sit = stringMap.find(stemp);
				if (sit != stringMap.end())
				{
					vec.push_back(sit->second);
				}
				else if (stemp == _T("DATA"))
				{
					if (etermData.size() > 0)
					{
						vec.push_back(etermData[etermData.size() - 1].rec);
					}
				}
				else if (stemp == _T("DATAS"))
				{
					sret = _T("");
					for (int ii = 0; ii< etermData.size(); ii++)
					{
						sret += etermData[ii].rec + _T("\r");
					}
					vec.push_back(sret);
					sret = _T("");
				}
				else if (stemp == _T("true") || stemp == _T("false"))
				{
					vec.push_back(stemp);
				}
				else if (ex.match(stemp, _T("^(\\s+|)[A-Z]([A-Z0-9]+|)\\[.*?\\](\\s+|)$")))
				{
					vec.push_back(GetListValue(stemp));
				}
			}
		}
	}
	
	return vec;
}

FUN CEtermCommand::InitFun(wstring sline, vector<wstring> codelist, int& i)
{
	FUN fun;

	stringEx ex;
	sline = ex.FindStr(sline, _T("\\(.*?\\)"), _T("\\(|\\)"));
	vector<wstring> params = ex.split(sline, _T(","));
	
	for (int ii = 0; ii < params.size(); ii++)
	{
		PARAM param;
		if (params[ii].find(_T("string ")) != wstring::npos)
		{
			wstring skey = ex.replace(params[ii], _T("string "), _T(""));
			param.key = skey;
			param.type = 1;
			fun.params.push_back(param);
		}
		else if (params[ii].find(_T("int ")) != wstring::npos)
		{
			wstring skey = ex.replace(params[ii], _T("int "), _T(""));
			param.key = skey;
			param.type = 2;
			fun.params.push_back(param);
		}
		else if (params[ii].find(_T("float ")) != wstring::npos)
		{
			wstring skey = ex.replace(params[ii], _T("float "), _T(""));
			param.key = skey;
			param.type = 3;
			fun.params.push_back(param);
		}
		if (params[ii].find(_T("list<string> ")) != wstring::npos)
		{
			wstring skey = ex.replace(params[ii], _T("list<string> "), _T(""));
			param.key = skey;
			param.type = 4;
			fun.params.push_back(param);
		}
	}
	int c1 = 0, c2 = 0;
	while (i<codelist.size())
	{
		wstring source = ex.replace(codelist[i++], _T("^\\s+|\\s+$"), _T(""));
		if (source == _T("{"))
			c1++;
		else if (source == _T("}"))
			c2++;		

		fun.sources.push_back(source);

		if (c1>0 && c2 == c1)
			break;			
	}

	i--;

	return fun;
}

ETERM_STATE CEtermCommand::function(FUN fun, int type, wstring& sret, int& iret, float& fret, vector<wstring>& listret)
{
	stringEx ex;
	wstring strErr;
	vector<bool> bElses;
	map<int, FOR> fors;

	for (auto it = fun.params.begin(); it != fun.params.end(); it++)
	{
		switch (it->type)
		{
		case 1:
			stringMap.insert(make_pair(it->key, it->sval));
			break;
		case 2:
			intMap.insert(make_pair(it->key, it->ival));
			break;
		case 3:
			floatMap.insert(make_pair(it->key, it->fval));
			break;
		case 4:
			listMap.insert(make_pair(it->key, it->lval));
			break;
		}
	}

	for (int i = 0; i < fun.sources.size(); i++)
	{
		wstring sline = ex.replace(fun.sources[i], _T("^\\s+|\\s+$"), _T(""));
		if (sline.empty()) continue;

		
		if (sline.find(_T("string")) == 0)
		{
			if (!syntaxcheck(sline, _T("string")))
			{
				return STRING_FORMAT;
			}
		}
		else if (sline.find(_T("int")) == 0)
		{
			if (!syntaxcheck(sline, _T("int")))
			{
				return INT_FORMAT;
			}
		}
		else if (sline.find(_T("float")) == 0)
		{
			if (!syntaxcheck(sline, _T("float")))
			{
				return FLOAT_FORMAT;
			}
		}
		else if (sline.find(_T("list<string>")) == 0)
		{
			if (!syntaxcheck(sline, _T("list<string>")))
			{
				return LIST_FORMAT;
			}
		}		
		else if (sline.find(_T("if")) == 0)
		{
			if (!analyseIF(sline))
			{
				int c1 = 0, c2 = 0;
				while (i<fun.sources.size())
				{
					sline = ex.replace(fun.sources[i++], _T("^\\s+|\\s+$"), _T(""));
					if (sline == _T("{"))
						c1++;
					else if (sline == _T("}"))
						c2++;
					if (c1>0 && c2 == c1)
						break;
				}
				int jj = i; i--;
				while (jj<fun.sources.size())
				{
					wstring key = ex.replace(fun.sources[jj], _T("^\\s+|\\s+$"), _T(""));
					if (key == _T("else"))
					{
						bElses.push_back(true);
						break;
					}
					else if (key.empty())
						continue;
					else if (ex.match(key, _T("//.*")))
						continue;
					else
						break;
					jj++;
				}
			}
			else
			{
				int jj = i;

				int c1 = 0, c2 = 0;
				while (jj<fun.sources.size())
				{
					sline = ex.replace(fun.sources[jj++], _T("^\\s+|\\s+$"), _T(""));
					if (sline == _T("{"))
						c1++;
					else if (sline == _T("}"))
						c2++;
					if (c1>0 && c2 == c1)
						break;

				}
				while (jj<fun.sources.size())
				{
					wstring key = ex.replace(fun.sources[jj], _T("^\\s+|\\s+$"), _T(""));
					if (key == _T("else"))
					{
						bElses.push_back(false);
						break;
					}
					else if (key.empty())
						continue;
					else if (ex.match(key, _T("//.*")))
						continue;
					else
						break;
					jj++;
				}
			}
		}
		else if (sline.find(_T("system")) == 0)
		{
			//执行Eterm指令
			Invoke(sline);
		}
		else if (sline.find(_T("goto")) == 0)
		{
			
			wstring key = ex.FindStr(sline, _T("goto.*?;"), _T("goto|;|\\s"), true);
			auto step = steplist.find(key);
			if (step != steplist.end())
			{
				i = step->second;					
			}
			else
			{				
				bool bfind = false;
				int ii = i;
				while (i < fun.sources.size())
				{
					sline = ex.replace(fun.sources[i++], _T("^\\s+|\\s+$"), _T(""));
					if (sline == key + _T(":"))
					{
						if (steplist.find(key) == steplist.end())
							steplist.insert(make_pair(key, i--));
						bfind = true;
						break;
					}
				}
				if (!bfind)
				{
					i = ii;
					while (i > 0)
					{
						sline = ex.replace(fun.sources[i--], _T("^\\s+|\\s+$"), _T(""));
						if (sline == key + _T(":"))
						{
							if (steplist.find(key) == steplist.end())
								steplist.insert(make_pair(key, i++));
							bfind = true;
							break;
						}
					}
				}
			}
			
		}
		else if (sline.find(_T("return")) == 0)
		{
			sline = ex.replace(sline, _T("((^\\s+|)return\\s+)|(;.*?)"), _T(""));
			switch (type)
			{
			case 1:
				sret = getparams(sline);
				break;
			case 2:
				iret = intValue(sline);
				break;
			case 3:
				fret = floatValue(sline);
				break;
			case 4:
				if (listMap.find(sline) == listMap.end())
				{
					listret = listMap.find(sline)->second;
				}				 
				break;
			default:
				return NONE_TYPE;
			}
			return FUN_SUC;
		}		
		else if (sline.find(_T("else")) == 0 && bElses.size()>0)
		{
			if (!bElses.back())
			{
				int c1 = 0, c2 = 0;
				while (i<fun.sources.size())
				{
					sline = ex.replace(fun.sources[i++], _T("^\\s+|\\s+$"), _T(""));
					if (sline == _T("{"))
						c1++;
					else if (sline == _T("}"))
						c2++;
					if (c1>0 && c2 == c1)
						break;

				}
				i--;
			}
			bElses.pop_back();
		}
		else if (ex.match(sline, _T("^(\\s+|)[A-Za-z][A-Za-z0-9]+?:(\\s+|)$")))
		{
			wstring key = sline.substr(0, sline.length() - 1);
			if (steplist.find(key) == steplist.end())
				steplist.insert(make_pair(key, i));
		}
		else if (sline.find(_T("Sleep")) == 0)
		{
			int interal = _ttoi(ex.FindStr(sline, _T("\\d+")).c_str());
			Sleep(interal);
		}
		else if (sline.find(_T("for")) == 0)
		{
			auto it = fors.find(i);
			if (it == fors.end())
			{
				wstring sfor = ex.FindStr(sline, _T("\\(.*?\\)"), _T("\\(|\\)"));
				vector<wstring> vfors = ex.split(sfor, _T(";"));
				if (vfors.size() == 3)
				{
					FOR _for;
					_for.sinit = SetintVal(vfors[0] + _T(";"));
					_for.sif = vfors[1];
					_for.step = ex.replace(vfors[2], _for.sinit + _T("="), _T(""));
					_for.ns = i;
					int c1 = 0, c2 = 0;
					int ii = i;
					while (i<fun.sources.size())
					{
						sline = ex.replace(fun.sources[ii++], _T("^\\s+|\\s+$"), _T(""));
						if (sline == _T("{"))
							c1++;
						else if (sline == _T("}"))
							c2++;
						if (c1>0 && c2 == c1)
							break;
					}
					_for.ne = ii - 1;
					fors.insert(make_pair(i, _for));
				}
			}
		}
		else if (sline == _T("}"))
		{
			if (fors.size()>0)
			{
				FOR _for;
				bool bfind = false;
				for (auto it = fors.begin(); it != fors.end(); it++)
				{
					if (it->second.ne == i)
					{
						_for = it->second;
						bfind = true;
						break;
					}
				}
				if (bfind)
				{
					auto it = intMap.find(_for.sinit);
					if (it != intMap.end())
					{
						it->second = intValue(_for.step);
						_for.bif = analyseIF(_for.sif);
						if (_for.bif)
						{
							i = _for.ns;
						}
					}
				}
			}
		}
		else if (sline.find(_T("break")) == 0)
		{
			for (auto it = fors.begin(); it != fors.end(); it++)
			{
				if (i> it->second.ns && it->second.ne > i)
				{
					i = it->second.ne;
					break;
				}
			}
		}
		else
		{
			int isplit = sline.find(_T("="));
			if (isplit>0)
			{
				wstring left = ex.replace(sline.substr(0, isplit), _T("^\\s+|\\s+$"), _T(""));
				wstring right = ex.replace(sline.substr(isplit + 1), _T("^\\s+|\\s+$|;"), _T(""));
				SetValue(left, right);
			}
		}	
	}

	for (auto it = fun.params.begin(); it != fun.params.end(); it++)
	{
		switch (it->type)
		{
		case 1:
			stringMap.erase(it->key);
			break;
		case 2:
			intMap.erase(it->key);
			break;
		case 3:
			floatMap.erase(it->key);
			break;
		case 4:
			listMap.erase(it->key);
			break;
		}
	}
}

bool CEtermCommand::SetVal(wstring left, wstring right,wstring sline,vector<wstring> codelist,int i)
{
	stringEx ex;
	if (ex.match(right, _T("^(\\s+|)fun_.*?\\(.*?\\)(\\s+|)$")))
	{
		wstring skey = ex.FindStr(right, _T("fun_.*?\\("), _T("\\("));
		auto fit = funMap.find(skey);
		if (fit == funMap.end())
		{
			funMap.insert(make_pair(skey, InitFun(sline, codelist, i)));
			fit = funMap.find(skey);
		}
		wstring strparams = ex.FindStr(right, _T("\\(.*?\\)"), _T("\\(|\\)"));
		vector<wstring> sparams = ex.split(strparams, _T(","));
		if (sparams.size() != fit->second.params.size())
		{
			return false;
		}
		for (int ii = 0; ii < sparams.size(); ii++)
		{
			switch (fit->second.params[ii].type)
			{
			case 1:
				fit->second.params[ii].sval = stringMap.find(sparams[ii]) == stringMap.end() ? ex.replace(sparams[ii], _T("^\"|\"$"), _T("")) : stringMap.find(sparams[ii])->second;
				break;
			case 2:
				fit->second.params[ii].ival = intMap.find(sparams[ii]) == intMap.end() ? _ttoi(sparams[ii].c_str()) : intMap.find(sparams[ii])->second;
				break;
			case 3:
				fit->second.params[ii].fval = floatMap.find(sparams[ii]) == floatMap.end() ? _ttof(sparams[ii].c_str()) : floatMap.find(sparams[ii])->second;
				break;
			case 4:
				if (listMap.find(sparams[ii]) != listMap.end())
				{
					fit->second.params[ii].lval = listMap.find(sparams[ii])->second;
				}
				break;
			}
		}
		auto sit = stringMap.find(left);
		auto iit = intMap.find(left);
		auto floatit = floatMap.find(left);
		auto lit = listMap.find(left);
		wstring sret;
		int iret;
		float fret;
		vector<wstring> listret;
		if (sit != stringMap.end())
		{
			function(fit->second, 1, sret, iret, fret, listret);
			sit->second = sret;
		}
		else if (iit != intMap.end())
		{
			function(fit->second, 2, sret, iret, fret, listret);
			iit->second = iret;
		}
		else if (floatit != floatMap.end())
		{
			function(fit->second, 3, sret, iret, fret, listret);
			floatit->second = fret;
		}
		else if (lit != listMap.end())
		{
			function(fit->second, 4, sret, iret, fret, listret);
			lit->second = listret;
		}
		else
		{
			function(fit->second, 0, sret, iret, fret, listret);
		}
	}
	else
	{
		SetValue(left, right);
	}
}

wstring CEtermCommand::SetintVal(wstring sline)
{
	stringEx ex;
	vector<wstring> kvs = ex.split(ex.substring(sline, _T("int"), _T(";")), _T("="));
	if (kvs.size() == 1)
		kvs.push_back(_T("0"));	
	auto it = intMap.find(kvs[0]);
	if (it == intMap.end())
		intMap.insert(make_pair(kvs[0], _ttoi(kvs[1].c_str())));
	else
		it->second = _ttoi(kvs[1].c_str());
	return kvs[0];
}

bool CEtermCommand::Using(wstring sline)
{
	stringEx ex;
	wstring config = ex.substring(sline, _T("USING:"), _T(";"));
	auto it = Global::configlist.find(config.c_str());
	if (it != Global::configlist.end())
	{
		m_config = it->first;
		return true;
	}
	return false;
}


CMD_TYPE CEtermCommand::cmdType(wstring& cmd, bool& bEn)
{
	stringEx ex;

	if (cmd.find(_T("PRINT:")) == 0)
	{
		bEn = ex.FindStr(cmd, _T("EN"))!=_T("");
		cmd = ex.replace(cmd, _T("(PRINT:)|(EN)|(CN)|(VV)|(-)"), _T(""));
		return print;
	}
	else if (cmd.find(_T("PNR:")) == 0)
	{
		cmd = ex.replace(cmd, _T("PNR:"), _T(""));
		return pnr;
	}
	else
	{	
		cmd = ex.replace(cmd, _T("\\[RN\\]"), _T("\r"));
		return normal;
	}
}

bool CEtermCommand::Invoke(wstring sline)
{	
	bool bret = false;
	
	int nWait = 60*1000;
	wstring cmd = get_system(sline, nWait);
	if (cmd.empty()) return false;

	auto it = Global::configlist.find(m_config.c_str());
	if (it == Global::configlist.end()) return false;
		
	if (it->second->m_config.Interval > 0)
	{
		CTimeSpan ts = CTime::GetCurrentTime() - it->second->m_ActiveTime;
		int ntime = ts.GetTotalSeconds() * 1000;
		ntime = min(ntime, it->second->m_config.Interval);
		if (ntime>0)
			Sleep(ntime);
	}
			
	int nresendcount = 0;
	resend:
	it->second->m_cs.Lock();
	it->second->m_EtermPacket.m_cmd = cmd.c_str();
	bool bEn = false;
	it->second->m_EtermPacket.m_cmdType = cmdType(cmd,bEn);
	it->second->m_EtermPacket.m_strResponse.Empty();
	it->second->m_EtermPacket.m_vecRev.clear();
	it->second->m_strResponse.Empty();
	it->second->m_config.Count++;
	it->second->m_cs.Unlock();

	ShowInfo::SendShowInfo(2, m_config.c_str(), cmd.c_str(), it->second->m_config.Count);
	Global::WriteLog(CLog(cmd.c_str(), it->first));

	if (it->second->m_EtermPacket.m_cmdType == print)
		bret=it->second->SendPrintMsg((string)CT2A(cmd.c_str()), bEn);
	else
		bret=it->second->SendMsg((string)CT2A(cmd.c_str()));
		
	ResetEvent(it->second->pEvent);
	DWORD dwOutTime = ::WaitForSingleObject(it->second->pEvent, nWait);
	
	switch (dwOutTime)
	{
		case WAIT_OBJECT_0:
		{
							SetEvent(it->second->pEvent);

							it->second->m_cs.Lock();										
							it->second->m_config.Count++;
							it->second->m_cs.Unlock();

							

							if (it->second->m_config.IsSSL)
							{
								if (it->second->m_strResponse == _T("S") ||
									it->second->m_strResponse.Find(_T("SIGN IN FIRST")) != -1)
								{
									it->second->SendMsg((string)CT2A(it->second->m_config.SI));
									it->second->m_cs.Lock();
									it->second->m_config.Count += 2;
									it->second->m_cs.Unlock();
									ShowInfo::SendShowInfo(3, it->first, it->second->m_config.SI, it->second->m_config.Count);
									Sleep(500);
									nresendcount++;
									if (nresendcount < 3)
										goto resend;
								}
							}
							
							if (it->second->m_EtermPacket.m_cmdType == print)
								this->etermData.push_back(EtermDATA(cmd, (wstring)it->second->m_strResponse));
							else
							{
								for (auto iit = it->second->m_EtermPacket.m_vecRev.begin(); iit != it->second->m_EtermPacket.m_vecRev.end(); iit++)
									this->etermData.push_back(EtermDATA(cmd, wstring(*iit)));
							}
							ShowInfo::SendShowInfo(3, m_config.c_str(), cmd.c_str(), it->second->m_config.Count);
		bret = true;
	}
		break;
	case WAIT_TIMEOUT:
	case WAIT_FAILED:
	{
			SetEvent(it->second->pEvent);

			/*it->second->m_cs.Lock();				
			it->second->m_nState = CONNECT_FAIL;
			it->second->m_cs.Unlock();
			auto vit = Global::EtermViews.find(it->first);
			if (vit != Global::EtermViews.end())
			{
				::PostMessage(vit->second->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));

				int ncount = 0;
				while (true)
				{
					Sleep(500);
					if (it->second->m_nState == CONFIG_STATE::AVAILABLE)
					{
						break;
					}
					ncount++;
					if (ncount > 60) break;
				}

				nresendcount++;
				if (nresendcount < 3)
					goto resend;
				
			}*/
	}		
		break;	
	default:
		SetEvent(it->second->pEvent);
		break;
	}
	//----------流量控制-----------------------------------------------------------------------------
	if (it->second->m_config.MaxCount - it->second->m_config.Count < Threshold)
	{		
		auto vit = Global::EtermViews.find(it->first);
		if (vit != Global::EtermViews.end())
			::PostMessage(vit->second->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(it->first)));
		CString strInfo;
		strInfo.Format(_T("配置[%s] 设定最大流量：%d 当前流量：%d 剩余流量：%d"), it->second->m_config.UserName, it->second->m_config.MaxCount, it->second->m_config.Count, it->second->m_config.MaxCount - it->second->m_config.Count);
		EmailHelper::SendEmail(_T("Eterm流量超限"), strInfo);		
	}
	return bret;
}

wstring CEtermCommand::GetListValue(wstring sline)
{
	wstring sret;
	stringEx ex;
	int id = -1;
	wstring key = ex.FindStr(sline, _T("^(\\s+|)[A-Z]([A-Z0-9]+|)"), _T("\\s"));
	wstring sid = ex.FindStr(sline, _T("\\[.*?\\]"), _T("\\[|\\]|\\s"));
	if (ex.match(sid, _T("\\d+")))
	{
		id = _ttoi(sid.c_str());
	}
	else
	{
		auto iit = intMap.find(sid);
		if (iit != intMap.end())
		{
			id = iit->second;
		}
	}
	auto llit = listMap.find(key);
	if (llit != listMap.end())
	{
		if (id >= 0 && id < llit->second.size())
		{
			sret = llit->second[id];
		}
	}
	return sret;
}