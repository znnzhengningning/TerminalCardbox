using System;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmPswd : Form
    {
        FrmCardboxTest fcdlg = new FrmCardboxTest();
        int currentpos;
        public FrmPswd()
        {
            InitializeComponent();
            getCtrlBtn();
            Init();
            //currentpos = 0;
            //currentTextBox.Focus();
            //currentTextBox.LostFocus += new EventHandler(ctl_LostFocus);
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
            //InterfaceClass.WxPicture.DownLoadPicture("411329198708110030");
            currentpos = 0;
            currentTextBox.Text = "";
            currentTextBox.Focus();
        }

        void ctl_EnterClick(object sender, EventArgs e)
        {
            if (currentTextBox.Text == Global.GetInstance().SysSetPwd) {
                fcdlg.Show();
                //fcdlg.TopMost = true;
                this.Hide();
            }
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
            btnDel.Click += new EventHandler(ctl_DeleteClick);
            btnEnter.Click += new EventHandler(ctl_EnterClick);
        }
        void ctl_DeleteClick(object sender, EventArgs e)
        {
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

        void ctl_Click(object sender, EventArgs e)
        {
            if (currentTextBox.Text.Length >= 6)
                return;
            currentTextBox.Text = currentTextBox.Text.Insert(currentpos, ( (Button)sender ).Text);
            currentpos += ( (Button)sender ).Text.Length;
            currentTextBox.Focus();
            currentTextBox.SelectionStart = currentpos;
            currentTextBox.SelectionLength = 0;
        }


        void ctl_LostFocus(object sender, EventArgs e)
        {
            currentTextBox = (TextBox)sender;
            currentpos = currentTextBox.SelectionStart;
        }

        private void FrmPswd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            FormMng.ShowMain(this);
            this.Hide();
        }
    }
}
