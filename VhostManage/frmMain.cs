using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Collections;
using System.Data.OleDb;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;
using System.Security.Cryptography;
using System.Management;
using System.Globalization;

/********************************************************************************

** 创始时间：2010-03-19

** 修改时间：2012-09-21

** 修改程序本身的配置

** 描述：

**  主要用于对站点的控制

*********************************************************************************/
namespace VhostManage
{
    public partial class frmMain : Form
    {

        public static string VersionNum = "3.0";
        public static string UpdateTime = "2011-05-10";
        public static string SiteName = "";
        public static string MysqlPath = "root";
        public static string MysqlDBPath = "root";
        public static string MysqlDBUser = "root";
        public static string MysqlDBPassWord = "set_your_passwd_here";
        public static string MysqlDBPort = "3306";

        public static string MssqlDBUser = "sa";
        public static string MssqlDBPassWord = "set_your_passwd_here";
        public static string MssqlDBPort = "1433";
        public static string MssqlDBPath = "d:\\mssql";
        public static string SitePath = "d:\\wwwroot";

        public frmMain()
        {
            InitializeComponent();
        }

        public static string asp = @".asp,C:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST;.asa,C:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST";
        public static string php = @".php,C:\VhostManage\PHP\php5isapi.dll,5,GET,HEAD,POST";
        public static string net1 = @".asax,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.ascx,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.ashx,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.asmx,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.aspx,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.axd,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.config,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.cs,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.csproj,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.licx,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.rem,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.resources,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.resx,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.soap,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.vb,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.vbproj,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST;.vsdisco,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,1,GET,HEAD,POST;.webinfo,C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll,5,GET,HEAD,POST";
        public static string net2 = @".asax,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.ascx,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.ashx,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.asmx,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.aspx,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.axd,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.config,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.cs,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.csproj,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.licx,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.rem,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.resources,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.resx,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.soap,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.vb,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.vbproj,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST;.vsdisco,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,1,GET,HEAD,POST;.webinfo,C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll,5,GET,HEAD,POST";
        public static string net4 = @".asax,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.ascx,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.ashx,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.asmx,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.aspx,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.axd,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.config,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.cs,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.csproj,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.licx,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.rem,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.resources,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.resx,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.soap,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.vb,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.vbproj,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST;.vsdisco,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,1,GET,HEAD,POST;.webinfo,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll,5,GET,HEAD,POST";
        public static string sitedir = "";
        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        private static frmSeting frmSeting = new frmSeting();
        private static frmIIS frmIIS = new frmIIS();
        private static frmMysqlBack frmMysqlBack = null;// = new frmMysqlBack();
        private static frmMssqlBack frmMssqlBack = null;// = new frmMssqlBack();
        private static frmIISReset frmIISReset = new frmIISReset();
        private static frmImport frmImport = new frmImport();

        Read.Ini ini = new Read.Ini();
        private void 主机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmSeting == null || frmSeting.IsDisposed)//判断是否存在窗口
            {
                frmSeting = new frmSeting();
            }

