using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Threading;
using System.DirectoryServices;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;

/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**  主要用于对站点进行管理

*********************************************************************************/

namespace VhostManage
{
    public partial class frmVhostManage : Form
    {
        public frmVhostManage()
        {
            InitializeComponent();
        }

        private void btnRandompassword_Click(object sender, EventArgs e)
        {
            txtSitePassword.Text = RandomPassword.GetRandomPassword(12);
        }

        private void frmVhosManage_Load(object sender, EventArgs e)
        {

            Application.DoEvents();
            string strSiteName = frmMain.SiteName.Trim();
            if (strSiteName != "")
            {
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;

                try
                {
                    string script = "";
                    OleCmd.CommandText = "select * from site where sitename = '" + strSiteName + "'";
                    OleConn.Open();
                    OleDbDataReader OleReader = null;
                    OleReader = OleCmd.ExecuteReader();
                    if (OleReader.HasRows)
                    {
                        OleReader.Read();
                        this.txtSiteName.Text = strSiteName;
                        this.txtSitePassword.Text = OleReader["sitepassword"].ToString();
                        this.txtSiteDir.Text = OleReader["sitedir"].ToString();
                        this.txtDiskQuota.Text = OleReader["diskquota"].ToString();
                        this.txtMaxBandwidth.Text = OleReader["maxbandwidth"].ToString();
                        this.txtMaxConnections.Text = OleReader["maxconnections"].ToString();
                        this.txtBindings.Text = OleReader["bindings"].ToString().Replace(",", "\r\n");
                        this.txtDefaultDoc.Text = OleReader["defaultdoc"].ToString().Replace(",", "\r\n");
                        this.cmbAppPoolId.Text = "";
                        //mysql
                        this.lbl_mysqldbname.Text = OleReader["mysqldbname"].ToString().Trim();
                        this.lbl_mysqldbuser.Text = OleReader["mysqldbuser"].ToString().Trim();
                        this.txt_mysqldbpassword.Text = OleReader["mysqldbpassword"].ToString().Trim();
                        //mssql
                        this.lbl_mssqldbname.Text = OleReader["mssqldbname"].ToString().Trim();
                        this.lbl_mssqldbuser.Text = OleReader["mssqldbuser"].ToString().Trim();
                        this.txt_mssqldbpassword.Text = OleReader["mssqldbpassword"].ToString().Trim();
                        //this.txt_mssqldbsize.Text = OleReader["mssqldbsize"].ToString().Trim();
                        //脚本script 
                        script = OleReader["script"].ToString().Trim();
                        OleReader.Close();
                        //asp; php; net2;
                        chkASP.Checked = false;
                        chkPHP.Checked = false;
                        chkNET1.Checked = false;
                        chkNET2.Checked = false;
                        chkNET4.Checked = false;
                        if (script.IndexOf("asp;") != -1)
                        {
                            chkASP.Checked = true;
                        }
                        if (script.IndexOf("php;") != -1)
                        {
                            chkPHP.Checked = true;
                        }
                        if (script.IndexOf("net1;") != -1)
                        {
                            chkNET1.Checked = true;
                        }
                        if (script.IndexOf("net2;") != -1)
                        {
                            chkNET2.Checked = true;
                        }
                        if (script.IndexOf("net4;") != -1)
                        {
                            chkNET4.Checked = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("未查到主机名为" + strSiteName + "的记录");
                        this.Close();
                    }
                    OleReader = null;
                    OleConn.Close();
                    //列出应用程序池
                    if (this.lbl_mysqldbname.Text.Trim() == "")
                    {
                        this.gpbxMysql.Visible = false;
                        //btn_ChangeMySQLDatabasePassword.Visible = false;
                    }
                    if (this.lbl_mssqldbname.Text.Trim() == "")
                    {
                        this.gpbxMssql.Visible = false;
                        //btn_ChangeMsSQLDatabasePassword.Visible = false;
                    }

                    if (this.gpbxMssql.Visible == false && this.gpbxMysql.Visible == false)
                    {
                        this.gpbxDatabase.Text = "该站点无相关数据库";
                    }
                    try
                    {
                        DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/W3SVC/AppPools");
                        IEnumerator AppPoolEnumer = appPoolRoot.Children.GetEnumerator();
                        while (AppPoolEnumer.MoveNext())
                        {
                            DirectoryEntry EntryPool = (System.DirectoryServices.DirectoryEntry)AppPoolEnumer.Current;
                            System.DirectoryServices.PropertyCollection properties = EntryPool.Properties;
                            IDictionaryEnumerator propertiesEnumer = properties.GetEnumerator();
                            cmbAppPoolId.Items.Add(EntryPool.Name.Trim());
                        }
                        appPoolRoot.Close();
                        cmbAppPoolId.SelectedIndex = 0;

                    }
                    catch
                    {
                        cmbAppPoolId.Items.Clear();
                    }
                }
                catch
                {
                    MessageBox.Show("查询出错");
                    this.Close();
                    try
                    {
                        OleCmd = null;
                        OleConn.Close();
                        OleConn = null;
                    }
                    catch { }
                }
            }
        }

        //FTP密码
        private void btnChangeFtpPass_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_SitePass = this.txtSitePassword.Text;
            if (!txtIsMatch.SecurePassword(s_SitePass))
            {
                MessageBox.Show("密码不符合规范,必须要求14位以上的数字、字母、特殊符号组合!");
                txtSitePassword.Focus();
                return;
            }
            if (IISControl.ChangeUserPassword(s_SiteName, s_SitePass) && IISControl.ModifyWebSite(IISControl.GetSiteID(s_SiteName), "AnonymousUserPass", s_SitePass))
            {
                MessageBox.Show("密码设置成功!");
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "update site set sitepassword = '" + s_SitePass + "' where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
                    OleConn.Close();
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                    try
                    {
                        OleCmd = null;
                        OleConn.Close();
                        OleConn = null;
                    }
                    catch { }
                }

            }
            else
            {
                MessageBox.Show("密码设置失败!");
            }
        }

