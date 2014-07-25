using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.DirectoryServices;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.OleDb;
using Microsoft.Win32;

/********************************************************************************

** 创始时间：2010-03-19

** 修改时间：2012-04-09

** 描述：

**  主要用于自动安装

*********************************************************************************/

namespace 自动安装
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        BackgroundWorker Bw = new BackgroundWorker();
        public int all = 20;
        public int start = 1;
        private void btnSetup_Click(object sender, EventArgs e)
        {
            this.label3.Visible = true;
            this.progressBar1.Visible = true;

            #region 进度条控制
            if (chkPHP.Checked)
            {
                all += 10;
            }
            if (chkModule.Checked)
            {
                all += 3;
            }
            if (chkApp.Checked)
            {
                all += 2;
            }
            if (chkManage.Checked)
            {
                all += 2;
            }
            if (chkUrl.Checked)
            {
                all += 2;
            }
            int max = all;
            if (max < 0)
            {
                max = 10;
            }
            #endregion

            progressBar1.Maximum = max;
            this.btnSetup.Enabled = false;
            Bw.WorkerSupportsCancellation = true;
            Bw.WorkerReportsProgress = true;
            Bw.DoWork += new DoWorkEventHandler(Setup);//绑定事件
            Bw.ProgressChanged += new ProgressChangedEventHandler(Progress);//报告进度
            Bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(End);//结束事件
            Bw.RunWorkerAsync();
        }

        public void Progress(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            // (int)e.UserState;           
        }

        public void End(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = all;//进度条
        }

        /// <summary>
        /// 安装步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Setup(object sender, DoWorkEventArgs e)
        {
            start = 1;
            //保证安装的正确性
            string aa = "/s WSHom.Ocx;/s shell32.dll;/s wshext.dll;/s msjetoledb40.dll";
            string[] systemdll = aa.Split(';');
            foreach (string strDll in systemdll)
            {
                RegsvrDll(strDll);
                ReportProgress(false);
            }
            string strPath = @"C:\VhostManage";
            strPath = txtSetUpPath.Text.Trim();
            if (!Directory.Exists(strPath))
            {
                try
                {
                    Directory.CreateDirectory(strPath);
                }
                catch
                {
                    strPath = @"C:\VhostManage";
                }
            }
            ReportProgress(false);

            #region 服务器安全

            //服务器安全
            //if (chkSafe.Checked)
            //{

            //    SecurityBat(Application.StartupPath + "\\safe");
            //}

            #endregion

            ReportProgress(false);

            #region PHP,MySQL环境
            if (chkPHP.Checked)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Application.StartupPath + "\\php", strPath + "\\php", true);
                    ReportProgress(false);
                    //设置php
                    //添加everyone默认权限
                    addDirectorySecurity1(strPath + "\\php");
                    ReportProgress(false);

                }
                catch { }
                try
                {
                    //修改PHP配置文件
                    string strTxt = ReadTxt(strPath + "\\php\\php.ini", Encoding.Default);
                    strTxt = strTxt.Replace(@"C:\VhostManage", strPath);
                    WriteTxt(strPath + "\\php\\php.ini", strTxt, false, Encoding.Default);
                }
                catch { }
                ReportProgress(false);
                try
                {
                    RegistryKey regLocalMachine = Registry.LocalMachine;
                    RegistryKey regSYSTEM = regLocalMachine.OpenSubKey("SYSTEM", true);//打开HKEY_LOCAL_MACHINE下的SYSTEM
                    RegistryKey regControlSet001 = regSYSTEM.OpenSubKey("CurrentControlSet", true);//打开CurrentControlSet 
                    RegistryKey regControl = regControlSet001.OpenSubKey("Control", true);//打开Control
                    RegistryKey regManager = regControl.OpenSubKey("Session Manager", true);//打开Session Manager
                    RegistryKey regEnvironment = regManager.OpenSubKey("Environment", true);//打开Environment
                    string path = regEnvironment.GetValue("path").ToString();//读取path的值                
                    if (path.IndexOf(strPath + "\\php") == -1)
                    {
                        regEnvironment.SetValue("path", regEnvironment.GetValue("path").ToString() + ";" + strPath + "\\php");
                    }
                    ReportProgress(false);
                    regEnvironment.SetValue("PHPRC", strPath + "\\php");
                }
                catch { }
                ReportProgress(false);
                SetPHP(strPath);
                ReportProgress(false);

                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Application.StartupPath + "\\mysql", strPath + "\\mysql", true);
                }
                catch { }//安装MySQL    
                ReportProgress(false);
                SetUpMySQL(strPath + "\\mysql\\bin");
                ReportProgress(false);
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Application.StartupPath + "\\phpmyadmin", strPath + "\\phpmyadmin", true);
                }
                catch { }//创建phpmyadmin站点
                ReportProgress(false);
                string[] s_Bindings ={ ":80:127.0.0.1" };
                string s_SitePass = GetRandomPassword(12);
                string s_SiteName = "mysqlmanage";
                string s_WebStatus = "0"; //WEB初始状态（正常 = 0 停止 = 1 需要跳转 = 2）
                string s_IPAddress = "";
                string s_MaxBandwidth = "0";//带宽
                string s_MaxConnections = "0";//连接数据
                string s_DefaultDoc = "index.php,index.html,default.htm,default.html,index.asp,index.aspx,default.aspx";
                string s_IPSecurity = "";
                string s_UrlForward = "";
                string strApp = "";//应用程序池名
                string[] ipaddress;
                try
                {
                    ipaddress = Regex.Split(s_IPAddress, ",", RegexOptions.IgnoreCase);
                    object[] ipls = new object[ipaddress.Length];
                    for (int c = 0; c < ipaddress.Length; c++)
                    {
                        Thread.Sleep(1);
                        ipls[c] = ipaddress[c].Replace("/", ",");
                    }
                    string[] s_Scriptmap = System.Text.RegularExpressions.Regex.Split(".php," + strPath + @"\php\php5isapi.dll,5,GET,HEAD,POST", ";");

                    CreateWebSite(s_SiteName, s_SitePass, strPath + "\\phpmyadmin", s_Bindings, s_MaxBandwidth, s_MaxConnections, s_DefaultDoc, s_Scriptmap, ipls, s_IPSecurity, s_WebStatus, s_UrlForward, strApp);

                }
                catch { }
                ReportProgress(false);
                // CreateWebSite("phpmyadmin", s_SitePass, strPath + "\\phpmyadmin", s_Bindings, "0", "0", "index.php", System.Text.RegularExpressions.Regex.Split(".php," + strPath + @"\php5isapi.dll,5,GET,HEAD,POST;", ";"), "0", "DefaultAppPool");
                DelIISUser(s_SiteName);
                CreateIISUser(s_SiteName, s_SitePass);
                ReportProgress(false);
                addDirectorySecurity(strPath + "\\phpmyadmin", s_SiteName);
            }
            #endregion

            ReportProgress(false);
            //frmLoading.Close();

            #region 静态规则

            if (chkUrl.Checked)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Application.StartupPath + "\\rewrite", strPath + "\\rewrite", true);

                }
                catch
                { }
                ReportProgress(false);
                //静态规则
                //SetWriteURL(strPath);
                ReportProgress(false);
                UpdateDirectorySecurity(strPath);

                //MessageBox.Show("注意：请使用默认设置安装!", "提示");
                //Rewrite3();
                //ReportProgress(false);
                //MessageBox.Show("安装Rewrite完成后点确定!", "提示");
                //try
                //{
                //    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(Application.StartupPath + @"\rewrite\ISAPI_Rewrite.dll", "C:\\Program Files\\Helicon\\ISAPI_Rewrite3\\ISAPI_Rewrite.dll", true);
                //    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(Application.StartupPath + @"\rewrite\ISAPI_RewriteSnapin.dll", "C:\\Program Files\\Helicon\\ISAPI_Rewrite3\\ISAPI_RewriteSnapin.dll", true);
                //    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(Application.StartupPath + @"\rewrite\httpd.conf", "C:\\Program Files\\Helicon\\ISAPI_Rewrite3\\httpd.conf", true);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.ToString());
                //}
                //System.Threading.Thread.Sleep(50000);
                //显示说明
                //ViewTxt();

                ReportProgress(false);
            }
            #endregion

            //frmLoading.Close();
            ReportProgress(false);

            #region 常用组件

            if (chkModule.Checked)
            {
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Application.StartupPath + "\\dll", strPath + "\\dll", true);
                ReportProgress(false);
                //设置权限
                string tmpRar = "/s \"" + strPath + "\\dll\\AspJpeg.dll\";";
                tmpRar += "/s \"" + strPath + "\\dll\\jmail.dll\";";
                tmpRar += "/s \"" + strPath + "\\dll\\LyfUpload.dll\"";
                string[] dll = tmpRar.Split(';');
                foreach (string strDll in dll)
                {
                    RegsvrDll(strDll);
                    ReportProgress(false);
                }
                UpdateDirectorySecurity(strPath + "\\dll");
            }
            #endregion

            ReportProgress(false);

            #region 管理助手

            if (chkManage.Checked)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Application.StartupPath + "\\sitemanage", strPath + "\\", true);
                }
                catch { }
                ReportProgress(false);
                //创建快捷方式
                CreatLink(strPath);
                ReportProgress(false);
                OleDbConnection OleConn = DataConn(strPath);
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                string strSQL = "update vhostseting set " +
                                "php = '.php," + strPath + "\\php\\php5isapi.dll,5,GET,HEAD,POST' " +
                                "where type = 'vhost'";
                try
                {
                    OleCmd.CommandText = strSQL;
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
            #endregion

            ReportProgress(false);

            #region 常用软件
            if (chkApp.Checked)
            {
                SetUp360();
            }
            #endregion

            ReportProgress(false);
            SetUploadSize();

            //aa = "/u /s %windir%\\System32\\wshom.ocx;/u /s %windir%\\system32\\shell32.dll;/u /s %windir%\\system32\\wshext.dll";
            //string[] systemdll1 = aa.Split(';');
            //foreach (string strDll in systemdll1)
            //{
            //    RegsvrDll(strDll);
            //}
            //重启IIS
            ReportProgress(false);
            iisreset();
            ReportProgress(true);
            MessageBox.Show("安装成功!");
        }

        private void iisreset()
        {
            string s = "iisreset.exe";
            ExecuteCmd(s.Split(','));
        }

        private void ViewTxt()
        {
            string strPath = Application.StartupPath + "\\rewrite";
            string tmpRar = "安装说明.txt";
            String s = "cd " + strPath.Replace(" ", "\" \"");
            s += "," + strPath.Substring(0, 2);
            s += ",notepad.exe " + tmpRar;
            ExecuteCmd(s.Split(','));
        }

        private string Rewrite3()
        {
            string strPath = Application.StartupPath + "\\rewrite";
            String s = "cd " + strPath.Replace(" ", "\" \"");
            s += "," + strPath.Substring(0, 2);
            s += ",ISAPI_Rewrite3_0082.msi";
            return ExecuteCmd(s.Split(','));
        }


        /// <summary>
        /// 控制进度条
        /// </summary>
        /// <param name="end">是否结束</param>
        private void ReportProgress(bool end)
        {
            start++;
            if (end)
            {
                start = all;
            }
            else
            {
                if (start >= all)
                {
                    start--;
                }
            }
            Bw.ReportProgress(start);
        }


        private void DelIISUser(string Username)
        {
            try
            {
                DirectoryEntry obComputer = new DirectoryEntry("WinNT://" + Environment.MachineName);//获得计算机实例
                DirectoryEntry obUser = obComputer.Children.Find(Username, "User");//找到用户
                obComputer.Children.Remove(obUser);//删除用户
                obUser.Close();
                obComputer.Close();
            }
            catch
            {

            }
        }

        public static OleDbConnection DataConn(string strPath)
        {
            string strg = strPath + @"\DataBase";
            strg += @"\site.mdb";
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strg);
        }

        private void SetUploadSize()
        {
            try
            {
                string vbs = string.Empty;
                vbs = "Sub SetUploadSize()\r\n";
                vbs += "on error resume next\r\n";
                vbs += "Set objW=GetObject(\"IIS://localhost/w3svc\")\r\n";
                vbs += "objW.AspMaxRequestEntityAllowed=10485760\r\n";
                vbs += "objW.AspEnableParentPaths=True\r\n";
                vbs += "objW.setinfo\r\n";
                vbs += "Set objW=nothing\r\n";
                vbs += "end Sub\r\n";
                vbs += "sub openEnableApplication()\r\n";
                vbs += "Set ow3svc1=GetObject(\"IIS://localhost/w3svc\")\r\n";
                vbs += "ow3svc1.EnableApplication(\"Active Server Pages\")\r\n";
                vbs += "set ow3svc1=nothing\r\n";
                vbs += "end sub\r\n";
                vbs += "Call SetUploadSize()\r\n";
                vbs += "Call openEnableApplication()\r\n";
                string tempFile = System.Environment.CurrentDirectory + "\\UploadSize.vbs";
                FileStream fs = null;
                try
                {
                    fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                    //这里必须用UnicodeEncoding. 因为用UTF-8或ASCII会造成VBS乱码 
                    System.Text.UnicodeEncoding uni = new UnicodeEncoding();
                    byte[] b = uni.GetBytes(vbs);
                    fs.Write(b, 0, b.Length);
                    fs.Flush();
                    fs.Close();
                }
                catch
                {
                    //MessageBox.Show(ex.Message, "写入临时文件时出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //释放资源 
                    fs.Dispose();
                }
                string[] s_tempFile ={ tempFile.Replace(" ", "\" \"") };
                ExecuteCmd(s_tempFile);
                Thread.Sleep(3000);
                try
                {
                    File.Delete(tempFile);
                }
                catch { }
            }
            catch { }
        }

        private void SetPHP(string strPath)
        {
            string vbs = string.Empty;
            vbs = "Sub AddExt(PHPDIR)\r\n";
            vbs += "on error resume next\r\n";
            vbs += "Set ow3=GetObject(\"IIS://localhost/w3svc\")\r\n";
            vbs += "xlist=ow3.WebSvcExtRestrictionList\r\n";
            vbs += "x=\"1,\" & PHPDIR & \",1,PHP,php\"\r\n";
            vbs += "blAdd=true\r\n";
            vbs += "for each xitem in xlist\r\n";
            vbs += "if instr(xitem,x)>0 then\r\n";
            vbs += "blAdd=false\r\n";
            vbs += "Exit Sub\r\n";
            vbs += "end if\r\n";
            vbs += "next\r\n";
            vbs += "if blAdd then\r\n";
            vbs += "redim preserve xlist(Ubound(xlist)+1)\r\n";
            vbs += "xlist(Ubound(xlist))=x\r\n";
            vbs += "ow3.WebSvcExtRestrictionList=xlist\r\n";
            vbs += "ow3.setInfo\r\n";
            vbs += "end if\r\n";
            vbs += "end Sub\r\n";
            vbs += "Call AddExt(\"" + strPath + "\\php\\php5isapi.dll\")\r\n";

            string tempFile = System.Environment.CurrentDirectory + "\\php.vbs";
            FileStream fs = null;
            try
            {
                fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                //这里必须用UnicodeEncoding. 因为用UTF-8或ASCII会造成VBS乱码 
                System.Text.UnicodeEncoding uni = new UnicodeEncoding();
                byte[] b = uni.GetBytes(vbs);
                fs.Write(b, 0, b.Length);
                fs.Flush();
                fs.Close();
            }
            catch
            {
                //MessageBox.Show(ex.Message, "写入临时文件时出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //释放资源 
                try
                {
                    fs.Dispose();
                }
                catch { }
            }
            string[] s_tempFile ={ tempFile.Replace(" ", "\" \"") };
            ExecuteCmd(s_tempFile);
            Thread.Sleep(3000);
            try
            {
                File.Delete(tempFile);
            }
            catch { }

        }

        private void SetWriteURL(string strPath)
        {
            string vbs = string.Empty;
            vbs = "Sub SetURL()\r\n";
            vbs += "on error resume next\r\n";
            vbs += "sFilterName=\"urlrewrite\"\r\n";
            vbs += "sFilterPath=\"" + strPath + "\\rewrite\\Rewrite.dll\"\r\n";
            vbs += "Set web_server = GetObject(\"IIS://LocalHost/W3SVC\")\r\n";
            vbs += "Set objFilters = web_server.GetObject(\"IIsFilters\", \"Filters\")\r\n";
            vbs += "If IsNull(objFilters) Or IsEmpty(objFilters) Then Set objFilters = web_server.Create(\"IIsFilters\", \"Filters\")\r\n";
            vbs += "look_str=objFilters.FilterLoadOrder\r\n";
            vbs += "if instr(look_str,sFilterName)<=0 then\r\n";
            vbs += "Set objFilter = objFilters.GetObject(\"IIsFilter\",sFilterName)\r\n";
            vbs += "If IsNull(objFilter) Or IsEmpty(objFilter) Then\r\n";
            vbs += "Set objFilter = objFilters.Create(\"IIsFilter\",sFilterName)\r\n";
            vbs += "End If\r\n";
            vbs += "objFilter.FilterDescription = sFilterName\r\n";
            vbs += "objFilter.FilterPath = sFilterPath\r\n";
            vbs += "objFilter.FilterEnabled = True\r\n";
            vbs += "objFilter.SetInfo\r\n";
            vbs += "loadOrder=\"\"\r\n";
            vbs += "LoadOrder = objFilters.FilterLoadOrder\r\n";
            vbs += "If LoadOrder <> \"\" Then LoadOrder = LoadOrder & \",\"\r\n";
            vbs += "LoadOrder = LoadOrder & sFilterName\r\n";
            vbs += "objFilters.FilterLoadOrder = LoadOrder\r\n";
            vbs += "objFilters.SetInfo\r\n";
            vbs += "set	objFilter = Nothing\r\n";
            vbs += "Set objFilters = Nothing\r\n";
            vbs += "end if\r\n";
            vbs += "Set web_server = Nothing\r\n";
            vbs += "end sub\r\n";
            vbs += "Call SetURL()\r\n";
            string tempFile = System.Environment.CurrentDirectory + "\\rewrite.vbs";
            FileStream fs = null;
            try
            {
                fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                //这里必须用UnicodeEncoding. 因为用UTF-8或ASCII会造成VBS乱码 
                System.Text.UnicodeEncoding uni = new UnicodeEncoding();
                byte[] b = uni.GetBytes(vbs);
                fs.Write(b, 0, b.Length);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "写入临时文件时出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //释放资源 
                try
                {
                    fs.Dispose();
                }
                catch { }
            }

            string[] s_tempFile ={ tempFile.Replace(" ", "\" \"") };
            ExecuteCmd(s_tempFile);
            Thread.Sleep(3000);
            try
            {
                File.Delete(tempFile);
            }
            catch { }
        }
        //创建快捷方式
        private void CreatLink(string strPath)
        {
            string vbs = string.Empty;
            vbs += ("set WshShell = WScript.CreateObject(\"WScript.Shell\")\r\n");
            vbs += ("strDesktop = WshShell.SpecialFolders(\"Desktop\")\r\n");
            vbs += ("set oShellLink = WshShell.CreateShortcut(strDesktop & \"\\网站管理助手.lnk\")\r\n");
            vbs += ("oShellLink.TargetPath = \"" + strPath + "\\网站管理助手.exe\"\r\n");
            vbs += ("oShellLink.WindowStyle = 1\r\n");
            vbs += ("oShellLink.Description = \"世纪东方:Deavel King\"\r\n");
            vbs += ("oShellLink.WorkingDirectory = \"" + System.Environment.CurrentDirectory + "\"\r\n");
            vbs += ("oShellLink.Save");
            string tempFile = System.Environment.CurrentDirectory + "\\temp.vbs";
            FileStream fs = null;
            try
            {
                fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                //这里必须用UnicodeEncoding. 因为用UTF-8或ASCII会造成VBS乱码 
                System.Text.UnicodeEncoding uni = new UnicodeEncoding();
                byte[] b = uni.GetBytes(vbs);
                fs.Write(b, 0, b.Length);
                fs.Flush();
                fs.Close();
            }
            catch
            {
                try
                {
                    //释放资源 
                    fs.Dispose();
                }
                catch { }
                //MessageBox.Show(ex.Message, "写入临时文件时出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    //释放资源 
                    fs.Dispose();
                }
                catch { }
            }
            string[] s_tempFile ={ tempFile.Replace(" ", "\" \"") };
            ExecuteCmd(s_tempFile);
            Thread.Sleep(3000);
            try
            {
                File.Delete(tempFile);
            }
            catch { }

        }

        private void SecurityBat(string strPath)
        {
            String s = "cd " + strPath.Replace(" ", "\" \"");
            s += "," + strPath.Substring(0, 2);
            s += ",safe.bat";
            ExecuteCmd(s.Split(','));
        }

        //dos命令执行
        public static string ExecuteCmd(string[] cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;
            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i].Trim().Length > 0)
                {
                    p.StandardInput.WriteLine(cmd[i].ToString());
                }
            }
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }

        //安装mysql
        private void SetUpMySQL(string MySQLPath)
        {
            string tmpRar = "/install";
            String s = "cd " + MySQLPath.Replace(" ", "\" \"");
            s += "," + MySQLPath.Substring(0, 2);
            s += ",mysqld-nt.exe " + tmpRar;
            ExecuteCmd(s.Split(','));
            s = "net.exe start mysql";
            ExecuteCmd(s.Split(','));


        }
        //安装360
        private void SetUp360()
        {
            string strPath = Application.StartupPath + "\\safe";
            String s = "cd " + strPath.Replace(" ", "\" \"");
            s += "," + strPath.Substring(0, 2);
            s += ",safe360.exe";
            ExecuteCmd(s.Split(','));
        }
        //注册组件
        private void RegsvrDll(string tmpRar)
        {
            //tmpRar = tmpRar.Replace(" ", "\" \"");
            string s = "regsvr32.exe " + tmpRar;
            ExecuteCmd(s.Split(','));
        }

        //更新权限
        public static bool UpdateDirectorySecurity(string PathDir)
        {
            try
            {
                string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F Everyone:R";
                Process Process = new System.Diagnostics.Process();              //声明一个程序类，启动外部程序压缩站点
                Process.StartInfo.WorkingDirectory = Application.StartupPath + "\\sitemanage";
                Process.StartInfo.FileName = "cacls.exe";                          //设置程式名            
                Process.StartInfo.Arguments = tmpRar;                            //设定执行参数
                Process.StartInfo.UseShellExecute = false;                       //关闭Shell的使用                      
                Process.StartInfo.CreateNoWindow = true;
                Process.StartInfo.RedirectStandardInput = true;
                Process.Start();
                Process.StandardInput.WriteLine("y");
                Process.StandardInput.Flush();
                Process.WaitForExit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //网站权限
        public static bool addDirectorySecurity(string PathDir, string SiteName)
        {
            try
            {
                string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F " + SiteName + ":F";
                Process Process = new System.Diagnostics.Process();              //声明一个程序类，启动外部程序压缩站点
                Process.StartInfo.WorkingDirectory = Application.StartupPath + "\\sitemanage";
                Process.StartInfo.FileName = "cacls.exe";                        //设置程式名            
                Process.StartInfo.Arguments = tmpRar;                            //设定执行参数
                Process.StartInfo.UseShellExecute = false;                       //关闭Shell的使用                      
                Process.StartInfo.CreateNoWindow = true;
                Process.StartInfo.RedirectStandardInput = true;
                Process.Start();
                Process.StandardInput.WriteLine("y");
                Process.StandardInput.Flush();
                Process.WaitForExit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //网站权限
        public static bool addDirectorySecurity1(string PathDir)
        {
            try
            {
                string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F Everyone:R";
                Process Process = new System.Diagnostics.Process();              //声明一个程序类，启动外部程序压缩站点
                Process.StartInfo.WorkingDirectory = Application.StartupPath + "\\sitemanage";
                Process.StartInfo.FileName = "cacls.exe";                        //设置程式名            
                Process.StartInfo.Arguments = tmpRar;                            //设定执行参数
                Process.StartInfo.UseShellExecute = false;                       //关闭Shell的使用                      
                Process.StartInfo.CreateNoWindow = true;
                Process.StartInfo.RedirectStandardInput = true;
                Process.Start();
                Process.StandardInput.WriteLine("y");
                Process.StandardInput.Flush();
                Process.WaitForExit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool CreateIISUser(string Username, string Userpassword)
        {
            try
            {
                DirectoryEntry AD = new DirectoryEntry("WinNT://" + Environment.MachineName);
                DirectoryEntry NewUser = AD.Children.Add(Username, "user");
                NewUser.Invoke("SetPassword", new object[] { Userpassword });
                const int ADS_UF_DEFAULT = 0x0201;                              //默认
                //const int ADS_UF_SCRIPT = 0x0001;                             //将执行登录脚本
                //const int ADS_UF_ACCOUNTDISABLE = 0x0002;               		//禁用帐户
                //const int ADS_UF_HOMEDIR_REQUIRED = 0x0008;	                //帐户需要主目录
                //const int ADS_UF_LOCKOUT = 0x0010;		                    //锁定帐户
                //const int ADS_UF_PASSWD_NOTREQD = 0x0020;	                    //帐户不需要密码
                const int ADS_UF_PASSWD_CANT_CHANGE = 0x0040;	                //用户不能更改密码
                //const int ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x0080;	//允许加密文本密码
                const int ADS_UF_DONT_EXPIRE_PASSWD = 0x10000;	                //帐户密码永不过期
                //const int ADS_UF_SMARTCARD_REQUIRED = 0x40000;	            //登录需要使用智能卡
                //const int ADS_UF_PASSWORD_EXPIRED = 0x800000;		            //密码已过期
                NewUser.Properties["UserFlags"].Value = ADS_UF_DEFAULT + ADS_UF_PASSWD_CANT_CHANGE + ADS_UF_DONT_EXPIRE_PASSWD;//设置用户不能更改密码/帐户密码永不过期
                NewUser.Invoke("Put", new object[] { "Description", "由主机管理助手创建" });
                NewUser.CommitChanges();
                DirectoryEntry grp;
                grp = AD.Children.Find("Users", "group");
                if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                NewUser.Close();
                AD.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateWebSite(string WebSiteName, string Password, string PathToRoot, string[] BindString, string MaxBandwidth, string MaxConnections, string DefaultDoc, string[] ScriptMapsLst, object[] IPList, string Ipsecurity, string WebStatus, string HttpRedirect, string strApp)
        {
            if (MaxBandwidth == "0")
                MaxBandwidth = "&HFFFFFFFF";
            else
            {
                try
                {
                    MaxBandwidth = (1024 * Convert.ToDouble(MaxBandwidth)).ToString();
                }
                catch
                {
                    MaxBandwidth = "&HFFFFFFFF";
                }
            }
            if (MaxConnections == "0")
                MaxConnections = "&HFFFFFFFF";
            bool WebState = true;
            if (WebStatus == "0")
                WebState = true;
            if (WebStatus == "1")
                WebState = false;
            int i;
            int ID = 1;
            string test_return;
            string siteID;
            Random ran = new Random();
            for (i = 1; i <= 10; i++)
            {
                System.Threading.Thread.Sleep(1);
                if (i == 10)
                {
                    return false;
                }
                ID = ran.Next(10000000, 99999999);//生成网站标识符
                try
                {
                    DirectoryEntry root1 = new DirectoryEntry("IIS://localhost/W3SVC/" + ID.ToString());
                    test_return = root1.Name.ToString();
                    root1.Close();
                }
                catch
                {
                    break;
                }
            }
            siteID = ID.ToString();
            DirectoryEntry root = new DirectoryEntry("IIS://localhost/W3SVC");
            try
            {

                string testappPoolRoot = "";
                //创建             
                try
                {
                    DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/W3SVC/AppPools/" + strApp);
                    testappPoolRoot = appPoolRoot.Name;
                }
                catch
                {

                    if (!CreateAppPool(strApp))
                        strApp = "DefaultAppPool";
                }

                DirectoryEntry site = (DirectoryEntry)root.Invoke("Create", "IIsWebServer", siteID);
                site.Properties["ServerComment"].Value = WebSiteName;
                site.Properties["KeyType"].Value = "IIsWebServer";
                site.Properties["ServerBindings"].Value = BindString;       //域名绑定
                site.Properties["ScriptMaps"].Value = ScriptMapsLst;        //脚本映射  
                site.Properties["ServerState"].Value = 2;                   //站点状态
                site.Properties["DefaultDoc"].Value = DefaultDoc;           //默认文挡设置
                site.Properties["AnonymousUserName"].Value = WebSiteName;
                site.Properties["AnonymousUserPass"].Value = Password;
                site.Properties["AspEnableParentPaths"].Value = true;       //父路径
                site.Properties["ServerAutoStart"].Value = WebState;        //运行状态
                site.Properties["ServerSize"].Value = 1;
                site.Properties["MaxBandwidth"].Value = MaxBandwidth;       //网络带宽
                site.Properties["MaxConnections"].Value = MaxConnections;   //最大连接数
                site.Properties["AppPoolId"].Value = strApp;                //指定应用程序池

                //创建应用程序的虚拟目录
                DirectoryEntry siteVDir = site.Children.Add("Root", "IISWebVirtualDir");
                siteVDir.Properties["Path"][0] = PathToRoot;
                siteVDir.Properties["AppFriendlyName"][0] = WebSiteName;
                siteVDir.Properties["AppIsolated"][0] = 2;
                siteVDir.Properties["AccessScript"][0] = true;
                siteVDir.Properties["AccessFlags"][0] = 513;
                siteVDir.Invoke("AppCreate", true);
                siteVDir.CommitChanges();
                site.CommitChanges();
                siteVDir.Close();
                site.Close();
                GC.Collect();
                return true;
            }
            catch
            {
                //GC.Collect();
                return false;
            }
        }

        public static bool CreateWebSite1(string WebSiteName, string Password, string PathToRoot, string[] BindString, string MaxBandwidth, string MaxConnections, string DefaultDoc, string[] ScriptMapsLst, string WebStatus, string strApp)
        {
            if (MaxBandwidth == "0")
                MaxBandwidth = "&HFFFFFFFF";
            if (MaxConnections == "0")
                MaxConnections = "&HFFFFFFFF";
            bool WebState = true;
            if (WebStatus == "0")
                WebState = true;
            if (WebStatus == "1")
                WebState = false;
            int i;
            int ID = 1;
            string test_return;
            string siteID;
            Random ran = new Random();
            for (i = 1; i <= 10; i++)
            {
                System.Threading.Thread.Sleep(1);
                if (i == 10)
                {
                    return false;
                }
                ID = ran.Next(10000000, 99999999);//生成网站标识符
                try
                {
                    DirectoryEntry root1 = new DirectoryEntry("IIS://localhost/W3SVC/" + ID.ToString());
                    test_return = root1.Name.ToString();
                    root1.Close();
                }
                catch
                {
                    break;
                }
            }
            siteID = ID.ToString();
            DirectoryEntry root = new DirectoryEntry("IIS://localhost/W3SVC");
            try
            {

                string testappPoolRoot = "";
                //创建             
                try
                {
                    DirectoryEntry appPoolRoot = new DirectoryEntry(@"IIS://localhost/W3SVC/AppPools/" + strApp);
                    testappPoolRoot = appPoolRoot.Name;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    if (!CreateAppPool(strApp))
                        strApp = "DefaultAppPool";
                }


                DirectoryEntry site = (DirectoryEntry)root.Invoke("Create", "IIsWebServer", siteID);
                site.Properties["ServerComment"].Value = WebSiteName;
                site.Properties["KeyType"].Value = "IIsWebServer";
                site.Properties["ServerBindings"].Value = BindString;       //域名绑定
                site.Properties["ScriptMaps"].Value = ScriptMapsLst;        //脚本映射  
                site.Properties["ServerState"].Value = 2;                   //站点状态
                site.Properties["DefaultDoc"].Value = DefaultDoc;           //默认文挡设置
                site.Properties["AnonymousUserName"].Value = WebSiteName;
                site.Properties["AnonymousUserPass"].Value = Password;
                //site.Invoke("Put", "AuthAnonymous", true);
                //site.Invoke("Put", "AnonymousPasswordSync", true);
                site.Properties["AspEnableParentPaths"].Value = true;       //父路径
                site.Properties["ServerAutoStart"].Value = WebState;        //运行状态
                site.Properties["ServerSize"].Value = 1;
                site.Properties["MaxBandwidth"].Value = MaxBandwidth;       //网络带宽
                site.Properties["MaxConnections"].Value = MaxConnections;   //最大连接数            
                //site.Invoke("Put", "MimeMap",);映射
                //RedirectHeaders 属性指定了其他重定向头
                site.Properties["AppPoolId"].Value = strApp;                //指定应用程序池

                //创建应用程序的虚拟目录
                DirectoryEntry siteVDir = site.Children.Add("Root", "IISWebVirtualDir");
                siteVDir.Properties["Path"][0] = PathToRoot;
                siteVDir.Properties["AppFriendlyName"][0] = WebSiteName;
                siteVDir.Properties["AppIsolated"][0] = 2;
                siteVDir.Properties["AccessScript"][0] = true;
                siteVDir.Properties["AccessFlags"][0] = 513;
                siteVDir.Invoke("AppCreate", true);
                siteVDir.CommitChanges();
                site.CommitChanges();
                siteVDir.Close();
                site.Close();
                GC.Collect();
                return true;
            }
            catch
            {
                GC.Collect();
                return false;
            }
        }

        private static bool CreateAppPool(string appPoolName)
        {
            try
            {
                DirectoryEntry newpool;
                DirectoryEntry apppools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");
                newpool.Properties["MaxProcesses"].Value = 1;//Web 园
                newpool.Properties["PeriodicRestartPrivateMemory"].Value = 102400;//回收工作进程
                //newpool.Properties["PeriodicRestartRequests"].Value = 35000;//回收工作进程请求数  
                newpool.Properties["PeriodicRestartTime"].Value = 20;
                //newpool.Properties["RapidFailProtection"].Value = false;
                newpool.Properties["ShutdownTimeLimit"].Value = 10;
                newpool.Properties["StartupTimeLimit"].Value = 10;
                newpool.CommitChanges();
                newpool.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetRandomPassword(int passwordLen)
        {
            string randomChars = "abcdefghijklmnopqrstuvwxyzABCDEMGHIJKLMNOPQRSTUVWXYZ0123456789!@#%^&*()";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password + "1aS!";
        }

        /// <summary>
        /// 读文本文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="encod">编码</param>
        /// <returns>内容</returns>
        public static string ReadTxt(string strPath, Encoding encoding)
        {
            string strTxt = "";
            try
            {
                StreamReader sr = new StreamReader(strPath, encoding);
                strTxt = sr.ReadToEnd();
                sr.Close();
            }
            catch { }
            return strTxt;
        }
        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="theStr">内容</param>
        /// <param name="append">是否追加</param>
        /// <param name="encod">编码</param>
        public static void WriteTxt(string strPath, string theStr, bool append, Encoding encoding)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strPath, append, encoding);
                sw.WriteLine(theStr);
                sw.Flush();
                sw.Close();
            }
            catch { }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static frmRewrite frmRewrite = new frmRewrite();
        private void lnk_pz_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (frmRewrite == null || frmRewrite.IsDisposed)//判断是否存在窗口
            {
                frmRewrite = new frmRewrite();
            }
            frmRewrite.Show();
            frmRewrite.Activate();

        }

    }
}
