using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmMain : Form
    {
        //FrmConfig fcdlg = new FrmConfig();
        FrmPswd fpwd = new FrmPswd();
        FrmSupplyLogin fs1 = new FrmSupplyLogin();
        FrmApplyLogin fs2 = new FrmApplyLogin();
        SSCTable ssc = SSCTable.getSSCTable();
        TerminalInfo terminfo = TerminalInfo.GetInstance();
        Global gb = Global.GetInstance();

        static public string g_soundDir; // 语音路径
        static public string g_debugFolder; // Debug路径
        string amTime;
        string pmTime;

        Thread tdStartRegist;  // 终端开机注册线程
        Thread tdRunRegist;    // 终端运行注册线程
        int nType;             // 注册类型
        static public bool g_isRegist = false; // 是否注册成功
        //bool b_exit = false;

        static public bool b_getPwdEnable = true; // 进入系统设置后、FrmApplyLogin活动时，b_getPwdEnable为false  从这两个页面回到主页，再true
        static public string sysPwd;

        static public int g_nBusiness = 0; // 1 补换卡  12 换照片补换卡  2 个人申领  3 综合查询  4  修改密码
        public FrmMain()
        {
            InitializeComponent();
            //FormMng.Init(this, fpwd);
            // 初始化配置文件
            g_debugFolder = AppDomain.CurrentDomain.BaseDirectory;
            SocialSecurityInfo.GetInstance().photopath = g_debugFolder + "pic\\photo.jpg";
            g_soundDir = g_debugFolder + "sound\\";
            string persLogDir = g_debugFolder + "PersLog";
            // 初始化加密机
            if (!Directory.Exists(persLogDir)) {
                Directory.CreateDirectory(persLogDir);
            }
            Jiamiji.SetLogDir(persLogDir);
            Jiamiji.InitTSParam(gb.JiamijiIP, gb.DevMac, gb.PsamNo, gb.MedicalID, gb.BankCode);
            KaGuan.GetSIService();

            terminfo.PrinterName = "Solid 510";
            terminfo.DistrctCode = gb.DISTRCT;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //ctrlLocation();

            //if (b_exit)
            //    Environment.Exit(0);

            FormMng.Init(this, fpwd);
            //CloseExplorer();

            tdStartRegist = new Thread(bootStarpRegister);
            tdStartRegist.Start();

            timer_Register.Enabled = true;
            GetRandTime();
        }
        private void ctrlLocation()
        {
            panel1.Location = new Point(301, 313);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        //开机第一次实现注册
        public void bootStarpRegister()
        {
            string logFolder = g_debugFolder + "Log";
            string peslogFolder = g_debugFolder + "PersLog";
            DeleteAgo10LogFile(logFolder);
            DeleteAgo10LogFile(peslogFolder);
            SSCSQL.Delete10Ago();

            //初始化设备 设备自检
            if (DevMng.DevInit() != 0) {
                Utility.WriteLog(DevMng.strError);
                MessageBox.Show(DevMng.strError);
                //Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };

            if (gb.CityName == "信阳市") {
                DoUploadConsum();
            }
            g_isRegist = true;
        }

        void CloseExplorer()
        {
            string str = @"taskkill /f /im explorer.exe";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(str + "&exit");
            p.StandardInput.AutoFlush = true;
            this.Focus();
        }

        void Fill_gdt_dynamic_bank()
        {
            FrmConfig.gdt_dynamic_bank.Rows.Clear();
            DataTable dtBanks = new DataTable();
            int nBanks = MDATASQL.getAllBank(ref dtBanks);
            if (nBanks > 0) {
                for (int i = 0; i < nBanks - 1; i++) {
                    FrmConfig.gdt_dynamic_bank.Rows.Add(dtBanks.Rows[i][0], MDATASQL.getBankName(dtBanks.Rows[i][0].ToString()));
                }
                if (dtBanks.Rows[nBanks - 1][0].ToString() != "99999") {
                    FrmConfig.gdt_dynamic_bank.Rows.Add(dtBanks.Rows[nBanks - 1][0], MDATASQL.getBankName(dtBanks.Rows[nBanks - 1][0].ToString()));
                }
            }
        }

        private void btMakeCard_Click(object sender, EventArgs e)
        {
            SocialSecurityInfo.GetInstance().Init();
            ssc.Init();
            if (!g_isRegist) return;
            this.Cursor = Cursors.WaitCursor;

            int nprinter = Solid510.getPrinterParam();
            if (nprinter == -1) {
                MessageBox.Show("两台打印机都不可用。");
                this.Cursor = Cursors.Default;
                return;
            }
            PrintParameter.GetInstance().nPrinter = nprinter;

            F5SAN f5san = F5SAN.GetInstance();
            if (!f5san.GetSensorStatus())
            {
                MessageBox.Show(f5san.strError);
                this.Cursor = Cursors.Default;
                return;
            }
            if (f5san.BoxStatus == F5SAN.BoxEmpty)
            {
                MessageBox.Show("卡盒无卡，请联系工作人员");
                this.Cursor = Cursors.Default;
                return;
            }

            g_nBusiness = 1;
            terminfo.maketype = TerminalInfo.Reissue;
            fs2.Init();
            FormMng.Show(this, fs2);
            this.Hide();
            this.Cursor = Cursors.Default;
        }

        private void btnApplyCard_Click(object sender, EventArgs e)
        {
            SocialSecurityInfo.GetInstance().Init();
            ssc.Init();
            this.Cursor = Cursors.WaitCursor;

            int nprinter = Solid510.getPrinterParam();
            if (nprinter == -1) {
                MessageBox.Show("两台打印机都不可用。");
                this.Cursor = Cursors.Default;
                return;
            }
            PrintParameter.GetInstance().nPrinter = nprinter;

            F5SAN f5san = F5SAN.GetInstance();
            if (!f5san.GetSensorStatus())
            {
                MessageBox.Show(f5san.strError);
                this.Cursor = Cursors.Default;
                return;
            }
            if (f5san.BoxStatus == F5SAN.BoxEmpty)
            {
                MessageBox.Show("卡盒无卡，请联系工作人员");
                this.Cursor = Cursors.Default;
                return;
            }

            g_nBusiness = 2;
            terminfo.maketype = TerminalInfo.New;
            fs2.Init();
            FormMng.Show(this, fs2);
            this.Hide();
            this.Cursor = Cursors.Default;
        }

        private void btn_get_card_Click(object sender, EventArgs e)
        {
            int siCount = MDATASQL.SelectSiCardAll();
            if(siCount == 0) {
                MessageBox.Show("卡盘无成品卡，请联系工作人员。");
                return;
            }

        }

        private void btnCompQuery_Click(object sender, EventArgs e)
        {
            SocialSecurityInfo.GetInstance().Init();
            g_nBusiness = 3;
            fs1.Init();
            FormMng.Show(this, fs1);
            //this.Hide();
        }

        private void btnChangePIN_Click(object sender, EventArgs e)
        {
            SocialSecurityInfo.GetInstance().Init();
            g_nBusiness = 4;
            fs1.Init();
            FormMng.Show(this, fs1);
            //pinloginDlg.Init();
            //FormMng.Show(this, pinloginDlg);
            //this.Hide();
        }
        

        /// <summary>
        /// 只保留最近10天的日志
        /// </summary>
        private void DeleteAgo10LogFile(string fileFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(fileFolder);
            FileInfo[] allFile = dir.GetFiles();
            int fileNum = allFile.Length;
            if(fileNum == 11) {
                new FileInfo(allFile[0].FullName).Attributes = FileAttributes.Normal;
                File.Delete(allFile[0].FullName);
                return;
            }
            if (fileNum <= 10) return;
            int nDele = fileNum - 10;
            for(int i = 0; i<nDele; i++) {
                new FileInfo(allFile[i].FullName).Attributes = FileAttributes.Normal;
                File.Delete(allFile[i].FullName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            Point mPoint = new Point(e.X, e.Y);

            int cc = 100;
            if (mPoint.X > this.Width - cc && mPoint.Y < cc) {
                sysPwd = Global.GetInstance().AdminPwd;
                b_getPwdEnable = false;
                fpwd.Show();
                fpwd.Init();
                //fcdlg.Show();
                //fcdlg.Init();
                //fcdlg.TopMost = true;
                //FrmSetting fs = new FrmSetting();
                //fs.Show();
            }
        }


        //获取09:00到11：00的一个时间和14：00到16：00的一个时间
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();
            TimeSpan ts = time1 - time2;
            //TimeSpan ts = new TimeSpan(time1.Minute - time2.Minute);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }

            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));
            return minTime.AddSeconds(i);
        }


        void GetRandTime()
        {
            DateTime currentTime = System.DateTime.Now;
            string defaultYear = currentTime.Year.ToString("d4");
            string defaultMonth = currentTime.Month.ToString("d2");
            string defaultDay = currentTime.Day.ToString("d2");
            string dateString09 = defaultYear + defaultMonth + defaultDay + "0900";
            DateTime dt09 = DateTime.ParseExact(dateString09, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            string dateString11 = defaultYear + defaultMonth + defaultDay + "1100";
            DateTime dt11 = DateTime.ParseExact(dateString11, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            DateTime am_dt = GetRandomTime(dt11, dt09);//随机得到早上9点到11的的某一个时间
            amTime = am_dt.ToString("HHmm");

            string dateString14 = defaultYear + defaultMonth + defaultDay + "1400";
            DateTime dt14 = DateTime.ParseExact(dateString14, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            string dateString16 = defaultYear + defaultMonth + defaultDay + "1600";
            DateTime dt16 = DateTime.ParseExact(dateString16, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            DateTime pm_dt = GetRandomTime(dt16, dt14);//随机得到下午2点到4的的某一个时间
            pmTime = pm_dt.ToString("HHmm");
        }

        //运行注册
        public void RunRegister()
        {
            int i = 0;
            int nFlag = -1;
            nFlag = Jiamiji.getActionData(nType);
            while (nFlag != 0) {
                Thread.Sleep(30000);
                nFlag = Jiamiji.getActionData(nType);
                i++;
                if (i == 6) {
                    Utility.WriteLog("运行注册失败!");
                    //g_isRegist = false;
                    return;
                }
            }
            g_isRegist = true;
        }
        // 注册定时器
        bool b_am = true, b_pm = true;
        private void timer_Register_Tick(object sender, EventArgs e)
        {
            string nowtime = DateTime.Now.ToString("HHmm");
            if (nowtime.Substring(0, 3) == amTime.Substring(0, 3) && b_am) {
                b_am = false;
                nType = 2;
                tdRunRegist = new Thread(RunRegister);
                tdRunRegist.Start();
            }
            if (nowtime.Substring(0, 3) == pmTime.Substring(0, 3) && b_pm) {
                b_pm = false;
                nType = 3;
                //timer_Register.Enabled = false;
                tdRunRegist = new Thread(RunRegister);
                tdRunRegist.Start();
            }
            if (nowtime.Substring(0, 2).CompareTo("18") >= 0) {
                timer_Register.Enabled = false;
            }
        }
        
        void DoUploadConsum()
        {
            terminfo.PrinterSN = gb.PrinterSN1;
            bool bOpenPrinter = false;
            bOpenPrinter = Solid510.OpenPrinter(terminfo.PrinterSN);
            if (bOpenPrinter) {
                terminfo.RibbonRemain = Solid510.Solid510_GetRibbonRemain();
            }
            else {
                Utility.WriteLog(Solid510.strError);
                terminfo.RibbonRemain = -1;
            }
            if (terminfo.PrinterSN != string.Empty) {
                if (!XindaTechApi.UpTerminalInfo()) {
                    Utility.WriteLog(XindaTechApi.error);
                }
            }
            terminfo.PrinterSN = gb.PrinterSN2;
            bOpenPrinter = false;
            bOpenPrinter = Solid510.OpenPrinter(terminfo.PrinterSN);
            if (bOpenPrinter) {
                terminfo.RibbonRemain = Solid510.Solid510_GetRibbonRemain();
            }
            else {
                Utility.WriteLog(Solid510.strError);
                terminfo.RibbonRemain = -1;
            }
            if (terminfo.PrinterSN != string.Empty) {
                if (!XindaTechApi.UpTerminalInfo()) {
                    Utility.WriteLog(XindaTechApi.error);
                }
            }
        }
    }
}
