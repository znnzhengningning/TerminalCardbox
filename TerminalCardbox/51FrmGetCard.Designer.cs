namespace SHBSS
{
    partial class FrmGetCard
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
            this.components = new System.ComponentModel.Container();
            this.picphoto = new System.Windows.Forms.PictureBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblCID = new System.Windows.Forms.Label();
            this.lblSID = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCountDown = new System.Windows.Forms.Label();
            this.btnReturn = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.btn_confrm = new System.Windows.Forms.Button();
            this.lbl_get_sicard = new System.Windows.Forms.Label();
            this.lbl_read_id = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picphoto)).BeginInit();
            this.SuspendLayout();
            // 
            // picphoto
            // 
            this.picphoto.BackColor = System.Drawing.Color.Transparent;
            this.picphoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picphoto.Location = new System.Drawing.Point(437, 278);
            this.picphoto.Name = "picphoto";
            this.picphoto.Size = new System.Drawing.Size(94, 125);
            this.picphoto.TabIndex = 36;
            this.picphoto.TabStop = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblDate.Location = new System.Drawing.Point(629, 364);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(55, 14);
            this.lblDate.TabIndex = 35;
            this.lblDate.Text = "label4";
            // 
            // lblCID
            // 
            this.lblCID.AutoSize = true;
            this.lblCID.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblCID.Location = new System.Drawing.Point(662, 338);
            this.lblCID.Name = "lblCID";
            this.lblCID.Size = new System.Drawing.Size(55, 14);
            this.lblCID.TabIndex = 34;
            this.lblCID.Text = "label3";
            // 
            // lblSID
            // 
            this.lblSID.AutoSize = true;
            this.lblSID.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblSID.Location = new System.Drawing.Point(662, 312);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(55, 14);
            this.lblSID.TabIndex = 33;
            this.lblSID.Text = "label2";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(591, 286);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 14);
            this.lblName.TabIndex = 32;
            this.lblName.Text = "label1";
            // 
            // lblCountDown
            // 
            this.lblCountDown.AutoSize = true;
            this.lblCountDown.BackColor = System.Drawing.Color.Transparent;
            this.lblCountDown.Font = new System.Drawing.Font("Consolas", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountDown.ForeColor = System.Drawing.Color.Gold;
            this.lblCountDown.Location = new System.Drawing.Point(893, 570);
            this.lblCountDown.Name = "lblCountDown";
            this.lblCountDown.Size = new System.Drawing.Size(128, 94);
            this.lblCountDown.TabIndex = 31;
            this.lblCountDown.Text = "20";
            // 
            // btnReturn
            // 
            this.btnReturn.BackgroundImage = global::SHBSS.Properties.Resources.returnup;
            this.btnReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReturn.FlatAppearance.BorderSize = 0;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Location = new System.Drawing.Point(619, 602);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(148, 47);
            this.btnReturn.TabIndex = 30;
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblError.Location = new System.Drawing.Point(109, 375);
            this.lblError.Name = "lblError";
            this.lblError.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblError.Size = new System.Drawing.Size(284, 262);
            this.lblError.TabIndex = 37;
            // 
            // btn_confrm
            // 
            this.btn_confrm.BackColor = System.Drawing.Color.Transparent;
            this.btn_confrm.BackgroundImage = global::SHBSS.Properties.Resources.ok;
            this.btn_confrm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_confrm.FlatAppearance.BorderSize = 0;
            this.btn_confrm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confrm.Location = new System.Drawing.Point(424, 602);
            this.btn_confrm.Name = "btn_confrm";
            this.btn_confrm.Size = new System.Drawing.Size(148, 47);
            this.btn_confrm.TabIndex = 38;
            this.btn_confrm.UseVisualStyleBackColor = false;
            this.btn_confrm.Click += new System.EventHandler(this.btn_confrm_Click);
            // 
            // lbl_get_sicard
            // 
            this.lbl_get_sicard.AutoSize = true;
            this.lbl_get_sicard.BackColor = System.Drawing.Color.Transparent;
            this.lbl_get_sicard.Font = new System.Drawing.Font("微软雅黑", 32F, System.Drawing.FontStyle.Bold);
            this.lbl_get_sicard.ForeColor = System.Drawing.Color.Navy;
            this.lbl_get_sicard.Location = new System.Drawing.Point(115, 232);
            this.lbl_get_sicard.Name = "lbl_get_sicard";
            this.lbl_get_sicard.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_get_sicard.Size = new System.Drawing.Size(292, 57);
            this.lbl_get_sicard.TabIndex = 40;
            this.lbl_get_sicard.Text = "领 取 社 保 卡";
            // 
            // lbl_read_id
            // 
            this.lbl_read_id.AutoSize = true;
            this.lbl_read_id.BackColor = System.Drawing.Color.Transparent;
            this.lbl_read_id.Font = new System.Drawing.Font("新宋体", 24F, System.Drawing.FontStyle.Bold);
            this.lbl_read_id.ForeColor = System.Drawing.Color.Navy;
            this.lbl_read_id.Location = new System.Drawing.Point(422, 537);
            this.lbl_read_id.Name = "lbl_read_id";
            this.lbl_read_id.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_read_id.Size = new System.Drawing.Size(345, 33);
            this.lbl_read_id.TabIndex = 41;
            this.lbl_read_id.Text = "请放置身份证在读卡区";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmGetCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SHBSS.Properties.Resources.制卡;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1155, 755);
            this.Controls.Add(this.lbl_read_id);
            this.Controls.Add(this.lbl_get_sicard);
            this.Controls.Add(this.btn_confrm);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.picphoto);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblCID);
            this.Controls.Add(this.lblSID);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblCountDown);
            this.Controls.Add(this.btnReturn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmGetCard";
            this.Text = "FrmGetCard";
            ((System.ComponentModel.ISupportInitialize)(this.picphoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picphoto;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblCID;
        private System.Windows.Forms.Label lblSID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCountDown;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btn_confrm;
        private System.Windows.Forms.Label lbl_get_sicard;
        private System.Windows.Forms.Label lbl_read_id;
        private System.Windows.Forms.Timer timer1;
    }
}