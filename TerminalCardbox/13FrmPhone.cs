using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using CommenLib.MP3;

namespace SHBSS
{
    public partial class FrmPhone : Form
    {

        int currentpos;
        string userPhone;
        //string ExceptionTaitou = "FrmPhone Exception - ";
        public FrmPhone()
        {
            InitializeComponent();

            ctrlLocation();
            getCtrlBtn();
            timer1.Enabled = false;
        }
        public void getCtrlBtn()
        {
            currentTextBox.LostFocus += new EventHandler(ctl_LostFocus);
            btn1.Enter += new EventHandler(ctl_Click);
            btn2.Enter += new EventHandler(ctl_Click);
            btn3.Enter += new EventHandler(ctl_Click);
            btn4.Enter += new EventHandler(ctl_Click);
            btn5.Enter += new EventHandler(ctl_Click);
            btn6.Enter += new EventHandler(ctl_Click);
            btn7.Enter += new EventHandler(ctl_Click);
            btn8.Enter += new EventHandler(ctl_Click);
            btn9.Enter += new EventHandler(ctl_Click);
            btn0.Enter += new EventHandler(ctl_Click);
            btnDel.Click += new EventHandler(ctl_ClearClick);
        }
        private void ctrlLocation()
        {
            label_Phone.Location = new Point(130, 252);
            panel1.Location = new Point(606, 328);
            btnReturn.Location = new Point(680, 860);
            lblCountDown.Location = new Point(977, 842);
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
            MP3Player.Play(FrmMain.g_soundDir + "4Number.mp3");
            this.currentpos = 0;
            if(SocialSecurityInfo.GetInstance().phone != "")
                currentTextBox.Text = SocialSecurityInfo.GetInstance().phone;
            else
                currentTextBox.Text = ""; // 13714168670
            currentTextBox.Focus();

            nCountDown = 30;
            lblCountDown.Text = nCountDown.ToString("00");
            timer1.Enabled = true;
        }
        void ctl_ClearClick(object sender, EventArgs e)
        {
            try
            {
                currentTextBox.Text = currentTextBox.Text.Remove(currentpos - 1, 1);
                currentpos -= ((Button)sender).Text.Length;
                currentTextBox.Focus();
                currentTextBox.SelectionStart = currentpos + 1;
                currentTextBox.SelectionLength = 0;
            }
            catch
            {
                MessageBox.Show("请从正确位置删除！");
            }
        }


        void ctl_Click(object sender, EventArgs e)
        {
            if (currentTextBox.Text.Length >= 11)
                return;
            currentTextBox.Text = currentTextBox.Text.Insert(currentpos, ((Button)sender).Text);
            currentpos += ((Button)sender).Text.Length;
            currentTextBox.Focus();
            currentTextBox.SelectionStart = currentpos;
            currentTextBox.SelectionLength = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (currentTextBox.Text.Length < 11)
                return;
            userPhone = currentTextBox.Text.Substring(0, 11);
            if(userPhone.Substring(0,1) != "1") {
                MessageBox.Show("手机号码首位数字不是1");
                return;
            }
            SocialSecurityInfo.GetInstance().phone = userPhone;
            Utility.WriteLog("用户输入手机号：" + userPhone);
            this.timer1.Enabled = false;
            SSCTable.getSSCTable().Phone = userPhone;
            //FrmService fs = new FrmService();
            //FormMng.Show(this, fs);
            FrmPay fs = new FrmPay();
            FormMng.Show(this, fs);
        }


        void ctl_LostFocus(object sender, EventArgs e)
        {
            currentTextBox = (TextBox)sender;
            currentpos = currentTextBox.SelectionStart;
        }
       

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            FormMng.ShowMain(this);
        }

        int nCountDown = 30;
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

        private void currentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (currentTextBox.Text.Length == 11)
            {
                btnOK.BackColor = Color.Orange;
            }
            else
                btnOK.BackColor = Control.DefaultBackColor;
        }
    }
}


      


