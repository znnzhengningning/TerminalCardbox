using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using CommenLib.MP3;
using CommenLib.QrCode;
using FSJF;

namespace SHBSS
{
    public partial class FrmPay : Form
    {
        Thread payThread;           // 支付线程
        Thread thread2;             // 等待客户看完回执单线程
        bool isEndth2;              // 是否结束thread2线程
        bool ispayfun;              // 是否结束支付线程
        int nTicket = 210;          // 给客户扫码支付的时间（s）
        int nDJS = 60;              // 支付查询倒计时
        int nTh2DJS = 5;            // 线程2倒计时
        Untax pay;                    // 缴费接口类
        SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
        bool isPay = false;

        string ExceptionTaitou = "FrmPay Exception - ";

        SSCTable ssc = SSCTable.getSSCTable();
        //string mdbQryResult = ""; // 数据库查询结果
        public FrmPay()
        {
            InitializeComponent();
            ctrlLocation();
        }

        private void FrmPay_Load(object sender, EventArgs e)
        {
            //this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            //string logofile = FrmMain.g_debugFolder + "pic\\支付宝LOGO.jpg";
            //string QrData = "http://125.46.249.117:8001/FSPayPlatform/payinfo/pay?data=eyJ0cmFuc3RpbWUiOiIyMDE5LTAxLTExIDEzOjI0OjQ1IiwicGF5Q29kZSI6IjQxMDAwMDE5MDAwMDAwOTk3MDgyIiwiZGV2aWNlVHlwZSI6IjIiLCJub3RpZnlfVVJMIjoiaHR0cDovLzIyMi4xNDMuMzQuMTgvSG5QdWJsaWNTZXJ2aWNlQ29uc29sZS9wYXlSZXN1bHQvc3luY1BheVJlc3VsdDIubXZjIn0&src=RSTSBK&noise=1547184285286&sign=shmN9036qL3KEePxJ5SdjjZrq408Bk-muFDJ0f3sSdCJwGuDfCnAqSEvsiaod2vNOw-39QqlvorPhMP_2UA--Q";
            //Bitmap qrCode = QrLogo.CreateQrLogo(QrData, AppDomain.CurrentDomain.BaseDirectory + logofile, 1.35);
            //pictureBox1.Image = qrCode;
            //lbl_25yuan.Visible = true;
            //btn_next.Visible = true;


            Init();
            payThread = new Thread(PayFun);
            payThread.Start();
            thread2 = new Thread(Timer2);

        }
        private void ctrlLocation()
        {
            label_Pay.Location = new Point(130, 252);
            lbl_error.Location = new Point(156, 460);
            pictureBox1.Location = new Point(650, 330);
            pictureBox1.Size = new Size(400,400);
            lbl_25yuan.Location = new Point(670, 730);
            btn_cancel.Location = new Point(650, 922);
            btn_next.Location = new Point(650, 922);
            lbl_time.Location = new Point(980, 905);
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
        private void Init()
        {
            //mdbQryResult = "";
            timer1.Enabled = false;
            lbl_25yuan.Visible = false;
            isEndth2 = false;
            ispayfun = false;
            btn_next.Visible = false;
            btn_cancel.Visible = false;
            lbl_time.Visible = false;
            pay = new Untax();
            pay.Init(ssi.ID, ssi.phone, ssi.ownerName, ssi.securityOldNo);

            ssc.IDCard = ssi.ID;
            ssc.Name = ssi.ownerName;
            ssc.Sex = ssi.sex;
            ssc.SSCard = ssi.securityOldNo;
            ssc.Phone = ssi.phone;

            //ssi.ID = "450802199104181282";
            //ssi.ownerName = "haungminhau";
            //SetPictureBox1();
        }
        public void Timer2()
        {
            if (!isPay) {
                BeginInvoke((MethodInvoker)delegate () {
                    pictureBox1.Image = null;
                });
            }
            for (int i = nTh2DJS; i > 0; i--) {
                if (isEndth2) return;
                BeginInvoke((MethodInvoker)delegate () {
                    lbl_time.Text = i.ToString("00");
                });
                Thread.Sleep(1000);
            }
            if (isPay) {
                BeginInvoke((MethodInvoker)delegate () {
                    LoginMakeCard();
                });
            }
            else {
                BeginInvoke((MethodInvoker)delegate () {
                    FormMng.ShowMain(this);
                    this.Close();
                });
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            isEndth2 = true;
            thread2.Join();
            LoginMakeCard();
        }

        void LoginMakeCard()
        {
            //U10.ContrlLight(U10.U10_LIGHT_3, U10.U10_CTRL_OFF);
            if (FrmMain.g_nBusiness == 12)
            {
                ssc.nPro = 1;
            }
            FrmService fs = new FrmService();
            //fs.Init();
            FormMng.Show(this, fs);
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FormMng.ShowMain(this);
            ispayfun = true;
            isEndth2 = true;
            if (payThread != null && payThread.ThreadState != ThreadState.Unstarted)
            {
                if ((ThreadState.WaitSleepJoin | ThreadState.Background) == (payThread.ThreadState))
                {
                    payThread.Interrupt();
                }
                payThread.Join();
            }
            if (thread2 != null && thread2.ThreadState != ThreadState.Unstarted)
            {
                if ((ThreadState.WaitSleepJoin | ThreadState.Background) == (thread2.ThreadState))
                {
                    thread2.Interrupt();
                }
                thread2.Join();
            }
            this.Close();
        }

        public void PayFun()
        {
            bool isqrCodeSuccess = false;
            try
            {
                // 挂失社保卡
                // 播放挂失语音1
                MP3Player.Play(FrmMain.g_soundDir + "5LossCard.mp3");
                //mdbQryResult = SSCSQL.Query();
                if (FrmMain.g_nBusiness == 1) {
                    if (!LossCard()) return;
                }
                
                // 播放挂失语音2
                MP3Player.Play(FrmMain.g_soundDir + "6LossSuccess.mp3");
                //if (!PayRegister()) return;
                // 查询是否已缴费                
                if (QueryRegister())
                {
                    isqrCodeSuccess = true;
                    MP3Player.Play(FrmMain.g_soundDir + "7Paied.mp3");
                    BeginInvoke((MethodInvoker)delegate ()
                    {
                        lbl_error.Text = "您已缴费，接下来为您制卡，请稍候";
                        Thread.Sleep(1000);
                        FrmService fs = new FrmService();
                        //fs.Init();
                        FormMng.Show(this, fs);
                        this.Close();
                    });
                    return;
                }
                Bitmap qrCode = pay.syncPayinfo();
                if (qrCode != null) {
                    isqrCodeSuccess = true;
                    BeginInvoke((MethodInvoker)delegate () {
                        lbl_25yuan.Visible = true;
                        pictureBox1.Image = qrCode;
                        lbl_error.Text = "请使用（微信/支付宝）扫描二维码进行支付";
                        btn_cancel.Visible = true;
                        timer1.Enabled = true;
                        lbl_time.Visible = true;
                    });
                    // 查询支付结果
                    while (true) {
                        if (ispayfun) return;
                        Thread.Sleep(3000);
                        if (QueryPayResult()) return;
                    }
                }
                else {
                    if(pay.errMsg == "Timeout") {
                        BeginInvoke((MethodInvoker)delegate () {
                            lbl_error.Text = "非税缴费服务器已断开，请稍后重试。";
                        });
                    }
                    else {
                        BeginInvoke((MethodInvoker)delegate () {
                            lbl_error.Text = "错误信息：" + pay.errMsg;
                        });
                    }
                    Utility.WriteLog("PayFee错误信息：" + pay.errMsg);
                }
            }
            catch (Exception ex) {
                BeginInvoke((MethodInvoker)delegate () {
                    lbl_error.Text = "错误信息：" + ex.Message;
                });
                Utility.WriteLog(ExceptionTaitou + "PayFun: " + ex.Message);
            }
            finally {
                if (!isqrCodeSuccess) {
                    isPay = false;
                    BeginInvoke((MethodInvoker)delegate () {
                        timer1.Enabled = false;
                        lbl_25yuan.Visible = false;
                        btn_cancel.Visible = true;
                        btn_next.Visible = false;
                        lbl_time.Visible = true;
                    });
                    thread2.Start();
                }
            }           
        }

        bool QueryPayResult()
        {
            int nRet = pay.queryPayresult();
            if (nRet == 0)
            {
                Utility.WriteLog("支付成功。");
                pay.cfmPayresult();

                ispayfun = true;  // 结束支付进程
                ssc.PayCode = pay.payCode;
                ssc.Transtime = pay.transtime;
                ssc.nPro = 0;
                if (!PayRegister())
                {
                    thread2.Start();
                    return false;
                }
                isPay = true;
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_error.Text = "支付完成，请拿好您的回执单 ";
                    timer1.Enabled = false;
                    lbl_25yuan.Visible = false;
                    btn_cancel.Visible = false;
                    btn_next.Visible = true;
                });
                thread2.Start();

                MP3Player.Play(FrmMain.g_soundDir + "8PaySuccess.mp3");
                BeginInvoke((MethodInvoker)delegate ()
                {
                    SetPictureBox1();
                    this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
                    pictureBox1.Refresh();
                });

                Msprint.PrintTicket(ssi.ownerName, ssi.ID, pay.payCode);
                //U10.ContrlLight(U10.U10_LIGHT_3, U10.U10_CTRL_ON);
                return true;
            }
            else if (nRet == 2) {
                if (nTicket < nDJS) {
                    BeginInvoke((MethodInvoker)delegate () {
                        lbl_error.Text = pay.errMsg;
                    });
                    Utility.WriteLog(pay.errMsg);
                }
            }
            else if(nRet == 101) {
                ispayfun = true;
                thread2.Start();
                MP3Player.Play(FrmMain.g_soundDir + "7ServerBreakup.mp3");
                BeginInvoke((MethodInvoker)delegate () {
                    lbl_error.Text = "服务器已断开，请勿缴费";
                    pictureBox1.Image = null;
                    lbl_25yuan.Visible = false;
                    btn_cancel.Visible = true;
                    btn_next.Visible = false;
                });
            }
            else {
                string.Format("QryPayResult nRet = {0},{1}", nRet, pay.errMsg);
                Utility.WriteLog(pay.errMsg);
            }
            return false;
        }

