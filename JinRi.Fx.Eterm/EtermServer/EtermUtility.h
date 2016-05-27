#pragma once
class EtermUtility
{
public:
	EtermUtility();
	~EtermUtility();

	enum CmdType
	{
		/// <summary>
		/// 订位
		/// </summary>
		SS = 0,
		/// <summary>
		/// P特价
		/// </summary>
		SSPAT = 1,
		/// <summary>
		/// RT编码
		/// </summary>
		RT = 2,
		/// <summary>
		/// RT+PAT 成人编码
		/// </summary>
		RT_PAT = 3,
		/// <summary>
		/// RT+PAT *CH 儿童编码
		/// </summary>
		RT_PATCH = 4,
		/// <summary>
		/// RT+PAT *INF 成人编码
		/// </summary>
		RT_PATINF,
		/// <summary>
		/// DETR大编码
		/// </summary> 
		DETR_CN,
		/// <summary>
		/// DETR票号 查票号状态
		/// </summary> 
		DETR_TN,
		/// <summary>
		/// DETR票号,S
		/// </summary> 
		DETR_TNS,
		/// <summary>
		/// DETR票号,F
		/// </summary> 
		DETR_TNF,
		/// <summary>
		/// DETR票号,H 查票号历史记录
		/// </summary> 
		DETR_TNH,
		/// <summary>
		/// 授权
		/// </summary>
		RMK,
		/// <summary>
		/// 擦编码
		/// </summary>
		XEPNR
	};

	enum ConfigState
	{
		/// <summary>
		/// 已连接
		/// </summary>
		connect,
		/// <summary>
		/// 连接关闭
		/// </summary>
		disconnect,
		/// <summary>
		/// 挂起
		/// </summary>
		suspend
	};

	//功能
	CmdType Utility;

	//请求地址
	CString ServerUrl;

	//OfficeNo
	CString OfficeNo;

	//当前状态
	ConfigState State;

};

