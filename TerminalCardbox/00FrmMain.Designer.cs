namespace SHBSS
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            this.timer_Register = new System.Windows.Forms.Timer(this.components);
            this.btMakeCard = new System.Windows.Forms.Button();
            this.btnApplyCard = new System.Windows.Forms.Button();
            this.btnChangePIN = new System.Windows.Forms.Button();
            this.btnCompQuery = new System.Windows.Forms.Button();
            this.btn_get_card = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_Register
            // 
            this.timer_Register.Interval = 180000;
            this.timer_Register.Tick += new System.EventHandler(this.timer_Register_Tick);
            // 
            // btMakeCard
            // 
            this.btMakeCard.BackColor = System.Drawing.Color.Transparent;
            this.btMakeCard.BackgroundImage = global::SHBSS.Properties.Resources.btnreplacecard;
            this.btMakeCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btMakeCard.FlatAppearance.BorderSize = 0;
            this.btMakeCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btMakeCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btMakeCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btMakeCard.ForeColor = System.Drawing.Color.White;
            this.btMakeCard.Location = new System.Drawing.Point(362, 3);
            this.btMakeCard.Name = "btMakeCard";
            this.btMakeCard.Size = new System.Drawing.Size(322, 252);
            this.btMakeCard.TabIndex = 11;
            this.btMakeCard.UseVisualStyleBackColor = false;
            this.btMakeCard.Click += new System.EventHandler(this.btMakeCard_Click);
            // 
            // btnApplyCard
            // 
            this.btnApplyCard.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyCard.BackgroundImage = global::SHBSS.Properties.Resources.申请社保卡;
            this.btnApplyCard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApplyCard.FlatAppearance.BorderSize = 0;
            this.btnApplyCard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyCard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyCard.ForeColor = System.Drawing.Color.Black;
            this.btnApplyCard.Location = new System.Drawing.Point(3, 3);
            this.btnApplyCard.Name = "btnApplyCard";
            this.btnApplyCard.Size = new System.Drawing.Size(322, 252);
            this.btnApplyCard.TabIndex = 12;
            this.btnApplyCard.UseVisualStyleBackColor = false;
            this.btnApplyCard.Click += new System.EventHandler(this.btnApplyCard_Click);
            // 
            // btnChangePIN
            // 
            this.btnChangePIN.BackColor = System.Drawing.Color.Transparent;
            this.btnChangePIN.BackgroundImage = global::SHBSS.Properties.Resources.btnreloadpin;
            this.btnChangePIN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnChangePIN.FlatAppearance.BorderSize = 0;
            this.btnChangePIN.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChangePIN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChangePIN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePIN.ForeColor = System.Drawing.Color.Black;
            this.btnChangePIN.Location = new System.Drawing.Point(362, 292);
            this.btnChangePIN.Name = "btnChangePIN";
            this.btnChangePIN.Size = new System.Drawing.Size(322, 252);
            this.btnChangePIN.TabIndex = 13;
            this.btnChangePIN.UseVisualStyleBackColor = false;
            this.btnChangePIN.Click += new System.EventHandler(this.btnChangePIN_Click);
            // 
            // btnCompQuery
            // 
            this.btnCompQuery.BackColor = System.Drawing.Color.Transparent;
            this.btnCompQuery.BackgroundImage = global::SHBSS.Properties.Resources.综合查询;
            this.btnCompQuery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCompQuery.FlatAppearance.BorderSize = 0;
            this.btnCompQuery.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCompQuery.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCompQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompQuery.ForeColor = System.Drawing.Color.Black;
            this.btnCompQuery.Location = new System.Drawing.Point(3, 292);
            this.btnCompQuery.Name = "btnCompQuery";
            this.btnCompQuery.Size = new System.Drawing.Size(322, 252);
            this.btnCompQuery.TabIndex = 14;
            this.btnCompQuery.UseVisualStyleBackColor = false;
            this.btnCompQuery.Click += new System.EventHandler(this.btnCompQuery_Click);
            // 
            // btn_get_card
            // 
            this.btn_get_card.BackColor = System.Drawing.SystemColors.Control;
            this.btn_get_card.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_get_card.FlatAppearance.BorderSize = 0;
            this.btn_get_card.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_get_card.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_get_card.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_get_card.ForeColor = System.Drawing.Color.Black;
            this.btn_get_card.Location = new System.Drawing.Point(985, 30);
            this.btn_get_card.Name = "btn_get_card";
            this.btn_get_card.Size = new System.Drawing.Size(121, 47);
            this.btn_get_card.TabIndex = 16;
            this.btn_get_card.Text = "领取社保卡";
            this.btn_get_card.UseVisualStyleBackColor = false;
            this.btn_get_card.Visible = false;
            this.btn_get_card.Click += new System.EventHandler(this.btn_get_card_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnApplyCard);
            this.panel1.Controls.Add(this.btMakeCard);
            this.panel1.Controls.Add(this.btnChangePIN);
            this.panel1.Controls.Add(this.btnCompQuery);
            this.panel1.Location = new System.Drawing.Point(301, 313);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 552);
            this.panel1.TabIndex = 17;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SHBSS.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_get_card);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmMain_MouseDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btMakeCard;
        private System.Windows.Forms.Timer timer_Register;
        private System.Windows.Forms.Button btnApplyCard;
        private System.Windows.Forms.Button btnChangePIN;
        private System.Windows.Forms.Button btnCompQuery;
        private System.Windows.Forms.Button btn_get_card;
        private System.Windows.Forms.Panel panel1;
    }
}