        private bool PayRegister()
        {
            string RecvData = "";
            string SendDataXML = string.Format("<操作*>交费登记</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*><城市代码>{4}</城市代码*>",
                                KaGuan.USER, KaGuan.PSWD, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);
            if (!KaGuan.CallDsjk(SendDataXML, ref RecvData))
            {
                BeginInvoke((MethodInvoker)delegate () { lbl_error.Text = "缴费登记失败：" + KaGuan.g_strErrMsg; });
                if (ssi.mdbQryResult == "")
                {
                    ssc.Error = "缴费登记失败：" + KaGuan.g_strErrMsg;
                    SSCSQL.Insert();
                }
                return false;
            }
            return true;
        }

        private bool QueryRegister()
        {
            if (ssi.mdbQryResult != "") {
                string[] result = ssi.mdbQryResult.Split('|');
                ssc.PayCode = result[0];
                ssc.Transtime = result[1];
                ssc.nPro = int.Parse(result[2]);
                return true;
            }
            string strSendXML = string.Format("<操作*>查询交费登记状态</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*>",
                    KaGuan.USER, KaGuan.PSWD, ssi.ID, ssi.ownerName);
            string strRecvData = "";
            if (!KaGuan.CallDsjk(strSendXML, ref strRecvData)) return false;
            else return true;
        }

