using System;

namespace JetermEntity
{
    /// <summary>
    /// 公共枚举订定义
    /// </summary>
    [Serializable]
    public class EtermCommand
    {
        /// <summary>
        /// Eterm调用方法枚举
        /// </summary>
        public enum CmdType
        {
            /// <summary>
            /// 订位
            /// </summary>
            Booking = 0,
            /// <summary>
            /// P特价
            /// </summary>
            GetPrice = 1,
            /// <summary>
            /// 提取PNR编码
            /// </summary>
            SeekPNR = 2,
            /// <summary>
            /// DETR大编码
            /// </summary> 
            TicketByBigPnr = 3,
            /// <summary>
            /// DETR票号 查票号状态
            /// </summary> 
            TicketInfo = 4,
            /// <summary>
            /// DETR票号,S
            /// </summary> 
            TicketInfoByS = 5,
            /// <summary>
            /// DETR票号,F
            /// </summary> 
            TicketInfoByF = 6,
            /// <summary>
            /// DETR票号,H 查票号历史记录
            /// </summary> 
            TicketHisInfo = 7,
            /// <summary>
            /// 授权
            /// </summary>
            Rmk = 8,
            /// <summary>
            /// 擦编码
            /// </summary>
            CancelPnr = 9,
            /// <summary>
            /// 自动出票
            /// </summary>
            AutoTicket = 10,
            /// <summary>
            /// AV查询
            /// </summary>
            AV = 11,
            /// <summary>
            /// AVH查询
            /// </summary>
            AVH = 12
        }

        /// <summary>
        /// 配置状态
        /// </summary>
        public enum ConfigState
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
        }

        /// <summary>
        /// 性别
        /// </summary>
        public enum Sexual
        {
            /// <summary>
            /// 男
            /// </summary>
            male,
            /// <summary>
            /// 女
            /// </summary>
            female
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public enum IDtype
        {
            /// <summary>
            /// 没有设置
            /// </summary>
            NotSet = -1,
            /// <summary>
            /// 身份证
            /// </summary>
            IDcard,
            /// <summary>
            /// 其他
            /// </summary>
            Other
        }

        /// <summary>
        /// 乘客类型
        /// </summary>
        public enum PassengerType
        {
            /// <summary>
            /// 没有设置
            /// </summary>
            NotSet = -1,
            /// <summary>
            /// 成人
            /// </summary>
            Adult = 0,
            /// <summary>
            /// 儿童
            /// </summary>
            Children = 1,
            /// <summary>
            /// 婴儿
            /// </summary>
            Baby = 2
        }

        /// <summary>
        /// 配置级别
        /// </summary>
        public enum ConfigLevel
        {
            /// <summary>
            /// A
            /// </summary>
            A,
            /// <summary>
            /// B
            /// </summary>
            B,
            /// <summary>
            /// C
            /// </summary>
            C,
            /// <summary>
            /// D
            /// </summary>
            D,
            /// <summary>
            /// E
            /// </summary>
            E
        }

        /// <summary>
        /// 外部操作方法（用于自动出票）
        /// </summary>
        public enum OperatMethod
        {
            /// <summary>
            /// 添加Eterm配置
            /// </summary>
            add,
            /// <summary>
            /// 删除Eterm配置
            /// </summary>
            del,
            /// <summary>
            /// 修改Eterm配置
            /// </summary>
            mod
        }
        /// <summary>
        /// 缓存时间
        /// </summary>
        public enum CacheTime
        {
            /// <summary>
            /// 不存/取缓存
            /// </summary>
            none = 0,
            /// <summary>
            /// 缓存1分钟
            /// </summary>
            min1 = 60,
            /// <summary>
            /// 缓存5分钟
            /// </summary>
            min5 = 5 * 60,
            /// <summary>
            /// 缓存10分钟
            /// </summary>
            min10 = 10 * 60,
            /// <summary>
            /// 缓存30分钟
            /// </summary>
            min30 = 30 * 60,
            /// <summary>
            /// 永久缓存(30天)
            /// </summary>
            infinite = 30 * 24 * 3600
        }

