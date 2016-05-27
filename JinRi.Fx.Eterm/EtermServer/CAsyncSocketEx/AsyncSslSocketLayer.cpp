/*CAsyncSslSocketLayer by Tim Kosse (Tim.Kosse@gmx.de)
               Version 1.0 (2003-04-05)
------------------------------------------------------

Introduction
------------

CAsyncSslSocketLayer is a layer class for CAsyncSocketEx which allows you to establish SSL secured
connections to servers.

How to use
----------

Using this class is really simple. In the easiest case, just add an instance of
CAsyncSslSocketLayer to your socket and call InitClientSsl after creation of the socket.

This class only has three new public functions:
- InitClientSsl();
  This functions establishes an SSL connection to the server. You can call it at any
  time one the socket has been created.
  Most likely you want to call this function right after calling Create for the socket.
  But sometimes, you'll need to call this function later. One example is for an FTP connection
  with explicit SSL: In this case you would have to call InitClientSsl after receiving the reply
  to an 'AUTH SSL' command.
- Is UsingSSL();
  Returns true if you've previously called InitClientSsl()
- SetNotifyReply(SetNotifyReply(int nID, int nCode, int result);
  You can call this function only after receiving a layerspecific callback with the SSL_VERIFY_CERT 
  id. See below for details.

This layer sends some layerspecific notifications to your socket instance, you can handle them in
OnLayerCallback of your socket class.
Valid notification IDs are:
- SSL_INFO 0
  There are two possible values for param2:
	SSL_INFO_ESTABLISHED 0 - You'll get this notification if the SSL negotiation was successful
	SSL_INFO_SHUTDOWNCOMPLETE 1 - You'll get this notification if the SSL connection has been shut 
                                  down sucessfully. See below for details.
- SSL_FAILURE 1
  This notification is sent if the SSL connection could not be established or if an existing 
  connection failed. Valid values for param2 are:
  - SSL_FAILURE_UNKNOWN 0 - Details may have been sent with a SSL_VERBOSE_* notification.
  - SSL_FAILURE_ESTABLISH 1 - Problem during SSL negotiation
  - SSL_FAILURE_LOADDLLS 2
  - SSL_FAILURE_INITSSL 4
  - SSL_FAILURE_VERIFYCERT 8 - The remote SSL certificate was invalid
- SSL_VERBOSE_WARNING 3
  SSL_VERBOSE_INFO 4
  This two notifications contain some additional information. The value given by param2 is a 
  pointer to a null-terminated char string (char *) with some useful information.
- SSL_VERIFY_CERT 2
  This notification is sent each time a remote certificate has to be verified.
  param2 is a pointer to a t_SslCertData structure which contains some information
  about the remote certificate.
  Return 1 if you trust the certificate and 0 if you don't trust it. If you're unsure so that the
  user has to choose to trust the certificate return 2.
  In this case, you have to call SetNotifyReply later to resume the SSL connection. nID has to be
  the priv_data element of the t_SslCertData structure and nCode has to be SSL_VERIFY_CERT.
  Set nAction to 1 if you trust the certificate and 0 if you don't trust it.

Be careful with closing the connection after sending data, not all data may have been sent already.
Before closing the connection, you should call Shutdown() and wait for the SSL_INFO_SHUTDOWNCOMPLETE
notification. This assures that all encrypted data really has been sent.

License
-------

Feel free to use this class, as long as you don't claim that you wrote it
and this copyright notice stays intact in the source files.
If you use this class in commercial applications, please send a short message
to tim.kosse@gmx.de

This product includes software developed by the OpenSSL Project
for use in the OpenSSL Toolkit. (http://www.openssl.org/)
*/

#include "stdafx.h"
#include "AsyncSslSocketLayer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//The following functions from the SSL libraries are used:
typedef int			(*tSSL_state)					(SSL *s);
typedef char*		(*tSSL_state_string_long)		(SSL *s);
typedef void		(*tSSL_set_info_callback)		(SSL *ssl, void(*cb)());
typedef void		(*tSSL_set_bio)					(SSL *s, BIO *rbio, BIO *wbio);
typedef void		(*tSSL_set_connect_state)		(SSL *s);
typedef int			(*tSSL_set_session)				(SSL *to, SSL_SESSION *session);
typedef BIO_METHOD*	(*tBIO_f_ssl)					(void);
typedef SSL*		(*tSSL_new)						(SSL_CTX *ctx);
typedef SSL_CTX*	(*tSSL_CTX_new)					(SSL_METHOD *meth);
typedef SSL_METHOD*	(*tSSLv23_method)				(void);
typedef SSL_METHOD*	(*tSSLv3_method)				(void);
typedef void		(*tSSL_load_error_strings)		(void);
typedef int			(*tSSL_library_init)			(void);
typedef void		(*tSSL_CTX_free)				(SSL_CTX *);
typedef void		(*tSSL_free)					(SSL *ssl);
typedef int			(*tSSL_get_error)				(SSL *s, int retcode);
typedef int			(*tSSL_shutdown)				(SSL *s);
typedef char*		(*tSSL_alert_type_string_long)	(int VALUE);
typedef char*		(*tSSL_alert_desc_string_long)	(int value);
typedef void		(*tSSL_CTX_set_verify)			(SSL_CTX *ctx, int mode, int (*callback)(int, X509_STORE_CTX *));
typedef X509_STORE*	(*tSSL_CTX_get_cert_store)		(SSL_CTX *);
typedef long		(*tSSL_get_verify_result)		(SSL *ssl);
typedef X509*		(*tSSL_get_peer_certificate)	(SSL *s);
typedef const char*	(*tSSL_get_version)				(SSL *ssl);
typedef SSL_CIPHER*	(*tSSL_get_current_cipher)		(SSL *ssl);
typedef const char*	(*tSSL_CIPHER_get_name)			(SSL_CIPHER *cipher);
typedef char*		(*tSSL_CIPHER_get_version)		(SSL_CIPHER *cipher);

