namespace 自动安装
{
    partial class frmRewrite
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 156);
            this.label1.TabIndex = 1;
            this.label1.Text = "单个站点配置Rewrite\r\n\r\n1、打开IIS管理器\r\n\r\n2、将管理助手安装目录下rewrite目录,\r\n复制到创建的站点目录下\r\n\r\n3、在单个站点属性中添" +
                "加ISAPI筛选器\r\n路径:\r\n站点路径/rewrite/ISAPI_Rewrite.dll\r\n\r\n4、设置规则\r\n站点路径/rewrite/httpd.ini" +
                " 默认为常用规则";
            // 
            // frmRewrite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 174);
            this.Controls.Add(this.label1);
            this.Name = "frmRewrite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rewrite 2.0 配置说明";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;

    }
}