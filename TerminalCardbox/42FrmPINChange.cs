using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmPINChange : Form
    {
        TextBox currentTextBox;
        int currentpos;
        int nCountDown;
        int nDJS = 120;
        int nDJS2 = 5;
        public FrmPINChange()
        {
            InitializeComponent();
        }
        private void FrmPINChange_Load(object sender, EventArgs e)
        {
            ctrlLocation();
            this.Show();
            Init();
            getCtrlBtn();
        }
        private void ctrlLocation()
        {
            btn_reload_pin.Location = new Point(23, 330);
            btn_change_pin.Location = new Point(23, 480);

            label_PIN.Location = new Point(296, 151);
            panel1.Location = new Point(490, 221);
            btnReturn.Location = new Point(642, 868);
            lblCountDown.Location = new Point(926, 851);
        }
        void Init()
        {
            nCountDown = nDJS;
            label_prompt.Text = "";
            timer1.Enabled = true;
            lblCountDown.Text = nCountDown.ToString("00");
            btn_reload_pin_Click(this, new EventArgs());
        }

        public void getCtrlBtn()
        {
            txtOldPwd.LostFocus += new EventHandler(ctl_LostFocus);
            txtNewPwd.LostFocus += new EventHandler(ctl_LostFocus);
            txtCheckPwd.LostFocus += new EventHandler(ctl_LostFocus);
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
            btnDel.Click += new EventHandler(ctl_DeleteClick);
            btnEmpty.Click += new EventHandler(ctl_EnptyClick);
            // btn_ok.Click += new EventHandler(ctl_OKClick);
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
        private void btn_pin_confirm_Click(object sender, EventArgs e)
        {
            if (!txtOldPwd.Enabled) {
                ReloadPin();
            }
            else {
                ChangePin();
            }
        }

        private void btn_reload_pin_Click(object sender, EventArgs e)
        {
            reloadPin = "";
            currentpos = 0;
            btnreloadpininit();
        }

        private void btn_change_pin_Click(object sender, EventArgs e)
        {
            currentpos = 0;
            btnchangepininit();
        }
        void btnreloadpininit()
        {
            txtOldPwd.Text = "";
            txtNewPwd.Text = "";
            txtCheckPwd.Text = "";

            label_PIN.Text = "重置密码";
            btn_pin_confirm.Text = "确认重置";
            txtOldPwd.Enabled = false;
            label1.Enabled = false;
            txtNewPwd.Focus();
        }
        void btnchangepininit()
        {
            txtOldPwd.Text = "";
            txtNewPwd.Text = "";
            txtCheckPwd.Text = "";

            label_PIN.Text = "修改密码";
            btn_pin_confirm.Text = "确认修改";
            txtOldPwd.Enabled = true;
            label1.Enabled = true;
            txtOldPwd.Focus();
        }
        void ctl_LostFocus(object sender, EventArgs e)
        {
            currentTextBox = (TextBox)sender;
            currentpos = currentTextBox.SelectionStart;
        }
        void ctl_Enter(object sender, EventArgs e)
        {
            currentTextBox = null;
        }
        void ctl_Click(object sender, EventArgs e)
        {
            if (currentTextBox != null && currentTextBox.TextLength < 6)
            {
                currentTextBox.Text = currentTextBox.Text.Insert(currentpos, ((Button)sender).Text);
                currentpos += ((Button)sender).Text.Length;
                currentTextBox.Focus();
                currentTextBox.SelectionStart = currentpos;
                currentTextBox.SelectionLength = 0;
            }
            else
            {
                if (txtOldPwd.TextLength == 6)
                {
                    txtNewPwd.Focus();
                }
                if (txtNewPwd.TextLength == 6)
                {
                    txtCheckPwd.Focus();
                }
                return;
            }
        }
        
        void ctl_DeleteClick(object sender, EventArgs e)
        {

            if (currentTextBox != null)
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
            else

                return;
        }
        void ctl_EnptyClick(object sender, EventArgs e)
        {

            if (currentTextBox != null)
            {
                try
                {
                    currentTextBox.Text = "";
                    currentTextBox.Focus();
                }
                catch
                {
                    MessageBox.Show("请从正确位置删除！");
                }
            }
            else
                return;

        }
        public void ChangePin()
        {
            string strOldPswd, strNewPswd, strCheckPswd;
            strOldPswd = txtOldPwd.Text;
            strNewPswd = txtNewPwd.Text;
            strCheckPswd = txtCheckPwd.Text;
            label_prompt.ForeColor = Color.Red;
            if (strOldPswd.Length != 6 || strNewPswd.Length != 6 || strCheckPswd.Length != 6)
            {
                label_prompt.Text = "请输入6位数密码！";
                return;
            }
            if (strNewPswd != strCheckPswd)
            {
                label_prompt.Text = "新密码与确认密码不一致！";
                txtNewPwd.Text = "";
                txtCheckPwd.Text = "";
                txtNewPwd.Focus();
                return;
            }
            StringBuilder strError = new StringBuilder(500);
            int nRet = SiCard.iPChangePIN(Global.GetInstance().nPortPin, SiCard.TP_CONTACT, strOldPswd, strNewPswd, strError);
            nCountDown = nDJS2;
            currentpos = 0;
            btnchangepininit();
            if (nRet == 0)
            {
                label_prompt.ForeColor = Color.Green;
                label_prompt.Text = "密码修改成功！";
                return;
            }
            //label_prompt.Text = "密码修改失败！";
            label_prompt.Text = strError.ToString();
            return;
        }

        void ReloadPin()
        {
            string strNewPswd, strCheckPswd;
            strNewPswd = txtNewPwd.Text;
            strCheckPswd = txtCheckPwd.Text;
            label_prompt.ForeColor = Color.Red;
            if (strNewPswd.Length != 6 || strCheckPswd.Length != 6) {
                label_prompt.Text = "请输入6位数密码！";
                return;
            }
            if (strNewPswd != strCheckPswd) {
                label_prompt.Text = "新密码与确认密码不一致！";
                txtNewPwd.Text = "";
                txtCheckPwd.Text = "";
                txtNewPwd.Focus();
                return;
            }
            reloadPin = strNewPswd;

            Thread th = new Thread(ReloadPinThread);
            th.Start();
        }

        string reloadPin = "";
        void ReloadPinThread()
        {
            string error = "";
            try
            {
                StringBuilder pOutputInfo = new StringBuilder(600);
                StringBuilder pErrMsg = new StringBuilder(600);
                int ret = SiCard.Print_ReadCard(Global.GetInstance().nPortPin, SiCard.TP_CONTACT, pOutputInfo, pErrMsg);
                if (ret == 0)
                {
                    string[] str = pOutputInfo.ToString().Split('|');
                    string xfzh = str[1];
                    string xm = str[2];
                    if(xfzh != SocialSecurityInfo.GetInstance().ID) { error = "不是本人身份证，不能重置密码";return; }
                    ret = SiCard.iReloadPIN_HSM(Global.GetInstance().nPortPin, SiCard.TP_CONTACT, xfzh, xm, reloadPin, pErrMsg);
                    if (ret == 0)
                    {
                        error = xm + "|重置密码成功";
                    }
                    else
                    {
                        error = pErrMsg.ToString(); 
                    }
                }
                else
                {
                    Utility.WriteLog("ReloadPin卡片读取返回值:" + ret.ToString() + "  " + pErrMsg.ToString());
                    error = "读卡失败:" + ret.ToString();
                }
            }
            catch (Exception ex)
            {
                error = "ReloadPin Exception: " + ex.Message;
            }
            finally
            {
                reloadPin = "";
                currentpos = 0;
                BeginInvoke((MethodInvoker)delegate () {
                    label_prompt.Text = error;
                    btnreloadpininit();
                });
                Utility.WriteLog(error);
                nCountDown = nDJS2;
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
