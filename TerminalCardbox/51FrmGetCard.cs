using CommenLib.MP3;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SHBSS
{
    public partial class FrmGetCard : Form
    {
        int nCountDown;
        int nDJS = 60;
        int nDJS1 = 30;
        int nDJS2 = 10;
        int threadsgn = 0;
        string cardid = "";
        string ExceptionTaitou = "FrmGetCard Exception - ";

        F8SAN f8san = F8SAN.GetInstance();
        SIService.CardServiceClient csc = new SIService.CardServiceClient("CardService");
        public FrmGetCard()
        {
            InitializeComponent();
            timer1.Enabled = false;
            ctrlLocation();
        }
        
        private void ctrlLocation()
        {
            lbl_get_sicard.Location = new Point(Global.nWidth * 4 / 36, Global.nHeight * 9 / 27);
            lblError.Location = new Point(Global.nWidth * 4 / 36, Global.nHeight * 13 / 27);
            btnReturn.Location = new Point(Global.nWidth * 19 / 36, Global.nHeight * 24 / 27 - 30);
            lblCountDown.Location = new Point(Global.nWidth * 27 / 36, Global.nHeight * 22 / 27);
        }
        public void Init()
        {
            MP3Player.Play(FrmMain.g_soundDir + "1IDCard.mp3");
            nCountDown = nDJS;
            lblCountDown.Text = nCountDown.ToString("00");
            lblCountDown.Refresh();
            lblError.Text = "";
            threadsgn = 0;
            timer1.Enabled = true;
            lbl_read_id.Text = "请放置身份证在读卡区";
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
            try {
                if (!SiCard.ReadIDCard()) {
                    //BeginInvoke((MethodInvoker)delegate () {
                    //    lblError.Text = SiCard.errMsg;
                    //});
                    return;
                }
                cardid = SiCard.userID;
                BeginInvoke((MethodInvoker)delegate ()
                {
                    timer1.Enabled = false;
                });

                int nblock, nslot;
                int[] nPos = new int[2];
                if (!MDATASQL.getSiCardPosition2(cardid, ref nPos)) {
                    BeginInvoke((MethodInvoker)delegate (){lblError.Text = "从数据库查找卡片失败。"; });
                    return;
                }
                nslot = nPos[0];
                nblock = nPos[1];
                if (!f8san.CheckSlot(nblock, nslot, F8SAN.TP_SiCard)) { lblError.Text = f8san.strError; return; }

                if (!f8san.pan2car(nblock, nslot, F8SAN.TP_SiCard)) { lblError.Text = f8san.strError; return; }

                if (!f8san.car2reader()) { lblError.Text = f8san.strError; return; }
                Thread.Sleep(100);
                int nPort;
                if (PrintParameter.GetInstance().nPrinter == 1) {
                    nPort = Global.GetInstance().RWPort1;
                }
                else {
                    nPort = Global.GetInstance().RWPort2;
                }
                StringBuilder pOutputInfo = new StringBuilder(600);
                StringBuilder pErrMsg = new StringBuilder(600);
                int ret1 = SiCard.Print_ReadCard(nPort, SiCard.TP_CONTACT, pOutputInfo, pErrMsg);
                if (ret1 != 0) {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = "卡片读取失败: " + pErrMsg.ToString(); });
                    f8san.reader2car();
                    f8san.car2recycle();
                    return;
                }

                string[] cardinfo = pOutputInfo.ToString().Split('|');
                string name = cardinfo[2];
                string securityNo = cardinfo[0];
                string makeDate = cardinfo[4];
                string photoRet = csc.getPhoto(KaGuan.PICUSER, KaGuan.PICPSWD, cardid, name, "");
                XElement pRoot = XElement.Parse("<root>" + photoRet.Trim() + "</root>");
                string err = pRoot.Element("ERR").Value;
                if (!err.Equals("OK")) {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = "查不到您的社保信息，请到服务台咨询。"; });
                    f8san.reader2car();
                    f8san.car2recycle();
                    return;
                }
                string base64Photo = pRoot.Element("PHOTO").Value;
                byte[] bytes = Convert.FromBase64String(base64Photo);
                MemoryStream memStream = new MemoryStream(bytes);
                Bitmap img = new Bitmap(memStream);
                BeginInvoke((MethodInvoker)delegate () {
                    lblName.Text = name;
                    lblSID.Text = cardid;
                    lblCID.Text = securityNo;
                    lblDate.Text = makeDate;
                    picphoto.BackgroundImage = img;
                    lbl_read_id.Text = "请确认信息无误后，领取社保卡";
                });
                nCountDown = nDJS1;
                // 语音：请确认信息无误后，领取社保卡
            }
            catch (Exception e) {
                Utility.WriteLog(ExceptionTaitou + "ReadIDCard：" + e.Message);
                nCountDown = nDJS2;
            }
            finally {
                threadsgn = 0;
            }
        }

        private void btn_confrm_Click(object sender, EventArgs e)
        {
            if (!f8san.OutCardFromReader()) { lblError.Text = f8san.strError; return; }
            lbl_read_id.Text = "请从持卡口取卡";
            nCountDown = nDJS2;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            FormMng.ShowMain(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nCountDown > 0) {
                nCountDown = nCountDown - 1;
                string strcount = nCountDown.ToString("00");
                lblCountDown.Text = nCountDown.ToString("00");
                if (nCountDown <= nDJS2) threadsgn = 1;
                if (threadsgn == 0) {
                    Thread th = new Thread(new ThreadStart(ReadIDCard));
                    th.Start();
                }
            }
            else {
                btnReturn_Click(sender, e);
            }
        }
    }
}