typedef size_t				(*tBIO_ctrl_pending)				(BIO *b);
typedef int					(*tBIO_read)						(BIO *b, void *data, int len);
typedef long				(*tBIO_ctrl)						(BIO *bp, int cmd, long larg, void *parg);
typedef int					(*tBIO_write)						(BIO *b, const void *data, int len);
typedef size_t				(*tBIO_ctrl_get_write_guarantee)	(BIO *b);
typedef int					(*tBIO_new_bio_pair)				(BIO **bio1, size_t writebuf1, BIO **bio2, size_t writebuf2);
typedef BIO*				(*tBIO_new)							(BIO_METHOD *type);
typedef int					(*tBIO_free)						(BIO *a);
typedef int					(*ti2t_ASN1_OBJECT)					(char *buf, int buf_len, ASN1_OBJECT *a);
typedef int					(*tOBJ_obj2nid)						(ASN1_OBJECT *o);
typedef ASN1_OBJECT*		(*tX509_NAME_ENTRY_get_object)		(X509_NAME_ENTRY *ne);
typedef X509_NAME_ENTRY*	(*tX509_NAME_get_entry)				(X509_NAME *name, int loc);
typedef int					(*tX509_NAME_entry_count)			(X509_NAME *name);
typedef X509_NAME*			(*tX509_get_subject_name)			(X509 *a);
typedef X509_NAME*			(*tX509_get_issuer_name)			(X509 *a);
typedef const char*			(*tOBJ_nid2sn)						(int n);
typedef ASN1_STRING*		(*tX509_NAME_ENTRY_get_data)		(X509_NAME_ENTRY *ne);
typedef void				(*tX509_STORE_CTX_set_error)		(X509_STORE_CTX *ctx, int s);
typedef int					(*tX509_digest)						(const X509 *data, const EVP_MD *type, unsigned char *md, unsigned int *len);
typedef EVP_MD*				(*tEVP_sha1)						(void);
typedef X509*				(*tX509_STORE_CTX_get_current_cert)	(X509_STORE_CTX *ctx);
typedef int					(*tX509_STORE_CTX_get_error)		(X509_STORE_CTX *ctx);
typedef void				(*tX509_free)						(X509 *a);
typedef EVP_PKEY*			(*tX509_get_pubkey)					(X509 *x);
typedef int					(*tBN_num_bits)						(const BIGNUM *a);
typedef void				(*tEVP_PKEY_free)					(EVP_PKEY *pkey);
typedef int                 (*tSSL_set_cipher_list)             (SSL*, const char* str);

static tSSL_state_string_long		pSSL_state_string_long;
static tSSL_state					pSSL_state;
static tSSL_set_info_callback		pSSL_set_info_callback;
static tSSL_set_bio					pSSL_set_bio;
static tSSL_set_connect_state		pSSL_set_connect_state;
static tSSL_set_session				pSSL_set_session;
static tBIO_f_ssl					pBIO_f_ssl;
static tSSL_new						pSSL_new;
static tSSL_CTX_new					pSSL_CTX_new;
static tSSLv23_method				pSSLv23_method;
static tSSLv3_method                pSSLv3_method;
static tSSL_load_error_strings		pSSL_load_error_strings;
static tSSL_library_init			pSSL_library_init;
static tSSL_CTX_free				pSSL_CTX_free;
static tSSL_free					pSSL_free;
static tSSL_get_error				pSSL_get_error;
static tSSL_shutdown				pSSL_shutdown;
static tSSL_alert_type_string_long	pSSL_alert_type_string_long;
static tSSL_alert_desc_string_long	pSSL_alert_desc_string_long;
static tSSL_CTX_set_verify			pSSL_CTX_set_verify;
static tSSL_CTX_get_cert_store		pSSL_CTX_get_cert_store;
static tSSL_get_verify_result		pSSL_get_verify_result;
static tSSL_get_peer_certificate	pSSL_get_peer_certificate;
static tSSL_get_version				pSSL_get_version;
static tSSL_get_current_cipher		pSSL_get_current_cipher;
static tSSL_CIPHER_get_name			pSSL_CIPHER_get_name;
static tSSL_CIPHER_get_version		pSSL_CIPHER_get_version;
static tSSL_set_cipher_list         pSSL_set_cipher_list;

static tBIO_ctrl_pending				pBIO_ctrl_pending;
static tBIO_read						pBIO_read;
static tBIO_ctrl						pBIO_ctrl;
static tBIO_write						pBIO_write;
static tBIO_ctrl_get_write_guarantee	pBIO_ctrl_get_write_guarantee;
static tBIO_new_bio_pair				pBIO_new_bio_pair;
static tBIO_new							pBIO_new;
static tBIO_free						pBIO_free;
static ti2t_ASN1_OBJECT					pi2t_ASN1_OBJECT;
static tOBJ_obj2nid						pOBJ_obj2nid;
static tX509_NAME_ENTRY_get_object		pX509_NAME_ENTRY_get_object;
static tX509_NAME_get_entry				pX509_NAME_get_entry;
static tX509_NAME_entry_count			pX509_NAME_entry_count;
static tX509_get_subject_name			pX509_get_subject_name;
static tX509_get_issuer_name			pX509_get_issuer_name;
static tOBJ_nid2sn						pOBJ_nid2sn;
static tX509_NAME_ENTRY_get_data		pX509_NAME_ENTRY_get_data;
static tX509_STORE_CTX_set_error		pX509_STORE_CTX_set_error;
static tX509_digest						pX509_digest;
static tEVP_sha1						pEVP_sha1;
static tX509_STORE_CTX_get_current_cert	pX509_STORE_CTX_get_current_cert;
static tX509_STORE_CTX_get_error		pX509_STORE_CTX_get_error;
static tX509_free						pX509_free;
static tX509_get_pubkey					pX509_get_pubkey;
static tBN_num_bits						pBN_num_bits;
static tEVP_PKEY_free					pEVP_PKEY_free;

// Critical section wrapper class
#ifndef CCRITICALSECTIONWRAPPERINCLUDED
class CCriticalSectionWrapper
{
public:
	CCriticalSectionWrapper()
	{
		InitializeCriticalSection(&m_criticalSection);
	}

	~CCriticalSectionWrapper()
	{
		DeleteCriticalSection(&m_criticalSection);
	}

	void Lock()
	{
		EnterCriticalSection(&m_criticalSection);
	}
	void Unlock()
	{
		LeaveCriticalSection(&m_criticalSection);
	}
protected:
	CRITICAL_SECTION m_criticalSection;
};
#define CCRITICALSECTIONWRAPPERINCLUDED
#endif

/////////////////////////////////////////////////////////////////////////////
// CAsyncSslSocketLayer
CCriticalSectionWrapper CAsyncSslSocketLayer::m_sCriticalSection;

CAsyncSslSocketLayer::t_SslLayerList* CAsyncSslSocketLayer::m_pSslLayerList = 0;
int CAsyncSslSocketLayer::m_nSslRefCount = 0;
HMODULE CAsyncSslSocketLayer::m_hSslDll1 = 0;
HMODULE CAsyncSslSocketLayer::m_hSslDll2 = 0;
SSL_CTX* CAsyncSslSocketLayer::m_ssl_ctx = 0;

CAsyncSslSocketLayer::CAsyncSslSocketLayer()
{
	m_ssl = 0;
	m_sslbio = 0;
	m_ibio = 0;
	m_nbio = 0;

	m_bUseSSL = false;
	m_bSslInitialized = FALSE;
	m_bSslEstablished = FALSE;
	m_nNetworkSendBufferLen = 0;
	m_pNetworkSendBuffer = 0;
	m_nNetworkError = 0;
	m_nShutDown = 0;

	m_bBlocking = FALSE;
	m_nSslAsyncNotifyId = 0;
	m_bFailureSent = FALSE;
}

CAsyncSslSocketLayer::~CAsyncSslSocketLayer()
{
	UnloadSSL();
}

