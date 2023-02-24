using CommenLib.MP3;
using SHBSS.InterfaceClass;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmApplyLogin : Form
    {
        int nCountDown;
        int nDJS = 30;
        int nDJS2 = 10;
        int threadsgn1 = 0;
        FrmCheck checkDlg = FrmCheck.CreateFrom();
        FrmApplyCheck applyCheckDlg = new FrmApplyCheck();
        SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
        SIService.CardServiceClient csc = new SIService.CardServiceClient("CardService");
        string strErr = "";
        string ExceptionTaitou = "FrmApplyLogin Exception - ";
        SSCTable ssc = SSCTable.getSSCTable();
        public FrmApplyLogin()
        {
            InitializeComponent();
            ctrlLocation();
            timer1.Enabled = false;
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

        private void ctrlLocation()
        {
            label_IDCards.Location = new Point(130, 252);
            lblError.Location = new Point(183, 715);
            btnReturn.Location = new Point(680, 860);
            lblCountDown.Location = new Point(977, 842);
        }
        public void Init()
        {
            FrmMain.b_getPwdEnable = false;
            nCountDown = nDJS;
            lblCountDown.Text = nCountDown.ToString("00");
            lblCountDown.Refresh();
            lblError.Text = "";
            threadsgn1 = 0;
            timer1.Enabled = true;
            MP3Player.Play(FrmMain.g_soundDir + "1IDCard.mp3");
        }
        private void ReadIDCard()
        {
            if (threadsgn1 == 0)
            {
                threadsgn1 = 1;
            }
            else
            {
                threadsgn1 = 0;
                return;
            }
            try
            {
                // 读取身份证信息
                if (!SiCard.ReadIDCard()) {
                    if(nCountDown < nDJS - 6)
                    {
                        BeginInvoke((MethodInvoker)delegate () { lblError.Text = SiCard.error; });
                    }
                    if(nCountDown > 28) {
                        Utility.WriteLog(SiCard.error);
                    }
                    return;
                }
                Utility.WriteLog(SiCard.userID + ", " + SiCard.userName);
                ssi.ownerName = SiCard.userName;
                ssi.ID = SiCard.userID;
                ssi.sex = SiCard.Sex;
                ssi.idvalid = SiCard.validTime;
                ssi.nation = SiCard.Nation;
                ssi.addr = SiCard.Addr;

                ssc.IDCard = ssi.ID;
                ssi.mdbQryResult = SSCSQL.Query();

                string today = DateTime.Now.ToString("yyyyMMdd");

                if(ssi.idvalid != "长期") {
                    if(int.Parse(today) > int.Parse(ssi.idvalid))
                    {
                        strErr = "您的身份证已过期";
                        BeginInvoke((MethodInvoker)delegate () { lblError.Text = strErr; });
                        Utility.WriteLog(strErr);
                        nCountDown = nDJS2;
                        return;
                    }
                }
                switch (FrmMain.g_nBusiness)
                {
                    case 1:
                        Business1();
                        break;
                    case 2:
                        Business2();
                        break;
                    default:
                        BeginInvoke((MethodInvoker)delegate () { lblError.Text = "业务办理类型不正确。"; });
                        break;
                }
            }
            catch (WebException)
            {
                strErr = "异常：网络超时。";
                DoErr();
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                strErr = "异常：网络超时。";
                DoErr();
            }
            catch (Exception e)
            {
                string type = e.GetType().ToString();
                Utility.WriteLog(ExceptionTaitou + "ReadIDCard: " + e.Message);
                strErr = "读取身份证异常：" + e.Message;
                DoErr();
            }
            finally
            {
                threadsgn1 = 0;
            }
        }
        
        void DoErr()
        {
            BeginInvoke((MethodInvoker)delegate () { lblError.Text = strErr; });
            Utility.WriteLog(strErr);
            nCountDown = nDJS2;
        }

        void Business1()
        {
            if (!KaGuan.GetZkjd()) {
                strErr = "查不到您的社保制卡进度信息，请到服务台咨询。";
                DoErr();
                return;
            }

            KaGuan.GetRysj();
            if (ssi.mdbQryResult == "" && ssi.securityOldNo == "" && !(ssi.sqlx == "5" && ssi.zkzt == "制卡中")) {
                strErr = "查不到您的社保信息，请到服务台咨询。";
                DoErr();
                return;
            }
            if (ssi.mdbQryResult == "" && ssi.securityOldNo == "" && ssi.sqlx == "5" && ssi.zkzt == "制卡中")
                FrmMain.g_nBusiness = 12;
            else
                ssi.sqlx = "5";

            if (!WxPicture.DownLoadPicture(ssi.ID))
            {
                if (!KaGuan.GetPhoto())
                {
                    strErr = "查不到您的照片信息，请到服务台咨询。";
                    DoErr();
                    return;
                }
            }
            else
            {
                ssi.b_replacephoto = true;
                Image image = GetStreamImage(WxPicture.filepath);
                ssi.photo = image;
                ssi.photopath = WxPicture.filepath;
            }
            if (ssi.mdbQryResult == "") {
                //strdata = "\查询卡状态...\n";
                string status = csc.getCard(KaGuan.USER, KaGuan.PSWD, ssi.securityOldNo, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);
                ssi.sbkstatus = status;
            }
            else {
                ssi.sbkstatus = "再次制卡";
            }

            BeginInvoke((MethodInvoker)delegate ()
            {
                timer1.Enabled = false;
                FrmMain.b_getPwdEnable = true;
                checkDlg.Init();
                FormMng.Show(this, checkDlg);
                this.Hide();
            });
        }

        void Business2()
        {
            if (ssi.mdbQryResult == "") {
                int nRet = KaGuan.getStatus();
                if (nRet == 0) {
                    strErr = "用户已经有社保卡，无法进行个人申请！";
                    DoErr();
                    return;
                }
                else if (nRet == 2) {
                    strErr = KaGuan.g_strErrMsg;
                    DoErr();
                    return;
                }
            }

            if (!WxPicture.DownLoadPicture(ssi.ID)) {
                strErr = "用户没有拍照，请使用首页的微信小程序拍摄证件照提交。";
                DoErr();
                return;
            }
            else {
                Image image = GetStreamImage(WxPicture.filepath);
                ssi.photo = image;
                ssi.photopath = WxPicture.filepath;
            }

            BeginInvoke((MethodInvoker)delegate ()
            {
                timer1.Enabled = false;
                FrmMain.b_getPwdEnable = true;
                applyCheckDlg.Init();
                FormMng.Show(this, applyCheckDlg);
                this.Hide();
            });
        }
        Image GetStreamImage(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            MemoryStream ms = new MemoryStream(data);
            return Image.FromStream(ms);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            FrmMain.b_getPwdEnable = true;
            timer1.Enabled = false;
            //FormMng.Hide(this);
            FormMng.ShowMain(this);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nCountDown > 0)
            {
                nCountDown = nCountDown - 1;
                string strcount = nCountDown.ToString("00");
                lblCountDown.Text = nCountDown.ToString("00");
                if(nCountDown <= nDJS2) { threadsgn1 = 1; }
                if (threadsgn1 == 0) {
                    Thread th = new Thread(new ThreadStart(ReadIDCard));
                    th.Start();
                }
            }
            else
            {
                btnReturn_Click(sender, e);
            }
        }
    }
}
