#include "stdafx.h"
#include "EtermSocket.h"
#include "Global.h"
#include "SocketHelper.h"
#include <algorithm>
#include "log.h"
#include "ShowInfo.h"
#include "stringEx.h"
#include "PrinterDecrypt.h"
#include "PingYingHelper.h"

UINT ReStartEterm(LPVOID lparam);
UINT CheckEtermLogin(LPVOID lparam);

CEtermSocket::CEtermSocket(void)
{
	m_nState = INIT;
	m_lastReboot = CTime::GetCurrentTime();

	m_bRunXS = false;
	pEvent = nullptr;
	m_pSslLayer = nullptr;
	m_pProxyLayer = nullptr;
	m_pSslTrustedCertHashList = nullptr;
}

CEtermSocket::CEtermSocket(CRichEditCtrl *pResponse, t_HashList *pSslTrustedCertHashList)
{
	__try
	{
		m_nState = INIT;
		m_bRunXS = false;
		pEvent = nullptr;
		m_hNotify = nullptr;
		m_pSslLayer = nullptr;
		m_pProxyLayer = nullptr;
		m_pSslTrustedCertHashList = nullptr;

		ASSERT(pResponse);
		ASSERT(pSslTrustedCertHashList);

		m_nState = INIT;

		m_pResponse = pResponse;
		m_pSslTrustedCertHashList = pSslTrustedCertHashList;

		m_pSslLayer = new CAsyncSslSocketLayer;
		m_pProxyLayer = new CAsyncProxySocketLayer;
		m_ActiveTime = CTime::GetCurrentTime();
		pEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
		m_hNotify = CreateEvent(NULL, TRUE, FALSE, _T("ConfigNotify"));
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}
}

void CEtermSocket::ReleaseEtermSocket()
{
	__try
	{

		Close();

		if (pEvent != nullptr)
		{
			CloseHandle(pEvent);
			pEvent = nullptr;
		}
		if (m_hNotify != nullptr)
		{
			CloseHandle(m_hNotify);
			m_hNotify = nullptr;
		}
		if (m_pSslLayer != nullptr)
		{
			delete m_pSslLayer;
			m_pSslLayer = nullptr;
		}
		if (m_pProxyLayer != nullptr)
		{
			delete m_pProxyLayer;
			m_pProxyLayer = nullptr;
		}
		/*if (m_pSslTrustedCertHashList != nullptr)
		{
			for (t_HashList::iterator iter = m_pSslTrustedCertHashList->begin(); iter != m_pSslTrustedCertHashList->end(); iter++)
				delete[] * iter;
			m_pSslTrustedCertHashList->clear();
		}*/
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}

}
void CEtermSocket::InitEtermSocket(CRichEditCtrl *pResponse, t_HashList *pSslTrustedCertHashList)
{
	
	__try
	{
		m_nState = INIT;
		m_bRunXS = false;
		pEvent = nullptr;
		m_hNotify = nullptr;
		m_pSslLayer = nullptr;
		m_pProxyLayer = nullptr;
		m_pSslTrustedCertHashList = nullptr;

		ASSERT(pResponse);
		ASSERT(pSslTrustedCertHashList);

		m_nState = INIT;
		m_pResponse = pResponse;
		m_pSslTrustedCertHashList = pSslTrustedCertHashList;

		m_pSslLayer = new CAsyncSslSocketLayer;
		m_pProxyLayer = new CAsyncProxySocketLayer;
		m_ActiveTime = CTime::GetCurrentTime();
		pEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
		m_hNotify = CreateEvent(NULL, TRUE, FALSE, _T("ConfigNotify"));
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}
}