        private bool LossCard()
        {
            BeginInvoke((MethodInvoker)delegate () { lbl_error.Text = "正在为您挂失社保卡，请稍候"; });
            if (ssi.sbkstatus == "正式挂失") {
                BeginInvoke((MethodInvoker)delegate () { lbl_error.Text = "社保卡已挂失。请使用（微信/支付宝）扫描二维码进行工本费支付 "; });
                return true;
            }
            if (ssi.mdbQryResult != "") {
                return true;
            }
            string SendDataXML, RecvData = "";
            if (ssi.sbkstatus == "封存" || ssi.sbkstatus == "待领卡")
            {
                if (ssi.mdbQryResult != "")
                {
                    return true;
                }
                else
                {
                    SendDataXML = string.Format("<操作*>领卡启用</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><社会保障卡卡号*>{3}</社会保障卡卡号*><社会保障号码*>{4}</社会保障号码*><姓名*>{5}</姓名*>",
               KaGuan.USER, KaGuan.PSWD, KaGuan.CITYCODE, ssi.securityOldNo, ssi.ID, ssi.ownerName);
                    if (!KaGuan.CallDsjk(SendDataXML, ref RecvData))
                    {
                        BeginInvoke((MethodInvoker)delegate () { lbl_error.Text = "挂失失败: 领卡启用失败"; });
                        return false;
                    }
                }
            }
            SendDataXML = string.Format("<操作*>正式挂失</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><社会保障卡卡号*>{3}</社会保障卡卡号*><社会保障号码*>{4}</社会保障号码*><姓名*>{5}</姓名*><开户银行></开户银行><银行卡号></银行卡号>",
                KaGuan.USER, KaGuan.PSWD, KaGuan.CITYCODE, ssi.securityOldNo, ssi.ID, ssi.ownerName);
            if (!KaGuan.CallDsjk(SendDataXML, ref RecvData)) {
                BeginInvoke((MethodInvoker)delegate () { lbl_error.Text = "挂失失败: 请与工作人员联系" ; });
                return false;
            }
            BeginInvoke((MethodInvoker)delegate () { lbl_error.Text = "社保卡挂失成功。请使用（微信/支付宝）扫描二维码进行工本费支付 "; });
            
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try {
                if (nTicket > 0) {
                    lbl_time.Text = nTicket.ToString("00");
                    nTicket--;
                }
                else {
                    btn_cancel_Click(sender, e);
                }
            }
            catch(Exception ex) {
                Utility.WriteLog(ExceptionTaitou + "timer1_Tick: " + ex.Message);
            }
        }       

