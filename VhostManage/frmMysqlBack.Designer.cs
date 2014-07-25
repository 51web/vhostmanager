namespace VhostManage
{
    partial class frmMysqlBack
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDatabases = new System.Windows.Forms.ComboBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCha = new System.Windows.Forms.Button();
            this.btnBroswing = new System.Windows.Forms.Button();
            this.txtBackPath = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbtnUTF8 = new System.Windows.Forms.RadioButton();
            this.rdbtnGBK = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelect1 = new System.Windows.Forms.Button();
            this.txtSourcePath1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnInput1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnInput = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDstpath = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库名:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "备份路径:";
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.Location = new System.Drawing.Point(89, 7);
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(108, 20);
            this.cmbDatabases.TabIndex = 3;
            this.cmbDatabases.SelectedIndexChanged += new System.EventHandler(this.cmbDatabases_SelectedIndexChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(297, 42);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(44, 23);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCha);
            this.groupBox1.Controls.Add(this.btnBroswing);
            this.groupBox1.Controls.Add(this.txtBackPath);
            this.groupBox1.Controls.Add(this.btnCopy);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 73);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MYSQL备份";
            // 
            // btnCha
            // 
            this.btnCha.Location = new System.Drawing.Point(397, 42);
            this.btnCha.Name = "btnCha";
            this.btnCha.Size = new System.Drawing.Size(64, 23);
            this.btnCha.TabIndex = 8;
            this.btnCha.Text = "查看备份";
            this.btnCha.UseVisualStyleBackColor = true;
            this.btnCha.Click += new System.EventHandler(this.btnCha_Click);
            // 
            // btnBroswing
            // 
            this.btnBroswing.Location = new System.Drawing.Point(422, 14);
            this.btnBroswing.Name = "btnBroswing";
            this.btnBroswing.Size = new System.Drawing.Size(38, 23);
            this.btnBroswing.TabIndex = 6;
            this.btnBroswing.Text = "选择";
            this.btnBroswing.UseVisualStyleBackColor = true;
            this.btnBroswing.Click += new System.EventHandler(this.btnBroswing_Click);
            // 
            // txtBackPath
            // 
            this.txtBackPath.Location = new System.Drawing.Point(77, 15);
            this.txtBackPath.Name = "txtBackPath";
            this.txtBackPath.ReadOnly = true;
            this.txtBackPath.Size = new System.Drawing.Size(344, 21);
            this.txtBackPath.TabIndex = 5;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(347, 42);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(44, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 204);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MYSQL还原";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbtnUTF8);
            this.groupBox4.Controls.Add(this.rdbtnGBK);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.btnSelect1);
            this.groupBox4.Controls.Add(this.txtSourcePath1);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.btnInput1);
            this.groupBox4.Location = new System.Drawing.Point(8, 120);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(458, 70);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "导入模式";
            // 
            // rdbtnUTF8
            // 
            this.rdbtnUTF8.AutoSize = true;
            this.rdbtnUTF8.Location = new System.Drawing.Point(119, 44);
            this.rdbtnUTF8.Name = "rdbtnUTF8";
            this.rdbtnUTF8.Size = new System.Drawing.Size(47, 16);
            this.rdbtnUTF8.TabIndex = 13;
            this.rdbtnUTF8.Text = "UTF8";
            this.rdbtnUTF8.UseVisualStyleBackColor = true;
            // 
            // rdbtnGBK
            // 
            this.rdbtnGBK.AutoSize = true;
            this.rdbtnGBK.Checked = true;
            this.rdbtnGBK.Location = new System.Drawing.Point(69, 44);
            this.rdbtnGBK.Name = "rdbtnGBK";
            this.rdbtnGBK.Size = new System.Drawing.Size(41, 16);
            this.rdbtnGBK.TabIndex = 12;
            this.rdbtnGBK.TabStop = true;
            this.rdbtnGBK.Text = "GBK";
            this.rdbtnGBK.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "字符集:";
            // 
            // btnSelect1
            // 
            this.btnSelect1.Location = new System.Drawing.Point(414, 13);
            this.btnSelect1.Name = "btnSelect1";
            this.btnSelect1.Size = new System.Drawing.Size(38, 23);
            this.btnSelect1.TabIndex = 10;
            this.btnSelect1.Text = "选择";
            this.btnSelect1.UseVisualStyleBackColor = true;
            this.btnSelect1.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // txtSourcePath1
            // 
            this.txtSourcePath1.Location = new System.Drawing.Point(69, 14);
            this.txtSourcePath1.Name = "txtSourcePath1";
            this.txtSourcePath1.ReadOnly = true;
            this.txtSourcePath1.Size = new System.Drawing.Size(344, 21);
            this.txtSourcePath1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "备份文件:";
            // 
            // btnInput1
            // 
            this.btnInput1.Location = new System.Drawing.Point(414, 41);
            this.btnInput1.Name = "btnInput1";
            this.btnInput1.Size = new System.Drawing.Size(38, 23);
            this.btnInput1.TabIndex = 6;
            this.btnInput1.Text = "还原";
            this.btnInput1.UseVisualStyleBackColor = true;
            this.btnInput1.Click += new System.EventHandler(this.btnInput1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblDstpath);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btnSelect);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtSourcePath);
            this.groupBox3.Controls.Add(this.btnInput);
            this.groupBox3.Location = new System.Drawing.Point(8, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(458, 90);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "覆盖模式";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(414, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(38, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "源 路 径:";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(69, 15);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(344, 21);
            this.txtSourcePath.TabIndex = 6;
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(414, 42);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(38, 23);
            this.btnInput.TabIndex = 5;
            this.btnInput.Text = "还原";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 328);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(353, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "注意：本程序提供的备份/还原不保证备份/还原的完整性和正确性";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(203, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(38, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "目的路径:";
            // 
            // lblDstpath
            // 
            this.lblDstpath.AutoSize = true;
            this.lblDstpath.Location = new System.Drawing.Point(67, 42);
            this.lblDstpath.Name = "lblDstpath";
            this.lblDstpath.Size = new System.Drawing.Size(53, 12);
            this.lblDstpath.TabIndex = 10;
            this.lblDstpath.Text = "d:\\mysql";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(293, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "注意:本模式需要备份与运行中的MYSQL同版本才能适用";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(248, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(73, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清空数据库";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmMysqlBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 346);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDatabases);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMysqlBack";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MYSQL备份/还原";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMysqlBack_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDatabases;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBroswing;
        private System.Windows.Forms.TextBox txtBackPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCha;
        private System.Windows.Forms.Button btnInput1;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSourcePath1;
        private System.Windows.Forms.Button btnSelect1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rdbtnUTF8;
        private System.Windows.Forms.RadioButton rdbtnGBK;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDstpath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClear;
    }
}