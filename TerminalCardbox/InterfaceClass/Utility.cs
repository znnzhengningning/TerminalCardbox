/*************************************************************************
*  @brief：杂项类
*  日志文件写入、数据文件写入
*  父子窗口显示
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace SHBSS
{
    class Utility
    {
        [DllImport("kernel32.dll")]
        static extern uint GetTickCount();

        public static void Delay(uint ms)
        {
            uint start = GetTickCount();
            while (GetTickCount() - start < ms)
            {
                Application.DoEvents();
            }
        }

        public static log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static void WriteLog(string info)
        {
            if (info == "" || info == "{}" || info == null) return;
            if (loginfo.IsInfoEnabled) {
                loginfo.Info(info);
            }
        }
        
        public static void WriteData(string msg, string filename)
        {
            string datapath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            if (!Directory.Exists(datapath)) {
                Directory.CreateDirectory(datapath);
            }
            string filePath = datapath + filename;
            try
            {
                // 追加写入
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(msg);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException e)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "     异常：" + e.Message);
                    sw.WriteLine("**************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public static void WritePayRet(string msg, string filename)
        {
            string datapath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            if (!Directory.Exists(datapath)) {
                Directory.CreateDirectory(datapath);
            }
            string filePath = datapath + filename;
            try {
                // 覆盖写入
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);              
                sw.WriteLine(msg);
                sw.Flush();
                sw.Close();
                fs.Close();
                fs.Dispose();
            }
            catch (IOException e) {
                using (StreamWriter sw = File.AppendText(filePath)) {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "     异常：" + e.Message);
                    sw.WriteLine("**************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public static string ReadData(string filePath)
        {
            string msg = "";
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            msg = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return msg;
        }

    }

    class FormMng
    {
        static Form _curfrm = null;
        static List<Form> frmlist = new List<Form>();
        static Form _setfrm = null;
        public static void Init(Form mainfrm, Form settingform)
        {
            _curfrm = mainfrm;
            frmlist.Clear();
            frmlist.Insert(0, mainfrm);

            _setfrm = settingform;
        }

        public static Form GetSettingForm()
        {
            return _setfrm;
        }

        public static void Show(Form srcfrm, Form desfrm)
        {
            frmlist.Insert(0, desfrm);

            if (_curfrm.InvokeRequired)
            {
                _curfrm.BeginInvoke((MethodInvoker)delegate ()
                {
                    _curfrm.Hide();
                });
                
            }
            else
                _curfrm.Hide();

            if (desfrm.InvokeRequired)
            {
                desfrm.BeginInvoke((MethodInvoker)delegate ()
                {
                    desfrm.Show();
                });

            }
            else
                desfrm.Show();
            _curfrm = desfrm;
        }
        public static void ShowCurFrm()
        {
            if (_curfrm == null)
                return;
            if (_curfrm.InvokeRequired)
            {
                _curfrm.BeginInvoke((MethodInvoker)delegate ()
                {
                    _curfrm.Show();
                });

            }
            else
                _curfrm.Show();
        }

        public static void ShowMain(Form curform)
        {
            if (curform == null)
                return;

            if (_curfrm == null)
                return;
            if (_curfrm.InvokeRequired)
            {
                _curfrm.BeginInvoke((MethodInvoker)delegate ()
                {
                    _curfrm.Hide();
                });

            }
            else
                _curfrm.Hide();
            

            if (frmlist.Count == 0)
                return;

            int index = frmlist.Count;
            _curfrm = frmlist[index-1];

            ShowCurFrm();
              
        }

        public static void Hide(Form srcfrm)
        {
            srcfrm.Hide();
            if (frmlist.Count != 0)
            {
                frmlist.RemoveAt(0);
                if (frmlist.Count != 0)
                {
                    _curfrm = frmlist[0];
                    _curfrm.Show();
                }
            }
            
        }
    }

    
}
