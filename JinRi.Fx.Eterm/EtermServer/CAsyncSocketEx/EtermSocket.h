#pragma once
#include "Asyncsocketex.h"
#include "AsyncProxySocketLayer.h"
#include "AsyncSslSocketLayer.h"
#include <afxtempl.h>
#include <afxmt.h>
#include <sstream>

class CEtermSocket:
	public CAsyncSocketEx
{
public:
	typedef std::list<unsigned char*> t_HashList;	
	CEtermSocket();
	CEtermSocket(CRichEditCtrl *pResponse,t_HashList *pSslTrustedCertHashList);
	~CEtermSocket(void);

	BOOL AddSslCertHashToTrustedList(unsigned char * pHash);
	virtual void OnConnect(int nErrorCode);
	virtual void OnReceive(int nErrorCode);
	virtual void OnClose(int nErrorCode);
	CAsyncProxySocketLayer *m_pProxyLayer;
	CAsyncSslSocketLayer *m_pSslLayer;
	int OnLayerCallback(const CAsyncSocketExLayer *pLayer, int nType, int nParam1, int nParam2);	
	void SetSessionID(BYTE* btSessionID);
	void InitSocket(CRichEditCtrl *pResponse,t_HashList *pSslTrustedCertHashList);
	void ReleaseSocket(void);
	//存活时间
	CTime m_ActiveTime;
	CConfigure m_config;
	CEtermPacket m_EtermPacket;
public:
	bool UserLogin(CConfigure cfg);
	bool SendMsg(string sMsg);
	bool SendPrintMsg(string sMsg);	
	bool SendPNRMsg(string sMsg);
	deque<ClientData> ClientSocket;

	//客户端临界区
	CCriticalSection m_cs;		
	CString m_strCmd;
	CString m_strResponse;
	HANDLE pEvent;
	HANDLE pReEvent;
	bool m_bRunXS;		//是否正在执行XS指令
	BOOL SetKeepAlive();
	//EtermSocket状态	
	int m_nState;		//-1结束 0初始化 1关闭连接 2登录成功 3连接失败	5已连接
	CTime m_cmdTime;	//当前指今发出时间
	int m_nSleep;		//等待
	float m_Rate;		//配置可用率

protected:
	void AddStringToLog(LPCTSTR pszString);
	t_HashList *m_pSslTrustedCertHashList;
	CRichEditCtrl *m_pResponse;	
	bool UpdateHeartWrap();
	
};
