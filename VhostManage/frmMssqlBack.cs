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

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**  ��Ҫ���ڶ�mssql�Ŀ���

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
            cmbDatabase.Items.Add("��ѡ��");
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
            cmbDatabase.Items.Add("��ѡ��");
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
                MessageBox.Show("ˢ�³ɹ�", "��ʾ");
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
        /// ���ݿⱸ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                btnExport.Enabled = false;
                btnBrows.Enabled = false;
                if (cmbDatabase.Text.Trim() == "" || cmbDatabase.Text.Trim() == "��ѡ��")
                {
                    MessageBox.Show("��ѡ��Ҫ���ݵ����ݿ���!");
                    return;
                }
                if (!System.IO.Directory.Exists(txtBack.Text))
                {
                    MessageBox.Show("����·������ȷ,������ѡ��!");
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
                    return "���ݳɹ�!" + execute1.ToString();
                }
                else
                {
                    return "����ʧ��!" + execute1.ToString();
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
                cmbDatabase.Items.Add("��ѡ��");
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
            file.Filter = "MSSQL���ݿⱸ���ļ�(*.bak)|*.bak";
            file.ShowDialog();
            txtBackFile.Text = file.FileName;
        }


        #region MSSQL��ԭ���ݿ�
        /// <summary>
        /// MSSQL���ݿ⻹ԭ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnReduction_Click(object sender, EventArgs e)
        {

            if (!System.IO.File.Exists(txtBackFile.Text))
            {
                MessageBox.Show("MSSQL���ݿⱸ���ļ�������,������ѡ��!");
                return;
            }
            MessageBox.Show(RestoreDB(cmbDatabase.SelectedItem.ToString(), "100", txtBackFile.Text));
        }

        /// <summary>
        /// MSSQL��ԭ���ݿ�ķ���
        /// </summary>
        /// <param name="strDataBase">���ݿ���</param>
        /// <param name="s_DBSize">���ݿ��С</param>
        /// <param name="strDatPath">Dat�ļ�·��</param>
        /// <returns>����ִ�н��</returns>
        public string RestoreDB(string strDataBase, string s_DBSize, string strDatPath)
        {
            //��鴫�ݵĲ���
            if (strDataBase == "")
            {
                return "���ݿ����Ʋ���Ϊ��!";
            }
            string MssqlDatePath = frmMain.MssqlDBPath;
            if (!System.IO.Directory.Exists(MssqlDatePath))
            {
                return "MSSQL���ݿ�����·�����ò���ȷ!��Ҫ�ڲ˵�[����]--[���ݿ�]--[MSSQL���ݿ�����]����������·��!";
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
                return "�����ļ�����!�������ϴ�����!";
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
            //�������ݿ⻹ԭ��С
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
                return "���ݿ⻹ԭ�ɹ�!";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion
    }
}