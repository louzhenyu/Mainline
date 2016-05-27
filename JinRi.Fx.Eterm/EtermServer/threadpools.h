#include "stdafx.h"
#include "Global.h"
#include <io.h>
#include <map>
#include "Data.h"
#include "stringEx.h"
#include "strcoding.h"
#include "EtermCommand.h"
#include <fstream>
#include <algorithm>
#include "ShowInfo.h"
#include <memory>
#include "ConfigData.h"
#include "json\json.h"
#include "ConfigData.h"
#include <ctime>
#include <chrono>
#include <thread>
#include <iomanip> 

volatile bool bSystemOver = false;
UINT RemotingAutoConfig(wstring surl, SOCKET sock);
void EtermStreamCount(SOCKET sock);

unsigned int _stdcall _EtermRequestDispose(void* lparam);
using std::chrono::system_clock;

UINT LogThread(LPVOID lparam)
{
	while (!bSystemOver)
	{
		Sleep(3000);

		__try
		{

			if (Global::loglist.size() == 0) continue;

			char strPath[MAX_PATH] = { 0 };
			Global::m_log.Lock();
			map<string, vector<CLog>> logs;
			for (auto it = Global::loglist.begin(); it != Global::loglist.end(); it++)
			{
				auto mit = logs.find(it->strConfig);
				if (mit == logs.end())
				{
					vector<CLog> loglist;
					loglist.push_back((*it));
					logs.insert(make_pair(it->strConfig, loglist));
				}
				else
				{
					mit->second.push_back((*it));
				}
			}
			Global::loglist.clear();
			Global::m_log.Unlock();
			stringEx ex;
			char szTime[25] = { 0 };

			for (auto it = logs.begin(); it != logs.end(); it++)
			{
				for (int i = 0; i < it->second.size(); i++)
				{
					SECURITY_ATTRIBUTES sa;
					memset(&sa, 0, sizeof(sa));
					sa.nLength = sizeof(sa);
					sa.bInheritHandle = FALSE;
					sa.lpSecurityDescriptor = NULL;

					sprintf_s(strPath, "%s\\logs", CT2A(Global::szAppPath));
					if (_access(strPath, 0) != 0) CreateDirectory(CA2T(strPath), &sa);
					sprintf_s(strPath, "%s\\%s", strPath, it->first.c_str());
					if (_access(strPath, 0) != 0) CreateDirectory(CA2T(strPath), &sa);
					sprintf_s(szTime, "%04d%02d%02d", it->second[i].logTime.wYear, it->second[i].logTime.wMonth, it->second[i].logTime.wDay);
					sprintf_s(strPath, "%s\\%s.txt", strPath, szTime);

					ofstream out(strPath, ios::app);
					if (out)
					{
						sprintf_s(szTime, "%04d-%02d-%02d %02d:%02d:%02d.%03d", it->second[i].logTime.wYear, it->second[i].logTime.wMonth, it->second[i].logTime.wDay, it->second[i].logTime.wHour, it->second[i].logTime.wMinute, it->second[i].logTime.wSecond, it->second[i].logTime.wMilliseconds);
						out << szTime << "\r\n";
						out << it->second[i].strLog << "\r\n\r\n";
						out.close();
					}
				}
			}
			logs.clear();
		}
		__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
		{
			continue;
		}
	}
	return 0;
}

UINT StartWebServer(LPVOID lParam)
{
	USES_CONVERSION;

	CString strPath;
	strPath.Format(_T("%s\\Config.ini"), Global::szAppPath);

	TCHAR buf[255] = { 0 };
	int nPort = ::GetPrivateProfileInt(_T("SYSTEM"), _T("PORT"), 5252, strPath);
	::GetPrivateProfileString(_T("SYSTEM"), _T("IP"), _T(""), buf, sizeof(buf), strPath);

	Global::Server.InitServer(T2A(buf), nPort);

	int nRet = Global::Server.StartServer();

	while (!bSystemOver)
	{
		Sleep(1000);
	}

	Global::Server.StopServer();

	return 0;
}

