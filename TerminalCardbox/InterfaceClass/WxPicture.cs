using CommenLib.HttpUtils;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SHBSS.InterfaceClass
{
    public class WxPicture
    {
        // 内网：http://inside.xy.xinda-tech.com/api  外网：http://api.xy.xinda-tech.com:9670/api
        static string serverurl { get; set; }
        static string host_code { get; set; } // 设备编码
        static string secretKeyAsy { get; set; } // 非对称加密密钥
        static string publicKey { get; set; } // 公钥
        static string filedir { get; set; } // 照片目录
        public static string filepath { get; set; } // 照片路径
        public static string error { get; set; }

        static string tempfile;

        public static void InitParam()
        {
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            serverurl = INIClass.IniReadValue("XINDATECH", "serverurl", iniPath);
            serverurl += "/card_image";
            host_code = INIClass.IniReadValue("XINDATECH", "host_code", iniPath);
            secretKeyAsy = INIClass.IniReadValue("XINDATECH", "secretKeyAsy", iniPath);
            string pemfile = AppDomain.CurrentDomain.BaseDirectory + "Data\\public.pem";
            string pKey = Utility.ReadData(pemfile);
            publicKey = pKey.Replace("-----BEGIN RSA PUBLIC KEY-----", "")
                .Replace("-----END RSA PUBLIC KEY-----", "");

            filedir = INIClass.IniReadValue("XINDATECH", "filedir", iniPath);

            tempfile = filedir + "\\temp\\picture.png";
            string dirTemp = Path.GetDirectoryName(tempfile);
            if (!Directory.Exists(dirTemp)) {
                Directory.CreateDirectory(dirTemp);
            }
        }

        public static bool DownLoadPicture(string ID)
        {
            string recvString;
            try {
                RestClient client = new RestClient();
                client.EndPoint = getSignature(ID);
                client.Method = HttpVerb.GET;
                client.ContentType = "multipart/form-data";
                recvString = client.MakeRequest();

                JObject obj = JObject.Parse(recvString);
                string status = obj["status"].ToString();
                if (!status.Equals("0")) {
                    Utility.WriteLog("DownLoadPicture: " + recvString);
                    error = "未获取照片信息";
                    return false;
                }
                filepath = filedir + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + ID + ".jpg";
                string dir = Path.GetDirectoryName(filepath);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                string resMag = obj["download_url"].ToString();
                WebClient mywebclient = new WebClient();
                mywebclient.DownloadFile(resMag, tempfile);

                GetPicThumbnail(tempfile, filepath, 358, 441, 95);
                return true;
            }
            catch (Exception ex) {
                error = ex.Message;
                return false;
            }
        }

        private static string getSignature(string ID)
        {
            Random random = new Random();
            string signatureMessage = ID + "_" + GetTimeStamp(System.DateTime.Now) + "_"
            + random.Next(100000, 999999) + "_" + secretKeyAsy;
            //            string strSecret = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCv4OcCtIjgWWNCWxy4sDkmycdy
            //RQkZlaWAE1lRENBBW8ip88iRA0BZZSnL29nQgPp2npeAQptjec+Ye/weE2sXXqR2
            //JGLM6wP6nAlvOdbI6I7u2sJ9zTlVLITWrb7jYAV+uVJQ6dgxE3RFVBEnpITLDpq7
            //Y98VvNpMU53v198BawIDAQAB";
            string signature = RSAPublicKey(publicKey, signatureMessage);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("host_code", host_code);
            dic.Add("signature", signature);
            string result = MakeGetUrl(serverurl, dic);
            return result;
        }
        private static int GetTimeStamp(DateTime dt)
        {
            DateTime date = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - date).TotalSeconds);
            return timeStamp;
        }

        private static string MakeGetUrl(string url, Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0) {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic) {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            return builder.ToString();
        }

        private static string RSAPublicKey(string publicKey, string content)
        {
            byte[] niddf = Convert.FromBase64String(publicKey);
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(niddf);
            string XML = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(XML);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherbytes).Replace("+", "%2B").Replace("=", "%3D").Replace("/", "%2F");
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="dHeight">高度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        static bool GetPicThumbnail(string sFile, string dFile, int dWidth, int dHeight, int flag)
        {
            Image iSource = Bitmap.FromFile(sFile);
            //System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            //System.Drawing.Image iSource = bitmap;
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth)) {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            ob.SetResolution(350, 350);
            Graphics g = Graphics.FromImage(ob);
            //g.Clear(Color.WhiteSmoke);
            g.Clear(Color.White);  //纯白背景
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++) {
                    if (arrayICI[x].FormatDescription.Equals("JPEG")) {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null) {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch {
                return false;
            }
            finally {
                iSource.Dispose();
                ob.Dispose();
            }
        }

    }
}