void CAsyncSslSocketLayer::OnReceive(int nErrorCode)
{
	if (m_bUseSSL)
	{
		if (m_bBlocking)
			return;
		if (m_nNetworkError)
			return;

		char buffer[16384];
		
		//Get number of bytes we can receive and store in the network input bio
		int len = pBIO_ctrl_get_write_guarantee(m_nbio);
		
		int numread = 0;
		if (len)
			// Receive data
			numread = ReceiveNext(buffer,len);
		while (numread>0)
		{
			//Store it in the network input bio and process data
			pBIO_write(m_nbio, buffer, numread);
			pBIO_ctrl(m_nbio, BIO_CTRL_FLUSH, 0, NULL);

			//Look if input data was valid
			int res = pBIO_read(m_sslbio, (void *)1, 0);
			if (res<0)
			{
				if (!BIO_should_retry(m_sslbio))
				{
					m_nNetworkError = WSAECONNABORTED;
					WSASetLastError(WSAECONNABORTED);
					if (!m_bFailureSent)
					{
						m_bFailureSent = TRUE;
						DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, m_bSslEstablished ? SSL_FAILURE_UNKNOWN : SSL_FAILURE_ESTABLISH);
					}
					TriggerEvent(FD_CLOSE, 0);
					return;
				}
			}
			if (numread<len)
				break;

			//Get number of bytes we can receive and store in the input network bio
			len = pBIO_ctrl_get_write_guarantee(m_nbio);
			numread = ReceiveNext(buffer, len);
		}
		if (numread==SOCKET_ERROR)
		{
			int nError = GetLastError();
			if (nError!=WSAEWOULDBLOCK && nError!=WSAENOTCONN)
			{
				m_nNetworkError = GetLastError();
				TriggerEvent(FD_CLOSE, 0);
				return;
			}
		}

		if (ShutDownComplete() && m_nShutDown == 1)
		{
			//Send shutdown notification if all pending data has been sent
			DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_INFO, SSL_INFO_SHUTDOWNCOMPLETE);
			m_nShutDown++;
		}

		if (pBIO_ctrl(m_sslbio, BIO_CTRL_PENDING, 0, NULL) && nErrorCode!=1)
			TriggerEvent(FD_READ, nErrorCode, TRUE);

		//Try to send encrypted data waiting in the network bio
		if (pBIO_ctrl_pending(m_nbio))
			OnSend(nErrorCode);
	}
	else
		TriggerEvent(FD_READ, nErrorCode, TRUE);
}

void CAsyncSslSocketLayer::OnSend(int nErrorCode)
{
	if (m_bUseSSL)
	{
		if (m_nNetworkError)
			return;

		//Send data in the send buffer
		if (m_pNetworkSendBuffer)
		{
			int numsent = SendNext(m_pNetworkSendBuffer, m_nNetworkSendBufferLen);
			if (numsent == SOCKET_ERROR)
			{
				int nError=GetLastError();
				if (nError!=WSAEWOULDBLOCK && nError!=WSAENOTCONN)
				{
					m_nNetworkError=nError;
					TriggerEvent(FD_CLOSE, 0);
				}
				return;
			}
			if (numsent==m_nNetworkSendBufferLen)
			{
				delete [] m_pNetworkSendBuffer;
				m_pNetworkSendBuffer = 0;
				m_nNetworkSendBufferLen = 0;
			}
			else
			{
				char *tmp = m_pNetworkSendBuffer;
				m_pNetworkSendBuffer = new char[m_nNetworkSendBufferLen-numsent];
				memcpy(m_pNetworkSendBuffer+numsent, tmp, m_nNetworkSendBufferLen-numsent);
				m_nNetworkSendBufferLen -= numsent;
				delete [] tmp;
				return;
			}
		}

		//Send the data waiting in the network bio
		char buffer[16384];
		int len = pBIO_ctrl_pending(m_nbio);
		int numread=pBIO_read(m_nbio, buffer, len);
		while(numread>0)
		{
			int numsent = SendNext(buffer, numread);
			if (numsent==SOCKET_ERROR || numsent<numread)
			{
				if (numsent==SOCKET_ERROR)
					if (GetLastError()!=WSAEWOULDBLOCK && GetLastError()!=WSAENOTCONN)
					{
						m_nNetworkError = GetLastError();
						TriggerEvent(FD_CLOSE, 0);
						return;
					}
					else
						numsent = 0;

				//Add all data that was retrieved from the network bio but could not be sent to the send buffer.
				if (!m_pNetworkSendBuffer)
					m_pNetworkSendBuffer = new char[numread-numsent];
				else
				{
					char *tmp = m_pNetworkSendBuffer;
					m_pNetworkSendBuffer = new char[m_nNetworkSendBufferLen+numread-numsent];
					memcpy(m_pNetworkSendBuffer, tmp, m_nNetworkSendBufferLen);
					delete [] tmp;
				}
				memcpy(m_pNetworkSendBuffer+m_nNetworkSendBufferLen, buffer, numread-numsent);
				m_nNetworkSendBufferLen += numread-numsent;
			}
			len = pBIO_ctrl_pending(m_nbio);
			if (!len)
				break;
			numread = pBIO_read(m_nbio,buffer,len);
		}
		if (ShutDownComplete() && m_nShutDown == 1)
		{
			DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_INFO, SSL_INFO_SHUTDOWNCOMPLETE);
			m_nShutDown++;
		}
	}
	else
		TriggerEvent(FD_WRITE, nErrorCode, TRUE);
}

int CAsyncSslSocketLayer::Send(const void* lpBuf, int nBufLen, int nFlags)
{
	if (m_bUseSSL)
	{
		if (!lpBuf)
			return 0;
		if (m_bBlocking)
		{
			SetLastError(WSAEWOULDBLOCK);
			return SOCKET_ERROR;
		}
		if (m_nNetworkError)
		{
			SetLastError(m_nNetworkError);
			return SOCKET_ERROR;
		}
		if (m_nShutDown)
		{
			SetLastError(WSAESHUTDOWN);
			return SOCKET_ERROR;
		}
		if (!nBufLen)
			return 0;

		int numwrite = pBIO_write(m_sslbio, lpBuf, nBufLen);
		pBIO_ctrl(m_sslbio, BIO_CTRL_FLUSH, 0, NULL);

		if (pBIO_ctrl_pending(m_nbio))
			CAsyncSslSocketLayer::OnSend(0);
		if (numwrite==-1)
			if (BIO_should_retry(m_sslbio))
			{
				TriggerEvent(FD_WRITE, 0);
				SetLastError(WSAEWOULDBLOCK);
			}
			else
				SetLastError(WSAECONNABORTED);

		return numwrite;
	}
	else
		return SendNext(lpBuf, nBufLen, nFlags);
}

