using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2012-07-04

** ������

**  ��Ҫ���ڶ�ϵͳ��������

*********************************************************************************/
namespace VhostManage
{
    public partial class frmSeting : Form
    {
        public frmSeting()
        {
            InitializeComponent();
        }
        Read.Ini ini = new Read.Ini();
        private void frmSeting_Load(object sender, EventArgs e)
        {

            OleDbConnection OleConn = ConnClass.DataConn();
            OleDbCommand OleCmd = new OleDbCommand();
            OleCmd.Connection = OleConn;
            try
            {
                OleCmd.CommandText = "select * from vhostseting where type = 'vhost'";
                OleConn.Open();
                OleDbDataReader OleReader = null;
                OleReader = OleCmd.ExecuteReader();
                if (OleReader.HasRows)
                {
                    OleReader.Read();
                    txtSitedir.Text = OleReader["vhostdir"].ToString();
                    txtASP.Text = OleReader["asp"].ToString();
                    txtPHP.Text = OleReader["php"].ToString();
                    txtNET1.Text = OleReader["net1"].ToString();
                    txtNET2.Text = OleReader["net2"].ToString();
                    txtDefaultDoc.Text = OleReader["defaultdoc"].ToString();
                    OleReader.Close();
                }
                OleConn.Close();
            }
            catch
            {
                try
                {
                    OleCmd = null;
                    OleConn.Close();
                    OleConn = null;
                }
                catch { }
            }
            try
            {
                //�������ļ�
                txtMysqlPath.Text = ini.IniReadValue("MysqlConfig", "MysqlPath");
                txtMysqlPort.Text = ini.IniReadValue("MysqlConfig", "MysqlPort");
                txtMysqlDataPath.Text = ini.IniReadValue("MysqlConfig", "MysqlDBPath");
                txtMySQLRootUser.Text = ini.IniReadValue("MysqlConfig", "MysqlRoot");
                txtMySQLRootPassword.Text = ini.IniReadValue("MysqlConfig", "MysqlPassword");

                //mssql����
                txtDBPath.Text = ini.IniReadValue("MssqlConfig", "MssqlDBPath");
                txtMssqlPort.Text = ini.IniReadValue("MssqlConfig", "MssqlPort");
                txtMsSQLSAUser.Text = ini.IniReadValue("MssqlConfig", "MssqlSa");
                txtMsSQLSAPassword.Text = ini.IniReadValue("MssqlConfig", "MssqlPassword");
            }
            catch { }

        }

        //��������
        private void btnVhost_Click(object sender, EventArgs e)
        {
            OleDbConnection OleConn = ConnClass.DataConn();
            OleDbCommand OleCmd = new OleDbCommand();
            OleCmd.Connection = OleConn;
            try
            {
                OleCmd.CommandText = "update vhostseting set " +
                                     "vhostdir = '" + txtSitedir.Text.Trim() + "'," +
                                     "asp = '" + txtASP.Text.Trim() + "'," +
                                     "php = '" + txtPHP.Text.Trim() + "'," +
                                     "net1 = '" + txtNET1.Text.Trim() + "'," +
                                     "net2 = '" + txtNET2.Text.Trim() + "'," +
                                     "net4 = '" + txtNET4.Text.Trim() + "'," +
                                     "defaultdoc = '" + txtDefaultDoc.Text.Trim() + "' " +
                                     "where type = 'vhost'";
                OleConn.Open();
                int ok = OleCmd.ExecuteNonQuery();

                OleConn.Close();
                if (ok == 1)
                {
                    MessageBox.Show("���óɹ�!", "��ʾ");
                }
                else
                {
                    MessageBox.Show("����ʧ��!", "��ʾ");
                }
            }
            catch
            {
                MessageBox.Show("�������!�����Ի���ϵ������!", "��ʾ");
                try
                {
                    OleCmd = null;
                    OleConn.Close();
                    OleConn = null;
                }
                catch { }
            }
        }
        //���ݿ����á�
        private void btnMySQL_Click(object sender, EventArgs e)
        {
            if (!txtIsMatch.SecurePassword(txtMySQLRootPassword.Text.Trim()))
            {
                MessageBox.Show("���齫��������Ϊ14λ���ϵ����֡���ĸ������������!", "��ʾ");
            }
            if (!System.IO.File.Exists(txtMysqlPath.Text + "\\bin\\mysql.exe"))
            {
                MessageBox.Show("MYSQL\"����·��\"����ȷ,������ѡ��·��", "����");
                return;
            }
            if (!System.IO.Directory.Exists(txtMysqlDataPath.Text))
            {
                MessageBox.Show("MYSQL\"����·��\"����ȷ,������ѡ��·��", "����");
                return;
            }
            try
            {
                //д�������ļ�
                ini.IniWriteValue("MysqlConfig", "MysqlPath", txtMysqlPath.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlPort", txtMysqlPort.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlDBPath", txtMysqlDataPath.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlRoot", txtMySQLRootUser.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlPassword", txtMySQLRootPassword.Text);
                //����mysql·������
                frmMain.MysqlPath = txtMysqlPath.Text;
                frmMain.MysqlDBPath = txtMysqlDataPath.Text;
                frmMain.MysqlDBPort = txtMysqlPort.Text.Trim();
                frmMain.MysqlDBUser = txtMySQLRootUser.Text.Trim();
                frmMain.MysqlDBPassWord = txtMySQLRootPassword.Text.Trim();
                MessageBox.Show("���óɹ�!������ʵ���ݿ������ͬ��!", "��ʾ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������!�����Ի���ϵ������!" + ex.ToString(), "��ʾ");
            }
        }

