using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**  ��Ҫ���ڶ�FTP�Ŀ���

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
                MessageBox.Show("�˿ڿ�ʼ��Χ����:5001-65535");
                txtFtpStartPort.Focus();
                return;
            }
            try
            {
                FtpEndPort = int.Parse(txtFtpEndPort.Text.Trim());
            }
            catch
            {
                MessageBox.Show("�˿ڽ�����Χ����:" + FtpStartPort + "-65535");
                txtFtpStartPort.Focus();
                return;
            }
            try
            {
                if (FtpStartPort < 5001 || FtpStartPort > 65535)
                {
                    MessageBox.Show("�˿ڿ�ʼ��Χ����:5001-65535");
                    txtFtpStartPort.Focus();
                    return;
                }
                if (FtpEndPort < FtpStartPort || FtpEndPort > 65535)
                {
                    MessageBox.Show("�˿ڽ�����Χ����:" + FtpStartPort + "-65535");
                    txtFtpEndPort.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("�˿ڿ�ʼ��Χ����:5001-65535");
                txtFtpStartPort.Focus();
                return;
            }
            if (MessageBox.Show("ȷ������?��Ҫ������IIS������Ч!", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/MSFTPSVC");
                    appPoolRoot.InvokeSet("PassivePortRange", FtpStartPort + "-" + FtpEndPort);
                    appPoolRoot.Invoke("SetInfo");
                    appPoolRoot.CommitChanges();
                    appPoolRoot.Close();
                    MessageBox.Show("���óɹ�!");
                }
                catch
                {
                    MessageBox.Show("����ʧ��!");
                }
            }
            FromRefresh();
        }
        //ˢ�½���
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
            if (MessageBox.Show("ȷ�ϻָ���Ĭ��ֵ?", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/MSFTPSVC");
                    appPoolRoot.InvokeSet("PassivePortRange", "1025-5000");
                    appPoolRoot.Invoke("SetInfo");
                    appPoolRoot.CommitChanges();
                    appPoolRoot.Close();
                    MessageBox.Show("���óɹ�!");
                }
                catch
                {
                    MessageBox.Show("����ʧ��!");
                }
                FromRefresh();
            }
        }
    }
}