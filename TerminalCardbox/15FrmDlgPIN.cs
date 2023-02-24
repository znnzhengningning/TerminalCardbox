using CommenLib.MP3;
using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmDlgPIN : Form
    {
        TextBox currentTextBox;
        int currentpos;
        public FrmDlgPIN()
        {
            InitializeComponent();
        }

        private void FrmDlgPIN_Load(object sender, EventArgs e)
        {
            this.Show();
            Init();
            getCtrlBtn();
        }
        void Init()
        {
            currentpos = 0;
            txtNewPwd.Focus();
            label_prompt.Visible = false;
            MP3Player.Play(FrmMain.g_soundDir + "szmm.mp3");
        }
        public void getCtrlBtn()
        {
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
            btn_ok.Click += new EventHandler(ctl_OKClick);
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
            if (currentTextBox != null && currentTextBox.TextLength < 6) {
                currentTextBox.Text = currentTextBox.Text.Insert(currentpos, ( (Button)sender ).Text);
                currentpos += ( (Button)sender ).Text.Length;
                currentTextBox.Focus();
                currentTextBox.SelectionStart = currentpos;
                currentTextBox.SelectionLength = 0;
            }
            else {
                if (txtNewPwd.TextLength == 6) {
                    txtCheckPwd.Focus();
                }
                return;
            }
        }

        void ctl_DeleteClick(object sender, EventArgs e)
        {

            if (currentTextBox != null) {
                try {
                    currentTextBox.Text = currentTextBox.Text.Remove(currentpos - 1, 1);
                    currentpos -= ( (Button)sender ).Text.Length;
                    currentTextBox.Focus();
                    currentTextBox.SelectionStart = currentpos + 1;
                    currentTextBox.SelectionLength = 0;

                }
                catch {
                    MessageBox.Show("请从正确位置删除！");
                }
            }
            else

                return;
        }
        void ctl_EnptyClick(object sender, EventArgs e)
        {

            if (currentTextBox != null) {
                try {
                    currentTextBox.Text = "";
                    currentTextBox.Focus();
                }
                catch {
                    MessageBox.Show("请从正确位置删除！");
                }
            }
            else
                return;

        }

        public void ctl_OKClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string strOldPswd, strNewPswd, strCheckPswd;
            strOldPswd = "123456";
            strNewPswd = txtNewPwd.Text;
            strCheckPswd = txtCheckPwd.Text;
            label_prompt.ForeColor = Color.Red;
            if (strOldPswd.Length != 6 || strNewPswd.Length != 6 || strCheckPswd.Length != 6) {
                label_prompt.Visible = true;
                label_prompt.Text = "请输入6位数密码！";
                this.Cursor = Cursors.Default;
                return;
            }
            if (strNewPswd != strCheckPswd) {
                label_prompt.Visible = true;
                label_prompt.Text = "新密码与确认密码不一致！";
                txtNewPwd.Text = "";
                txtCheckPwd.Text = "";
                txtNewPwd.Focus();
                this.Cursor = Cursors.Default;
                return;
            }
            int nPort;
            if (PrintParameter.GetInstance().nPrinter == 1) {
                nPort = Global.GetInstance().RWPort1;
            }
            else {
                nPort = Global.GetInstance().RWPort2;
            }
            StringBuilder strError = new StringBuilder(500);
            int nRet = SiCard.iPChangePIN(nPort, SiCard.TP_CONTACT, strOldPswd, strNewPswd, strError);
            if (nRet == 0) {
                label_prompt.ForeColor = Color.Green;
                label_prompt.Text = "密码设置成功！";
                label_prompt.Visible = true;
                txtNewPwd.Text = "";
                txtCheckPwd.Text = "";
                MP3Player.Play(FrmMain.g_soundDir + "mmszcg.mp3");
                this.Cursor = Cursors.Default;
                this.Close();
            }
            //label_prompt.Text = "密码修改失败！";
            label_prompt.Text = strError.ToString();
            label_prompt.Visible = true;
            txtNewPwd.Text = "";
            txtCheckPwd.Text = "";
            Thread.Sleep(3000);
            this.Cursor = Cursors.Default;
            this.Close();

        }
    }
}
