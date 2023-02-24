using CommenLib.MP3;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmCheck : Form
    {
        FrmPhone fp = new FrmPhone();
        SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
        int nCountDown;
        int nDJS = 30; // 倒计时
        int nDJS2 = 10; // 结束倒计时

        string ExceptionTaitou = "FrmCheck Exception - ";

        private static FrmCheck instance;
        public static FrmCheck CreateFrom()
        {
            //判断是否存在该窗体,或时候该字窗体是否被释放过,如果不存在该窗体,则 new 一个字窗体  
            if (instance == null || instance.IsDisposed)
            {
                instance = new FrmCheck();
            }
            return instance;
        }
        public FrmCheck()
        {
            InitializeComponent();
            //comboBox1.DataSource = FrmConfig.gdt_dynamic_bank;
            //comboBox1.DisplayMember = "bankname";
            //comboBox1.ValueMember = "bankcode";
        }
        private void FrmCheck_Load(object sender, EventArgs e)
        {
            ctrlLocation();
        }
        private void ctrlLocation()
        {
            //lblError.Text = "fgdfhgfhghgfhkfgdfkldjgdfgjefigjdfkgjdfkgfdkgdfngdfg";
            label_Check.Location = new Point(130, 252);
            panel1.Location = new Point(150, 423);
            lblError.Location = new Point(217, 735);
            btn_next.Location = new Point(647, 860);
            btnReturn.Location = new Point(406, 860);
            lblCountDown.Location = new Point(990, 837);
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

       

        /// <summary>
        /// 调用卡管照片接口  人员数据查询  照片查询
        /// </summary>
        public void Init()
        {
            try {
                btn_next.Visible = true;
                nCountDown = nDJS;
                timer1.Enabled = true;

                //bool isExist = false;
                //if (ssi.mdbQryResult != "" || FrmMain.g_nBusiness == 12 || FrmMain.g_nBusiness == 2) {
                //    for(int i = 0; i< FrmConfig.gdt_dynamic_bank.Rows.Count; i++) {
                //        if (ssi.bankcode == FrmConfig.gdt_dynamic_bank.Rows[i][0].ToString()) {
                //            isExist = true;
                //            break;
                //        }
                //    }
                //    comboBox1.SelectedValue = ssi.bankcode;
                //    comboBox1.Enabled = false;
                //    if (!isExist) {
                //        btn_next.Visible = false;
                //        lblError.Text = "卡盘没有您申请的银行卡，请联系工作人员";
                //        nCountDown = nDJS2;
                //        Utility.WriteLog(lblError.Text);
                //    }
                //}
                //else {
                //    comboBox1.SelectedIndex = 0;
                //    comboBox1.Enabled = true;
                //}

                comboBox1.Items.Add(KaGuan.getBankName(Global.GetInstance().BankCode));
                comboBox1.SelectedIndex = 0;

                lblError.Text = "";
                
                label1.Text = "姓名：" + ssi.ownerName;
                label2.Text = "身份证号：" + ssi.ID;
                label3.Text = "地址：" + ssi.addr;
                label5.ForeColor = Color.Red;
                label5.Text = ssi.sbkstatus;
                ////picphoto.
                picphoto.Image = ssi.photo;

                // 语音：请确认社保信息无误后点下一步
                MP3Player.Play(FrmMain.g_soundDir + "2CheckInfo.mp3");

            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lblError.Text = "异常：网络超时。";
                });
                Utility.WriteLog(ExceptionTaitou + "Init: 网络超时。");
                nCountDown = nDJS2;
                return;
            }
            catch (System.Net.WebException wex) {
                btn_next.Visible = false;
                lblError.Text = "卡管服务器超时，请稍后重试。";
                nCountDown = nDJS2;
                Utility.WriteLog(ExceptionTaitou + "Init: " + wex.Message);
            }
            catch(Exception ex) {
                btn_next.Visible = false;
                lblError.Text = ex.Message;
                nCountDown = nDJS2;
                Utility.WriteLog(ExceptionTaitou + "Init: " + ex.Message);
            }
        }


        private void btn_next_Click(object sender, EventArgs e)
        {
            //string bankcode = comboBox1.SelectedValue.ToString();
            //if (!isBankExist(bankcode)) {
            //    if (bankcode != "00000")
            //        nCountDown = nDJS2;
            //    return;
            //}
            //ssi.bankcode = bankcode;

            timer1.Enabled = false;

            if (ssi.phone == "")
            {
                fp.Init();
                FormMng.Show(this, fp);
            }
            else
            {
                FrmService fs = new FrmService();
                FormMng.Show(this, fs);
            }
            
        }

        bool isBankExist(string bankcode)
        {
            if (bankcode == "00000") { lblError.Text =  "请选择银行。"; return false; }
            int[] nPos = new int[2]; string bankno = "";
            if (!MDATASQL.getCardPosition(bankcode, ref nPos, ref bankno)) {
                lblError.Text = "从数据库查找卡片位置失败。";
                return false;
            }
            PrintParameter printerParam = PrintParameter.GetInstance();
            printerParam.nCardPosition = nPos[0];
            printerParam.nCartRidge = nPos[1];
            printerParam.BankNO = bankno;
            if (!F8SAN.GetInstance().CheckSlot(printerParam.nCartRidge, printerParam.nCardPosition, F8SAN.TP_PreMadeCard)) {
                lblError.Text = "指定位置的卡槽无卡。";
                string dfds = string.Format("银行：{0},nslot:{1},nblock:{2}", bankno, printerParam.nCardPosition, printerParam.nCartRidge);
                Utility.WriteLog(dfds);
                return false;
            }
            return true;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            FormMng.ShowMain(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nCountDown > 0) {
                nCountDown--;

                string strcount = nCountDown.ToString("00");

                lblCountDown.Text = nCountDown.ToString("00");
            }
            else {
                btnReturn_Click(sender, e);
            }
        }
        
    }
}