        /// <summary>
        /// Flight Type
        /// </summary>
        public enum FlightType
        {
            O, //  单程
            F, // 往返
            T // 联程

        }
                
        /// <summary>
        /// 客票状态类型
        /// </summary>
        public enum TicketStatus
        {
            NotSet,
            OPEN_FOR_USE,
            VOID,
            REFUNDED,
            CHECKED_IN,
            USED_FLOWN,
            SUSPENDED,
            PRINT_EXCH,
            EXCHANGED,
            LIFT_BOARDED,
            FIM_EXCH,
            AIRP_CNTL_YY,
            CPN_NOTE,
            USED_CLOSED
        }

        /// <summary>
        /// 服务器来源
        /// </summary>
        public enum ServerSource
        {
            /// <summary>
            /// 新版本调用Eterm方式(志勇)
            /// </summary>
            EtermServer,
            /// <summary>
            ///旧版本调用Eterm方式(宋的)
            /// </summary>
            EtermRemote,
            /// <summary>
            /// 走大系统
            /// </summary>
            BigSystem
        }
        public enum BookingState
        {
            /// <summary>
            /// 订位成功
            /// </summary>
            BookingSuccess,
            /// <summary>
            /// 等待订位
            /// </summary>
            WaitBooking,
            /// <summary>
            /// 订位失败
            /// </summary>
            BookingFail
        }

