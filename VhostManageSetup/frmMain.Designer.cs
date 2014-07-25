namespace 自动安装
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.chkPHP = new System.Windows.Forms.CheckBox();
            this.chkUrl = new System.Windows.Forms.CheckBox();
            this.chkModule = new System.Windows.Forms.CheckBox();
            this.chkApp = new System.Windows.Forms.CheckBox();
            this.chkManage = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnk_pz = new System.Windows.Forms.LinkLabel();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetup = new System.Windows.Forms.Button();
            this.txtSetUpPath = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkPHP
            // 
            this.chkPHP.AutoSize = true;
            this.chkPHP.Location = new System.Drawing.Point(19, 22);
            this.chkPHP.Name = "chkPHP";
            this.chkPHP.Size = new System.Drawing.Size(270, 16);
            this.chkPHP.TabIndex = 1;
            this.chkPHP.Text = "PHP5(ISAPI模式)+MySQL5+ZEND3.3+PHPMYADMIN";
            this.chkPHP.UseVisualStyleBackColor = true;
            // 
            // chkUrl
            // 
            this.chkUrl.AutoSize = true;
            this.chkUrl.Location = new System.Drawing.Point(19, 72);
            this.chkUrl.Name = "chkUrl";
            this.chkUrl.Size = new System.Drawing.Size(180, 16);
            this.chkUrl.TabIndex = 3;
            this.chkUrl.Text = "URLRewrite(URL重写,伪静态)";
            this.chkUrl.UseVisualStyleBackColor = true;
            // 
            // chkModule
            // 
            this.chkModule.AutoSize = true;
            this.chkModule.Location = new System.Drawing.Point(19, 47);
            this.chkModule.Name = "chkModule";
            this.chkModule.Size = new System.Drawing.Size(222, 16);
            this.chkModule.TabIndex = 4;
            this.chkModule.Text = "常用组件(Jmail/AspJpeg/Lyfupload)";
            this.chkModule.UseVisualStyleBackColor = true;
            // 
            // chkApp
            // 
            this.chkApp.AutoSize = true;
            this.chkApp.Location = new System.Drawing.Point(19, 97);
            this.chkApp.Name = "chkApp";
            this.chkApp.Size = new System.Drawing.Size(162, 16);
            this.chkApp.TabIndex = 5;
            this.chkApp.Text = "常用软件 360安全卫士7.0";
            this.chkApp.UseVisualStyleBackColor = true;
            // 
            // chkManage
            // 
            this.chkManage.AutoSize = true;
            this.chkManage.Checked = true;
            this.chkManage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkManage.Location = new System.Drawing.Point(19, 122);
            this.chkManage.Name = "chkManage";
            this.chkManage.Size = new System.Drawing.Size(96, 16);
            this.chkManage.TabIndex = 6;
            this.chkManage.Text = "网站管理助手";
            this.chkManage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnk_pz);
            this.groupBox1.Controls.Add(this.chkSafe);
            this.groupBox1.Controls.Add(this.chkPHP);
            this.groupBox1.Controls.Add(this.chkManage);
            this.groupBox1.Controls.Add(this.chkApp);
            this.groupBox1.Controls.Add(this.chkUrl);
            this.groupBox1.Controls.Add(this.chkModule);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 154);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "软件信息";
            // 
            // lnk_pz
            // 
            this.lnk_pz.AutoSize = true;
            this.lnk_pz.Location = new System.Drawing.Point(205, 74);
            this.lnk_pz.Name = "lnk_pz";
            this.lnk_pz.Size = new System.Drawing.Size(53, 12);
            this.lnk_pz.TabIndex = 8;
            this.lnk_pz.TabStop = true;
            this.lnk_pz.Text = "配置说明";
            this.lnk_pz.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_pz_LinkClicked);
            // 
            // chkSafe
            // 
            this.chkSafe.AutoSize = true;
            this.chkSafe.Location = new System.Drawing.Point(121, 122);
            this.chkSafe.Name = "chkSafe";
            this.chkSafe.Size = new System.Drawing.Size(84, 16);
            this.chkSafe.TabIndex = 7;
            this.chkSafe.Text = "服务器安全";
            this.chkSafe.UseVisualStyleBackColor = true;
            this.chkSafe.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "安装路径：";
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(242, 175);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(44, 23);
            this.btnSetup.TabIndex = 2;
            this.btnSetup.Text = "安装";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // txtSetUpPath
            // 
            this.txtSetUpPath.Location = new System.Drawing.Point(79, 177);
            this.txtSetUpPath.Name = "txtSetUpPath";
            this.txtSetUpPath.Size = new System.Drawing.Size(157, 21);
            this.txtSetUpPath.TabIndex = 7;
            this.txtSetUpPath.Text = "d:\\VhostManage";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(79, 205);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(257, 14);
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "安装进度：";
            this.label3.Visible = false;
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(292, 175);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(44, 23);
            this.btn_exit.TabIndex = 11;
            this.btn_exit.Text = "退出";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 227);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtSetUpPath);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(357, 254);
            this.MinimumSize = new System.Drawing.Size(357, 254);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网站管理助手V3.4安装工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPHP;
        private System.Windows.Forms.CheckBox chkUrl;
        private System.Windows.Forms.CheckBox chkModule;
        private System.Windows.Forms.CheckBox chkApp;
        private System.Windows.Forms.CheckBox chkManage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.TextBox txtSetUpPath;
        private System.Windows.Forms.CheckBox chkSafe;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.LinkLabel lnk_pz;
    }
}