UINT CheckEtermState(LPVOID lParam)
{
	while (!bSystemOver)
	{
		__try
		{
			//每日凌晨开始自动重启
			std::time_t tt = system_clock::to_time_t(system_clock::now());
			tt += 24 * 3600;
			struct std::tm * ptm = std::localtime(&tt);
			ptm->tm_hour = 0;
			ptm->tm_min = 0;
			ptm->tm_sec = 1;

			std::this_thread::sleep_until(system_clock::from_time_t(mktime(ptm)));

			for (auto it = Global::configlist.begin(); it != Global::configlist.end(); it++)
			{
				if (it->second->m_config.AutoSI)
				{
					it->second->SendMsg((string)CT2A(it->second->m_config.SI));	
					it->second->m_cs.Lock();
					it->second->m_config.Count += 2;
					it->second->m_cs.Unlock();
					ShowInfo::SendShowInfo(3, it->first, it->second->m_config.SI, it->second->m_config.Count);
					Sleep(5 * 1000);
				}
			}

			Sleep(100);

			CTime curTime = CTime::GetCurrentTime();
			if (curTime.GetDay() == 1)
			{

				FILE* file = NULL;
				TCHAR szFilePath[1024] = { 0 };
				_stprintf_s(szFilePath, _T("%s\\logs\\%s配置流量.csv"), Global::szAppPath, curTime.Format(_T("%Y年%m月")));
				errno_t no = _tfopen_s(&file, szFilePath, _T("w"));
				if (no == 0)
				{
					CConfigData ccd;
					vector<Config*> configs = ccd.LoadConfig();
					fprintf_s(file, "%s", "配置名称,当月流量,总流量,剩余流量\n");
					for (auto it = configs.begin(); it != configs.end(); it++)
					{
						int ncount = (*it)->Count;
						int nmax = (*it)->MaxCount;
						fwprintf_s(file, _T("%s,%d,%d,%d\n"), (*it)->UserName, ncount, nmax, nmax - ncount);
						CView* pView = Global::GetView((*it)->UserName);
						if (pView)
						{
							
							(*it)->Count = 0;
							ccd.UpdateConfig(**it);

							auto cit = Global::configlist.find((*it)->UserName);
							if (cit != Global::configlist.end()) cit->second->m_config = **it;

							::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString((*it)->UserName)));
							Sleep(5000);
							::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString((*it)->UserName)));
														
						}
						Sleep(5000);
						delete (*it);
					}
					configs.clear();
					fclose(file);
				}
			}
		}
		__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
		{
			continue;
		}
	}
	return 0;
}

inline string GetEtermState(ETERM_STATE nret)
{
	switch (nret)
	{
	case NO_FIND_CONFIG:
		return "没有发现可用配置";
	case CONFIG_INVALID:
		return "当前配置不可用";
	case STRING_FORMAT:
		return "变量赋值错误";
	case CONFIG_LOCKED:
		return "当前配置已锁定";
	case INT_FORMAT:
		return "int变量赋值错误";
	case FLOAT_FORMAT:
		return "float变量赋值错误";
	case LIST_FORMAT:
		return "list变量赋值错误";
	case SET_VALUE:
		return "设置变量失败";
	case INVALID_BUSINESS:
		return "无效的业务名称";
	case NONE_TYPE:
		return "未知类型";
	case SUCCESS:
		return "";
	default:
		return "未知错误";
	}
}

inline RespnseFormat GetForamt(wstring format)
{
	if (format == _T("text")) return text;
	else if (format == _T("json")) return json;
	else return text;
}

inline EtermLanguage GetLanguage(wstring lang)
{
	if (lang == _T("Script")) return Script;
	else if (lang == _T("CSharp")) return CSharp;
	else if (lang == _T("CPP")) return CPP;
	else return Script;
}

