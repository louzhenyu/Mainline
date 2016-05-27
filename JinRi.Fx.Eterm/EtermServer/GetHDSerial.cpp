#include "StdAfx.h"
#include "GetHDSerial.h"

#pragma warning(disable: 4996)

//定义全局变量
char m_buffer[256];
WORD m_serial[256];
DWORD m_OldInterruptAddress;
DWORDLONG m_IDTR;

#define IsChar(c) (c>=48&&c<=57)||(c>=65&&c<=90) ||(c>=97&&c<=122)

/*	功能：等待硬盘空闲
返回值：Static unsigned int
*/
static unsigned int WaitHardDiskIdle()
{
	BYTE byTemp;
Waiting:
	_asm
	{
		mov dx, 0x1f7
		in al, dx
		cmp al, 0x80
		jb EndWaiting
		jmp Waiting
	}
EndWaiting:
	_asm
	{
		mov byTemp, al
	}
	return byTemp;
}

/*
功能：中断进程
返回值：void
*/
void _declspec(naked)InterruptProcess(void)
{
	int byTemp;
	int i;
	WORD temp;
	//保存寄存器值
	_asm
	{
		push eax
		push ebx
		push ecx
		push edx
		push esi
	}

	WaitHardDiskIdle();	//等待硬盘空闲状态
	__asm
	{
		mov dx, 0x1f6
		mov al, 0xa0
		out dx, al
	}
	byTemp = WaitHardDiskIdle();	//若直接在Ring3级执行等待命令，会进入死循环
	if ((byTemp & 0x50) != 0x50)
	{
		_asm	//恢复中断现场并退出 中断服务程序
		{
			pop esi
			pop edx
			pop edx
			pop ebx
			pop eax
			iretd
		}
	}

	_asm
	{
		mov dx, 0x1f6	//命令商品1f6,选择驱动器0
		{
			pop esi
			pop edx
			pop ecx
			pop ebx
			pop eax
			iretd
		}
	}
	//读取硬盘插口的全部信息
	for (i = 0; i<256; i++)
	{
		_asm
		{
			mov dx, 0x1f0
			in ax, dx
			mov temp, ax
		}
		m_serial[i] = temp;
	}
	_asm
	{
		pop esi
		pop edx
		pop ecx
		pop ebx
		pop eax
		iretd
	}
}

CGetHDSerial::CGetHDSerial(void)
{
}

CGetHDSerial::~CGetHDSerial(void)
{
}

/*
功能：Window0x/ME系列下读取硬盘序列号
*/
void _stdcall CGetHDSerial::Win9xReadHDSerial(WORD* buffer)
{
	int i;
	for (i = 0; i<256; i++)
		buffer[i] = 0;
	_asm
	{
		push eax
		//获取修改中断的中断描述符（中断门）地址
		sidt m_IDTR
		mov eax, dword ptr[m_IDTR + 02h]
		add eax, 3 * 08h + 04h
		cli
		//保存原先的中断入口地址
		push ecx
		mov ecx, dword ptr[eax]
		mov cx, word ptr[eax - 04h]
		mov dword ptr m_OldInterruptAddress, ecx
		pop ecx
		//设置修改的中断入口地址为新的中断处理程序入口地址
		push ebx
		lea ebx, InterruptProcess
		mov word ptr[eax + 04h], bx
		shr ebx, 10h
		mov word ptr[eax + 02h], cx
		pop ecx
		//执行中断，转到Ring 0
		int 3h
		//恢复原先的中断入口地址
		push ecx
		mov ecx, dword ptr m_OldInterruptAddress
		mov word ptr[eax - 04h], cx
		shr ecx, 10h
		mov word ptr[eax + 02h], cx
		pop ecx
		sti
		pop eax
	}
	for (i = 0; i<256; i++)
		buffer[i] = m_serial[i];
}