CEtermSocket::~CEtermSocket()
{
	__try
	{

		Close();

		if (pEvent != nullptr)
		{
			CloseHandle(pEvent);
			pEvent = nullptr;
		}
		if (m_hNotify != nullptr)
		{
			CloseHandle(m_hNotify);
			m_hNotify = nullptr;
		}
		if (m_pSslLayer != nullptr)
		{
			delete m_pSslLayer;
			m_pSslLayer = nullptr;
		}
		if (m_pProxyLayer != nullptr)
		{
			delete m_pProxyLayer;
			m_pProxyLayer = nullptr;
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
	}
}

void CEtermSocket::OnReceive(int nErrorCode)
{
	if (nErrorCode)
	{
		Close();
		return;
	}

	BYTE btData[40960];
	UINT nSize = 0;
	nSize = Receive(btData, 40960);
	if (nSize == -1 || nSize>=40960) return;

#ifdef DEBUG
	CString strLog(_T("recv:\r\n"));
	for (int i = 0; i < nSize; i++)
	{
		strLog.AppendFormat(_T("%02X "), btData[i]);
	}
	Global::WriteLog(CLog(strLog));
#endif

	if (!this->m_EtermPacket.ValidatePakcet(btData, nSize))
	{
		return;
	}		

	if (nSize != SOCKET_ERROR)
	{
		__try
		{
			this->m_ActiveTime = CTime::GetCurrentTime();
			this->m_EtermPacket.ParseData(btData, nSize);			
						
			if (this->m_EtermPacket.m_pt == PACKET_TYPE::LOGIN)
			{
				SetEvent(this->pEvent);
				this->m_nState = LOGIN_SUCCESS;
			}
			else if (this->m_EtermPacket.m_pt == PACKET_TYPE::COMPLETE)
			{
				this->m_strResponse += this->m_EtermPacket.m_strResponse;
				
				if (this->m_nState == LOGIN_SUCCESS)
				{
					this->m_nState = AVAILABLE;					
				}

				if (this->m_EtermPacket.m_strResponse.Find(_T("SESSION CURRENTLY LOCKED")) == -1)
				{
					SetEvent(pEvent);
				}

			}
			else
			{
				this->m_strResponse = this->m_EtermPacket.m_strResponse;
			}
		}
		__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
		{
	
		}
	}

	if (!m_strResponse.IsEmpty()) AddStringToLog(m_strResponse);
	
}

void CEtermSocket::OnConnect(int nErrorCode)
{
	if (0 == nErrorCode)
	{
		this->m_nState = CONNECTED;		//已连接
		CView* pView = Global::GetView(this->m_config.UserName);
		if (pView)
		{
			if (!m_config.IsSSL)
			{
				PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 1, (LPARAM)(new CString(this->m_config.UserName)));
			}			
		}
		ShowInfo::SendShowInfo(0, m_config.UserName, _T("正在登录……"));
	}	
}

void CEtermSocket::OnClose(int nErrorCode)
{
	//if (nErrorCode == 0)
	{		
		if (this->m_nState == AVAILABLE)
		{			
			AfxBeginThread(ReStartEterm, (LPVOID)(new CString(this->m_config.UserName)));
		}		
	}
}

