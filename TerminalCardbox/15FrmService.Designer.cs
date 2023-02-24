namespace SHBSS
{
    partial class FrmService
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnReturn = new System.Windows.Forms.Button();
            this.lblCountDown = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblName = new System.Windows.Forms.Label();
            this.lblSID = new System.Windows.Forms.Label();
            this.lblCID = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.picphoto = new System.Windows.Forms.PictureBox();
            this.label_CardState = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picphoto)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReturn
            // 
            this.btnReturn.BackgroundImage = global::SHBSS.Properties.Resources.returnup;
            this.btnReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Location = new System.Drawing.Point(610, 804);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(180, 60);
            this.btnReturn.TabIndex = 23;
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // lblCountDown
            // 
            this.lblCountDown.AutoSize = true;
            this.lblCountDown.BackColor = System.Drawing.Color.Transparent;
            this.lblCountDown.Font = new System.Drawing.Font("Consolas", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountDown.ForeColor = System.Drawing.Color.Gold;
            this.lblCountDown.Location = new System.Drawing.Point(945, 789);
            this.lblCountDown.Name = "lblCountDown";
            this.lblCountDown.Size = new System.Drawing.Size(128, 94);
            this.lblCountDown.TabIndex = 24;
            this.lblCountDown.Text = "20";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(818, 416);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 14);
            this.lblName.TabIndex = 25;
            this.lblName.Text = "label1";
            // 
            // lblSID
            // 
            this.lblSID.AutoSize = true;
            this.lblSID.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblSID.Location = new System.Drawing.Point(887, 441);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(55, 14);
            this.lblSID.TabIndex = 26;
            this.lblSID.Text = "label2";
            // 
            // lblCID
            // 
            this.lblCID.AutoSize = true;
            this.lblCID.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblCID.Location = new System.Drawing.Point(887, 466);
            this.lblCID.Name = "lblCID";
            this.lblCID.Size = new System.Drawing.Size(55, 14);
            this.lblCID.TabIndex = 27;
            this.lblCID.Text = "label3";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblDate.Location = new System.Drawing.Point(855, 492);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(55, 14);
            this.lblDate.TabIndex = 28;
            this.lblDate.Text = "label4";
            // 
            // picphoto
            // 
            this.picphoto.BackColor = System.Drawing.Color.Transparent;
            this.picphoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picphoto.Location = new System.Drawing.Point(656, 390);
            this.picphoto.Name = "picphoto";
            this.picphoto.Size = new System.Drawing.Size(94, 125);
            this.picphoto.TabIndex = 29;
            this.picphoto.TabStop = false;
            // 
            // label_CardState
            // 
            this.label_CardState.AutoSize = true;
            this.label_CardState.BackColor = System.Drawing.Color.Transparent;
            this.label_CardState.Font = new System.Drawing.Font("微软雅黑", 40F, System.Drawing.FontStyle.Bold);
            this.label_CardState.ForeColor = System.Drawing.Color.Navy;
            this.label_CardState.Location = new System.Drawing.Point(104, 240);
            this.label_CardState.Name = "label_CardState";
            this.label_CardState.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label_CardState.Size = new System.Drawing.Size(345, 72);
            this.label_CardState.TabIndex = 30;
            this.label_CardState.Text = "正在制卡中...";
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("微软雅黑", 24F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblError.Location = new System.Drawing.Point(200, 699);
            this.lblError.Name = "lblError";
            this.lblError.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblError.Size = new System.Drawing.Size(931, 47);
            this.lblError.TabIndex = 31;
            this.lblError.Text = "错误提示";
            // 
            // FrmService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SHBSS.Properties.Resources.制卡;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.label_CardState);
            this.Controls.Add(this.picphoto);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblCID);
            this.Controls.Add(this.lblSID);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblCountDown);
            this.Controls.Add(this.btnReturn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmService";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmService_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmService_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.picphoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label lblCountDown;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblSID;
        private System.Windows.Forms.Label lblCID;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.PictureBox picphoto;
        private System.Windows.Forms.Label label_CardState;
        private System.Windows.Forms.Label lblError;
    }
}