/*
功能：发送和获取服务端计算机硬盘序列号
返回值：调用成功返回TRUE,否则返回FALSE
*/
char* CGetHDSerial::GetHDSerial()
{
	m_buffer[0] = '\n';
	//得到当前操作系统版本
	OSVERSIONINFO OSVersionInfo;
	OSVersionInfo.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&OSVersionInfo);
	if (OSVersionInfo.dwPlatformId != VER_PLATFORM_WIN32_NT)
	{
		//Window9x/Me下读取硬盘序列号
		WORD m_wWin9xHDSerial[256];
		Win9xReadHDSerial(m_wWin9xHDSerial);
		strcpy(m_buffer, WORDToChar(m_wWin9xHDSerial, 10, 19));
	}
	else
	{
		//Windows NT/2000/XP 下读取硬盘序列号
		DWORD m_wWinNTHDSerial[256];
		//判断是否有SCSI硬盘
		if (!WinNTReadIDEHDSerial(m_wWinNTHDSerial))
			WinNTReadSCSIHDSerial(m_wWinNTHDSerial);
		strcpy(m_buffer, DWORDToChar(m_wWinNTHDSerial, 10, 19));
	}

	char szBuf[255] = { 0 };
	int ii = 0;
	for (int i = 0; i < strlen(m_buffer);i++)
	if (IsChar(m_buffer[i]))
		szBuf[ii++] = m_buffer[i];
	szBuf[ii++] = 0;
	if (strlen(szBuf) <5)
	{
		DWORD dw = GetCPUID();
		sprintf(szBuf, "%d", dw);
	}
	strcpy(m_buffer, szBuf);
	return m_buffer;
}

/*
功能：Windows 9x/ME系统下，将字类型(WORD)的硬盘信息转换为字符类型(char)
*/
char* CGetHDSerial::WORDToChar(WORD diskdata[256], int firstIndex, int lastIndex)
{
	static char string[1024];
	int index = 0;
	int position = 0;
	//按照高字节在前，低字节在后的顺序将字符将字符数组diskdata中内容存入到字符串string中
	for (index = firstIndex; index <= lastIndex; index++)
	{
		//存入字中的高字节
		string[position] = (char)(diskdata[index] / 256);
		position++;
		//存入字中的低字节
		string[position] = (char)(diskdata[index] % 256);
		position++;
	}
	//添加字符串结束标志
	string[position] = '\0';
	//删除字符串中空格
	for (index = position - 1; index>0 && ' ' == string[index]; index--)
		string[index] = '\0';

	return string;
}

/*
功能：Window NT/2000/XP系统下，将双字类型（DWORD)的硬盘信息转换为字符类型(char)
*/
char* CGetHDSerial::DWORDToChar(DWORD diskdata[256], int firstIndex, int lastIndex)
{
	static char string[1024];
	int index = 0;
	int position = 0;

	//按照高字节在前，低字节在后的顺序将双字节中的低字存入到字符串string中
	for (index = firstIndex; index <= lastIndex; index++)
	{
		//存入低字中的高字节
		string[position] = (char)(diskdata[index] / 256);
		position++;
		//存入低字中的低字节
		string[position] = (char)(diskdata[index] % 256);
		position++;
	}

	//添加字符串结束标志
	string[position] = '\0';

	//删除字符串中空格
	for (index = position - 1; index>0 && ' ' == string[index]; index--)
		string[index] = '\0';
	return string;
}

