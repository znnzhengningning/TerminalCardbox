/*************************************************************************
*  @brief：外设接口类集合
*  松美小票打印机，参考文件《SiupoPrinterSDK-Demo-DLL-CShare-V2.0.2.3.zip》
*  社保卡个人化接口，动态库源码《HN_PERSONAL_UDLL》
*  身份证读卡器接口
***************************************************************************/
using CommenLib.Sec;
using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

//using F5HANDLE = System.Int32;

namespace SHBSS
{
    public class DevMng
    {
        static public string strError = "";
        static public int DevInit()
        {
            // 小票机初始化
            //Txp532Printer.GetInstance().OpenPrinter();

            // 身份证阅读器初始化
            //if (!( SiCard.isOpen = SiCard.OpenCVR())) { strError = SiCard.errMsg; return -1; }      

            //F8SAN f8 = F8SAN.GetInstance();
            //if(f8.F8Connect() != 0) { strError = f8.strError; return -1; }

            //if (f8.CheckCar()) { f8.car2recycle(); }

            //if (f8.CheckReader()) { f8.reader2car(); f8.car2recycle(); }

            F5SAN f5 = F5SAN.GetInstance();
            if (f5.F5Connect() != 0) { strError = f5.strError; return -1; }

            //if (!Solid510.Init()) { strError = Solid510.strError; return -1; }

            return 0;
        }
    }

    /// <summary>
    /// 卡盒模组
    /// </summary>
    public class F5SAN
    {
        private static F5SAN _instance = null;
        public F5SAN()
        {
        }
        public static F5SAN GetInstance()
        {
            if (_instance == null)
            {
                _instance = new F5SAN();
            }
            return _instance;
        }

        #region 卡盒模组

