#pragma once
#ifndef _BASE_DEFINE_H_
#define _BASE_DEFINE_H_

#include <winsock2.h>
#include <MSWSock.h>
#pragma comment(lib,"ws2_32")   // Standard socket API.
#pragma comment(lib,"mswsock")  // AcceptEx, TransmitFile, etc,.
#pragma comment(lib,"shlwapi")  // UrlUnescape.

using namespace std;
#include <iostream>

#include <MSTcpIP.h> //此文件包括下面的define定义及struct定义,用于socket实现heartbeat检测.但此文件在win2000上可能没有，所以自己定义


#define  SIO_KEEPALIVE_VALS  IOC_IN | IOC_VENDOR | 4

//自定义的结构体,用于TCP服务器
typedef struct tcp_keepalive_v
{
	unsigned long onoff;
	unsigned long keepalivetime;
	unsigned long keepaliveinterval;
}TCP_KEEPALIVE_V,*PTCP_KEEPALIVE_V;


//#define NULL 0
#define SA struct sockaddr_in

const int MAX_PROCESSOR_COUNTER = 10;//最多定义10个处理器

enum SERVER_COMPLETE_KEY
{
    COMPLETION_KEY_IO = 0,
	COMPLETION_KEY_SHUTDOWN = 1,
};

enum SOCKET_CONTEXT_STATE
{
	SC_WAIT_ACCEPT   =   0,
	SC_WAIT_RECEIVE  =   1,
	SC_WAIT_TRANSMIT =   2,
	SC_WAIT_RESET    =   3,
};












#endif