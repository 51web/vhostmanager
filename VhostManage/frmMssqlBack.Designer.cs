namespace VhostManage
{
    partial class frmMssqlBack
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
            this.btnChaKan = new System.Windows.Forms.Button();
            this.btnBrows = new System.Windows.Forms.Button();
            this.txtBack = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGetDatabase = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtBackFile = new System.Windows.Forms.TextBox();
            this.btnReduction = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnChaKan);
            this.groupBox2.Controls.Add(this.btnBrows);
            this.groupBox2.Controls.Add(this.txtBack);
            this.groupBox2.Controls.Add(this.btnExport);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(6, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(493, 61);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "备份";
            // 
            // btnChaKan
            // 
            this.btnChaKan.Location = new System.Drawing.Point(443, 20);
            this.btnChaKan.Name = "btnChaKan";
            this.btnChaKan.Size = new System.Drawing.Size(44, 23);
            this.btnChaKan.TabIndex = 7;
            this.btnChaKan.Text = "查看";
            this.btnChaKan.UseVisualStyleBackColor = true;
            this.btnChaKan.Visible = false;
            this.btnChaKan.Click += new System.EventHandler(this.btnChaKan_Click);
            // 
            // btnBrows
            // 
            this.btnBrows.Location = new System.Drawing.Point(324, 20);
            this.btnBrows.Name = "btnBrows";
            this.btnBrows.Size = new System.Drawing.Size(63, 23);
            this.btnBrows.TabIndex = 6;
            this.btnBrows.Text = "选择路径";
            this.btnBrows.UseVisualStyleBackColor = true;
            this.btnBrows.Click += new System.EventHandler(this.btnBrows_Click);
            // 
            // txtBack
            // 
            this.txtBack.Location = new System.Drawing.Point(70, 22);
            this.txtBack.Name = "txtBack";
            this.txtBack.ReadOnly = true;
            this.txtBack.Size = new System.Drawing.Size(248, 21);
            this.txtBack.TabIndex = 5;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(393, 20);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(44, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "备份";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "备份路径：";
            // 
            // btnGetDatabase
            // 
            this.btnGetDatabase.Location = new System.Drawing.Point(190, 17);
            this.btnGetDatabase.Name = "btnGetDatabase";
            this.btnGetDatabase.Size = new System.Drawing.Size(38, 23);
            this.btnGetDatabase.TabIndex = 8;
            this.btnGetDatabase.Text = "刷新";
            this.btnGetDatabase.UseVisualStyleBackColor = true;
            this.btnGetDatabase.Click += new System.EventHandler(this.btnGetDatabase_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "数据库名：";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(76, 19);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(108, 20);
            this.cmbDatabase.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(293, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "注意：本程序提供的备份不保证备份的完整性和正确性";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.txtBackFile);
            this.groupBox1.Controls.Add(this.btnReduction);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(7, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 61);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "还原";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(324, 23);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(63, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "选择文件";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtBackFile
            // 
            this.txtBackFile.Location = new System.Drawing.Point(69, 24);
            this.txtBackFile.Name = "txtBackFile";
            this.txtBackFile.ReadOnly = true;
            this.txtBackFile.Size = new System.Drawing.Size(248, 21);
            this.txtBackFile.TabIndex = 5;
            // 
            // btnReduction
            // 
            this.btnReduction.Location = new System.Drawing.Point(393, 23);
            this.btnReduction.Name = "btnReduction";
            this.btnReduction.Size = new System.Drawing.Size(44, 23);
            this.btnReduction.TabIndex = 4;
            this.btnReduction.Text = "还原";
            this.btnReduction.UseVisualStyleBackColor = true;
            this.btnReduction.Click += new System.EventHandler(this.btnReduction_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "备份文件：";
            // 
            // frmMssqlBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 210);
            this.Controls.Add(this.btnGetDatabase);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMssqlBack";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MSSQL备份/还原";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMssqlBack_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrows;
        private System.Windows.Forms.TextBox txtBack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGetDatabase;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtBackFile;
        private System.Windows.Forms.Button btnReduction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnChaKan;
    }
}