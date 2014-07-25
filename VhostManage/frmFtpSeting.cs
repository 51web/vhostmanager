using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**  主要用于对FTP的控制

*********************************************************************************/

namespace VhostManage
{
    public partial class frmFtpSeting : Form
    {
        public frmFtpSeting()
        {
            InitializeComponent();
        }

        private void frmFtpSeting_Load(object sender, EventArgs e)
        {
            FromRefresh();
        }
        private void BtnChangeFtpProt_Click(object sender, EventArgs e)
        {
            int FtpStartPort;
            int FtpEndPort;
            try
            {
                FtpStartPort = int.Parse(txtFtpStartPort.Text.Trim());
            }
            catch
            {
                MessageBox.Show("端口开始范围错误:5001-65535");
                txtFtpStartPort.Focus();
                return;
            }
            try
            {
                FtpEndPort = int.Parse(txtFtpEndPort.Text.Trim());
            }
            catch
            {
                MessageBox.Show("端口结束范围错误:" + FtpStartPort + "-65535");
                txtFtpStartPort.Focus();
                return;
            }
            try
            {
                if (FtpStartPort < 5001 || FtpStartPort > 65535)
                {
                    MessageBox.Show("端口开始范围错误:5001-65535");
                    txtFtpStartPort.Focus();
                    return;
                }
                if (FtpEndPort < FtpStartPort || FtpEndPort > 65535)
                {
                    MessageBox.Show("端口结束范围错误:" + FtpStartPort + "-65535");
                    txtFtpEndPort.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("端口开始范围错误:5001-65535");
                txtFtpStartPort.Focus();
                return;
            }
            if (MessageBox.Show("确认设置?需要重启动IIS才能生效!", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/MSFTPSVC");
                    appPoolRoot.InvokeSet("PassivePortRange", FtpStartPort + "-" + FtpEndPort);
                    appPoolRoot.Invoke("SetInfo");
                    appPoolRoot.CommitChanges();
                    appPoolRoot.Close();
                    MessageBox.Show("设置成功!");
                }
                catch
                {
                    MessageBox.Show("设置失败!");
                }
            }
            FromRefresh();
        }
        //刷新界面
        private void FromRefresh()
        {
            try
            {
                DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/MSFTPSVC");
                string strPort = appPoolRoot.InvokeGet("PassivePortRange").ToString();
                strPort = strPort.Trim();
                if (strPort != "")
                {
                    if (strPort.IndexOf("-") != -1)
                    {
                        txtFtpStartPort.Text = strPort.Substring(0, strPort.IndexOf("-"));
                        txtFtpEndPort.Text = strPort.Substring(strPort.IndexOf("-") + 1);
                    }
                    else
                    {
                        txtFtpStartPort.Text = strPort;
                        txtFtpEndPort.Text = strPort;
                    }
                    lblFTPPort.Text = strPort;
                }
                else
                {
                    label13.Text = "1025-5000";
                }
                appPoolRoot.Close();
            }
            catch
            { }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认恢复成默认值?", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/MSFTPSVC");
                    appPoolRoot.InvokeSet("PassivePortRange", "1025-5000");
                    appPoolRoot.Invoke("SetInfo");
                    appPoolRoot.CommitChanges();
                    appPoolRoot.Close();
                    MessageBox.Show("设置成功!");
                }
                catch
                {
                    MessageBox.Show("设置失败!");
                }
                FromRefresh();
            }
        }
    }
}