int CAsyncSslSocketLayer::Receive(void* lpBuf, int nBufLen, int nFlags)
{
	if (m_bUseSSL)
	{
		if (m_bBlocking)
		{
			SetLastError(WSAEWOULDBLOCK);
			return SOCKET_ERROR;
		}
		if (m_nNetworkError)
		{
			if (pBIO_ctrl(m_sslbio, BIO_CTRL_PENDING, 0, NULL) && !m_nShutDown)
			{
				return pBIO_read(m_sslbio, lpBuf,nBufLen);
			}
			WSASetLastError(m_nNetworkError);
			return SOCKET_ERROR;
		}
		if (m_nShutDown)
		{
			SetLastError(WSAESHUTDOWN);
			return SOCKET_ERROR;
		}
		CAsyncSslSocketLayer::OnReceive(1);
		if (!nBufLen)
			return 0;
		if (!pBIO_ctrl(m_sslbio, BIO_CTRL_PENDING, 0, NULL))
		{
			SetLastError(WSAEWOULDBLOCK);
			return SOCKET_ERROR;
		}
		int numread=pBIO_read(m_sslbio, lpBuf, nBufLen);
		if (pBIO_ctrl(m_nbio, BIO_CTRL_PENDING, 0, NULL))
			CAsyncSslSocketLayer::OnSend(0);
		if (numread<0)
			if (!BIO_should_retry(m_sslbio))
			{
				m_nNetworkError=WSAECONNABORTED;
				WSASetLastError(WSAECONNABORTED);
				TriggerEvent(FD_CLOSE, 0);
				return SOCKET_ERROR;
			}
			else
			{
				SetLastError(WSAEWOULDBLOCK);
				return SOCKET_ERROR;
			}
			if (pBIO_ctrl(m_sslbio, BIO_CTRL_PENDING, 0, NULL))
			{
				TriggerEvent(FD_FORCEREAD, 0);
				TriggerEvent(FD_WRITE, 0);
			}
		return numread;
	}
	else
		return ReceiveNext(lpBuf, nBufLen, nFlags);
}

void CAsyncSslSocketLayer::Close()
{
	m_nShutDown = 0;
	ResetSslSession();
	CloseNext();
}

BOOL CAsyncSslSocketLayer::Connect(const SOCKADDR *lpSockAddr, int nSockAddrLen)
{
	BOOL res = ConnectNext(lpSockAddr, nSockAddrLen);
	if (!res)
		if (GetLastError() != WSAEWOULDBLOCK)
			ResetSslSession();
	return res;
}

BOOL CAsyncSslSocketLayer::Connect(LPCTSTR lpszHostAddress, UINT nHostPort)
{
	BOOL res = ConnectNext(lpszHostAddress, nHostPort);
	if (!res)
		if (GetLastError()!=WSAEWOULDBLOCK)
			ResetSslSession();
	return res;
}

bool CAsyncSslSocketLayer::InitClientSSL()
{
	if (m_bUseSSL)
		return true;
	if (!InitSSL())
		return false;

	//Create new SSL session
	if (!(m_ssl = pSSL_new( m_ssl_ctx )))
	{
		if (!m_bFailureSent)
		{
			m_bFailureSent=TRUE;
			DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_INITSSL);
		}
		ResetSslSession();
		return false;
	}

	//Add current instance to list of active instances
	m_sCriticalSection.Lock();
	t_SslLayerList *tmp = m_pSslLayerList;
	m_pSslLayerList = new t_SslLayerList;
	m_pSslLayerList->pNext = tmp;
	m_pSslLayerList->pLayer = this;
	m_sCriticalSection.Unlock();

	pSSL_set_info_callback(m_ssl, (void(*)())apps_ssl_info_callback);

	//Create bios
	m_sslbio = pBIO_new(pBIO_f_ssl());
	pBIO_new_bio_pair(&m_ibio, 40960, &m_nbio, 40960);

	if (!m_sslbio || !m_nbio || !m_ibio)
	{
		if (!m_bFailureSent)
		{
			m_bFailureSent = TRUE;
			DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_INITSSL);
		}
		ResetSslSession();
		return false;
	}

	//设置指定的加密算法
	pSSL_set_cipher_list(m_ssl, "DES-CBC-SHA");

	//Init SSL connection
	pSSL_set_session(m_ssl, NULL);
	pSSL_set_connect_state(m_ssl);
	pSSL_set_bio(m_ssl, m_ibio, m_ibio);
	pBIO_ctrl(m_sslbio, BIO_C_SET_SSL, BIO_NOCLOSE, m_ssl);
	pBIO_read(m_sslbio, (void *)1, 0);

	// Trigger FD_WRITE so that we can initialize SSL negotiation
	if (GetLayerState() == connected)
		TriggerEvent(FD_WRITE, 0);

	m_bUseSSL = true;

	return true;
}

void CAsyncSslSocketLayer::ResetSslSession()
{
	m_bFailureSent = FALSE;
	m_bBlocking = FALSE;
	m_nSslAsyncNotifyId++;
	m_nNetworkError = 0;

	m_bSslEstablished = FALSE;
	if (m_ssl)
		pSSL_set_session(m_ssl,NULL);
	if (m_nbio)
		pBIO_free(m_nbio);
	if (m_ibio)
		pBIO_free(m_ibio);
	if (m_sslbio)
		pBIO_free(m_sslbio);

	delete [] m_pNetworkSendBuffer;
	m_pNetworkSendBuffer = NULL;
	m_nNetworkSendBufferLen = 0;

	m_nbio = 0;
	m_ibio = 0;
	m_sslbio = 0;

	if (m_ssl)
		pSSL_free(m_ssl);
	m_sCriticalSection.Lock();

	m_ssl = 0;

	t_SslLayerList *cur = m_pSslLayerList;
	if (!cur)
	{
		m_sCriticalSection.Unlock();
		return;
	}

	if (cur->pLayer == this)
	{
		m_pSslLayerList = cur->pNext;
		delete cur;
	}
	else
		while (cur->pNext)
		{
			if (cur->pNext->pLayer == this)
			{
				t_SslLayerList *tmp = cur->pNext;
				cur->pNext = cur->pNext->pNext;
				delete tmp;

				m_sCriticalSection.Unlock();
				return;
			}
			cur = cur->pNext;
		}
	m_sCriticalSection.Unlock();
}

bool CAsyncSslSocketLayer::IsUsingSSL()
{
	return m_bUseSSL;
}

BOOL CAsyncSslSocketLayer::ShutDown(int nHow /*=sends*/)
{
	if (!m_nShutDown)
		m_nShutDown = 1;
	if (m_bUseSSL)
	{
		if (ShutDownComplete())
			return TRUE;
		if (pSSL_shutdown(m_ssl) != -1)
			return ShutDownNext();
		else
		{
			int error = pSSL_get_error(m_ssl,-1);
			if (error==SSL_ERROR_WANT_READ || error==SSL_ERROR_WANT_WRITE)
				return ShutDownNext();
			else
			{
				WSASetLastError(WSAEWOULDBLOCK);
				return FALSE;
			}
		}
	}
	else
		return ShutDownNext();
}

BOOL CAsyncSslSocketLayer::ShutDownComplete()
{
	//If a ShutDown was issued, has the connection already been shut down?
	if (!m_nShutDown)
		return FALSE;
	else if (!m_bUseSSL)
		return FALSE;
	else if (m_nNetworkSendBufferLen)
		return FALSE;
	else if (pBIO_ctrl_pending(m_nbio))
		return FALSE;
	else
		return TRUE;
}

