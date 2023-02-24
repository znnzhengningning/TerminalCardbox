namespace SHBSS
{
    partial class FrmSetting
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
            if (disposing && (components != null))
            {
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btn_payment = new System.Windows.Forms.Button();
            this.btn_lost = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.btnSiupoPrinterer = new System.Windows.Forms.Button();
            this.btn_GetCardInfo = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtidcode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PaycodeBox = new System.Windows.Forms.TextBox();
            this.textIDBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.tboxQrcode = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.tboxNLight = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(559, 475);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(160, 100);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "系统退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btn_payment
            // 
            this.btn_payment.Location = new System.Drawing.Point(559, 69);
            this.btn_payment.Margin = new System.Windows.Forms.Padding(4);
            this.btn_payment.Name = "btn_payment";
            this.btn_payment.Size = new System.Drawing.Size(157, 60);
            this.btn_payment.TabIndex = 1;
            this.btn_payment.Text = "交费登记";
            this.btn_payment.UseVisualStyleBackColor = true;
            this.btn_payment.Click += new System.EventHandler(this.btn_payment_Click);
            // 
            // btn_lost
            // 
            this.btn_lost.Location = new System.Drawing.Point(333, 69);
            this.btn_lost.Margin = new System.Windows.Forms.Padding(4);
            this.btn_lost.Name = "btn_lost";
            this.btn_lost.Size = new System.Drawing.Size(157, 60);
            this.btn_lost.TabIndex = 2;
            this.btn_lost.Text = "挂失测试";
            this.btn_lost.UseVisualStyleBackColor = true;
            this.btn_lost.Click += new System.EventHandler(this.btn_lost_Click);
            // 
            // lblError
            // 
            this.lblError.Location = new System.Drawing.Point(31, 475);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(473, 112);
            this.lblError.TabIndex = 3;
            this.lblError.Text = "Info";
            // 
            // btnSiupoPrinterer
            // 
            this.btnSiupoPrinterer.Location = new System.Drawing.Point(95, 164);
            this.btnSiupoPrinterer.Margin = new System.Windows.Forms.Padding(4);
            this.btnSiupoPrinterer.Name = "btnSiupoPrinterer";
            this.btnSiupoPrinterer.Size = new System.Drawing.Size(157, 60);
            this.btnSiupoPrinterer.TabIndex = 5;
            this.btnSiupoPrinterer.Text = "小票打印";
            this.btnSiupoPrinterer.UseVisualStyleBackColor = true;
            this.btnSiupoPrinterer.Click += new System.EventHandler(this.btnSiupoPrinterer_Click);
            // 
            // btn_GetCardInfo
            // 
            this.btn_GetCardInfo.Location = new System.Drawing.Point(95, 69);
            this.btn_GetCardInfo.Margin = new System.Windows.Forms.Padding(4);
            this.btn_GetCardInfo.Name = "btn_GetCardInfo";
            this.btn_GetCardInfo.Size = new System.Drawing.Size(157, 60);
            this.btn_GetCardInfo.TabIndex = 6;
            this.btn_GetCardInfo.Text = "获取身份证信息";
            this.btn_GetCardInfo.UseVisualStyleBackColor = true;
            this.btn_GetCardInfo.Click += new System.EventHandler(this.btn_GetCardInfo_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(121, 28);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(132, 25);
            this.txtName.TabIndex = 7;
            // 
            // txtidcode
            // 
            this.txtidcode.Location = new System.Drawing.Point(377, 28);
            this.txtidcode.Margin = new System.Windows.Forms.Padding(4);
            this.txtidcode.Name = "txtidcode";
            this.txtidcode.Size = new System.Drawing.Size(237, 25);
            this.txtidcode.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "姓名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "身份证号";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(559, 388);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(157, 60);
            this.btnBack.TabIndex = 11;
            this.btnBack.Text = "返回";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(559, 261);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 60);
            this.button1.TabIndex = 12;
            this.button1.Text = "卡管接口测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(331, 261);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 60);
            this.button2.TabIndex = 13;
            this.button2.Text = "回调测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(559, 164);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 60);
            this.button3.TabIndex = 14;
            this.button3.Text = "撤销缴费登记";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(97, 261);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 60);
            this.button4.TabIndex = 15;
            this.button4.Text = "六卡盒打印测试";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(347, 341);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(157, 65);
            this.button5.TabIndex = 16;
            this.button5.Text = "查询支付结果";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 388);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "缴款码：";
            // 
            // PaycodeBox
            // 
            this.PaycodeBox.Location = new System.Drawing.Point(95, 380);
            this.PaycodeBox.Margin = new System.Windows.Forms.Padding(4);
            this.PaycodeBox.Name = "PaycodeBox";
            this.PaycodeBox.Size = new System.Drawing.Size(237, 25);
            this.PaycodeBox.TabIndex = 18;
            // 
            // textIDBox
            // 
            this.textIDBox.Location = new System.Drawing.Point(95, 341);
            this.textIDBox.Margin = new System.Windows.Forms.Padding(4);
            this.textIDBox.Name = "textIDBox";
            this.textIDBox.Size = new System.Drawing.Size(237, 25);
            this.textIDBox.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 346);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 19;
            this.label4.Text = "身份证号：";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(331, 164);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(157, 60);
            this.button6.TabIndex = 21;
            this.button6.Text = "写社保卡测试";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1047, 93);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(167, 51);
            this.button7.TabIndex = 22;
            this.button7.Text = "读取二维码";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // tboxQrcode
            // 
            this.tboxQrcode.Location = new System.Drawing.Point(801, 31);
            this.tboxQrcode.Margin = new System.Windows.Forms.Padding(4);
            this.tboxQrcode.Multiline = true;
            this.tboxQrcode.Name = "tboxQrcode";
            this.tboxQrcode.Size = new System.Drawing.Size(444, 54);
            this.tboxQrcode.TabIndex = 23;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1047, 164);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(167, 51);
            this.button8.TabIndex = 24;
            this.button8.Text = "指示灯开";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tboxNLight
            // 
            this.tboxNLight.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tboxNLight.Location = new System.Drawing.Point(976, 177);
            this.tboxNLight.Name = "tboxNLight";
            this.tboxNLight.Size = new System.Drawing.Size(54, 38);
            this.tboxNLight.TabIndex = 25;
            this.tboxNLight.Text = "0";
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1373, 820);
            this.Controls.Add(this.tboxNLight);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.tboxQrcode);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.textIDBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PaycodeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtidcode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btn_GetCardInfo);
            this.Controls.Add(this.btnSiupoPrinterer);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btn_lost);
            this.Controls.Add(this.btn_payment);
            this.Controls.Add(this.btnExit);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统";
            this.Load += new System.EventHandler(this.FrmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btn_payment;
        private System.Windows.Forms.Button btn_lost;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnSiupoPrinterer;
        private System.Windows.Forms.Button btn_GetCardInfo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtidcode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PaycodeBox;
        private System.Windows.Forms.TextBox textIDBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox tboxQrcode;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox tboxNLight;
    }
}