unsigned int _stdcall CheckUnlock(void* lParam);

void DataDispose(SOCKET sock, wstring ip, wstring content, wstring source)
{
	__try
	{
		EtermLanguage language = Script;
		ETERM_STATE	nret = NONE;
		wstring config;
		wstring SessionId;
		RespnseFormat format = text;
		unsigned int nlock = 0;
		bool bUnlock = false;

		if (!content.empty())
		{
			stringEx ex;
			language = GetLanguage(ex.GetParam(content, _T("language"), true));
			config = ex.GetParam(content, _T("USING"), true);
			format = GetForamt(ex.GetParam(content, _T("FORMAT"), true));
			bUnlock = ex.GetParam(content, _T("unlock"), true) == _T("true");
			nlock = _ttoi(ex.GetParam(content, _T("lock"), true).c_str());
			SessionId = ex.GetParam(content, _T("SessionId"), true);

			if (language == Script)
			{
				wstring method = ex.GetParam(content, _T("METHOD"), true);

				if (!method.empty())
				{
					auto it = Global::scripts.find(method);
					if (it != Global::scripts.end())
					{
						wstring source = it->second;
						vector<wstring> params = ex.FindStrs(content, _T("param\\d+=.*?(&|$)"), _T("^param|&"));
						for (int i = 0; i < params.size(); i++)
						{
							int npos = params[i].find(_T("="));
							if (npos == wstring::npos) continue;
							wstring skey = _T("｛") + params[i].substr(0, npos) + _T("｝");
							wstring svalue = params[i].substr(npos + 1);
							source = ex.replace(source, skey, svalue);
						}
						source = source;

					}
					else
					{
						if (method == _T("EtermConfig"))
						{
							RemotingAutoConfig(content, sock);
							return;
						}
						else if (method == _T("StreamCount"))
						{
							EtermStreamCount(sock);
							return;
						}
						else
							nret = INVALID_BUSINESS;
					}
				}
				else
				{
					vector<wstring> contents = ex.split(content, _T(";"));
					if (source.empty()) source.append(_T("return DATAS;"));
					for (int i = contents.size() - 1; i >= 0; i--)
					{
						if (!contents[i].empty())
						{
							source.insert(0, _T("system(\"") + contents[i] + _T("\");\r\n"));
						}
					}
				}
			}
		}
		//重复次数
		int nreload = 0;
	reload:
		bool bUseConfig = false;
		//Global::m_cs.Lock();
		if (Global::configlist.size() > 0)
		{
			if (!config.empty())
			{
				auto it = Global::configlist.find(config.c_str());
				if (it == Global::configlist.end())
				{
					config = _T("");
				}
				bUseConfig = !config.empty();
				nret = config == _T("") ? NO_FIND_CONFIG : NONE;
			}
			else
			{
				vector<wstring> configs;
				for (auto it = Global::configlist.begin(); it != Global::configlist.end(); it++)
				{
					if (it->second->m_nState == AVAILABLE &&
						it->second->m_config.MaxCount - it->second->m_config.Count >= Threshold&&
						it->second->clients.size() == 0 &&
						it->second->m_lock.lockcount == 0)
					{
						configs.push_back((wstring)it->first);
					}
				}
				if (configs.size() > 0)
				{
					SYSTEMTIME st;
					GetLocalTime(&st);
					srand(st.wMilliseconds);
					config = configs[getrandom(0, configs.size() - 1)];
				}
				else
				{
					for (auto it = Global::configlist.begin(); it != Global::configlist.end(); it++)
					{
						if (it->second->m_nState == AVAILABLE &&
							it->second->m_config.MaxCount - it->second->m_config.Count >= Threshold&&
							it->second->m_lock.lockcount == 0)
						{
							configs.push_back((wstring)it->first);
						}
					}
					if (configs.size() > 0)
					{
						SYSTEMTIME st;
						GetLocalTime(&st);
						srand(st.wMilliseconds);
						config = configs[getrandom(0, configs.size() - 1)];
					}
				}
			}
		}
		//Global::m_cs.Unlock();

		string strResponse;
		Json::Value root;

		if (config.empty())
		{
			nret = NO_FIND_CONFIG;
		}
		else
		{
			auto it = Global::configlist.find(config.c_str());
			if (it != Global::configlist.end())
			{
				if (it->second->m_nState != AVAILABLE)
				{
					nret = CONFIG_INVALID;
				}
				else
				{
					if (nlock > 0)
					{
						//已经被其它用户锁定
						if (
							it->second->m_lock.lockcount > 0 &&
							it->second->m_lock.SessionId != SessionId
							)
						{
							nret = CONFIG_LOCKED;
						}
						else
						{
							it->second->m_lock.lockcount = nlock;
							GUID guid;
							CString strGuid;
							if (S_OK == ::CoCreateGuid(&guid))
							{
								strGuid.Format(_T("%04x%04x%04x%04x%04x%04x%04x%04x"), guid.Data4[0], guid.Data4[1], guid.Data4[2], guid.Data4[3], guid.Data4[4], guid.Data4[5], guid.Data4[6], guid.Data4[7]);
								it->second->m_lock.SessionId = strGuid;
							}
							unsigned int dwThreadId;
							_beginthreadex(NULL, 0, &CheckUnlock, new CString(config.c_str()), 0, &dwThreadId);

						}
					}
					else
					{
						if (bUseConfig &&
							it->second->m_lock.lockcount > 0 &&
							it->second->m_lock.SessionId != SessionId
							)
						{
							if (!bUnlock)
							{
								nret = CONFIG_LOCKED;
							}
							else
							{
								it->second->m_lock.lockcount = 0;
							}
						}
					}


					if (nret == NONE)
					{
						std::unique_lock<std::mutex> locker(it->second->g_lockqueue);

						CData data;
						data.config = config;
						data.format = format;
						data.language = language;
						data.content = content;
						data.ip = ip;
						data.language = language;
						data.sock = sock;
						data.source = source;
						it->second->clients.push_back(data);

						it->second->m_reloadCS.Lock();
						if (it->second->m_nState == CONFIG_STATE::CONNECT_FAIL)
						{
							CView* pView = Global::GetView(it->first);
							if (pView)
							{
								Global::WriteLog(CLog("CONNECT_FAIL,重新连接"));
								PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));
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
							}
						}
						it->second->m_reloadCS.Unlock();

						it->second->g_notified = true;
						it->second->g_queuecheck.notify_one();

						/*DWORD dwResult = ::WaitForSingleObject(it->second->m_hNotify, 60000);
						unsigned int dwThreadId;
						if (dwResult == WAIT_OBJECT_0)
						{
						_beginthreadex(NULL, 0, &_EtermRequestDispose, new CString(it->first), 0, &dwThreadId);
						}
						else
						{
						SetEvent(it->second->m_hNotify);
						_beginthreadex(NULL, 0, &_EtermRequestDispose, new CString(it->first), 0, &dwThreadId);
						}*/

						ShowInfo::SendShowInfo(1, config.c_str(), _T(""), it->second->clients.size());


						CString strLog;
						strLog.Format(_T("请求内容：\r\n%s\r\n%s"), content.c_str(), source.c_str());
						Global::WriteLog(CLog(strLog));

						return;
					}
				}
			}
			else
			{
				nret = NO_FIND_CONFIG;
			}
		}
		//补偿机制
		if (nret == NO_FIND_CONFIG)
		{
			nreload++;
			if (nreload < 30)
			{
				Global::WriteLog(CLog("NO_FIND_CONFIG"));
				Sleep(100);
				goto reload;
			}			
		}
		string strState = GetEtermState(nret);
		if (format == json)
		{
			root["state"] = strState.empty();
			root["reqtime"] = string(CT2A(CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S")));
			Json::Value error;
			error["ErrorCode"] = 1;
			error["ErrorMessage"] = strState;
			root["error"] = error;
			root["config"] = string(CT2A(config.c_str()));
			root["OfficeNo"] = string(CT2A(Global::etermconfig.OfficeNo));
			root["result"] = Json::Value(Json::nullValue);
			strResponse = root.toStyledString();
		}
		else
		{
			strResponse += strState;
		}
		Global::Server.SendMsg(sock, strResponse);


		return;
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		Global::SendSMS(ip.c_str());
		return;
	}
}