void CAsyncSslSocketLayer::apps_ssl_info_callback(SSL *s, int where, int ret)
{
	CAsyncSslSocketLayer *pLayer = 0;
	m_sCriticalSection.Lock();
	t_SslLayerList *cur = m_pSslLayerList;
	while (cur)
	{
		if (cur->pLayer->m_ssl == s)
			break;
		cur = cur->pNext;
	}
	if (!cur)
	{
		m_sCriticalSection.Unlock();
		MessageBox(0, _T("Can't lookup SSL session!"), _T("Critical error"), MB_ICONEXCLAMATION);
		return;
	}
	else
		pLayer = cur->pLayer;
	m_sCriticalSection.Unlock();

	LPCTSTR str;
	int w;

	w=where& ~SSL_ST_MASK;

	if (w & SSL_ST_CONNECT) str=_T("SSL_connect");
	else if (w & SSL_ST_ACCEPT) str=_T("SSL_accept");
	else str=_T("undefined");

	if (where & SSL_CB_LOOP)
	{
		CHAR buffer[4096];
		sprintf(buffer, "%s:%s",
				str,
				pSSL_state_string_long(s));
		pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_VERBOSE_INFO, (int)buffer);
	}
	else if (where & SSL_CB_ALERT)
	{
		/*if (ret==SSL_AD_CLOSE_NOTIFY)
			if (!pLayer->m_nNetworkError)
				pLayer->m_nNetworkError=WSAESHUTDOWN;*/
		str=(where & SSL_CB_READ)?_T("read"):_T("write");
		CHAR buffer[4096];
		sprintf(buffer, "SSL3 alert %s:%s:%s",
				str,
				pSSL_alert_type_string_long(ret),
				pSSL_alert_desc_string_long(ret));
		pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_VERBOSE_WARNING, (int)buffer);
	}

	else if (where & SSL_CB_EXIT)
	{
		if (ret == 0)
		{
			CHAR buffer[4096];
			sprintf(buffer, "%s:failed in %s",
					str,
					pSSL_state_string_long(s));
			pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_VERBOSE_WARNING, (int)buffer);
			if (!pLayer->m_bFailureSent)
			{
				pLayer->m_bFailureSent=TRUE;
				pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, pLayer->m_bSslEstablished ? SSL_FAILURE_UNKNOWN : SSL_FAILURE_ESTABLISH);
			}
		}
		else if (ret < 0)
		{
			int error=pSSL_get_error(s,ret);
			if (error!=SSL_ERROR_WANT_READ && error!=SSL_ERROR_WANT_WRITE)
			{
				CHAR buffer[4096];
				sprintf(buffer, "%s: error in %s",
						str,
						pSSL_state_string_long(s));
				pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_VERBOSE_WARNING, (int)buffer);
				if (!pLayer->m_bFailureSent)
				{
					pLayer->m_bFailureSent=TRUE;
					pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, pLayer->m_bSslEstablished ? SSL_FAILURE_UNKNOWN : SSL_FAILURE_ESTABLISH);
				}
			}
		}
	}
	if (where&SSL_CB_HANDSHAKE_DONE)
	{
		int error = pSSL_get_verify_result(pLayer->m_ssl);
		if (error)
		{
			int res = pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_VERIFY_CERT, error);
			if (res==2)
			{
				pLayer->m_bBlocking=TRUE;
				return;
			}
			else if (!res)
			{
				pLayer->m_nNetworkError = WSAECONNABORTED;
				WSASetLastError(WSAECONNABORTED);
				if (!pLayer->m_bFailureSent)
				{
					pLayer->m_bFailureSent=TRUE;
					pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_VERIFYCERT);
				}
				return;
			}
		}
		pLayer->m_bSslEstablished = TRUE;
		pLayer->PrintSessionInfo();
		pLayer->DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_INFO, SSL_INFO_ESTABLISHED);
	}
}


void CAsyncSslSocketLayer::UnloadSSL()
{
	if (!m_bSslInitialized)
		return;
	ResetSslSession();

	m_bSslInitialized = false;

	m_sCriticalSection.Lock();
	m_nSslRefCount--;
	if (m_nSslRefCount)
	{
		m_sCriticalSection.Unlock();
		return;
	}
	if (m_ssl_ctx)
		pSSL_CTX_free(m_ssl_ctx);
	m_ssl_ctx = 0;

	if (m_hSslDll1)
		FreeLibrary(m_hSslDll1);
	if (m_hSslDll2)
		FreeLibrary(m_hSslDll2);
	m_hSslDll1 = NULL;
	m_hSslDll2 = NULL;

	m_sCriticalSection.Unlock();
}

