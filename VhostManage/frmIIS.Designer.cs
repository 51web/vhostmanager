namespace VhostManage
{
    partial class frmIIS
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
            this.btnIISRest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnIISRest
            // 
            this.btnIISRest.Location = new System.Drawing.Point(97, 29);
            this.btnIISRest.Name = "btnIISRest";
            this.btnIISRest.Size = new System.Drawing.Size(75, 23);
            this.btnIISRest.TabIndex = 0;
            this.btnIISRest.Text = "重启IIS";
            this.btnIISRest.UseVisualStyleBackColor = true;
            this.btnIISRest.Click += new System.EventHandler(this.btnIISRest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "说明：重新启动IIS";
            // 
            // frmIIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 167);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIISRest);
            this.MaximizeBox = false;
            this.Name = "frmIIS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IIS管理";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIISRest;
        private System.Windows.Forms.Label label1;
    }
}