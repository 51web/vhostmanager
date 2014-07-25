using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**  主要用于对mssql的控制

*********************************************************************************/
namespace VhostManage
{
    public partial class frmMssqlBack : Form
    {
        public frmMssqlBack()
        {
            InitializeComponent();
        }
        private string MssqlDBUser = frmMain.MssqlDBUser;
        private string MssqlDBPassword = frmMain.MssqlDBPassWord;


        private void frmMssqlBack_Load(object sender, EventArgs e)
        {
            // 
            cmbDatabase.Items.Clear();
            cmbDatabase.Items.Add("请选择");
            cmbDatabase.SelectedIndex = 0;
            btnChaKan.Visible = false;
            //OleDbConnection OleConn = ConnClass.DataConn();
            //OleDbCommand OleCmd = new OleDbCommand();
            //OleCmd.Connection = OleConn;
            //try
            //{
            //    OleCmd.CommandText = "select * from sqlseting where dbtype = 'mssql'";
            //    OleConn.Open();
            //    OleDbDataReader OleReader = null;
            //    OleReader = OleCmd.ExecuteReader();
            //    if (OleReader.HasRows)
            //    {
            //        while (OleReader.Read())
            //        {
            //            MssqlDBUser = OleReader["user"].ToString();
            //            MssqlDBPassword = OleReader["password"].ToString();
            //        }
            //        OleReader.Close();
            //    }
            //}
            //catch { }
            //finally
            //{
            //    try
            //    {
            //        OleCmd = null;
            //        OleConn.Close();
            //        OleConn = null;
            //    }
            //    catch { }

            //}

        }

        private void btnBrows_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            dd.ShowDialog();
            txtBack.Text = dd.SelectedPath;
        }

