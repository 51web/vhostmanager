namespace VhostManage
{
    partial class frmFtpSeting
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFtpSeting));
            this.BtnChangeFtpProt = new System.Windows.Forms.Button();
            this.txtFtpStartPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFtpEndPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblFTPPort = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnChangeFtpProt
            // 
            this.BtnChangeFtpProt.Location = new System.Drawing.Point(227, 67);
            this.BtnChangeFtpProt.Name = "BtnChangeFtpProt";
            this.BtnChangeFtpProt.Size = new System.Drawing.Size(41, 23);
            this.BtnChangeFtpProt.TabIndex = 0;
            this.BtnChangeFtpProt.Text = "修改";
            this.BtnChangeFtpProt.UseVisualStyleBackColor = true;
            this.BtnChangeFtpProt.Click += new System.EventHandler(this.BtnChangeFtpProt_Click);
            // 
            // txtFtpStartPort
            // 
            this.txtFtpStartPort.Location = new System.Drawing.Point(93, 68);
            this.txtFtpStartPort.MaxLength = 5;
            this.txtFtpStartPort.Name = "txtFtpStartPort";
            this.txtFtpStartPort.Size = new System.Drawing.Size(55, 21);
            this.txtFtpStartPort.TabIndex = 1;
            this.txtFtpStartPort.Text = "5000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "从";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "到";
            // 
            // txtFtpEndPort
            // 
            this.txtFtpEndPort.Location = new System.Drawing.Point(169, 68);
            this.txtFtpEndPort.MaxLength = 5;
            this.txtFtpEndPort.Name = "txtFtpEndPort";
            this.txtFtpEndPort.Size = new System.Drawing.Size(55, 21);
            this.txtFtpEndPort.TabIndex = 4;
            this.txtFtpEndPort.Text = "5100";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.lblFTPPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.BtnChangeFtpProt);
            this.groupBox1.Controls.Add(this.txtFtpEndPort);
            this.groupBox1.Controls.Add(this.txtFtpStartPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(16, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 102);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端口范围";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 8;
            this.label13.Text = "当前范围：";
            // 
            // lblFTPPort
            // 
            this.lblFTPPort.AutoSize = true;
            this.lblFTPPort.Location = new System.Drawing.Point(93, 25);
            this.lblFTPPort.Name = "lblFTPPort";
            this.lblFTPPort.Size = new System.Drawing.Size(0, 12);
            this.lblFTPPort.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "修改为：";
            // 
            // frmFtpSeting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 130);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFtpSeting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP被动模式端口范围";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmFtpSeting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnChangeFtpProt;
        private System.Windows.Forms.TextBox txtFtpStartPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFtpEndPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblFTPPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label3;
    }
}