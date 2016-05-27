#include "stdafx.h"
#include "EmailHelper.h"
#include "Global.h"

JetermUntility::ICommunicationPtr EmailHelper::pComm = NULL;
CString EmailHelper::FromEamil;
CString EmailHelper::FromPwd;
CString EmailHelper::FormServer;
vector<CString> EmailHelper::Address;

EmailHelper::EmailHelper()
{
}


EmailHelper::~EmailHelper()
{
}

void EmailHelper::SendEmail(CString Subject, CString Body)
{
	__try
	{
		if (EmailHelper::pComm)
		{
			SAFEARRAY *pArr = SafeArrayCreateVector(VT_BSTR, 0, 1);
			if (pArr != NULL)
			{
				LONG index = 0;
				for (int i = 0; i < EmailHelper::Address.size(); i++, index++)
				{
					BSTR bs = SysAllocString(EmailHelper::Address[i]);
					SafeArrayPutElement(pArr, &index, bs);
					SysFreeString(bs);
				}
				EmailHelper::pComm->SendMail((_bstr_t)EmailHelper::FromEamil, (_bstr_t)EmailHelper::FromPwd, (_bstr_t)EmailHelper::FormServer,(_bstr_t)Subject, (_bstr_t)Body, pArr);

				SafeArrayDestroy(pArr);
			}
		}	
	}
	__except (Global::MyUnhandledExceptionFilter(GetExceptionInformation()))
	{

	}
}