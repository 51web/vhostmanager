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

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**  主要用于对mysql的控制

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
            cmbDatabases.Items.Add("请选择");
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
                MessageBox.Show("MYSQL数据路径设置不正确!需要在菜单[设置]--[数据库]--[MYSQL数据库配置]中设置数据路径.", "错误");
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
            if (cmbDatabases.Text.Trim() == "" || cmbDatabases.Text.Trim() == "请选择")
            {
                MessageBox.Show("请选择需要备份的数据库名!");
                return;
            }
            if (txtBackPath.Text.Trim() == "")
            {
                MessageBox.Show("请选择备份路径!");
                return;
            }
            string MysqlPath = strMysqlDataPath + "\\" + cmbDatabases.Text.Trim();
            if (!System.IO.Directory.Exists(strMysqlDataPath))
            {
                MessageBox.Show("MYSQL数据路径设置不正确!需要在菜单[设置]--[数据库]--[MYSQL数据库配置]中设置数据路径.", "错误");
                return;
            }
            if (!System.IO.Directory.Exists(MysqlPath))
            {
                MessageBox.Show("选择的数据库不存在,请重新选择!", "错误");
                return;
            }
            if (DirectoryCopy(MysqlPath, txtBackPath.Text.Trim() + "\\" + cmbDatabases.Text.Trim() + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), true))
            {
                MessageBox.Show("备份命令执行完成!请查看是否备份成功!");
                btnCha.Visible = true;
            }
            else
            {
                MessageBox.Show("备份失败!");
            }
            btnCha.Visible = true;
        }

        /// <summary>
        /// 创建虚拟目录
        /// </summary>
        /// <param name="sourceDirName">源文件目录名</param>
        /// <param name="destDirName">目标文件目录名</param>
        /// <param name="copySubDirs">是否拷贝子目录</param>

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
            if (cmbDatabases.Text.Trim() == "" || cmbDatabases.Text.Trim() == "请选择")
            {
                MessageBox.Show("请选择需要备份的数据库名!");
                return;
            }
            if (txtBackPath.Text.Trim() == "")
            {
                MessageBox.Show("请选择备份路径!");
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
                MessageBox.Show("备份命令执行完成!请查看是否备份成功!");
            }
            else
            {
                MessageBox.Show("备份失败!");
            }
            btnCha.Visible = true;
        }

        #region 导出sql文件(仅mysql)
        /// <summary>
        /// 导出sql文件
        /// </summary>
        /// <param name="strDataBase">数据库名</param>
        /// <param name="strSQLFilePath">sql文件路径</param>
        /// <returns>执行成功/失败</returns>
        public bool MysqlDump(string strDataBase, string MysqlDBUser, string MysqlDBPassWord, string strSQLFilePath)
        {
            string MysqlPath = strMysqlPath + "\\bin\\";
            //检查传递的参数           
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
                MessageBox.Show("备份路径不存在");
            }
        }

        /// <summary>
        /// 覆盖还原
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
        /// 导入SQL文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = "sql";
            file.Filter = "MYSQL数据库备份文件(*.sql)|*.sql";
            file.ShowDialog();
            txtSourcePath1.Text = file.FileName;
        }

        /// <summary>
        /// 刷新所有数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cmbDatabases.Items.Clear();
            cmbDatabases.Items.Add("请选择");
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
                MessageBox.Show("MYSQL数据路径设置不正确!需要在菜单[设置]--[数据库]--[MYSQL数据库配置]中设置数据路径.", "错误");
            }
            cmbDatabases.SelectedIndex = 0;
            MessageBox.Show("刷新成功", "提示");
        }

        #region  MYSQL还原

        /// <summary>
        /// 源文件覆盖模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
            btnInput.Visible = false;
            if (cmbDatabases.Text == "请选择")
            {
                MessageBox.Show("请选择需要还原的数据库", "提示");
                return;
            }
            if (!System.IO.Directory.Exists(txtSourcePath.Text))
            {
                MessageBox.Show("数据库备份数据\"源 路 径\"不正确", "错误");
                return;
            }
            if (!System.IO.Directory.Exists(lblDstpath.Text))
            {
                MessageBox.Show("数据库还原路径\"还原路径\"不正确", "错误");
                return;
            }
            if (lblDstpath.Text == txtSourcePath.Text)
            {
                MessageBox.Show("请选择数据库", "提示");
                return;
            }
            MessageBox.Show("为保证还原的正确性,建议还原前停止mysql服务,还原后再启动");
            //停止服务
            //开始服务
            //复制文件
            if (DirectoryCopy(txtSourcePath.Text, lblDstpath.Text, false))
            {
                MessageBox.Show("覆盖还原成功!请查看是否还原成功!");
            }
            else
            {
                MessageBox.Show("覆盖还原失败!");
            }
            btnInput.Visible = true;
        }

        /// <summary>
        /// 命令行导入模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput1_Click(object sender, EventArgs e)
        {
            if (cmbDatabases.Text == "请选择")
            {
                MessageBox.Show("请选择需要还原的数据库", "提示");
                return;
            }
            if (!System.IO.File.Exists(txtSourcePath1.Text))
            {
                MessageBox.Show("备份文件不存在,请重新选择.", "提示");
                return;
            }
            //字符集
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
        /// 导入sql文件（仅mysql）
        /// </summary>
        /// <param name="FilePath">sql文件路径</param>
        /// <returns>执行结果</returns>
        public string MysqlExport(string strDataBase, string strSQLFilePath, string Encoding)
        {
            //检查传递的参数
            if (strDataBase == "")
            {
                return "数据库名不能为空!";
            }
            if (strSQLFilePath == "")
            {
                return "SQL文件路径不能为空!";
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
                    return "SQL导入失败!错误:" + strRst;
                }
                else
                {
                    return "SQL导入成功!";
                }
            }
            catch (Exception ex)
            {
                return "出错了:" + ex.Message.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabases.Text != "请选择")
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
            if (cmbDatabases.Text != "请选择")
            {
                MessageBox.Show("请选择要操作的数据库");
                return;
            }
            if (!System.IO.Directory.Exists(lblDstpath.Text))
            {
                MessageBox.Show("选择的数据库不存在");
                return;
            }
            if (MessageBox.Show("是否清空数据库?执行后目标数据库数据将不可恢复.", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //


            }
        }

    }
}