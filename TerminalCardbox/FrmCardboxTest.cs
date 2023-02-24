using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHBSS
{
    public partial class FrmCardboxTest : Form
    {
        F5SAN f5san = F5SAN.GetInstance();
        Global gb = Global.GetInstance();

        DataTable gdt_test2_printer = new DataTable();
        public FrmCardboxTest()
        {
            InitializeComponent();
            comboxList();
            F5CtrlEnabled(false);
        }

        void comboxList()
        {
            cbox_car_2printer.Items.Add("1");
            cbox_car_2printer.Items.Add("2");
            cbox_car_2printer.SelectedIndex = 0;

            cbox_printer_2car.Items.Add("1");
            cbox_printer_2car.Items.Add("2");
            cbox_printer_2car.SelectedIndex = 0;

            gdt_test2_printer.Columns.Add("printerno", typeof(String));
            gdt_test2_printer.Columns.Add("display", typeof(String));
            gdt_test2_printer.Rows.Add("1", "彩印机1");
            gdt_test2_printer.Rows.Add("2", "彩印机2");
            cbox_test2_printer.DataSource = gdt_test2_printer;
            cbox_test2_printer.DisplayMember = "display";
            cbox_test2_printer.ValueMember = "printerno";
            cbox_test2_printer.SelectedIndex = 0;
        }

        /// <summary>
        /// 打开/关闭连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_close_Click(object sender, EventArgs e)
        {
            if (btn_open_close.Text == "建立连接") {
                // 建立连接  --测试
                if (f5san.F5Connect() != 0) { lbl_f5san_context.Text = f5san.strError; return; }

                ShowRemain();

                btn_open_close.Text = "断开连接";
                F5CtrlEnabled(true);
                lbl_f5san_context.Text = "连接成功。";
            }
            else {
                if (f5san.F5Disconnect() != 0) { lbl_f5san_context.Text = f5san.strError; return; }
                btn_open_close.Text = "建立连接";
                F5CtrlEnabled(false);
            }
        }

        void ShowRemain()
        {
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

            bool bret = f5san.GetSensorStatus();
            if (!bret) return;
            if (f5san.BoxStatus == F5SAN.BoxCard)
            {
                lbl_box_hascards.Text = "卡盒有无卡：有卡";
            }
            else if (f5san.BoxStatus == F5SAN.BoxEmpty)
            {
                lbl_box_hascards.Text = "卡盒有无卡：无卡";
            }
            if (f5san.ResycleStatus == F5SAN.ResycleEmpty)
            {
                lbl_recycle_status.Text = "回收箱：无卡";
            }
            else if (f5san.ResycleStatus == F5SAN.ResycleCard)
            {
                lbl_recycle_status.Text = "回收箱：有卡";
            }
            else if (f5san.ResycleStatus == F5SAN.ResycleFull)
            {
                lbl_recycle_status.Text = "回收箱：箱满";
            }

            //if (!Solid510.bsolidopen0) {
            //    if (Solid510.Solid510_SBSOpen() != 0)
            //        lbl_f5san_context.Text = "彩印机1连接失败。";
            //    else
            //        Solid510.bsolidopen0 = true;
            //}
            //if (!Solid510.bsolidopen1) {
            //    if (Solid510.Solid510_SBSOpen(1) != 0)
            //        lbl_f5san_context.Text = "彩印机2连接失败。";
            //    else
            //        Solid510.bsolidopen1 = true;
            //}
        }

        void F5CtrlEnabled(bool enabled)
        {
            btn_box_2car.Enabled = enabled;
            btn_car_2printer.Enabled = enabled;
            btn_printer_2car.Enabled = enabled;
            btn_car_2reader.Enabled = enabled;
            // btn_reader_2car.Enabled = enabled;
            // btn_reader_out.Enabled = enabled;
            btn_car_2recycle.Enabled = enabled;
            btn_f5san_reset.Enabled = enabled;
            btn_get_printersn.Enabled = enabled;
            btn_cardtest.Enabled = enabled;
            btn_reflesh.Enabled = enabled;
        }

        /// <summary>
        /// 模组全面复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_f5san_reset_Click(object sender, EventArgs e)
        {
            if (!f5san.reset_all()) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "全面复位成功。";
        }

        /// <summary>
        /// 小车到回收箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_car_2recycle_Click(object sender, EventArgs e)
        {
            if (!f5san.car2recycle()) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从小车到回收箱成功。";
        }
        /// <summary>
        /// 卡盒到小车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_box_2car_Click(object sender, EventArgs e)
        {
            // 查询发卡机卡箱状态
            if (!f5san.GetSensorStatus()) { lbl_f5san_context.Text = f5san.strError; return; }
            if (f5san.BoxStatus == F5SAN.BoxEmpty)
            {
                lbl_f5san_context.Text = "卡盒空，请放卡。";
                return;
            }
            if (!f5san.box2car()) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从卡箱到小车成功。";
        }
        /// <summary>
        /// 小车到打印机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_car_2printer_Click(object sender, EventArgs e)
        {
            string strprinter = cbox_car_2printer.SelectedItem.ToString();
            int nprinter = int.Parse(strprinter);
            bool bopen = false;
            if (nprinter == 1)
                bopen = Solid510.OpenPrinter(gb.PrinterSN1);
            else if (nprinter == 2)
                bopen = Solid510.OpenPrinter(gb.PrinterSN2);

            if (!bopen) {
                lbl_f5san_context.Text = "指定的彩印机连接失败。";
                return;
            }
            if (!f5san.car2printer(nprinter)) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从小车到彩印机成功。";
        }

        /// <summary>
        /// 打印机到小车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                lbl_f5san_context.Text = "指定的彩印机连接失败。";
                return;
            }
            if (!f5san.printer2car(nprinter)) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从彩印机到小车成功。";
        }

        /// <summary>
        /// 小车到读写器通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_car_2reader_Click(object sender, EventArgs e)
        {
            if (!f5san.car2reader()) { lbl_f5san_context.Text = f5san.strError; return; }
            if (!f5san.reader2out()) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从小车出卡成功。";
        }
        /// <summary>
        /// 读写器通道到小车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reader_2car_Click(object sender, EventArgs e)
        {
            if (!f5san.reader2car()) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从读写器到小车成功。";
        }
        /// <summary>
        /// 读写器通道出卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reader_out_Click(object sender, EventArgs e)
        {
            if (!f5san.reader2out()) { lbl_f5san_context.Text = f5san.strError; return; }
            lbl_f5san_context.Text = "从读写器出卡成功。";
        }

        /// <summary>
        /// 获取打印机序列号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                lbl_f5san_context.Text = SN;
                Utility.WriteLog(SN);
            }
            else {
                lbl_f5san_context.Text = "打印机打开失败。";
            }
        }

        /// <summary>
        /// 卡片测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cardtest_Click(object sender, EventArgs e)
        {
            int nprinter;
            int nPort;
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
                lbl_f5san_context.Text = "打印机无效";
                return;
            }


            if (!bopen) {
                lbl_f5san_context.Text = Solid510.strError;
                return;
            }
            
            if (!f5san.box2car()) { lbl_f5san_context.Text = f5san.strError; return; }
            Thread.Sleep(50);

            if (!f5san.car2printer(nprinter)) { lbl_f5san_context.Text = f5san.strError; return; }
            Thread.Sleep(Global.SleepTime);

            if (checkBox_print.Checked) {
                Thread.Sleep(1500);
                string picpath = System.Environment.CurrentDirectory + "\\pic\\test.jpg";
                Solid510.PrintCard("王梁奎", "450802*********1282", "A01234567", picpath);
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
            if (!f5san.printer2car(nprinter)) { lbl_f5san_context.Text = f5san.strError; return; }
            Thread.Sleep(50);

            if (!f5san.car2reader()) { lbl_f5san_context.Text = f5san.strError; return; }
            Thread.Sleep(50);

            if (!f5san.reader2out()) { lbl_f5san_context.Text = f5san.strError; return; }

            if (string.IsNullOrEmpty(writeinfo) && string.IsNullOrEmpty(readinfo)) {
                lbl_f5san_context.Text = "走卡测试完成，请从持卡口取卡。";
            }
            else if (!string.IsNullOrEmpty(writeinfo)) {
                lbl_f5san_context.Text = writeinfo + "走卡测试完成，请从持卡口取卡。";
            }
            else {
                lbl_f5san_context.Text = readinfo + "走卡测试完成，请从持卡口取卡.|" + SiCard.siOutInfo;
            }

            ShowRemain();
        }


        /// <summary>
        /// 刷新余量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reflesh_Click(object sender, EventArgs e)
        {
            ShowRemain();
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btn_open_close.Text == "断开连接")
                f5san.F5Disconnect();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void btn_read_idcard_Click(object sender, EventArgs e)
        {
            if (!SiCard.ReadIDCard())
            {
                lbl_f5san_context.Text = SiCard.error;
                return;
            }
            lbl_f5san_context.Text = SiCard.idcardinfo;
        }

        private void btn_read_sicard_Click(object sender, EventArgs e)
        {
            string strprinter = cbox_car_2printer.SelectedItem.ToString();
            int nprinter = int.Parse(strprinter);
            int nPort;
            if (nprinter == 1)
            {
                nPort = Global.GetInstance().RWPort1;
            }
            else if (nprinter == 2)
            {
                nPort = Global.GetInstance().RWPort2;
            }
            else
            {
                lbl_f5san_context.Text = "读写器无效";
                return;
            }
            string readinfo;
            if (SiCard.ReadSiCard(nPort) == 0)
            {
                readinfo = "读卡成功|" + SiCard.siOutInfo;
            }
            else
            {
                readinfo = "读卡失败|" + SiCard.error;
            }
            lbl_f5san_context.Text = readinfo;
        }

        private void FrmCardboxTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            FrmMain.b_getPwdEnable = true;
            e.Cancel = true;
            FormMng.ShowMain(this);
            this.Hide();
        }

    }
}