        /// <summary>
        /// 错误枚举
        /// </summary>
        public enum ERROR
        {
            /// <summary>
            /// 未知错误
            /// </summary>
            [Description("未知错误")]
            NONE,
            /// <summary>
            /// 没有发现可用配置
            /// </summary>
            [Description("没有发现可用配置")]
            NO_FIND_CONFIG,
            /// <summary>
            /// 无效的业务名称
            /// </summary>
            [Description("无效的业务名称")]
            INVALID_BUSINESS,
            /// <summary>
            /// 无效的请求参数
            /// </summary>
            [Description("无效的请求参数")]
            INVALID_REQUEST_PARAM,
            /// <summary>
            /// 没有传必须传的请求参数值
            /// </summary>
            [Description("没有传必须传的请求参数值")]
            EMPTY_REQUEST_PARAM,
            /// <summary>
            /// 给儿童订位时，成人PNR请求参数必传
            /// </summary>
            [Description("给儿童订位时，成人PNR请求参数必传")]
            EMPTY_REQUEST_ADULT_PNR,
            /// <summary>
            /// 没有给各Passenger传入必须传的请求参数值，如：乘客类型、姓名、ID号、ID卡类型
            /// </summary>
            [Description("没有给各Passenger传入必须传的请求参数值，如：乘客类型、姓名、ID号、ID卡类型")]
            EMPTY_PASSENGERLIST,
            /// <summary>
            /// 没有给Mobile请求参数传值，或者是给Mobile传了无效的值
            /// </summary>
            [Description("没有给Mobile请求参数传值，或者是给Mobile传了无效的值")]
            EMPTY_OR_INVALID_MOBILE,
            /// <summary>
            /// 无效的Phone No值
            /// </summary>
            [Description("无效的Phone No值")]
            INVALID_PHONENO,
            /// <summary>
            /// 无效的【PAT:A】指令请求参数
            /// </summary>
            [Description("无效的【PAT:A】指令请求参数")]
            PATINVALID_REQUEST_PARAM,
            /// <summary>
            /// 没有给【PAT:A】指令传必须传的请求参数值
            /// </summary>
            [Description("没有给【PAT:A】指令传必须传的请求参数值")]
            EMPTY_PAT_REQUEST_PARAM,
            /// <summary>
            /// 航班号请求参数格式不正确，输入格式应如CZ380
            /// </summary>
            [Description("航班号请求参数格式不正确，输入格式应如CZ380")]
            INVALID_FLIGHTNO,
            /// <summary>
            /// 出发日期请求参数格式不正确，输入格式应如02JAN
            /// </summary>
            [Description("出发日期请求参数格式不正确，输入格式应如02JAN")]
            INVALID_SDATE,
            /// <summary>
            /// 传入的乘客列表中，有乘客名字不合法
            /// </summary>
            [Description("传入的乘客列表中，有乘客名字不合法")]         
            INVALID_PASSENGERLIST,
            /// <summary>
            /// 预订命令返回为空
            /// </summary>
            [Description("预订命令返回为空")]
            ORDER_COMMAND_EMPTY,
            /// <summary>
            /// RT命令返回为空
            /// </summary>
            [Description("RT命令返回为空")]
            RT_COMMAND_EMPTY,
            /// <summary>
            /// 指令返回为空
            /// </summary>
            [Description("指令返回为空")]
            COMMAND_EMPTY,
            /// <summary>
            /// 服务器返回错误
            /// </summary>
            [Description("服务器返回错误")]
            REMOTE_ERROR,
            /// <summary>
            /// 订位系统出现故障，暂时无法进行订位
            /// </summary>
            [Description("订位系统出现故障，暂时无法进行订位")]
            BOOKING_REMOTE_ERROR,
            /// <summary>
            /// 航信信息解析错误
            /// </summary>
            [Description("航信信息解析错误")]
            PARSE_INFO_ERROR,
            /// <summary>
            /// 很抱歉，订位失败，请重新预订
            /// </summary>
            [Description("很抱歉，订位失败，请重新预订")]
            BOOKING_FAIL,
            /// <summary>
            /// 很抱歉，不能授权
            /// </summary>
            [Description("很抱歉，不能授权")]
            RMK_FAIL,
            /// <summary>
            /// 正则验证错误
            /// </summary>
            [Description("正则验证错误")]
            REGEX_FORMAT,
            /// <summary>
            /// 缺少请求参数
            /// </summary>
            [Description("缺少请求参数")]
            REQUEST_PARAM_ABSENCE,
            /// <summary>
            /// 缺少舱位或航班号【PAT:A】指令请求参数
            /// </summary>
            [Description("缺少舱位或航班号【PAT:A】指令请求参数")]
            PATNO_CABIN_FLIGHTNO,
            /// <summary>
            /// 系统出现故障
            /// </summary>
            [Description("系统出现故障")]
            SYSTEM_FAULT,
            /// <summary>
            /// 需要授权
            /// </summary>
            [Description("需要授权")]
            REMARK_OFFICE,
            /// <summary>
            /// 此记录编号不存在
            /// </summary>
            [Description("此记录编号不存在")]
            NOT_EXIST_PNR,
            /// <summary>
            /// 此记录编号无效，状态为已取消
            /// </summary>
            [Description("此记录编号无效，状态为已取消")]
            CANCELLED,
            /// <summary>
            /// 不允许擦编码状态为RR的编码，因为您CancelOut请求参数传的是false。
            /// </summary>
            [Description("不允许擦编码状态为RR的编码，因为您CancelOut请求参数传的是false。")]
            CANCEL_PNR_STATE_RR,
            /// <summary>
            /// 未将成人编码输入到儿童编码里，指令为：“SSR OTHS 航空公司代码 ADULT PNR IS 成人编码”
            /// </summary>
            [Description("未将成人编码输入到儿童编码里，指令为：“SSR OTHS 航空公司代码 ADULT PNR IS 成人编码”")]
            NO_AUDLT_PNR,
            /// <summary>
            /// 成人编码不含婴儿航段，请做入后再导入，或选择预订创单！
            /// </summary>
            [Description("成人编码不含婴儿航段，请做入后再导入，或选择预订创单！")]
            AUDLT_NO_BABY,
            /// <summary>
            /// 一位乘客只能带一个婴儿，请检查！
            /// </summary>
            [Description("一位乘客只能带一个婴儿，请检查！")]
            ONE_PASSENGER_ON_BABY,
            /// <summary>
            /// 此记录第1段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第1段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号")]
            FIRST_SEGMENT_STATE_NO,
            /// <summary>
            /// 此记录第1段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第1段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号")]
            FIRST_SEGMENT_STATE_HL,
            /// <summary>
            /// 此记录第1段航班信息的编号为“TN”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第1段航班信息的编号为“TN”状态不是有效记录编号，请检查您的PNR记录编号")]
            FIRST_SEGMENT_STATE_TN,
            /// <summary>
            /// 此记录第1段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第1段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号")]
            FIRST_SEGMENT_STATE_HN,
            /// <summary>
            /// 此记录第1段航班信息的编号为“HX”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第1段航班信息的编号为“HX”状态不是有效记录编号，请检查您的PNR记录编号")]
            FIRST_SEGMENT_STATE_HX,
            /// <summary>
            /// 此记录第1段航班信息的编号为“SA”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第1段航班信息的编号为“SA”状态不是有效记录编号，请检查您的PNR记录编号")]
            FIRST_SEGMENT_STATE_SA,
            /// <summary>
            /// 此记录第2段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第2段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号")]
            SECOND_SEGMENT_STATE_NO,
            /// <summary>
            /// 此记录第1段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第2段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号")]
            SECOND_SEGMENT_STATE_HL,
            /// <summary>
            /// 此记录第2段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第2段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号")]
            SECOND_SEGMENT_STATE_HN,
            /// <summary>
            /// 此记录第3段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第3段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号")]
            THIRD_SEGMENT_STATE_NO,
            /// <summary>
            /// 此记录第3段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第3段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号")]
            THIRD_SEGMENT_STATE_HL,
            /// <summary>
            /// 此记录第3段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第3段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号")]
            THIRD_SEGMENT_STATE_HN,
            /// <summary>
            /// 此记录第4段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第4段航班信息的编号位置被“NO”掉了，请检查您的PNR记录编号")]
            FORTH_SEGMENT_STATE_NO,
            /// <summary>
            /// 此记录第4段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第4段航班信息的编号为“HL”状态不是有效记录编号，请检查您的PNR记录编号")]
            FORTH_SEGMENT_STATE_HL,
            /// <summary>
            /// 此记录第4段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号
            /// </summary>
            [Description("此记录第4段航班信息的编号为“HN”状态不是有效记录编号，请检查您的PNR记录编号")]
            FORTH_SEGMENT_STATE_HN,
            /// <summary>
            /// 针对海航(HU)A舱请备注OSI项，OSI项格式如：OSI HU CKIN SSAC/S1
            /// </summary>
            [Description("针对海航(HU)A舱请备注OSI项，OSI项格式如：OSI HU CKIN SSAC/S1")]
            HU_CABIN_OSI,
            /// <summary>
            /// 您订的往返程编码不属于同一家航空公司，请检查后重新导入
            /// </summary>
            [Description("您订的往返程编码不属于同一家航空公司，请检查后重新导入")]
            PNR_AIRCOM_NO_INCLUDE,
            /// <summary>
            /// 此记录编号暂时无法导入创建订单
            /// </summary>
            [Description("此记录编号暂时无法导入创建订单")]
            PNR_NO_CREATE_ORDER,
            /// <summary>
            /// 此记录编号含有儿童，请使用儿童PNR导入创建订单
            /// </summary>
            [Description("此记录编号含有儿童，请使用儿童PNR导入创建订单")]
            PNR_USE_CHILD,
            /// <summary>
            /// 需要PAT:A*IN指令
            /// </summary>
            [Description("需要PAT:A*IN指令")]
            NEED_PAT,
            /// <summary>
            /// 南航(CZ)儿童需要加以下SSR项，
            /// 应在SSR项输入旅客标识，SSR项格式如下：
            /// SSR CHLD 航空公司代码 Action-Code1/出生日期/Pn
            /// 示例指令： SSR CHLD CZ HK1/03MAR04/P1
            /// </summary>
            [Description("南航(CZ)儿童需要加以下SSR项，<br>应在SSR项输入旅客标识，SSR项格式如下：<br>SSR CHLD 航空公司代码 Action-Code1/出生日期/Pn<br>示例指令： SSR CHLD CZ HK1/03MAR04/P1")]
            CZ_CHILD_NEED_SSR,
            /// <summary>
            /// 此记录编号含有成人，请将成人与儿童拆开导入，请将成人编码输入到儿童编码里
            /// </summary>
            [Description("此记录编号含有成人，请将成人与儿童拆开导入，请将成人编码输入到儿童编码里")]
            PNR_CONTACT_AUDLT,
            /// <summary>
            /// 当前暂不支持往返儿童票，请将往返儿童票拆成两个单程编码导入
            /// </summary>
            [Description("当前暂不支持往返儿童票，请将往返儿童票拆成两个单程编码导入")]
            NOT_SUPPORT_ROUNDTRIP_CHILDTICKET,
            /// <summary>
            /// 【DETR:CN/{大编码号},C】命令没有返回结果
            /// </summary>
            [Description("【DETR:CN/{大编码号},C】命令没有返回结果")]
            DETR_CN_C_COMMAND_RESULT_EMPTY,
            /// <summary>
            /// 命令返回结果格式不正确
            /// </summary>
            [Description("命令返回结果格式不正确")]
            COMMAND_RESULT_FORMAT_INCORRECT,
            /// <summary>
            /// 【PAT:A】命令返回结果格式不正确
            /// </summary>
            [Description("【PAT:A】命令返回结果格式不正确")]
            PATCOMMAND_RESULT_FORMAT_INCORRECT,
            /// <summary>
            /// 【PAT:A】命令没有返回结果
            /// </summary>
            [Description("【PAT:A】命令没有返回结果")]
            PATCOMMAND_NO_RESULT,
            /// <summary>
            /// 命令返回结果格式不正确--缺乏起飞航班信息或起飞航班信息格式不正确
            /// </summary>
            [Description("命令返回结果格式不正确--缺乏起飞航班信息或起飞航班信息格式不正确")]
            COMMAND_RESULT_FORMAT_INCORRECT_FM,
            /// <summary>
            /// 命令返回结果格式不正确--出发城市三字码格式不正确
            /// </summary>
            [Description("命令返回结果格式不正确--出发城市三字码格式不正确")]
            COMMAND_RESULT_FORMAT_INCORRECT_SCITY,
            /// <summary>
            /// 命令返回结果格式不正确--缺乏RL信息
            /// </summary>
            [Description("命令返回结果格式不正确--缺乏RL信息")]
            COMMAND_RESULT_FORMAT_INCORRECT_RL,
            /// <summary>
            /// 命令返回结果格式不正确--缺乏TO信息
            /// </summary>
            [Description("命令返回结果格式不正确--缺乏TO信息")]
            COMMAND_RESULT_FORMAT_INCORRECT_TO,
            /// <summary>
            /// 没有符合查询条件的记录
            /// </summary>
            [Description("没有符合查询条件的记录")]
            NO_MATCHED_RECORD,
            /// <summary>
            /// 成人编码不是RR状态，无法创单，请成人出票后再创单！
            /// </summary>
            [Description("成人编码不是RR状态，无法创单，请成人出票后再创单！")]
            NO_ADULT_TICKET,
            /// <summary>
            /// 婴儿航段状态不对，请检查！
            /// </summary>
            [Description("婴儿航段状态不对，请检查！")]
            INCORRECT_BABY_FLIGHTINFO,
            /// <summary>
            /// 出生年月非婴儿，请核实！
            /// </summary>
            [Description("出生年月非婴儿，请核实！")]
            INCORRECT_BABY_BIRTHDAY,
            /// <summary>
            /// 此记录编号未添加证件号码，加入后请重新导入PNR记录编号
            /// </summary>
            [Description("此记录编号未添加证件号码，加入后请重新导入PNR记录编号")]
            NO_CARDNO,
            /// <summary>
            /// 获取价格PAT指令返回结果解析失败
            /// </summary>
            [Description("获取价格PAT指令返回结果解析失败")]
            PARSE_PRICE_FAIL,
            /// <summary>
            /// Eterm指令返回结果解析失败
            /// </summary>
            [Description("Eterm指令返回结果解析失败")]
            PARSE_FAIL,
            /// <summary>
            /// 票号不存在
            /// </summary>
            [Description("票号不存在")]
            NO_TICKET,
            /// <summary>
            /// 没找到票
            /// </summary>
            [Description("没找到票")]
            TICKET_NOT_FOUND,          
            /// <summary>
            /// 没有记录
            /// </summary>
            [Description("没有记录")]
            NO_RECORD,
            /// <summary>
            /// 请先初始化请求对象
            /// </summary>
            [Description("请先初始化请求对象")]
            FIRST_INITIALISE_REQUEST,
            /// <summary>
            /// 请先设置查询条件：航班号FlightNo、出发城市SCity以及到达城市ECity
            /// </summary>
            [Description("请先设置查询条件：航班号FlightNo、出发城市SCity以及到达城市ECity")]
            NO_QUERY_CONDITION_OF_TICKETBYBIGPNR,
            /// <summary>
            /// 自定义错误信息
            /// </summary>
            [Description("自定义错误信息")]
            SELFDEFINE_ERROR_MESSAGE,
            /// <summary>
            /// 请给AVH指令传具有正确格式的请求参数值，如：出发城市三字码SCity传入格式如SHE、到达城市三字码ECity传入格式如PEK、航司Airline传入格式如CA
            /// </summary>
            [Description("请给AVH指令传具有正确格式的请求参数值，如：出发城市三字码SCity传入格式如SHE、到达城市三字码ECity传入格式如PEK、航司Airline传入格式如CA")]
            INVALID_AVH_REQUEST_PARAM,
            /// <summary>
            /// AVH指令返回结果格式不正确
            /// </summary>
            [Description("AVH指令返回结果格式不正确")]
            INVALID_AVH_RESULT,
            /// <summary>
            /// 很抱歉，您没有权限查看舱位剩余可订数
            /// </summary>
            [Description("很抱歉，您没有权限查看舱位剩余可订数")]
            NO_PERMISSION_CHECK_AVNUMBER,
            /// <summary>
            /// 很抱歉，不能查询到历史起飞日期的舱位剩余可订数
            /// </summary>
            [Description("很抱歉，不能查询历史起飞日期的舱位剩余可订数")]
            NO_CHECK_HISTORY_AVNUMBER,
            /// <summary>
            /// 很抱歉，只能查询到起飞日期在1年内的舱位剩余可订数
            /// </summary>
            [Description("很抱歉，只能查询到起飞日期在1年内的舱位剩余可订数")]
            ONLY_CHECK_WITHINONEYEAR_AVNUMBER,
            /// <summary>
            /// AV指令返回结果格式不正确
            /// </summary>
            [Description("AV指令返回结果格式不正确")]
            INVALID_AV_RESULT,
            /// <summary>
            /// 很抱歉，指令返回结果中没有显示舱位可订数
            /// </summary>
            [Description("很抱歉，指令返回结果中没有显示舱位可订数")]
            NOSHOW_CARBIN_NUMBER//,
            ///// <summary>
            ///// 很抱歉，没有查找到符合条件的舱位可订数
            ///// </summary>
            //[Description("很抱歉，没有查找到符合条件的舱位可订数")]
            //NO_QUERY_AV_RESULT
        }
    }
}