int CEtermSocket::OnLayerCallback(const CAsyncSocketExLayer *pLayer, int nType, int nParam1, int nParam2)
{
	__try
	{
		ASSERT(pLayer);

		if (nType == LAYERCALLBACK_STATECHANGE)
		{
#ifdef DEBUG
			CString str;
			if (pLayer == m_pProxyLayer)
				str.Format(_T("Layer Status: m_pProxyLayer changed state from %d to %d"), nParam2, nParam1);
			else if (pLayer == m_pSslLayer)
				str.Format(_T("Layer Status: m_pSslLayer changed state from %d to %d"), nParam2, nParam1);
			else
				str.Format(_T("Layer Status: Layer @ %d changed state from %d to %d"), pLayer, nParam2, nParam1);

			AddStringToLog(str);
#endif
			/*if (nParam2==4&&this->m_nState==AVAILABLE)
			{
				AfxBeginThread(ReStartEterm, (LPVOID)(new CString(this->m_config.UserName)));
			}*/
			return 1;
		}
		else if (nType == LAYERCALLBACK_LAYERSPECIFIC)
		{
			if (pLayer == m_pProxyLayer)
			{
#ifdef DEBUG
				switch (nParam1)
				{
				case PROXYERROR_NOCONN:
					AddStringToLog(_T("Proxy error: Can't connect to proxy server."));
					break;
				case PROXYERROR_REQUESTFAILED:
					AddStringToLog(_T("Proxy error: Proxy request failed, can't connect through proxy server."));
					if (nParam2)
						AddStringToLog((LPCTSTR)nParam2);
					break;
				case PROXYERROR_AUTHTYPEUNKNOWN:
					AddStringToLog(_T("Proxy error: Required authtype reported by proxy server is unknown or not supported."));
					break;
				case PROXYERROR_AUTHFAILED:
					AddStringToLog(_T("Proxy error: Authentication failed"));
					break;
				case PROXYERROR_AUTHNOLOGON:
					AddStringToLog(_T("Proxy error: Proxy requires authentication"));
					break;
				case PROXYERROR_CANTRESOLVEHOST:
					AddStringToLog(_T("Proxy error: Can't resolve host of proxy server."));
					break;
				default:
					AddStringToLog(_T("Proxy error: Unknown proxy error"));
				}
#endif
			}
			else if (pLayer == m_pSslLayer)
			{
				switch (nParam1)
				{
				case SSL_INFO:
					switch (nParam2)
					{
					case SSL_INFO_ESTABLISHED:
#ifdef DEBUG
						AddStringToLog(_T("SSL connection established"));
#endif
						CView* pView = Global::GetView(this->m_config.UserName);
						if (pView)	PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 1, (LPARAM)(new CString(this->m_config.UserName)));
						break;
					}
					break;
				case SSL_FAILURE:
#ifdef DEBUG
					switch (nParam2)
					{
					case SSL_FAILURE_UNKNOWN:
						AddStringToLog(_T("Unknown error in SSL layer"));
						break;
					case SSL_FAILURE_ESTABLISH:
						AddStringToLog(_T("Could not establish SSL connection"));
						break;
					case SSL_FAILURE_LOADDLLS:
						AddStringToLog(_T("Failed to load OpenSSL DLLs"));
						break;
					case SSL_FAILURE_INITSSL:
						AddStringToLog(_T("Failed to initialize SSL"));
						break;
					case SSL_FAILURE_VERIFYCERT:
						AddStringToLog(_T("Could not verify SSL certificate"));
						break;
					}
#endif
					break;
				case SSL_VERBOSE_INFO:
				case SSL_VERBOSE_WARNING:
#ifdef DEBUG
					AddStringToLog(CString(CA2T((char*)nParam2)));
#endif
					break;
				case SSL_VERIFY_CERT:
#ifdef DEBUG
					AddStringToLog(_T("SSL_VERIFY_CERT"));
#endif
					t_SslCertData *pData = new t_SslCertData;
					if (m_pSslLayer->GetPeerCertificateData(*pData))
					{
						/* Do a first validation of the certificate here, return 1 if it's valid
						* and 0 if not.
						* If still unsure, let the user decide and return 2;
						*/
						for (t_HashList::iterator iter = m_pSslTrustedCertHashList->begin(); iter != m_pSslTrustedCertHashList->end(); iter++)
						if (!memcmp(pData->hash, *iter, 20))
						{
#ifdef DEBUG
							AddStringToLog(_T("ssl verify cert return 1."));
#endif
							return 1;
						}							

						CView* pView = Global::GetView(this->m_config.UserName);
						if (pView)
						{
							PostMessage(pView->GetSafeHwnd(), WM_USER, (WPARAM)pData, (LPARAM)(new CString(this->m_config.UserName)));
#ifdef DEBUG
							AddStringToLog(_T("ssl verify cert PostMessage."));
#endif
						}
							
						return 2;
					}
					else
					{
#ifdef DEBUG
						AddStringToLog(_T("ssl verify cert GetPeerCertificateData error."));
#endif
						return 1;
					}
						
				}
			}
		}
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 1;
	}

	return 1;
}


