using SHBSS.InterfaceClass;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew;
            using (Mutex mutex = new Mutex(true, Application.ProductName, out createNew))
            {
                if (createNew) {
                    //byte[] byteCmd = System.Text.Encoding.Default.GetBytes("CvF");
                    if (Global.GetInstance().ReadConfig() != 0) {
                        MessageBox.Show("配置文件出错，请检查！");
                    }
                    if (TerminalInfo.GetInstance().ReadConfig() != 0) {
                        MessageBox.Show("配置文件出错，请检查！");
                    }
                    WxPicture.InitParam();
                    log4net.Config.XmlConfigurator.Configure();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //FrmSetting  FrmMain FrmSupplyLogin FrmCheck FrmPhone FrmPay FrmService
                    // FrmCompQuery  FrmPINChange  FrmConfig  FrmPINLogin
                    // FrmApplyLogin  FrmApplyCheck
                    Application.Run(new FrmMain());
                }
                else
                {
                    MessageBox.Show("应用程序已经在运行中...");
                    Thread.Sleep(1000);
                    System.Environment.Exit(0);
                }
            }
            
        }
    }
}