        private void SetPictureBox1()
        {
            pictureBox1.Image = null;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.BackColor = SystemColors.Control;
            //Rectangle rect = Screen.GetWorkingArea(this);
            //int x = rect.Width / 3-10;
            //int y = 320;
            pictureBox1.Location = new Point(470, 200);
            pictureBox1.Size = new Size(685, 688);
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //SetPictureBox1();
            //pictureBox1.Image = null;
            //pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            //pictureBox1.BackColor = SystemColors.Control;
            //Rectangle rect = Screen.GetWorkingArea(this);
            //int x = rect.Width / 3;
            //int y = 320;
            //pictureBox1.Location = new Point(x, y);
            //pictureBox1.Size = new Size(rect.Width / 2 - 20, rect.Height - y + 10);
            int lineheight = 32;
            int nTextX = 60;
            int nTextY = 10;

            Font font = new Font("宋体", 16, FontStyle.Regular);
            Brush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawString("自助终端缴费回执单", font, brush, 200, nTextY);
            string text = string.Format("尊敬的{0}您好：", ssi.ownerName);
            e.Graphics.DrawString(text, font, brush, nTextX - 40, nTextY + lineheight);
            e.Graphics.DrawString("您已成功缴纳社会保障卡补换卡工本费！", font, brush, nTextX, nTextY + lineheight * 2);
            e.Graphics.DrawString("缴费金额：16元", font, brush, nTextX, nTextY + lineheight * 3);
            text = "社会保障号码：" + ssi.ID.Substring(0, 6) + "********" + ssi.ID.Substring(ssi.ID.Length-4, 4);
            e.Graphics.DrawString(text, font, brush, nTextX, nTextY + lineheight * 4);
            text = "缴款码：" + pay.payCode;
            e.Graphics.DrawString(text, font, brush, nTextX, nTextY + lineheight * 5);
            e.Graphics.DrawString("感谢您的使用，如需财政票据，请到柜台咨询，此回执", font, brush, nTextX, nTextY + lineheight * 6);
            e.Graphics.DrawString("单为财政票据领取凭证！", font, brush, nTextX - 40, nTextY + lineheight * 7);
            e.Graphics.DrawString("社会保障卡服务热线：12333", font, brush, nTextX, nTextY + lineheight * 8);
            e.Graphics.DrawString("（电子签章）", font, brush, nTextX + 300, nTextY + lineheight * 9);
            e.Graphics.DrawString(DateTime.Now.ToString("yyyy年MM月dd日"), font, brush, nTextX + 280, nTextY + lineheight * 10);

            Image colorImg1, colorImg2, colorImg3;

            string picdir = System.Environment.CurrentDirectory;
            string picpath1 = picdir + "\\pic\\河南12333”微信公众号.jpg";
            string picpath2 = picdir + "\\pic\\河南人社二维码Android版.jpg";
            string picpath3 = picdir + "\\pic\\河南人社二维码iphone版.jpg";
            colorImg1 = Image.FromFile(picpath1);
            colorImg2 = Image.FromFile(picpath2);
            colorImg3 = Image.FromFile(picpath3);

            int qrSize = 70;
            Rectangle rect1 = new Rectangle(nTextX + 160, nTextY + lineheight * 11, qrSize, qrSize);
            e.Graphics.DrawImage(colorImg1, rect1);
            e.Graphics.DrawString("“河南12333”微信公众号", font, brush, nTextX + 100, nTextY + lineheight * 11 + qrSize + 5);
            Rectangle rect2 = new Rectangle(nTextX + 80, nTextY + lineheight * 12 + qrSize, qrSize, qrSize);
            Rectangle rect3 = new Rectangle(nTextX + qrSize + 140, nTextY + lineheight * 12 + qrSize, qrSize, qrSize);
            e.Graphics.DrawImage(colorImg2, rect2);
            e.Graphics.DrawImage(colorImg3, rect3);
            e.Graphics.DrawString("（Android版）", font, brush, nTextX + 65, nTextY + lineheight * 19);
            e.Graphics.DrawString("（iphone版）", font, brush, nTextX + qrSize + 125, nTextY + lineheight * 19);
            e.Graphics.DrawString("“河南人社”手机APP", font, brush, nTextX + 110, nTextY + lineheight * 20);
        }

    }
}
