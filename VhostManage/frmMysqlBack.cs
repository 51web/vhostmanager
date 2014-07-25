using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**  ��Ҫ���ڶ�mysql�Ŀ���

*********************************************************************************/
namespace VhostManage
{
    public partial class frmMysqlBack : Form
    {
        public frmMysqlBack()
        {
            InitializeComponent();
        }
        Read.Ini ini = new Read.Ini();
        string strMysqlPath = "";
        string strMysqlDataPath = "";
        private void frmMysqlBack_Load(object sender, EventArgs e)
        {
            strMysqlPath = frmMain.MysqlPath;
            strMysqlDataPath = frmMain.MysqlDBPath;
            lblDstpath.Text = strMysqlDataPath;
            cmbDatabases.Items.Clear();
            cmbDatabases.Items.Add("��ѡ��");
            DirectoryInfo dirinfo = null;
            if (Directory.Exists(strMysqlDataPath))
            {
                foreach (string dir in Directory.GetDirectories(strMysqlDataPath, "*.*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    dirinfo = new DirectoryInfo(dir);
                    if (dirinfo.Name.Trim().ToLower() != "mysql")
                    {
                        cmbDatabases.Items.Add(dirinfo.Name);
                    }
                }
            }
            else
            {
                MessageBox.Show("MYSQL����·�����ò���ȷ!��Ҫ�ڲ˵�[����]--[���ݿ�]--[MYSQL���ݿ�����]����������·��.", "����");
            }
            cmbDatabases.SelectedIndex = 0;
        }

        private void btnBroswing_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            dd.ShowDialog();
            txtBackPath.Text = dd.SelectedPath;

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            btnCha.Visible = false;
            if (cmbDatabases.Text.Trim() == "" || cmbDatabases.Text.Trim() == "��ѡ��")
            {
                MessageBox.Show("��ѡ����Ҫ���ݵ����ݿ���!");
                return;
            }
            if (txtBackPath.Text.Trim() == "")
            {
                MessageBox.Show("��ѡ�񱸷�·��!");
                return;
            }
            string MysqlPath = strMysqlDataPath + "\\" + cmbDatabases.Text.Trim();
            if (!System.IO.Directory.Exists(strMysqlDataPath))
            {
                MessageBox.Show("MYSQL����·�����ò���ȷ!��Ҫ�ڲ˵�[����]--[���ݿ�]--[MYSQL���ݿ�����]����������·��.", "����");
                return;
            }
            if (!System.IO.Directory.Exists(MysqlPath))
            {
                MessageBox.Show("ѡ������ݿⲻ����,������ѡ��!", "����");
                return;
            }
            if (DirectoryCopy(MysqlPath, txtBackPath.Text.Trim() + "\\" + cmbDatabases.Text.Trim() + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), true))
            {
                MessageBox.Show("��������ִ�����!��鿴�Ƿ񱸷ݳɹ�!");
                btnCha.Visible = true;
            }
            else
            {
                MessageBox.Show("����ʧ��!");
            }
            btnCha.Visible = true;
        }

        /// <summary>
        /// ��������Ŀ¼
        /// </summary>
        /// <param name="sourceDirName">Դ�ļ�Ŀ¼��</param>
        /// <param name="destDirName">Ŀ���ļ�Ŀ¼��</param>
        /// <param name="copySubDirs">�Ƿ񿽱���Ŀ¼</param>

        internal bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();

