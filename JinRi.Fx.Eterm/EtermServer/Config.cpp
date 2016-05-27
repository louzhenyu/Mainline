#include "stdafx.h"
#include "Config.h"


Config::Config()
{
	IsSSL=false;
	AutoSI=false;
	KeepAlive = true;
	Port=350;
	Interval=0;
	MaxCount=130000;
	Count=0;	
	Type = _T("eEterm");
}


Config::~Config()
{
}
