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

#ifndef ASYNCSSLSOCKETLEAYER_INCLUDED
#define ASYNCSSLSOCKETLEAYER_INCLUDED

#include "AsyncSocketExLayer.h"
#include "openssl\ssl.h"

// Details of SSL certificate, can be used by app to verify if certificate is valid
struct t_SslCertData
{
	struct t_Contact
	{
		char Organization[256];
		char Unit[256];
		char CommonName[256];
		char Mail[256];
		char Country[256];
		char StateProvince[256];
		char Town[256];
		char Other[1024];
	} subject, issuer;

	struct t_validTime
	{
		//Year, Month, day, hour, minute, second
		int y,M,d,h,m,s;
	} validFrom, validUntil;

	unsigned char hash[20];

	int priv_data; //Internal data, do not modify
};

class CCriticalSectionWrapper;
class CAsyncSslSocketLayer : public CAsyncSocketExLayer
{
public:
	CAsyncSslSocketLayer();
	virtual ~CAsyncSslSocketLayer();

	void SetNotifyReply( int nID, int nCode, int result );
	BOOL GetPeerCertificateData(t_SslCertData &SslCertData);

	bool IsUsingSSL();
	bool InitClientSSL();

private:
	virtual void Close();
	virtual BOOL Connect(LPCTSTR lpszHostAddress, UINT nHostPort );
	virtual BOOL Connect(const SOCKADDR* lpSockAddr, int nSockAddrLen );
	virtual void OnReceive(int nErrorCode);
	virtual void OnSend(int nErrorCode);
	virtual int Receive(void* lpBuf, int nBufLen, int nFlags = 0);
	virtual int Send(const void* lpBuf, int nBufLen, int nFlags = 0);
	virtual BOOL ShutDown( int nHow = sends );

	void ResetSslSession();
	void PrintSessionInfo();
	BOOL ShutDownComplete();
	bool InitSSL();
	void UnloadSSL();

	//Will be called from the OpenSSL library
	static void apps_ssl_info_callback(SSL *s, int where, int ret);

	bool m_bUseSSL;
	BOOL m_bFailureSent;

	//Critical section for thread synchronization
	static CCriticalSectionWrapper m_sCriticalSection;

	// Status variables
	static int m_nSslRefCount;
	BOOL m_bSslInitialized;
	int m_nShutDown;
	BOOL m_nNetworkError;
	int m_nSslAsyncNotifyId;
	BOOL m_bBlocking;
	BOOL m_bSslEstablished;

	static struct t_SslLayerList
	{
		CAsyncSslSocketLayer *pLayer;
		t_SslLayerList *pNext;
	} *m_pSslLayerList;

	// Handles to the SLL libraries
	static HMODULE m_hSslDll1;
	static HMODULE m_hSslDll2;

	// SSL data
	static SSL_CTX* m_ssl_ctx;	// SSL context, valid for all threads
	SSL* m_ssl;					// current session handle

	//Data channels for encrypted/unencrypted data
	BIO* m_nbio;	//Network side, sends/received encrypted data
	BIO* m_ibio;	//Internal side, won't be used directly
	BIO* m_sslbio;	//The data to encrypt / the decrypted data has to go though this bio

	//Send buffer
	char* m_pNetworkSendBuffer;
	int m_nNetworkSendBufferLen;
};

#define SSL_INFO 0
#define SSL_FAILURE 1
#define SSL_VERIFY_CERT 2
#define SSL_VERBOSE_WARNING 3
#define SSL_VERBOSE_INFO 4

#define SSL_INFO_ESTABLISHED 0
#define SSL_INFO_SHUTDOWNCOMPLETE 1

#define SSL_FAILURE_UNKNOWN 0
#define SSL_FAILURE_ESTABLISH 1
#define SSL_FAILURE_LOADDLLS 2
#define SSL_FAILURE_INITSSL 4
#define SSL_FAILURE_VERIFYCERT 8

#endif // ASYNCSSLSOCKETLEAYER_INCLUDED
