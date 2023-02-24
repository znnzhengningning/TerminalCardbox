using CommenLib.MP3;
using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmPINLogin : Form
    {
        int nCountDown;
        int nDJS = 30;
        int nDJS2 = 5;
        string strErr = "";
        int threadsgn;
        public FrmPINLogin()
        {
            InitializeComponent();
            timer1.Enabled = false;
        }
        private void FrmPINLogin_Load(object sender, EventArgs e)
        {
            ctrlLocation();
        }
        private void ctrlLocation()
        {
            //lblError.Text = "fgdfhgfhghgfhkfgdfkldjgdfgjefigjdfkgjdfkgfdkgdfngdfg";
            label_SICards.Location = new Point(Global.nWidth * 7 / 68, Global.nHeight * 11 / 38);
            pictureBox1.Location = new Point(Global.nWidth * 38 / 68, Global.nHeight * 13 / 38);
            lblError.Location = new Point(Global.nWidth * 8 / 68, Global.nHeight * 23 / 38);
            btnReturn.Location = new Point(Global.nWidth * 38 / 68, Global.nHeight * 31 / 38);
            lblCountDown.Location = new Point(Global.nWidth * 53 / 68, Global.nHeight * 31 / 38 - 20);
        }
        public void Init()
        {
            MP3Player.Play(FrmMain.g_soundDir + "1SICard.mp3");
            nCountDown = nDJS;
            lblCountDown.Text = nCountDown.ToString("00");
            lblCountDown.Refresh();
            lblError.Text = "";
            threadsgn = 0;
            timer1.Enabled = true;
            //U10.ContrlLight(U10.U10_LIGHT_2, U10.U10_CTRL_ON);
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
        private void ReadSICard()
        {
            try
            {
                if (threadsgn == 0)
                    threadsgn = 1;
                else
                    return;
                //停止倒计时，获取该用户的社保信息
                StringBuilder pOutInfo = new StringBuilder(600);
                StringBuilder strSiErr = new StringBuilder(600);
                int ret = SiCard.ReadSSSE();

                if(ret == -2) {
                    strErr = strSiErr.ToString();
                    DoErr();
                    return;
                }
                if (ret != 0) return;
                //U10.ContrlLight(U10.U10_LIGHT_2, U10.U10_CTRL_OFF);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    timer1.Enabled = false;
                    FrmPINChange fpc = new FrmPINChange();
                    fpc.Show();
                    this.Hide();
                });
            }
            catch (Exception e)
            {
                strErr = "读取社保卡异常：" + e.Message;
                DoErr();
            }
            finally
            {
                threadsgn = 0;
            }
        }

        void DoErr()
        {
            BeginInvoke((MethodInvoker)delegate () { lblError.Text = strErr; });
            Utility.WriteLog(strErr);
            nCountDown = nDJS2;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
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
                if (nCountDown <= nDJS2) threadsgn = 1;
                if (threadsgn == 0)
                {
                    Thread th = new Thread(new ThreadStart(ReadSICard));
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