        //磁盘配额
        private void btnChangeDiskQuota_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_DiskQuota = this.txtDiskQuota.Text;
            if (!txtIsMatch.DiskQuota(s_DiskQuota))
            {
                MessageBox.Show("空间大小错误!");
                return;
            }
            if (IISControl.UpdateDiskQuotas(this.txtSiteDir.Text.Substring(0, 3), s_SiteName, s_DiskQuota))
            {
                MessageBox.Show("空间大小设置成功!");
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "update site set diskquota = '" + s_DiskQuota + "' where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
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
            }
            else
            {
                MessageBox.Show("空间大小设置失败!");
            }

        }

        //网络带宽
        private void btnChangeMaxBandwidth_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_MaxBandwidth = txtMaxBandwidth.Text;
            if (!txtIsMatch.DiskQuota(s_MaxBandwidth))
            {
                MessageBox.Show("带宽限制输入错误!");
                return;
            }
            if (s_MaxBandwidth == "0")
                s_MaxBandwidth = "&HFFFFFFFF";
            else
                s_MaxBandwidth = (1024 * Convert.ToDouble(s_MaxBandwidth)).ToString();
            if (IISControl.EditWebSite(IISControl.GetSiteID(s_SiteName), "MaxBandwidth", s_MaxBandwidth))
            {
                MessageBox.Show("带宽限制设置成功!");
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "update site set maxbandwidth = '" + txtMaxBandwidth.Text + "' where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
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
            }
            else
            {
                MessageBox.Show("带宽限制设置失败!");

            }

        }
        //修改IIS数
        private void btnChangeMaxConnections_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_MaxConnections = txtMaxConnections.Text;
            if (!txtIsMatch.DiskQuota(s_MaxConnections))
            {
                MessageBox.Show("IIS连接数输入错误!");
                return;
            }
            if (s_MaxConnections == "0")
                s_MaxConnections = "&HFFFFFFFF";
            if (IISControl.EditWebSite(IISControl.GetSiteID(s_SiteName), "MaxConnections", s_MaxConnections))
            {
                MessageBox.Show("IIS连接数设置成功!");
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "update site set maxconnections = '" + txtMaxConnections.Text + "' where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
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
            }
            else
            {
                MessageBox.Show("IIS连接数设置失败!");
            }
        }

        //域名绑定
        private void btnChangeBindings_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string Domain = txtBindings.Text;
            Domain = Domain.Replace(" ", string.Empty);
            if (Domain.Replace("\r\n", "").Trim() == "")
            {
                MessageBox.Show("请输入绑定域名");
                txtBindings.Focus();
                return;
            }
            string strBinding = "";
            StringBuilder bing = new StringBuilder();
            string[] bds = Regex.Split(Domain, "\r\n", RegexOptions.IgnoreCase);
            IdnMapping idn = new IdnMapping();
            try
            {
                foreach (string bd in bds)
                {
                    bing.Append(idn.GetAscii(bd)).Append(";");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("绑定域名有误,不允许出现空格及空行!\r\n" + ex.ToString());
                txtBindings.Focus();
                return;
            }
            strBinding = ":80:" + bing.ToString().TrimEnd(';');
            strBinding = strBinding.Replace(";", "\r\n");
            string[] s_Bindings = Regex.Split(strBinding.Replace("\r\n", ",:80:"), ",", RegexOptions.IgnoreCase);
            if (IISControl.UpdateBindString(IISControl.GetSiteID(s_SiteName), s_Bindings))
            {
                MessageBox.Show("绑定域名成功!");
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "update site set bindings = '" + Domain.Replace("\r\n", ",") + "' where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
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
            }
            else
            {
                MessageBox.Show("绑定域名失败!");
            }

        }

        //默认文档
        private void btnChangeDefaultDoc_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_DefaultDoc = txtDefaultDoc.Text;
            if (s_DefaultDoc != "")
            {
                s_DefaultDoc = s_DefaultDoc.Replace("\r\n", ",");
            }
            else
            {
                MessageBox.Show("请输入默认文档!");
                txtDefaultDoc.Focus();
                return;
            }
            s_DefaultDoc = s_DefaultDoc.Trim(',');
            if (IISControl.EditWebSite(IISControl.GetSiteID(s_SiteName), "DefaultDoc", s_DefaultDoc))
            {
                MessageBox.Show("默认首页设置成功!");
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "update site set defaultdoc = '" + s_DefaultDoc + "' where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
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
            }
            else
            {
                MessageBox.Show("默认首页设置失败!");
            }
        }

        private void btnChangeScript_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;

            #region 脚本映射
            string Script = "";
            string strScript = "";
            if (chkASP.Checked)
            {
                strScript += "asp;";
                Script += frmMain.asp + ";";
            }
            if (chkPHP.Checked)
            {
                strScript += "php;";
                Script += frmMain.php + ";";
            }
            if (this.chkNET1.Checked)
            {
                strScript += "net1;";
                Script += frmMain.net1 + ";";
            }
            if (this.chkNET2.Checked)
            {
                strScript += "net2;";
                Script += frmMain.net2 + ";";
            }
            if (this.chkNET4.Checked)
            {
                strScript += "net4;";
                Script += frmMain.net4 + ";";
            }
            if (Script == "")
            {
                strScript = "asp;";
                Script = @".asp,c:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST;.asa,c:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST";
            }
            Script = Script.Substring(0, Script.Length - 1);
            string[] s_Scriptmap = System.Text.RegularExpressions.Regex.Split(Script, ";");
            for (int a = 0; a < s_Scriptmap.Length; a++)
            {
                Thread.Sleep(1);
                s_Scriptmap[a] = s_Scriptmap[a];
            }
            #endregion

            string siteid = IISControl.GetSiteID(s_SiteName);
            if (IISControl.ModifyWebSite(siteid, "ScriptMaps", s_Scriptmap))
            {
                try
                {
                    OleDbConnection OleConn = ConnClass.DataConn();
                    OleDbCommand OleCmd = new OleDbCommand();
                    OleCmd.Connection = OleConn;
                    OleCmd.CommandText = "update site set siteid = '" + siteid + "'" +
                                         ",script = '" + strScript + "'" +
                                         " where sitename = '" + s_SiteName + "'";
                    OleConn.Open();
                    OleCmd.ExecuteNonQuery();
                    OleConn.Close();
                }
                catch { }

                MessageBox.Show("脚本映射设置成功!");
            }
            else
            {
                MessageBox.Show("脚本映射设置失败!");
            }

        }

        private void chkNET1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNET1.Checked)
            {
                chkNET2.Checked = false;
                chkNET4.Checked = false;
            }
        }

        private void chkNET2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNET2.Checked)
            {
                chkNET1.Checked = false;
                chkNET4.Checked = false;
            }
        }

        private void chkNET4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNET4.Checked)
            {
                chkNET1.Checked = false;
                chkNET2.Checked = false;
            }
        }

        /// <summary>
        /// 修改MySQL密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ChangeMySQLDatabasePassword_Click(object sender, EventArgs e)
        {
            if (lbl_mysqldbname.Text.Trim() != "" || this.lbl_mysqldbuser.Text.Trim() != "")
            {
                if (this.txt_mysqldbpassword.Text.Trim() == "")
                {
                    MessageBox.Show("请输入数据库密码!");
                }
                else
                {
                    if (IISControl.ChangeMySQLPassword(this.lbl_mysqldbname.Text.Trim(), this.lbl_mysqldbuser.Text.Trim(), this.txt_mysqldbpassword.Text.Trim()))
                    {

                        OleDbConnection OleConn = ConnClass.DataConn();
                        OleDbCommand OleCmd = new OleDbCommand();
                        OleCmd.Connection = OleConn;
                        try
                        {
                            OleCmd.CommandText = "update site set mysqldbpassword = '" + this.txt_mysqldbpassword.Text.Trim() + "' where sitename = '" + this.txtSiteName.Text.Trim() + "'";
                            OleConn.Open();
                            OleCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            IISControl.WriteLog(ex.ToString());
                        }
                        finally
                        {
                            OleConn.Close();
                            OleConn = null;
                        }
                        MessageBox.Show("数据库密码修改成功!");
                    }
                    else
                    {
                        MessageBox.Show("数据库密码修改失败!");
                    }
                }
            }
            else
            {
                MessageBox.Show("数据库名或用户名不能为空!");
            }
        }

        private void btn_ChangeMsSQLDatabasePassword_Click(object sender, EventArgs e)
        {

            if (lbl_mysqldbname.Text.Trim() != "" || this.lbl_mysqldbuser.Text.Trim() != "")
            {
                if (this.txt_mysqldbpassword.Text.Trim() == "")
                {
                    MessageBox.Show("请输入数据库密码!");
                }
                else
                {
                    if (IISControl.ChangeMsSQLPassword(this.lbl_mssqldbname.Text.Trim(), this.lbl_mssqldbuser.Text.Trim(), this.txt_mssqldbpassword.Text.Trim()))
                    {
                        OleDbConnection OleConn = ConnClass.DataConn();
                        OleDbCommand OleCmd = new OleDbCommand();
                        OleCmd.Connection = OleConn;
                        try
                        {
                            OleCmd.CommandText = "update site set mysqldbpassword = '" + this.txt_mysqldbpassword.Text.Trim() + "' where sitename = '" + this.txtSiteName.Text.Trim() + "'";
                            OleConn.Open();
                            OleCmd.ExecuteNonQuery();
                        }
                        catch
                        { }
                        finally
                        {
                            OleConn.Close();
                            OleConn = null;
                        }
                        MessageBox.Show("数据库密码修改成功!");
                    }
                    else
                    {
                        MessageBox.Show("数据库密码修改失败!");
                    }
                }
            }
            else
            {
                MessageBox.Show("数据库名或用户名不能为空!");
            }

        }

    }
}