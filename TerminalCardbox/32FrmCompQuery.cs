using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmCompQuery : Form
    {
        SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();     // 社保信息
        SIService.CardServiceClient csc = new SIService.CardServiceClient("CardService");
        int nCountDown;
        int nDJS = 60; // 倒计时
        int nDJS2 = 5;

        string ExceptionTaitou = "FrmCompQuery Exception - ";
        public FrmCompQuery()
        {
            InitializeComponent();
        }
        private void FrmCompQuery_Load(object sender, EventArgs e)
        {
            ctrlLocation();
        }
        private void ctrlLocation()
        {
            //lblError.Text = "fgdfhgfhghgfhkfgdfkldjgdfgjefigjdfkgjdfkgfdkgdfngdfg";
            panel1.Location = new Point(93, 203);
            //lblError.Location = new Point(Global.nWidth * 16 / 68, Global.nHeight * 30 / 38);
            btnReturn.Location = new Point(625, 897);
            lblCountDown.Location = new Point(995, 880);
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
        public void Init()
        {
            nCountDown = nDJS;
            timer1.Enabled = true;

            Thread th = new Thread(Query);
            th.Start();
        }

        void Query()
        {
            string error = "";
            try {
                bool resultRet = KaGuan.GetRysj();
                if (!resultRet)
                {
                    error = "查不到您的社保信息，请到服务台咨询。";
                    return;
                }
                resultRet = KaGuan.GetPhoto();
                if (!resultRet)
                {
                    error = "查不到您的照片信息，请到服务台咨询。";
                    return;
                }
                //strdata = "\查询卡状态...\n";
                string status = csc.getCard(KaGuan.USER, KaGuan.PSWD, ssi.securityOldNo, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);

                resultRet = KaGuan.GetZkjd();
                if (!resultRet)
                {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = "查不到您的银行信息，请到服务台咨询。"; });
                }
                KaGuan.GetLkdInfo();

                BeginInvoke((MethodInvoker)delegate () {
                    label1.Text = "姓名：" + ssi.ownerName;
                    label2.Text = "性别：" + SiCard.SexShow;
                    label3.Text = "手机号：" + KaGuan.kgPhone;
                    label4.Text = "身份证号：" + ssi.ID;
                    label5.Text = "社保卡号：" + ssi.securityOldNo;
                    label6.Text = "社保卡状态：" + status;
                    label7.Text = "所属银行：" + KaGuan.kgBankName;
                    label8.Text = "市接收时间：" + KaGuan.jsTime;
                    label10.Text = "领卡地信息：" + KaGuan.lkdInfo;
                    label9.Text = "通讯地址：" + ssi.addr;
                    picphoto.Image = ssi.photo;
                });
            }
            catch (System.Net.WebException) {
                error = "卡管服务器超时，请稍后重试。";
            }
            catch (Exception ex) {
                error = ExceptionTaitou + ex.Message;
            }
            finally
            {
                if(error != "")
                {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = error; });
                    Utility.WriteLog(error);
                    nCountDown = nDJS2;
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            FormMng.ShowMain(this);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nCountDown > 0)
            {
                nCountDown--;

                string strcount = nCountDown.ToString("00");

                lblCountDown.Text = nCountDown.ToString("00");
            }
            else
            {
                btnReturn_Click(sender, e);
            }
        }

    }
}