BOOL CEtermSocket::AddSslCertHashToTrustedList(unsigned char * pHash)
{
	__try
	{
		if (!pHash)
			return FALSE;
		for (t_HashList::iterator iter = m_pSslTrustedCertHashList->begin(); iter != m_pSslTrustedCertHashList->end(); iter++)
		if (!memcmp(pHash, *iter, 20))
			return FALSE;

		unsigned char* pData = new unsigned char[20];
		memcpy(pData, pHash, 20);
		m_pSslTrustedCertHashList->push_back(pData);

	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		
	}
	return TRUE;

}


void CEtermSocket::AddStringToLog(LPCTSTR pszString)
{
	__try
	{	
		if (m_pResponse->GetLineCount() > 1000)
		{
			m_pResponse->SetSel(0, -1);
			m_pResponse->Clear();
		}

		m_pResponse->SetSel(-1, -1);
		m_pResponse->ReplaceSel(pszString);
				
		m_pResponse->SetSel(-1, -1);
		m_pResponse->ReplaceSel(_T("\r\n"));

		
		Global::WriteLog(CLog(pszString,this->m_config.UserName));
		/*CHARFORMAT cf;
		ZeroMemory(&cf, sizeof(CHARFORMAT));
		cf.cbSize = sizeof(CHARFORMAT);
		cf.dwMask = CFM_COLOR;
		cf.crTextColor = RGB(255, 0, 0);

		long lstart, lend;
		FINDTEXTEX ft;
		ft.chrg.cpMin = m_pResponse->GetTextLength();
		ft.chrg.cpMax = 0;
refind:
		ft.lpstrText = _T("『");
		long lPos = m_pResponse->FindText(0, &ft);
		if (lPos >= 0)
		{
			lstart = lPos;			
			ft.lpstrText = _T("』");
			lPos = m_pResponse->FindText(0, &ft);
			if (lPos >= 0)
			{
				lend = lPos + 1;
				m_pResponse->SetSel(lstart, lend);
				m_pResponse->SetSelectionCharFormat(cf);
				ft.chrg.cpMin = lstart-1;
				goto refind;
			}
		}
		m_pResponse->SetSel(-1, -1);*/
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		
	}
}

