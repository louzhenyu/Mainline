#pragma once
#include <list>
#include <deque>
#include "Config.h"
#include "EtermPacket.h"
#include "CAsyncSocketEx\AsyncSocketEx.h"
#include "CAsyncSocketEx\AsyncSslSocketLayer.h"
#include "CAsyncSocketEx\AsyncProxySocketLayer.h"
#include "LockEterm.h"
#include "Data.h"
#include "Script.h"
#include <mutex>
#include <condition_variable>

enum CONFIG_STATE
{
	ETERM_OVER = -1,		//结束 
	INIT = 0,				//初始化 
	CLOSE_CONN = 1,		//关闭连接 
	LOGIN_SUCCESS = 2,	//登录成功 
	CONNECT_FAIL = 3,		//连接失败	
	CONNECTED = 5,			//已连接
	AVAILABLE=6              //已签入或可用
};

class CEtermSocket
	:public CAsyncSocketEx
{
public:
	typedef std::list<unsigned char*> t_HashList;
	CEtermSocket(void);
	CEtermSocket(CRichEditCtrl *pResponse, t_HashList *pSslTrustedCertHashList);
	void InitEtermSocket(CRichEditCtrl *pResponse, t_HashList *pSslTrustedCertHashList);
	void ReleaseEtermSocket();
	~CEtermSocket(void);

	BOOL AddSslCertHashToTrustedList(unsigned char * pHash);
	virtual void OnConnect(int nErrorCode);
	virtual void OnReceive(int nErrorCode);
	virtual void OnClose(int nErrorCode);
	CAsyncSslSocketLayer *m_pSslLayer;
	CAsyncProxySocketLayer *m_pProxyLayer;
	int OnLayerCallback(const CAsyncSocketExLayer *pLayer, int nType, int nParam1, int nParam2);
	void SetSessionID(BYTE* btSessionID);

	//存活时间
	CTime m_ActiveTime;
	Config m_config;
	CEtermPacket m_EtermPacket;
	deque<CData> clients;

	std::mutex              g_lockqueue;
	std::condition_variable g_queuecheck;
	bool                    g_notified;
	CLockEterm m_lock;			   //锁定配置
public:
	bool UserLogin();
	bool SendMsg(string sMsg);
	bool SendPrintMsg(string sMsg, bool bEn = false);

	//客户端临界区
	CCriticalSection m_cs;
	CCriticalSection m_configCS;
	CCriticalSection m_cmdCS;			 //取命令队列时锁定
	CCriticalSection m_reloadCS;
	deque<CScript> cmdlist;			 //命令队列

	CString m_strCmd;
	CString m_strResponse;
	HANDLE pEvent;
	HANDLE m_hNotify;			   //配置执行事件通知
	bool m_bRunXS;		//是否正在执行XS指令
	BOOL SetKeepAlive();
	//EtermSocket状态	
	CONFIG_STATE m_nState;		//-1结束 0初始化 1关闭连接 2登录成功 3连接失败	5已连接 6正在使用
	CTime m_cmdTime;	//当前指今发出时间
	CTime m_lastReboot;	//上次重启时间
	//float m_Rate;		//配置可用率

protected:
	void AddStringToLog(LPCTSTR pszString);
	t_HashList *m_pSslTrustedCertHashList;
	CRichEditCtrl *m_pResponse;
private:
	void UsasToGb(byte& c1, byte& c2);
	void GetCmd(string smsg, BYTE* btCmd, int& nlen);
};