                // If the source directory does not exist, throw an exception.
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                // If the destination directory does not exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    try
                    {
                        // Create the path to the new copy of the file.
                        string temppath = Path.Combine(destDirName, file.Name);

                        // Copy the file.
                        file.CopyTo(temppath, true);
                    }
                    catch { }
                }

                // If copySubDirs is true, copy the subdirectories.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        // Create the subdirectory.
                        string temppath = Path.Combine(destDirName, subdir.Name);

                        // Copy the subdirectories.
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnCha.Visible = false;
            if (cmbDatabases.Text.Trim() == "" || cmbDatabases.Text.Trim() == "��ѡ��")
            {
                MessageBox.Show("��ѡ����Ҫ���ݵ����ݿ���!");
                return;
            }
            if (txtBackPath.Text.Trim() == "")
            {
                MessageBox.Show("��ѡ�񱸷�·��!");
                return;
            }

            OleDbConnection OleConn = ConnClass.DataConn();
            OleDbCommand OleCmd = new OleDbCommand();
            OleCmd.Connection = OleConn;
            string RootUser = "";
            string Password = "";
            try
            {
                OleCmd.CommandText = "select * from sqlseting where dbtype = 'mysql'";
                OleConn.Open();
                OleDbDataReader OleReader = null;
                OleReader = OleCmd.ExecuteReader();
                if (OleReader.HasRows)
                {
                    while (OleReader.Read())
                    {
                        RootUser = OleReader["user"].ToString();
                        Password = OleReader["password"].ToString();
                    }
                    OleReader.Close();
                }
            }
            catch { }
            finally
            {
                try
                {
                    OleCmd = null;
                    OleConn.Close();
                    OleConn = null;
                }
                catch { }

            }
            if (MysqlDump(cmbDatabases.Text.Trim(), RootUser, Password, txtBackPath.Text.Trim() + "\\" + cmbDatabases.Text.Trim() + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".sql"))
            {
                MessageBox.Show("��������ִ�����!��鿴�Ƿ񱸷ݳɹ�!");
            }
            else
            {
                MessageBox.Show("����ʧ��!");
            }
            btnCha.Visible = true;
        }

        #region ����sql�ļ�(��mysql)
        /// <summary>
        /// ����sql�ļ�
        /// </summary>
        /// <param name="strDataBase">���ݿ���</param>
        /// <param name="strSQLFilePath">sql�ļ�·��</param>
        /// <returns>ִ�гɹ�/ʧ��</returns>
        public bool MysqlDump(string strDataBase, string MysqlDBUser, string MysqlDBPassWord, string strSQLFilePath)
        {
            string MysqlPath = strMysqlPath + "\\bin\\";
            //��鴫�ݵĲ���           
            try
            {
                //string cmd = " --host=" + mysql_severip + " --user=" + mysql_dbuser + " --password=" + mysql_dbpass + " mymmm -r \"" + string.Format("{0}", strSQLFilePath) + "\"";
                string cmd = "--host=127.0.0.1 --port=" + frmMain.MysqlDBPort + " --user=" + MysqlDBUser + " --password=" + MysqlDBPassWord + " " + strDataBase + " > " + strSQLFilePath.Replace(" ", "\" \"");
                String s = "cd " + MysqlPath.Replace(" ", "\" \"");
                s += "," + MysqlPath.Substring(0, 2);
                s += ",mysqldump.exe " + cmd;
                IISControl.ExecuteCmd(s.Split(','));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private void btnCha_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(txtBackPath.Text))
            {
                System.Diagnostics.Process.Start("Explorer.exe", txtBackPath.Text);
            }
            else
            {
                MessageBox.Show("����·��������");
            }
        }

        /// <summary>
        /// ���ǻ�ԭ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            dd.ShowDialog();
            txtSourcePath.Text = dd.SelectedPath;
        }

        /// <summary>
        /// ����SQL�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = "sql";
            file.Filter = "MYSQL���ݿⱸ���ļ�(*.sql)|*.sql";
            file.ShowDialog();
            txtSourcePath1.Text = file.FileName;
        }

        /// <summary>
        /// ˢ���������ݿ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbDatabases.Items.Clear();
            cmbDatabases.Items.Add("��ѡ��");
            DirectoryInfo dirinfo = null;
            if (Directory.Exists(strMysqlDataPath))
            {
                foreach (string dir in Directory.GetDirectories(strMysqlDataPath, "*.*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    dirinfo = new DirectoryInfo(dir);
                    if (dirinfo.Name.Trim().ToLower() != "mysql")
                    {
                        cmbDatabases.Items.Add(dirinfo.Name);
                    }
                }
            }
            else
            {
                MessageBox.Show("MYSQL����·�����ò���ȷ!��Ҫ�ڲ˵�[����]--[���ݿ�]--[MYSQL���ݿ�����]����������·��.", "����");
            }
            cmbDatabases.SelectedIndex = 0;
            MessageBox.Show("ˢ�³ɹ�", "��ʾ");
        }

        #region  MYSQL��ԭ

        /// <summary>
        /// Դ�ļ�����ģʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
            btnInput.Visible = false;
            if (cmbDatabases.Text == "��ѡ��")
            {
                MessageBox.Show("��ѡ����Ҫ��ԭ�����ݿ�", "��ʾ");
                return;
            }
            if (!System.IO.Directory.Exists(txtSourcePath.Text))
            {
                MessageBox.Show("���ݿⱸ������\"Դ · ��\"����ȷ", "����");
                return;
            }
            if (!System.IO.Directory.Exists(lblDstpath.Text))
            {
                MessageBox.Show("���ݿ⻹ԭ·��\"��ԭ·��\"����ȷ", "����");
                return;
            }
            if (lblDstpath.Text == txtSourcePath.Text)
            {
                MessageBox.Show("��ѡ�����ݿ�", "��ʾ");
                return;
            }
            MessageBox.Show("Ϊ��֤��ԭ����ȷ��,���黹ԭǰֹͣmysql����,��ԭ��������");
            //ֹͣ����
            //��ʼ����
            //�����ļ�
            if (DirectoryCopy(txtSourcePath.Text, lblDstpath.Text, false))
            {
                MessageBox.Show("���ǻ�ԭ�ɹ�!��鿴�Ƿ�ԭ�ɹ�!");
            }
            else
            {
                MessageBox.Show("���ǻ�ԭʧ��!");
            }
            btnInput.Visible = true;
        }

        /// <summary>
        /// �����е���ģʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput1_Click(object sender, EventArgs e)
        {
            if (cmbDatabases.Text == "��ѡ��")
            {
                MessageBox.Show("��ѡ����Ҫ��ԭ�����ݿ�", "��ʾ");
                return;
            }
            if (!System.IO.File.Exists(txtSourcePath1.Text))
            {
                MessageBox.Show("�����ļ�������,������ѡ��.", "��ʾ");
                return;
            }
            //�ַ���
            string Encoding = "gbk";
            if (rdbtnGBK.Checked == true)
            {
                Encoding = "gbk";
            }
            if (rdbtnUTF8.Checked == true)
            {
                Encoding = "utf8";
            }
            MessageBox.Show(MysqlExport(cmbDatabases.Text, txtSourcePath1.Text, Encoding));

        }

        /// <summary>
        /// ����sql�ļ�����mysql��
        /// </summary>
        /// <param name="FilePath">sql�ļ�·��</param>
        /// <returns>ִ�н��</returns>
        public string MysqlExport(string strDataBase, string strSQLFilePath, string Encoding)
        {
            //��鴫�ݵĲ���
            if (strDataBase == "")
            {
                return "���ݿ�������Ϊ��!";
            }
            if (strSQLFilePath == "")
            {
                return "SQL�ļ�·������Ϊ��!";
            }
            try
            {
                string cmd = "--host=127.0.0.1 --port=" + frmMain.MysqlDBPort + " --user=" + frmMain.MysqlDBUser + " --password=" + frmMain.MysqlDBPassWord + " --default-character-set=" + Encoding + " " + strDataBase + " < " + strSQLFilePath.Replace(" ", "\" \"");
                String s = "cd " + (strMysqlPath + "\\bin").Replace(" ", "\" \"");
                s += "," + strMysqlPath.Substring(0, 2);
                s += ",mysql.exe " + cmd;
                string strRst = IISControl.ExecuteCmd(s.Split(','));
                if (strRst.IndexOf("ERROR") != -1)
                {
                    return "SQL����ʧ��!����:" + strRst;
                }
                else
                {
                    return "SQL����ɹ�!";
                }
            }
            catch (Exception ex)
            {
                return "������:" + ex.Message.ToString();
            }
        }
        #endregion

        /// <summary>
        /// ѡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabases.Text != "��ѡ��")
            {
                lblDstpath.Text = strMysqlDataPath + "\\" + cmbDatabases.Text;
            }
            else
            {
                lblDstpath.Text = strMysqlDataPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cmbDatabases.Text != "��ѡ��")
            {
                MessageBox.Show("��ѡ��Ҫ���������ݿ�");
                return;
            }
            if (!System.IO.Directory.Exists(lblDstpath.Text))
            {
                MessageBox.Show("ѡ������ݿⲻ����");
                return;
            }
            if (MessageBox.Show("�Ƿ�������ݿ�?ִ�к�Ŀ�����ݿ����ݽ����ɻָ�.", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //


            }
        }

    }
}