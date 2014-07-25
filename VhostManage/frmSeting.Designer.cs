namespace VhostManage
{
    partial class frmSeting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeting));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDefaultDoc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtNET4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNET1 = new System.Windows.Forms.TextBox();
            this.txtNET2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtASP = new System.Windows.Forms.TextBox();
            this.txtPHP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnVhost = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSitedir = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMysqlDataPath = new System.Windows.Forms.Button();
            this.txtMysqlDataPath = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnMysqlPathBrowse = new System.Windows.Forms.Button();
            this.txtMysqlPath = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMySQLRootPassword = new System.Windows.Forms.TextBox();
            this.txtMySQLRootUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMySQL = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtDBPath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMsSQLSAPassword = new System.Windows.Forms.TextBox();
            this.txtMsSQLSAUser = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnMsSQL = new System.Windows.Forms.Button();
            this.lblMysqlProt = new System.Windows.Forms.Label();
            this.txtMysqlPort = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtMssqlPort = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtDefaultDoc);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnVhost);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSitedir);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 275);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "虚拟主机";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Crimson;
            this.label10.Location = new System.Drawing.Point(182, 249);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "警告:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.DarkBlue;
            this.label9.Location = new System.Drawing.Point(217, 249);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "默认设置请不要随意更改";
            // 
            // txtDefaultDoc
            // 
            this.txtDefaultDoc.Location = new System.Drawing.Point(64, 44);
            this.txtDefaultDoc.Name = "txtDefaultDoc";
            this.txtDefaultDoc.Size = new System.Drawing.Size(363, 21);
            this.txtDefaultDoc.TabIndex = 9;
            this.txtDefaultDoc.Text = "index.html,default.htm,default.html,index.asp,index.php,index.aspx,default.aspx";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "默认文档";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.txtNET4);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtNET1);
            this.groupBox3.Controls.Add(this.txtNET2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtASP);
            this.groupBox3.Controls.Add(this.txtPHP);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(8, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 164);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "脚本映射";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 13;
            this.label13.Text = "NET4.X";
            // 
            // txtNET4
            // 
            this.txtNET4.Location = new System.Drawing.Point(56, 132);
            this.txtNET4.Name = "txtNET4";
            this.txtNET4.ReadOnly = true;
            this.txtNET4.Size = new System.Drawing.Size(363, 21);
            this.txtNET4.TabIndex = 12;
            this.txtNET4.Text = resources.GetString("txtNET4.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "NET2.X";
            // 
            // txtNET1
            // 
            this.txtNET1.Location = new System.Drawing.Point(56, 76);
            this.txtNET1.Name = "txtNET1";
            this.txtNET1.ReadOnly = true;
            this.txtNET1.Size = new System.Drawing.Size(363, 21);
            this.txtNET1.TabIndex = 8;
            this.txtNET1.Text = resources.GetString("txtNET1.Text");
            // 
            // txtNET2
            // 
            this.txtNET2.Location = new System.Drawing.Point(56, 104);
            this.txtNET2.Name = "txtNET2";
            this.txtNET2.ReadOnly = true;
            this.txtNET2.Size = new System.Drawing.Size(363, 21);
            this.txtNET2.TabIndex = 9;
            this.txtNET2.Text = resources.GetString("txtNET2.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "NET1.X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "PHP";
            // 
            // txtASP
            // 
            this.txtASP.Location = new System.Drawing.Point(56, 20);
            this.txtASP.Name = "txtASP";
            this.txtASP.ReadOnly = true;
            this.txtASP.Size = new System.Drawing.Size(363, 21);
            this.txtASP.TabIndex = 3;
            this.txtASP.Text = ".asp,C:\\WINDOWS\\system32\\inetsrv\\asp.dll,5,GET,HEAD,POST;.asa,C:\\WINDOWS\\system32" +
                "\\inetsrv\\asp.dll,5,GET,HEAD,POST";
            // 
            // txtPHP
            // 
            this.txtPHP.Location = new System.Drawing.Point(56, 48);
            this.txtPHP.Name = "txtPHP";
            this.txtPHP.Size = new System.Drawing.Size(363, 21);
            this.txtPHP.TabIndex = 5;
            this.txtPHP.Text = ".php,C:\\VhostManage\\php\\php5isapi.dll,5,GET,HEAD,POST";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "ASP";
            // 
            // btnVhost
            // 
            this.btnVhost.Location = new System.Drawing.Point(380, 244);
            this.btnVhost.Name = "btnVhost";
            this.btnVhost.Size = new System.Drawing.Size(47, 23);
            this.btnVhost.TabIndex = 0;
            this.btnVhost.Text = "修改";
            this.btnVhost.UseVisualStyleBackColor = true;
            this.btnVhost.Click += new System.EventHandler(this.btnVhost_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "主目录";
            // 
            // txtSitedir
            // 
            this.txtSitedir.Location = new System.Drawing.Point(64, 17);
            this.txtSitedir.Name = "txtSitedir";
            this.txtSitedir.Size = new System.Drawing.Size(363, 21);
            this.txtSitedir.TabIndex = 1;
            this.txtSitedir.Text = "d:\\wwwroot";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMysqlPort);
            this.groupBox2.Controls.Add(this.lblMysqlProt);
            this.groupBox2.Controls.Add(this.btnMysqlDataPath);
            this.groupBox2.Controls.Add(this.txtMysqlDataPath);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.btnMysqlPathBrowse);
            this.groupBox2.Controls.Add(this.txtMysqlPath);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtMySQLRootPassword);
            this.groupBox2.Controls.Add(this.txtMySQLRootUser);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnMySQL);
            this.groupBox2.Location = new System.Drawing.Point(3, 283);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(217, 170);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MYSQL数据库配置";
            // 
            // btnMysqlDataPath
            // 
            this.btnMysqlDataPath.Location = new System.Drawing.Point(172, 112);
            this.btnMysqlDataPath.Name = "btnMysqlDataPath";
            this.btnMysqlDataPath.Size = new System.Drawing.Size(38, 23);
            this.btnMysqlDataPath.TabIndex = 12;
            this.btnMysqlDataPath.Text = "浏览";
            this.btnMysqlDataPath.UseVisualStyleBackColor = true;
            this.btnMysqlDataPath.Click += new System.EventHandler(this.btnMysqlDataPath_Click);
            // 
            // txtMysqlDataPath
            // 
            this.txtMysqlDataPath.Location = new System.Drawing.Point(61, 113);
            this.txtMysqlDataPath.Name = "txtMysqlDataPath";
            this.txtMysqlDataPath.ReadOnly = true;
            this.txtMysqlDataPath.Size = new System.Drawing.Size(109, 21);
            this.txtMysqlDataPath.TabIndex = 11;
            this.txtMysqlDataPath.Text = "D:\\mysql";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 117);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 10;
            this.label16.Text = "数据路径";
            // 
            // btnMysqlPathBrowse
            // 
            this.btnMysqlPathBrowse.Location = new System.Drawing.Point(172, 87);
            this.btnMysqlPathBrowse.Name = "btnMysqlPathBrowse";
            this.btnMysqlPathBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnMysqlPathBrowse.TabIndex = 9;
            this.btnMysqlPathBrowse.Text = "浏览";
            this.btnMysqlPathBrowse.UseVisualStyleBackColor = true;
            this.btnMysqlPathBrowse.Click += new System.EventHandler(this.btnMysqlPathBrowse_Click);
            // 
            // txtMysqlPath
            // 
            this.txtMysqlPath.Location = new System.Drawing.Point(61, 88);
            this.txtMysqlPath.Name = "txtMysqlPath";
            this.txtMysqlPath.ReadOnly = true;
            this.txtMysqlPath.Size = new System.Drawing.Size(109, 21);
            this.txtMysqlPath.TabIndex = 8;
            this.txtMysqlPath.Text = "C:\\VhostManage\\mysql";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 91);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 7;
            this.label15.Text = "程序路径";
            // 
            // txtMySQLRootPassword
            // 
            this.txtMySQLRootPassword.Location = new System.Drawing.Point(61, 64);
            this.txtMySQLRootPassword.Name = "txtMySQLRootPassword";
            this.txtMySQLRootPassword.PasswordChar = '*';
            this.txtMySQLRootPassword.Size = new System.Drawing.Size(149, 21);
            this.txtMySQLRootPassword.TabIndex = 5;
            this.txtMySQLRootPassword.Text = "set_your_passwd_here";
            // 
            // txtMySQLRootUser
            // 
            this.txtMySQLRootUser.Location = new System.Drawing.Point(61, 41);
            this.txtMySQLRootUser.Name = "txtMySQLRootUser";
            this.txtMySQLRootUser.Size = new System.Drawing.Size(149, 21);
            this.txtMySQLRootUser.TabIndex = 4;
            this.txtMySQLRootUser.Text = "root";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "ROOT密码";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "ROOT帐户";
            // 
            // btnMySQL
            // 
            this.btnMySQL.Location = new System.Drawing.Point(86, 142);
            this.btnMySQL.Name = "btnMySQL";
            this.btnMySQL.Size = new System.Drawing.Size(47, 23);
            this.btnMySQL.TabIndex = 1;
            this.btnMySQL.Text = "修改";
            this.btnMySQL.UseVisualStyleBackColor = true;
            this.btnMySQL.Click += new System.EventHandler(this.btnMySQL_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtMssqlPort);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.btnBrowse);
            this.groupBox4.Controls.Add(this.txtDBPath);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtMsSQLSAPassword);
            this.groupBox4.Controls.Add(this.txtMsSQLSAUser);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.btnMsSQL);
            this.groupBox4.Location = new System.Drawing.Point(226, 283);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 170);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "MSSQL2000数据库配置";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(167, 88);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtDBPath
            // 
            this.txtDBPath.Location = new System.Drawing.Point(56, 88);
            this.txtDBPath.Name = "txtDBPath";
            this.txtDBPath.ReadOnly = true;
            this.txtDBPath.Size = new System.Drawing.Size(109, 21);
            this.txtDBPath.TabIndex = 7;
            this.txtDBPath.Text = "d:\\mssql";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 91);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "数据路径";
            // 
            // txtMsSQLSAPassword
            // 
            this.txtMsSQLSAPassword.Location = new System.Drawing.Point(56, 64);
            this.txtMsSQLSAPassword.Name = "txtMsSQLSAPassword";
            this.txtMsSQLSAPassword.PasswordChar = '*';
            this.txtMsSQLSAPassword.Size = new System.Drawing.Size(149, 21);
            this.txtMsSQLSAPassword.TabIndex = 5;
            this.txtMsSQLSAPassword.Text = "set_your_passwd_here";
            // 
            // txtMsSQLSAUser
            // 
            this.txtMsSQLSAUser.Location = new System.Drawing.Point(56, 41);
            this.txtMsSQLSAUser.Name = "txtMsSQLSAUser";
            this.txtMsSQLSAUser.Size = new System.Drawing.Size(149, 21);
            this.txtMsSQLSAUser.TabIndex = 4;
            this.txtMsSQLSAUser.Text = "sa";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 3;
            this.label11.Text = "SA密码";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "SA帐户";
            // 
            // btnMsSQL
            // 
            this.btnMsSQL.Location = new System.Drawing.Point(85, 142);
            this.btnMsSQL.Name = "btnMsSQL";
            this.btnMsSQL.Size = new System.Drawing.Size(47, 23);
            this.btnMsSQL.TabIndex = 1;
            this.btnMsSQL.Text = "修改";
            this.btnMsSQL.UseVisualStyleBackColor = true;
            this.btnMsSQL.Click += new System.EventHandler(this.btnMsSQL_Click);
            // 
            // lblMysqlProt
            // 
            this.lblMysqlProt.AutoSize = true;
            this.lblMysqlProt.Location = new System.Drawing.Point(8, 21);
            this.lblMysqlProt.Name = "lblMysqlProt";
            this.lblMysqlProt.Size = new System.Drawing.Size(53, 12);
            this.lblMysqlProt.TabIndex = 13;
            this.lblMysqlProt.Text = "端    口";
            // 
            // txtMysqlPort
            // 
            this.txtMysqlPort.Location = new System.Drawing.Point(61, 18);
            this.txtMysqlPort.Name = "txtMysqlPort";
            this.txtMysqlPort.Size = new System.Drawing.Size(149, 21);
            this.txtMysqlPort.TabIndex = 14;
            this.txtMysqlPort.Text = "3306";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 21);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 12);
            this.label17.TabIndex = 14;
            this.label17.Text = "端   口";
            // 
            // txtMssqlPort
            // 
            this.txtMssqlPort.Location = new System.Drawing.Point(56, 18);
            this.txtMssqlPort.Name = "txtMssqlPort";
            this.txtMssqlPort.Size = new System.Drawing.Size(149, 21);
            this.txtMssqlPort.TabIndex = 15;
            this.txtMssqlPort.Text = "1433";
            // 
            // frmSeting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 465);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置";
            this.Load += new System.EventHandler(this.frmSeting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnVhost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPHP;
        private System.Windows.Forms.TextBox txtASP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSitedir;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNET1;
        private System.Windows.Forms.TextBox txtNET2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMySQL;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMySQLRootPassword;
        private System.Windows.Forms.TextBox txtMySQLRootUser;
        private System.Windows.Forms.TextBox txtDefaultDoc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtMsSQLSAPassword;
        private System.Windows.Forms.TextBox txtMsSQLSAUser;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnMsSQL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtNET4;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnMysqlPathBrowse;
        private System.Windows.Forms.TextBox txtMysqlPath;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtDBPath;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnMysqlDataPath;
        private System.Windows.Forms.TextBox txtMysqlDataPath;
        private System.Windows.Forms.TextBox txtMysqlPort;
        private System.Windows.Forms.Label lblMysqlProt;
        private System.Windows.Forms.TextBox txtMssqlPort;
        private System.Windows.Forms.Label label17;
    }
}