/*
功能：Windows NT/2000/xp系统下读取SCSI硬盘序列号
*/
BOOL CGetHDSerial::WinNTReadSCSIHDSerial(DWORD* buffer)
{
	buffer[0] = '\n';
	int controller = 0;
	HANDLE hScsiDriveIOCTL = 0;
	char driveName[256];
	sprintf(driveName, "\\\\.\\Scsi%d:", controller);
	//Windows NT/2000/xp 下任何权限都可以进行
	hScsiDriveIOCTL = CreateFileA(driveName,
		GENERIC_READ | GENERIC_WRITE,
		FILE_SHARE_READ | FILE_SHARE_WRITE, NULL,
		OPEN_EXISTING, 0, NULL);
	if (hScsiDriveIOCTL != INVALID_HANDLE_VALUE)
	{
		int drive = 0;
		DWORD dummy;
		for (drive = 0; drive<2; drive++)
		{
			char buffer[sizeof(SRB_IO_CONTROL)+SENDIDLENGTH];
			SRB_IO_CONTROL *p = (SRB_IO_CONTROL*)buffer;
			SENDCMDINPARAMS* pin = (SENDCMDINPARAMS*)(buffer + sizeof(SRB_IO_CONTROL));
			//准备参数
			memset(buffer, 0, sizeof(buffer));
			p->HeaderLength = sizeof(SRB_IO_CONTROL);
			p->Timeout = 10000;
			p->Length = SENDIDLENGTH;
			p->ControlCode = IOCTL_SCSI_MINIPORT_IDENTIFY;
			strncpy((char*)p->Signauture, "SCSIDISK", 8);
			pin->irDriveRegs.bCommandReg = IDE_ATA_IDENTIFY;
			pin->bDriveNumber = drive;
			//得到SCSI硬盘信息
			if (DeviceIoControl(hScsiDriveIOCTL, IOCTL_SCSI_MINIPORT,
				buffer,
				sizeof(SRB_IO_CONTROL)+sizeof(SENDCMDINPARAMS)-1,
				buffer,
				sizeof(SRB_IO_CONTROL)+SENDIDLENGTH,
				&dummy, NULL))
			{
				SENDCMDOUTPARAMS* pOut = (SENDCMDOUTPARAMS*)(buffer + sizeof(SRB_IO_CONTROL));
				IDSECTOR* pId = (IDSECTOR*)(pOut->bBuffer);
				if (pId->sModelNumber[0])
				{
					int n = 0;
					USHORT* pIdSector = (USHORT*)pId;
					for (n = 0; n<256; n++)
						buffer[n] = pIdSector[n];
					return TRUE;	//读取成功
				}
			}

		}
		CloseHandle(hScsiDriveIOCTL);	//关闭句柄
	}
	return FALSE;	//读取失败
}

/*
功能：Window NT/2000/xp下读取IDE硬盘序列号
*/
BOOL CGetHDSerial::WinNTReadIDEHDSerial(DWORD* buffer)
{
	BYTE IdOutCmd[sizeof(SENDCMDOUTPARAMS)+IDENTTIFY_BUFFER_SIZE - 1];
	BOOL bFlag = FALSE;
	int drive = 0;
	char driveName[256];
	HANDLE hPhysicalDriveIOCTL = 0;

	sprintf(driveName, "\\\\.\\physicalDrive%d", drive);
	//Windows NT/200/xp 下创建文件需要管理员权限
	hPhysicalDriveIOCTL = CreateFileA(driveName,
		GENERIC_READ | GENERIC_WRITE,
		FILE_SHARE_READ | FILE_SHARE_WRITE, NULL,
		OPEN_EXISTING, 0, NULL);
	if (hPhysicalDriveIOCTL != INVALID_HANDLE_VALUE)
	{
		GETVERSIONOUTPARAMS VersionParams;
		DWORD cbBytesReturned = 0;
		//得到驱动器的IO控制器版本
		memset((void*)&VersionParams, 0, sizeof(VersionParams));
		if (DeviceIoControl(hPhysicalDriveIOCTL, IOCTL_GET_VERSION,
			NULL, 0, &VersionParams,
			sizeof(VersionParams),
			&cbBytesReturned, NULL))
		{
			if (VersionParams.bIDEDeviceMap >= 0)
			{
				BYTE bIDCmd = 0;	//IDE或者ATAPI 识别命令
				SENDCMDINPARAMS scip;

				//如果驱动器是光驱，采用命令IDE_ATAIP_IDENTIFY,
				//否则采用命令IDE_ATA_IDENTIFY读取驱动器信息
				bIDCmd = (VersionParams.bIDEDeviceMap >> drive & 0x10) ? IDE_ATAPI_IDENTIFY : IDE_ATA_IDENTIFY;

				memset(&scip, 0, sizeof(scip));
				memset(IdOutCmd, 0, sizeof(IdOutCmd));
				//获取驱动器信息
				if (WinNTGetIDEHDInfo(hPhysicalDriveIOCTL,
					&scip,
					(PSENDCMDOUTPARAMS)&IdOutCmd,
					(BYTE)bIDCmd,
					(BYTE)drive,
					&cbBytesReturned))
				{
					int m = 0;
					USHORT* PIdSector = (USHORT*)((PSENDCMDOUTPARAMS)IdOutCmd)->bBuffer;
					for (m = 0; m<256; m++)
						buffer[m] = PIdSector[m];
					bFlag = TRUE;	//读取硬盘信息成功
				}
			}
		}
		CloseHandle(hPhysicalDriveIOCTL);	//关闭句柄
	}
	return bFlag;
}

