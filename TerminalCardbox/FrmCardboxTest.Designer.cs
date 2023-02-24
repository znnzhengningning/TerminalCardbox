namespace SHBSS
{
    partial class FrmCardboxTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_read_sicard = new System.Windows.Forms.Button();
            this.btn_read_idcard = new System.Windows.Forms.Button();
            this.checkBox_readcard = new System.Windows.Forms.CheckBox();
            this.checkBox_writecard = new System.Windows.Forms.CheckBox();
            this.checkBox_print = new System.Windows.Forms.CheckBox();
            this.lbl_f5san_context = new System.Windows.Forms.Label();
            this.cbox_test2_printer = new System.Windows.Forms.ComboBox();
            this.btn_cardtest = new System.Windows.Forms.Button();
            this.cbox_car_2printer = new System.Windows.Forms.ComboBox();
            this.cbox_printer_2car = new System.Windows.Forms.ComboBox();
            this.btn_get_printersn = new System.Windows.Forms.Button();
            this.btn_car_2recycle = new System.Windows.Forms.Button();
            this.btn_open_close = new System.Windows.Forms.Button();
            this.btn_f5san_reset = new System.Windows.Forms.Button();
            this.btn_car_2reader = new System.Windows.Forms.Button();
            this.btn_car_2printer = new System.Windows.Forms.Button();
            this.btn_printer_2car = new System.Windows.Forms.Button();
            this.btn_box_2car = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_recycle_status = new System.Windows.Forms.Label();
            this.lbl_box_hascards = new System.Windows.Forms.Label();
            this.btn_reflesh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbl_ribbon_remain2 = new System.Windows.Forms.Label();
            this.lbl_ribbon_remain1 = new System.Windows.Forms.Label();
            this.btn_reader_out = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_reader_out);
            this.groupBox1.Controls.Add(this.btn_read_sicard);
            this.groupBox1.Controls.Add(this.btn_read_idcard);
            this.groupBox1.Controls.Add(this.checkBox_readcard);
            this.groupBox1.Controls.Add(this.checkBox_writecard);
            this.groupBox1.Controls.Add(this.checkBox_print);
            this.groupBox1.Controls.Add(this.lbl_f5san_context);
            this.groupBox1.Controls.Add(this.cbox_test2_printer);
            this.groupBox1.Controls.Add(this.btn_cardtest);
            this.groupBox1.Controls.Add(this.cbox_car_2printer);
            this.groupBox1.Controls.Add(this.cbox_printer_2car);
            this.groupBox1.Controls.Add(this.btn_get_printersn);
            this.groupBox1.Controls.Add(this.btn_car_2recycle);
            this.groupBox1.Controls.Add(this.btn_open_close);
            this.groupBox1.Controls.Add(this.btn_f5san_reset);
            this.groupBox1.Controls.Add(this.btn_car_2reader);
            this.groupBox1.Controls.Add(this.btn_car_2printer);
            this.groupBox1.Controls.Add(this.btn_printer_2car);
            this.groupBox1.Controls.Add(this.btn_box_2car);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 16F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(744, 761);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "卡盒模组";
            // 
            // btn_read_sicard
            // 
            this.btn_read_sicard.Location = new System.Drawing.Point(524, 195);
            this.btn_read_sicard.Name = "btn_read_sicard";
            this.btn_read_sicard.Size = new System.Drawing.Size(207, 30);
            this.btn_read_sicard.TabIndex = 101;
            this.btn_read_sicard.Text = "读  卡";
            this.btn_read_sicard.UseVisualStyleBackColor = true;
            this.btn_read_sicard.Click += new System.EventHandler(this.btn_read_sicard_Click);
            // 
            // btn_read_idcard
            // 
            this.btn_read_idcard.Location = new System.Drawing.Point(29, 197);
            this.btn_read_idcard.Name = "btn_read_idcard";
            this.btn_read_idcard.Size = new System.Drawing.Size(207, 30);
            this.btn_read_idcard.TabIndex = 100;
            this.btn_read_idcard.Text = "读身份证";
            this.btn_read_idcard.UseVisualStyleBackColor = true;
            this.btn_read_idcard.Click += new System.EventHandler(this.btn_read_idcard_Click);
            // 
            // checkBox_readcard
            // 
            this.checkBox_readcard.AutoSize = true;
            this.checkBox_readcard.Location = new System.Drawing.Point(510, 248);
            this.checkBox_readcard.Name = "checkBox_readcard";
            this.checkBox_readcard.Size = new System.Drawing.Size(73, 26);
            this.checkBox_readcard.TabIndex = 99;
            this.checkBox_readcard.Text = "读卡";
            this.checkBox_readcard.UseVisualStyleBackColor = true;
            // 
            // checkBox_writecard
            // 
            this.checkBox_writecard.AutoSize = true;
            this.checkBox_writecard.Location = new System.Drawing.Point(625, 248);
            this.checkBox_writecard.Name = "checkBox_writecard";
            this.checkBox_writecard.Size = new System.Drawing.Size(73, 26);
            this.checkBox_writecard.TabIndex = 98;
            this.checkBox_writecard.Text = "写卡";
            this.checkBox_writecard.UseVisualStyleBackColor = true;
            // 
            // checkBox_print
            // 
            this.checkBox_print.AutoSize = true;
            this.checkBox_print.Location = new System.Drawing.Point(395, 248);
            this.checkBox_print.Name = "checkBox_print";
            this.checkBox_print.Size = new System.Drawing.Size(73, 26);
            this.checkBox_print.TabIndex = 97;
            this.checkBox_print.Text = "打印";
            this.checkBox_print.UseVisualStyleBackColor = true;
            // 
            // lbl_f5san_context
            // 
            this.lbl_f5san_context.Font = new System.Drawing.Font("宋体", 16F);
            this.lbl_f5san_context.ForeColor = System.Drawing.Color.Red;
            this.lbl_f5san_context.Location = new System.Drawing.Point(30, 357);
            this.lbl_f5san_context.Name = "lbl_f5san_context";
            this.lbl_f5san_context.Size = new System.Drawing.Size(701, 182);
            this.lbl_f5san_context.TabIndex = 96;
            this.lbl_f5san_context.Text = "label1";
            this.lbl_f5san_context.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbox_test2_printer
            // 
            this.cbox_test2_printer.Font = new System.Drawing.Font("宋体", 16F);
            this.cbox_test2_printer.FormattingEnabled = true;
            this.cbox_test2_printer.Location = new System.Drawing.Point(265, 279);
            this.cbox_test2_printer.Name = "cbox_test2_printer";
            this.cbox_test2_printer.Size = new System.Drawing.Size(124, 29);
            this.cbox_test2_printer.TabIndex = 95;
            // 
            // btn_cardtest
            // 
            this.btn_cardtest.Location = new System.Drawing.Point(395, 279);
            this.btn_cardtest.Name = "btn_cardtest";
            this.btn_cardtest.Size = new System.Drawing.Size(336, 30);
            this.btn_cardtest.TabIndex = 94;
            this.btn_cardtest.Text = "卡盒—>打印机—>通道—>出卡";
            this.btn_cardtest.UseVisualStyleBackColor = true;
            this.btn_cardtest.Click += new System.EventHandler(this.btn_cardtest_Click);
            // 
            // cbox_car_2printer
            // 
            this.cbox_car_2printer.Font = new System.Drawing.Font("宋体", 16F);
            this.cbox_car_2printer.FormattingEnabled = true;
            this.cbox_car_2printer.Location = new System.Drawing.Point(694, 34);
            this.cbox_car_2printer.Name = "cbox_car_2printer";
            this.cbox_car_2printer.Size = new System.Drawing.Size(37, 29);
            this.cbox_car_2printer.TabIndex = 93;
            // 
            // cbox_printer_2car
            // 
            this.cbox_printer_2car.Font = new System.Drawing.Font("宋体", 16F);
            this.cbox_printer_2car.FormattingEnabled = true;
            this.cbox_printer_2car.Location = new System.Drawing.Point(29, 116);
            this.cbox_printer_2car.Name = "cbox_printer_2car";
            this.cbox_printer_2car.Size = new System.Drawing.Size(37, 29);
            this.cbox_printer_2car.TabIndex = 92;
            // 
            // btn_get_printersn
            // 
            this.btn_get_printersn.Location = new System.Drawing.Point(29, 279);
            this.btn_get_printersn.Name = "btn_get_printersn";
            this.btn_get_printersn.Size = new System.Drawing.Size(207, 30);
            this.btn_get_printersn.TabIndex = 91;
            this.btn_get_printersn.Text = "获取打印机序列号";
            this.btn_get_printersn.UseVisualStyleBackColor = true;
            this.btn_get_printersn.Click += new System.EventHandler(this.btn_get_printersn_Click);
            // 
            // btn_car_2recycle
            // 
            this.btn_car_2recycle.Location = new System.Drawing.Point(294, 115);
            this.btn_car_2recycle.Name = "btn_car_2recycle";
            this.btn_car_2recycle.Size = new System.Drawing.Size(164, 30);
            this.btn_car_2recycle.TabIndex = 90;
            this.btn_car_2recycle.Text = "小车—>回收箱";
            this.btn_car_2recycle.UseVisualStyleBackColor = true;
            this.btn_car_2recycle.Click += new System.EventHandler(this.btn_car_2recycle_Click);
            // 
            // btn_open_close
            // 
            this.btn_open_close.Location = new System.Drawing.Point(294, 33);
            this.btn_open_close.Name = "btn_open_close";
            this.btn_open_close.Size = new System.Drawing.Size(164, 30);
            this.btn_open_close.TabIndex = 89;
            this.btn_open_close.Text = "建立连接";
            this.btn_open_close.UseVisualStyleBackColor = true;
            this.btn_open_close.Click += new System.EventHandler(this.btn_open_close_Click);
            // 
            // btn_f5san_reset
            // 
            this.btn_f5san_reset.Location = new System.Drawing.Point(294, 197);
            this.btn_f5san_reset.Name = "btn_f5san_reset";
            this.btn_f5san_reset.Size = new System.Drawing.Size(164, 30);
            this.btn_f5san_reset.TabIndex = 88;
            this.btn_f5san_reset.Text = "全面复位";
            this.btn_f5san_reset.UseVisualStyleBackColor = true;
            this.btn_f5san_reset.Click += new System.EventHandler(this.btn_f5san_reset_Click);
            // 
            // btn_car_2reader
            // 
            this.btn_car_2reader.Location = new System.Drawing.Point(524, 87);
            this.btn_car_2reader.Name = "btn_car_2reader";
            this.btn_car_2reader.Size = new System.Drawing.Size(207, 30);
            this.btn_car_2reader.TabIndex = 82;
            this.btn_car_2reader.Text = "小车—>出卡";
            this.btn_car_2reader.UseVisualStyleBackColor = true;
            this.btn_car_2reader.Click += new System.EventHandler(this.btn_car_2reader_Click);
            // 
            // btn_car_2printer
            // 
            this.btn_car_2printer.Location = new System.Drawing.Point(524, 33);
            this.btn_car_2printer.Name = "btn_car_2printer";
            this.btn_car_2printer.Size = new System.Drawing.Size(164, 30);
            this.btn_car_2printer.TabIndex = 85;
            this.btn_car_2printer.Text = "小车—>打印机";
            this.btn_car_2printer.UseVisualStyleBackColor = true;
            this.btn_car_2printer.Click += new System.EventHandler(this.btn_car_2printer_Click);
            // 
            // btn_printer_2car
            // 
            this.btn_printer_2car.Location = new System.Drawing.Point(72, 115);
            this.btn_printer_2car.Name = "btn_printer_2car";
            this.btn_printer_2car.Size = new System.Drawing.Size(164, 30);
            this.btn_printer_2car.TabIndex = 84;
            this.btn_printer_2car.Text = "打印机—>小车";
            this.btn_printer_2car.UseVisualStyleBackColor = true;
            this.btn_printer_2car.Click += new System.EventHandler(this.btn_printer_2car_Click);
            // 
            // btn_box_2car
            // 
            this.btn_box_2car.Location = new System.Drawing.Point(72, 33);
            this.btn_box_2car.Name = "btn_box_2car";
            this.btn_box_2car.Size = new System.Drawing.Size(164, 30);
            this.btn_box_2car.TabIndex = 83;
            this.btn_box_2car.Text = "卡盒—>小车";
            this.btn_box_2car.UseVisualStyleBackColor = true;
            this.btn_box_2car.Click += new System.EventHandler(this.btn_box_2car_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_recycle_status);
            this.groupBox2.Controls.Add(this.lbl_box_hascards);
            this.groupBox2.Controls.Add(this.btn_reflesh);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.lbl_ribbon_remain2);
            this.groupBox2.Controls.Add(this.lbl_ribbon_remain1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 16F);
            this.groupBox2.Location = new System.Drawing.Point(744, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 761);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "耗材状态";
            // 
            // lbl_recycle_status
            // 
            this.lbl_recycle_status.AutoSize = true;
            this.lbl_recycle_status.Font = new System.Drawing.Font("宋体", 16F);
            this.lbl_recycle_status.Location = new System.Drawing.Point(45, 237);
            this.lbl_recycle_status.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_recycle_status.Name = "lbl_recycle_status";
            this.lbl_recycle_status.Size = new System.Drawing.Size(98, 22);
            this.lbl_recycle_status.TabIndex = 74;
            this.lbl_recycle_status.Text = "回收箱：";
            // 
            // lbl_box_hascards
            // 
            this.lbl_box_hascards.AutoSize = true;
            this.lbl_box_hascards.Font = new System.Drawing.Font("宋体", 16F);
            this.lbl_box_hascards.Location = new System.Drawing.Point(45, 178);
            this.lbl_box_hascards.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_box_hascards.Name = "lbl_box_hascards";
            this.lbl_box_hascards.Size = new System.Drawing.Size(142, 22);
            this.lbl_box_hascards.TabIndex = 73;
            this.lbl_box_hascards.Text = "卡盒有无卡：";
            // 
            // btn_reflesh
            // 
            this.btn_reflesh.Font = new System.Drawing.Font("宋体", 18F);
            this.btn_reflesh.Location = new System.Drawing.Point(49, 342);
            this.btn_reflesh.Name = "btn_reflesh";
            this.btn_reflesh.Size = new System.Drawing.Size(136, 63);
            this.btn_reflesh.TabIndex = 71;
            this.btn_reflesh.Text = "余量刷新";
            this.btn_reflesh.UseVisualStyleBackColor = true;
            this.btn_reflesh.Click += new System.EventHandler(this.btn_reflesh_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("宋体", 18F);
            this.btnExit.Location = new System.Drawing.Point(255, 342);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(136, 63);
            this.btnExit.TabIndex = 70;
            this.btnExit.Text = "退出系统";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbl_ribbon_remain2
            // 
            this.lbl_ribbon_remain2.AutoSize = true;
            this.lbl_ribbon_remain2.Font = new System.Drawing.Font("宋体", 16F);
            this.lbl_ribbon_remain2.Location = new System.Drawing.Point(45, 119);
            this.lbl_ribbon_remain2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ribbon_remain2.Name = "lbl_ribbon_remain2";
            this.lbl_ribbon_remain2.Size = new System.Drawing.Size(197, 22);
            this.lbl_ribbon_remain2.TabIndex = 15;
            this.lbl_ribbon_remain2.Text = "彩印机2色带余量：";
            // 
            // lbl_ribbon_remain1
            // 
            this.lbl_ribbon_remain1.AutoSize = true;
            this.lbl_ribbon_remain1.Font = new System.Drawing.Font("宋体", 16F);
            this.lbl_ribbon_remain1.Location = new System.Drawing.Point(45, 60);
            this.lbl_ribbon_remain1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ribbon_remain1.Name = "lbl_ribbon_remain1";
            this.lbl_ribbon_remain1.Size = new System.Drawing.Size(197, 22);
            this.lbl_ribbon_remain1.TabIndex = 14;
            this.lbl_ribbon_remain1.Text = "彩印机1色带余量：";
            // 
            // btn_reader_out
            // 
            this.btn_reader_out.Location = new System.Drawing.Point(524, 141);
            this.btn_reader_out.Name = "btn_reader_out";
            this.btn_reader_out.Size = new System.Drawing.Size(207, 30);
            this.btn_reader_out.TabIndex = 102;
            this.btn_reader_out.Text = "通道—>出卡";
            this.btn_reader_out.UseVisualStyleBackColor = true;
            this.btn_reader_out.Click += new System.EventHandler(this.btn_reader_out_Click);
            // 
            // FrmCardboxTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "FrmCardboxTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCardboxTest_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_get_printersn;
        private System.Windows.Forms.Button btn_car_2recycle;
        private System.Windows.Forms.Button btn_open_close;
        private System.Windows.Forms.Button btn_f5san_reset;
        private System.Windows.Forms.Button btn_car_2reader;
        private System.Windows.Forms.Button btn_car_2printer;
        private System.Windows.Forms.Button btn_printer_2car;
        private System.Windows.Forms.Button btn_box_2car;
        private System.Windows.Forms.ComboBox cbox_printer_2car;
        private System.Windows.Forms.ComboBox cbox_car_2printer;
        private System.Windows.Forms.CheckBox checkBox_readcard;
        private System.Windows.Forms.CheckBox checkBox_writecard;
        private System.Windows.Forms.CheckBox checkBox_print;
        private System.Windows.Forms.Label lbl_f5san_context;
        private System.Windows.Forms.ComboBox cbox_test2_printer;
        private System.Windows.Forms.Button btn_cardtest;
        private System.Windows.Forms.Label lbl_ribbon_remain2;
        private System.Windows.Forms.Label lbl_ribbon_remain1;
        private System.Windows.Forms.Button btn_reflesh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lbl_recycle_status;
        private System.Windows.Forms.Label lbl_box_hascards;
        private System.Windows.Forms.Button btn_read_sicard;
        private System.Windows.Forms.Button btn_read_idcard;
        private System.Windows.Forms.Button btn_reader_out;
    }
}