//用户登录
bool CEtermSocket::UserLogin()
{
	USES_CONVERSION;

	int ret;

	BYTE str0[162] = { 0 };
	BYTE str1[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	BYTE str2[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x29, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	
	//BYTE str3[] = { 0x01, 0xF9, 0x00, 0x44, 0x00, 0x01, 0x1B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

	//3849310
	BYTE bVer[] = { 0x33, 0x38, 0x34, 0x39, 0x33, 0x31, 0x30, 0x00, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
	
	
	//BYTE bt443_11[] = { 0x01,0xF7,0x01,0x04,0x33,0x38,0x34,0x39,0x33,0x31,0x30,0x00,0x00,0x00,0x00,0x00 };
	//BYTE bt443_1[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };	
	//BYTE bt443_2[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	//BYTE bt443_3[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x29, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	//
	//BYTE bt443_22[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	//BYTE bt443_33[] = { 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xFE, 0x00, 0x11, 0x14, 0x10, 0x00, 0x02, 0x29, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	BYTE bt443_3[] = { 0x01, 0xF7, 0x01, 0x04, 0x33, 0x38, 0x34, 0x39, 0x33, 0x31, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	BYTE bt443_4[] = { 0x01, 0xF9, 0x00, 0x44, 0x00, 0x01, 0x1B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

	CSocketHelper socketHelper;

	int nlen = 0;

	memset(str0, 0x00, 162);

	//两位魔术数据
	BYTE magic[] = { 0x01, 0xA2 };
	memcpy(str0, (void*)magic, _countof(magic));
	nlen += 2;

	char* szUserName = T2A(m_config.UserName);
	char* szPassWord = T2A(m_config.PassWord);

	//16位用户名
	memcpy(&str0[nlen], (void*)szUserName, strlen(szUserName));
	nlen += 16;

	//16位密码
	memcpy(&str0[nlen], (void*)szPassWord, strlen(szPassWord));
	nlen += 16;

	//16位空位
	nlen += 16;

	//网卡地址
	if (strlen(Global::szMac) == 0) strcpy(Global::szMac, socketHelper.GetMAC());
	memcpy(&str0[nlen], (void*)Global::szMac, strlen(Global::szMac));
	nlen += 12;

	//IP地址
	if (strlen(Global::szHostIP) == 0) strcpy(Global::szHostIP, socketHelper.GetHostIP());
	BYTE btIP[15] = { 0 };
	memset(btIP, 0x20, sizeof(btIP));
	memcpy(btIP, (void*)Global::szHostIP, strlen(Global::szHostIP));
	memcpy(&str0[nlen], btIP, sizeof(btIP));
	nlen += 15;

	//版本号
	memcpy(&str0[nlen], (void*)bVer, _countof(bVer));
	//Global::WriteFlow(cfg.UserName);
#ifdef DEBUG
	CString strLog(_T("login:\r\n"));
	for (int i = 0; i < 162; i++)
	{
		strLog.AppendFormat(_T("%02X "), str0[i]);
	}
	Global::WriteLog(CLog(strLog));
#endif

	//BYTE bttest[] = { 0x01,0xA2,0x42,0x39,0x39,0x33,0x35,0x31,0x34,0x32,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x67,0x78,0x70,0x31,0x32,0x33,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x0000,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x30,0x30,0x30,0x63,0x32,0x39,0x33,0x33,0x66,0x38,0x66,0x36,0x31,0x39,0x32,0x2E,0x31,0x36,0x38,0x2E,0x36,0x2E,0x31,0x33,0x39,0x20,0x20,0x33,0x38,0x34,0x39,0x33,0x31,0x30,0x00,0x30,0x30,0x30,0x30,0x30,0x30,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00 };
	//              //{ 0x01, 0xA2, 0x6D, 0x63, 0x33, 0x33, 0x31, 0x35, 0x33, 0x35, 0x75, 0x35, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x69, 0x75, 0x7A, 0x68, 0x69, 0x79, 0x6F, 0x6E, 0x67, 0x33, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x30, 0x30, 0x63, 0x32, 0x39, 0x33, 0x33, 0x66, 0x38, 0x66, 0x36, 0x31, 0x39, 0x32, 0x2E, 0x31, 0x36, 0x38, 0x2E, 0x36, 0x2E, 0x31, 0x33, 0x39, 0x20, 0x20, 0x33, 0x38, 0x34, 0x39, 0x33, 0x31, 0x30, 0x00, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
	//memset(str0, 0, sizeof(str0));
	//memcpy(str0, bttest, sizeof(bttest));

	ret = Send(str0, sizeof(str0));
	if (ret == -1) return false;
	ResetEvent(pEvent);
	DWORD dwOutTime = ::WaitForSingleObject(pEvent, 30 * 1000);
	if (dwOutTime != WAIT_OBJECT_0)
	{
		SetEvent(pEvent);
		return false;
	}
#ifdef DEBUG
	Global::WriteLog(CLog("str1"));
#endif
	ret = Send(str1, sizeof(str1)); if (ret == -1) return false;
#ifdef DEBUG
	Global::WriteLog(CLog("str2"));
#endif
	ret = Send(str2, sizeof(str2)); if (ret == -1) return false;
		
	if (this->m_config.IsSSL)
	{
		ResetEvent(pEvent);
		dwOutTime = ::WaitForSingleObject(pEvent, 10 * 1000);
		if (dwOutTime != WAIT_OBJECT_0)
		{
			SetEvent(pEvent);
			return false;
		}
		if (this->m_EtermPacket.m_big == 2)
		{
#ifdef DEBUG
			Global::WriteLog(CLog("bt443_3"));
#endif
			ret = Send(bt443_3, sizeof(bt443_3)); if (ret == -1) return false;
#ifdef DEBUG
			Global::WriteLog(CLog("bt443_4"));
#endif
			ret = Send(bt443_4, sizeof(bt443_4)); if (ret == -1) return false;
		}
	}		

	return true;
}

void CEtermSocket::UsasToGb(byte& c1, byte& c2)
{
	if (c2-10>0xA4 && c2-10<0xA9)
	{
		byte temp = c2;
		c2 = c1;
		c1 = temp-10;
	}
	if (c1>0xA4)
	{
		c1 -= 0x80;
	}
	else
	{
		c1 -= 0x8E;
	}

	c2 -= 0x80;
}

void CEtermSocket::GetCmd(string smsg, BYTE* btCmd,int& nlen)
{
	BYTE btPrefix[] = { 0x1B, 0x0E };
	BYTE btSuffix[] = { 0x1B, 0x0F };

	bool bhz = false;
	int n = smsg.length();
	for (int i = 0; i < n; i++)
	{
		if (~(smsg.at(i) >> 8) == 0)
		{
			if (!bhz)
			{
				memcpy(&btCmd[nlen], btPrefix, _countof(btPrefix));
				nlen += _countof(btPrefix);
				bhz = true;
			}
			if (i + 1 < n)
			{
				BYTE b1 = smsg.at(i);
				BYTE b2 = smsg.at(i + 1);
				UsasToGb(b1, b2);

				btCmd[nlen] = b1;
				nlen++;
				btCmd[nlen] = b2;
				nlen++;
				i++;
			}
		}
		else
		{
			if (bhz)
			{
				memcpy(&btCmd[nlen], btSuffix, _countof(btSuffix));
				nlen += _countof(btSuffix);
				bhz = false;
			}
			btCmd[nlen] = smsg.at(i);
			nlen++;
		}
	}
}


//发送数据
bool CEtermSocket::SendMsg(string sMsg)
{
	if (sMsg.empty())
		return false;
	

	PingYingHelper pyHelper;
	vector<BYTE> bts = pyHelper.GetPingYin(sMsg);

	//BYTE btMsg[4096] = { 0 };
	//int nclen = 0;
	

	//前缀1
	BYTE prefix1[] = { 0x01, 0x00 };
	//前缀2
	//BYTE prefix2[] = { 0x00, 0x00, 0x00, 0x01 };
	//前缀3
	//BYTE prefix[] = { 0x51, 0x70, 0x02, 0x1B, 0x0B, 0x23, 0x20, 0x00, 0x0F, 0x1E };
	//BYTE prefix[] = { 0x51, 0x70, 0x02, 0x1B, 0x0B, 0x20, 0x20, 0x00, 0x0F, 0x1E };
	//BYTE prefix350[] = { 0x00, 0x00, 0x00, 0x01, 0x39, 0x51, 0x70, 0x02, 0x1B, 0x0B, 0x20, 0x20, 0x00, 0x0F, 0x1E }; //{ 0x00, 0x00, 0x00, 0x01, 0x51, 0x40, 0x70, 0x02, 0x1B, 0x0B, 0x20, 0x20, 0x00, 0x0F, 0x1E };
	//BYTE prefix443[] = { 0x00, 0x00, 0x00, 0x01, 0x21, 0x51, 0x70, 0x02, 0x1B, 0x0B, 0x20, 0x20, 0x00, 0x0F, 0x1E };
	BYTE prefix[] = { 0x00, 0x00, 0x00, 0x01, 0x21, 0x51, 0x70, 0x02, 0x1B, 0x0B, 0x20, 0x20, 0x00, 0x0F, 0x1E };
	
	//后缀
	BYTE suffix[] = { 0x20, 0x03 };

	BYTE cmd[4096] = { 0 };

	//前缀1
	int nlen = _countof(prefix1);
	memcpy(cmd, prefix1, nlen);

	//数据包长度
	byte cmdLen[4] = { 0 };
	byte cmdL[4] = { 0 };
	*(int*)cmdLen = bts.size() + 21;
	cmdL[0] = cmdLen[1];
	cmdL[1] = cmdLen[0];
	memcpy(&cmd[nlen], cmdL, 2);
	nlen += 2;

	prefix[4] = this->m_EtermPacket.SID;
	memcpy(&cmd[nlen], prefix, _countof(prefix));
	nlen += _countof(prefix);

	/*if (this->m_config.IsSSL)
	{
		prefix443[4] = this->m_EtermPacket.SID;
		memcpy(&cmd[nlen], prefix443, _countof(prefix443));
		nlen += _countof(prefix443);
	}
	else
	{
		prefix350[4] = this->m_EtermPacket.SID;
		memcpy(&cmd[nlen], prefix350, _countof(prefix350));
		nlen += _countof(prefix350);
	}*/
	//前缀2
	//memcpy(&cmd[nlen], prefix2, _countof(prefix2));
	//nlen += _countof(prefix2);

	//SID
	//memcpy(&cmd[nlen], &this->m_EtermPacket.SID, 1);
	//nlen += 1;

	//前缀3
	//memcpy(&cmd[nlen], prefix, _countof(prefix));
	//nlen += _countof(prefix);

	//命令
	memcpy(&cmd[nlen], &bts[0], bts.size());
	nlen += bts.size();

	//后缀	
	memcpy(&cmd[nlen], suffix, _countof(suffix));
	nlen += _countof(suffix);

	int ret = Send(cmd, nlen);
		
#ifdef DEBUG
	CString strLog(_T("send:\r\n"));
	for (int i = 0; i < nlen; i++)
	{
		strLog.AppendFormat(_T("%02X "), cmd[i]);
	}
	Global::WriteLog(CLog(strLog)); 
#endif

	if (ret == -1)
		return false;
	else
		return true;
}

//发送数据(打印)
bool CEtermSocket::SendPrintMsg(string sMsg, bool bEn)
{
	CPrinterDecrypt pd;
	

	//前缀1
	BYTE prefix1[] = { 0x01, 0x00, 0x00 };
	//前缀2
	BYTE prefix2[] = { 0x0C, 0x00, 0x00, 0x01, 0x8C, 0x0C, 0x00, 0x02, 0x49, 0x54, 0x49, 0x4E, 0x3A, 0x54, 0x4E, 0x2F };
	//前缀3
	BYTE prefix3[] = { 0x0C, 0x00, 0x02, 0x49, 0x54, 0x49, 0x4E, 0x3A, 0x54, 0x4E, 0x2F };
	//后缀           
	BYTE suffixCN[] = { 0x2C, 0x7B, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x3D, 0x74, 0x69, 0x70, 0x42, 0x3B, 0x6C, 0x61, 0x6E, 0x67, 0x75, 0x61, 0x67, 0x65, 0x3D, 0x43, 0x4E, 0x3B, 0x45, 0x54, 0x3D, 0x59, 0x7D, 0x03 };
	BYTE suffixEN[] = { 0x2C, 0x7B, 0x73, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x3D, 0x74, 0x69, 0x70, 0x42, 0x3B, 0x6C, 0x61, 0x6E, 0x67, 0x75, 0x61, 0x67, 0x65, 0x3D, 0x45, 0x4E, 0x3B, 0x45, 0x54, 0x3D, 0x59, 0x7D, 0x03 };

	BYTE cmd[1024];

	//前缀1	
	int nlen = _countof(prefix1);
	memcpy(cmd, prefix1, nlen);

	//数据包长度	
	char cmdLen[5];
	sprintf(cmdLen, "%c", sMsg.length() + 52);
	memcpy(&cmd[nlen], cmdLen, 1);
	nlen += 1;

	//前缀2
	memcpy(&cmd[nlen], prefix2, _countof(prefix2));
	nlen += _countof(prefix2);

	//命令	
	memcpy(&cmd[nlen], (char*)sMsg.c_str(), sMsg.length());
	nlen += sMsg.length();

	//后缀
	if (bEn)
	{
		memcpy(&cmd[nlen], suffixEN, _countof(suffixEN));
		nlen += _countof(suffixEN);
	}
	else
	{
		memcpy(&cmd[nlen], suffixCN, _countof(suffixCN));
		nlen += _countof(suffixCN);
	}

	int ret = Send((char*)cmd, nlen);
	if (ret == -1)
		return false;
	else
		return true;
}

BOOL CEtermSocket::SetKeepAlive()
{
	// 开启KeepAlive
	BOOL bKeepAlive = TRUE;
	int nRet = ::setsockopt(this->GetSocketHandle(), SOL_SOCKET, SO_KEEPALIVE, (char*)&bKeepAlive, sizeof(bKeepAlive));
	if (nRet == SOCKET_ERROR)
	{
		//PRINTDEBUG(WSAGetLastError());
		return FALSE;
	}

	// 设置KeepAlive参数
	tcp_keepalive alive_in = { 0 };
	tcp_keepalive alive_out = { 0 };
	alive_in.keepalivetime = 10000;                // 开始首次KeepAlive探测前的TCP空闭时间
	alive_in.keepaliveinterval = 10000;              // 两次KeepAlive探测间的时间间隔
	alive_in.onoff = TRUE;

	unsigned long ulBytesReturn = 0;
	nRet = WSAIoctl(this->GetSocketHandle(), SIO_KEEPALIVE_VALS, &alive_in, sizeof(alive_in), &alive_out, sizeof(alive_out), &ulBytesReturn, NULL, NULL);
	if (nRet == SOCKET_ERROR)
	{
		//PRINTDEBUG(WSAGetLastError());
		return FALSE;
	}

	return TRUE;
}

UINT ReStartEterm(LPVOID lparam)
{
	__try
	{
		CString* strConfig = (CString*)lparam;
		auto it = Global::configlist.find(*strConfig);
		if (it != Global::configlist.end())
		{
			it->second->m_cs.Lock();
			it->second->m_nState = CONFIG_STATE::CONNECT_FAIL;
			it->second->m_cs.Unlock();

			ShowInfo::SendShowInfo(0, it->second->m_config.UserName, _T("服务器断开连接"));

			//if (it->second->m_nState != CONFIG_STATE::CLOSE_CONN)
			{				
				CTimeSpan ts = CTime::GetCurrentTime()-it->second->m_lastReboot;
				int nSleep = (it->second->m_config.IsSSL ? 300 : 30) - ts.GetTotalSeconds();
				if (nSleep > 0)
				{
					CString strInfo;
					for (int i = 0; i < nSleep; i++)
					{
						if (it->second->m_nState == CONFIG_STATE::CLOSE_CONN)
						{
							ShowInfo::SendShowInfo(0, it->second->m_config.UserName, _T("服务器连接已断开"));
							goto exit;
						}
						Sleep(1000);
						strInfo.Format(_T("%d秒后开始重启……"), nSleep - i);
						ShowInfo::SendShowInfo(0, it->second->m_config.UserName, strInfo);
					}					
				}
				
				it->second->m_lastReboot = CTime::GetCurrentTime();
				CView* pView = Global::GetView(it->first);
				if (pView)
				{
					PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));
					Global::WriteLog(CLog(_T("重新启动"),it->first));
				}
				else
				{
					Global::WriteLog(CLog(_T("重新启动失败!"),it->first));
				}
			}
		}
		exit:
		if (strConfig)
		{
			delete strConfig;
			strConfig = nullptr;
		}
		return 0;
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}
}

UINT CheckEtermLogin(LPVOID lparam)
{
	__try
	{
		CString* strConfig = (CString*)lparam;
		auto it = Global::configlist.find(*strConfig);
		if (it != Global::configlist.end())
		{
			Sleep(15 * 1000);

			if (it->second->m_nState != AVAILABLE)
			{
				CView* pView = Global::GetView(it->first);
				if (pView)
				{
					PostMessage(pView->GetSafeHwnd(), WM_CONNECT, 0, (LPARAM)(new CString(it->first)));
					Global::WriteLog(CLog(_T("重新启动"), it->first));
				}
				else
				{
					Global::WriteLog(CLog(_T("重新启动失败!"), it->first));
				}
			}
		}
		if (strConfig)
		{
			delete strConfig;
			strConfig = nullptr;
		}
		return 0;
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{
		return 0;
	}
}