            frmSeting.Show();
            frmSeting.Activate();
        }

        private void btnRandompassword_Click(object sender, EventArgs e)
        {
            string Password = RandomPassword.GetRandomPassword(12);
            this.txtSitePassword.Text = Password;
            this.txtMySQLDBPassword.Text = Password;
            this.txtMsSQLDBPassword.Text = Password;
        }
        //检测站点是否存在
        private void txtSiteName_TextChanged(object sender, EventArgs e)
        {
            txtSiteDir.Text = sitedir + "\\" + txtSiteName.Text.Trim();
            txtMySQLDBName.Text = "db_" + txtSiteName.Text.Trim();
            txtMsSQLDBName.Text = "sq_" + txtSiteName.Text.Trim();
            //string SiteID = "0";
            //if (!txtSiteName.Focused)
            //{
            //    if (txtSiteName.Text != "")
            //        SiteID = IISControl.GetSiteID(txtSiteName.Text);
            //    if (SiteID != "0")
            //        MessageBox.Show("站点:" + txtSiteName.Text + " 已存在");
            //}
            //txtSiteName.Focus();

        }

        private void btnADDSite_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            string SiteID = "0";//站点标识符
            string s_SiteName = txtSiteName.Text.Trim().ToLower();//站点名
            string s_SitePass = txtSitePassword.Text.Trim();//站点密码
            string SiteDir = txtSiteDir.Text.Trim();//站点路径
            string s_WebStatus = "0"; //WEB初始状态（正常 = 0 停止 = 1 需要跳转 = 2）
            string s_FtpStatus = "0"; //FTP初始状态（正常 = 0 停止 = 1）     
            string s_DiskQuota = "0"; //空间大小
            string s_IPAddress = "";
            string s_MaxBandwidth = "0";//带宽
            string s_MaxConnections = "0";//连接数据
            string s_DefaultDoc = "index.html,default.htm,default.html,index.asp,index.php,index.aspx,default.aspx";
            string s_IPSecurity = "";
            string s_UrlForward = "";
            string strApp = "";//应用程序池名
            string mysqldbname = "";//数据库名
            string mysqldbpassword = "";//数据库密码 
            string mssqldbname = "";//数据库名
            string mssqldbpassword = "";//数据库密码
            string mssqldbsize = "50";//数据库大小
            if (chkAppPoolId.Checked)
            {
                strApp = s_SiteName;
            }
            else
            {
                strApp = cmbAppPoolId.Text;
            }
            s_DiskQuota = txtDiskQuota.Text.Trim();
            s_MaxBandwidth = txtMaxBandwidth.Text.Trim();
            s_MaxConnections = txtMaxConnections.Text.Trim();


            #region 数据验证
            if (s_SiteName == "")
            {
                MessageBox.Show("请输入FTP用户名");
                txtSiteName.Focus();
                return;
            }
            if (!txtIsMatch.SiteName(s_SiteName))
            {
                MessageBox.Show("您输入的FTP用户名无效，应为3-20位字母、数字、-_组合,-_不能出现在开头和结尾");
                txtSiteName.Focus();
                return;
            }
            SiteID = IISControl.GetSiteID(s_SiteName);
            if (SiteID != "0")
            {
                MessageBox.Show("站点:" + txtSiteName.Text.ToLower() + " 已存在");
                txtSiteName.Focus();
                return;
            }
            if (s_SitePass == "")
            {
                MessageBox.Show("请输入FTP密码");
                txtSitePassword.Focus();
                return;
            }
            if (!txtIsMatch.SecurePassword(s_SitePass))
            {
                MessageBox.Show("您输入的FTP密码无效，必须为字母、数字、特殊符号组合且长度为14位以上");
                txtSitePassword.Focus();
                return;
            }
            if (txtDefaultDoc.Text.Trim() != "")
            {
                s_DefaultDoc = txtDefaultDoc.Text.Trim().Replace("\r\n", ",");
            }
            else
            {
                MessageBox.Show("请输入默认文档!");
                txtDefaultDoc.Focus();
                return;
            }
            s_DefaultDoc = s_DefaultDoc.Trim(',');
            if (chkAddMySQL.Checked)
            {
                if (txtMySQLDBName.Text.Trim().Length > 16)
                {
                    MessageBox.Show("MYSQL数据库创建失败!您输入的MYSQL数据库名太长，不能超过16个字符。");
                    return;
                }
            }
            #endregion

            string strScript = "";

            #region 脚本映射
            string Script = "";
            if (chkASP.Checked)
            {
                strScript += "asp;";
                Script += asp + ";";
            }
            if (chkPHP.Checked)
            {
                strScript += "php;";
                Script += php + ";";
            }
            if (this.chkNET1.Checked)
            {
                strScript += "net1;";
                Script += net1 + ";";
            }
            if (this.chkNET2.Checked)
            {
                strScript += "net2;";
                Script += net2 + ";";
            }
            if (this.chkNET4.Checked)
            {
                strScript += "net4;";
                Script += net4 + ";";
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
                s_Scriptmap[a] = s_Scriptmap[a].Trim();
            }
            #endregion

            //拆分绑定域名

            #region
            string Domain = txtBing.Text;
            Domain = Domain.Replace(" ", string.Empty);
            if (Domain.Replace("\r\n", "").Trim() == "")
            {
                MessageBox.Show("请输入绑定域名");
                txtBing.Focus();
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
                txtBing.Focus();
                return;
            }
            strBinding = ":80:" + bing.ToString().TrimEnd(';');
            strBinding = strBinding.Replace(";", "\r\n");
            string[] s_Bindings = Regex.Split(strBinding.Replace("\r\n", ",:80:"), ",", RegexOptions.IgnoreCase);
            #endregion
            string[] ipaddress;
            ipaddress = Regex.Split(s_IPAddress, ",", RegexOptions.IgnoreCase);
            object[] ipls = new object[ipaddress.Length];
            for (int c = 0; c < ipaddress.Length; c++)
            {
                Thread.Sleep(1);
                ipls[c] = ipaddress[c].Replace("/", ",");
            }
            string siteid = "";

            #region
            if (!Directory.Exists(SiteDir))
            {
                Directory.CreateDirectory(SiteDir);
            }
            string temp = "";
            if (IISControl.CreateWebSite(s_SiteName, s_SitePass, SiteDir, s_Bindings, s_MaxBandwidth, s_MaxConnections, s_DefaultDoc, s_Scriptmap, ipls, s_IPSecurity, s_WebStatus, s_UrlForward, strApp))
                if (IISControl.CreateFtpVDir(s_SiteName, SiteDir, s_FtpStatus))
                {
                    IISControl.DelIISUser(s_SiteName);
                    if (IISControl.CreateIISUser(s_SiteName, s_SitePass))
                    {
                        if (this.txtDiskQuota.Text != "0")
                        {
                            IISControl.CreateDiskQuotas(SiteDir.Substring(0, 3), s_SiteName, s_DiskQuota);
                        }

                        if (IISControl.addDirectorySecurity(SiteDir, s_SiteName))
                        {
                            //设置状态
                            IISControl.SetChown(s_SiteName, SiteDir);
                            siteid = IISControl.GetSiteID(s_SiteName);
                            if (s_WebStatus == "1")
                            {
                                IISControl.ChangeWebSiteStatus(siteid, s_WebStatus);
                            }
                            temp = "站点创建成功!";
                        }
                        else
                        {
                            temp = "站点创建成功!但站点权限应用失败!";
                        }
                        //创建数据库
                        ListSite();
                        if (chkAddMySQL.Checked)
                        {
                            bool creatmysql = false;
                            mysqldbname = txtMySQLDBName.Text.Trim();
                            mysqldbpassword = txtMySQLDBPassword.Text.Trim();
                            if (!txtIsMatch.SiteName(mysqldbname) && !txtIsMatch.SitePassword(mysqldbpassword))
                            {
                                temp += "您输入的MYSQL数据库名或MYSQL密码无效，应为字母、数字组合。";
                                creatmysql = false;
                            }
                            else if (mysqldbname.Length > 16)
                            {
                                temp += "MYSQL数据库创建失败!您输入的MYSQL数据库名太长，不能超过16个字符。";
                                creatmysql = false;
                            }
                            else
                            {
                                creatmysql = true;
                            }
                            if (creatmysql)
                            {

                                //建库
                                if (IISControl.CreateDataBases(mysqldbname, mysqldbname, mysqldbpassword))
                                {
                                    temp += "MYSQL数据库创建成功!";
                                }
                                else
                                {
                                    temp += "MYSQL数据库创建失败!请确认MySQL服务是否已启动。或者数据库管理帐户密码不正确,可以在设置里面修改。";
                                    mysqldbname = "";
                                    mysqldbpassword = "";
                                }
                            }
                            else
                            {
                                //temp += "MYSQL数据库创建失败!";
                                mysqldbname = "";
                                mysqldbpassword = "";
                            }

                        }
                        if (chkAddMsSQL.Checked)
                        {
                            bool creatmssql = false;

                            try
                            {
                                int Size = int.Parse(txtDBSize.Text);

                                mssqldbsize = txtDBSize.Text;
                            }
                            catch { }
                            mssqldbname = txtMsSQLDBName.Text.Trim();
                            mssqldbpassword = txtMsSQLDBPassword.Text.Trim();
                            if (!txtIsMatch.SiteName(mssqldbname) && !txtIsMatch.SitePassword(mssqldbpassword))
                            {
                                temp += "您输入的MSSQL数据库名或MSSQL密码无效，应为字母、数字组合。";
                                creatmssql = false;
                            }
                            else
                            {
                                creatmssql = true;
                            }
                            if (creatmssql)
                            {
                                //建库
                                if (IISControl.CreateMsSQL(mssqldbname, mssqldbname, mssqldbpassword, mssqldbsize))
                                {
                                    temp += "MSSQL数据库创建成功!";
                                }
                                else
                                {
                                    temp += "MSSQL数据库创建失败!管理帐户密码不正确,可以在设置里面修改。";
                                    mssqldbname = "";
                                    mssqldbpassword = "";
                                }
                            }
                            else
                            {
                                temp += "MSSQL数据库创建失败!";
                                mssqldbname = "";
                                mssqldbpassword = "";
                            }

                        }
                        MessageBox.Show(temp);
                        clearTxt();
                        //入库
                        OleDbConnection OleConn = ConnClass.DataConn();
                        OleDbCommand OleCmd = new OleDbCommand();
                        OleCmd.Connection = OleConn;
                        string strSQL = "";
                        try
                        {

                            bool ok = false;
                            OleCmd.CommandText = "select * from site where sitename = '" + s_SiteName + "'";
                            OleConn.Open();
                            OleDbDataReader OleReader = null;
                            OleReader = OleCmd.ExecuteReader();
                            if (OleReader.HasRows)
                            {
                                ok = true;
                            }
                            OleReader = null;
                            OleConn.Close();
                            OleConn.Open();
                            if (ok)
                            {
                                strSQL = "update site set siteid = '" + siteid + "'" +
                                        ",sitepassword = '" + s_SitePass + "'" +
                                        ",sitedir = '" + SiteDir + "'" +
                                        ",bindings = '" + Domain.Replace("\r\n", ",") + "'" +
                                        ",defaultdoc = '" + s_DefaultDoc + "'" +
                                        ",diskquota = '" + s_DiskQuota + "'" +
                                        ",maxbandwidth = '" + s_MaxBandwidth + "'" +
                                        ",maxconnections = '" + s_MaxConnections + "'" +
                                        ",script = '" + strScript + "'" +
                                        ",webstatus = '0'" +
                                        ",ftpstatus = '0'" +
                                        ",mysqldbname = '" + mysqldbname + "'" +
                                        ",mysqldbuser = '" + mysqldbname + "'" +
                                        ",mysqldbpassword = '" + mysqldbpassword +
                                        ",mssqldbname = '" + mssqldbname + "'" +
                                        ",mssqldbuser = '" + mssqldbname + "'" +
                                        ",mssqldbpassword = '" + mssqldbpassword +
                                        ",mssqldbsize = '" + mssqldbsize + "' where sitename = '" + s_SiteName + "'";
                            }
                            else
                            {
                                strSQL = "insert into site(siteid,sitename,sitepassword,sitedir,bindings,defaultdoc,diskquota,maxbandwidth,maxconnections,script,mysqldbname,mysqldbuser,mysqldbpassword,mssqldbname,mssqldbuser,mssqldbpassword,mssqldbsize,remark) " +
                                         "values('" + siteid + "','" + s_SiteName + "','" + s_SitePass + "','" + SiteDir + "','" + Domain.Replace("\r\n", ",") + "','" + s_DefaultDoc + "','" + s_DiskQuota + "','" + s_MaxBandwidth + "', '" + s_MaxConnections + "', '" + strScript + "','" + mysqldbname + "','" + mysqldbname + "','" + mysqldbpassword + "','" + mssqldbname + "','" + mssqldbname + "','" + mssqldbpassword + "','" + mssqldbsize + "','')";
                            }
                            OleCmd.CommandText = strSQL;
                            if (!(OleCmd.ExecuteNonQuery() > 0))
                            {
                                //入库失败
                            } OleConn.Close();

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
                        IISControl.DelIISUser(s_SiteName);
                        IISControl.DeleteFtpVDir(s_SiteName);
                        IISControl.DeleteWebSite(s_SiteName);
                        MessageBox.Show("站点创建失败,创建IIS帐户失败！");
                        //Temp = "<return_code><![CDATA[433]]></return_code><return_msg><![CDATA[创建帐户失败!]]></return_msg>";
                    }
                }
                else
                {
                    IISControl.DeleteFtpVDir(s_SiteName);
                    IISControl.DeleteWebSite(s_SiteName);
                    MessageBox.Show("站点创建失败,创建FTP失败！");
                    //Temp = "<return_code><![CDATA[433]]></return_code><return_msg><![CDATA[创建ftp失败!]]></return_msg>";
                }
            else
            {
                IISControl.DeleteWebSite(s_SiteName);
                MessageBox.Show("站点创建失败！");
            }
            #endregion

            ListSite();

        }
        //重置
        private void clearTxt()
        {
            txtSiteName.Text = "";
            txtSitePassword.Text = "";
            txtSiteDir.Text = "";
            txtDiskQuota.Text = "0";
            txtMaxBandwidth.Text = "0";
            txtMaxConnections.Text = "0";
            txtBing.Text = "";
            chkASP.Checked = true;
            chkPHP.Checked = true;
            chkNET1.Checked = false;
            chkNET2.Checked = false;
            chkAppPoolId.Checked = false;
            chkAddMySQL.Checked = true;
            txtMySQLDBName.Text = "";
            txtMySQLDBPassword.Text = "";
        }

        public string getLocalIP()
        {
            return (Dns.GetHostAddresses(Dns.GetHostName())[0].ToString());
        }

        //获取本机的MAC
        public string getLocalMac()
        {
            string mac = null;
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return (mac);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //线程          
            try
            {
                string sysConfig = Application.StartupPath.ToString() + "\\config.ini";
                //检查程序配置文件是否存在
                if (!System.IO.File.Exists(sysConfig))
                {
                    IISControl.WriteTxt(Application.StartupPath.ToString() + "\\config.ini", "", true, Encoding.Default);
                }
                if (!System.IO.File.Exists(sysConfig))
                {
                    MessageBox.Show("程序已损坏,请重新下载!", "错误");
                    this.Close();
                    Application.Exit();
                }
                try
                {
                    Read.Ini ini = new Read.Ini();
                    //检查数据库配置文件是否存在
                    string dbpath = ini.IniReadValue("SysConfig", "DBPath");
                    if (dbpath == "defalut")
                    {
                        dbpath = Application.StartupPath.ToString() + "\\DataBase\\site.mdb";
                    }
                    if (!System.IO.File.Exists(dbpath))
                    {

                        if (MessageBox.Show("程序配置数据库(site.mdb)找不到,是否选择路径？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bool reSelect = true;
                            //选择路径并写入配置文件
                            FolderBrowserDialog dd = new FolderBrowserDialog();
                            while (reSelect)
                            {
                                dd.ShowDialog();
                                if (!System.IO.Directory.Exists(dd.SelectedPath))
                                {
                                    if (MessageBox.Show("路径不正确,是否重新选择路径？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        reSelect = true;
                                    }
                                    else
                                    {
                                        reSelect = false;
                                        this.Close();
                                        Application.Exit();
                                    }
                                }
                                else
                                {
                                    dbpath = dd.SelectedPath + "\\site.mdb";
                                    if (!System.IO.File.Exists(dbpath))
                                    {
                                        if (MessageBox.Show("路径:" + dd.SelectedPath + "下数据库site.mdb不存在,是否重新选择路径？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            reSelect = true;
                                        }
                                        else
                                        {
                                            reSelect = false;
                                            this.Close();
                                            Application.Exit();
                                        }
                                    }
                                    else
                                    {
                                        //写入配置文件
                                        ini.IniWriteValue("SysConfig", "DBPath", dd.SelectedPath + "\\site.mdb");
                                        reSelect = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("程序配置数据库已损坏,请重新下载!", "错误");
                            this.Close();
                            Application.Exit();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("程序配置数据库已损坏,请重新下载安装!", "错误");
                    this.Close();
                    Application.Exit();
                }
            }
            catch
            {
                MessageBox.Show("程序已损坏(配置文件无法加载),请重新下载安装!", "错误");
                this.Close();
                Application.Exit();
            }
            //列出应用程序池
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
                chkAppPoolId.Checked = false;
            }
            catch
            {
                cmbAppPoolId.Items.Clear();
                cmbAppPoolId.Visible = false;
                chkAppPoolId.Checked = true;
            }
            //读出设置的路径
            OleDbConnection OleConn = ConnClass.DataConn();
            OleDbCommand OleCmd = new OleDbCommand();
            OleCmd.Connection = OleConn;
            OleDbDataReader OleReader = null;
            try
            {
                OleConn.Open();
                OleCmd.CommandText = "select * from vhostseting where type='vhost'";
                OleReader = OleCmd.ExecuteReader();
                if (OleReader.HasRows)
                {
                    OleReader.Read();
                    txtSiteDir.Text = OleReader["vhostdir"].ToString();
                    SitePath = OleReader["vhostdir"].ToString();
                    sitedir = txtSiteDir.Text;
                    asp = OleReader["asp"].ToString();
                    php = OleReader["php"].ToString();
                    net1 = OleReader["net1"].ToString();
                    net2 = OleReader["net2"].ToString();
                    txtDefaultDoc.Text = OleReader["defaultdoc"].ToString().Replace(",", "\r\n");
                    OleReader.Close();
                }
                OleConn.Close();
            }
            catch
            {
                try
                {
                    OleConn.Close();
                }
                catch { }
            }
            try
            {
                //读配置文件
                MysqlPath = ini.IniReadValue("MysqlConfig", "MysqlPath");
                MysqlDBPort = ini.IniReadValue("MysqlConfig", "MysqlPort");
                MysqlDBPath = ini.IniReadValue("MysqlConfig", "MysqlDBPath");
                MysqlDBUser = ini.IniReadValue("MysqlConfig", "MysqlRoot");
                MysqlDBPassWord = ini.IniReadValue("MysqlConfig", "MysqlPassword");

                //mssql配置
                MssqlDBPath = ini.IniReadValue("MssqlConfig", "MssqlDBPath");
                MssqlDBPort = ini.IniReadValue("MssqlConfig", "MssqlPort");
                MssqlDBUser = ini.IniReadValue("MssqlConfig", "MssqlSa");
                MssqlDBPassWord = ini.IniReadValue("MssqlConfig", "MssqlPassword");
            }
            catch { }

            if (txtSiteDir.Text.ToString() == "")
            {
                txtSiteDir.Text = "d:\\wwwroot\\";
            }
            else
            {
                txtSiteDir.Text += "\\";
            }
            if (chkAddMySQL.Checked)
            {
                gbxMYSQL.Visible = true;
            }
            else
            {
                gbxMYSQL.Visible = false;

            }
            if (chkAddMsSQL.Checked)
            {
                gbxMSSQL.Visible = true;
            }
            else
            {
                gbxMSSQL.Visible = false;

            }

            ListSite();

        }
        //列出所有站点
        public void ListSite()
        {
            Application.DoEvents();
            lstSite.Items.Clear();
            OleDbConnection OleConn = ConnClass.DataConn();
            OleDbCommand OleCmd = new OleDbCommand();
            OleCmd.Connection = OleConn;
            try
            {
                OleCmd.CommandText = "select * from site";
                OleConn.Open();
                OleDbDataReader OleReader = null;
                OleReader = OleCmd.ExecuteReader();
                if (OleReader.HasRows)
                {
                    while (OleReader.Read())
                    {
                        lstSite.Items.Add(OleReader["sitename"].ToString());
                    }
                    OleReader.Close();
                }
                OleConn.Close();
            }
            catch //(Exception ex)
            {
                //regsvr32 msjetoledb40.dll
                //MessageBox.Show(ex.ToString());
                try
                {
                    OleCmd = null;
                    OleConn.Close();
                    OleConn = null;
                }
                catch { }
            }
        }
        //状态切换
        private void chkAppPoolId_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAppPoolId.Checked)
                cmbAppPoolId.Visible = false;
            else
                cmbAppPoolId.Visible = true;
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

        private void chkAddMySQL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddMySQL.Checked)
            {
                gbxMYSQL.Visible = true;
            }
            else
            {
                gbxMYSQL.Visible = false;
            }
        }

        private void 升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://update.example.com/");
        }
        private static frmAbout frmAbout = new frmAbout();
        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frmAbout == null || frmAbout.IsDisposed)//判断是否存在窗口
            {
                frmAbout = new frmAbout();
            }
            frmAbout.Show();
            frmAbout.Activate();
        }

        private static frmGongneng frmGongneng = new frmGongneng();
        private void 使用说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmGongneng == null || frmGongneng.IsDisposed)//判断是否存在窗口
            {
                frmGongneng = new frmGongneng();
            }
            frmGongneng.Show();
            frmGongneng.Activate();

        }

        private void 数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmSeting == null || frmSeting.IsDisposed)//判断是否存在窗口
            {
                frmSeting = new frmSeting();
            }
            frmSeting.Show();
            frmSeting.Activate();
        }

        private void 更多功能ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmMore frmMore = new frmMore();
            //frmMore.Show();
        }
        //跳转到主机功能
        private static frmVhostManage frmVhosManage = new frmVhostManage();

        private void btnSiteManage_Click(object sender, EventArgs e)
        {
            SiteName = this.lstSite.Text;
            if (SiteName.Trim() == "")
            {
                MessageBox.Show("请选择站点名");
                this.lstSite.Focus();
            }
            else
            {
                if (frmVhosManage == null || frmVhosManage.IsDisposed)//判断是否存在窗口
                {
                    frmVhosManage = new frmVhostManage();
                }

                frmVhosManage.Show();
                frmVhosManage.Activate();

            }
        }
        //删除选中站点
        private void btnDelSite_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            string strSiteName = this.lstSite.Text;
            string MySqlDatabaseName = "";
            string MySqlDatabaseUser = "";
            string MsSqlDatabaseName = "";
            string MsSqlDatabaseUser = "";
            string SiteDir = "";
            if (strSiteName.Trim() == "")
            {
                MessageBox.Show("请选择要删除的站点!");
                this.lstSite.Focus();
                return;
            }
            OleDbConnection OleConn = ConnClass.DataConn();
            OleDbCommand OleCmd = new OleDbCommand();
            OleCmd.Connection = OleConn;

            try
            {
                OleCmd.CommandText = "select * from site where sitename = '" + strSiteName + "'";
                OleConn.Open();
                OleDbDataReader OleReader = null;
                OleReader = OleCmd.ExecuteReader();
                if (OleReader.HasRows)
                {
                    OleReader.Read();
                    SiteDir = OleReader["sitedir"].ToString();
                    MySqlDatabaseName = OleReader["mysqldbname"].ToString();
                    MySqlDatabaseUser = OleReader["mysqldbuser"].ToString();
                    MsSqlDatabaseName = OleReader["mssqldbname"].ToString();
                    MsSqlDatabaseUser = OleReader["mssqldbuser"].ToString();
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
                }
                catch { }

            }
            if (MessageBox.Show("确认删除该主机吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //删除站点的所有配置和数据             

                if (MessageBox.Show("是否删除该主机所有数据？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (SiteDir != "")
                        {

                            if (Directory.Exists(SiteDir))
                            {
                                Directory.Delete(SiteDir, true);
                            }
                            IISControl.RemoveDiskQuotas(SiteDir.Substring(0, 3), strSiteName);
                        }
                    }
                    catch { }
                }
                //删除数据库
                if (MySqlDatabaseName != "")
                {
                    if (MessageBox.Show("是否删除该主机的数据库？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (MySqlDatabaseName != "")
                        {
                            IISControl.DeleteMySqlDataBase(MySqlDatabaseName, MySqlDatabaseUser);
                        }
                        if (MsSqlDatabaseName != "")
                        {
                            IISControl.DeleteMsSqlDataBase(MsSqlDatabaseName, MsSqlDatabaseUser);
                        }

                    }
                }

                if (MessageBox.Show("是否删除该主机的所有配置？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    IISControl.DeleteWebSite(strSiteName);
                    IISControl.DeleteFtpVDir(strSiteName);

                    OleCmd.CommandText = "delete from site where sitename = '" + strSiteName + "'";
                    try
                    {
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
                        }
                        catch { }
                    }
                    IISControl.DelIISUser(strSiteName);
                }

                MessageBox.Show("站点删除成功!");
                ListSite();
            }
        }

        private static frmFtpSeting frmFtpSeting = new frmFtpSeting();

        private void FTP被动模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmFtpSeting == null || frmFtpSeting.IsDisposed)//判断是否存在窗口
            {
                frmFtpSeting = new frmFtpSeting();
            }

            frmFtpSeting.Show();
            frmFtpSeting.Activate();

        }

        private void chkAddMsSQL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddMsSQL.Checked)
            {
                gbxMSSQL.Visible = true;
            }
            else
            {
                gbxMSSQL.Visible = false;

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

        private void iIS重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmIIS == null || frmIIS.IsDisposed)//判断是否存在窗口
            {
                frmIIS = new frmIIS();
            }
            frmIIS.Show();
            frmIIS.Activate();
        }

        private void mYSQL备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmMysqlBack == null || frmMysqlBack.IsDisposed)//判断是否存在窗口
            {
                frmMysqlBack = new frmMysqlBack();
            }

            if (!System.IO.Directory.Exists(MysqlPath))
            {
                MessageBox.Show("MYSQL程序路径设置不正确!需要在菜单[设置]--[数据库]--[MYSQL数据库配置]中设置程序路径.", "错误");
                return;
            }
            if (!System.IO.Directory.Exists(MysqlDBPath))
            {
                MessageBox.Show("MYSQL数据路径设置不正确!需要在菜单[设置]--[数据库]--[MYSQL数据库配置]中设置数据路径.", "错误");
                return;
            }
            frmMysqlBack.Show();
            frmMysqlBack.Activate();
        }

        private void mSSQL备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmMysqlBack == null || frmMysqlBack.IsDisposed)//判断是否存在窗口
            {
                frmMssqlBack = new frmMssqlBack();
            }
            frmMssqlBack.Show();
            frmMssqlBack.Activate();
        }

        private void 站点恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmIISReset == null || frmIISReset.IsDisposed)//判断是否存在窗口
            {
                frmIISReset = new frmIISReset();
            }
            frmIISReset.Show();
            frmIISReset.Activate();


        }

        private void 老版本数据导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmImport == null || frmImport.IsDisposed)//判断是否存在窗口
            {
                frmImport = new frmImport();
            }

            frmImport.Show();
            frmImport.Activate();
        }

        private void mYSQL备份还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (frmMysqlBack == null || frmMysqlBack.IsDisposed)//判断是否存在窗口
                {
                    frmMysqlBack = new frmMysqlBack();
                }
                Read.Ini ini = new Read.Ini();


                if (!System.IO.Directory.Exists(MysqlPath))
                {
                    MessageBox.Show("MYSQL程序路径设置不正确!需要在菜单[设置]--[数据库]--[MYSQL数据库配置]中设置程序路径.", "错误");
                    return;
                }
                frmMysqlBack.Show();
                frmMysqlBack.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void mSSQL备份还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (frmMssqlBack == null || frmMssqlBack.IsDisposed)//判断是否存在窗口
                {
                    frmMssqlBack = new frmMssqlBack();
                }

                frmMssqlBack.Show();
                frmMssqlBack.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 老版本数据导入ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frmImport == null || frmImport.IsDisposed)//判断是否存在窗口
            {
                frmImport = new frmImport();
            }
            frmImport.Show();
            frmImport.Activate();
        }

        //网站重建
        //private void btnRebuild_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("该功能适合系统重装后恢复数据库中的所有站点配置,确认？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {
        //        Application.DoEvents();
        //        string SiteID = "0";//站点标识符
        //        string s_SiteName = txtSiteName.Text.Trim();//站点名
        //        string s_SitePass = txtSitePassword.Text.Trim();//站点密码
        //        string SiteDir = txtSiteDir.Text.Trim();//站点路径
        //        string s_WebStatus = "0"; //WEB初始状态（正常 = 0 停止 = 1 需要跳转 = 2）
        //        string s_FtpStatus = "0"; //FTP初始状态（正常 = 0 停止 = 1）     
        //        string s_DiskQuota = "0"; //空间大小
        //        string s_IPAddress = "";
        //        string s_MaxBandwidth = "0";//带宽
        //        string s_MaxConnections = "0";//连接数据
        //        string s_DefaultDoc = "index.html,default.htm,default.html,index.asp,index.php,index.aspx,default.aspx";
        //        string s_IPSecurity = "";
        //        string s_UrlForward = "";
        //        string strApp = "";//应用程序池名
        //        string mysqldbname = "";//数据库名
        //        string mysqldbpassword = "";//数据库密码 
        //        if (chkAppPoolId.Checked)
        //        {
        //            strApp = s_SiteName;
        //        }
        //        else
        //        {
        //            strApp = cmbAppPoolId.Text;
        //        }
        //        s_DiskQuota = txtDiskQuota.Text.Trim();
        //        s_MaxBandwidth = txtMaxBandwidth.Text.Trim();
        //        s_MaxConnections = txtMaxConnections.Text.Trim();


        //        #region 数据验证
        //        if (s_SiteName == "")
        //        {
        //            MessageBox.Show("请输入FTP用户名");
        //            txtSiteName.Focus();
        //            return;
        //        }
        //        if (!txtIsMatch.SiteName(s_SiteName))
        //        {
        //            MessageBox.Show("您输入的FTP用户名无效，应为3-20位字母、数字、-_组合,-_不能出现在开头和结尾");
        //            txtSiteName.Focus();
        //            return;
        //        }
        //        SiteID = IISControl.GetSiteID(s_SiteName);
        //        if (SiteID != "0")
        //        {
        //            MessageBox.Show("站点:" + txtSiteName.Text + " 已存在");
        //            txtSiteName.Focus();
        //            return;
        //        }
        //        if (s_SitePass == "")
        //        {
        //            MessageBox.Show("请输入FTP密码");
        //            txtSitePassword.Focus();
        //            return;
        //        }
        //        if (!txtIsMatch.SitePassword(s_SitePass))
        //        {
        //            MessageBox.Show("您输入的FTP密码无效，长度为6-16位");
        //            txtSitePassword.Focus();
        //            return;
        //        }
        //        if (txtDefaultDoc.Text.Trim() != "")
        //        {
        //            s_DefaultDoc = txtDefaultDoc.Text.Trim().Replace("\r\n", ",");
        //        }
        //        else
        //        {
        //            MessageBox.Show("请输入默认文档!");
        //            txtDefaultDoc.Focus();
        //            return;
        //        }
        //        s_DefaultDoc = s_DefaultDoc.Trim(',');
        //        #endregion


        //        #region 脚本映射
        //        string Script = "";
        //        Script += asp + ";";
        //        Script += php + ";";
        //        Script += net2 + ";";

        //        if (Script == "")
        //        {
        //            Script = @".asp,c:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST;.asa,c:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST";
        //        }
        //        Script = Script.Substring(0, Script.Length - 1);
        //        string[] s_Scriptmap = System.Text.RegularExpressions.Regex.Split(Script, ";");
        //        for (int a = 0; a < s_Scriptmap.Length; a++)
        //        {
        //            Thread.Sleep(1);
        //            s_Scriptmap[a] = s_Scriptmap[a];
        //        }
        //        #endregion

        //        //拆分绑定域名

        //        #region
        //        string strBinding = txtBing.Text;
        //        if (strBinding.Trim() == "")
        //        {
        //            MessageBox.Show("请输入绑定域名");
        //            txtBing.Focus();
        //            return;
        //        }

        //        strBinding = ":80:" + strBinding;
        //        string[] s_Bindings = Regex.Split(strBinding.Replace(",", ",:80:"), ",", RegexOptions.IgnoreCase);
        //        #endregion
        //        string[] ipaddress;
        //        ipaddress = Regex.Split(s_IPAddress, ",", RegexOptions.IgnoreCase);
        //        object[] ipls = new object[ipaddress.Length];
        //        for (int c = 0; c < ipaddress.Length; c++)
        //        {
        //            Thread.Sleep(1);
        //            ipls[c] = ipaddress[c].Replace("/", ",");
        //        }
        //        string siteid = "";
        //        #region
        //        if (!Directory.Exists(SiteDir))
        //        {
        //            Directory.CreateDirectory(SiteDir);
        //        }
        //        string temp = "";
        //        if (IISControl.CreateWebSite(s_SiteName, s_SitePass, SiteDir, s_Bindings, s_MaxBandwidth, s_MaxConnections, s_DefaultDoc, s_Scriptmap, ipls, s_IPSecurity, s_WebStatus, s_UrlForward, strApp))
        //            if (IISControl.CreateFtpVDir(s_SiteName, SiteDir, s_FtpStatus))
        //                if (IISControl.CreateIISUser(s_SiteName, s_SitePass))
        //                    if (IISControl.CreateDiskQuotas(SiteDir.Substring(0, 3), s_SiteName, s_DiskQuota))
        //                    {
        //                        if (IISControl.addDirectorySecurity(SiteDir, s_SiteName))
        //                        {
        //                            //设置状态
        //                            IISControl.SetChown(s_SiteName, SiteDir);
        //                            siteid = IISControl.GetSiteID(s_SiteName);
        //                            if (s_WebStatus == "1")
        //                            {
        //                                IISControl.ChangeWebSiteStatus(siteid, s_WebStatus);
        //                            }
        //                            temp = "站点创建成功!";
        //                        }
        //                        else
        //                        {
        //                            temp = "站点创建成功!但站点权限应用失败!";
        //                        }
        //                        //创建数据库
        //                        ListSite();
        //                        if (chkAddMySQL.Checked)
        //                        {
        //                            bool creatmysql = false;
        //                            mysqldbname = txtMySQLDBName.Text.Trim();
        //                            mysqldbpassword = txtMySQLDBPassword.Text.Trim();
        //                            if (!txtIsMatch.SiteName(mysqldbname) && !txtIsMatch.SitePassword(mysqldbpassword))
        //                            {
        //                                temp += "您输入的MySQL数据库名或MySQL密码无效，应为字母、数字组合。";
        //                                creatmysql = false;
        //                            }
        //                            else
        //                            {
        //                                creatmysql = true;
        //                            }
        //                            if (creatmysql)
        //                            {
        //                                //建库
        //                                if (IISControl.CreateDataBases(mysqldbname, mysqldbname, mysqldbpassword))
        //                                {
        //                                    temp += "数据库创建成功!";
        //                                }
        //                                else
        //                                {
        //                                    temp += "数据库创建失败!";
        //                                    mysqldbname = "";
        //                                    mysqldbpassword = "";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                temp += "数据库创建失败!";
        //                                mysqldbname = "";
        //                                mysqldbpassword = "";
        //                            }
        //                        }
        //                        MessageBox.Show(temp);

        //                        //入库
        //                        OleDbConnection OleConn = ConnClass.DataConn();
        //                        OleDbCommand OleCmd = new OleDbCommand();
        //                        OleCmd.Connection = OleConn;
        //                        string strSQL = "";
        //                        try
        //                        {

        //                            bool ok = false;
        //                            OleCmd.CommandText = "select * from site where sitename = '" + s_SiteName + "'";
        //                            OleConn.Open();
        //                            OleDbDataReader OleReader = null;
        //                            OleReader = OleCmd.ExecuteReader();
        //                            if (OleReader.HasRows)
        //                            {
        //                                ok = true;
        //                            }
        //                            OleReader = null;
        //                            OleConn.Close();
        //                            OleConn.Open();
        //                            if (ok)
        //                            {
        //                                strSQL = "update site set siteid = '" + siteid + "'" +
        //                                        ",sitepassword = '" + s_SitePass + "'" +
        //                                        ",sitedir = '" + SiteDir + "'" +
        //                                        ",bindings = '" + txtBing.Text.Replace("\r\n", ",") + "'" +
        //                                        ",defaultdoc = '" + s_DefaultDoc + "'" +
        //                                        ",diskquota = '" + s_DiskQuota + "'" +
        //                                        ",maxbandwidth = '" + s_MaxBandwidth + "'" +
        //                                        ",maxconnections = '" + s_MaxConnections + "'" +
        //                                        ",mysqldbname = '" + mysqldbname + "'" +
        //                                        ",mysqldbuser = '" + mysqldbname + "'" +
        //                                        ",mysqldbpassword = '" + mysqldbpassword + "'";
        //                            }
        //                            else
        //                            {
        //                                strSQL = "insert into site(siteid,sitename,sitepassword,sitedir,bindings,defaultdoc,diskquota,maxbandwidth,maxconnections,mysqldbname,mysqldbuser,mysqldbpassword,remark) " +
        //                                         "values('" + siteid + "','" + s_SiteName + "','" + s_SitePass + "','" + SiteDir + "','" + txtBing.Text.Replace("\r\n", ",") + "','" + s_DefaultDoc + "','" + s_DiskQuota + "','" + s_MaxBandwidth + "', '" + s_MaxConnections + "','" + mysqldbname + "','" + mysqldbname + "','" + mysqldbpassword + "','')";
        //                            }
        //                            OleCmd.CommandText = strSQL;
        //                            if (!(OleCmd.ExecuteNonQuery() > 0))
        //                            {
        //                                //入库失败
        //                            } OleConn.Close();

        //                        }
        //                        catch
        //                        {
        //                            try
        //                            {
        //                                OleCmd = null;
        //                                OleConn.Close();
        //                                OleConn = null;
        //                            }
        //                            catch { }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        IISControl.RemoveDiskQuotas(SiteDir.Substring(0, 3), s_SiteName);
        //                        IISControl.DelIISUser(s_SiteName);
        //                        IISControl.DeleteFtpVDir(s_SiteName);
        //                        IISControl.DeleteWebSite(s_SiteName);
        //                        Directory.Delete(SiteDir, true);
        //                        MessageBox.Show("站点创建失败,设置磁盘配额失败！");
        //                        //Temp = "<return_code><![CDATA[433]]></return_code><return_msg><![CDATA[设置磁盘配额失败!]]></return_msg>";
        //                    }

        //                else
        //                {
        //                    IISControl.DelIISUser(s_SiteName);
        //                    IISControl.DeleteFtpVDir(s_SiteName);
        //                    IISControl.DeleteWebSite(s_SiteName);
        //                    MessageBox.Show("站点创建失败,创建IIS帐户失败！");
        //                    //Temp = "<return_code><![CDATA[433]]></return_code><return_msg><![CDATA[创建帐户失败!]]></return_msg>";
        //                }
        //            else
        //            {
        //                IISControl.DeleteFtpVDir(s_SiteName);
        //                IISControl.DeleteWebSite(s_SiteName);
        //                MessageBox.Show("站点创建失败,创建FTP失败！");
        //                //Temp = "<return_code><![CDATA[433]]></return_code><return_msg><![CDATA[创建ftp失败!]]></return_msg>";
        //            }
        //        else
        //        {
        //            IISControl.DeleteWebSite(s_SiteName);
        //            MessageBox.Show("站点创建失败！");
        //        }
        //        ListSite();
        //        #endregion

        //    }

        //}

    }
}
