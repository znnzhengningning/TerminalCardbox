using CommenLib.HttpUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SHBSS
{
    public class XindaTechApi
    {
        public static string error { get; set; }      
        public static bool UpTerminalInfo()
        {
            string recvString = string.Empty;
            try {
                TerminalInfo terminfo = TerminalInfo.GetInstance();
                string body = "{{\"name\": \"{0}\", \"code\": \"{1}\", \"type\": \"{2}\", \"margin\": \"{3}\", \"county\": \"{4}\", \"timestamp\":\"{5}\"}}";
                TimeSpan timespan = DateTime.UtcNow - new DateTime(1970, 1, 1);
                string noise = ((long)timespan.TotalMilliseconds).ToString();
                body = string.Format(body, terminfo.PrinterName, terminfo.PrinterSN, terminfo.TerminalType
                    , terminfo.RibbonRemain, terminfo.DistrctCode, noise);
                string ciphertext = AES_ECB.AesEncryptor_Hex(body, terminfo.secretKeySy);
                string send = string.Format("{{\"host_code\":\"{0}\", \"ciphertext\":\"{1}\"}}", terminfo.host_code, ciphertext);

                RestClient client = new RestClient();
                client.EndPoint = terminfo.serverurl + "/terminal";
                client.Method = HttpVerb.POST;
                //client.ContentType = "application/x-www-form-urlencoded";
                client.ContentType = "application/json";
                client.PostData = send;
                recvString = client.MakeRequest();

                //Utility.WriteLog(send);
                //Utility.WriteLog(recvString);

                dynamic jobject1 = Newtonsoft.Json.Linq.JObject.Parse(recvString) as dynamic;
                string code = jobject1["code"].ToString();
                if (code == "0") return true;
                error = jobject1["message"].ToString();

                return false;
            }
            catch(Exception ex) {
                error = ex.Message;
                Utility.WriteLog(recvString);
                return false;
            }
        }

        public static bool UpCardInfo()
        {
            string recvString = string.Empty;
            try {
                TerminalInfo terminfo = TerminalInfo.GetInstance();
                SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
                string body = "{{\"code\": \"{0}\", \"name\": \"{1}\", \"type\": \"{2}\", \"age\": \"{3}\", \"gender\": \"{4}\", \"card_no\":\"{5}\", \"phone\": \"{6}\", \"county\": \"{7}\", \"bank_name\": \"{8}\", \"id_card\": \"{9}\", \"timestamp\": \"{10}\"}}";
                TimeSpan timespan = DateTime.UtcNow - new DateTime(1970, 1, 1);
                string noise = ((long)timespan.TotalMilliseconds).ToString();
                int age = GetAgeByBirthdate(IdCardtoDate(ssi.ID));
                body = string.Format(body, terminfo.PrinterSN, ssi.ownerName, terminfo.maketype, age, ssi.sex,
                    ssi.securityNo, ssi.phone, terminfo.DistrctCode, KaGuan.BANK, ssi.ID, noise);
                string ciphertext = AES_ECB.AesEncryptor_Hex(body, terminfo.secretKeySy);
                string send = string.Format("{{\"host_code\":\"{0}\", \"ciphertext\":\"{1}\"}}", terminfo.host_code, ciphertext);

                RestClient client = new RestClient();
                client.EndPoint = terminfo.serverurl + "/card";
                client.Method = HttpVerb.POST;
                //client.ContentType = "application/x-www-form-urlencoded";
                client.ContentType = "application/json";
                client.PostData = send;
                recvString = client.MakeRequest();
                //Utility.WriteLog(send);
                //Utility.WriteLog(recvString);
                dynamic jobject1 = Newtonsoft.Json.Linq.JObject.Parse(recvString) as dynamic;
                string code = jobject1["code"].ToString();
                if (code == "0") return true;
                error = jobject1["message"].ToString();

                return false;
            }
            catch (Exception ex) {
                error = ex.Message;
                Utility.WriteLog(recvString);
                return false;
            }
        }

        static int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day)) {
                age--;
            }
            return age < 0 ? 0 : age;
        }
        static DateTime IdCardtoDate(string cardno)
        {
            try {
                if (string.IsNullOrWhiteSpace(cardno) || cardno.Length != 18) {
                    return new DateTime(2099, 1, 1);
                }
                else {
                    int year = Convert.ToInt32(cardno.Substring(6, 4));
                    int month = Convert.ToInt32(cardno.Substring(10, 2));
                    int day = Convert.ToInt32(cardno.Substring(12, 2));
                    return new DateTime(year, month, day);
                }
            }
            catch {
                return new DateTime(2099, 1, 1);
            }
        }
    }

    public class AES_ECB
    {
        /// <summary>
        /// AES 算法加密(ECB模式) 将明文加密，加密后进行base64编码，返回密文
        /// </summary>
        /// <param name="EncryptStr">明文</param>
        /// <param name="Key">密钥</param>
        /// <returns>加密后base64编码的密文</returns>
        public static string AesEncryptor_Base64(string EncryptStr, string Key)
        {
            try {
                //byte[] keyArray = Encoding.UTF8.GetBytes(Key);
                byte[] keyArray = Convert.FromBase64String(Key);
                byte[] toEncryptArray = Encoding.UTF8.GetBytes(EncryptStr);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex) {
                return null;
            }
        }

        /// <summary>
        /// AES 算法解密(ECB模式) 将密文base64解码进行解密，返回明文
        /// </summary>
        /// <param name="DecryptStr">密文</param>
        /// <param name="Key">密钥</param>
        /// <returns>明文</returns>
        public static string AesDecryptor_Base64(string DecryptStr, string Key)
        {
            try {
                //byte[] keyArray = Encoding.UTF8.GetBytes(Key);
                byte[] keyArray = Convert.FromBase64String(Key);
                byte[] toEncryptArray = Convert.FromBase64String(DecryptStr);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);//  UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex) {
                return null;
            }
        }

        /// <summary>
        ///AES 算法加密(ECB模式) 将明文加密，加密后进行Hex编码，返回密文
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后Hex编码的密文</returns>
        public static string AesEncryptor_Hex(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = StrToHexByte(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return ToHexString(resultArray);
        }

        /// <summary>
        ///AES 算法解密(ECB模式) 将密文Hex解码后进行解密，返回明文
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string AesDecryptor_Hex(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = StrToHexByte(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = StrToHexByte(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// byte数组Hex编码
        /// </summary>
        /// <param name="bytes">需要进行编码的byte[]</param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null) {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        /// <summary> 
        /// 字符串进行Hex解码(Hex.decodeHex())
        /// </summary> 
        /// <param name="hexString">需要进行解码的字符串</param> 
        /// <returns></returns> 
        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