        [StructLayout(LayoutKind.Sequential)]
        public struct F5CMDRESULT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] RDTBuff;
            public int RDTLength;
            byte b1;
            byte b0;
            byte b2;
        }

        /**
	*【功能】：建立与设备间的连接
	*【参数】：
	*@pszPortName：   串口名称如："COM1" 范围(1-255）
	*@iBaudRate： 波特率，默认38400
	*@bParity  :  奇偶校验，默认偶校验
	*@bTCMode ：  传输控制方式。可用值:
	*				    F5TCMODE_CCB(1)
	*			  默认	F5TCMODE_MST(2) 
	*@pszLogDir： 日志所在路劲
	*@phDevice：  返回一个标识发卡机连接的句柄
	*【返回值】:  成功为0，失败非0
	*/

        [DllImport("f5sanapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int F5_Connect(
             string pszPortName,
             int iBaudRate,
             byte bParity,
             byte bTCMode,
             string pszLogDir,
             ref Int32 phDevice
        );

        /**
        *【功能】：   断开与设备间的连接
        *@hDevice：   引用F5_Connect 返回的句柄值
        *【返回值】:  成功为0，失败非0
        */
        [DllImport("f5sanapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int F5_Disconnect(
             Int32 hDevice
        );


        /*
        *【功能】：    命令报文发送
        *【参数】:
        * @hDevice：   引用F5_Connect 返回的句柄值
        * @pbCDTBuff： 指向要发送的命令包(具体可参考协议文档)，CM+PM+DATA
        * @iCDTLength：发送命令包pbCDTBuff的实际长度
        * @iTimeOut：  命令响应的最大超时时间(毫秒)
        * @lpResult： 指向 F5CMDRESULT 结构体的指针
        *【返回】:  成功为0，失败非0
        */
        [DllImport("f5sanapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int F5_Execute(
            int hDevice,
            byte[] pbCDTBuff,
            int cbCDTLength,
            int nTimeOut,
            ref F5CMDRESULT lpResult
        );


        /*
        *【功能】：    与射频卡有关的命令报文发送
        *【参数】:
        * @hDevice：   引用F5_Connect 返回的句柄值
        * @pbCDTBuff： 指向要发送的命令包(具体可参考协议文档)，CM+PM+DATA
        * @iCDTLength：发送命令包pbCDTBuff的实际长度
        * @iTimeOut：  命令响应的最大超时时间(毫秒)
        * @lpResult：  指向 F5CMDRESULT 结构体的指针
        *【返回】:     成功为0，失败非0
        */
        [DllImport("f5sanapi.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int F5_RFExecute(
            int hDevice,
            byte[] pbCDTBuff,
            int cbCDTLength,
            int nTimeOut,
            ref F5CMDRESULT lpResult
        );

        const int E_PORT_NOT_AVAIL = -1;// 通信端口不可用
        const int E_PORT_WRITE = -2;// 串口数据发送失败
        const int E_PORT_READ = -3;// 串口数据读取失败
        const int E_TIMEOUT = -4;//通讯超时
        const int E_RECEPTION = -5;//数据异常
        const int E_DEV_NOT_READY = -6;//设备未就绪
        const int E_INVALID_ARG = -7;//无效参数
        const int E_OUT_MEMORY = -8;//内存溢出
        const int E_INVALID_HANDLE = -9;//无效句柄
        const int E_NULL_POINTER = -10;//空指针
        const int E_INTERNAL_ERROR = -11;//内部错误
        const int E_CANCELED = -12;//当前操作被取消
        const int E_USB_DLL = -13;//USB动态库加载失败
        const int E_USB_ENUM_EMPTY = -14;// 找不到USB设备
        const int E_USB_OPEN = -15;// USB设备打开失败
        const int E_USB_WRITE = -16;// USB设备写入数据失败
        const int E_USB_READ = -17;// USB设备读取数据失败
        const int E_USB_INIT_FAILED = -18;// USB初始化失败
        const int E_USB_LOAD = -19;//USB接口加载失败

        const int F5_E_VERTICAL_MOTOR = (-22); // 垂直电机故障
        const int F5_E_CAM_MOTOR = (-23); //凸轮电机故障
        const int F5_E_ACCESS_ROADS = (-24); //入口堵塞
        const int F5_E_READER_TIMEOUT = (-25); //读卡机超时故障
        const int F5_E_READER_COMM_ERROR = (-26); //读卡机不能执行命令
        const int F5_E_SLOT_FULL = (-27); //卡插槽满
        const int F5_E_RECYCLY_FULL = (-28); //回收箱满
        const int F5_E_DOOR_OPEN = (-29); //门被打开
        const int F5_E_COMM_EXECUTE_ERROR = (-30); //命令不能被执行（上电检测到卡或上电初始化有故障）
        const int F5_E_COMM_PARAM_ERROR = (-31); //命令参数错误
        const int F5_E_READER_NO_CARD = (-32); //读卡机内无卡
        const int F5_E_READER_HAVEN_CARD = (-33); //读卡机内有卡
        const int F5_E_SLOT_NO_CARD = (-34); //卡槽内无卡
        const int F5_E_INFOR_MEMORY_CHIP = (-35); //信息存储芯片故障
        const int F5_E_VOLTAGE_NOT_RANGE = (-36); //电压不在范围
        const int F5_E_SENSOR_ERROR = (-37); //传感器故障
        const int F5_E_SLOT_HAVEN_CARD = (-38); //卡槽内有卡
        const int F5_E_CARD_BLOCK_INVALID = (-39); //退卡机模块故障无效

        #endregion

        static public Int32 ghDevice = 0;
        public string ERRHEAD = "ZK0-900E-00: ";
        public string strError = "";
        public int F5Connect()
        {
            if (ghDevice != 0) return 0;
            string strLog = AppDomain.CurrentDomain.BaseDirectory + "F5Log";
            int ret = F5_Connect(Global.GetInstance().F5Com, 38400, 2, 2, strLog, ref ghDevice);
            if (ret == 0) return 0;
            GetReturnMessage(ret, null, ref strError);
            strError = ERRHEAD + strError;
            return ret;
        }

        public int F5Disconnect()
        {
            return F5_Disconnect(ghDevice);
        }

        public int F5Excute(byte[] byteCmd)
        {
            F5CMDRESULT result = new F5CMDRESULT();
            int ret = F5_Execute(ghDevice, byteCmd, byteCmd.Length, 0, ref result);
            if (ret == 0) return 0;
            GetReturnMessage(ret, null, ref strError);
            strError = ERRHEAD + strError;
            return ret;
        }

        public int F5RFExecute(byte[] byteCmd)
        {
            F5CMDRESULT result = new F5CMDRESULT();
            int ret = F5_RFExecute(ghDevice, byteCmd, byteCmd.Length, 0, ref result);
            if (ret == 0) return 0;
            GetReturnMessage(ret, null, ref strError);
            strError = ERRHEAD + strError;
            return ret;
        }

        /// <summary>
        /// 全面复位
        /// </summary>
        /// <returns></returns>
        public bool reset_all()
        {
            // CvF2
            byte[] byteCmd = Encoding.Default.GetBytes("0132400000000");
            if (F5Excute(byteCmd) != 0) return false;
            return true;
        }

        /// <summary>
        /// 小车到回收箱
        /// </summary>
        /// <returns></returns>
        public bool car2recycle()
        {
            int ret = -1;
            // 小车到回收箱
            byte[] bSend = new byte[] { 0x33, 0x71 };
            if ((ret = F5Excute(bSend)) != 0) return false;
            return true;
        }

        /// <summary>
        /// 卡盒到小车
        /// </summary>
        /// <returns></returns>
        public bool box2car()
        {
            int ret = -1;
            // 卡箱到小车
            byte[] bSend = new byte[] { 0x76, 0x43, 0x31 };
            if ((ret = F5Excute(bSend)) != 0) return false;
            return true;
        }

        /// <summary>
        /// 小车到打印机
        /// </summary>
        /// <param name="iPrinter">打印机序号</param>
        /// <returns></returns>
        public bool car2printer(int iPrinter)
        {
            int ret = -1;

            //// 检查小车内是否有卡  
            //if (!CheckCar()) return false;
            Utility.WriteLog(iPrinter.ToString() + "号打印机进卡");
            byte[] bSend;
            if (iPrinter == 1)
            {
                // 小车到彩印机位置1
                bSend = new byte[] { 0x76, 0x44, 0x33 };
                if ((ret = F5Excute(bSend)) != 0) return false;
            }
            else if (iPrinter == 2)
            {
                // 小车到彩印机位置2
                bSend = new byte[] { 0x76, 0x44, 0x34 };
                if ((ret = F5Excute(bSend)) != 0) return false;
            }

            // 卡片进入打印机
            nType = 1;
            Thread th = new Thread(new ThreadStart(Car_Printer));
            th.Start();
            Thread.Sleep(500);

            // 小车前出卡
            bSend = new byte[] { 0x76, 0x44, 0x3b };
            if (F5Excute(bSend) != 0) return false;
            Utility.WriteLog(iPrinter.ToString() + "号打印机进卡成功");
            return true;
        }

        /// <summary>
        /// 打印机到小车
        /// </summary>
        /// <param name="iPrinter"></param>
        /// <returns></returns>
        public bool printer2car(int iPrinter)
        {
            int ret = -1;

            //// 检查小车内是否有卡
            // if (CheckCar()) return false;
            Utility.WriteLog(iPrinter.ToString() + "号打印机出卡");
            byte[] bSend;
            if (iPrinter == 1)
            {
                // 小车到彩印机位置1
                bSend = new byte[] { 0x76, 0x44, 0x33 };
                if ((ret = F5Excute(bSend)) != 0) return false;
            }
            else if (iPrinter == 2)
            {
                // 小车到彩印机位置2
                bSend = new byte[] { 0x76, 0x44, 0x34 };
                if ((ret = F5Excute(bSend)) != 0) return false;
            }

            // 小车前进卡
            nType = 2;
            Thread th = new Thread(new ThreadStart(Car_Printer));
            th.Start();

            // 打印机同步出卡
            if (Solid510.Solid510_Out2Front() != 0) return false;
            Utility.WriteLog(iPrinter.ToString() + "号打印机出卡成功");
            return true;
        }

        static int nType; // 1进卡  2出卡
        static bool b_thRet;
        void Car_Printer()
        {
            if (nType == 1)
            {
                // 打印机进卡
                Solid510.Solid510_FeedContact();
            }
            else if (nType == 2)
            {
                // 小车开始前进卡
                byte[] bSend = new byte[] { 0x76, 0x44, 0x3c };
                if (F5Excute(bSend) != 0) b_thRet = false;
                else b_thRet = true;
            }
        }

        /// <summary>
        /// 小车到读写器通道
        /// </summary>
        /// <returns></returns>
        public bool car2reader()
        {
            int ret = -1;
            // 小车到读写器
            byte[] bSend = new byte[] { 0x76, 0x44, 0x37 };
            if ((ret = F5Excute(bSend)) != 0) return false;
            return true;
        }

        /// <summary>
        /// 读写器通道到小车
        /// </summary>
        /// <returns></returns>
        public bool reader2car()
        {
            int ret = -1;
            byte[] bSend = new byte[] { 0x76, 0x44, 0x38 };
            if ((ret = F5Excute(bSend)) != 0) return false;
            return true;
        }

        /// <summary>
        /// 读写器通道出卡
        /// </summary>
        /// <returns></returns>
        public bool reader2out()
        {
            int ret = -1;
            byte[] bSend = new byte[] { 0x33, 0x30 };
            if ((ret = F5Excute(bSend)) != 0) return false;
            return true;
        }

        /// <summary>
        /// 小车是否夹卡
        /// </summary>
        /// <returns></returns>
        public int IsCarJar()
        {
            F5CMDRESULT result = new F5CMDRESULT();
            byte[] byteCmd = Encoding.Default.GetBytes("v1");
            int ret = F5_Execute(ghDevice, byteCmd, byteCmd.Length, 0, ref result);
            if (ret == 0)
            {
                if (0 != (result.RDTBuff[1] & 0x8) && 0 != (result.RDTBuff[1] & 0x4) && 0 != (result.RDTBuff[1] & 0x2) && 0 != (result.RDTBuff[1] & 0x01))
                    return -44;
                return 0;
            }
            GetReturnMessage(ret, null, ref strError);
            strError = ERRHEAD + strError;
            return ret;
        }

        public const int BoxEmpty = 0;      // 卡盒空
        public const int BoxCard = 1;       // 卡盒有卡
        public const int ResycleEmpty = 0;  // 回收箱空
        public const int ResycleCard = 1;   // 回收箱有卡
        public const int ResycleFull = 2;   // 回收箱满

        public int BoxStatus { get; set; }
        public int ResycleStatus { get; set; }

        /// <summary>
        /// 检测卡箱是否有卡
        /// </summary>
        /// <returns>0 有卡  非0 无卡</returns>
        public bool GetSensorStatus()
        {
            F5CMDRESULT result = new F5CMDRESULT();
            byte[] byteCmd = Encoding.Default.GetBytes("v1");
            int ret = F5_Execute(ghDevice, byteCmd, byteCmd.Length, 0, ref result);
            if (ret == 0)
            {
                byte b;
                if (result.RDTLength >= 1)
                {
                    b = result.RDTBuff[0];
                    if ((b & 1) != 0)
                    {
                        ResycleStatus = ResycleCard;
                    }
                    else
                    {
                        ResycleStatus = ResycleEmpty;
                    }
                    if ((b & 4) != 0)
                    {
                        ResycleStatus = ResycleFull;
                    }
                }
                if (result.RDTLength >= 3)
                {
                    b = result.RDTBuff[2];
                    if ((b & 0x10) != 0)
                        BoxStatus = BoxCard;
                    else
                        BoxStatus = BoxEmpty;
                }
                return true;
            }
            GetReturnMessage(ret, null, ref strError);
            strError = ERRHEAD + strError;
            return false;
        }

        public void GetReturnMessage(long ret, F5CMDRESULT[] lpResult, ref string strMsg)
        {
            switch (ret)
            {
                case 0: strMsg = "操作成功完成。"; break;
                case E_PORT_NOT_AVAIL: strMsg = "通信端口不可用"; break;
                case E_PORT_WRITE: strMsg = "串口数据发送失败"; break;
                case E_PORT_READ: strMsg = "串口数据读取失败。"; break;
                case E_TIMEOUT: strMsg = "通讯超时。"; break;
                case E_RECEPTION: strMsg = "数据异常。"; break;
                case E_DEV_NOT_READY: strMsg = "设备未就绪。"; break;
                case E_INVALID_ARG: strMsg = "无效参数。"; break;
                case E_OUT_MEMORY: strMsg = "内存溢出。"; break;
                case E_INVALID_HANDLE: strMsg = "无效句柄。"; break;
                case E_NULL_POINTER: strMsg = "空指针。"; break;
                case E_INTERNAL_ERROR: strMsg = "内部错误。"; break;
                case E_CANCELED: strMsg = "当前操作被取消。"; break;
                case E_USB_DLL: strMsg = "USB动态库加载失败。"; break;
                case E_USB_ENUM_EMPTY: strMsg = "找不到USB设备"; break;
                case E_USB_OPEN: strMsg = "USB设备打开失败"; break;
                case E_USB_WRITE: strMsg = "USB设备写入数据失败"; break;
                case E_USB_READ: strMsg = "USB设备读取数据失败"; break;
                case E_USB_INIT_FAILED: strMsg = "USB初始化失败"; break;
                case E_USB_LOAD: strMsg = "USB接口加载失败"; break;


                case F5_E_VERTICAL_MOTOR: strMsg = "垂直电机故障"; break;
                case F5_E_CAM_MOTOR: strMsg = "凸轮电机故障"; break;
                case F5_E_ACCESS_ROADS: strMsg = "入口堵塞"; break;
                case F5_E_READER_TIMEOUT: strMsg = "读卡机超时故障"; break;
                case F5_E_READER_COMM_ERROR: strMsg = "读卡机不能执行命令"; break;
                case F5_E_SLOT_FULL: strMsg = "卡插槽满"; break;
                case F5_E_RECYCLY_FULL: strMsg = "回收箱满"; break;
                case F5_E_DOOR_OPEN: strMsg = "门被打开"; break;
                case F5_E_COMM_EXECUTE_ERROR: strMsg = "命令不能被执行（上电检测到卡或上电初始化有故障）"; break;
                case F5_E_COMM_PARAM_ERROR: strMsg = "命令参数错误"; break;
                case F5_E_READER_NO_CARD: strMsg = "读卡机内无卡"; break;
                case F5_E_READER_HAVEN_CARD: strMsg = "读卡机内有卡"; break;
                case F5_E_SLOT_NO_CARD: strMsg = "卡槽内无卡"; break;
                case F5_E_INFOR_MEMORY_CHIP: strMsg = "信息存储芯片故障"; break;
                case F5_E_VOLTAGE_NOT_RANGE: strMsg = "电压不在范围"; break;
                case F5_E_SENSOR_ERROR: strMsg = "传感器故障"; break;
                case F5_E_SLOT_HAVEN_CARD: strMsg = "卡槽内有卡"; break;
                case F5_E_CARD_BLOCK_INVALID: strMsg = "退卡机模块故障无效"; break;
                default:
                    strMsg = "其他错误：" + ret.ToString();
                    break;
            }
            Utility.WriteLog(strMsg);
        }
    }

    /// <summary>
    /// 多卡盘模组
    /// </summary>
    public class F8SAN
    {
        private static F8SAN _instance = null;   //静态私有成员变量，存储唯一实例
        public F8SAN()    //私有构造函数，保证唯一性
        {
            iSlotBuffer[0] = new int[2048];
            iSlotBuffer[1] = new int[2048];
        }
        public static F8SAN GetInstance()    //公有静态方法，返回一个唯一的实例
        {
            if (_instance == null)
            {
                _instance = new F8SAN();
            }
            return _instance;
        }
        #region 宏定义
        public static int F8_S_SUCCESS = 0;
        public static int F8_E_PORT_NOT_AVAIL = -1;
        public static int F8_E_DEV_NOT_READY = (-2);
        public static int F8_E_INTERNAL_ERROR = (-10);
        public static int F8_E_RECEPTION = (-13);
        public static int F8_E_INVALID_HANDLE = (-20);
        public static int F8_E_INVALID_ARG = (-21);
        public static int F8_E_NULL_POINTER = (-22);
        public static int F8_E_BUFFER_TOO_SMALL = (-23);

        public static int F8_E_PRINTER_RESET = (-24);
        public static int F8_E_PRINTER_SENSOR = (-25);
        public static int F8_E_RIBBON = (-26);
        public static int F8_E_INVALID_VALUE_FORMAT = (-27);
        public static int F8_E_PRINTER_EJECT = (-28);
        public static int F8_E_ENUM_PRINTER = (-29);
        public static int F8_E_VERSION = (-30);
        public static int F8_E_PRINTER_INSERT = (-31);
        public static int F8_E_PRINT_CARD = (-32);
        public static int F8_E_PICTURE_PATH = (-33);
        public static int F8_E_CAPTURE = (-34);
        public static int F8_E_CARDIN_PRINTER = (-35);
        public static int F8_E_OUT_OF_MEMORY = (-36);
        public static int F8_E_COMMAND_FAILURE = (-37);
        public static int F8_E_RIBINFO_LESS = (-38);
        public static int F8_E_FILEINFO_LESS = (-39);
        public static int F8_E_PRINTER_NO_CARD = (-40);
        #endregion

        #region 接口定义
        [StructLayout(LayoutKind.Sequential)]
        public struct RESULTDATA
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] RDT;
            public int RDTLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] retain;
        }

        [DllImport("F8API.dll")]
        public static extern int F8_Connect(
            int dwPortNumber,
            ref int phDevice,
            int Parity
            );

        [DllImport("F8API.dll")]
        public static extern int F8_Disconnect(
            int hDevice
            );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int F8_Execute(
            int hDevice,
            byte[] pbCDT,
            int cbCDTLength,
            int dwReqLength,
            ref RESULTDATA lpRD
            );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int F8_ComExecute(
            int hDevice,
            byte[] pbCDT,
            int cbCDTLength,
            int dwReqLength,
            ref RESULTDATA lpRD
            );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int OMR_Execute(
            int hDevice,
            byte[] pbCDT,
            int cbCDTLength,
            int dwReqLength,
            ref RESULTDATA lpRD
            );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int SANKYO_Execute(
            int hDevice,
            byte[] pbCDT,
            int cbCDTLength,
            int dwReqLength,
            ref RESULTDATA lpRD
            );

        ///////获取5A，57标签值
        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int SANKYO_GetICCInfo(
            int hDevice,
            ref StringBuilder pszIDBuff,
            ref int pcbIDLength,
            ref StringBuilder pszTrack2Buff,
            ref int pcbTrack2Length
            );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_Execute(
                int hDevice,
                byte[] pbCDT,
                int dwCDTLength,
                int dwReqLength,
                ref RESULTDATA lpRD
                );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int R2_Execute(
                int hDevice,
                byte[] pbCDT,
                int dwCDTLength,
                int dwReqLength,
                ref RESULTDATA lpRD,
                int iStateOffset = 0,
                int iReplyOffset = 0,
                bool fCheckStatus = false
                );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int R2_GetICCInfo(
                int hDevice,
                ref StringBuilder pszIDBuff,
                ref int pcbIDLength,
                ref StringBuilder pszTrack2Buff,
                ref int pcbTrack2Length
                );
        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_ReadTracks();  // 读磁道  弃用

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_IccCardActivate(
                int hDevice,
                byte bCM,
                byte bPM,
                byte bVoltage,
                ref byte[] pbProtocol,
                ref byte[] pbATRBuff,
                ref int pcbATRLength
                );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_CpuTransmit(
                int hDevice,
                byte bProtocol,
                byte[] pbSendBuff,
                int SendLength,
                ref byte[] pbRevBuff,
                ref int pcbRevLength
                );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_CpuDeactivate(
                int hDevice
                );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_MoveIcPost(
                int hDevice
                );

        [DllImport("F8API.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int A6_DetectICCType(
            int hDevice,
            ref byte[] pbCardType
            );

        #endregion


        static public int ghDevice = 0;
        public string ERRHEAD = "F8-1500-DY-C28: ";
        public string strError = "";
        int[][] iSlotBuffer = new int[2][];
        public int F8Connect()
        {
            if (ghDevice != 0) return 0;
            int ret = F8_Connect(Global.GetInstance().F8Com, ref ghDevice, 0);
            if (ret == 0) return 0;
            F8_GetResultDesc(ret);
            return ret;
        }

        public int F8Disconnect()
        {
            int ret = F8_Disconnect(ghDevice);
            if (ret == 0) return 0;
            F8_GetResultDesc(ret);
            return ret;
        }

        public int F8Excute(byte[] byteCmd)
        {
            RESULTDATA result = new RESULTDATA();
            int ret = F8_Execute(ghDevice, byteCmd, byteCmd.Length, 0, ref result);
            if (ret == 0) return 0;
            F8_GetResultDesc(ret);
            return ret;
        }
        public int SANKYOExecute(byte[] byteCmd)
        {
            RESULTDATA result = new RESULTDATA();
            int ret = SANKYO_Execute(ghDevice, byteCmd, byteCmd.Length, 0, ref result);
            if (ret == 0) return 0;
            F8_GetResultDesc(ret);
            return ret;
        }

        public bool reset_all()
        {
            byte[] bSend = new byte[] { 0x40, 0x30 };
            if (F8Excute(bSend) != 0) return false;
            return true;
        }
        public bool box2car()
        {
            int ret = -1;
            //// 建立连接
            //if (( ret = F8Connect() ) != 0) return false;

            // 查询发卡机卡箱状态
            byte bySendFakaji = 0x00;
            byte bySendBoxAddr = 0x00;
            if (!CheckFakaji(ref bySendFakaji, ref bySendBoxAddr)) return false;

            // 卡箱到小车
            byte[] bSend = new byte[4];
            bSend[0] = 0x43;
            bSend[1] = 0x49;
            //bSend[2] = 0x31;
            bSend[2] = bySendFakaji;
            bSend[3] = bySendBoxAddr;
            if ((ret = F8Excute(bSend)) != 0) return false;
            return true;
        }


        public static int TP_PreMadeCard = 1;
        public static int TP_SiCard = 2;
        public static int TP_ClearAll = 4;
        public bool pan2car(int nBlock, int nSlot, int TP_Card)
        {
            // 查询卡槽是否有卡
            //if(!CheckSlot(nBlock, nSlot)) return false;

            // 卡盘到小车
            byte[] bSend = new byte[] { 0x43, 0x4e, (byte)nBlock, (byte)(nSlot >> 8), (byte)nSlot };
            int ret = F8Excute(bSend);
            if (ret != 0)
            {
                if (ret == 0x4E0006)
                {
                    if (TP_Card == TP_PreMadeCard)
                        MDATASQL.DeletePreCard(nSlot, nBlock);
                    else if (TP_Card == TP_SiCard)
                        MDATASQL.DeleteSiCard(nSlot, nBlock);
                }
                return false;
            }

            // 从数据库移除数据
            if (TP_Card == TP_PreMadeCard)
                MDATASQL.DeletePreCard(nSlot, nBlock);
            else if (TP_Card == TP_SiCard)
                MDATASQL.DeleteSiCard(nSlot, nBlock);

            return true;
        }
        public bool car2pan(int nBlock, int nSlot, int TP_Card)
        {
            //if (( nSlot = CheckPan(nBlock) ) < 0) return false;
            // 小车到卡槽
            byte[] bSend = new byte[5];
            //if (nBlock == 1) nSlot = nSlot - 540;
            bSend[0] = 0x43;
            bSend[1] = 0x4a;
            bSend[2] = (byte)nBlock;//0号卡盘
            bSend[3] = (byte)(nSlot / 256);//卡槽地址高字节
            bSend[4] = (byte)nSlot; //卡槽地址低字节
            if (F8Excute(bSend) != 0) return false;

            // 添加到数据库
            MDATA mdata = MDATA.getInstance();
            mdata.nCardPosition = nSlot;
            mdata.nCartRidge = nBlock;
            mdata.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (TP_Card == TP_PreMadeCard)
            {
                MDATASQL.AddCardAllowance();
            }
            else if (TP_Card == TP_SiCard)
            {
                MDATASQL.AddSiCard();
            }

            return true;
        }


        public bool printer2car(int iPos)
        {
            int ret = -1;

            //// 检查小车内是否有卡
            // if (CheckCar()) return false;

            byte[] bSend;
            if (iPos == 1)
            {
                // 小车到彩印机位置1
                bSend = new byte[] { 0x43, 0x45 };
                if ((ret = F8Excute(bSend)) != 0) return false;
            }
            else if (iPos == 2)
            {
                // 小车到彩印机位置2
                bSend = new byte[] { 0x43, 0x53 };
                if ((ret = F8Excute(bSend)) != 0) return false;
            }

            // 小车前进卡
            nType = 2;
            Thread th = new Thread(new ThreadStart(Car_Printer));
            th.Start();

            // 打印机同步出卡
            if (Solid510.Solid510_Out2Front() != 0) return false;

            return true;
        }

        public bool car2printer(int iPos)
        {
            int ret = -1;

            //// 检查小车内是否有卡  
            //if (!CheckCar()) return false;

            byte[] bSend;
            if (iPos == 1)
            {
                // 小车到彩印机位置1
                bSend = new byte[] { 0x43, 0x45 };
                if ((ret = F8Excute(bSend)) != 0) return false;
            }
            else if (iPos == 2)
            {
                // 小车到彩印机位置2
                bSend = new byte[] { 0x43, 0x53 };
                if ((ret = F8Excute(bSend)) != 0) return false;
            }

            // 卡片进入打印机
            nType = 1;
            Thread th = new Thread(new ThreadStart(Car_Printer));
            th.Start();
            Thread.Sleep(500);

            // 小车前出卡
            bSend = new byte[] { 0x43, 0x47 };
            if (F8Excute(bSend) != 0) return false;
            return true;
        }

        static int nType; static bool b_thRet;
        void Car_Printer()
        {
            if (nType == 1)
            {
                // 打印机进卡
                Solid510.Solid510_FeedContact();
            }
            else if (nType == 2)
            {
                // 小车开始前进卡
                byte[] bSend = new byte[] { 0x43, 0x48 };
                if (F8Excute(bSend) != 0) b_thRet = false;
                else b_thRet = true;
            }
        }

        public bool reader2car()
        {
            int ret = -1;
            // 触头离开
            byte[] bSend = new byte[] { 0x49, 0x31 };
            if ((ret = SANKYOExecute(bSend)) != 0) return false;
            bSend = new byte[] { 0x40, 0x32 };
            if ((ret = SANKYOExecute(bSend)) != 0) return false;

            // 读卡器到小车
            //先把卡片移动到机内
            bSend = new byte[] { 0x33, 0x32 };
            if ((ret = SANKYOExecute(bSend)) != 0) return false;
            bSend = new byte[] { 0x43, 0x3c };
            if ((ret = F8Excute(bSend)) != 0) return false;
            return true;
        }

        public bool car2reader()
        {
            int ret = -1;
            // 小车到读写器
            byte[] bSend = new byte[] { 0x43, 0x39 };
            if ((ret = F8Excute(bSend)) != 0) return false;

            // 触头落下
            bSend = new byte[] { 0x40, 0x30 };
            if ((ret = SANKYOExecute(bSend)) != 0) return false;
            return true;
        }

        public bool car2recycle()
        {
            int ret = -1;
            // 小车到回收箱
            byte[] bSend = new byte[] { 0x43, 0x3f, 0x32 };
            if ((ret = F8Excute(bSend)) != 0) return false;
            return true;
        }

        public bool car2errorbox()
        {
            int ret = -1;
            // 小车到废卡箱
            byte[] bSend = new byte[] { 0x43, 0x3f, 0x31 };
            if ((ret = F8Excute(bSend)) != 0) return false;
            return true;
        }
        public bool OutCardFromReader()
        {
            int ret = -1;
            // 卡片从读写器通道出来，前端持卡
            byte[] bSend = new byte[] { 0x43, 0x30 };
            if ((ret = F8Excute(bSend)) != 0) return false;
            return true;
        }

        /// <summary>
        /// 从持卡楼进卡到读写器
        /// </summary>
        /// <returns></returns>
        public bool Out2Reader()
        {
            int ret = -1;
            // 卡片从读写器通道出来，前端持卡
            byte[] bSend = new byte[] { 0x43, 0x31 };
            if ((ret = F8Excute(bSend)) != 0) return false;
            return true;
        }


        /// <summary>
        /// 检查小车是否有卡
        /// </summary>
        /// <returns>true 有卡  false 无卡</returns>
        public bool CheckCar()
        {
            RESULTDATA RD = new RESULTDATA();
            //发卡机状态查询
            byte[] bSend = new byte[] { 0x41, 0x34 };
            int ret = F8_Execute(ghDevice, bSend, 2, 0, ref RD);
            if (ret != 0)
            {
                F8_GetResultDesc(ret);
                return false;
            }
            if (RD.RDT[0] == '1') return true;
            else if (RD.RDT[0] == '2') return true;
            return false;
        }

        /// <summary>
        /// 检查读卡器通道是否有卡
        /// </summary>
        /// <returns>true 有卡 false 无卡</returns>
        public bool CheckReader()
        {
            RESULTDATA RD = new RESULTDATA();
            //读卡器状态查询
            byte[] bSend = new byte[] { 0x41, 0x3C };
            int ret = F8_Execute(ghDevice, bSend, 2, 0, ref RD);
            if (ret != 0)
            {
                F8_GetResultDesc(ret);
                return false;
            }
            if (RD.RDT[0] == '0' && RD.RDT[1] == '0') return false; // 机内无卡
            if (RD.RDT[0] == '0' && RD.RDT[1] == '1') return true;  // 卡在出卡口
            else if (RD.RDT[0] == '0' && RD.RDT[1] == '2') return true; // 卡在机内
            return false;
        }

        /// <summary>
        /// 检查发卡机（2个）
        /// </summary>
        /// <param name="byKaji">返回卡机号</param>
        /// <param name="byBoxAddr">返回卡箱地址 现在每个发卡机只有一个卡箱</param>
        /// <returns></returns>
        public bool CheckFakaji(ref byte byKaji, ref byte byBoxAddr)
        {
            RESULTDATA RD = new RESULTDATA();
            //发卡机状态查询
            byte[] bSend = new byte[] { 0x41, 0x36 };
            int ret = F8_Execute(ghDevice, bSend, 2, 0, ref RD);
            if (ret != 0)
            {
                F8_GetResultDesc(ret);
                return false;
            }
            //1号发卡箱发卡
            //查询发卡箱1状态  
            byKaji = 0x31;
            if (RD.RDT[0] == '3' || RD.RDT[0] == '4') byBoxAddr = 0x31;
            else if (RD.RDT[1] == '3' || RD.RDT[1] == '4') byBoxAddr = 0x32;
            else if (RD.RDT[2] == '3' || RD.RDT[2] == '4') byBoxAddr = 0x33;
            else byBoxAddr = 0x00;
            if (byBoxAddr == 0x00)
            {
                //2号发卡箱发卡
                //查询发卡箱2状态  
                byKaji = 0x32;
                if (RD.RDT[3] == '3' || RD.RDT[3] == '4') byBoxAddr = 0x31;
                else if (RD.RDT[4] == '3' || RD.RDT[4] == '4') byBoxAddr = 0x32;
                else if (RD.RDT[5] == '3' || RD.RDT[5] == '4') byBoxAddr = 0x33;
                else byBoxAddr = 0x00;
                if (byBoxAddr == 0x00)
                {
                    strError = ("2个发卡机的卡箱已空...!");
                    return false;
                }
                Utility.WriteLog("2号发卡机发卡");
            }
            return true;
        }

        public bool CheckRecycleBox(ref string recycle, ref string errbox)
        {
            RESULTDATA RD = new RESULTDATA();
            //发卡机状态查询
            byte[] bSend = new byte[] { 0x41, 0x39 };
            int ret = F8_Execute(ghDevice, bSend, 2, 0, ref RD);
            if (ret != 0)
            {
                F8_GetResultDesc(ret);
                return false;
            }
            if (RD.RDT[0] == '0')
                recycle = ("回收箱：无回收箱");
            else if (RD.RDT[0] == '1')
                recycle = ("回收箱：有卡");
            else if (RD.RDT[0] == '2')
                recycle = ("回收箱：满");
            else if (RD.RDT[0] == '3')
                recycle = ("回收箱：空");
            else if (RD.RDT[0] == '4')
                recycle = ("回收箱：传感器异常");

            if (RD.RDT[1] == '0')
                errbox += ("废卡箱：无废卡箱");
            else if (RD.RDT[1] == '1')
                errbox += ("废卡箱：有卡");
            else if (RD.RDT[1] == '2')
                errbox += ("废收箱：满");
            else if (RD.RDT[1] == '3')
                errbox += ("废卡箱：空");
            else if (RD.RDT[1] == '4')
                errbox = ("废卡箱：传感器异常");

            return true;
        }
        public void getSlotSum(ref int nSlotHaveCards, ref int nSlotNoCards)
        {
            int ret;
            int nIndex = 0;
            nSlotHaveCards = 0;
            nSlotNoCards = 0;

            RESULTDATA rd = new RESULTDATA();
            // 卡盘状态查询
            byte[] bSend = new byte[] { 0x41, 0x32 };
            ret = F8_Execute(ghDevice, bSend, 2, 0, ref rd);
            if (ret != 0)
            {
                F8_GetResultDesc(ret);
                return;
            }

            for (int i = 0; i < 2; i++)
            {

                nIndex = 0;
                for (int j = 68 * i; j < 68 * (i + 1); j++)
                {

                    if (j < rd.RDTLength)
                    {

                        int iValues = 0; int num = nIndex;
                        for (int k = 0; k < 8; k++)
                        {
                            iValues = (int)Math.Pow(2.0, k);
                            if (((rd.RDT[j] & iValues) >> k) != 0)
                            {
                                if (num < 540)
                                    nSlotHaveCards++;
                            }
                            else
                            {
                                if (num < 540)
                                    nSlotNoCards++;
                            }
                            num++;
                        }

                        nIndex += 8;
                    }
                }
            }
        }

        /// <summary>
        /// 检查指定卡盘的卡槽是否有卡
        /// </summary>
        /// <param name="nBlock">卡盘块号</param>
        /// <param name="nSlot">卡槽卡位</param>
        /// <returns>true, 有卡，false, 无卡</returns>
        public bool CheckSlot(int nBlock, int nSlot, int TP_Card)
        {
            if (nSlot > 540 || nSlot < 0) return false;

            RESULTDATA RD = new RESULTDATA();
            //查询卡槽状态
            byte[] bSend = new byte[] { 0x41, 0x32 };
            int ret = F8_Execute(ghDevice, bSend, 2, 0, ref RD);
            if (ret != 0)
            {
                //Utility.WriteLog(ret.ToString("0x{ 0:X8}"));
                if (ret == 0x4E0006)
                {
                    //Utility.WriteLog(TP_PreMadeCard.ToString());
                    if (TP_Card == TP_PreMadeCard)
                        MDATASQL.DeletePreCard(nSlot, nBlock);
                    else if (TP_Card == TP_SiCard)
                        MDATASQL.DeleteSiCard(nSlot, nBlock);
                }
                F8_GetResultDesc(ret);
                return false;
            }
            int iValues, iCount, j;
            if (nBlock == 0)
            {
                for (iCount = 0; iCount < 2; iCount++)
                {
                    j = (68 * iCount) + nSlot / 8;
                    for (int iparam = 0; iparam < 8; iparam++)
                    {
                        iValues = (int)Math.Pow(2.0, iparam);
                        if (((RD.RDT[j] & iValues) >> iparam) != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                iCount = 1;
                j = (68 * iCount) + nSlot / 8;
                for (int iparam = 0; iparam < 8; iparam++)
                {
                    iValues = (int)Math.Pow(2.0, iparam);
                    if (((RD.RDT[j] & iValues) >> iparam) != 0)
                    {
                        return true;
                    }
                }
            }
            strError = "卡位无卡...";

            // 从数据库移除数据
            if (TP_Card == TP_PreMadeCard)
                MDATASQL.DeletePreCard(nSlot, nBlock);
            else if (TP_Card == TP_SiCard)
                MDATASQL.DeleteSiCard(nSlot, nBlock);

            return false;
        }

        /// <summary>
        /// 查询卡盘状态  卡盘是否有空卡位，返回空卡位的位置  卡盘是否有卡，返回有卡的位置
        /// </summary>
        /// <param name="nblock">卡盘块号</param>
        /// <param name="hascard">false 查询卡盘是否有空卡位   true 查询卡盘是否有卡</param>
        /// <returns></returns>
        public int CheckPan(int nblock, bool hascard)
        {
            int SlotLen, nSlot;
            //int[] SlotBuf = new int[1024];
            RESULTDATA RD = new RESULTDATA();
            //查询卡槽状态
            byte[] bSend = new byte[] { 0x41, 0x32 };
            int ret = F8_Execute(ghDevice, bSend, 2, 0, ref RD);
            if (ret != 0)
            {
                F8_GetResultDesc(ret);
                return -1;
            }
            SlotLen = GetSlotBuff(RD, 0, 540, nblock, hascard);
            if (hascard)
            {
                if (SlotLen > 0)
                {
                    nSlot = iSlotBuffer[0][0];
                    if (nblock == 1) nSlot = nSlot - 540;
                    return nSlot;
                }
                else
                {
                    strError = ("卡槽已空。");
                    return -1;
                }
            }
            else
            {
                if (SlotLen > 0)
                {
                    nSlot = iSlotBuffer[1][0];
                    if (nblock == 1) nSlot = nSlot - 540;
                    return nSlot;
                }
                else
                {
                    strError = ("卡槽已满。");
                    return -1;
                }
            }
        }

        int GetSlotBuff(RESULTDATA rdata, int BeginNum, int EndNum, int nBlock, bool nflg)
        {
            int iValues;
            int CardSlot = 0;
            int nSub1 = 0, nSub2 = 0;
            //string str;
            int nSlotEndNum = EndNum / 8;
            for (int iCount = 0; iCount < 2; iCount++)
            {
                CardSlot = (BeginNum / 8) * 8;
                if (nBlock == 1) iCount = 1;

                for (int j = (68 * iCount) + BeginNum / 8; j <= (68 * iCount) + nSlotEndNum + 1; j++) // 
                {
                    for (int iparam = 0; iparam < 8; iparam++)
                    {
                        iValues = (int)Math.Pow(2.0, iparam);
                        if (((rdata.RDT[j] & iValues) >> iparam) != 0)
                        {
                            if (CardSlot < EndNum && CardSlot >= BeginNum)
                            {
                                if (iCount == 1) iSlotBuffer[0][nSub1] = CardSlot + 540;
                                else
                                    iSlotBuffer[0][nSub1] = CardSlot;
                                nSub1++;
                            }
                        }
                        else
                        {

                            if (CardSlot < EndNum && CardSlot >= BeginNum)
                            {

                                if (iCount == 1) iSlotBuffer[1][nSub2] = CardSlot + 540;
                                else
                                    iSlotBuffer[1][nSub2] = CardSlot;
                                nSub2++;
                            }
                        }
                        if (CardSlot < EndNum) CardSlot++;
                    }
                }
                if (nBlock != 2 || rdata.RDTLength <= 68) break; //
            }

            if (nflg)
                return nSub1;
            else
                return nSub2;
        }
        void F8_GetResultDesc(int ret)
        {
            string strDesc;
            if (ret <= 0)
            {
                GetBaseResultDesc(ret);
                return;
            }

            switch (ret)
            {
                case 0x4E00FF:
                    strDesc = ("命令参数错误。");
                    break;
                case 0x4E0001:
                    strDesc = ("码盘孔堵塞或被损坏。");
                    break;

                case 0x4E0004:
                    strDesc = ("卡槽堵塞。");
                    break;
                case 0x4E0005:
                    strDesc = ("位置传感器未检测到轨道孔。");
                    break;
                case 0x4E0006:
                    strDesc = ("卡槽内无卡。");
                    break;
                case 0x4E0007:
                    strDesc = ("卡槽内有卡。");
                    break;
                case 0x4E0008:
                    strDesc = ("卡槽块地址或地址越界。");
                    break;
                case 0x4E0009:
                    strDesc = ("电压不在范围。");
                    break;
                case 0x4E000a:
                    strDesc = ("卡盘寻找零位失败。");
                    break;
                case 0x4E000c:
                    strDesc = ("圆盘外顶卡电机运动堵塞。");
                    break;
                case 0x4E000d:
                    strDesc = ("垂直通道堵塞。");
                    break;
                case 0x4E000e:
                    strDesc = ("机器有故障，需要复位。");
                    break;
                case 0x4E000f:
                    strDesc = ("传输装置通道无卡。");
                    break;


                case 0x4E0012:
                    strDesc = ("回收箱满。");
                    break;
                case 0x4E0013:
                    strDesc = ("传输小车通道无卡。");
                    break;
                case 0x4E0015:
                    strDesc = ("传输小车通道有卡。");
                    break;

                case 0x4E0017:
                    strDesc = ("无回收箱。");
                    break;
                case 0x4E0019:
                    strDesc = ("传输小车将卡片移入中间位置失败。");
                    break;

                case 0x4E0024:
                    strDesc = ("传输小车出卡失败。");
                    break;
                case 0x4E0025:
                    strDesc = ("传输小车进卡失败。");
                    break;
                case 0x4E0026:
                    strDesc = ("传输小车推卡电机通道异常。");
                    break;
                case 0x4E0027:
                    strDesc = ("传输小车进卡到卡槽失败。");
                    break;
                case 0x4E0028:
                    strDesc = ("左右通道堵塞。");
                    break;
                case 0x4E002b:
                    strDesc = ("回收通道堵塞。");
                    break;

                case 0x4E002c:
                    strDesc = ("传输小车进卡到凸字模块失败。");
                    break;

                case 0x4E002d:
                    strDesc = ("传输小车进卡到彩印模块失败。");
                    break;
                case 0x4E002e:
                    strDesc = ("传输小车进卡到读卡器失败。");
                    break;
                case 0x4E002f:
                    strDesc = ("传输小车进卡到烫金模块失败。");
                    break;

                case 0x4E0030:
                    strDesc = ("发卡机钩子运动模块堵塞。");
                    break;
                case 0x4E0031:
                    strDesc = ("发卡机通道堵塞。");
                    break;
                case 0x4E0032:
                    strDesc = ("发卡机传感器故障。");
                    break;
                case 0x4E0033:
                    strDesc = ("发卡机脱钩出错。");
                    break;
                case 0x4E0034:
                    strDesc = ("发卡机通道中有卡。");
                    break;
                case 0x4E0035:
                    strDesc = ("发卡机卡箱空。");
                    break;
                case 0x4E0036:
                    strDesc = ("发卡机卡箱未就绪。");
                    break;
                case 0x4E0037:
                    strDesc = ("发卡机回收箱满。");
                    break;
                case 0x4E0038:
                    strDesc = ("发卡机通道中没有卡。");
                    break;
                case 0x4E0039:
                    strDesc = ("发卡机回收推卡模块故障。");
                    break;
                case 0x4E003a:
                    strDesc = ("发卡机回收凸轮模块故障。");
                    break;
                case 0x4E003b:
                    strDesc = ("发卡机回收箱未就绪。");
                    break;
                case 0x4E003c:
                    strDesc = ("发卡机电压不在范围。");
                    break;
                case 0x4E003d:
                    strDesc = ("发卡机通讯失败。");
                    break;
                case 0x4E003E:
                    strDesc = ("与烫银模块通讯失败。");
                    break;

                case 0x4E0050:
                    strDesc = ("读卡器通讯失败。");
                    break;
                case 0x4E0051:
                    strDesc = ("读卡器内没有卡。");
                    break;
                case 0x4E0052:
                    strDesc = ("读卡器内有卡。");
                    break;
                case 0x4E0053:
                    strDesc = ("读卡器故障。");
                    break;

                case 0x4E0090:
                    strDesc = ("读卡器进卡到垂直小车失败。");
                    break;

                case 0x4E00B0:
                    strDesc = ("读卡器未定义的命令。");
                    break;
                case 0x4E00B1:
                    strDesc = ("读卡器未定义的命令参数。");
                    break;

                case 0x4E00B2:
                    strDesc = ("读卡器命令不能执行。");
                    break;
                case 0x4E00B3:
                    strDesc = ("读卡器命令数据错误。");
                    break;
                case 0x4E00B4:
                    strDesc = ("读卡器卡片堵塞。");
                    break;
                case 0x4E00B5:
                    strDesc = ("读卡器闸门关闭失败。");
                    break;
                case 0x4E00B6:
                    strDesc = ("读卡器传感器错误。");
                    break;
                case 0x4E00B7:
                    strDesc = ("读卡器不规则的卡片长度（长卡）。");
                    break;
                case 0x4E00B8:
                    strDesc = ("读卡器不规则的卡片长度（短卡）。");
                    break;
                case 0x4E00B9:
                    strDesc = ("读卡器F-ROM error/EEPROM error。");
                    break;
                case 0x4E00BA:
                    strDesc = ("读卡器卡片禁止移动。");
                    break;
                case 0x4E00BB:
                    strDesc = ("读卡器回收堵塞。");
                    break;
                case 0x4E00BC:
                    strDesc = ("读卡器卡片没有从后端进入。");
                    break;
                case 0x4E00BD:
                    strDesc = ("读卡器掉电。");
                    break;
                case 0x4E00BE:
                    strDesc = ("读卡器卡片回收时被取走。");
                    break;
                case 0x4E00BF:
                    strDesc = ("读卡器ICRW禁止弹卡。");
                    break;


                case 0x4E00C0:
                    strDesc = ("读卡器弹卡取出超时。");
                    break;
                case 0x4E00C1:
                    strDesc = ("读卡器回收计数溢出。");
                    break;
                case 0x4E00C2:
                    strDesc = ("读卡器电机错误。");
                    break;
                case 0x4E00C3:
                    strDesc = ("读卡器没有接收到复位命令。");
                    break;
                case 0x4E00EE:
                    strDesc = ("设备未连接。");
                    break;
                //
                case 0xff:
                    strDesc = ("卡盘检测。");
                    break;

                //R2射频模块错误
                case 0x5E0000:
                    strDesc = ("未定义命令。");
                    break;
                case 0x5E0001:
                    strDesc = ("无效的命令参数。");
                    break;
                case 0x5E0002:
                    strDesc = ("命令不能在当前的状态下执行。");
                    break;
                case 0x5E0004:
                    strDesc = ("无效的命令参数。");
                    break;
                case 0x5E0045:
                    strDesc = ("命令执行失败。");
                    break;
                case 0x5E0030:
                    strDesc = ("寻卡失败。");
                    break;
                case 0x5E0031:
                    strDesc = ("扇区未认证。");
                    break;
                case 0x5E0032:
                    strDesc = ("卡序列号错误。");
                    break;
                case 0x5E0033:
                    strDesc = ("认证密码错误。");
                    break;
                case 0x5E0034:
                    strDesc = ("读/写块错误。");
                    break;
                case 0x5E0035:
                    strDesc = ("增值或减值溢出。");
                    break;


                case 0x4E3230:
                    strDesc = ("PARITY 错误。");
                    break;
                case 0x4E3234:
                    strDesc = ("空白轨。");
                    break;
                case 0x4E3236:
                    strDesc = ("SS错误。");
                    break;
                case 0x4E3237:
                    strDesc = ("ES错误。");
                    break;
                case 0x4E3238:
                    strDesc = ("LRC错误。");
                    break;

                default:
                    strDesc = ("其它错误");
                    break;
            }
            strError = ERRHEAD + strDesc;
        }

        void GetBaseResultDesc(int ret)
        {
            string strDesc;
            switch (ret)
            {
                case 0:
                    strDesc = ("操作成功");
                    break;

                case -1:
                    strDesc = ("通信端口不可用");
                    break;

                case -2:
                    strDesc = ("设备未就绪。可能的原因:\r\n1 设备未上电或未连接计算机\r\n2 通信端口或设备故障");
                    break;

                case -10:
                    strDesc = ("未知错误");
                    break;

                case -13:
                    strDesc = ("接数数据异常");
                    break;

                case -20:
                    strDesc = ("无效句柄");
                    break;

                case -21:
                    strDesc = ("无效参数");
                    break;

                case -22:
                    strDesc = ("空指针异常");
                    break;

                case -23:
                    strDesc = ("缓存太小");
                    break;
                case -27:

                    strDesc = ("无效的数据格式");
                    break;

                case -36:
                    strDesc = ("内存溢出");
                    break;

                case -37:
                    strDesc = ("命令执行失败");
                    break;
                default:
                    strDesc = ("");
                    break;
            }
            strError = ERRHEAD + strDesc;
        }

        string recv, sw1sw2;
        public int AddBankData()
        {
            MDATA mdata = MDATA.getInstance();
            string bankno = string.Empty;
            int nRet = GetBankNum(ref bankno);
            if (nRet == 0)
            {
                mdata.BankNO = bankno;
                mdata.BankCode = MDATASQL.getBankCode(mdata.BankNO);
            }
            else
            {
                mdata.BankNO = "sitest" + DateTime.Now.ToString("yyyyMMddHHmmss");
                mdata.BankCode = MDATASQL.getBankCode(mdata.BankNO);
            }
            return nRet;
        }
        public int GetBankNum(ref string bankno)
        {
            int nRet = R2CardReset();
            if (nRet != 0)
            {
                strError = "激活卡片失败: " + nRet.ToString();
                return (int)GetBankErr.notsicard;
            }

            string csCommand = "";

            if (!ApduSend("00A404000E315041592E5359532E4444463031")) return (int)GetBankErr.notsicard;

            if (!ApduSend("00A4040007A0000003330101")) return (int)GetBankErr.notsicard;

            int ipos = 0;
            if (!ApduSend("00B2010C00")) return (int)GetBankErr.notsicard; //00B2011400
            if (sw1sw2.Substring(0, 2) == "6C")
            {
                csCommand = "00B2010C" + sw1sw2.Substring(2, 2);
                if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                ipos = recv.IndexOf("5713", 0);
                if (ipos == -1)
                {
                    if (!ApduSend("00B2020C00")) return (int)GetBankErr.notsicard; //00B2011400
                    if (sw1sw2.Substring(0, 2) == "6C")
                    {
                        csCommand = "00B2020C" + sw1sw2.Substring(2, 2);
                        if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                        ipos = recv.IndexOf("5713", 0);
                        if (ipos == -1)
                        {
                            ipos = recv.IndexOf("570F", 0);
                            if (ipos == -1)
                            {
                                strError = recv + "：570F";
                                Utility.WriteLog(strError);
                                return (int)GetBankErr.notsicard;
                            }
                        }
                    }
                }
            }

            int nPos = recv.IndexOf("D", 8);
            if (nPos <= 0)
            {
                csCommand = "00B2010C" + sw1sw2.Substring(2, 2);
                if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                ipos = recv.IndexOf("5713", 0);
                if (ipos == -1)
                {
                    if (!ApduSend("00B2020C00")) return (int)GetBankErr.notsicard; //00B2011400
                    if (sw1sw2.Substring(0, 2) == "6C")
                    {
                        csCommand = "00B2020C" + sw1sw2.Substring(2, 2);
                        if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                        ipos = recv.IndexOf("5713", 0);
                        if (ipos == -1)
                        {
                            ipos = recv.IndexOf("570F", 0);
                            if (ipos == -1)
                            {
                                strError = recv + "：570F";
                                Utility.WriteLog(strError);
                                return (int)GetBankErr.notsicard;
                            }
                        }
                    }
                }
                nPos = recv.IndexOf("D", 8);
                if (nPos <= 0)
                {
                    strError = recv + "：未发现d";
                    Utility.WriteLog(strError);
                    return (int)GetBankErr.notsicard;
                }
            }

            string csCreditCardID = "";
            int Pos57 = recv.IndexOf("571");
            if (Pos57 >= 0)
            {
                csCreditCardID = recv.Substring(Pos57 + 4, nPos - Pos57 - 4);  // 银行卡号
            }
            else if (nPos < 22)
            {
                csCreditCardID = recv.Substring(0, nPos);
            }
            else
            {
                Utility.WriteLog(recv);
                return (int)GetBankErr.notsicard;
            }
            Utility.WriteLog(recv);
            Utility.WriteLog(bankno);
            bankno = csCreditCardID;
            return 0;
        }

        bool ApduSend(string cmd)
        {
            RESULTDATA RD = new RESULTDATA();
            string CDT1 = "3565";
            int bCmdLenth = cmd.Length / 2;
            string BodyData1 = ((bCmdLenth >> 8) & 0xff).ToString("x2");
            string BodyData2 = ((bCmdLenth) & 0xff).ToString("x2");
            string BodyData = BodyData1 + BodyData2 + cmd;
            string CDT2 = CDT1 + BodyData;
            byte[] bCDT = strToToHexByte(CDT2);
            int lResult = R2_Execute(ghDevice, bCDT, bCDT.Length, 0, ref RD, 0, 3, true);
            if (lResult != 0)
            {
                strError = "指令发送失败: " + cmd;
                Utility.WriteLog(strError);
                return false;
            }
            else
            {
                string strResp = ByteToHexStr(RD.RDT, RD.RDTLength);
                recv = strResp.Substring(0, strResp.Length - 4);
                sw1sw2 = strResp.Substring(strResp.Length - 4, 4);
                if (recv.Length > 4)
                {
                    recv = recv.Substring(4);
                }
                if (sw1sw2.Substring(0, 2) == "61" || sw1sw2 == "9000") return true;
                Utility.WriteLog(cmd);
                Utility.WriteLog(strResp);
                return false;
            }
        }

        int R2CardReset()
        {
            RESULTDATA rd = new RESULTDATA();
            byte[] bCDT = { 0x35, 0x60 };
            int ret = R2_Execute(ghDevice, bCDT, bCDT.Length, 0, ref rd, 0, 1, true);
            if (ret == 0)
            {
                string strATR = ByteToHexStr(rd.RDT, rd.RDTLength);
                return 0;
            }
            return (int)GetBankErr.resetfail;
        }

        private byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = hexString.Insert(hexString.Length - 1, 0.ToString());
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        string ByteToHexStr(byte[] bytes, int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

    }

    /// <summary>
    /// 三代卡读写接口
    /// </summary>
    public class SiCard
    {
        public static int PORT_RC = 5;
        public static int PORT_FG = 4;

        public static int TP_CONTACT = 1;
        public static int TP_CONTACTLESS = 2;

        [DllImport("SiCardDriver.dll")]
        public static extern int Print_ReadCard(
            int port, int iType, StringBuilder pOutputInfo, StringBuilder pErrMsg
           );
        [DllImport("SiCardDriver.dll")]
        public static extern int Print_WriteCard(
             int port, int iType, string pOutputInfo, StringBuilder pErrMsg
            );
        [DllImport("SiCardDriver.dll")]
        public static extern int iPChangePIN(
             int port, int iType, string szOldPasswd, string szNewPasswd, StringBuilder pErrMsg
            );

        //重置密码操作
        [DllImport("SiCardDriver.dll")]
        public static extern int iReloadPIN_HSM(int port, int iType, string sfzh, string xm, string szNewPasswd, StringBuilder pErrMsg);

        [DllImport("SiCardDriver.dll", EntryPoint = "ICCOpenDevice")]
        public static extern int ICCOpenDevice(int port, StringBuilder pErrMsg);

        [DllImport("SiCardDriver.dll", EntryPoint = "ICCCloseDevice")]
        public static extern int ICCCloseDevice();

        [DllImport("SiCardDriver.dll", EntryPoint = "iGetReaderInfo")]
        public static extern int iGetReaderInfo(StringBuilder psamNo, StringBuilder devCSN);

        [DllImport("SiCardDriver.dll", EntryPoint = "ICCPowerOn")]
        public static extern int ICCPowerOn(int iType, byte ICC_Slot_No, byte[] ATR);

        [DllImport("SiCardDriver.dll", EntryPoint = "ICCApplication")]
        public static extern int ICCApplication(int iType, byte ICC_Slot_No, int Lenth_of_Command_APDU, byte[] Command_APDU, byte[] Response_APDU);

        [DllImport("SiCardDriver.dll", EntryPoint = "iReadIDCard")]
        public static extern int iReadIDCard(int port, string bmp, StringBuilder cardInfo, StringBuilder err);

        static string recv, sw1sw2;
        public static string error;

        public static string idcardinfo;
        public static string userName = "";
        public static string userID = "";
        public static string validTime = "";
        public static string Sex = "";
        public static string Nation = "";
        public static string Addr = "";
        public static string SexShow = "";
        public static string NationShow = "";

        public static string siOutInfo { get; set; }

        public static int ReadSiCard(int nPort)
        {
            siOutInfo = "";
            StringBuilder pOutputInfo = new StringBuilder(600);
            StringBuilder pErrMsg = new StringBuilder(600);
            int ret = Print_ReadCard(nPort, TP_CONTACT, pOutputInfo, pErrMsg);
            if (ret != 0)
            {
                Utility.WriteLog("卡片读取返回值:" + ret.ToString());
                error = pErrMsg.ToString();
                return ret;
            }
            siOutInfo = pOutputInfo.ToString();
            return 0;
        }
        public static int PersonalizationTest(int nPort)
        {
            StringBuilder pOutputInfo = new StringBuilder(600);
            StringBuilder pErrMsg = new StringBuilder(600);
            int ret = Print_ReadCard(nPort, TP_CONTACT, pOutputInfo, pErrMsg);
            if (ret != 0)
            {
                Utility.WriteLog("卡片读取返回值:" + ret.ToString());
                error = pErrMsg.ToString();
                return ret;
            }
            //Utility.WriteLog(pOutputInfo.ToString());

            string[] infos = pOutputInfo.ToString().Split('|');
            string cardverson = infos[8];
            string cardIdentify = infos[3];
            string cardReset = infos[7];
            string cardagent = infos[7].Substring(18, 6);
            string CA = infos[10];
            string makeDate = DateTime.Now.ToString("yyyyMMdd");
            string valid = DateTime.Now.AddYears(10).ToString("yyyyMMdd");

            StringBuilder pErrMsg2 = new StringBuilder(600);
            string pWrinteInfo = "";
            if (cardverson == "2.00")
            {
                pWrinteInfo = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|2.00|CA|",
                    "410002201005061326", "测试", "1", "01", "20100506", makeDate, valid, "A01234567", cardReset, cardIdentify);
            }
            else
            {
                pWrinteInfo = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|3.00|{10}|",
                    "410002201005061326", "测试", "1", "01", "20100506", makeDate, valid, "A01234567", cardReset, cardIdentify, CA);
            }
            ret = SiCard.Print_WriteCard(nPort, SiCard.TP_CONTACT, pWrinteInfo, pErrMsg2);
            if (ret != 0)
            {
                Utility.WriteLog("个人化返回值:" + ret.ToString());
                error = pErrMsg2.ToString();
                return ret;
            }

            return 0;
        }
        public static int CheckICReader(int nPort)
        {
            StringBuilder sb = new StringBuilder(518);
            int iRet = ICCOpenDevice(nPort, sb);
            if (iRet != 0)
            {
                error = "读卡器打开失败";
                //Utility.WriteLog(error);
                return -1;
            }
            int iType = 1;
            byte ICC_Slot_No = 0x01;
            byte[] sb1 = new byte[30];
            //打开读卡器
            iRet = ICCPowerOn(iType, ICC_Slot_No, sb1);
            if (iRet <= 0)
            {
                error = "卡片上电失败";
                //Utility.WriteLog(error);
                return -2;
            }
            return 0;
        }
        public static bool ReadIDCard()
        {
            StringBuilder cardinfo = new StringBuilder(600);
            StringBuilder err = new StringBuilder(600);
            int nRet = iReadIDCard(Global.GetInstance().nPortPin, "", cardinfo, err);
            if (nRet == 0)
            {
                idcardinfo = cardinfo.ToString();
                string[] infos = cardinfo.ToString().Split('|');
                userName = infos[0];
                SexShow = infos[1];
                NationShow = infos[2];
                Nation = GetNationNum(NationShow);
                userID = infos[5];
                validTime = infos[8].Replace(" ", "");
                Addr = infos[4];

                if (SexShow == "男") Sex = "1";
                else if (SexShow == "女") Sex = "2";

                return true;
            }
            else
            {
                error = nRet.ToString() + err.ToString();
                return false;
            }
        }

        public static int ReadSSSE()
        {
            StringBuilder sb = new StringBuilder(518);
            int iRet = ICCOpenDevice(Global.GetInstance().nPortPin, sb);
            if (iRet != 0) { Utility.WriteLog("读卡器打开失败"); return -1; }
            int iType = 1;
            byte ICC_Slot_No = 0x01;
            byte[] sb1 = new byte[30];
            //打开读卡器
            iRet = ICCPowerOn(iType, ICC_Slot_No, sb1);
            if (iRet <= 0) { Utility.WriteLog("上电失败"); return -2; }

            if (ApduSend("00A404000E315041592E5359532E4444463031")) return 0;
            else return -3;
        }
        public static int ReadCreditCardID(int nPort, ref string BankNO)
        {
            StringBuilder err = new StringBuilder(600);
            int i = ICCOpenDevice(nPort, err);
            //int i = ICCOpenDevice(PORT_FG, err);
            if (i != 0) { error = "读卡器打开失败"; Utility.WriteLog(error); return -1; }
            int iType = 1;
            byte ICC_Slot_No = 0x01;
            byte[] sb1 = new byte[30];
            //打开读卡器
            i = ICCPowerOn(iType, ICC_Slot_No, sb1);
            if (i <= 0) { error = "上电失败"; Utility.WriteLog(error); return -2; }

            string csCommand = "";

            if (!ApduSend("00A404000E315041592E5359532E4444463031")) return -3;

            if (!ApduSend("00A4040007A0000003330101")) return -3;

            int ipos = 0;
            if (!ApduSend("00B2010C00")) return -4;//00B2011400
            if (sw1sw2.Substring(0, 2) == "6C")
            {
                csCommand = "00B2010C" + sw1sw2.Substring(2, 2);
                if (!ApduSend(csCommand)) return -5;
                ipos = recv.IndexOf("5713", 0);
                if (ipos == -1)
                {
                    if (!ApduSend("00B2020C00")) return -6;//00B2011400
                    if (sw1sw2.Substring(0, 2) == "6C")
                    {
                        csCommand = "00B2020C" + sw1sw2.Substring(2, 2);
                        if (!ApduSend(csCommand)) return -7;
                        ipos = recv.IndexOf("5713", 0);
                        if (ipos == -1)
                        {
                            ipos = recv.IndexOf("570F", 0);
                            if (ipos == -1)
                            {
                                error = recv + "：570F";
                                Utility.WriteLog(error);
                                return -8;
                            }
                        }
                    }
                }
            }

            int nPos = recv.IndexOf("D", 8);
            if (nPos <= 0)
            {
                csCommand = "00B2010C" + sw1sw2.Substring(2, 2);
                if (!ApduSend(csCommand)) return -9;
                ipos = recv.IndexOf("5713", 0);
                if (ipos == -1)
                {
                    if (!ApduSend("00B2020C00")) return -10;//00B2011400
                    if (sw1sw2.Substring(0, 2) == "6C")
                    {
                        csCommand = "00B2020C" + sw1sw2.Substring(2, 2);
                        if (!ApduSend(csCommand)) return -11;
                        ipos = recv.IndexOf("5713", 0);
                        if (ipos == -1)
                        {
                            ipos = recv.IndexOf("570F", 0);
                            if (ipos == -1)
                            {
                                error = recv + "：570F";
                                Utility.WriteLog(error);
                                return -12;
                            }
                        }
                    }
                }
                nPos = recv.IndexOf("D", 8);
                if (nPos <= 0)
                {
                    error = recv + "：未发现d";
                    Utility.WriteLog(error); return -13;
                }
            }
            int Pos57 = recv.IndexOf("571");
            string csCreditCardID = recv.Substring(Pos57 + 4, nPos - Pos57 - 4);  // 银行卡号
            BankNO = csCreditCardID;
            return 0;
        }
        private static bool ApduSend(string cmd)
        {
            cmd = cmd.Replace(" ", "");
            byte[] by1 = strToToHexByte(cmd);
            byte[] by2 = new byte[255];
            //上电
            int nRt = ICCApplication(1, 0x01, by1.Length, by1, by2);
            if (nRt < 2)
            {
                error = "指令发送失败: " + cmd;
                Utility.WriteLog(error);
                return false;
            }
            else
            {
                string strResp = ByteToHexStr(by2, nRt);
                //Utility.WriteLog(cmd);
                //Utility.WriteLog(strResp);
                recv = strResp.Substring(0, strResp.Length - 4);
                sw1sw2 = strResp.Substring(strResp.Length - 4, 4);
                if (sw1sw2.Substring(0, 2) == "61" || sw1sw2 == "9000") return true;
                //MessageBox.Show("命令执行成功：" + strResp);
                return false;
            }
        }

        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = hexString.Insert(hexString.Length - 1, 0.ToString());
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        private static string ByteToHexStr(byte[] bytes, int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public static string GetNationNum(string strNationName)
        {
            string strNationNum = "";
            if (strNationName.IndexOf("汉") >= 0)
                strNationNum = "01";

            else if (strNationName.IndexOf("蒙古") >= 0)
                strNationNum = "02";

            else if (strNationName.IndexOf("回") >= 0)
                strNationNum = "03";

            else if (strNationName.IndexOf("藏") >= 0)
                strNationNum = "04";

            else if (strNationName.IndexOf("维吾尔") >= 0)
                strNationNum = "05";

            else if (strNationName.IndexOf("苗") >= 0)
                strNationNum = "06";

            else if (strNationName.IndexOf("彝") >= 0)
                strNationNum = "07";

            else if (strNationName.IndexOf("壮") >= 0)
                strNationNum = "08";

            else if (strNationName.IndexOf("布依") >= 0)
                strNationNum = "09";

            else if (strNationName.IndexOf("朝鲜") >= 0)
                strNationNum = "10";

            else if (strNationName.IndexOf("满") >= 0)
                strNationNum = "11";

            else if (strNationName.IndexOf("侗") >= 0)
                strNationNum = "12";

            else if (strNationName.IndexOf("瑶") >= 0)
                strNationNum = "13";

            else if (strNationName.IndexOf("白") >= 0)
                strNationNum = "14";

            else if (strNationName.IndexOf("土家") >= 0)
                strNationNum = "15";

            else if (strNationName.IndexOf("哈尼") >= 0)
                strNationNum = "16";

            else if (strNationName.IndexOf("哈萨克") >= 0)
                strNationNum = "17";

            else if (strNationName.IndexOf("傣") >= 0)
                strNationNum = "18";

            else if (strNationName.IndexOf("黎") >= 0)
                strNationNum = "19";

            else if (strNationName.IndexOf("傈僳") >= 0)
                strNationNum = "20";

            else if (strNationName.IndexOf("佤") >= 0)
                strNationNum = "21";

            else if (strNationName.IndexOf("畲") >= 0)
                strNationNum = "22";

            else if (strNationName.IndexOf("高山") >= 0)
                strNationNum = "23";

            else if (strNationName.IndexOf("拉祜") >= 0)
                strNationNum = "24";

            else if (strNationName.IndexOf("水") >= 0)
                strNationNum = "25";

            else if (strNationName.IndexOf("东乡") >= 0)
                strNationNum = "26";

            else if (strNationName.IndexOf("纳西") >= 0)
                strNationNum = "27";

            else if (strNationName.IndexOf("景颇") >= 0)
                strNationNum = "28";

            else if (strNationName.IndexOf("克尔克孜") >= 0)
                strNationNum = "29";

            else if (strNationName.IndexOf("土") >= 0)
                strNationNum = "30";

            else if (strNationName.IndexOf("达斡尔") >= 0)
                strNationNum = "31";

            else if (strNationName.IndexOf("仫佬") >= 0)
                strNationNum = "32";

            else if (strNationName.IndexOf("羌") >= 0)
                strNationNum = "33";

            else if (strNationName.IndexOf("布朗") >= 0)
                strNationNum = "34";

            else if (strNationName.IndexOf("撒拉") >= 0)
                strNationNum = "35";

            else if (strNationName.IndexOf("毛南") >= 0)
                strNationNum = "36";

            else if (strNationName.IndexOf("仡佬") >= 0)
                strNationNum = "37";

            else if (strNationName.IndexOf("锡伯") >= 0)
                strNationNum = "38";

            else if (strNationName.IndexOf("阿昌") >= 0)
                strNationNum = "39";

            else if (strNationName.IndexOf("普米") >= 0)
                strNationNum = "40";

            else if (strNationName.IndexOf("塔吉克") >= 0)
                strNationNum = "41";

            else if (strNationName.IndexOf("怒") >= 0)
                strNationNum = "42";

            else if (strNationName.IndexOf("乌兹别克") >= 0)
                strNationNum = "43";

            else if (strNationName.IndexOf("俄罗斯") >= 0)
                strNationNum = "44";

            else if (strNationName.IndexOf("鄂温克") >= 0)
                strNationNum = "45";

            else if (strNationName.IndexOf("德昂") >= 0)
                strNationNum = "46";

            else if (strNationName.IndexOf("保安") >= 0)
                strNationNum = "47";

            else if (strNationName.IndexOf("裕固") >= 0)
                strNationNum = "48";

            else if (strNationName.IndexOf("京") >= 0)
                strNationNum = "49";

            else if (strNationName.IndexOf("塔塔尔") >= 0)
                strNationNum = "50";

            else if (strNationName.IndexOf("独龙") >= 0)
                strNationNum = "51";

            else if (strNationName.IndexOf("鄂伦春") >= 0)
                strNationNum = "52";

            else if (strNationName.IndexOf("赫哲") >= 0)
                strNationNum = "53";

            else if (strNationName.IndexOf("珞巴") >= 0)
                strNationNum = "54";

            else if (strNationName.IndexOf("门巴") >= 0)
                strNationNum = "55";

            else if (strNationName.IndexOf("基诺") >= 0)
                strNationNum = "56";

            else if (strNationName.IndexOf("外国血统中国籍人士") >= 0)
                strNationNum = "98";
            else
                strNationNum = "00";

            return strNationNum;
        }
    }

    public class R2SAPI
    {
        private const string dllName = "R2SAPI.dll";

        [DllImport(dllName)]
        public static extern int R2_Connect(int dwPortNumber, int dwBaudRate, ref IntPtr pHandle);

        [DllImport(dllName)]
        public static extern int R2_Disconnect(IntPtr pHandle);

        // 接触式CPU

        [DllImport(dllName)]
        public static extern int R2_CpuPowerOn(IntPtr pHandle);

        [DllImport(dllName)]
        public static extern int R2_CpuPowerOff(IntPtr pHandle);

        [DllImport(dllName)]
        public static extern int R2_CpuColdReset(IntPtr pHandle, StringBuilder pbATRBuff, ref int pcbATRLength);

        [DllImport(dllName)]
        public static extern int R2_CpuWarmReset(IntPtr pHandle, StringBuilder pbATRBuff, ref int pcbATRLength);

        [DllImport(dllName)]
        public static extern int R2_CpuTransmit(IntPtr pHandle, int bProtocol, string pbSendBuff, int cbSendLength, StringBuilder pbRecvBuff, ref int pcbRecvLength);


        // 非接CPU

        [DllImport(dllName)]
        public static extern int R2_TypeACpuSelect(IntPtr pHandle, byte[] pbATRBuff, ref int pcbATRLength);

        [DllImport(dllName)]
        public static extern int R2_TypeBCpuSelect(IntPtr pHandle, byte[] pbATRBuff, ref int pcbATRLength);

        [DllImport(dllName)]
        public static extern int R2_TypeABCpuTransmit(IntPtr pHandle, byte[] pbSendBuff, int cbSendLength, byte[] pbRecvBuff, ref int pcbRecvLength);

        [DllImport(dllName)]
        public static extern int R2_TypeACpuGetUID(IntPtr pHandle, byte[] pbUIDBuff, ref int pcbUIDLength);


        int ICC_PROTOCOL_T0 = 0; // T = 0 协议
        int ICC_PROTOCOL_T1 = 1; // T = 1 协
        public static string R2Err { get; set; }
        static string recv, sw1sw2;

        static IntPtr pR2Handle = IntPtr.Zero;

        public int AddBankData()
        {
            MDATA mdata = MDATA.getInstance();
            string bankno = string.Empty;
            int nRet = GetBankNum(ref bankno);
            if (nRet == 0)
            {
                mdata.BankNO = bankno;
                mdata.BankCode = MDATASQL.getBankCode(mdata.BankNO);
            }
            else if (nRet == (int)GetBankErr.notsicard)
            {
                mdata.BankNO = "sitest" + DateTime.Now.ToString("yyyyMMddHHmmss");
                mdata.BankCode = MDATASQL.getBankCode(mdata.BankNO);
            }
            return nRet;
        }
        public int R2Connect()
        {
            int nRet = R2_Connect(Global.GetInstance().R2Port, 9600, ref pR2Handle);
            if (nRet != 0)
            {
                return nRet;
            }
            return 0;
        }


        public int GetBankNum(ref string bankno)
        {
            if (pR2Handle == IntPtr.Zero)
            {
                R2Connect();
            }
            if (pR2Handle == IntPtr.Zero)
            {
                R2Err = "非接读写器连接失败";
                return (int)GetBankErr.readerfail;
            }
            byte[] pbATRBuff = new byte[80];
            int pcbATRLength = 80;
            int nRet = R2_TypeACpuSelect(pR2Handle, pbATRBuff, ref pcbATRLength);
            if (nRet != 0)
            {
                R2Err = "激活卡片失败";
                return (int)GetBankErr.resetfail;
            }
            string csCommand = "";

            if (!ApduSend("00A404000E315041592E5359532E4444463031")) return (int)GetBankErr.notsicard;

            if (!ApduSend("00A4040007A0000003330101")) return (int)GetBankErr.notsicard;

            int ipos = 0;
            if (!ApduSend("00B2010C00")) return (int)GetBankErr.notsicard; //00B2011400
            if (sw1sw2.Substring(0, 2) == "6C")
            {
                csCommand = "00B2010C" + sw1sw2.Substring(2, 2);
                if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                ipos = recv.IndexOf("5713", 0);
                if (ipos == -1)
                {
                    if (!ApduSend("00B2020C00")) return (int)GetBankErr.notsicard; //00B2011400
                    if (sw1sw2.Substring(0, 2) == "6C")
                    {
                        csCommand = "00B2020C" + sw1sw2.Substring(2, 2);
                        if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                        ipos = recv.IndexOf("5713", 0);
                        if (ipos == -1)
                        {
                            ipos = recv.IndexOf("570F", 0);
                            if (ipos == -1)
                            {
                                R2Err = recv + "：570F";
                                Utility.WriteLog(R2Err);
                                return (int)GetBankErr.notsicard;
                            }
                        }
                    }
                }
            }

            int nPos = recv.IndexOf("D", 8);
            if (nPos <= 0)
            {
                csCommand = "00B2010C" + sw1sw2.Substring(2, 2);
                if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                ipos = recv.IndexOf("5713", 0);
                if (ipos == -1)
                {
                    if (!ApduSend("00B2020C00")) return (int)GetBankErr.notsicard; //00B2011400
                    if (sw1sw2.Substring(0, 2) == "6C")
                    {
                        csCommand = "00B2020C" + sw1sw2.Substring(2, 2);
                        if (!ApduSend(csCommand)) return (int)GetBankErr.notsicard;
                        ipos = recv.IndexOf("5713", 0);
                        if (ipos == -1)
                        {
                            ipos = recv.IndexOf("570F", 0);
                            if (ipos == -1)
                            {
                                R2Err = recv + "：570F";
                                Utility.WriteLog(R2Err);
                                return (int)GetBankErr.notsicard;
                            }
                        }
                    }
                }
                nPos = recv.IndexOf("D", 8);
                if (nPos <= 0)
                {
                    R2Err = recv + "：未发现d";
                    Utility.WriteLog(R2Err);
                    return (int)GetBankErr.notsicard;
                }
            }
            int Pos57 = recv.IndexOf("571");
            string csCreditCardID = recv.Substring(Pos57 + 4, nPos - Pos57 - 4);  // 银行卡号
            bankno = csCreditCardID;
            return 0;
        }

        private bool ApduSend(string cmd)
        {
            cmd = cmd.Replace(" ", "");
            byte[] pbSendBuff = strToToHexByte(cmd);
            byte[] pbRecvBuff = new byte[255];
            int pcbRecvLength = 255;
            int nRt = R2_TypeABCpuTransmit(pR2Handle, pbSendBuff, cmd.Length, pbRecvBuff, ref pcbRecvLength);
            if (nRt != 0)
            {
                R2Err = "指令发送失败: " + cmd;
                Utility.WriteLog(R2Err);
                return false;
            }
            else
            {
                string strResp = ByteToHexStr(pbRecvBuff, nRt);
                //Utility.WriteLog(cmd);
                //Utility.WriteLog(strResp);
                recv = strResp.Substring(0, strResp.Length - 4);
                sw1sw2 = strResp.Substring(strResp.Length - 4, 4);
                if (sw1sw2.Substring(0, 2) == "61" || sw1sw2 == "9000") return true;
                //MessageBox.Show("命令执行成功：" + strResp);
                return false;
            }
        }

        private byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = hexString.Insert(hexString.Length - 1, 0.ToString());
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        private string ByteToHexStr(byte[] bytes, int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
    }

    /// <summary>
    /// 510 证卡打印
    /// </summary>
    public class Solid510
    {
        /*********************************************************************************/
        /*  Solid510打印机接口
        /***********************************************************************************/

        [DllImport("Printer_Solid510.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_GetSN(StringBuilder snBuf, ref int buf_len);

        [DllImport("Printer_Solid510.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_GetErr(StringBuilder errBuf, ref int buf_len);

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_CardIn(); // 进卡到打印头位置

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_FeedContact(bool bDown = true);

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_ContactBack2Header(); // 从IC模块移卡到打印头位置

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_FeedContactLess();

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_OutKeep();       // 持卡

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_Out2Front();      // 出卡到前卡口

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_Out2Back();       // 出卡到后卡口

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 Solid510_GetStatus();     //查询打印机状态

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_GetRibbonRemain();  //查看色带剩余量

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_Reboot();           //重启

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_DoCleaning();      //清洁


        // 无驱接口
        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_GetDeviceList(StringBuilder descListBuf, ref int buf_len);

        [DllImport("Printer_Solid510.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_SBSOpen(int nDevice = 0);

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_SBSClose();

        [DllImport("Printer_Solid510.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_SBSPrintStart(bool b180 = true, int nDPI = 3);

        [DllImport("Printer_Solid510.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_SBSPrintText(
            [MarshalAs(UnmanagedType.LPWStr)] string chrText,    //打印的文字
            [MarshalAs(UnmanagedType.LPWStr)] string FontType,   //字体
            int nFontSize, //大小
            int nTextX,    //X轴方向位置
            int nTextY,    //y轴方向位置
            bool bBold = true // 是否加粗
        );

        [DllImport("Printer_Solid510.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Solid510_SBSPrintPicture(
            [MarshalAs(UnmanagedType.LPWStr)] string pchrPicPath, // 图片路径
            int nPicX,      // 打印位置X轴
            int nPicY,      // 打印位置Y轴
            int nWidth,     // 图片宽
            int nHeight     // 图片高
        );

        [DllImport("Printer_Solid510.dll")]
        public static extern int Solid510_SBSPrintEnd();

        static public bool bsolidopen0 = false;
        static public bool bsolidopen1 = false;
        static public string strError;
        static public int nStatus1 = 0, nStatus2 = 0;
        public static bool Init()
        {
            // 证卡机初始化

            if (Solid510_SBSOpen() == 0)
            {
                bsolidopen0 = true;
                nStatus1 = CheckPrinter();
                if (nStatus1 == 4)
                {
                    F8SAN.GetInstance().printer2car(1);
                    F8SAN.GetInstance().car2recycle();
                }
            }
            else
            {
                bsolidopen0 = false;
                strError = "彩印机1打开失败!";
            }
            if (Solid510_SBSOpen(1) == 0)
            {
                bsolidopen1 = true;
                nStatus2 = CheckPrinter();
                if (nStatus2 == 4)
                {
                    F8SAN.GetInstance().printer2car(2);
                    F8SAN.GetInstance().car2recycle();
                }
            }
            else
            {
                bsolidopen1 = false; strError = "彩印机2打开失败!";
            }
            if (!bsolidopen0 && !bsolidopen1) { strError = "两台彩印机都打开失败!"; return false; }

            if (nStatus1 == -1 && nStatus2 == -1) { strError = "两台彩印机都出故障，请检查!"; return false; }

            return true;
        }

        public static bool OpenPrinter(string SN)
        {
            int snLenth = SN.Length;
            StringBuilder sbSN1 = new StringBuilder(30);
            int buf_len = 30;
            if (Solid510_SBSOpen() == 0) {
                Solid510_GetSN(sbSN1, ref buf_len);
                string SN1 = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(sbSN1.ToString())).Trim();
                SN1 = SN1.Replace("?", "");//机器有14 16位序列号的，将？乱码去掉
                if (string.Compare(SN, SN1.Substring(0, snLenth)) == 0) {
                    Utility.WriteLog("打开打印机" + SN);
                    return true;
                }
                else {
                    if (Solid510_SBSOpen(1) != 0) {
                        strError = "彩印机2打开失败!";
                        return false;
                    }
                    StringBuilder sbSN2 = new StringBuilder(30);
                    buf_len = 30;
                    Solid510_GetSN(sbSN2, ref buf_len);
                    string SN2 = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(sbSN2.ToString())).Trim();
                    SN2 = SN2.Replace("?", "").Replace("RS", "");
                    if (string.Compare(SN, SN2.Substring(0, snLenth)) == 0) {
                        Utility.WriteLog("打开打印机" + SN);
                        return true;
                    }
                    else {
                        strError = SN + " != " + SN2;
                        return false;
                    }
                }
            }
            else {
                strError = "彩印机1打开失败!";
                if (Solid510_SBSOpen(1) != 0) {
                    strError = "彩印机2打开失败!";
                    return false;
                }
                StringBuilder sbSN2 = new StringBuilder(30);
                buf_len = 30;
                Solid510_GetSN(sbSN2, ref buf_len);
                string SN2 = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(sbSN2.ToString())).Trim();
                SN2 = SN2.Replace("?", "").Replace("RS", "");
                if (string.Compare(SN, SN2.Substring(0, snLenth)) == 0) {
                    Utility.WriteLog("打开打印机" + SN);
                    return true;
                }
                else {
                    strError = SN + " != " + SN2;
                    return false;
                }
            }
        }


        public static int CheckPrinter()
        {
            Int64 uis = Solid510.Solid510_GetStatus();
            if (uis == 0 || uis == 0x0000000040000000) { return 0; }
            else
            {
                switch (uis)
                {
                    case 0x0000000040121000: { strError = "无色带"; break; }
                    case 0x0000000040041000: { strError = "打印机夹卡"; return 4; }
                    default: { strError = string.Format("status => 0x{0:X8}", uis); break; }
                }
                return -1;
            }
        }

        public static int getPrinterParam()
        {
            Global gb = Global.GetInstance();
            if (OpenPrinter(gb.PrinterSN1))
            {
                if (CheckPrinter() == 0)
                {
                    return 1;
                }
                else
                {
                    if (OpenPrinter(gb.PrinterSN2))
                    {
                        if (CheckPrinter() == 0)
                        {
                            return 2;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            else
            {
                if (OpenPrinter(gb.PrinterSN2))
                {
                    if (CheckPrinter() == 0)
                    {
                        return 2;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public static int PrintCard(string name, string ID, string SICardNum, string picpath)
        {
            int iRet = -1;
            iRet = Solid510_SBSPrintStart(true, 3);
            if (iRet != 0) { return iRet; }

            Global gb = Global.GetInstance();
            float fPix = 11.8f;
            string m_date = DateTime.Now.ToString("yyyy年M月d日");
            string fonttype = gb.FontType;
            int nFontSize = gb.FontSize;
            iRet = Solid510_SBSPrintText(name, fonttype, nFontSize, (int)(gb.Name_X * fPix), (int)(gb.Name_Y * fPix), true);
            if (iRet != 0) { return iRet; }
            iRet = Solid510_SBSPrintText(ID, fonttype, nFontSize, (int)(gb.ID_X * fPix), (int)(gb.ID_Y * fPix), true);
            iRet = Solid510_SBSPrintText(SICardNum, fonttype, nFontSize, (int)(gb.CardNo_X * fPix), (int)(gb.CardNo_Y * fPix), true);
            iRet = Solid510_SBSPrintText(m_date, fonttype, nFontSize, (int)(gb.Date_X * fPix), (int)(gb.Date_Y * fPix), true);
            iRet = Solid510_SBSPrintPicture(picpath, 47, 130, 236, 295);
            if (iRet != 0) { return iRet; }
            Solid510_SBSPrintEnd();

            return 0;
        }

        /*********************************************************************************/
    }

    public class Msprint
    {
        //小票打印机部分
        private const string dllName = "MsprintsdkRM.dll";
        /// <summary>
        /// 设置设备名称
        /// </summary>
        /// <param name="strPort">设备端口名称，格式为USB001/COM1，当为USB时，iBaudrate参数无效格式为USB001/COM1，当为USB时，iBaudrate参数无效</param>
        /// <param name="iBaudrate">波特率</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetPrintport", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetPrintport(StringBuilder strPort, int iBaudrate);

        /// <summary>
        /// 打开USB接口（V2.0.2.4  通过查询PID/VID方式自动打开usb端口，无需手动查找端口）
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetUsbportauto", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetUsbportauto();

        /// <summary>
        /// 打印机初始化,完成缓存的清空等工作
        /// </summary>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetInit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetInit();

        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetClean", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetClean();

        /// <summary>
        /// 打印机关闭
        /// </summary>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetClose", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetClose();

        /// <summary>
        /// 设置行间距
        /// </summary>
        /// <param name="iLinespace">行间距，取值0-127，单位0.125mm</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetLinespace", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLinespace(int iLinespace);

        /// <summary>
        /// 设置字符间距
        /// </summary>
        /// <param name=" iSpace">字符间距，取值0-64，单位0.125mm</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetSpacechar", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSpacechar(int iSpace);

        /// <summary>
        /// 设置汉字间距
        /// </summary>
        /// <param name="iChsleftspace">汉字左空，取值0-64，单位0.125mm</param>
        /// <param name="iChsrightspace">汉字右空，取值0-64，单位0.125mm</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetSpacechinese", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSpacechinese(int iChsleftspace, int iChsrightspace);

        /// <summary>
        /// 设置左边界
        /// </summary>
        /// <param name="iLeftspace">设置左边距，取值0-576，单位0.125mm</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetLeftmargin", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLeftmargin(int iLeftspace);



        /// <summary>
        /// 设置黑标切纸偏移量
        /// </summary>
        /// <param name="iOffset">偏移量，取值0-1600</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetMarkoffsetcut", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetMarkoffsetcut(int iOffset);

        /// <summary>
        /// 设置黑标打印进纸偏移量（不兼容旧版本小票机）
        /// </summary>
        /// <param name="iOffset">偏移量，取值0-1600</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetMarkoffsetprint", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetMarkoffsetprint(int iOffset);


        /// <summary>
        /// 设置汉字放大 
        /// </summary>
        /// <param name="iHeight">倍高 0 无效 1 有效</param>
        /// <param name="iWidth">倍宽 0 无效 1 有效</param>
        /// <param name="iUnderline">下划线 0 无效 1 有效</param>
        /// <param name="iChinesetype">汉字字形 0：24*24； 1：16*16</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetSizechinese", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSizechinese(int iHeight, int iWidth, int iUnderline, int iChinesetype);

        /// <summary>
        /// 设置字符放大
        /// </summary>
        /// <param name="iHeight">倍高 0 无效 1 有效</param>
        /// <param name="iWidth">倍宽 0 无效 1 有效</param>
        /// <param name="iUnderline">下划线 0 无效 1 有效</param>
        /// <param name="iAsciitype">ASCII字形 0:12*24； 1:9*17</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetSizechar", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSizechar(int iHeight, int iWidth, int iUnderline, int iAsciitype);

        /// <summary>
        /// 设置文本放大
        /// </summary>
        /// <param name="iHeight">放大高度，取值(1-8)</param>
        /// <param name="iWidth">放大宽度，取值(1-8)</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetSizetext", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSizetext(int iHeight, int iWidth);

        /// <summary>
        ///  设置字符对齐
        /// </summary>
        /// <param name="iAlignment">0 左、1 居中、2 右</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetAlignment", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetAlignment(int iAlignment);

        /// <summary>
        /// 设置字体加粗
        /// </summary>
        /// <param name="iBold">0 不加粗、1 加粗</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetBold", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetBold(int iBold);

        /// <summary>
        /// 设置字体旋转 
        /// </summary>
        /// <param name="iRotate">0 解除旋转、1 顺时针度旋转90°</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetRotate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetRotate(int iRotate);

        /// <summary>
        /// 设置字体方向
        /// </summary>
        /// <param name="iDirection">0 左至右、1 旋转180度</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetDirection", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetDirection(int iDirection);

        /// <summary>
        /// 设置反白
        /// </summary>
        /// <param name="iWhite">0 取消反白；1 设置反白</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetWhitemodel", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetWhitemodel(int iWhite);

        /// <summary>
        /// 设置斜体（不兼容旧版本）
        /// </summary>
        /// <param name="iItalic">0 取消斜体；1 设置斜体</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetItalic", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetItalic(int iItalic);



        /// <summary>
        /// 设置下划线（字符，ASCII 都有效）
        /// </summary>
        /// <param name=" underline"> 0 无 1 一个点下划线  2 两个点下划线 其他无效</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "SetUnderline", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetUnderline(int underline);

        /// <summary>
        ///   设置汉字模式
        /// </summary>
        /// <param name="mode">0 进入汉字模式；1 退出汉字模式</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetReadZKmode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetReadZKmode(int mode);

        /// <summary>
        /// 设置水平制表位置
        /// </summary>
        /// <param name="bHTseat"> 水平制表的位置 从小到大 单位一个ASCII字符 不能为0。</param>
        /// <param name="iLength">水平制表的位置数据的个数。</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetHTseat", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetHTseat(string bHTseat, int iLength);

        /// <summary>
        /// 设置区域国家和代码页
        /// </summary>
        /// <param name="country">区域国家 0 美国 1 法国 2 德国 3 英国 4 丹麦 I 5 瑞典 6 意大利 7 西班牙 I 8 日本 9 挪威 10 丹麦 II</param>
        /// <param name="CPnumber"> 代码页 0 PC437[美国欧洲标准] 1 PC737 2 PC775 
        /// 3 PC850 4 PC852 5 PC855 6 PC857 7 PC858 8 PC860 9 PC862
        /// 10 PC863 11 PC864 12 PC865 13 PC866 14 PC1251 15 PC1252 16 PC1253
        /// 17 PC1254 18 PC1255 19 PC1256 20 PC1257 21 PC928 22 Hebrew old
        /// 23 IINTEL CHAR 18 Katakana 25 特殊符号00-1F 26 SPACE PAGE</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetCodepage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetCodepage(int country, int CPnumber);

        /// <summary>
        /// 设置NV位图
        /// </summary>
        /// <param name="iNums">位图数量(单个文件最大64K，所有文件最大192K)</param>
        /// <param name="strPath">图像文件路径（若只有文件名则使用当前路径，若指定全路径则使用指定的
        /// 路径），以”;”分隔，个数需和iNums参数一致/param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetNvbmp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetNvbmp(int iNums, string strPath);


        /// <summary>
        /// 设置打印机ID或名称(不兼容旧版小票机)
        /// </summary>
        /// <param name="strIDorNAME">打印机ID或名称</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "SetPrintIDorName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetPrintIDorName(ref StringBuilder strIDorNAME);


        /// <summary>
        /// 获取打印机ID或名称
        /// </summary>
        /// <param name="strIDorNAME">打印机ID或名称</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "GetPrintIDorName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPrintIDorName(byte[] strIDorNAME);


        /// <summary>
        /// 打印自检页
        /// </summary>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintSelfcheck", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintSelfcheck();

        /// <summary>
        /// 打印并走纸
        /// </summary>
        /// <param name="iLine">走纸行数</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintFeedline", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintFeedline(int iLine);

        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="strData">打印的字符串内容</param>
        /// <param name="iImme">是否加换行指令0x0a： 0 加换行指令 1 不加换行指令</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintString(string strData, int iImme);

        /// <summary>
        /// 打印内容并换行，无打印内容时走一空白行
        /// </summary>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintChargeRow", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintChargeRow();

        /// <summary>
        /// 打印细走纸，单位点0.125mm
        /// </summary>
        /// <param name="Lnumber">范围0-250</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintFeedDot", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintFeedDot(int Lnumber);

        /// <summary>
        /// 执行到下一个水平制表位置
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintNextHT", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintNextHT();


        /// <summary>
        /// 打印切纸
        /// </summary>
        /// <param name="iMode">0 全切  1半切</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintCutpaper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintCutpaper(int iMode);

        /// <summary>
        /// 黑标模式下检测黑标，停止在黑标位置
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintMarkposition", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintMarkposition();
        /// <summary>
        /// 黑标模式下检测黑标并进纸到打印位置（偏移量打印影响走纸距离)
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintMarkpositionprint", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintMarkpositionprint();

        /// <summary>
        /// 检测黑标进纸到切纸位置 黑标模式下检测黑标并进纸到切纸位置（偏移量切纸影响走纸距离）
        /// </summary>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintMarkpositioncut", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintMarkpositioncut();

        /// <summary>
        ///   打印黑标切纸
        /// </summary>
        /// <param name="iMode">0 检测黑标全切、1 不检测黑标半切</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintMarkcutpaper", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintMarkcutpaper(int iMode);
        /// <summary>
        /// 打印QR码（旧标准程序参数需更改（char* const strData，int iLmargin，int iMside，0））
        /// </summary>
        /// <param name="strData">内容</param>
        /// <param name="iLmargin">左边距，取值0-27 单位mm</param>
        /// <param name="iMside">单位长度，即QR码大小，取值1-8，（有些打印机型只支持1-4）</param>
        /// <param name="iRound">环绕模式，0环绕（混排，有些机型不支持）、1立即打印（不混排）</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintQrcode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintQrcode(StringBuilder strData, int iLmargin, int iMside, int iRound);

        /// <summary>
        /// QR混排打印时候，打印剩下QR码(不兼容久版小票机)
        /// </summary>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintRemainQR", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintRemainQR();

        /// <summary>
        /// 打印PDF417码（不兼容旧版小票机）
        /// </summary>
        /// <param name="iDotwidth">宽度，取值0-255</param>
        /// <param name="iDotheight">高度，取值0-255</param>
        /// <param name="iDatarows">行数</param>
        /// <param name="iDatacolumns">列数</param>
        /// <param name="strData">内容</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintPdf417", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintPdf417(int iDotwidth, int iDotheight, int iDatarows, int iDatacolumns, string strData);

        /// <summary>
        ///   打印 一维 条 码 
        /// </summary>
        /// <param name="iWidth">条码宽度，取值2-6 单位 0.125mm</param>
        /// <param name="iHeight">条码高度，取值1-255 单位0.125mm</param>
        /// <param name="iHrisize">条码显示字符字型 0 12*24 1 9*17</param>
        /// <param name="iHriseat">条码显示字符位置 0 无 1 上 2 下 3 上下</param>
        /// <param name="iCodetype">条码的类型
        /// （* UPC-A 0,* UPC-E 1,* EAN13 2,* EAN8 3,
        /// * CODE39 4,* ITF 5,* CODABAR 6,* Standard EAN13 7,
        /// * Standard EAN8 8,* CODE93 9,* CODE128 10)
        /// </param>
        /// <param name="strData">条码内容</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "Print1Dbar", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Print1Dbar(int iWidth, int iHeight, int iHrisize, int iHriseat, int iCodetype, StringBuilder strData);

        /// <summary>
        /// 打印磁盘BMP文件，仅支持单色BMP文件
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintDiskbmpfile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintDiskbmpfile(string strPath);

        /// <summary>
        /// 打印磁盘BMP文件，仅支持1bit/24bit深度BMP文件
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintDiskimgfile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintDiskimgfile(string strPath);


        /// <summary>
        /// 打印NV BMP文件，仅支持单色BMP文件
        /// </summary>
        /// <param name="iNvindex">NV位图索引</param>
        /// <param name="iMode"> 48 普通、49 倍宽、50 倍高、51 倍宽倍高(4倍大小)</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "PrintNvbmp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintNvbmp(int iNvindex, int iMode);

        /// <summary>
        /// 转发指令，将指令原样转给打印机
        /// </summary>
        /// <param name="strCmd">指令（参数类型可能会有问题）</param>
        /// <param name="iLength">指令长度</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "PrintTransmit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PrintTransmit(String strCmd, int iLength);

        /// <summary>
        /// 转发指令，将指令原样转给打印机，并接收返回值
        /// </summary>
        /// <param name="strCmd">指令（参数类型可能会有问题）</param>
        /// <param name="iLength">指令长度</param>
        /// <param name="strRecv">接收数据的地址（参数类型可能会有问题）</param>
        /// <param name="iRelen">接收数据的长度</param>
        /// <returns>0成功、1失败</returns>
        [DllImport(dllName, EntryPoint = "GetTransmit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTransmit(string strCmd, int iLength, StringBuilder strRecv, int iRelen);

        /// <summary>
        /// 获取打印机状态
        /// </summary>
        /// <returns>
        /// 0 打印机正常
        /// 1 打印机未连接或未上电
        /// 2 打印机和调用库不匹配
        /// 3 打印头未打开
        /// 4 切刀未复位
        /// 5 打印头过热
        /// 6 黑标错误
        /// 7 纸尽
        /// 8 纸将尽</returns>
        [DllImport(dllName, EntryPoint = "GetStatus", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetStatus();

        /// <summary>
        /// 获取打印机特殊功能状态(不兼容旧版小票机)仅适用于D347 部分机型
        /// 0 打印机正常
        /// 1 打印机未连接或未上电
        /// 2 打印机和调用库不匹配
        /// 3 当前使用打印机无特殊功能
        /// 4 容纸器错误
        /// 5 堵纸
        /// 6 卡纸
        /// 7 拽纸
        /// 8 出纸传感器有纸
        /// </summary>
        [DllImport(dllName, EntryPoint = "GetStatusspecial", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetStatusspecial();

        /// <summary>
        /// 获取打印机信息
        /// </summary>
        /// <param name="iFstype"> 信息类型
        /// 1 打印头型号ID
        /// 2 类型ID
        /// 3 软件版本
        /// 4 生产厂商信息
        /// 5 打印机型号
        /// 6 支持的中文编码格式</param>
        /// <param name="bFiddata">返回信息</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "GetProductinformation", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetProductinformation(int iFstype, byte[] bFiddata);

        /// <summary>
        /// 获取开发包版本等信息
        /// </summary>
        /// <param name="bInfodata">返回信息</param>
        /// <returns></returns>
        [DllImport(dllName, EntryPoint = "GetSDKinformation", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSDKinformation(byte[] bInfodata);

        public static int iTickitInit = 1;
        public static string strErr = "";

        public static int CheckMsPrinter()
        {
            if (iTickitInit != 0)
            {
                // USB连接
                //int ret = SetUsbportauto();
                // 串口连接
                int ret = SetPrintport(new StringBuilder(Global.GetInstance().TicketPort), 9600);
                if (ret != 0)
                {
                    Utility.WriteLog("打开端口失败！");
                    return 1;
                }
                iTickitInit = SetInit();
                if (iTickitInit != 0)
                {
                    iTickitInit = SetInit();
                    if (iTickitInit != 0)
                    {
                        Utility.WriteLog("小票打印机初始化失败！");
                        return 2;
                    }
                }
            }

            ////查询打印机状态
            //int iStatus = GetStatus();
            //if (iStatus != 0) {
            //    string status = "";
            //    switch (iStatus) {
            //        case 1: status = "打印机未连接或未上电"; break;
            //        case 2: status = "打印机和调用库不匹配"; break;
            //        case 3: status = "打印头未打开"; break;
            //        case 4: status = "切刀未复位"; break;
            //        case 5: status = "打印头过热"; break;
            //        case 6: status = "黑标错误"; break;
            //        case 7: status = "纸尽"; break;
            //        case 8: status = "纸将尽"; return 0;
            //        default:
            //            status = "未知错误，返回值：" + iStatus.ToString();
            //            break;
            //    }
            //    strErr = status;
            //    Utility.WriteLog(strErr);
            //    return 3;
            //}
            return 0;
        }

        public static void PrintTicket(string name, string id, string paycode)
        {
            try
            {
                if (CheckMsPrinter() != 0) return;
                SetClean();
                SetAlignment(1);// 设置文字居中
                SetLinespace(30); // 设置行间距30*0.125mm
                string strData = "自助终端缴费回执单";
                PrintString(strData, 0);


                SetAlignment(0); // 设置文字左对齐
                strData = string.Format("尊敬的{0}您好：", name);
                PrintString(strData, 0);
                strData = "    您已成功缴纳社会保障卡补换卡工本费！";
                PrintString(strData, 0);

                strData = "    缴费金额：16元";
                PrintString(strData, 0);
                strData = "    社会保障号码：" + id.Substring(0, 6) + "********" + id.Substring(id.Length - 4, 4);
                PrintString(strData, 0);
                strData = "    缴款码：" + paycode;
                PrintString(strData, 0);
                strData = "    感谢您的使用，如需财政票据，请到柜台咨询，";
                PrintString(strData, 0);
                strData = "此回执单为财政票据领取凭证！";
                PrintString(strData, 0);
                strData = "    社会保障卡服务热线：12333";
                PrintString(strData, 0);
                SetAlignment(2); // 设置文字右对齐
                strData = "（电子签章）    ";
                PrintString(strData, 0);
                strData = DateTime.Now.ToString("yyyy年MM月dd日    ");
                PrintString(strData, 0);
                SetLeftmargin(0);
                string qrcodepath1 = System.Environment.CurrentDirectory + "\\pic\\QrImg570p1.bmp";
                strData = qrcodepath1;
                PrintDiskbmpfile(strData);
                PrintFeedline(6);
                PrintCutpaper(0);
            }
            catch (Exception ex)
            {
                Utility.WriteLog("美松小票打印异常：" + ex.Message);
            }
        }
        public static int PrintQrString(StringBuilder qrCode, string title)
        {
            try
            {
                if (CheckMsPrinter() != 0) return -1;
                SetClean();
                SetAlignment(1);// 设置文字居中
                PrintFeedline(1);
                PrintQrcode(qrCode, 20, 8, 1);
                PrintString(title, 0);
                PrintFeedline(7);
                PrintCutpaper(0);
                return 0;
            }
            catch (Exception ex)
            {
                Utility.WriteLog("小票打印异常：" + ex.Message);
                return -1;
            }
        }

    }

    /// <summary>
    /// 指示灯
    /// </summary>
    public class U10
    {
        // 打开串口
        // 0	_OK_ COMMAND
        // -101 端口已经打开  
        // -102 端口没有打开	
        // -103	_ERR_ GET CLASS DEVS
        // -104	_ERR_ NO FIND USB DEVS
        // -105	_ERR_ CONNECT DEVS
        // -108 _ERR_PARMERR
        // 使用usb，参数没实际意义
        [DllImport("U10_Control.dll")]
        public static extern int GcOpenDevice(string vid, string pid, string SerialString);

        [DllImport("U10_Control.dll")]
        public static extern int GcCloseDevice();

        [DllImport("U10_Control.dll")]
        public static extern int GcGetData(int comid, byte[] buff);

        [DllImport("U10_Control.dll")]
        public static extern int GcSendData(int comid, byte[] buff);

        [DllImport("U10_Control.dll")]
        public static extern int GcSendDataEX(int comid, int hightime, int lowtime, bool done, byte[] buff);

        static public int U10_LIGHT_1 = 0;
        static public int U10_LIGHT_2 = 1;
        static public int U10_LIGHT_3 = 2;
        static public int U10_LIGHT_4 = 3;

        static public byte U10_CTRL_ON = 1;
        static public byte U10_CTRL_OFF = 0;

        static public int nOpen = -1;
        static byte[] buff = new byte[30];
        static public int OpenU10()
        {
            return GcOpenDevice("0483", "5750", "");
        }

        /// <summary>
        /// 控制灯开关
        /// </summary>
        /// <param name="nWhich">第几个灯：0、1、2、3</param>
        /// <param name="nCtrl">控制：1 开、0 关</param>
        /// <returns></returns>
        static public int ContrlLight(int nWhich, byte nCtrl)
        {
            if (nOpen <= 0)
            {
                nOpen = OpenU10();
                if (nOpen <= 0)
                {
                    Utility.WriteLog("U10指示灯连接失败！");
                    return nOpen;
                }
            }
            buff[nWhich] = nCtrl;
            return GcSendData(0, buff);
        }
    }

    /// <summary>
    /// 条码器
    /// </summary>
    public class EM20_EX
    {
        [DllImport("com_api.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool com_open(int portnr, int baud = 9600, char parity = 'N', int databits = 8, int stopbits = 1);

        [DllImport("com_api.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void com_close();

        [DllImport("com_api.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool com_send(string sendData);

        [DllImport("com_api.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool com_read(StringBuilder readBuf, ref int buf_len, bool isHex = false);

        public static bool isScannerOpen = false;
        public static string errMsg = "";
        public static string userName = "";
        public static string userID = "";
        public static string Phone = "";
        public static string DateTime = "";
        public static bool ReadQrData()
        {
            if (!isScannerOpen)
            {
                isScannerOpen = com_open(Global.GetInstance().ScannerCom);
                if (!isScannerOpen)
                {
                    errMsg = "扫码器端口打开失败！";
                    Utility.WriteLog(errMsg);
                    return false;
                }
            }
            StringBuilder readBuf = new StringBuilder(1024);
            int len = 1024;
            if (!com_read(readBuf, ref len))
                return false;
            string info = readBuf.ToString();
            if (info != "" && info.IndexOf(Global.GetInstance().codeTaitou) >= 0)
            {
                string[] temp = info.Split('|');
                string DecData = AES.AESDecrypt(temp[1].Trim(), Global.GetInstance().desKey);
                DecData = AES.HexStringToString(DecData.Replace("00000000", ""), Encoding.UTF8).Replace("\0", "");
                Utility.WriteLog(DecData);
                string[] personinfo = DecData.Split('|');
                userID = personinfo[0];
                userName = personinfo[1];
                Phone = personinfo[2];
                DateTime = personinfo[3];
                return true;
            }
            return false;
        }

        public static string ReadQrPwd()
        {
            if (!isScannerOpen)
            {
                isScannerOpen = com_open(Global.GetInstance().ScannerCom);
                if (!isScannerOpen)
                {
                    errMsg = "扫码器端口打开失败！";
                    Utility.WriteLog(errMsg);
                    return "";
                }
            }
            StringBuilder readBuf = new StringBuilder(20);
            int len = 20;
            if (!com_read(readBuf, ref len))
                return "";
            string info = readBuf.ToString().Trim();
            return info;
            //if (!string.IsNullOrEmpty(info)) return info;
        }

        /////////////////////////////////// 控制灯 /////////////////////////////////
        public static void ControlLight(bool b_on)
        {
            if (!isScannerOpen)
            {
                isScannerOpen = com_open(Global.GetInstance().ScannerCom);
                if (!isScannerOpen)
                {
                    errMsg = "控制灯端口打开失败！";
                    Utility.WriteLog(errMsg);
                    return;
                }
            }
            string cmd;
            if (b_on) cmd = "024F0641313B";
            else cmd = "024F06323049";
            com_send(cmd);
        }
    }

    /// <summary>
    /// 加密机
    /// </summary>
    public class Jiamiji
    {
        [DllImport("HN_PERSONAL_UDLL.dll")]
        public extern static void SetLogDir(string strLogDir);

        // 密钥认证升级
        [DllImport("HN_PERSONAL_UDLL.dll")]
        public extern static void InitTSParam(string strIP, string strMac, string strPsamID, string strMedicalID, string strBankID);

        [DllImport("HN_PERSONAL_UDLL.dll")]
        public extern static int getActionData(int iType);

        [DllImport("HN_PERSONAL_UDLL.dll")]
        public extern static int UploadCardData(string strPersID, string strCardID, string strName);

        [DllImport("HN_PERSONAL_UDLL.dll")]
        public extern static int DevQualifiedAuth();

    }
}
