namespace VhostManage
{
    partial class frmIISReset
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAllSiteReset = new System.Windows.Forms.Button();
            this.btnSiteReset = new System.Windows.Forms.Button();
            this.lstSite = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstSite);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 469);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "站点列表";
            // 
            // btnAllSiteReset
            // 
            this.btnAllSiteReset.Location = new System.Drawing.Point(318, 454);
            this.btnAllSiteReset.Name = "btnAllSiteReset";
            this.btnAllSiteReset.Size = new System.Drawing.Size(63, 23);
            this.btnAllSiteReset.TabIndex = 2;
            this.btnAllSiteReset.Text = "全部修复";
            this.btnAllSiteReset.UseVisualStyleBackColor = true;
            this.btnAllSiteReset.Click += new System.EventHandler(this.btnAllSiteReset_Click);
            // 
            // btnSiteReset
            // 
            this.btnSiteReset.Location = new System.Drawing.Point(318, 425);
            this.btnSiteReset.Name = "btnSiteReset";
            this.btnSiteReset.Size = new System.Drawing.Size(63, 23);
            this.btnSiteReset.TabIndex = 1;
            this.btnSiteReset.Text = "单个修复";
            this.btnSiteReset.UseVisualStyleBackColor = true;
            this.btnSiteReset.Click += new System.EventHandler(this.btnSiteReset_Click);
            // 
            // lstSite
            // 
            this.lstSite.FormattingEnabled = true;
            this.lstSite.ItemHeight = 12;
            this.lstSite.Location = new System.Drawing.Point(6, 15);
            this.lstSite.Name = "lstSite";
            this.lstSite.Size = new System.Drawing.Size(286, 448);
            this.lstSite.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(316, 374);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 36);
            this.label1.TabIndex = 4;
            this.label1.Text = "注意：\r\n仅修复站点\r\nWEB和FTP";
            // 
            // frmIISReset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 493);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAllSiteReset);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSiteReset);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIISReset";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "站点修复";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmIISReset_Load);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstSite;
        private System.Windows.Forms.Button btnAllSiteReset;
        private System.Windows.Forms.Button btnSiteReset;
        private System.Windows.Forms.Label label1;
    }
}