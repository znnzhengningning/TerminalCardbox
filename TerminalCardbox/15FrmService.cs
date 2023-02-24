using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Threading;
using CommenLib.MP3;

namespace SHBSS
{
    public partial class FrmService : Form
    {
        SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
        PrintParameter printerParam = PrintParameter.GetInstance();

        F5SAN f5san = F5SAN.GetInstance();

        Thread persThread;            // 个人化线程    
        string newBatch = "";         // 批次号
        bool timerFlag = false;       // 进入制成卡出卡定时器标志
        int nCountDown = 120;         // 界面倒计时
        int nDJS2 = 10;               // 制卡后倒计时返回主页
        string g_strErrMsg = "";      // 错误信息

        bool isCardInPrinter = false; // 卡片是否在打印机里
        //bool isCardInReader = false;  // 卡片是否在读写器里

        string ExceptionTaitou = "FrmService Exception - ";

        SSCTable ssc = SSCTable.getSSCTable();
        public FrmService()
        {
            InitializeComponent();
        }

        private void ctrlLocation()
        {
            label_CardState.Location = new Point(130, 252);
            lblError.Location = new Point(183, 715);
            btnReturn.Location = new Point(680, 860);
            lblCountDown.Location = new Point(977, 842);
            
            lblName.Location = new Point(818, 416);
            lblSID.Location = new Point(887, 441);
            lblCID.Location = new Point(887, 466);
            lblDate.Location = new Point(855, 492);
            picphoto.Location = new Point(656, 390);
        }