BOOL CAsyncSslSocketLayer::GetPeerCertificateData(t_SslCertData &SslCertData)
{
	X509 *pX509=pSSL_get_peer_certificate(m_ssl);
	if (!pX509)
		return FALSE;
	
	//Reset the contents of SslCertData
	memset(&SslCertData, 0, sizeof(t_SslCertData));

	//Set subject data fields
	X509_NAME *pX509Name=pX509_get_subject_name(pX509);

	if (pX509Name)
	{
		int count=pX509_NAME_entry_count(pX509Name);
		for (int i=0;i<count;i++)
		{
			X509_NAME_ENTRY *pX509NameEntry=pX509_NAME_get_entry(pX509Name,i);
			if (!pX509NameEntry)
				continue;
			ASN1_STRING *pString=pX509_NAME_ENTRY_get_data(pX509NameEntry);
			ASN1_OBJECT *pObject=pX509_NAME_ENTRY_get_object(pX509NameEntry);
			const char *str = reinterpret_cast<const char *>(pString->data);
			switch(pOBJ_obj2nid(pObject))
			{
			case NID_organizationName:
				strncpy(SslCertData.subject.Organization, str, 255);
				SslCertData.subject.Organization[255] = 0;
				break;
			case NID_organizationalUnitName:
				strncpy(SslCertData.subject.Unit, str, 255);
				SslCertData.subject.Unit[255] = 0;
				break;
			case NID_commonName:
				strncpy(SslCertData.subject.CommonName, str, 255);
				SslCertData.subject.CommonName[255] = 0;
				break;
			case NID_pkcs9_emailAddress:
				strncpy(SslCertData.subject.Mail, str, 255);
				SslCertData.subject.Mail[255] = 0;
				break;
			case NID_countryName:
				strncpy(SslCertData.subject.Country, str, 255);
				SslCertData.subject.Country[255] = 0;
				break;
			case NID_stateOrProvinceName:
				strncpy(SslCertData.subject.StateProvince, str, 255);
				SslCertData.subject.StateProvince[255] = 0;
				break;
			case NID_localityName:
				strncpy(SslCertData.subject.Town, str, 255);
				SslCertData.subject.Town[255] = 0;
				break;
			default:
				if ( pOBJ_nid2sn(pOBJ_obj2nid(pObject)) )
				{
					CHAR tmp[20];
					sprintf(tmp, "%d", pOBJ_obj2nid(pObject));
					int maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), tmp, maxlen);

					maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), "=", maxlen);

					maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), str, maxlen);

					maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), ";", maxlen);
				}
				else
				{
					int maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), reinterpret_cast<const char *>(pOBJ_nid2sn(pOBJ_obj2nid(pObject))), maxlen);

					maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), "=", maxlen);

					maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), str, maxlen);

					maxlen = 1024 - strlen(SslCertData.subject.Other)-1;
					strncpy(SslCertData.subject.Other+strlen(SslCertData.subject.Other), ";", maxlen);
				}
				break;
			}
		}
	}

	//Set issuer data fields
	pX509Name=pX509_get_issuer_name(pX509);
	if (pX509Name)
	{
		int count=pX509_NAME_entry_count(pX509Name);
		for (int i=0;i<count;i++)
		{
			X509_NAME_ENTRY *pX509NameEntry=pX509_NAME_get_entry(pX509Name,i);
			if (!pX509NameEntry)
				continue;
			ASN1_STRING *pString=pX509_NAME_ENTRY_get_data(pX509NameEntry);
			ASN1_OBJECT *pObject=pX509_NAME_ENTRY_get_object(pX509NameEntry);
			const char *str = reinterpret_cast<const char *>(pString->data);
			switch(pOBJ_obj2nid(pObject))
			{
			case NID_organizationName:
				strncpy(SslCertData.issuer.Organization, str, 255);
				SslCertData.issuer.Organization[255] = 0;
				break;
			case NID_organizationalUnitName:
				strncpy(SslCertData.issuer.Unit, str, 255);
				SslCertData.issuer.Unit[255] = 0;
				break;
			case NID_commonName:
				strncpy(SslCertData.issuer.CommonName, str, 255);
				SslCertData.issuer.CommonName[255] = 0;
				break;
			case NID_pkcs9_emailAddress:
				strncpy(SslCertData.issuer.Mail, str, 255);
				SslCertData.issuer.Mail[255] = 0;
				break;
			case NID_countryName:
				strncpy(SslCertData.issuer.Country, str, 255);
				SslCertData.issuer.Country[255] = 0;
				break;
			case NID_stateOrProvinceName:
				strncpy(SslCertData.issuer.StateProvince, str, 255);
				SslCertData.issuer.StateProvince[255] = 0;
				break;
			case NID_localityName:
				strncpy(SslCertData.issuer.Town, str, 255);
				SslCertData.issuer.Town[255] = 0;
				break;
			default:
				if ( pOBJ_nid2sn(pOBJ_obj2nid(pObject)) )
				{
					CHAR tmp[20];
					sprintf(tmp, "%d", pOBJ_obj2nid(pObject));
					int maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), tmp, maxlen);

					maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), "=", maxlen);

					maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), str, maxlen);

					maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), ";", maxlen);
				}
				else
				{
					int maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), reinterpret_cast<const char *>(pOBJ_nid2sn(pOBJ_obj2nid(pObject))), maxlen);

					maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), "=", maxlen);

					maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), str, maxlen);

					maxlen = 1024 - strlen(SslCertData.issuer.Other)-1;
					strncpy(SslCertData.issuer.Other+strlen(SslCertData.issuer.Other), ";", maxlen);
				}
				break;
			}
		}
	}

	//Set date fields

	static const char *mon[12]=
    {
    "Jan","Feb","Mar","Apr","May","Jun",
    "Jul","Aug","Sep","Oct","Nov","Dec"
    };

	//Valid from
	ASN1_UTCTIME *pTime=X509_get_notBefore(pX509);
	if (!pTime)
	{
		pX509_free(pX509);
		return FALSE;
	}

	char *v;
	int gmt = 0;
	int i;
	int y=0, M=0, d=0, h=0, m=0, s=0;

	i = pTime->length;
	v = (char *)pTime->data;

	if (i < 10)
	{
		pX509_free(pX509);
		return FALSE;
	}
	if (v[i-1] == 'Z') gmt=1;
	for (i=0; i<10; i++)
		if ((v[i] > '9') || (v[i] < '0'))
		{
			pX509_free(pX509);
			return FALSE;
		}
	y= (v[0]-'0')*10+(v[1]-'0');
	if (y < 50) y+=100;
	M= (v[2]-'0')*10+(v[3]-'0');
	if ((M > 12) || (M < 1))
	{
		pX509_free(pX509);
		return FALSE;
	}
	d= (v[4]-'0')*10+(v[5]-'0');
	h= (v[6]-'0')*10+(v[7]-'0');
	m=  (v[8]-'0')*10+(v[9]-'0');
	if (	(v[10] >= '0') && (v[10] <= '9') &&
		(v[11] >= '0') && (v[11] <= '9'))
		s=  (v[10]-'0')*10+(v[11]-'0');

	SslCertData.validFrom.y = y+1900;
	SslCertData.validFrom.M = M;
	SslCertData.validFrom.d = d;
	SslCertData.validFrom.h = h;
	SslCertData.validFrom.m = m;
	SslCertData.validFrom.s = s;

	//Valid until
	pTime = X509_get_notAfter(pX509);
	if (!pTime)
	{
		pX509_free(pX509);
		return FALSE;
	}

	gmt = 0;
	i;
	y=0,M=0,d=0,h=0,m=0,s=0;

	i=pTime->length;
	v=(char *)pTime->data;

	if (i < 10)
	{
		pX509_free(pX509);
		return FALSE;
	}
	if (v[i-1] == 'Z') gmt=1;
	for (i=0; i<10; i++)
		if ((v[i] > '9') || (v[i] < '0'))
		{
			pX509_free(pX509);
			return FALSE;
		}
	y= (v[0]-'0')*10+(v[1]-'0');
	if (y < 50) y+=100;
	M= (v[2]-'0')*10+(v[3]-'0');
	if ((M > 12) || (M < 1))
	{
		pX509_free(pX509);
		return FALSE;
	}
	d= (v[4]-'0')*10+(v[5]-'0');
	h= (v[6]-'0')*10+(v[7]-'0');
	m=  (v[8]-'0')*10+(v[9]-'0');
	if (	(v[10] >= '0') && (v[10] <= '9') &&
		(v[11] >= '0') && (v[11] <= '9'))
		s=  (v[10]-'0')*10+(v[11]-'0');

	SslCertData.validUntil.y = y+1900;
	SslCertData.validUntil.M = M;
	SslCertData.validUntil.d = d;
	SslCertData.validUntil.h = h;
	SslCertData.validUntil.m = m;
	SslCertData.validUntil.s = s;

	unsigned int length = 20;
	pX509_digest(pX509, pEVP_sha1(), SslCertData.hash, &length);

	SslCertData.priv_data = m_nSslAsyncNotifyId;

	pX509_free(pX509);

	return TRUE;
}

void CAsyncSslSocketLayer::SetNotifyReply(int nID, int nCode, int result)
{
	if (!m_bBlocking)
		return;
	if (nID!=m_nSslAsyncNotifyId)
		return;
	if (nCode != SSL_VERIFY_CERT)
		return;

	m_bBlocking=FALSE;

	if (!result)
	{
		m_nNetworkError = WSAECONNABORTED;
		WSASetLastError(WSAECONNABORTED);
		if (!m_bFailureSent)
		{
			m_bFailureSent=TRUE;
			DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_VERIFYCERT);
		}
		TriggerEvent(FD_CLOSE, 0);
		return;
	}
	m_bSslEstablished=TRUE;
	PrintSessionInfo();
	DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_INFO, SSL_INFO_ESTABLISHED);

	TriggerEvent(FD_FORCEREAD, 0);
	TriggerEvent(FD_WRITE, 0);
}