        private void btnGetDatabase_Click(object sender, EventArgs e)
        {
            cmbDatabase.Items.Clear();
            cmbDatabase.Items.Add("请选择");
            cmbDatabase.SelectedIndex = 0;
            SqlConnection conn = new SqlConnection("server=127.0.0.1;uid=" + MssqlDBUser + ";pwd=" + MssqlDBPassword + ";database=master");
            SqlCommand MssqlCommand = new SqlCommand();
            MssqlCommand.CommandTimeout = 30;
            MssqlCommand.Connection = conn;
            try
            {
                conn.Open();
                MssqlCommand.CommandText = "select name from master.dbo.sysdatabases order by dbid desc";
                SqlDataReader MssqlReader = MssqlCommand.ExecuteReader();

                if (MssqlReader.HasRows)
                {
                    while (MssqlReader.Read())
                    {
                        cmbDatabase.Items.Add(MssqlReader["name"].ToString());
                    }
                }
                MssqlReader.Close();
                MessageBox.Show("刷新成功", "提示");
            }
            catch { }
            finally
            {
                try
                {
                    conn.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// 数据库备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                btnExport.Enabled = false;
                btnBrows.Enabled = false;
                if (cmbDatabase.Text.Trim() == "" || cmbDatabase.Text.Trim() == "请选择")
                {
                    MessageBox.Show("请选择要备份的数据库名!");
                    return;
                }
                if (!System.IO.Directory.Exists(txtBack.Text))
                {
                    MessageBox.Show("备份路径不正确,请重新选择!");
                    return;
                }
                string strDataPath = txtBack.Text + "\\" + cmbDatabase.Text.Trim() + "_" + System.DateTime.Now.ToString("yyyyMMddhhssmm") + ".bak";
                MessageBox.Show(msSQLBack(cmbDatabase.Text.Trim(), strDataPath));
            }
            finally
            {
                btnExport.Enabled = true;
                btnBrows.Enabled = true;
            }

        }

        private string msSQLBack(string strDataBase, string strDataPath)
        {
            try
            {
                SqlConnection conn = new SqlConnection("server=127.0.0.1;uid=" + MssqlDBUser + ";pwd=" + MssqlDBPassword + ";database=master");
                SqlCommand MssqlCommand = new SqlCommand();
                MssqlCommand.CommandTimeout = 30;
                MssqlCommand.Connection = conn;
                conn.Open();
                MssqlCommand.CommandText = "backup database " + strDataBase + " to disk='" + strDataPath + "'";
                int execute1 = MssqlCommand.ExecuteNonQuery();
                conn.Close();
                if (execute1 == 1)
                {
                    btnChaKan.Visible = true;
                    return "备份成功!" + execute1.ToString();
                }
                else
                {
                    return "备份失败!" + execute1.ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection("server=127.0.0.1;uid=" + MssqlDBUser + ";pwd=" + MssqlDBPassword + ";database=master");
            SqlCommand MssqlCommand = new SqlCommand();
            MssqlCommand.CommandTimeout = 300;
            MssqlCommand.Connection = conn;
            try
            {
                cmbDatabase.Items.Clear();
                cmbDatabase.Items.Add("请选择");
                conn.Open();
                MssqlCommand.CommandText = "select name from master.dbo.sysdatabases order by dbid desc";
                SqlDataReader MssqlReader = MssqlCommand.ExecuteReader();

                if (MssqlReader.HasRows)
                {
                    while (MssqlReader.Read())
                    {
                        cmbDatabase.Items.Add(MssqlReader["name"].ToString());
                    }
                }
                MssqlReader.Close();
                cmbDatabase.SelectedIndex = 0;
            }
            catch { }
            finally
            {
                try
                {
                    conn.Close();
                }
                catch { }
            }



        }

        private void btnChaKan_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", txtBack.Text);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = "bak";
            file.Filter = "MSSQL数据库备份文件(*.bak)|*.bak";
            file.ShowDialog();
            txtBackFile.Text = file.FileName;
        }


        #region MSSQL还原数据库
        /// <summary>
        /// MSSQL数据库还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnReduction_Click(object sender, EventArgs e)
        {

            if (!System.IO.File.Exists(txtBackFile.Text))
            {
                MessageBox.Show("MSSQL数据库备份文件不存在,请重新选择!");
                return;
            }
            MessageBox.Show(RestoreDB(cmbDatabase.SelectedItem.ToString(), "100", txtBackFile.Text));
        }

        /// <summary>
        /// MSSQL还原数据库的方法
        /// </summary>
        /// <param name="strDataBase">数据库名</param>
        /// <param name="s_DBSize">数据库大小</param>
        /// <param name="strDatPath">Dat文件路径</param>
        /// <returns>返回执行结果</returns>
        public string RestoreDB(string strDataBase, string s_DBSize, string strDatPath)
        {
            //检查传递的参数
            if (strDataBase == "")
            {
                return "数据库名称不能为空!";
            }
            string MssqlDatePath = frmMain.MssqlDBPath;
            if (!System.IO.Directory.Exists(MssqlDatePath))
            {
                return "MSSQL数据库数据路径设置不正确!需要在菜单[设置]--[数据库]--[MSSQL数据库配置]中设置数据路径!";
            }
            string tmpDataFileName = "", tmpLogFileName = "", tmpDataSize = "", tmpLogSize = "";
            string ConnString = "server=127.0.0.1;uid=" + MssqlDBUser + ";pwd=" + MssqlDBPassword + ";database=master";
            SqlConnection MyConn = new SqlConnection(ConnString);
            MyConn.ConnectionString = ConnString;
            SqlCommand MyCommand = new SqlCommand();
            MyCommand.CommandType = CommandType.Text;
            MyCommand.CommandTimeout = 300;
            MyCommand.Connection = MyConn;
            MyCommand.CommandText = "RESTORE FILELISTONLY FROM DISK = '" + strDatPath + "'";
            SqlDataReader MyReader = null;
            int i = 0;
            try
            {
                MyConn.Open();
                MyReader = MyCommand.ExecuteReader();
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        i++;
                        if (MyReader["Type"].ToString() == "D")
                        {
                            tmpDataFileName = MyReader["LogicalName"].ToString();
                            tmpDataSize = MyReader["Size"].ToString();
                        }
                        if (MyReader["Type"].ToString() == "L")
                        {
                            tmpLogFileName = MyReader["LogicalName"].ToString();
                            tmpLogSize = MyReader["Size"].ToString();
                        }
                    }
                    MyReader.Close();
                }
                MyConn.Close();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            string tmpSQL = "";
            if (tmpDataFileName == "")
                return "备份文件错误!请重新上传备份!";
            //kill Connect
            try
            {
                MyCommand.CommandText = "USE master SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='" + strDataBase + "'";
                MyConn.Open();
                MyReader = MyCommand.ExecuteReader();
                if (MyReader.HasRows)
                {
                    while (MyReader.Read())
                    {
                        SqlConnection MyConn1 = new SqlConnection(ConnString);
                        MyConn1.Open();
                        SqlCommand cmd = new SqlCommand(string.Format("KILL {0}", MyReader["spid"].ToString()), MyConn1);
                        cmd.ExecuteNonQuery();
                        MyConn1.Close();
                    }
                    MyReader.Close();
                }
                MyConn.Close();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            //控制数据库还原大小
            if (i == 2)
            {
                tmpSQL = "RESTORE DATABASE [" + strDataBase + "]" +
                        " FROM DISK='" + strDatPath + "'" +
                        " WITH REPLACE," +
                        "MOVE '" + tmpDataFileName + "' TO '" + MssqlDatePath + "\\" + strDataBase + "_Data.mdf'";
                tmpSQL += ",MOVE '" + tmpLogFileName + "' TO '" + MssqlDatePath + "\\" + strDataBase + "_Log.ldf'";
            }
            else
            {
                tmpSQL = "RESTORE DATABASE [" + strDataBase + "]" +
                        " FROM DISK='" + strDatPath + "'" +
                        " WITH REPLACE," +
                        "MOVE '" + tmpDataFileName + "' TO '" + MssqlDatePath + "\\" + strDataBase + "_Data.mdf'";
            }
            tmpSQL += " use " + strDataBase + " EXEC sp_changedbowner '" + strDataBase + "'";
            if ((strDataBase + "_Data") != tmpDataFileName)
                tmpSQL += " ALTER DATABASE [" + strDataBase + "] MODIFY FILE (NAME ='" + tmpDataFileName + "',NEWNAME ='" + strDataBase + "_Data',FILEGROWTH=1MB)";
            else
                tmpSQL += " ALTER DATABASE [" + strDataBase + "] MODIFY FILE (NAME ='" + tmpDataFileName + "',FILEGROWTH=1MB)";
            if ((strDataBase + "_Log") != tmpLogFileName)
                tmpSQL += " ALTER DATABASE [" + strDataBase + "] MODIFY FILE (NAME ='" + tmpLogFileName + "',NEWNAME ='" + strDataBase + "_Log',FILEGROWTH=1MB)";
            else
                tmpSQL += " ALTER DATABASE [" + strDataBase + "] MODIFY FILE (NAME ='" + tmpLogFileName + "',FILEGROWTH=1MB)";
            tmpSQL += " ALTER DATABASE [" + strDataBase + "] SET RECOVERY SIMPLE";
            try
            {
                MyCommand.CommandText = tmpSQL;
                MyConn.Open();
                MyCommand.ExecuteNonQuery();
                MyConn.Close();
                return "数据库还原成功!";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion
    }
}