unsigned int _stdcall CheckUnlock(void* lParam)
{
	__try
	{
		CString* Config = (CString*)(lParam);
		auto it = Global::configlist.find(*Config);
		if (it != Global::configlist.end())
		{
			while (true)
			{
				Sleep(1);
				it->second->m_lock.lockcount--;
				if (it->second->m_lock.lockcount <= 0)
				{
					it->second->m_lock.lockcount = 0;
					it->second->m_lock.SessionId = _T("");
					break;
				}
			}
		}

		if (Config)
		{
			delete Config;
			Config = nullptr;
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}
}

//远程自动登录处理
UINT RemotingAutoConfig(wstring surl, SOCKET sock)
{
	stringEx ex;
	wstring operat = ex.GetParam(surl, _T("operat"));

	Config* config = new Config;
	config->AutoSI = ex.GetParam(surl, _T("AutoSI")) == _T("True");
	config->Count = _ttoi(ex.GetParam(surl, _T("Count")).c_str());
	config->MaxCount = _ttoi(ex.GetParam(surl, _T("MaxCount")).c_str());
	config->Interval = _ttoi(ex.GetParam(surl, _T("Interval")).c_str());
	config->IsSSL = ex.GetParam(surl, _T("IsSSL")) == _T("True");
	config->PassWord = ex.GetParam(surl, _T("PassWord")).c_str();
	config->Port = _ttoi(ex.GetParam(surl, _T("Port")).c_str());
	config->ServerIP = ex.GetParam(surl, _T("ServerIP")).c_str();
	config->SI = ex.GetParam(surl, _T("SI")).c_str();
	if (config->SI.GetLength() > 2) if (config->SI.CompareNoCase(_T("SI:")) != 0) config->SI.Insert(0, _T("SI:"));
	config->Type = _T("eTerm");
	config->UserName = ex.GetParam(surl, _T("UserName")).c_str();

	if (config->UserName.IsEmpty())
	{
		Global::Server.SendMsg(sock, _T("用户名不能为空"));
		return 0;
	}
	if (operat == _T("add") && config->PassWord.IsEmpty())
	{
		Global::Server.SendMsg(sock, _T("密码不能为空"));
		return 0;
	}
	if (operat == _T("add") && config->AutoSI && config->SI.GetLength()<2)
	{
		Global::Server.SendMsg(sock, _T("SI不能为空"));
		return 0;
	}
	if (operat == _T("add") && config->ServerIP.IsEmpty())
	{
		Global::Server.SendMsg(sock, _T("服务器地址不能为空"));
		return 0;
	}

	CMainFrame *pMain = (CMainFrame *)AfxGetApp()->m_pMainWnd;
	if (pMain)
	{
		LVFINDINFO lvf;
		lvf.flags = LVFI_STRING;
		lvf.psz = config->UserName;
		int nIndex = pMain->m_wndConfigureListBar.m_ConfigureList.FindItem(&lvf);
		if (operat == _T("del"))
		{
			if (nIndex != -1)
			{
				CView* pView = Global::GetView(config->UserName);
				if (pView)
				{
					::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(config->UserName)));
					::SendMessage(pView->GetSafeHwnd(), WM_COMMAND, ID_FILE_CLOSE, NULL);
				}
				pMain->m_wndConfigureListBar.m_ConfigureList.DeleteItem(nIndex);
				Global::Server.SendMsg(sock, _T("true"));
				Global::SetEtermConfig(config->UserName, false);
				CConfigData ccd;
				ccd.DeleteConfig(config->UserName);
			}
			return 0;
		}
		else if (operat == _T("mod"))
		{
			auto fit = Global::configlist.find(config->UserName);
			if (fit != Global::configlist.end())
			{
				fit->second->m_config = *config;
				CConfigData ccd;
				ccd.UpdateConfig(*config);
			}
		}
		if (nIndex == -1)
		{
			nIndex = pMain->m_wndConfigureListBar.m_ConfigureList.GetItemCount();
			pMain->m_wndConfigureListBar.m_ConfigureList.InsertItem(nIndex, config->UserName, 0);

			if (Global::configlist.find(config->UserName) == Global::configlist.end())
			{
				CConfigData ccd;
				if (ccd.AppendConfig(*config))
				{
					CEtermSocket* pSocket = new CEtermSocket;
					pSocket->m_config = *config;
					Global::configlist.insert(make_pair(config->UserName, unique_ptr<CEtermSocket>(pSocket)));
				}
			}
		}

		CString strVal;
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 1, config->Type);
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 2, config->ServerIP);
		strVal.Format(_T("%d"), config->Port);
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 3, strVal);
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 4, config->IsSSL ? _T("true") : _T("false"));
		strVal.Format(_T("%d"), config->Interval);
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 8, strVal);
		strVal.Format(_T("%d"), config->MaxCount - config->Count);
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemText(nIndex, 10, strVal);
		pMain->m_wndConfigureListBar.m_ConfigureList.SetItemData(nIndex, (DWORD_PTR)config);
		CView* pView = Global::GetView(config->UserName);
		if (pView)
		{
			::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(config->UserName)));
		}
		else
		{
			::SendMessage(pMain->GetSafeHwnd(), WM_COMMAND, ID_FILE_NEW, NULL);
		}
		if (!pView)	pView = Global::GetView(config->UserName);
		bool bsuc = false;
		if (pView)
		{
			::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(config->UserName)));
			auto it = Global::configlist.find(config->UserName);
			if (it->second)
			{
				for (int n = 0; n < 100; n++)
				{
					Sleep(100);
					if (it->second->m_nState == CONFIG_STATE::AVAILABLE)
					{
						Global::Server.SendMsg(sock, _T("true"));
						Global::SetEtermConfig(config->UserName, true);
						bsuc = true;
						break;
					}
				}
			}

		}
		Global::Server.SendMsg(sock, _T("fail"));
		Global::WriteLog(CLog("登录失败"));
		if (!bsuc)
		{
			CView* pView = Global::GetView(config->UserName);
			if (pView)
			{
				::PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 2, (LPARAM)(new CString(config->UserName)));
				::SendMessage(pView->GetSafeHwnd(), WM_COMMAND, ID_FILE_CLOSE, NULL);
			}
			pMain->m_wndConfigureListBar.m_ConfigureList.DeleteItem(nIndex);
		}
	}

}

