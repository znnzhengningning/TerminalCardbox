namespace SHBSS
{
    partial class FrmDlgPIN
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
            if (disposing && ( components != null )) {
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
            this.label_PIN = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_prompt = new System.Windows.Forms.Label();
            this.txtNewPwd = new System.Windows.Forms.TextBox();
            this.txtCheckPwd = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_PIN
            // 
            this.label_PIN.AutoSize = true;
            this.label_PIN.BackColor = System.Drawing.Color.Transparent;
            this.label_PIN.Font = new System.Drawing.Font("微软雅黑", 46F, System.Drawing.FontStyle.Bold);
            this.label_PIN.ForeColor = System.Drawing.Color.Navy;
            this.label_PIN.Location = new System.Drawing.Point(47, 88);
            this.label_PIN.Name = "label_PIN";
            this.label_PIN.Size = new System.Drawing.Size(337, 83);
            this.label_PIN.TabIndex = 72;
            this.label_PIN.Text = "设 置 密 码";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label_prompt);
            this.panel2.Controls.Add(this.txtNewPwd);
            this.panel2.Controls.Add(this.txtCheckPwd);
            this.panel2.Location = new System.Drawing.Point(61, 253);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(480, 352);
            this.panel2.TabIndex = 71;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(17, 93);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 31);
            this.label2.TabIndex = 61;
            this.label2.Text = "设置密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(17, 179);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 31);
            this.label3.TabIndex = 62;
            this.label3.Text = "确认密码：";
            // 
            // label_prompt
            // 
            this.label_prompt.AutoSize = true;
            this.label_prompt.BackColor = System.Drawing.Color.Transparent;
            this.label_prompt.Font = new System.Drawing.Font("宋体", 26.25F);
            this.label_prompt.ForeColor = System.Drawing.Color.Green;
            this.label_prompt.Location = new System.Drawing.Point(54, 258);
            this.label_prompt.Name = "label_prompt";
            this.label_prompt.Size = new System.Drawing.Size(190, 35);
            this.label_prompt.TabIndex = 64;
            this.label_prompt.Text = "设置成功！";
            this.label_prompt.Visible = false;
            // 
            // txtNewPwd
            // 
            this.txtNewPwd.BackColor = System.Drawing.Color.AliceBlue;
            this.txtNewPwd.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNewPwd.ForeColor = System.Drawing.Color.Navy;
            this.txtNewPwd.Location = new System.Drawing.Point(155, 86);
            this.txtNewPwd.Margin = new System.Windows.Forms.Padding(2);
            this.txtNewPwd.MaxLength = 6;
            this.txtNewPwd.Name = "txtNewPwd";
            this.txtNewPwd.PasswordChar = '*';
            this.txtNewPwd.Size = new System.Drawing.Size(273, 44);
            this.txtNewPwd.TabIndex = 58;
            // 
            // txtCheckPwd
            // 
            this.txtCheckPwd.BackColor = System.Drawing.Color.AliceBlue;
            this.txtCheckPwd.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCheckPwd.ForeColor = System.Drawing.Color.Navy;
            this.txtCheckPwd.Location = new System.Drawing.Point(155, 172);
            this.txtCheckPwd.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckPwd.MaxLength = 6;
            this.txtCheckPwd.Name = "txtCheckPwd";
            this.txtCheckPwd.PasswordChar = '*';
            this.txtCheckPwd.Size = new System.Drawing.Size(273, 44);
            this.txtCheckPwd.TabIndex = 59;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.btn1);
            this.panel1.Controls.Add(this.btn5);
            this.panel1.Controls.Add(this.btn6);
            this.panel1.Controls.Add(this.btn4);
            this.panel1.Controls.Add(this.btnDel);
            this.panel1.Controls.Add(this.btn7);
            this.panel1.Controls.Add(this.btn3);
            this.panel1.Controls.Add(this.btnEmpty);
            this.panel1.Controls.Add(this.btn8);
            this.panel1.Controls.Add(this.btn2);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn0);
            this.panel1.Controls.Add(this.btn9);
            this.panel1.Location = new System.Drawing.Point(642, 253);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 352);
            this.panel1.TabIndex = 70;
            // 
            // btn1
            // 
            this.btn1.BackColor = System.Drawing.Color.AliceBlue;
            this.btn1.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn1.ForeColor = System.Drawing.Color.Navy;
            this.btn1.Location = new System.Drawing.Point(15, 11);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(132, 56);
            this.btn1.TabIndex = 17;
            this.btn1.TabStop = false;
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = false;
            // 
            // btn5
            // 
            this.btn5.BackColor = System.Drawing.Color.AliceBlue;
            this.btn5.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn5.ForeColor = System.Drawing.Color.Navy;
            this.btn5.Location = new System.Drawing.Point(173, 79);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(132, 56);
            this.btn5.TabIndex = 21;
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = false;
            // 
            // btn6
            // 
            this.btn6.BackColor = System.Drawing.Color.AliceBlue;
            this.btn6.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn6.ForeColor = System.Drawing.Color.Navy;
            this.btn6.Location = new System.Drawing.Point(331, 79);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(132, 56);
            this.btn6.TabIndex = 22;
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = false;
            // 
            // btn4
            // 
            this.btn4.BackColor = System.Drawing.Color.AliceBlue;
            this.btn4.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn4.ForeColor = System.Drawing.Color.Navy;
            this.btn4.Location = new System.Drawing.Point(15, 79);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(132, 56);
            this.btn4.TabIndex = 20;
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = false;
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.AliceBlue;
            this.btnDel.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btnDel.ForeColor = System.Drawing.Color.Navy;
            this.btnDel.Location = new System.Drawing.Point(331, 215);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(132, 56);
            this.btnDel.TabIndex = 28;
            this.btnDel.Text = "回退";
            this.btnDel.UseVisualStyleBackColor = false;
            // 
            // btn7
            // 
            this.btn7.BackColor = System.Drawing.Color.AliceBlue;
            this.btn7.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn7.ForeColor = System.Drawing.Color.Navy;
            this.btn7.Location = new System.Drawing.Point(15, 147);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(132, 56);
            this.btn7.TabIndex = 23;
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = false;
            // 
            // btn3
            // 
            this.btn3.BackColor = System.Drawing.Color.AliceBlue;
            this.btn3.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn3.ForeColor = System.Drawing.Color.Navy;
            this.btn3.Location = new System.Drawing.Point(331, 11);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(132, 56);
            this.btn3.TabIndex = 19;
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = false;
            // 
            // btnEmpty
            // 
            this.btnEmpty.BackColor = System.Drawing.Color.AliceBlue;
            this.btnEmpty.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btnEmpty.ForeColor = System.Drawing.Color.Navy;
            this.btnEmpty.Location = new System.Drawing.Point(173, 215);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(132, 56);
            this.btnEmpty.TabIndex = 27;
            this.btnEmpty.Text = "清空";
            this.btnEmpty.UseVisualStyleBackColor = false;
            // 
            // btn8
            // 
            this.btn8.BackColor = System.Drawing.Color.AliceBlue;
            this.btn8.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn8.ForeColor = System.Drawing.Color.Navy;
            this.btn8.Location = new System.Drawing.Point(173, 147);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(132, 56);
            this.btn8.TabIndex = 24;
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = false;
            // 
            // btn2
            // 
            this.btn2.BackColor = System.Drawing.Color.AliceBlue;
            this.btn2.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn2.ForeColor = System.Drawing.Color.Navy;
            this.btn2.Location = new System.Drawing.Point(173, 11);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(132, 56);
            this.btn2.TabIndex = 18;
            this.btn2.TabStop = false;
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = false;
            // 
            // btn_ok
            // 
            this.btn_ok.BackgroundImage = global::SHBSS.Properties.Resources.ok;
            this.btn_ok.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_ok.FlatAppearance.BorderSize = 0;
            this.btn_ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ok.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ok.Location = new System.Drawing.Point(162, 285);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(175, 56);
            this.btn_ok.TabIndex = 29;
            this.btn_ok.UseVisualStyleBackColor = true;
            // 
            // btn0
            // 
            this.btn0.BackColor = System.Drawing.Color.AliceBlue;
            this.btn0.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn0.ForeColor = System.Drawing.Color.Navy;
            this.btn0.Location = new System.Drawing.Point(15, 215);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(132, 56);
            this.btn0.TabIndex = 26;
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = false;
            // 
            // btn9
            // 
            this.btn9.BackColor = System.Drawing.Color.AliceBlue;
            this.btn9.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.btn9.ForeColor = System.Drawing.Color.Navy;
            this.btn9.Location = new System.Drawing.Point(331, 147);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(132, 56);
            this.btn9.TabIndex = 25;
            this.btn9.Text = "9";
            this.btn9.UseVisualStyleBackColor = false;
            // 
            // FrmDlgPIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(187)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.label_PIN);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDlgPIN";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmDlgPIN";
            this.Load += new System.EventHandler(this.FrmDlgPIN_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_PIN;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.TextBox txtNewPwd;
        private System.Windows.Forms.TextBox txtCheckPwd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btn9;
    }
}