/*
功能：Windows NT/2000/XP下读取IDE设备信息
*/
BOOL CGetHDSerial::WinNTGetIDEHDInfo(HANDLE hPhysicalDriveIOCTL,
	PSENDCMDINPARAMS pSCIP,
	PSENDCMDOUTPARAMS pSCOP,
	BYTE bIDCmd,
	BYTE bDriveNum,
	PDWORD lpcbBytesReturned)
{
	//为读取设备信息准备参数
	pSCIP->cBufferSize = IDENTTIFY_BUFFER_SIZE;
	pSCIP->irDriveRegs.bFeaturesReg = 0;
	pSCIP->irDriveRegs.bSectorCountReg = 1;
	pSCIP->irDriveRegs.bSectorNumberReg = 1;
	pSCIP->irDriveRegs.bCylLowReg = 0;
	pSCIP->irDriveRegs.bCylHighReg = 0;

	//计算驱动器位置
	pSCIP->irDriveRegs.bDriveHeadReg = 0xA0 | ((bDriveNum & 1) << 4);

	//设备读取命令
	pSCIP->irDriveRegs.bCommandReg = bIDCmd;
	pSCIP->bDriveNumber = bDriveNum;
	pSCIP->cBufferSize = IDENTTIFY_BUFFER_SIZE;

	//读取驱动器信息
	return (DeviceIoControl(hPhysicalDriveIOCTL, IOCTL_GET_DRIVE_INFO,
		(LPVOID)pSCIP,
		sizeof(SENDCMDINPARAMS)-1,
		(LPVOID)pSCOP,
		sizeof(SENDCMDOUTPARAMS)+IDENTTIFY_BUFFER_SIZE - 1,
		lpcbBytesReturned, NULL));
}

DWORD CGetHDSerial::GetCPUID()
{
	DWORD   dwId1, dwId2, dwId3, dwId4;
	char   szCompany[13];
	PCHAR   pCompany = szCompany;

	//DWORD dwCPU;  

	szCompany[12] = 0;
	_asm
	{
		pushfd
			pushad
			//取得CPU的ID号     
			mov   eax, 1   //功能号     
			_emit   0x0f
			_emit   0xa2
			mov   dwId1, eax
			mov   dwId2, ebx
			mov   dwId3, ecx
			mov   dwId4, edx

			//取得CPU的制造公司名称  
			mov   edi, pCompany   //功能号     
			mov   eax, 0
			_emit   0x0f
			_emit   0xa2
			mov   eax, ebx
			stosd
			mov   eax, edx
			stosd
			mov   eax, ecx
			stosd
			popad
			popfd
	}

	DWORD dwResult = 0;
	DWORD dwTemp1 = dwId1 << 12;
	DWORD dwTemp2 = dwId2 << 8;
	DWORD dwTemp3 = dwId3 << 4;

	dwResult = dwTemp1 + dwTemp2 + dwTemp3 + dwId4;

	return dwResult;
}