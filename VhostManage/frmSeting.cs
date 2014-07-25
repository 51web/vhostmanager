using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2012-07-04

** 描述：

**  主要用于对系统进行设置

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
                //读配置文件
                txtMysqlPath.Text = ini.IniReadValue("MysqlConfig", "MysqlPath");
                txtMysqlPort.Text = ini.IniReadValue("MysqlConfig", "MysqlPort");
                txtMysqlDataPath.Text = ini.IniReadValue("MysqlConfig", "MysqlDBPath");
                txtMySQLRootUser.Text = ini.IniReadValue("MysqlConfig", "MysqlRoot");
                txtMySQLRootPassword.Text = ini.IniReadValue("MysqlConfig", "MysqlPassword");

                //mssql配置
                txtDBPath.Text = ini.IniReadValue("MssqlConfig", "MssqlDBPath");
                txtMssqlPort.Text = ini.IniReadValue("MssqlConfig", "MssqlPort");
                txtMsSQLSAUser.Text = ini.IniReadValue("MssqlConfig", "MssqlSa");
                txtMsSQLSAPassword.Text = ini.IniReadValue("MssqlConfig", "MssqlPassword");
            }
            catch { }

        }

        //主机设置
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
                    MessageBox.Show("设置成功!", "提示");
                }
                else
                {
                    MessageBox.Show("设置失败!", "提示");
                }
            }
            catch
            {
                MessageBox.Show("程序出错!请重试或联系开发者!", "提示");
                try
                {
                    OleCmd = null;
                    OleConn.Close();
                    OleConn = null;
                }
                catch { }
            }
        }
        //数据库设置　
        private void btnMySQL_Click(object sender, EventArgs e)
        {
            if (!txtIsMatch.SecurePassword(txtMySQLRootPassword.Text.Trim()))
            {
                MessageBox.Show("建议将密码设置为14位以上的数字、字母、特殊符号组合!", "提示");
            }
            if (!System.IO.File.Exists(txtMysqlPath.Text + "\\bin\\mysql.exe"))
            {
                MessageBox.Show("MYSQL\"程序路径\"不正确,请重新选择路径", "错误");
                return;
            }
            if (!System.IO.Directory.Exists(txtMysqlDataPath.Text))
            {
                MessageBox.Show("MYSQL\"数据路径\"不正确,请重新选择路径", "错误");
                return;
            }
            try
            {
                //写入配置文件
                ini.IniWriteValue("MysqlConfig", "MysqlPath", txtMysqlPath.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlPort", txtMysqlPort.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlDBPath", txtMysqlDataPath.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlRoot", txtMySQLRootUser.Text);
                ini.IniWriteValue("MysqlConfig", "MysqlPassword", txtMySQLRootPassword.Text);
                //设置mysql路径配置
                frmMain.MysqlPath = txtMysqlPath.Text;
                frmMain.MysqlDBPath = txtMysqlDataPath.Text;
                frmMain.MysqlDBPort = txtMysqlPort.Text.Trim();
                frmMain.MysqlDBUser = txtMySQLRootUser.Text.Trim();
                frmMain.MysqlDBPassWord = txtMySQLRootPassword.Text.Trim();
                MessageBox.Show("设置成功!请与真实数据库的配置同步!", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序出错!请重试或联系开发者!" + ex.ToString(), "提示");
            }
        }

        private void btnMsSQL_Click(object sender, EventArgs e)
        {
            try
            {
                //写入配置文件            
                ini.IniWriteValue("MssqlConfig", "MssqlPort", txtMssqlPort.Text);
                ini.IniWriteValue("MssqlConfig", "MssqlDBPath", txtDBPath.Text);
                ini.IniWriteValue("MssqlConfig", "MssqlSa", txtMsSQLSAUser.Text);
                ini.IniWriteValue("MssqlConfig", "MssqlPassword", txtMsSQLSAPassword.Text);
                //设置mssql路径配置
                frmMain.MssqlDBPath = txtDBPath.Text;
                frmMain.MssqlDBPort = txtMssqlPort.Text.Trim();
                frmMain.MssqlDBUser = txtMsSQLSAPassword.Text.Trim();
                frmMain.MssqlDBPassWord = txtMsSQLSAUser.Text.Trim();

                MessageBox.Show("设置成功!请与真实数据库的配置同步!", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序出错!请重试或联系开发者!" + ex.ToString(), "提示");
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

                    if (MessageBox.Show("选择的MSSQL\"存放路径\"不正确,是否重新选择路径？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    if (MessageBox.Show("选择的MYSQL\"程序路径\"不正确,是否重新选择路径？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
        /// MYSQL数据路径
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
                    if (MessageBox.Show("选择的MYSQL\"数据路径\"不正确,是否重新选择路径？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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