        private void FrmService_Load(object sender, EventArgs e)
        {
            ctrlLocation();
            InitUI();
            persThread = new Thread(MakeCard);
            persThread.Start();
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
        //public void Init()
        //{
        //    //feedcount = 0;
        //    Utility.WriteLog(ssi.photopath);
        //    ssi.photo.Save(ssi.photopath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //}
        private void InitUI()
        {
            btnReturn.Visible = false;
            lblError.Text = "";  //大错特错
            lblName.Text = "";   //黄敏华
            lblCID.Text = "";    //
            lblSID.Text = "";    //Z12345678
            lblDate.Text = "";   //20201219
            picphoto.BackgroundImage = null;
            timer1.Enabled = true;
            lblCountDown.Text = nCountDown.ToString("00");
            label_CardState.Text = "准备制卡...";
            //picphoto.BackgroundImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "pic\\test.jpg");
        }

        public void MakeCard()
        {
            timerFlag = false;
            isCardInPrinter = false;
            //isCardInReader = false;
            try {
                //Utility.WriteLog(ssi.photopath);
                //ssi.photo.Save(ssi.photopath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //Thread.Sleep(50);

                int nRet = -2;
                ssi.mdbQryResult = SSCSQL.Query();
                // 进卡
                if ((nRet = FeedCard()) != 0)
                {
                    if (ssi.mdbQryResult == "")
                    {
                        if (FrmMain.g_nBusiness == 1)
                            InsertnPro(0);
                        else
                            InsertnPro(1);
                    }
                    if (nRet  == 2)
                    {
                        Utility.WriteLog("小车到回收箱");
                        f5san.car2recycle(); // 小车到回收箱
                    }
                    return;
                }
                isCardInPrinter = true;
                Thread.Sleep(Global.SleepTime);

                // 检查卡片上电情况
                Utility.WriteLog("检查卡片上电");
                int nPort;
                if (printerParam.nPrinter == 1) {
                    nPort = Global.GetInstance().RWPort1;
                }
                else {
                    nPort = Global.GetInstance().RWPort2;
                }
                if (SiCard.CheckICReader(nPort) != 0) {
                    g_strErrMsg = SiCard.error;
                    return;
                }

                Utility.WriteLog("补换照片");
                if (ssi.mdbQryResult == "" && FrmMain.g_nBusiness == 1) {
                    if (ssi.b_replacephoto) {
                        // 补换卡换照片
                        if (KaGuan.xgryzp()) {
                            // 换照片成功后，重新标记制卡进度
                            InsertnPro(1);
                        }
                        else {
                            g_strErrMsg = KaGuan.g_strErrMsg;
                            return;
                        }

                    }
                    //else {
                    //    ssi.photopath = FrmMain.g_debugFolder + "pic\\photo.jpg";
                    //    ssi.photo.Save(ssi.photopath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //}
                }
                Utility.WriteLog("获取数据");

                // 获取数据
                if (( nRet = GetCardInfo() ) != 0) {
                    g_strErrMsg = KaGuan.g_strErrMsg;
                    if (nRet == -1)
                        InsertnPro(0);
                    else
                        InsertnPro(nRet);
                    return;
                }

                ssi.photopath = FrmMain.g_debugFolder + "pic\\photo.jpg";
                ssi.photo.Save(ssi.photopath, System.Drawing.Imaging.ImageFormat.Jpeg);

                ssc.SSCard = ssi.securityNo;
                InsertnPro(5);
                Utility.WriteLog("个人化");
                // 个人化
                if (Personalization() != 0) return;

                Utility.WriteLog("打印");
                // 打印
                if (PrintCard() != 0) return;

                Utility.WriteLog("数据回盘");
                //数据回盘
                if (DataBack() != 0) return;

                InsertnPro(0);

                this.Invoke(new FrmShowDialog(FrmShowDialog_F));

                // 打印机到读写器
                if (Printer2Reader() != 0) {
                    //InsertnPro(5);
                    if (nRet == 2) {
                        isCardInPrinter = false;
                        Utility.WriteLog("小车到回收箱");
                        f5san.car2recycle(); // 小车到回收箱
                    }
                    return;
                }
                isCardInPrinter = false;
                //isCardInReader = true;

                // 出卡
                if (OutCard() != 0) return;

                //isCardInReader = false;
                //U10.ContrlLight(U10.U10_LIGHT_4, U10.U10_CTRL_ON);
                timerFlag = true;
            }
            catch (Exception e) {
                timerFlag = false;
                Utility.WriteLog(ExceptionTaitou + "MakeCard: " + e.Message);
                BeginInvoke((MethodInvoker)delegate () {lblError.Text = e.Message; });
            }
            finally {
                DoFinally();
            }
        }

        delegate void FrmShowDialog();
        void FrmShowDialog_F()
        {
            FrmDlgPIN pinDlg = new FrmDlgPIN();
            pinDlg.ShowDialog();
        }
        void InsertnPro(int n)
        {
            ssc.nPro = n;
            ssc.Error = g_strErrMsg;
            SSCSQL.Insert();
        }

        private void DoFinally()
        {
            DateTime dtNow = DateTime.Now;            
            
            ssc.MakeTime = dtNow.ToString("yyyy-MM-dd HH:mm:ss");
            nCountDown = nDJS2;
            SSCSQL.InsertCardsRec();

            if (timerFlag)
            {
                if (Global.GetInstance().CityName == "信阳市") {
                    if (!XindaTechApi.UpCardInfo()) {
                        Utility.WriteLog("上传制卡数据失败：" + XindaTechApi.error);
                    }
                }
                int nRet = Jiamiji.UploadCardData(ssi.ID, ssi.securityNo, ssi.ownerName);
                if (nRet != 0) {
                    Utility.WriteLog("上传制卡数据接口失败！");
                }
                SSCSQL.Delete();
                Utility.WriteLog("删除进度数据！");
            }
            else {
                BeginInvoke((MethodInvoker)delegate () { label_CardState.Text = "不能制卡"; });
                Utility.WriteLog(g_strErrMsg);
                ssc.Error = g_strErrMsg;
                // 出卡到废卡槽
                if (isCardInPrinter) {
                    Utility.WriteLog("卡片从打印机到回收箱");
                    f5san.printer2car(printerParam.nPrinter);
                    f5san.car2recycle();
                }
                //if (isCardInReader) {
                //    Utility.WriteLog("卡片从读写器到回收箱");
                //    f8san.reader2car();
                //    f8san.car2recycle();
                //}
            }
        }

        int OutCard()
        {
            Utility.WriteLog("卡片从读写器弹出");
            if (f5san.reader2out())
            {
                //nCountDown = nDJS2;
                MP3Player.Play(FrmMain.g_soundDir + "10Done.mp3");
                BeginInvoke((MethodInvoker)delegate () {
                    label_CardState.Text = "制卡成功";
                    lblError.Text = "请从下方卡槽取走您的社保卡。";
                    btnReturn.Visible = true;
                });
                return 0;
            }
            else
            {
                g_strErrMsg = f5san.strError;
                BeginInvoke((MethodInvoker)delegate (){lblError.Text = f5san.strError;});
                return -1;
            }
        }
        private bool PayRegister()
        {
            string RecvData = "";
            string SendDataXML = string.Format("<操作*>交费登记</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*><城市代码>{4}</城市代码*>",
                                KaGuan.USER, KaGuan.PSWD, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);
            if (!KaGuan.CallDsjk(SendDataXML, ref RecvData)) {
                g_strErrMsg = "缴费登记失败：" + KaGuan.g_strErrMsg;
                lblError.Text = g_strErrMsg;
                return false;
            }
            return true;
        }

        private bool CheckCardStatus()
        {
            int nRet = 0;
            if ((nRet = KaGuan.CheckCard()) != 0) {
                g_strErrMsg = KaGuan.g_strErrMsg;
                if (nRet == 1) {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = "您的卡未挂失，请到前台挂失"; });
                    return false;
                }
                else {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = "您的卡未缴费，不能制卡"; });
                    return false;
                }
            }
            BeginInvoke((MethodInvoker)delegate () { lblError.Text = "检查卡片完毕"; });
            return true;
        }

        int FeedCard()
        {
            bool bRet = false;
            // 卡盘到小车 
            Utility.WriteLog("卡盒到小车");
            bRet = f5san.box2car();
            if (!bRet) { BeginInvoke((MethodInvoker)delegate () { lblError.Text = f5san.strError; }); return 1; }
            Thread.Sleep(50);

            //小车到打印机位置
            Utility.WriteLog("小车到打印机位置");
            bRet = f5san.car2printer(printerParam.nPrinter);
            if (!bRet) { BeginInvoke((MethodInvoker)delegate () { lblError.Text = f5san.strError; }); return 2; }
            Thread.Sleep(50);
            
            return 0;
        }

        int Printer2Reader()
        {
            bool bRet = false;
            // 打印机到小车 
            Utility.WriteLog("打印机到小车");
            bRet = f5san.printer2car(printerParam.nPrinter);
            if (!bRet) { BeginInvoke((MethodInvoker)delegate () { lblError.Text = f5san.strError; }); return 1; }
            Thread.Sleep(50);

            //小车到读写器
            Utility.WriteLog("小车到读写器");
            bRet = f5san.car2reader();
            if (!bRet) { BeginInvoke((MethodInvoker)delegate () { lblError.Text = f5san.strError; }); return 2; }
            Thread.Sleep(50);

            return 0;
        }

        /// <summary>
        /// 检查IC模块是否可用
        /// </summary>
        /// <returns></returns>
        //private bool CheckICReader()
        //{
        //    StringBuilder sb = new StringBuilder(50);
        //    int nRt = SiCard.ICCOpenDevice(Global.GetInstance().nPort, sb);
        //    if (nRt != 0){
        //        g_strErrMsg = "设备连接失败!";
        //        BeginInvoke((MethodInvoker)delegate () { lblError.Text = g_strErrMsg; });
        //        return false;
        //    }
        //    byte[] ATR = new byte[30];
        //    nRt = SiCard.ICCPowerOn(0x01, 0x01, ATR);

        //    if (nRt <= 0)
        //    {
        //        g_strErrMsg = "接触上电失败!";
        //        BeginInvoke((MethodInvoker)delegate () { lblError.Text = g_strErrMsg; });
        //        return false;
        //    }
        //    return true;
        //}
        
        public int GetCardInfo()
        {
            BeginInvoke((MethodInvoker)delegate () { lblError.Text = "正在获取数据"; });

            int ret = KaGuan.QryCardInfo(ref newBatch);

            if (ret != 0) {
                BeginInvoke((MethodInvoker)delegate () { lblError.Text = KaGuan.g_strErrMsg; });
                g_strErrMsg = KaGuan.g_strErrMsg;
                return ret;
            }
            BeginInvoke((MethodInvoker)delegate ()
            {
                label_CardState.Text = "正在制卡...";
                lblError.Text = "获取数据成功，正在个人化...";
                lblName.Text = ssi.ownerName;
                lblSID.Text = ssi.ID;
                lblCID.Text = ssi.securityNo;
                lblDate.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                picphoto.BackgroundImage = ssi.photo;
            });
            MP3Player.Play(FrmMain.g_soundDir + "9MadingCard.mp3");
            return 0;
        }

        private int Personalization()
        {
            int nPort;
            if (printerParam.nPrinter == 1) {
                nPort = Global.GetInstance().RWPort1;
            }
            else{
                nPort = Global.GetInstance().RWPort2;
            }
            StringBuilder pOutputInfo = new StringBuilder(600);
            StringBuilder pErrMsg = new StringBuilder(600);
            int ret = SiCard.Print_ReadCard(nPort, SiCard.TP_CONTACT, pOutputInfo, pErrMsg);
            if (ret != 0) {
                Utility.WriteLog("卡片读取返回值:" + ret.ToString());
                BeginInvoke((MethodInvoker)delegate () { lblError.Text = "卡片读取失败"; });
                g_strErrMsg = pErrMsg.ToString();
                return ret;
            }
            Utility.WriteLog(pOutputInfo.ToString());

            string[] infos = pOutputInfo.ToString().Split('|');
            ssi.cardverson = infos[8];
            ssi.cardIdentify = infos[3];
            ssi.cardReset = infos[7];
            ssi.cardagent = infos[7].Substring(18, 6);
            ssi.CA = infos[10];

            StringBuilder pErrMsg2 = new StringBuilder(600);
            string pWrinteInfo = "";
            if (ssi.cardverson == "2.00")
            {
                pWrinteInfo = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|2.00|CA|",
                    ssi.ID, ssi.ownerName, ssi.sex, ssi.nation, ssi.birthday, ssi.makeDate, ssi.valid, ssi.securityNo, ssi.cardReset, ssi.cardIdentify);
            }
            else
            {
                pWrinteInfo = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|3.00|{10}|",
                    ssi.ID, ssi.ownerName, ssi.sex, ssi.nation, ssi.birthday, ssi.makeDate, ssi.valid, ssi.securityNo, ssi.cardReset, ssi.cardIdentify, ssi.CA);
            }
            ret = SiCard.Print_WriteCard(nPort, SiCard.TP_CONTACT, pWrinteInfo, pErrMsg2);
            if (ret != 0)
            {
                Utility.WriteLog("个人化返回值:" + ret.ToString());
                BeginInvoke((MethodInvoker)delegate () { lblError.Text = "卡片个人化失败"; });
                g_strErrMsg = pErrMsg2.ToString();
                return ret;
            }

            return 0;
        }

        private int PrintCard()
        {
            // 打印
            Utility.WriteLog("打印");
            Thread.Sleep(1500);
            if (Solid510.PrintCard(ssi.ownerName, ssi.ID, ssi.securityNo, ssi.photopath) != 0) {
                BeginInvoke((MethodInvoker)delegate () { lblError.Text = Solid510.strError; });
                return -1;
            }
            return 0;
        }

        void ShowPrinterErr()
        {
            StringBuilder errBuff = new StringBuilder(500);
            int buflen = 500;
            Solid510.Solid510_GetErr(errBuff, ref buflen);
            g_strErrMsg = errBuff.ToString();
            BeginInvoke((MethodInvoker)delegate () { lblError.Text = g_strErrMsg; });
        }

        //bool isDataBack = true;
        private int DataBack()
        {
            BeginInvoke((MethodInvoker)delegate () {
                lblError.Text = "\n\r写卡成功，正在回盘...\n";
            });

            string SendDataXML = string.Format("<操作*>即制卡回盘</操作*><用户名*>{9}</用户名*><密码*>{10}</密码*><批次号*>{8}</批次号*><人数*>1</人数*>\r\n"
                + "序号,社保卡号,社会保障号码,姓名,制卡日期,有效期至,卡识别码,卡复位码,个人帐户,银行卡号,金融帐户,装箱位置,失败环节,失败原因\r\n"
                + "1,{0},{1},{2},{3},{4},{5},{6},个人帐户1,{7},金融帐户1,1-1-1-1,,",
                ssi.securityNo, ssi.ID, ssi.ownerName, ssi.makeDate,
                ssi.valid, ssi.cardIdentify, ssi.cardReset, "", newBatch, KaGuan.USER, KaGuan.PSWD);
            string RecvData = "";
            if (!KaGuan.CallDsjk(SendDataXML, ref RecvData)) {
                BeginInvoke((MethodInvoker)delegate () {
                    lblError.Text = "回盘失败: " + KaGuan.g_strErrMsg;
                    g_strErrMsg = "回盘失败: " + KaGuan.g_strErrMsg;
                });
                return -1;
            }

            Utility.WriteLog("回盘成功。");
            BeginInvoke((MethodInvoker)delegate ()
            {
                lblError.Text = "回盘成功，正在领卡启用";
            });

            SendDataXML = string.Format("<操作*>领卡启用</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><社会保障卡卡号*>{3}</社会保障卡卡号*><社会保障号码*>{4}</社会保障号码*><姓名*>{5}</姓名*>",
                KaGuan.USER, KaGuan.PSWD, KaGuan.CITYCODE, ssi.securityNo, ssi.ID, ssi.ownerName);
            if (!KaGuan.CallDsjk(SendDataXML, ref RecvData)) {
                BeginInvoke((MethodInvoker)delegate () {
                    lblError.Text = "领卡启用失败: " + KaGuan.g_strErrMsg;
                    g_strErrMsg = "领卡启用失败: " + KaGuan.g_strErrMsg;
                });
                return -1;
            }
            BeginInvoke((MethodInvoker)delegate ()
            {
                lblError.Text = "领卡启用成功";
            });

            return 0;
        }
        //private int Syssinfo(string id, string name)
        //{
        //    //MIUnitInfo ui = MedicalEnquiry.UnitInfo;  //医保单位信息
        //    //string updateInfo = "";
        //    //string SendDataXML = string.Format("<操作*>人员信息修改</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><证件号码*>{2}</证件号码*><姓名*>{3}</姓名*><性别></性别><移动电话></移动电话><通讯地址></通讯地址><邮政编码></邮政编码><电子邮箱></电子邮箱><单位编号>{4}</单位编号><单位名称>{5}</单位名称><所在社区(村)></所在社区(村)>",
        //    //    KaGuan.USER, KaGuan.PSWD, ssi.ID, ssi.ownerName, ui.UnitCode, ui.UnitName);
        //    ////Utility.WriteLog("单位名称：" + ui.UnitCode);
        //    //if (!KaGuan.CallDsjk(SendDataXML, ref updateInfo)) {
        //    //    BeginInvoke((MethodInvoker)delegate () { label_CardState.Text = "已制卡，同步失败"; lblError.Text = KaGuan.g_strErrMsg; });
        //    //    return -1;
        //    //}
        //    //return 0;

        //}


        int exitcount = 0;
        private void FrmService_MouseDown(object sender, MouseEventArgs e)
        {
            //Point mPoint = new Point(e.X, e.Y);

            //int cc = 100;
            //if (mPoint.X < cc && mPoint.Y < cc) {
            //    exitcount = 1;
            //}
            //else if (mPoint.X > this.Width - cc && mPoint.Y < cc) {
            //    if (exitcount == 1)
            //        exitcount = 2;
            //    else
            //        exitcount = 0;
            //}
            //else if (mPoint.X > this.Width - cc && mPoint.Y > this.Height - cc) {
            //    if (exitcount == 2)
            //        exitcount = 3;
            //    else
            //        exitcount = 0;
            //}
            //else if (mPoint.X < cc && mPoint.Y > this.Height - cc) {
            //    if (exitcount == 3)
            //        exitcount = 4;
            //    else
            //        exitcount = 0;
            //}

            //if (exitcount == 4) {
            //    FormMng.GetSettingForm().ShowDialog(this);
            //}
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            //U10.ContrlLight(U10.U10_LIGHT_4, U10.U10_CTRL_OFF);
            timer1.Enabled = false;
            //FormMng.Hide(this);
            FormMng.ShowMain(this);
            this.Close();
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nCountDown > 0) {
                nCountDown--;
                string strcount = nCountDown.ToString("00");
                lblCountDown.Text = nCountDown.ToString("00");
            }
            else {
                btnReturn_Click(sender, e);
            }
        }
    }
}
