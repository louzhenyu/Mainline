#pragma once

//包类型
enum PACKET_TYPE
{
	UNHNOWN,	//未知包
	LOGIN,		//登录包
	SEND,		//发送包
	RECIVE,		//接收包
	HALF,		//半个数据包
	COMPLETE	//完整包
};

enum CMD_TYPE
{
	normal,			//普通命令
	print,			//打印命令
	xs,				//XS指令
	pnr,			//PNR
	login
};

class CEtermPacket
{
public:
	CEtermPacket(void);
	~CEtermPacket(void);
public:
	byte SID;						//配置SID
	byte RID;						//配置RID
	int  nPacketLength;				//包长度
	int	 nPatcketTotalLength;		//包总长度
	int  m_nFirstLegnth;			//起始长度
	int  m_unlessLength;			//未尾长度
	PACKET_TYPE		m_pt;			//包类型
	CMD_TYPE		m_cmdType;		//命令类型
	vector<CString> m_vecRev;		//接收数据集合	
	CString         m_cmd;			//发送的命令
	CString         m_strResponse;	//当前接收到的数据
	//验证数据包的有效性
	bool ValidatePakcet(byte* lpBuf,int nlen);
	//解析数据包，返回结果
	void ParseData(BYTE* pData,int nSize);
private:
	//数据包解析
	void UnPacket(CString& buffer);
	//汉字编码
	void UsasToGb(byte& c1,byte& c2);
	void PrintParse();
	void xsParse();
	void ParseHZ(BYTE* pData, char* szHZ);
	BYTE* m_btye;
};