void EtermStreamCount(SOCKET sock)
{
	CConfigData ccd;
	vector<Config*> vec = ccd.LoadConfig();
	CString strInfo;
	strInfo = _T("配置,状态,最大流量,当前流量,剩余流量\n");
	for (auto it = vec.begin(); it != vec.end(); ++it)
	{
		auto fit = Global::configlist.find((*it)->UserName);
		if (fit != Global::configlist.end())
		{
			strInfo.AppendFormat(_T("%s,%s,%d,%d,%d\n"),
				(*it)->UserName,
				fit->second->m_nState != AVAILABLE ? _T("下线") : _T("在线"),
				(*it)->MaxCount,
				(*it)->Count,
				(*it)->MaxCount - (*it)->Count);
		}
	}
	Global::Server.SendMsg(sock, strInfo);

}

void _EtermRequestDispose(CString strConfig)
{
	auto it = Global::configlist.find(strConfig);
	if (it != Global::configlist.end())
	{
		while (!bSystemOver)
		{
			//Sleep(100);

			__try
			{
				std::unique_lock<std::mutex> locker(it->second->g_lockqueue);

				while (!it->second->g_notified)
				{
					it->second->g_queuecheck.wait(locker);
				}

				if (it->second->m_nState == ETERM_OVER) break;

				//---取队列-----------------------------------------------------------------------
				//it->second->m_cs.Lock();
				//if (it->second->clients.size() == 0) continue;
				while (it->second->clients.size() > 0){
					it->second->m_cs.Lock();
					CData data = it->second->clients.front();
					it->second->m_cs.Unlock();

					//---业务处理-----------------------------------------------------------------------
					string strResponse;
					ETERM_STATE	nret = NONE;
					Json::Value root;

					//it->second->m_configCS.Lock();

					__try
					{
						if (data.language == Script)
						{

							CEtermCommand cec;
							cec.m_config = data.config;
							cec.m_sock = data.sock;
							nret = cec.ScanSourceCode(data.source);
							if (data.format == json)
								root["result"] = string(CT2A(cec.m_sret.c_str()));
							else
								strResponse = string(CT2A(cec.m_sret.c_str()));

						}
						else if (data.language == CSharp)
						{
							if (_tcslen(Global::szConnectString) > 0)
							{
								CView* pView = Global::GetView(it->first);
								if (pView != NULL)
								{

									Json::Value post;
									stringEx ex;
									post["Config"] = string(CT2A(data.config.c_str()));
									post["OfficeNo"] = string(CT2A(Global::etermconfig.OfficeNo));
									post["ClassName"] = string(CT2A(ex.GetParam(data.content, _T("method")).c_str()));
									BSTR _result = 0;
									HRESULT hr = Global::pProxy->InvokeEterm(
										(long)pView->GetSafeHwnd(),
										(long)it->second->m_hNotify,
										::_com_util::ConvertStringToBSTR(post.toStyledString().c_str()),
										_bstr_t(data.source.c_str()),
										&_result);
									if (SUCCEEDED(hr)) strResponse = string(_com_util::ConvertBSTRToString(_result));
									if (!strResponse.empty()) nret = SUCCESS;

								}
							}
							else
							{
								root["result"] = Json::Value(Json::nullValue);
							}
						}
					}
					__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
					{
						Global::SendSMS(strConfig);
					}

					string strState = GetEtermState(nret);
					if (data.format == json&&strResponse.empty())
					{
						root["state"] = strState.empty();
						root["reqtime"] = string(CT2A(CTime::GetCurrentTime().Format("%Y-%m-%d %H:%M:%S")));
						Json::Value error;
						error["ErrorCode"] = 1;
						error["ErrorMessage"] = strState;
						root["error"] = error;
						root["config"] = string(CT2A(data.config.c_str()));
						root["OfficeNo"] = string(CT2A(Global::etermconfig.OfficeNo));
						root["SessionId"] = string(CT2A(it->second->m_lock.SessionId.c_str()));
						strResponse = root.toStyledString();
					}
					else
					{
						strResponse += strState;
					}
					Global::Server.SendMsg(data.sock, strResponse);

					//it->second->m_configCS.Unlock();

					it->second->m_cs.Lock();
					it->second->clients.pop_front();
					it->second->m_cs.Unlock();

					ShowInfo::SendShowInfo(1, it->first, _T(""), it->second->clients.size());

					CString strLog;
					strLog.Format(_T("回复内容:\r\n%s"), CA2T(strResponse.c_str()));
					Global::WriteLog(CLog(strLog));
				}
				it->second->g_notified = false;
			}
			__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
			{
				Global::SendSMS(strConfig);
				continue;
			}
		}


		return;
	}
}