using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using CommenLib.MP3;

namespace SHBSS
{    
    public partial class FrmSupplyLogin : Form
    {
        int nCountDown;
        int nDJS = 30;
        int nDJS2 = 10;
        int threadsgn = 0;       

        //FrmCheck fp = FrmCheck.CreateFrom();

        string ExceptionTaitou = "FrmSupplyLogin Exception - ";
        public FrmSupplyLogin()
        {
            InitializeComponent();
            timer2.Enabled = false;           
            ctrlLocation();
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
            MP3Player.Play(FrmMain.g_soundDir + "1IDCard.mp3");
            nCountDown = nDJS;
            lblCountDown.Text = nCountDown.ToString("00");
            lblCountDown.Refresh();
            lblError.Text = "";
            threadsgn = 0;
            timer2.Enabled = true;
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
        private void ReadIDCard()
        {
            if (threadsgn == 0)
                threadsgn = 1;
            else
                return;
            try
            {
                // 读取身份证信息
                if(!SiCard.ReadIDCard()) {
                    //BeginInvoke((MethodInvoker)delegate () {
                    //    lblError.Text = SiCard.errMsg;
                    //});
                    return;
                }

                Utility.WriteLog(SiCard.userID + ", " + SiCard.userName);
                SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
                if (SiCard.userID != ssi.ID)
                {
                    ssi.phone = "";
                }
                ssi.ownerName = SiCard.userName;
                ssi.ID = SiCard.userID;
                ssi.sex = SiCard.Sex;
                ssi.idvalid = SiCard.validTime;

                string today = DateTime.Now.ToString("yyyyMMdd");
                if (ssi.idvalid != "长期" && ( int.Parse(today) > int.Parse(ssi.idvalid) )) {
                    BeginInvoke((MethodInvoker)delegate ()
                    {
                        lblError.Text = "您的身份证已过期，无法进行补换社保卡。";
                    });
                    Utility.WriteLog("您的身份证已过期，无法进行补换社保卡。");
                    nCountDown = nDJS2;
                    return;
                }
                //ssi.addr = ICReader.addr;

                BeginInvoke( (MethodInvoker)delegate()
                {
                    timer2.Enabled = false;
                    if (FrmMain.g_nBusiness == 3) {
                        // 综合查询
                        FrmCompQuery fcq = new FrmCompQuery();
                        FormMng.Show(this, fcq);
                        fcq.Init();
                    }
                    else
                    {
                        FrmPINChange fpc = new FrmPINChange();
                        fpc.Show();
                        //FormMng.Show(this, fp);
                        //fp.Init();
                    }
                    this.Hide();
                });
            }
            catch(Exception e)
            {
                Utility.WriteLog(ExceptionTaitou + "ReadIDCard：" + e.Message);
                nCountDown = nDJS2;
            }
            finally
            {
                threadsgn = 0;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            //FormMng.Hide(this);
            FormMng.ShowMain(this);
        }
        
      
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (nCountDown > 0)
            {
                nCountDown = nCountDown - 1;
                string strcount = nCountDown.ToString("00");
                lblCountDown.Text = nCountDown.ToString("00");
                if (nCountDown <= nDJS2) threadsgn = 1;
                if (threadsgn == 0)
                {
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
