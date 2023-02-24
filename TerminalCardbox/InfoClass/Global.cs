using System;
using System.IO;

namespace SHBSS
{

    enum GetBankErr
    {
        g_ok = 0,
        readerfail = 1,
        nofindcard = 2,
        resetfail = 3,
        notsicard = 4
    }
    public class Global
    {
        private static Global _instance = null;   //静态私有成员变量，存储唯一实例
        private Global()    //私有构造函数，保证唯一性
        {
        }
        public static Global GetInstance()    //公有静态方法，返回一个唯一的实例
        {
            if (_instance == null) {
                _instance = new Global();
            }
            return _instance;
        }

        public static int SleepTime { get { return 5000; } }

        public string CityName { get; set; } // 城市名称

        // 读写器码值
        public int RWPort1 { get; set; }
        public int RWPort2 { get; set; }
        public int nPortPin { get; set; }

        public int R2Port { get; set; }

        /// <summary>
        /// 小票机端口号
        /// </summary>
        public string TicketPort { get; set; }

        // 打印机序列号
        public string PrinterSN1 { get; set; }
        public string PrinterSN2 { get; set; }

        public string AdminPwd { get; set; }
        public string UserPwd { get; set; }

        // 卡盒模组端口号
        public string F5Com { get; set; }
        public int F8Com { get; set; }

        // 扫码器串口
        public int ScannerCom { get; set; }

        // 身份二维码抬头
        public string codeTaitou { get { return "xindakeji"; } }

        // 卡管用户名
        public string KGUser { get; set; }

        // 卡管密码
        public string KGPwd { get; set; }

        // 卡管照片用户名
        public string KGPicUser { get; set; }

        // 卡管照片密码
        public string KGPicPwd { get; set; }

        // 城市代码
        public string CITYCODE { get; set; }

        // 经办机构
        public string AGENCY { get; set; }

        // 所属区县
        public string DISTRCT { get; set; }

        public string PostCode { get; set; } // 邮政编码

        // 加密机IP
        public string JiamijiIP { get; set; }

        // 终端识别码  通过cmd的ipconfig /all命令  当前网卡的物理地址即Mac
        public string DevMac { get; set; }

        // PSAM卡号 无PSAM卡12个0  有PSAM卡填实际卡号
        public string PsamNo { get; set; }

        // 定点医院或药店编号  若没有要求，使用默认
        public string MedicalID { get; set; }

        // 银行代码
        public string BankCode { get; set; }

        // 屏幕分辨率
        static public int nWidth { get { return 1280; }}
        static public int nHeight { get { return 1024; }}

        public string SysSetPwd { get; set; } // 系统设置密码
        public string desKey { get { return "78696E6461363636"; } }    // DES加密密钥

        public string FontType { get; set; }
        public int FontSize { get; set; }
        public int Name_X { get; set; }
        public float Name_Y { get; set; }
        public int ID_X { get; set; }
        public float ID_Y { get; set; }
        public int CardNo_X { get; set; }
        public float CardNo_Y { get; set; }
        public int Date_X { get; set; }
        public float Date_Y { get; set; }

        public int ReadConfig()
        {
            try {
                string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
                if (!File.Exists(iniPath)) {
                    Utility.WriteLog("配置文件不存在！");
                    return -1;
                }
                SysSetPwd = INIClass.IniReadValue("DEVICE", "SysSetPwd", iniPath);
                RWPort1 = INIClass.IniReadInt("DEVICE", "RWPort1", iniPath);
                RWPort2 = INIClass.IniReadInt("DEVICE", "RWPort2", iniPath);
                nPortPin = INIClass.IniReadInt("DEVICE", "nPortPin", iniPath);
                TicketPort = INIClass.IniReadValue("DEVICE", "TicketPort", iniPath);
                PrinterSN1 = INIClass.IniReadValue("DEVICE", "PrinterSN1", iniPath);
                PrinterSN2 = INIClass.IniReadValue("DEVICE", "PrinterSN2", iniPath);
                ScannerCom = INIClass.IniReadInt("DEVICE", "ScannerCom", iniPath);
                R2Port = INIClass.IniReadInt("DEVICE", "R2Port", iniPath);

                AdminPwd = INIClass.IniReadValue("F5SAN", "AdminPwd", iniPath);
                UserPwd = INIClass.IniReadValue("F5SAN", "UserPwd", iniPath);
                F8Com = INIClass.IniReadInt("F5SAN", "F8Com", iniPath);
                F5Com = INIClass.IniReadValue("F5SAN", "F5Com", iniPath);



                KGUser = INIClass.IniReadValue("KAGUAN", "KGUser", iniPath);
                KGPwd = INIClass.IniReadValue("KAGUAN", "KGPwd", iniPath);
                KGPicUser = INIClass.IniReadValue("KAGUAN", "KGPicUser", iniPath);
                KGPicPwd = INIClass.IniReadValue("KAGUAN", "KGPicPwd", iniPath);
                CITYCODE = INIClass.IniReadValue("KAGUAN", "CITYCODE", iniPath);
                AGENCY = INIClass.IniReadValue("KAGUAN", "AGENCY", iniPath);
                DISTRCT = INIClass.IniReadValue("KAGUAN", "DISTRCT", iniPath);
                PostCode = INIClass.IniReadValue("KAGUAN", "PostCode", iniPath);
                CityName = INIClass.IniReadValue("KAGUAN", "CityName", iniPath);

                JiamijiIP = INIClass.IniReadValue("JIAMIJI", "IP", iniPath);
                DevMac = INIClass.IniReadValue("JIAMIJI", "Mac", iniPath);
                PsamNo = INIClass.IniReadValue("JIAMIJI", "PsamNo", iniPath);
                MedicalID = INIClass.IniReadValue("JIAMIJI", "MedicalID", iniPath);
                BankCode = INIClass.IniReadValue("JIAMIJI", "BankCode", iniPath);

                FontType = INIClass.IniReadValue("TXTPOS", "FontType", iniPath);
                FontSize = INIClass.IniReadInt("TXTPOS", "FontSize", iniPath);
                Name_X = int.Parse(INIClass.IniReadValue("TXTPOS", "Name_X", iniPath));
                Name_Y = float.Parse(INIClass.IniReadValue("TXTPOS", "Name_Y", iniPath));
                ID_X = int.Parse(INIClass.IniReadValue("TXTPOS", "ID_X", iniPath));
                ID_Y = float.Parse(INIClass.IniReadValue("TXTPOS", "ID_Y", iniPath));
                CardNo_X = int.Parse(INIClass.IniReadValue("TXTPOS", "CardNo_X", iniPath));
                CardNo_Y = float.Parse(INIClass.IniReadValue("TXTPOS", "CardNo_Y", iniPath));
                Date_X = int.Parse(INIClass.IniReadValue("TXTPOS", "Date_X", iniPath));
                Date_Y = float.Parse(INIClass.IniReadValue("TXTPOS", "Date_Y", iniPath));

                return 0;
            }
            catch (Exception ex) {
                Utility.WriteLog("ReadConfig Exception：" + ex.Message);
                return -1;
            }
        }
    }
}
