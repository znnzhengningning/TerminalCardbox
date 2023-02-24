using CommenLib.MP3;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmApplyCheck : Form
    {
        int currentpos;
        int nDJS = 300;
        int nDJS2 = 10;
        int nCountDown;
        SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
        public FrmApplyCheck()
        {
            InitializeComponent();
            ctrlLocation();
            comboxList();
        }

        private void FrmApplyCheck_Load(object sender, EventArgs e)
        {
            this.Show();
            getCtrlBtn();
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
            //lblError.Text = "fgdfhgfhghgfhkfgdfkldjgdfgjefigjdfkgjdfkgfdkgdfngdfg";
            panel1.Location = new Point(83, 201);
            //lblError.Location = new Point(Global.nWidth * 16 / 68, Global.nHeight * 31 / 38);
            btnReturn.Location = new Point(406, 917);
            btn_next.Location = new Point(647,917);
            lblCountDown.Location = new Point(1054, 900);
        }
        public void getCtrlBtn()
        {
            tbox_apply_phone.LostFocus += new EventHandler(ctl_LostFocus);
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
            btnEmpty.Click += new EventHandler(ctl_EmptyClick);
        }
        public void Init()
        {
            combox_bank.SelectedIndex = 0;
            MP3Player.Play(FrmMain.g_soundDir + "applyinfos.mp3");
            this.currentpos = 0;
            nCountDown = nDJS;
            lblCountDown.Text = nCountDown.ToString("00");
            timer1.Enabled = true;

            ShowCtrlInfo();
        }

        void ShowCtrlInfo()
        {
            lbl_name.Text = "姓    名: " + ssi.ownerName;
            lbl_sex.Text = "性别: " + SiCard.SexShow;
            lbl_nation.Text = "民族: " + SiCard.NationShow;
            lbl_id.Text = "身份证号: " + ssi.ID;
            lbl_family_addr.Text = "家庭住址: " + ssi.addr;
            picture_box_apply.Image = ssi.photo;
            b_select_sqbh = false;
            tbox_apply_sqbh.Text = "";
            tbox_apply_phone.Text = "";
            combox_bank.SelectedIndex = 0;
            lblError.Text = "";
        }
        void ctl_LostFocus(object sender, EventArgs e)
        {
            tbox_apply_phone = (TextBox)sender;
            currentpos = tbox_apply_phone.SelectionStart;
        }
        void ctl_DeleteClick(object sender, EventArgs e)
        {
            try {
                tbox_apply_phone.Text = tbox_apply_phone.Text.Remove(currentpos - 1, 1);
                currentpos -= ( (Button)sender ).Text.Length;
                tbox_apply_phone.Focus();
                tbox_apply_phone.SelectionStart = currentpos + 1;
                tbox_apply_phone.SelectionLength = 0;
            }
            catch {
                MessageBox.Show("请从正确位置删除！");
            }
        }
        void ctl_EmptyClick(object sender, EventArgs e)
        {
            tbox_apply_phone.Text = "";
            currentpos = 0;
        }

        void ctl_Click(object sender, EventArgs e)
        {
            if (tbox_apply_phone.Text.Length >= 11)
                return;
            tbox_apply_phone.Text = tbox_apply_phone.Text.Insert(currentpos, ( (Button)sender ).Text);
            currentpos += ( (Button)sender ).Text.Length;
            tbox_apply_phone.Focus();
            tbox_apply_phone.SelectionStart = currentpos;
            tbox_apply_phone.SelectionLength = 0;
        }
        bool isBankExist(string bankcode)
        {
            if (bankcode == "00000") { lblError.Text = "请选择银行。"; return false; }
            int[] nPos = new int[2]; string bankno = "";
            if (!MDATASQL.getCardPosition(bankcode, ref nPos, ref bankno))
            {
                lblError.Text = "从数据库查找卡片位置失败。";
                return false;
            }
            PrintParameter printerParam = PrintParameter.GetInstance();
            printerParam.nCardPosition = nPos[0];
            printerParam.nCartRidge = nPos[1];
            printerParam.BankNO = bankno;
            if (!F8SAN.GetInstance().CheckSlot(printerParam.nCartRidge, printerParam.nCardPosition, F8SAN.TP_PreMadeCard))
            {
                lblError.Text = "指定位置的卡槽无卡。";
                string dfds = string.Format("银行：{0},nslot:{1},nblock:{2}", bankno, printerParam.nCardPosition, printerParam.nCartRidge);
                Utility.WriteLog(dfds);
                return false;
            }
            return true;
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            ssi.rylb = combox_person_categories.SelectedValue.ToString();
            ssi.ryzt = combox_person_status.SelectedValue.ToString();
            ssi.hkxz = combox_addr_nature.SelectedValue.ToString();
            //ssi.strIsCollege = cbox_is_college.SelectedValue.ToString();
            ssi.phone = tbox_apply_phone.Text;
            //ssi.sqbh = tbox_apply_sqbh.Text;
            //ssi.cbdw = tbox_apply_cbdw.Text;
            //ssi.dwbh = tbox_apply_dwbh.Text;
            //ssi.universitySeries = tbox_apply_series.Text;
            //ssi.dwbh = tboxDwbh.Text;
            //string bankcode = combox_bank.SelectedValue.ToString();
            //if (!isBankExist(bankcode)) {
            //    if (bankcode != "00000")
            //        nCountDown = nDJS2;
            //    return;
            //}
            //ssi.bankcode = bankcode;

            if (ssi.rylb == "0" || ssi.ryzt == "0" || KaGuan.BANK == "" || ssi.hkxz == "0") {
                lblError.Text = "提交信息有空白，请检查！";
                return;
            }

            if (ssi.rylb == "1") {
                if (ssi.cbdw == "" || ssi.dwbh == "") {
                    lblError.Text = "单位信息不能为空！";
                    return;
                }
            }
            else if (ssi.rylb == "4") {
                //if (ssi.strIsCollege == "0") {

                //}
                //else if (ssi.strIsCollege == "1" && ssi.universitySeries == "") {
                //    lbl_apply_context.Text = "请填写学校所在系";
                //    return;
                //}
                //else if (ssi.strIsCollege == "2") {
                //    lbl_apply_context.Text = "请选择是否大学生";
                //    return;
                //}
                //ssi.className = tbox_apply_class.Text;
                //if (ssi.cbdw == "" || ssi.dwbh == "" || ssi.className == "") {
                //    lbl_apply_context.Text = "学校、班级不能为空！";
                //    return;
                //}
            }
            else {
                if (ssi.sqbh == "") {
                    lblError.Text = "社区编号不能为空！";
                    return;
                }
            }
            if (ssi.phone.Length != 11) {
                lblError.Text = "手机号不足11位，请检查！";
                return;
            }

            Thread th = new Thread(SubmitApply);
            th.Start();
        }

        void SubmitApply() {
            string error = "";
            try {
                SSCTable ssc = SSCTable.getSSCTable();
                ssc.Name = ssi.ownerName;
                ssc.IDCard = ssi.ID;
                ssc.Sex = ssi.sex;

                if (ssi.mdbQryResult == "") {
                    //int nRet = KaGuan.getStatus();
                    //if (nRet == 0) {
                    //    error = "用户已经有社保卡，无法进行个人申请！";
                    //    nCountDown = nDJS2;
                    //    return;
                    //}
                    //else if (nRet == 2) {
                    //    error = KaGuan.g_strErrMsg;
                    //    nCountDown = nDJS2;
                    //    return;
                    //}
                    if (!KaGuan.sqsbk()) {
                        error = KaGuan.g_strErrMsg;
                        nCountDown = nDJS2;
                        return;
                    }
                    ssc.nPro = 1;  // 注意：个人申领制卡进度从1开始，即从即制卡标注开始
                }
                else {
                    // 二次制卡 跳过检测卡片状态
                    ssc.nPro = int.Parse(ssi.mdbQryResult.Split('|')[2]);
                }

                SSCSQL.Insert();
                FrmMain.b_getPwdEnable = true;

                BeginInvoke((MethodInvoker)delegate () {
                    timer1.Enabled = false;
                    FrmService fs = new FrmService();
                    FormMng.Show(this, fs);
                    this.Hide();
                });
            }
            catch (Exception ex) {
                error = ex.Message;
            }
            finally {
                if (!string.IsNullOrEmpty(error)) {
                    BeginInvoke((MethodInvoker)delegate () { lblError.Text = error; });
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            FormMng.ShowMain(this);
            this.Hide();
        }
        private void tbox_apply_phone_TextChanged(object sender, EventArgs e)
        {
            if (tbox_apply_phone.Text.Length == 11)
                tbox_apply_phone.ForeColor = ColorTranslator.FromHtml("#0f539e");
            else
                tbox_apply_phone.ForeColor = Color.Black;
        }
        

        //加载提交信息下拉框
        void comboxList()
        {
            //combox_bank.DataSource = FrmConfig.gdt_dynamic_bank;
            //combox_bank.ValueMember = "bankcode";
            //combox_bank.DisplayMember = "bankname";
            //if (combox_bank.Items.Count != 0) {
            //    combox_bank.SelectedIndex = 0;
            //}
            combox_bank.Items.Add(KaGuan.getBankName(Global.GetInstance().BankCode));

            DataTable dt_person_categories = new DataTable();
            dt_person_categories.Columns.Add("number", typeof(String));
            dt_person_categories.Columns.Add("chnname", typeof(String));
            //dt_person_categories.Rows.Add("1", "城镇职工");
            dt_person_categories.Rows.Add("2", "城镇居民");
            dt_person_categories.Rows.Add("3", "农村居民");
            //dt_person_categories.Rows.Add("4", "学生");
            dt_person_categories.Rows.Add("0", "");
            combox_person_categories.DataSource = dt_person_categories;
            combox_person_categories.ValueMember = "number";
            combox_person_categories.DisplayMember = "chnname";
            combox_person_categories.SelectedIndex = 0;

            DataTable dt_person_status = new DataTable();
            dt_person_status.Columns.Add("number", typeof(String));
            dt_person_status.Columns.Add("chnname", typeof(String));
            //dt_person_status.Rows.Add("1", "在职");
            dt_person_status.Rows.Add("2", "退休");
            dt_person_status.Rows.Add("9", "其他");
            dt_person_status.Rows.Add("0", "");
            combox_person_status.DataSource = dt_person_status;
            combox_person_status.ValueMember = "number";
            combox_person_status.DisplayMember = "chnname";
            combox_person_status.SelectedIndex = 1;

            DataTable dt_addr_nature = new DataTable();
            dt_addr_nature.Columns.Add("number", typeof(String));
            dt_addr_nature.Columns.Add("chnname", typeof(String));
            dt_addr_nature.Rows.Add("11", "本市非农业");
            dt_addr_nature.Rows.Add("12", "外市非农业");
            dt_addr_nature.Rows.Add("21", "本市农业");
            dt_addr_nature.Rows.Add("22", "外市农业");
            dt_addr_nature.Rows.Add("30", "台港澳人员");
            dt_addr_nature.Rows.Add("40", "外籍人士");
            dt_addr_nature.Rows.Add("0", "");
            combox_addr_nature.DataSource = dt_addr_nature;
            combox_addr_nature.ValueMember = "number";
            combox_addr_nature.DisplayMember = "chnname";
            combox_addr_nature.SelectedIndex = 0;
        }

        FrmCSQ fs2 = new FrmCSQ();
        static public bool b_select_sqbh = false;
        static public string strSqbh = "";
        static public string strSqcmc = "";
        private void tbox_apply_sqbh_Click(object sender, EventArgs e)
        {
            b_select_sqbh = false;
            fs2.Show();
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

            if (b_select_sqbh) {
                tbox_apply_sqbh.Text = strSqcmc;
                //tbox_apply_phone.Focus();
                ssi.sqbh = strSqbh;
                ssi.sqcmc = strSqcmc;
            }
        }

    }
}
