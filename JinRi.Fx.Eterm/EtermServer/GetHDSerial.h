#pragma once


//宏定义
#define SENDIDLENGTH sizeof(SENDCMDOUTPARAMS) + IDENTTIFY_BUFFER_SIZE
#define IDENTTIFY_BUFFER_SIZE 512
#define FILE_DEVICE_SCSI	0X00140106b
#define IOCTL_SCSI_MINIPORT_IDENTIFY ((FILE_DEVICE_SCSI<<16)+0x0501)
#define IOCTL_SCSI_MINIPORT 0x0004d008
#define IDE_ATAPI_IDENTIFY 0xA1
#define IDE_ATA_IDENTIFY 0xEC
#define IOCTL_GET_DRIVE_INFO 0x0007c088
#define IOCTL_GET_VERSION 0x00074080

//定义IDSECTOR结构
typedef struct _IDSECTOR
{
	USHORT wGenConfig;
	USHORT wNumCyls;
	USHORT wReserved;
	USHORT wNumHeads;
	USHORT wBytesPerTrack;
	USHORT wBytesPerSector;
	USHORT wSectorsPerTrack;
	USHORT wVendorUnique[3];
	CHAR   sSerialNumber[20];
	USHORT wBufferType;
	USHORT wBufferSize;
	USHORT wECCSize;
	CHAR   sFirmwareRev[8];
	CHAR   sModelNumber[40];
	USHORT wMoreVeWndorUnique;
	USHORT wDoubleWordIO;
	USHORT wCapabilities;
	USHORT wReserved1;
	USHORT wPIOTiming;
	USHORT wDMATiming;
	USHORT wBS;
	USHORT wNumCurrentCyls;	
	USHORT wNumCurrentHeads;
	USHORT wNumCurrentSectorsPerTrack;
	ULONG  ulCurrentSectorCapacity;
	USHORT wMultSectorStuff;
	ULONG  ulTotalAddressableSectors;
	USHORT wSingleWordDMA;
	USHORT wMultiWordDMA;
	BYTE   bReserved[128];
}IDSECTOR,*PIDSECTOR;

//定义DRIVERSTATUS结构
typedef struct _DRIVERSTATUS
{
	BYTE  bDriverError;	//驱动返回错误代码
	BYTE  bIDEStatus;	//IDE内容错误记录
						//仅当bDriverError为SMART_IDE_ERROR时有效
	BYTE  bReserved[2];
	DWORD dwReserved[2];
}DRIVERSTATUS,*PDRIVERSTATUS,*LPDRIVERSTATUS;

//定义SENDCMDOUTPARAMS结构
typedef struct _SENDCMDOUTPARAMS
{
	DWORD		 cBufferSize;	//bBuffer的大小
	DRIVERSTATUS DriverStatus;	//驱动器的状态结构
	BYTE		 bBuffer[1];
}SENDCMDOUTPARAMS,*PSENDCMDOUTPARAMS,*LPSENDCMDOUTPARAMS;

//定义SRB_IO_CONTROL 结构
typedef struct _SRB_IO_CONTROL
{
	ULONG HeaderLength;
	UCHAR Signauture[8];
	ULONG Timeout;
	ULONG ControlCode;
	ULONG ReturnCode;
	ULONG Length;
}SRB_IO_CONTROL,*PSRB_IO_CONTROL;

//定义IDEREGS结构
typedef struct _IDEREGS
{
	BYTE bFeaturesReg;
	BYTE bSectorCountReg;
	BYTE bSectorNumberReg;
	BYTE bCylLowReg;
	BYTE bCylHighReg;
	BYTE bDriveHeadReg;
	BYTE bCommandReg;
	BYTE bReserved;
}IDEREGS,*PIDEREGS,*LPIDEREGS;

//定义SENDCMDINPARAMS结构
typedef struct _SENDCMDINPARAMS
{
	DWORD	cBufferSize;	//缓冲区的大小
	IDEREGS irDriveRegs;	//驱动器在注册表的结构
	BYTE	bDriveNumber;	//有4种物理磁盘样式（0，1，2，3）
	DWORD	dwReserved[4]; //保留字段
	BYTE	bBusffer[1];	//输出缓冲区
}SENDCMDINPARAMS,*PSENDCMDINPARAMS,*LPSENDINPARAMS;

//定义GETVERSIONOUTPARAMS结构
typedef struct _GETVERSIONOUTPARAMS
{
	BYTE	bVersion;	//红运器的版本
	BYTE	bRevision;	//红运器的新版本
	BYTE	bReserved;	//没有应用
	BYTE	bIDEDeviceMap;	//IDE驱动器的map图
	DWORD	fCapabilities;	//驱动器的大小
	DWORD	dwReservedc[4];	//为其他属性保留
}GETVERSIONOUTPARAMS,*PGETVERSIONOUTPARAMS,*LPGETVERSIONOUTPARAMS;

class CGetHDSerial
{
public:
	CGetHDSerial(void);
	virtual ~CGetHDSerial(void);
	void _stdcall Win9xReadHDSerial(WORD* buffer);
	char* GetHDSerial();
	DWORD GetCPUID();
	char* WORDToChar(WORD diskdata[256],int firstIndex,int lastIndex);
	char* DWORDToChar(DWORD diskdata[256],int firstIndex,int lastIndex);
	BOOL WinNTReadSCSIHDSerial(DWORD* buffer);
	BOOL WinNTReadIDEHDSerial(DWORD* buffer);
	BOOL WinNTGetIDEHDInfo(HANDLE hPhysicalDriveIOCTL,
						   PSENDCMDINPARAMS pSCIP,
						   PSENDCMDOUTPARAMS pSCOP,
						   BYTE bIDCmd,
						   BYTE bDriveNum,
						   PDWORD lpcbBytesReturned);
};
