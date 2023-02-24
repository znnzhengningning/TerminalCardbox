using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;

namespace SHBSS
{
    public class INIClass
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="INIPath">文件路径</param>
        public INIClass(string INIPath)
        {

        }
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void IniWriteValue(string Section, string Key, string Value, string path)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }
        /// <summary>
        /// 读出INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        public static string IniReadValue(string Section, string Key, string path)
        {
            //StringBuilder temp = new StringBuilder(1200);
            byte[] temp = new byte[1200];
            int i = GetPrivateProfileString(Section, Key, "", temp, 1200, path);
            var enc = System.Text.Encoding.UTF8;
            string s = enc.GetString(temp);
            s = s.Substring(0, i);
            s = s.Replace("\0", "");
            return s.Trim();
            //return temp.ToString();
        }

        public static Int32 IniReadInt(string Section, string Key, string path)
        {
            int i = GetPrivateProfileInt(Section, Key, 0, path);
            return i;
        }

        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        /// <returns>布尔值</returns>
        public static bool ExistINIFile(string path)
        {
            return File.Exists(path);
        }
    }
}