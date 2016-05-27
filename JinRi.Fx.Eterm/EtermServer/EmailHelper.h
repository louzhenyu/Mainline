#pragma once
#import "JetermUntility.tlb"
using namespace JetermUntility;

class EmailHelper
{
public:
	EmailHelper();
	~EmailHelper();

	static JetermUntility::ICommunicationPtr pComm;
	static CString FromEamil;
	static CString FromPwd;
	static CString FormServer;
	static vector<CString> Address;
	static void SendEmail(CString Subject, CString Body);
};

