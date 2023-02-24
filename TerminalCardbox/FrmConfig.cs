using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmConfig : Form
    {
        public static DataTable gdt_dynamic_bank = new DataTable();
        public DataTable gdt_test1_pan = new DataTable();
        public DataTable gdt_test2_printer = new DataTable();
        public static int gs_nBox;
        string iniPath;
        public List<Thread> threadPool;
        public FrmConfig()
        {
            InitializeComponent();
            // 配置文件初始化
            iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            comboxList();
            LoadThreadPool();
        }

        void LoadThreadPool()
        {
            threadPool = new List<Thread>();
            Thread thRemain = new Thread(ShowBoxRemain);   // 查看余量
            Thread thLoading = new Thread(Loading);         // 装预制卡
            Thread thRecycle = new Thread(CardRecycle);     // 清预制卡
            Thread thClearAll = new Thread(ClearAll);        // 清除所有卡片
            Thread thFeedSicard = new Thread(FeedSicard);      // 装成品卡
            Thread thRecycleSiCard = new Thread(RecycleSiCard);   // 清成品卡
            threadPool.Add(thRemain);
            threadPool.Add(thLoading);
            threadPool.Add(thRecycle);
            threadPool.Add(thClearAll);
            threadPool.Add(thFeedSicard);
            threadPool.Add(thRecycleSiCard);
        }
        private void FrmConfig_Load(object sender, EventArgs e)
        {
            F8CtrlEnabled(false);
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
        void comboxList()
        {
            gdt_dynamic_bank.Columns.Add("bankcode", typeof(String));
            gdt_dynamic_bank.Columns.Add("bankname", typeof(String));
            cbox_dynamic_bank.DataSource = gdt_dynamic_bank;
            cbox_dynamic_bank.DisplayMember = "bankname";
            cbox_dynamic_bank.ValueMember = "bankcode";

            cbox_recycle_dynamic_bank.DataSource = gdt_dynamic_bank;
            cbox_recycle_dynamic_bank.DisplayMember = "bankname";
            cbox_recycle_dynamic_bank.ValueMember = "bankcode";

            Fill_gdt_dynamic_bank();
            if(gdt_dynamic_bank.Rows.Count > 0) {
                cbox_dynamic_bank.SelectedIndex = 0;
                cbox_recycle_dynamic_bank.SelectedIndex = 0;
            }

            gdt_test1_pan.Columns.Add("blockcode", typeof(String));
            gdt_test1_pan.Columns.Add("blockname", typeof(String));
            gdt_test1_pan.Rows.Add("0", "存入卡盘0");
            gdt_test1_pan.Rows.Add("1", "存入卡盘1");
            cbox_test1_pan.DataSource = gdt_test1_pan;
            cbox_test1_pan.DisplayMember = "blockname";
            cbox_test1_pan.ValueMember = "blockcode";
            cbox_test1_pan.SelectedIndex = 0;

            gdt_test2_printer.Columns.Add("printerno", typeof(String));
            gdt_test2_printer.Columns.Add("display", typeof(String));
            gdt_test2_printer.Rows.Add("1", "彩印机1");
            gdt_test2_printer.Rows.Add("2", "彩印机2");
            cbox_test2_printer.DataSource = gdt_test2_printer;
            cbox_test2_printer.DisplayMember = "display";
            cbox_test2_printer.ValueMember = "printerno";
            cbox_test2_printer.SelectedIndex = 0;

            cbox_pan_2car.Items.Add("0");
            cbox_pan_2car.Items.Add("1");
            cbox_pan_2car.SelectedIndex = 0;

            cbox_printer_2car.Items.Add("1");
            cbox_printer_2car.Items.Add("2");
            cbox_printer_2car.SelectedIndex = 0;

            cbox_car_2pan.Items.Add("0");
            cbox_car_2pan.Items.Add("1");
            cbox_car_2pan.SelectedIndex = 0;

            cbox_car_2printer.Items.Add("1");
            cbox_car_2printer.Items.Add("2");
            cbox_car_2printer.SelectedIndex = 0;

        }
       

        /************************************************************************
         * 密码键盘操作
         ************************************************************************/
        public void Init()
        {
            if(threadPool[0].ThreadState == System.Threading.ThreadState.Stopped) {
                threadPool[0] = new Thread(ShowBoxRemain);
            }
            threadPool[0].Start();
        }

        public void lblRemainEmpty()
        {
            lbl_bank01_remain.Text = "";
            lbl_bank02_remain.Text = "";
            lbl_bank03_remain.Text = "";
            lbl_bank04_remain.Text = "";
            lbl_bank05_remain.Text = "";
            lbl_bank06_remain.Text = "";
            lbl_bank07_remain.Text = "";
            lbl_bank08_remain.Text = "";
            lbl_bank09_remain.Text = "";
            lbl_bank10_remain.Text = "";
            lbl_bank11_remain.Text = "";
            lbl_bank12_remain.Text = "";
            lbl_bank13_remain.Text = "";
            lbl_bank14_remain.Text = "";
            lbl_bank15_remain.Text = "";
            lbl_bank16_remain.Text = "";
            lbl_bank17_remain.Text = "";
            lbl_bank18_remain.Text = "";
            lbl_bank19_remain.Text = "";
            lbl_bank20_remain.Text = "";
        }

        public void ShowBoxRemain()
        {
            BeginInvoke((MethodInvoker)delegate ()
            {
                lblRemainEmpty();
            });

            DataTable dtEachBank = new DataTable();
            if (MDATASQL.getEachBankSum(ref dtEachBank)) {
                int count = dtEachBank.Rows.Count;
                for (int i = 0; i < count; i++) {
                    foreach (Control clbl in Controls) {
                        if (clbl.GetType().Equals(typeof(Label))) {
                            string n = clbl.Name.Substring(8, 2);
                            if(n == ( i + 1 ).ToString("00")) {
                                string text = MDATASQL.getBankName(dtEachBank.Rows[i][0].ToString()) + ": " + dtEachBank.Rows[i][1].ToString();
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    clbl.Text = text;
                                });
                                break;
                            }                               
                        }
                    }
                }
            }

            //int iRet = -1;
            bool bOpenPrinter = false;
            bOpenPrinter = Solid510.OpenPrinter(gb.PrinterSN1);
            if (bOpenPrinter) {
                int nRemain1 = Solid510.Solid510_GetRibbonRemain();
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_ribbon_remain1.Text = "彩印机1色带余量：" + nRemain1.ToString();
                });
            }
            else {
                Utility.WriteLog(Solid510.strError);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_ribbon_remain1.Text = "彩印机1色带余量：故障";
                });
            }

            bOpenPrinter = Solid510.OpenPrinter(gb.PrinterSN2);
            if (bOpenPrinter) {
                int nRemain2 = Solid510.Solid510_GetRibbonRemain();
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_ribbon_remain2.Text = "彩印机2色带余量：" + nRemain2.ToString();
                });
            }
            else {
                Utility.WriteLog(Solid510.strError);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_ribbon_remain2.Text = "彩印机2色带余量：故障";
                });
            }
        }
        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            FrmMain.b_getPwdEnable = true;
            e.Cancel = true;
            //Fill_gdt_dynamic_bank();
            //if (gdt_dynamic_bank.Rows.Count > 0)
            //    cbox_recycle_dynamic_bank.SelectedIndex = 0;
            FormMng.ShowMain(this);
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(btn_open_close.Text == "断开连接")
                f8san.F8Disconnect();
            //Process.Start("C:\\Windows\\explorer.exe");
            bstopclear = true;
            bstopfeed = true;
            bstopfeedsicard = true;
            bstoprecycle = true;
            bstopreyclesicard = true;
            foreach (Thread th in threadPool) {
                if (th != null && th.ThreadState != System.Threading.ThreadState.Unstarted) {
                    if ((System.Threading.ThreadState.WaitSleepJoin |
                        System.Threading.ThreadState.Background) == (th.ThreadState)) {
                        th.Interrupt();
                    }
                    th.Join();
                }
            }
            Thread.Sleep(500);
            //Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 获取所有银行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbox_dynamic_bank_MouseDown(object sender, MouseEventArgs e)
        {
            Fill_gdt_dynamic_bank();
            if (gdt_dynamic_bank.Rows.Count > 0)
                cbox_dynamic_bank.SelectedIndex = 0;
        }

        void Fill_gdt_dynamic_bank()
        {
            gdt_dynamic_bank.Rows.Clear();
            DataTable dtBanks = new DataTable();
            int nBanks = MDATASQL.getAllBank(ref dtBanks);
            if (nBanks > 0) {
                for (int i = 0; i < nBanks; i++) {
                    gdt_dynamic_bank.Rows.Add(dtBanks.Rows[i][0], MDATASQL.getBankName(dtBanks.Rows[i][0].ToString()));
                }
            }
        }


        F8SAN f8san = F8SAN.GetInstance();
        Global gb = Global.GetInstance();

        int CardLoading(int nblock, ref string error)
        {
            int nRet = 0;
            int nslot = f8san.CheckPan(nblock, false);
            if (nslot < 0) { error = f8san.strError; return 1; }

            // 卡箱到小车
            if (!f8san.box2car()) { error = f8san.strError; return 2; }
            Thread.Sleep(50);

            //// 小车到读写器
            //if (!f8san.car2reader()) { error = f8san.strError; f8san.car2recycle(); return 3; }
            ////Thread.Sleep(50);

            nRet = f8san.AddBankData();
           //if (  nRet == (int)GetBankErr.resetfail) {
           //     error = f8san.strError;
           //     //if (f8san.reader2car()) {
           //     //    f8san.car2recycle();
           //     //}
           //     f8san.car2recycle();
           //     return 5;
           // }
           // else if(nRet == (int)GetBankErr.notsicard) {
           //     nRet = 6;
           // }

            //// 读写器到小车
            //if (!f8san.reader2car()) { error = f8san.strError; return 4; }
            //Thread.Sleep(50);

            // 小车到卡盘
            if (!f8san.car2pan(nblock, nslot, F8SAN.TP_PreMadeCard)) { error = f8san.strError; f8san.car2recycle(); return 5; }

            return 0;
        }
        private void btn_test1_Click(object sender, EventArgs e)
        {
            // 查询卡盘状态
            string strBlock = cbox_test1_pan.SelectedValue.ToString();
            int nblock = int.Parse(strBlock);
            string error = "";
            int nRet = CardLoading(nblock, ref error);
            if (nRet != 0) { lbl_f8san_context.Text = error; return; }
            //if (nRet == 5 ||(nRet != 0 && nRet != 6)) { lbl_f8san_context.Text = error; return; }
            lbl_f8san_context.Text = "存入卡盘成功。";
        }


        private void btn_test2_Click(object sender, EventArgs e)
        {
            int nprinter, nblock, nslot;
            int nPort;
            if (cbox_dynamic_bank.Items.Count == 0) return;
            string strBankCode = cbox_dynamic_bank.SelectedValue.ToString();
            string strprinter = cbox_test2_printer.SelectedValue.ToString();
            nprinter = int.Parse(strprinter);
            bool bopen = false;
            if (nprinter == 1) {
                nPort = Global.GetInstance().RWPort1;
                bopen = Solid510.OpenPrinter(gb.PrinterSN1);
            }
            else if (nprinter == 2) {
                nPort = Global.GetInstance().RWPort2;
                bopen = Solid510.OpenPrinter(gb.PrinterSN2);
            }
            else {
                lbl_f8san_context.Text = "打印机无效";
                return;
            }


            if (!bopen) {
                lbl_f8san_context.Text = Solid510.strError;
                return;
            }

            int[] nPos = new int[2]; string bankno = "";
            if (!MDATASQL.getCardPosition(strBankCode, ref nPos, ref bankno)) { lbl_f8san_context.Text = "从数据库查找卡片位置失败。"; return; }
            nslot = nPos[0];
            nblock = nPos[1];

            if (!f8san.CheckSlot(nblock, nslot, F8SAN.TP_PreMadeCard)) { lbl_f8san_context.Text = f8san.strError; return; }

            if (!f8san.pan2car(nblock, nslot, F8SAN.TP_PreMadeCard)) { lbl_f8san_context.Text = f8san.strError; return; }
            Thread.Sleep(50);

            if (!f8san.car2printer(nprinter)) { lbl_f8san_context.Text = f8san.strError; return; }
            Thread.Sleep(Global.SleepTime);

            if (checkBox_print.Checked) {
                Thread.Sleep(1500);
                string picpath = System.Environment.CurrentDirectory + "\\pic\\test.jpg";
                Solid510.PrintCard("黄敏华", "450802*********1282", "A01234567", picpath);
            }
            string writeinfo = string.Empty;
            string readinfo = string.Empty;
            if (checkBox_readcard.Checked) {
                if (SiCard.ReadSiCard(nPort) == 0) {
                    readinfo = "读卡成功|";
                }
                else {
                    readinfo = "读卡失败|" + SiCard.error + "|";
                }
            }
            if (checkBox_writecard.Checked) {
                if (SiCard.PersonalizationTest(nPort) == 0) {
                    writeinfo = "写卡成功|";
                }
                else {
                    writeinfo = "写卡失败|" + SiCard.error + "|";
                }
            }
            if (!f8san.printer2car(nprinter)) { lbl_f8san_context.Text = f8san.strError; return; }
            Thread.Sleep(50);

            if (!f8san.car2reader()) { lbl_f8san_context.Text = f8san.strError; return; }
            Thread.Sleep(50);

            if (!f8san.OutCardFromReader()) { lbl_f8san_context.Text = f8san.strError; return; }

            if (string.IsNullOrEmpty(writeinfo) && string.IsNullOrEmpty(readinfo)) {
                lbl_f8san_context.Text = "走卡测试完成，请从持卡口取卡。";
            }
            else if(!string.IsNullOrEmpty(writeinfo)) {
                lbl_f8san_context.Text = writeinfo + "走卡测试完成，请从持卡口取卡。";
            }
            else {
                lbl_f8san_context.Text = readinfo + "走卡测试完成，请从持卡口取卡.|" + SiCard.siOutInfo;
            }
            
            cbox_dynamic_bank.Refresh();
        }

        private void btn_box_2car_Click(object sender, EventArgs e)
        {
            if (!f8san.box2car()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从卡箱到小车成功。";
        }
        private void btnReadBank_Click(object sender, EventArgs e)
        {
            string bankno = string.Empty;
            if (f8san.GetBankNum(ref bankno) != 0) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "银行卡号：" + bankno;
        }
        private void btn_pan_2car_Click(object sender, EventArgs e)
        {
            // 查询卡盘状态
            string strBlock = cbox_pan_2car.SelectedItem.ToString();
            int nblock = int.Parse(strBlock);
            int nslot = f8san.CheckPan(nblock, true);
            if (nslot < 0) { lbl_f8san_context.Text = f8san.strError; return; }

            if (!f8san.pan2car(nblock, nslot, F8SAN.TP_PreMadeCard)) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从卡盘到小车成功。";
        }

        private void btn_printer_2car_Click(object sender, EventArgs e)
        {
            string strprinter = cbox_printer_2car.SelectedItem.ToString();
            int nprinter = int.Parse(strprinter);
            bool bopen = false;
            if (nprinter == 1)
                bopen = Solid510.OpenPrinter(gb.PrinterSN1);
            else if (nprinter == 2)
                bopen = Solid510.OpenPrinter(gb.PrinterSN2);

            if (!bopen) {
                lbl_f8san_context.Text = "指定的彩印机连接失败。";
                return;
            }
            if (!f8san.printer2car(nprinter)) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从彩印机到小车成功。";
        }

        private void btn_reader_2car_Click(object sender, EventArgs e)
        {
            if (!f8san.reader2car()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从读写器到小车成功。";
        }

        private void btn_car_2pan_Click(object sender, EventArgs e)
        {
            // 查询卡盘状态
            string strBlock = cbox_car_2pan.SelectedItem.ToString();
            int nblock = int.Parse(strBlock);
            int nslot = f8san.CheckPan(nblock, false);
            if (nslot < 0) { lbl_f8san_context.Text = f8san.strError; return; }

            f8san.AddBankData();

            if (!f8san.car2pan(nblock, nslot, F8SAN.TP_PreMadeCard)) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从小车到卡盘成功。";
        }

        private void btn_car_2printer_Click(object sender, EventArgs e)
        {
            string strprinter = cbox_car_2printer.SelectedItem.ToString();
            int nprinter = int.Parse(strprinter);
            bool bopen = false;
            if(nprinter == 1) 
                bopen = Solid510.OpenPrinter(gb.PrinterSN1);
            else if (nprinter == 2)
                bopen = Solid510.OpenPrinter(gb.PrinterSN2);
            
            if (!bopen) {
                lbl_f8san_context.Text = "指定的彩印机连接失败。";
                return;
            }
            if (!f8san.car2printer(nprinter)) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从小车到彩印机成功。";
        }

        private void btn_car_2reader_Click(object sender, EventArgs e)
        {
            if (!f8san.car2reader()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从小车到读写器成功。";
        }

        private void btn_reader_out_Click(object sender, EventArgs e)
        {
            if (!f8san.OutCardFromReader()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从读写器出卡成功。";
        }

        private void btn_f8san_reset_Click(object sender, EventArgs e)
        {
            if (!f8san.reset_all()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "全面复位成功。";
        }

        private void btn_car_2recycle_Click(object sender, EventArgs e)
        {
            if (!f8san.car2recycle()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从小车到回收箱成功。";
        }

        private void btn_car_2errorbox_Click(object sender, EventArgs e)
        {
            if (!f8san.car2errorbox()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "从小车到废卡箱成功。";
        }
        private void btn_open_close_Click(object sender, EventArgs e)
        {
            if (btn_open_close.Text == "建立连接") {
                // 建立连接
                if ( f8san.F8Connect()  != 0) { lbl_f8san_context.Text = f8san.strError; return; }

                int nSlotHaveCards = 0, nSlotNoCards = 0;
                f8san.getSlotSum(ref nSlotHaveCards, ref nSlotNoCards);
                lbl_slot_hascards.Text = "卡盘有卡数量：" + nSlotHaveCards.ToString();
                lbl_slot_nocards.Text = "卡盘无卡数量：" + nSlotNoCards.ToString();

                string recyclestatus = "", errboxstatus = "";
                f8san.CheckRecycleBox(ref recyclestatus, ref errboxstatus);
                lbl_recycle_status.Text = recyclestatus;
                lbl_errbox_status.Text = errboxstatus;

                if (!Solid510.bsolidopen0) {
                    if (Solid510.Solid510_SBSOpen() != 0)
                        lbl_f8san_context.Text = "彩印机1连接失败。";
                    else
                        Solid510.bsolidopen0 = true;
                }
                if (!Solid510.bsolidopen1) {
                    if (Solid510.Solid510_SBSOpen(1) != 0)
                        lbl_f8san_context.Text = "彩印机2连接失败。";
                    else
                        Solid510.bsolidopen1 = true;
                }
                btn_open_close.Text = "断开连接";
                F8CtrlEnabled(true);
                lbl_f8san_context.Text = "连接成功。";
            }
            else {
                if(f8san.F8Disconnect() != 0) { lbl_f8san_context.Text = f8san.strError; return; }
                btn_open_close.Text = "建立连接";
                F8CtrlEnabled(false);
            }
        }

        void F8CtrlEnabled(bool enabled)
        {
            //  管理员
            if (FrmMain.sysPwd == Global.GetInstance().AdminPwd) {
                //bool enablefalse = false;
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btn_box_2car.Enabled = enabled;
                    btn_pan_2car.Enabled = enabled;
                    btn_printer_2car.Enabled = enabled;
                    btn_reader_2car.Enabled = enabled;
                    btn_car_2pan.Enabled = enabled;
                    btn_car_2printer.Enabled = enabled;
                    btn_car_2reader.Enabled = enabled;
                    btn_reader_out.Enabled = enabled;
                    btn_test1.Enabled = enabled;
                    btn_test2.Enabled = enabled;
                    btn_car_2recycle.Enabled = enabled;
                    btn_car_2errorbox.Enabled = enabled;
                    btn_clear_all.Enabled = enabled;
                    btn_get_printersn.Enabled = enabled;
                    //btn_stop_clear.Enabled = enabled;
                });
            }
            else {
                bool enablefalse = false;
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btn_box_2car.Enabled = enablefalse;
                    btn_pan_2car.Enabled = enablefalse;
                    btn_printer_2car.Enabled = enablefalse;
                    btn_reader_2car.Enabled = enablefalse;
                    btn_car_2pan.Enabled = enablefalse;
                    btn_car_2printer.Enabled = enablefalse;
                    btn_car_2reader.Enabled = enablefalse;
                    btn_reader_out.Enabled = enablefalse;
                    btn_test1.Enabled = enablefalse;
                    btn_test2.Enabled = enablefalse;
                    btn_car_2recycle.Enabled = enablefalse;
                    btn_car_2errorbox.Enabled = enablefalse;
                    btn_clear_all.Enabled = enablefalse;
                    btn_stop_clear.Enabled = enablefalse;
                    btn_get_printersn.Enabled = enablefalse;
                });
            }
            BeginInvoke((MethodInvoker)delegate ()
            {
                btn_card_recycle.Enabled = enabled;
                //btn_stop_recycle.Enabled = enabled;
                btn_feed_sicard.Enabled = enabled;
                //btn_feed_sicard_stop.Enabled = enabled;
                btn_reycle_sicard.Enabled = enabled;
                //btn_reycle_sicard_stop.Enabled = enabled;
                btn_getcard_readid.Enabled = enabled;
                btn_reflesh.Enabled = enabled;

                btn_feed_card.Enabled = enabled;
                //btn_stop_feed.Enabled = enabled;
                btn_f8san_reset.Enabled = enabled;
            });
         }

        bool bstopfeed = false;
        int nCountFeed;
        int nTestContinue = 0; // 0 弹窗  1 继续是  2 继续否
        private void btn_feed_card_Click(object sender, EventArgs e)
        {
            bstopfeed = false;
            nTestContinue = 0;
            //nCountFeed = int.Parse(tbox_feed_num.Text);
            //if (nCountFeed <= 0) return;
            nCountFeed = 1080;
            F8CtrlEnabled(false);
            btnExit.Enabled = false;
            //btn_stop_feed.Enabled = true;
            if (threadPool[1].ThreadState == System.Threading.ThreadState.Stopped) {
                threadPool[1] = new Thread(Loading);
            }
            threadPool[1].Start();
        }
        private void btn_stop_feed_Click(object sender, EventArgs e)
        {
            bstopfeed = true;
            F8CtrlEnabled(true);
            btnExit.Enabled = true;
        }

        void Loading()
        {
            try {
                int j = 0;
                int nRet = -1; string error = "";
                while (nCountFeed > 0) {
                    if (bstopfeed) return;
                    nRet = CardLoading(1, ref error);
                    if (nRet == 0) {
                        nCountFeed--;
                        j++;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            tbox_feed_num.Text = j.ToString();
                        });
                    }
                    else if (nRet == 1) {
                        while (nCountFeed > 0) {
                            if (bstopfeed) return;
                            nRet = CardLoading(0, ref error);
                            if (nRet == 0) {
                                nCountFeed--;
                                j++;
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    tbox_feed_num.Text = j.ToString();
                                });
                            }
                            else if (nRet == 6) {
                                if (nTestContinue == 0) ShowMessage();
                                if (nTestContinue == 1) continue;
                                else return;
                            }
                            else {
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    lbl_f8san_context.Text = error;
                                });
                                return;
                            }
                        }
                    }
                    else if (nRet == 6) {
                        if (nTestContinue == 0) ShowMessage();
                        if (nTestContinue == 1) continue;
                        else return;
                    }
                    else {
                        //if (nRet == 5) // 读卡器有问题
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = error;
                        });
                        return;
                    }
                }
            }
            catch(Exception ex) {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_f8san_context.Text = ex.Message;
                });
            }
            finally {
                F8CtrlEnabled(true);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btnExit.Enabled = true;
                    btn_reflesh_Click(this, new EventArgs());
                });
            }
        }
        public void ShowMessage()
        {
            string msg = "弹窗内容";
            this.Invoke(new MessageBoxShow(MessageBoxShow_F), new object[] { msg });
        }

        delegate void MessageBoxShow(string msg);
        void MessageBoxShow_F(string msg)
        {
            var dlgResult = MessageBox.Show("测试卡，请问是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(dlgResult == DialogResult.No) {
                nTestContinue = 2;
            }else {
                nTestContinue = 1;
            }
        }


        bool bstoprecycle = false;
        int nCountRecycle;
        string strRecycleBank;
        private void btn_card_recycle_Click(object sender, EventArgs e)
        {
            bstoprecycle = false;
            //nCountRecycle = int.Parse(tbox_recycle_num.Text);
            //if (nCountRecycle <= 0) return;
            nCountRecycle = 1080;
            if (cbox_recycle_dynamic_bank.Items.Count == 0) return;
            F8CtrlEnabled(false);
            btnExit.Enabled = false;
            btn_stop_recycle.Enabled = true;
            strRecycleBank = cbox_recycle_dynamic_bank.SelectedValue.ToString();
            if (threadPool[2].ThreadState == System.Threading.ThreadState.Stopped) {
                threadPool[2] = new Thread(CardRecycle);
            }
            threadPool[2].Start();
        }
        private void btn_stop_recycle_Click(object sender, EventArgs e)
        {
            bstoprecycle = true;
            F8CtrlEnabled(true);
            btnExit.Enabled = true;
        }

        void CardRecycle()
        {
            try {
                int j = 0;
                while (nCountRecycle > 0) {
                    if (bstoprecycle) return;
                    int nblock, nslot;
                    int[] nPos = new int[2]; string bankno = "";
                    if (!MDATASQL.getCardPosition(strRecycleBank, ref nPos, ref bankno)) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            //lbl_f8san_context.Text = "从数据库查找卡片位置失败。";
                            lbl_f8san_context.Text = "卡片已清完。";
                        });
                        return;
                    }
                    nslot = nPos[0];
                    nblock = nPos[1];
                    if (!f8san.CheckSlot(nblock, nslot, F8SAN.TP_PreMadeCard)) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = f8san.strError;
                        });
                        continue;
                        //return;
                    }
                    if (!f8san.pan2car(nblock, nslot, F8SAN.TP_PreMadeCard)) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = f8san.strError;
                        });
                        return;
                    }
                    if (!f8san.car2recycle()) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = f8san.strError;
                        });
                        return;
                    }
                    nCountRecycle--;
                    j++;
                    BeginInvoke((MethodInvoker)delegate ()
                    {
                        tbox_recycle_num.Text = j.ToString();
                    });
                }
            }
            catch(Exception ex) {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_f8san_context.Text = ex.Message;
                });
            }
            finally {
                F8CtrlEnabled(true);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btnExit.Enabled = true;
                    btn_reflesh_Click(this, new EventArgs());
                    cbox_recycle_dynamic_bank.Refresh();
                });
            }
        }


        bool bstopclear = false;
        int nCountClear;
        private void btn_clear_all_Click(object sender, EventArgs e)
        {
            bstopclear = false;
            nCountClear = 1080;
            F8CtrlEnabled(false);
            btnExit.Enabled = false;
            btn_stop_clear.Enabled = true;
            if (threadPool[3].ThreadState == System.Threading.ThreadState.Stopped) {
                threadPool[3] = new Thread(ClearAll);
            }
            threadPool[3].Start();
        }

        private void btn_stop_clear_Click(object sender, EventArgs e)
        {
            bstopclear = true;
            F8CtrlEnabled(true);
            btnExit.Enabled = true;
        }

        void ClearAll()
        {
            try {
                int j = 0;
                int nRet = -1; string error = "";
                while (nCountClear > 0) {
                    if (bstopclear) return;
                    nRet = CardClear(1, ref error);
                    if (nRet == 0) {
                        nCountClear--;
                        j++;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            tbox_clear_num.Text = j.ToString();
                        });
                    }
                    else if (nRet == 1) {
                        while (nCountClear > 0) {
                            if (bstopclear) return;
                            nRet = CardClear(0, ref error);
                            if (nRet == 0) {
                                nCountClear--;
                                j++;
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    tbox_clear_num.Text = j.ToString();
                                });
                            }
                            else {
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    lbl_f8san_context.Text = error;
                                });
                                return;
                            }
                        }
                    }
                    else {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = error;
                        });
                        return;
                    }
                }
            }
            catch (Exception ex) {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_f8san_context.Text = ex.Message;
                });
            }
            finally {
                F8CtrlEnabled(true);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btnExit.Enabled = true;
                    btn_reflesh_Click(this, new EventArgs());
                });
                MDATASQL.DeletePreCardAll();
                MDATASQL.DeleteSiCardAll();
            }
        }

        int CardClear(int nblock, ref string error)
        {
            int nslot = f8san.CheckPan(nblock, true);
            if (nslot < 0) { error = f8san.strError; return 1; }

            if (!f8san.pan2car(nblock, nslot, F8SAN.TP_ClearAll)) { error = f8san.strError; return 2; }

            if (!f8san.car2recycle()) {
                error = f8san.strError;
                return 3;
            }

            return 0;
        }

        private void cbox_recycle_dynamic_bank_MouseDown(object sender, MouseEventArgs e)
        {
            Fill_gdt_dynamic_bank();
            if (gdt_dynamic_bank.Rows.Count > 0)
                cbox_recycle_dynamic_bank.SelectedIndex = 0;
        }

        private void btn_reflesh_Click(object sender, EventArgs e)
        {
            ShowBoxRemain();
            int nSlotHaveCards = 0, nSlotNoCards = 0;
            f8san.getSlotSum(ref nSlotHaveCards, ref nSlotNoCards);
            lbl_slot_hascards.Text = "卡盘有卡数量：" + nSlotHaveCards.ToString();
            lbl_slot_nocards.Text = "卡盘无卡数量：" + nSlotNoCards.ToString();

            string recyclestatus = "", errboxstatus = "";
            f8san.CheckRecycleBox(ref recyclestatus, ref errboxstatus);
            lbl_recycle_status.Text = recyclestatus;
            lbl_errbox_status.Text = errboxstatus;
        }

        int SiCardLoading(int nblock, ref string error)
        {
            int nslot = f8san.CheckPan(nblock, false);
            if (nslot < 0) { error = f8san.strError; return 1; }

            // 卡箱到小车
            if (!f8san.box2car()) { error = f8san.strError; return 2; }
            //Thread.Sleep(50);

            // 小车到读写器
            if (!f8san.car2reader()) { error = f8san.strError; f8san.car2recycle(); return 3; }
            Thread.Sleep(500);
            StringBuilder pOutputInfo = new StringBuilder(600);
            StringBuilder pErrMsg = new StringBuilder(600);
            //int ret1 = SiCard.Print_ReadCard(Global.GetInstance().nPort, SiCard.TP_CONTACT, pOutputInfo, pErrMsg);
            //if (ret1 != 0) {
            //    error = "卡片读取失败: " + pErrMsg.ToString();
            //    f8san.reader2car();
            //    f8san.car2recycle();
            //    return 4;
            //}

            string cardid = pOutputInfo.ToString().Split('|')[1];
            if(cardid.Length != 18) {
                error = "该卡不是成品卡，已入回收箱";
                f8san.reader2car();
                f8san.car2recycle();
                return 5;
            }
            MDATA.getInstance().CardId = cardid;

            // 读写器到小车
            if (!f8san.reader2car()) { error = f8san.strError; return 5; }
            //Thread.Sleep(50);

            // 小车到卡盘
            if (!f8san.car2pan(nblock, nslot, F8SAN.TP_SiCard)) { error = f8san.strError; f8san.car2recycle(); return 6; }

            return 0;
        }

        bool bstopfeedsicard = false;
        int nCountfeedsicard;
        private void btn_feed_sicard_Click(object sender, EventArgs e)
        {
            bstopfeedsicard = false;
            //nCountfeedsicard = int.Parse(tbox_feed_sicard_num.Text);
            //if (nCountfeedsicard <= 0) return;
            nCountfeedsicard = 1080;
            F8CtrlEnabled(false);
            btnExit.Enabled = false;
            btn_feed_sicard_stop.Enabled = true;
            if (threadPool[4].ThreadState == System.Threading.ThreadState.Stopped) {
                threadPool[4] = new Thread(FeedSicard);
            }
            threadPool[4].Start();
        }

        void FeedSicard()
        {
            try {
                int j = 0;
                int nRet = -1; string error = "";
                while (nCountfeedsicard > 0) {
                    if (bstopfeedsicard) return;
                    nRet = SiCardLoading(1, ref error);
                    if (nRet == 0) {
                        nCountfeedsicard--;
                        j++;
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            tbox_feed_sicard_num.Text = j.ToString();
                        });
                    }
                    else if (nRet == 1) {
                        while (nCountfeedsicard > 0) {
                            if (bstopfeedsicard) return;
                            nRet = SiCardLoading(0, ref error);
                            if (nRet == 0) {
                                nCountfeedsicard--;
                                j++;
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    tbox_feed_sicard_num.Text = j.ToString();
                                });
                            }
                            else {
                                BeginInvoke((MethodInvoker)delegate ()
                                {
                                    lbl_f8san_context.Text = error;
                                });
                                return;
                            }
                        }
                    }
                    else {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = error;
                        });
                        return;
                    }
                }
            }
            catch (Exception ex) {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_f8san_context.Text = ex.Message;
                });
            }
            finally {
                F8CtrlEnabled(true);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btnExit.Enabled = true;
                    btn_reflesh_Click(this, new EventArgs());
                });
            }
        }
        private void btn_feed_sicard_stop_Click(object sender, EventArgs e)
        {
            bstopfeedsicard = true;
            F8CtrlEnabled(true);
            btnExit.Enabled = true;
        }

        bool bstopreyclesicard = false;
        int nCountreyclesicard;
        private void btn_reycle_sicard_Click(object sender, EventArgs e)
        {
            bstopreyclesicard = false;
            //nCountreyclesicard = int.Parse(tbox_reycle_sicard_num.Text);
            //if (nCountreyclesicard <= 0) return;
            nCountreyclesicard = 1080;
            F8CtrlEnabled(false);
            btnExit.Enabled = false;
            btn_reycle_sicard_stop.Enabled = true;
            if (threadPool[5].ThreadState == System.Threading.ThreadState.Stopped) {
                threadPool[5] = new Thread(RecycleSiCard);
            }
            threadPool[5].Start();
            
        }
        void RecycleSiCard()
        {
            try {
                int j = 0;
                while (nCountreyclesicard > 0) {
                    if (bstopreyclesicard) return;
                    int nblock, nslot;
                    int[] nPos = new int[2];
                    if (!MDATASQL.getSiCardPosition(ref nPos)) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            //lbl_f8san_context.Text = "从数据库查找卡片位置失败。";
                            lbl_f8san_context.Text = "卡片已清完。";
                        });
                        return;
                    }
                    nslot = nPos[0];
                    nblock = nPos[1];
                    if (!f8san.CheckSlot(nblock, nslot, F8SAN.TP_SiCard)) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = f8san.strError;
                        });
                        continue;
                        //return;
                    }
                    if (!f8san.pan2car(nblock, nslot, F8SAN.TP_SiCard)) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = f8san.strError;
                        });
                        return;
                    }
                    if (!f8san.car2recycle()) {
                        BeginInvoke((MethodInvoker)delegate ()
                        {
                            lbl_f8san_context.Text = f8san.strError;
                        });
                        return;
                    }
                    nCountreyclesicard--;
                    j++;
                    BeginInvoke((MethodInvoker)delegate ()
                    {
                        tbox_reycle_sicard_num.Text = j.ToString();
                    });
                }
            }
            catch (Exception ex) {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_f8san_context.Text = ex.Message;
                });
            }
            finally {
                F8CtrlEnabled(true);
                BeginInvoke((MethodInvoker)delegate ()
                {
                    btnExit.Enabled = true;
                    btn_reflesh_Click(this, new EventArgs());
                });
            }
        }
        private void btn_reycle_sicard_stop_Click(object sender, EventArgs e)
        {
            bstopreyclesicard = true;
            F8CtrlEnabled(true);
            btnExit.Enabled = true;
        }

        private void btn_getcard_readid_Click(object sender, EventArgs e)
        {
            if (SiCard.ReadIDCard()) {
                tbox_getcard_id.Text = SiCard.userID;
            }
            string cardid = tbox_getcard_id.Text;
            if (string.IsNullOrEmpty(cardid)) {
                return;
            }

            int nblock, nslot;
            int[] nPos = new int[2];
            if (!MDATASQL.getSiCardPosition2(cardid, ref nPos)) {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lbl_f8san_context.Text = "从数据库查找卡片失败。";
                });
                return;
            }
            nslot = nPos[0];
            nblock = nPos[1];
            if (!f8san.CheckSlot(nblock, nslot, F8SAN.TP_SiCard)) { lbl_f8san_context.Text = f8san.strError; return; }

            if (!f8san.pan2car(nblock, nslot, F8SAN.TP_SiCard)) { lbl_f8san_context.Text = f8san.strError; return; }

            if (!f8san.car2reader()) { lbl_f8san_context.Text = f8san.strError; return; }
            //Thread.Sleep(50);

            if (!f8san.OutCardFromReader()) { lbl_f8san_context.Text = f8san.strError; return; }

            lbl_f8san_context.Text = "请从持卡口取卡。";
        }

        //void ReadSI()
        //{
        //    StringBuilder pOutputInfo = new StringBuilder(600);
        //    StringBuilder pErrMsg = new StringBuilder(600);
        //    int ret1 = SiCard.Print_ReadCard(Global.GetInstance().nPort, SiCard.TP_CONTACT, pOutputInfo, pErrMsg);
        //    if (ret1 != 0) {
        //        lbl_f8san_context.Text = pErrMsg.ToString(); return;
        //    }
        //    MessageBox.Show(pOutputInfo.ToString());
        //}

        private void btn_get_printersn_Click(object sender, EventArgs e)
        {
            StringBuilder sbSN1 = new StringBuilder(30);
            StringBuilder sbSN2 = new StringBuilder(30);
            int buf_len = 30;
            if (Solid510.Solid510_SBSOpen() == 0) {
                Solid510.Solid510_GetSN(sbSN1, ref buf_len);
                if (Solid510.Solid510_SBSOpen(1) == 0) {
                    Solid510.Solid510_GetSN(sbSN2, ref buf_len);
                }
                string SN1 = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(sbSN1.ToString())).Trim();
                string SN2 = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(sbSN2.ToString())).Trim();
                SN1 = SN1.Replace("?", "").Replace("RS", "");//机器有14 16位序列号的，将？乱码去掉
                SN2 = SN2.Replace("?", "").Replace("RS", "");
                string SN = string.Format("{0},{1}", SN1, SN2);
                lbl_f8san_context.Text = SN;
                Utility.WriteLog(SN);
            }
            else {
                lbl_f8san_context.Text = "打印机打开失败。";
            }
        }

        private void checkBox_readcard_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_readcard.Checked) {
                checkBox_writecard.Checked = false;
            }
        }

        private void checkBox_writecard_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_writecard.Checked) {
                checkBox_readcard.Checked = false;
            }
        }

        private void btnReadSi_Click(object sender, EventArgs e)
        {
            string strprinter = cbox_car_2printer.SelectedItem.ToString();
            int nprinter = int.Parse(strprinter);
            int nPort;
            if (nprinter == 1) {
                nPort = Global.GetInstance().RWPort1;
            }
            else if (nprinter == 2) {
                nPort = Global.GetInstance().RWPort2;
            }
            else {
                lbl_f8san_context.Text = "读写器无效";
                return;
            }
            string readinfo;
            if (SiCard.ReadSiCard(nPort) == 0) {
                readinfo = "读卡成功|" + SiCard.siOutInfo;
            }
            else {
                readinfo = "读卡失败|" + SiCard.error;
            }
            lbl_f8san_context.Text = readinfo;
        }

        private void btnOut2Reader_Click(object sender, EventArgs e)
        {
            if (!f8san.Out2Reader()) { lbl_f8san_context.Text = f8san.strError; return; }
            lbl_f8san_context.Text = "持卡口进卡到读写器通道成功";
        }

    
    }
}