bool CAsyncSslSocketLayer::InitSSL()
{
	if (m_bSslInitialized)
		return true;

	m_sCriticalSection.Lock();

	if (!m_nSslRefCount)
	{
		m_hSslDll1=LoadLibrary(_T("ssleay32.dll"));
		if (!m_hSslDll1)
		{
			m_sCriticalSection.Unlock();
			if (!m_bFailureSent)
			{
				m_bFailureSent=TRUE;
				DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_LOADDLLS);
			}
			return false;
		}
		pSSL_state_string_long		= (tSSL_state_string_long)		GetProcAddress(m_hSslDll1, "SSL_state_string_long");
		pSSL_state					= (tSSL_state)					GetProcAddress(m_hSslDll1, "SSL_state");
		pSSL_set_info_callback		= (tSSL_set_info_callback)		GetProcAddress(m_hSslDll1, "SSL_set_info_callback");
		pSSL_set_bio				= (tSSL_set_bio)				GetProcAddress(m_hSslDll1, "SSL_set_bio");
		pSSL_set_connect_state		= (tSSL_set_connect_state)		GetProcAddress(m_hSslDll1, "SSL_set_connect_state");
		pSSL_set_session			= (tSSL_set_session)			GetProcAddress(m_hSslDll1, "SSL_set_session");
		pBIO_f_ssl					= (tBIO_f_ssl)					GetProcAddress(m_hSslDll1, "BIO_f_ssl");
		pSSL_new					= (tSSL_new)					GetProcAddress(m_hSslDll1, "SSL_new");
		pSSL_CTX_new				= (tSSL_CTX_new)				GetProcAddress(m_hSslDll1, "SSL_CTX_new");
		pSSLv23_method				= (tSSLv23_method)				GetProcAddress(m_hSslDll1, "SSLv23_method");
		pSSLv3_method               = (tSSLv3_method)               GetProcAddress(m_hSslDll1, "SSLv3_method");
		pSSL_load_error_strings		= (tSSL_load_error_strings)		GetProcAddress(m_hSslDll1, "SSL_load_error_strings");
		pSSL_library_init			= (tSSL_library_init)			GetProcAddress(m_hSslDll1, "SSL_library_init");
		pSSL_CTX_free				= (tSSL_CTX_free)				GetProcAddress(m_hSslDll1, "SSL_CTX_free");
		pSSL_free					= (tSSL_free)					GetProcAddress(m_hSslDll1, "SSL_free");
		pSSL_get_error				= (tSSL_get_error)				GetProcAddress(m_hSslDll1, "SSL_get_error");
		pSSL_shutdown				= (tSSL_shutdown)				GetProcAddress(m_hSslDll1, "SSL_shutdown");
		pSSL_alert_type_string_long	= (tSSL_alert_type_string_long)	GetProcAddress(m_hSslDll1, "SSL_alert_type_string_long");
		pSSL_alert_desc_string_long	= (tSSL_alert_desc_string_long)	GetProcAddress(m_hSslDll1, "SSL_alert_desc_string_long");
		pSSL_CTX_set_verify			= (tSSL_CTX_set_verify)			GetProcAddress(m_hSslDll1, "SSL_CTX_set_verify");
		pSSL_CTX_get_cert_store		= (tSSL_CTX_get_cert_store)		GetProcAddress(m_hSslDll1, "SSL_CTX_get_cert_store");
		pSSL_get_verify_result		= (tSSL_get_verify_result)		GetProcAddress(m_hSslDll1, "SSL_get_verify_result");
		pSSL_get_peer_certificate	= (tSSL_get_peer_certificate)	GetProcAddress(m_hSslDll1, "SSL_get_peer_certificate");
		pSSL_get_version			= (tSSL_get_version)			GetProcAddress(m_hSslDll1, "SSL_get_version");
		pSSL_get_current_cipher		= (tSSL_get_current_cipher)		GetProcAddress(m_hSslDll1, "SSL_get_current_cipher");
		pSSL_CIPHER_get_name		= (tSSL_CIPHER_get_name)		GetProcAddress(m_hSslDll1, "SSL_CIPHER_get_name");
		pSSL_CIPHER_get_version		= (tSSL_CIPHER_get_version)		GetProcAddress(m_hSslDll1, "SSL_CIPHER_get_version");
		pSSL_set_cipher_list        = (tSSL_set_cipher_list)        GetProcAddress(m_hSslDll1, "SSL_set_cipher_list");

		if (!pSSL_state_string_long		||
			!pSSL_state					||
			!pSSL_set_info_callback		||
			!pSSL_set_bio				||
			!pSSL_set_connect_state		||
			!pSSL_set_session			||
			!pBIO_f_ssl					||
			!pSSL_new					||
			!pSSL_CTX_new				||
			!pSSLv23_method				||
			!pSSLv3_method              ||
			!pSSL_load_error_strings	||
			!pSSL_library_init			||
			!pSSL_CTX_free				||
			!pSSL_free					||
			!pSSL_get_error				||
			!pSSL_shutdown				||
			!pSSL_alert_type_string_long||
			!pSSL_alert_desc_string_long||
			!pSSL_CTX_set_verify		||
			!pSSL_CTX_get_cert_store	||
			!pSSL_get_verify_result		||
			!pSSL_get_peer_certificate	||
			!pSSL_get_version			||
			!pSSL_get_current_cipher	||
			!pSSL_CIPHER_get_name		||
			!pSSL_CIPHER_get_version	||
			!pSSL_set_cipher_list)

		{
			FreeLibrary(m_hSslDll1);
			m_hSslDll1=0;

			m_sCriticalSection.Unlock();
			if (!m_bFailureSent)
			{
				m_bFailureSent=TRUE;
				DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_LOADDLLS);
			}
			return FALSE;
		}

		m_hSslDll2=LoadLibrary(_T("libeay32.dll"));
		if (!m_hSslDll2)
		{
			FreeLibrary(m_hSslDll1);
			m_hSslDll1=0;

			m_sCriticalSection.Unlock();
			if (!m_bFailureSent)
			{
				m_bFailureSent=TRUE;
				DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_LOADDLLS);
			}
			return FALSE;
		}

		pBIO_ctrl_pending					= (tBIO_ctrl_pending)					GetProcAddress(m_hSslDll2, "BIO_ctrl_pending");
		pBIO_read							= (tBIO_read)							GetProcAddress(m_hSslDll2, "BIO_read");
		pBIO_ctrl							= (tBIO_ctrl)							GetProcAddress(m_hSslDll2, "BIO_ctrl");
		pBIO_write							= (tBIO_write)							GetProcAddress(m_hSslDll2, "BIO_write");
		pBIO_ctrl_get_write_guarantee		= (tBIO_ctrl_get_write_guarantee)		GetProcAddress(m_hSslDll2, "BIO_ctrl_get_write_guarantee");
		pBIO_new_bio_pair					= (tBIO_new_bio_pair)					GetProcAddress(m_hSslDll2, "BIO_new_bio_pair");
		pBIO_new							= (tBIO_new)							GetProcAddress(m_hSslDll2, "BIO_new");
		pBIO_free							= (tBIO_free)							GetProcAddress(m_hSslDll2, "BIO_free");
		pi2t_ASN1_OBJECT					= (ti2t_ASN1_OBJECT)					GetProcAddress(m_hSslDll2, "i2t_ASN1_OBJECT");
		pOBJ_obj2nid						= (tOBJ_obj2nid)						GetProcAddress(m_hSslDll2, "OBJ_obj2nid");
		pX509_NAME_ENTRY_get_object			= (tX509_NAME_ENTRY_get_object)			GetProcAddress(m_hSslDll2, "X509_NAME_ENTRY_get_object");
		pX509_NAME_get_entry				= (tX509_NAME_get_entry)				GetProcAddress(m_hSslDll2, "X509_NAME_get_entry");
		pX509_NAME_entry_count				= (tX509_NAME_entry_count)				GetProcAddress(m_hSslDll2, "X509_NAME_entry_count");
		pX509_get_subject_name				= (tX509_get_subject_name)				GetProcAddress(m_hSslDll2, "X509_get_subject_name");
		pX509_get_issuer_name				= (tX509_get_issuer_name)				GetProcAddress(m_hSslDll2, "X509_get_issuer_name");
		pOBJ_nid2sn							= (tOBJ_nid2sn)							GetProcAddress(m_hSslDll2, "OBJ_nid2sn");
		pX509_NAME_ENTRY_get_data			= (tX509_NAME_ENTRY_get_data)			GetProcAddress(m_hSslDll2, "X509_NAME_ENTRY_get_data");
		pX509_STORE_CTX_set_error			= (tX509_STORE_CTX_set_error)			GetProcAddress(m_hSslDll2, "X509_STORE_CTX_set_error");
		pX509_digest						= (tX509_digest)						GetProcAddress(m_hSslDll2, "X509_digest");
		pEVP_sha1							= (tEVP_sha1)							GetProcAddress(m_hSslDll2, "EVP_sha1");
		pX509_STORE_CTX_get_current_cert	= (tX509_STORE_CTX_get_current_cert)	GetProcAddress(m_hSslDll2, "X509_STORE_CTX_get_current_cert");
		pX509_STORE_CTX_get_error			= (tX509_STORE_CTX_get_error)			GetProcAddress(m_hSslDll2, "X509_STORE_CTX_get_error");
		pX509_free							= (tX509_free)							GetProcAddress(m_hSslDll2, "X509_free");
		pX509_get_pubkey					= (tX509_get_pubkey)					GetProcAddress(m_hSslDll2, "X509_get_pubkey");
		pBN_num_bits						= (tBN_num_bits)						GetProcAddress(m_hSslDll2, "BN_num_bits");
		pEVP_PKEY_free						= (tEVP_PKEY_free)						GetProcAddress(m_hSslDll2, "EVP_PKEY_free");

		if (!pBIO_ctrl_pending					||
			!pBIO_read							||
			!pBIO_ctrl							||
			!pBIO_write							||
			!pBIO_ctrl_get_write_guarantee		||
			!pBIO_new_bio_pair					||
			!pBIO_new							||
			!pBIO_free							||
			!pi2t_ASN1_OBJECT					||
			!pOBJ_obj2nid						||
			!pX509_NAME_ENTRY_get_object		||
			!pX509_NAME_get_entry				||
			!pX509_NAME_entry_count				||
			!pX509_get_subject_name				||
			!pX509_get_issuer_name				||
			!pOBJ_nid2sn						||
			!pX509_NAME_ENTRY_get_data			||
			!pX509_STORE_CTX_set_error			||
			!pX509_digest						||
			!pEVP_sha1							||
			!pX509_STORE_CTX_get_current_cert	||
			!pX509_STORE_CTX_get_error			||
			!pX509_free							||
			!pX509_get_pubkey					||
			!pBN_num_bits						||
			!pEVP_PKEY_free)
		{
			FreeLibrary(m_hSslDll1);
			m_hSslDll1=0;
			FreeLibrary(m_hSslDll2);
			m_hSslDll2=0;

			m_sCriticalSection.Unlock();
			if (!m_bFailureSent)
			{
				m_bFailureSent=TRUE;
				DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_LOADDLLS);
			}
			return FALSE;
		}

		if (!pSSL_library_init())
		{
			FreeLibrary(m_hSslDll1);
			m_hSslDll1=0;
			FreeLibrary(m_hSslDll2);
			m_hSslDll2=0;

			m_sCriticalSection.Unlock();
			if (!m_bFailureSent)
			{
				m_bFailureSent=TRUE;
				DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_INITSSL);
			}
			return FALSE;
		}
		pSSL_load_error_strings();
		if (!(m_ssl_ctx = pSSL_CTX_new(pSSLv23_method())))
		{
			FreeLibrary(m_hSslDll1);
			m_hSslDll1=0;
			FreeLibrary(m_hSslDll2);
			m_hSslDll2=0;

			m_sCriticalSection.Unlock();
			if (!m_bFailureSent)
			{
				m_bFailureSent=TRUE;
				DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_FAILURE, SSL_FAILURE_INITSSL);
			}
			return FALSE;
		}
	}

	m_nSslRefCount++;
	m_sCriticalSection.Unlock();

	m_bSslInitialized = true;

	return true;
}