        private void btnMsSQL_Click(object sender, EventArgs e)
        {
            try
            {
                //д�������ļ�            
                ini.IniWriteValue("MssqlConfig", "MssqlPort", txtMssqlPort.Text);
                ini.IniWriteValue("MssqlConfig", "MssqlDBPath", txtDBPath.Text);
                ini.IniWriteValue("MssqlConfig", "MssqlSa", txtMsSQLSAUser.Text);
                ini.IniWriteValue("MssqlConfig", "MssqlPassword", txtMsSQLSAPassword.Text);
                //����mssql·������
                frmMain.MssqlDBPath = txtDBPath.Text;
                frmMain.MssqlDBPort = txtMssqlPort.Text.Trim();
                frmMain.MssqlDBUser = txtMsSQLSAPassword.Text.Trim();
                frmMain.MssqlDBPassWord = txtMsSQLSAUser.Text.Trim();

                MessageBox.Show("���óɹ�!������ʵ���ݿ������ͬ��!", "��ʾ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������!�����Ի���ϵ������!" + ex.ToString(), "��ʾ");
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            bool reSelect = true;
            while (reSelect)
            {
                dd.ShowDialog();
                if (!System.IO.Directory.Exists(dd.SelectedPath))
                {

                    if (MessageBox.Show("ѡ���MSSQL\"���·��\"����ȷ,�Ƿ�����ѡ��·����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        reSelect = true;
                    }
                    else
                    {
                        reSelect = false;
                    }
                }
                else
                {
                    txtDBPath.Text = dd.SelectedPath;
                    reSelect = false;
                }
            }
        }

        private void btnMysqlPathBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            bool reSelect = true;
            while (reSelect)
            {
                dd.ShowDialog();
                if (!System.IO.File.Exists(dd.SelectedPath + "\\bin\\mysql.exe"))
                {
                    if (MessageBox.Show("ѡ���MYSQL\"����·��\"����ȷ,�Ƿ�����ѡ��·����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        reSelect = true;
                    }
                    else
                    {
                        reSelect = false;
                    }
                }
                else
                {
                    txtMysqlPath.Text = dd.SelectedPath;
                    reSelect = false;
                }

            }

        }
        /// <summary>
        /// MYSQL����·��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMysqlDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            bool reSelect = true;
            while (reSelect)
            {
                dd.ShowDialog();
                if (!System.IO.Directory.Exists(dd.SelectedPath))
                {
                    if (MessageBox.Show("ѡ���MYSQL\"����·��\"����ȷ,�Ƿ�����ѡ��·����", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        reSelect = true;
                    }
                    else
                    {
                        reSelect = false;
                    }
                }
                else
                {
                    txtMysqlDataPath.Text = dd.SelectedPath;
                    reSelect = false;
                }
            }

        }

    }
}