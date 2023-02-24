using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBSS
{
    public class TerminalInfo
    {
        private static TerminalInfo _instance = null;   //静态私有成员变量，存储唯一实例
        private TerminalInfo()    //私有构造函数，保证唯一性
        {
        }
        public static TerminalInfo GetInstance()    //公有静态方法，返回一个唯一的实例
        {
            if (_instance == null) {
                _instance = new TerminalInfo();
            }
            return _instance;
        }

        public string serverurl { get; set; } // 欣大内网服务器
        public string host_code { get; set; } // 
        public string secretKeySy { get; set; } // 对称加密密钥
        public string Rundate { get; set; }
        public int dayam { get; set; }
        public int daypm { get; set; }

        public static int SelfTerm { get { return 1; } }
        public static int Desktop { get { return 2; } }
        public string PrinterName { get; set; } // 打印机名称
        public string PrinterSN { get; set; } // 打印机序列号
        public int TerminalType { get { return Desktop; } } // 终端类型 1自助机  2桌面机
        public int RibbonRemain { get; set; } // 色带剩余量
        public string DistrctCode { get; set; } // 所属区县代码

        public static int New { get { return 1; } }
        public static int Reissue { get { return 2; } }
        public int maketype { get; set; } // 办卡类型  1新增  2补办


        public int ReadConfig()
        {
            try {
                string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
                if (!File.Exists(iniPath)) {
                    Utility.WriteLog("配置文件不存在！");
                    return -1;
                }
                serverurl = INIClass.IniReadValue("XINDATECH", "serverurl", iniPath);
                host_code = INIClass.IniReadValue("XINDATECH", "host_code", iniPath);
                secretKeySy = INIClass.IniReadValue("XINDATECH", "secretKeySy", iniPath);

                Rundate = INIClass.IniReadValue("XINDATECH", "Rundate", iniPath);
                dayam = INIClass.IniReadInt("XINDATECH", "dayam", iniPath);
                daypm = INIClass.IniReadInt("XINDATECH", "daypm", iniPath);

                return 0;
            }
            catch (Exception ex) {
                Utility.WriteLog("XindaTechApi ReadConfig异常：" + ex.Message);
                return -1;
            }
        }

        public void ReadRunDay()
        {
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            Rundate = INIClass.IniReadValue("XINDATECH", "Rundate", iniPath);
            dayam = INIClass.IniReadInt("XINDATECH", "dayam", iniPath);
            daypm = INIClass.IniReadInt("XINDATECH", "daypm", iniPath);
        }

        public void SetRundate()
        {
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            INIClass.IniWriteValue("XINDATECH", "Rundate", DateTime.Now.ToString("yyyyMMdd"), iniPath);
        }
        public void SetDayam(string value)
        {
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            INIClass.IniWriteValue("XINDATECH", "dayam", value, iniPath);
        }
        public void SetDaypm(string value)
        {
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            INIClass.IniWriteValue("XINDATECH", "daypm", value, iniPath);
        }
    }
}