void CAsyncSslSocketLayer::PrintSessionInfo()
{
	SSL_CIPHER *ciph;
	X509 *cert;

	ciph = pSSL_get_current_cipher(m_ssl);
	CHAR enc[4096] = {0};
	cert=pSSL_get_peer_certificate(m_ssl);

	if (cert != NULL)
	{
		EVP_PKEY *pkey = pX509_get_pubkey(cert);
		if (pkey != NULL)
		{
			if (0)
				;
#ifndef NO_RSA
			else if (pkey->type == EVP_PKEY_RSA && pkey->pkey.rsa != NULL
				&& pkey->pkey.rsa->n != NULL)
				sprintf(enc,	"%d bit RSA", pBN_num_bits(pkey->pkey.rsa->n));
#endif
#ifndef NO_DSA
			else if (pkey->type == EVP_PKEY_DSA && pkey->pkey.dsa != NULL
					&& pkey->pkey.dsa->p != NULL)
				sprintf(enc,	"%d bit DSA", pBN_num_bits(pkey->pkey.dsa->p));
#endif
			pEVP_PKEY_free(pkey);
		}
		pX509_free(cert);
		/* The SSL API does not allow us to look at temporary RSA/DH keys,
		 * otherwise we should print their lengths too */
	}

	CHAR buffer[4096];
	sprintf(buffer, "Using %s, cipher %s: %s, %s",
			pSSL_get_version(m_ssl),
			pSSL_CIPHER_get_version(ciph),
			pSSL_CIPHER_get_name(ciph),
			enc);
	DoLayerCallback(LAYERCALLBACK_LAYERSPECIFIC, SSL_VERBOSE_INFO, (int)buffer);
}
