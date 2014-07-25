using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using System.Configuration;
using System.Reflection;
using DiskQuotaTypeLibrary;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**    主要用于对IIS6.0的控制

*********************************************************************************/


namespace VhostManage
{
    public class IISControl
    {
        #region 创建站点
        /// <summary>
        /// 创建站点
        /// </summary>
        /// <param name="WebSiteName">站点名称</param>
        /// <param name="Password">FTP密码</param>
        /// <param name="PathToRoot">物理地址</param>
        /// <param name="BindString"></param>
        /// <param name="MaxBandwidth">网络带宽</param>
        /// <param name="MaxConnections">最大连接数</param>
        /// <param name="DefaultDoc">默认文档</param>
        /// <param name="ScriptMapsLst">脚本映射</param>
        /// <param name="IPList">ip列表</param>
        /// <param name="Ipsecurity">ip策略</param>
        /// <param name="WebStatus">站点状态</param>
        /// <param name="HttpRedirect">重定向到URL</param>
        /// <returns>是否成功</returns>   
        public static bool CreateWebSite(string WebSiteName, string Password, string PathToRoot, string[] BindString, string MaxBandwidth, string MaxConnections, string DefaultDoc, string[] ScriptMapsLst, object[] IPList, string Ipsecurity, string WebStatus, string HttpRedirect, string strApp)
        {
            if (GetSiteID(WebSiteName) != "0")
            {
                return false;
            }
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
                bool IPDefault;
                if (IPList[0].ToString() != "")
                {
                    if (Ipsecurity == "allow")
                    {
                        IPDefault = true;
                    }
                    else
                    {
                        IPDefault = false;
                    }

                    DirectoryEntry root2 = new DirectoryEntry("IIS://localhost/W3SVC/" + siteID + "/root");
                    Type typ = root2.Properties["IPSecurity"].Value.GetType();
                    object IPSecurity = root2.Properties["IPSecurity"].Value;
                    // 获取默认行为
                    bool bGrantByDefault = (bool)typ.InvokeMember("GrantByDefault", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);
                    // 设置默认行为
                    typ.InvokeMember("GrantByDefault", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { IPDefault });
                    if (IPDefault)
                    {
                        typ.InvokeMember("IPDeny", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { IPList });
                    }
                    else
                    {
                        typ.InvokeMember("IPGrant", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, IPSecurity, new object[] { IPList });
                    }
                    root2.Properties["IPSecurity"].Value = IPSecurity;
                    root2.Invoke("SetInfo");
                    root2.CommitChanges();
                    root2.Close();
                    if (WebStatus == "2")
                    {
                        DirectoryEntry root3 = new DirectoryEntry("IIS://localhost/W3SVC/" + siteID + "/root");
                        root3.Properties["HttpRedirect"].Value = HttpRedirect + ",EXACT_DESTINATION, PERMANENT";
                        root3.Invoke("SetInfo");
                        root3.CommitChanges();
                        root3.Close();
                    }

                }
                GC.Collect();
                return true;
            }
            catch
            {
                GC.Collect();
                return false;
            }
        }

        #endregion

        #region 创建Ftp虚拟目录
        /// <summary>
        /// 创建Ftp虚拟目录
        /// </summary>
        /// <param name="webSiteName">站点名</param>
        /// <param name="VDir">路径</param>
        /// <param name="status">站点状态（正常 = 0 停止 = 1）</param>
        /// <returns></returns>

        public static bool CreateFtpVDir(string webSiteName, string VDir, string Status)
        {
            bool strStatus = false;
            if (Status == "0")
                strStatus = true;
            if (Status == "1")
                strStatus = false;

            try
            {
                DirectoryEntry root = new DirectoryEntry("IIS://localhost/MSFTPSVC/1/root");
                DirectoryEntry site = (DirectoryEntry)root.Invoke("Create", "IIsFtpVirtualDir", webSiteName);
                site.Invoke("Put", "Path", VDir);
                site.Properties["AccessRead"][0] = strStatus;//读权限
                site.Properties["AccessWrite"][0] = strStatus;//写权限
                site.Properties["DontLog"][0] = false;//true不记录日志
                //foreach (DirectoryEntry e in site.InvokeGet("IPSecurity"))
                //{
                //   Console.WriteLine(site.InvokeGet("IPSecurity"));
                //}
                //site.Properties["IPSECLIST"].Value = "127.0.0.1";
                //Console.WriteLine("  "+site.Properties["IPSECLIST"]);
                site.Invoke("SetInfo");
                site.CommitChanges();
                root.Close();
                site.Close();
                return true;//创建成功
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                //ftp已存在
                return false;
            }

        }
        #endregion

        #region 创建IIS系统帐号
        /// <summary>
        /// 创建IIS系统帐号
        /// </summary>
        /// <param name="Username">帐户名</param>
        /// <param name="Userpassword">密码</param>
        /// <returns>成功/失败</returns>
        public static bool CreateIISUser(string Username, string Userpassword)
        {
            try
            {
                DirectoryEntry AD = new DirectoryEntry("WinNT://" + Environment.MachineName);
                //DirectoryEntry NewUser = null;
                //try
                //{
                //    NewUser = AD.Children.Find(Username);
                //}
                //catch
                //{
                //    try
                //    {
                //        //MessageBox.Show(ex.ToString());
                //        NewUser.Children.Add(Username, "user");
                //    }
                //    catch { }
                //}
                //NewUser.Invoke("SetPassword", new object[] { Userpassword });

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
                NewUser.Invoke("Put", new object[] { "Description", "由主机管理帮手创建" });
                NewUser.CommitChanges();
                DirectoryEntry grp;
                grp = AD.Children.Find("Users", "group");
                if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                NewUser.Close();
                AD.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        #endregion

        #region 添加用户磁盘配额
        /// <summary>
        /// 添加用户磁盘配额
        /// </summary>
        /// <param name="Path">磁盘盘符</param>
        /// <param name="UsersName">站点名</param>
        /// <param name="DiskQuota"></param>
        /// <returns>成功/失败</returns>
        public static bool CreateDiskQuotas(string Path, string UsersName, string DiskQuota)
        {
            double strLimit;
            double strThreshold;
            if (DiskQuota == "0")
            {
                strLimit = 1048576 * Convert.ToDouble(10485760);
                strThreshold = 1048576 * Convert.ToDouble(10485760);
            }
            else
            {
                strLimit = 1048576 * Convert.ToDouble(DiskQuota.Trim());
                strThreshold = 0.8 * 1048576 * Convert.ToDouble(DiskQuota);
            }
            try
            {
                DiskQuotaControlClass diskQuotaControl = new DiskQuotaControlClass();
                diskQuotaControl.Initialize(Path, true);
                DIDiskQuotaUser dskuser = null;
                dskuser = diskQuotaControl.AddUser(System.Net.Dns.GetHostName() + "\\" + UsersName);
                dskuser.QuotaLimit = strLimit;
                dskuser.QuotaThreshold = strThreshold;
                //dskuser = diskQuotaControl.FindUser(UsersName);
                //Console.WriteLine("用户名:" + dskuser.LogonName);
                //Console.WriteLine("总大小" + dskuser.QuotaLimitText);
                //Console.WriteLine("警告等级" + dskuser.QuotaThresholdText);
                //Console.WriteLine("状态" + dskuser.AccountStatus);
                //Console.WriteLine("已经使用" + dskuser.QuotaUsedText);            
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region 更新磁盘配额
        /// <summary>
        /// 更新磁盘配额
        /// </summary>
        /// <param name="Path">磁盘盘符</param>
        /// <param name="UsersName">站点名</param>
        /// <param name="DiskQuota">配额大小</param>
        /// <returns>成功/失败</returns>
        public static bool UpdateDiskQuotas(string Path, string UsersName, string DiskQuota)
        {
            Application.DoEvents();
            double strLimit;
            double strThreshold;
            if (DiskQuota == "0")
            {
                strLimit = 1048576 * Convert.ToDouble(10485760);
                strThreshold = 1048576 * Convert.ToDouble(10485760);
            }
            else
            {
                strLimit = 1048576 * Convert.ToDouble(DiskQuota.Trim());
                strThreshold = 0.8 * 1048576 * Convert.ToDouble(DiskQuota);
            }
            try
            {
                DiskQuotaControlClass diskQuotaControl = new DiskQuotaControlClass();
                diskQuotaControl.Initialize(Path, true);
                DIDiskQuotaUser dskuser = null;
                dskuser = diskQuotaControl.FindUser(System.Net.Dns.GetHostName() + "\\" + UsersName);
                dskuser.QuotaLimit = strLimit;
                dskuser.QuotaThreshold = strThreshold;
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region 更新域名绑定
        /// <summary>
        /// 更新域名绑定
        /// </summary>
        /// <param name="Siteid">站点标识符</param>
        /// <param name="BindString">绑定字符串</param>
        /// <returns>成功/失败</returns>
        public static bool UpdateBindString(string Siteid, string[] BindString)
        {
            if (Siteid == "0")
                return false;
            try
            {
                DirectoryEntry site = new DirectoryEntry("IIS://localhost/W3SVC/" + Siteid);
                site.Properties["ServerBindings"].Value = BindString;
                site.CommitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 设置所有者
        /// <summary>
        /// 设置所有者
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="SitePath">路径d:\wwwroot\test</param>
        /// <returns>成功/失败</returns>
        public static bool SetChown(string UserName, string SitePath)
        { //System.Diagnostics.Process Proc = System.Diagnostics.Process.Start("chown.exe", @"chown.exe -r -q vtest4 d:\wwwroot\vtest3\*");
            string TmpChown = " -r -q " + UserName + " " + SitePath + "\\*";
            try
            {
                System.Diagnostics.Process Process = new System.Diagnostics.Process();
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
                Process.StartInfo.FileName = "chown.exe";           //设置程式名            
                Process.StartInfo.Arguments = TmpChown; //设定执行参数
                Process.StartInfo.UseShellExecute = false;   //关闭Shell的使用                      
                Process.StartInfo.CreateNoWindow = true;
                Process.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetChown1(string UserName, string SitePath)
        { //System.Diagnostics.Process Proc = System.Diagnostics.Process.Start("chown.exe", @"chown.exe -r -q vtest4 d:\wwwroot\vtest3\*");
            string TmpChown = " -r -q " + UserName + " " + SitePath + "\\*";
            try
            {
                System.Diagnostics.Process Process = new System.Diagnostics.Process();
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
                Process.StartInfo.FileName = "chown.exe";           //设置程式名            
                Process.StartInfo.Arguments = TmpChown; //设定执行参数
                Process.StartInfo.UseShellExecute = false;   //关闭Shell的使用                      
                Process.StartInfo.CreateNoWindow = true;
                Process.Start();
                Process.WaitForExit();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获得站点标识符
        /// <summary>
        ///  获得站点标识符
        /// </summary>
        /// <param name="WebSiteName">站点名</param>
        /// <returns>站点标识符</returns>
        public static string GetSiteID(string WebSiteName)
        {
            Application.DoEvents();
            string SiteID = "0";
            DirectoryEntry root = new DirectoryEntry("IIS://localhost/W3SVC");
            try
            {
                // string hostname;
                foreach (DirectoryEntry bb in root.Children)
                {
                    if (bb.SchemaClassName == "IIsWebServer")
                    {
                        if (WebSiteName == bb.Properties["ServerComment"].Value.ToString())
                        {
                            SiteID = bb.Name;
                            break;
                        }
                    }
                }

            }
            catch
            {
                SiteID = "0";
            }
            return SiteID;

        }
        #endregion

        #region 添加目录权限
        /// <summary>
        /// 设置目录权限
        /// </summary>
        /// <param name="PathDir">路径</param>
        /// <param name="SiteName">站点名</param>
        /// <returns></returns>
        public static bool addDirectorySecurity(string PathDir, string SiteName)
        {
            try
            {
                //string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F " + SiteName + ":F";
                string tmpRar = string.Format("echo Y|cacls.exe \"{0}\" /C /T /G \"Administrators\":F \"SYSTEM\":F;icacls.exe \"{0}\" /grant iis_wpg:(oI)(ci)(R) {1}:(oi)(CI)(R);icacls.exe {0} /deny {1}:(oi)(ci)(wa,wea);icacls.exe \"{0}\" /grant {1}:(oi)(CI)(R,w,D)", PathDir, SiteName);
                ExecuteCmd(tmpRar.Split(';'));
                //Process Process = new System.Diagnostics.Process();              //声明一个程序类，启动外部程序压缩站点
                //Process.StartInfo.WorkingDirectory = Application.StartupPath;
                //Process.StartInfo.FileName = "cacls.exe";                        //设置程式名   
                //Process.StartInfo.FileName = "cmd.exe";
                //Process.StartInfo.Arguments = tmpRar;                            //设定执行参数
                //Process.StartInfo.UseShellExecute = false;                       //关闭Shell的使用                      
                //Process.StartInfo.CreateNoWindow = true;
                //Process.StartInfo.RedirectStandardInput = true;
                //Process.Start();
                //Process.StandardInput.WriteLine("y");
                //Process.StandardInput.Flush();
                //Process.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
            //try
            //{
            //    bool ok;
            //    DirectoryInfo dInfo = new DirectoryInfo(PathDir);
            //    DirectorySecurity dSecurity = dInfo.GetAccessControl();
            //    InheritanceFlags iFlags = new InheritanceFlags();
            //    iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            //    FileSystemAccessRule AccessRule2 = new FileSystemAccessRule(SiteName, FileSystemRights.FullControl, iFlags, PropagationFlags.None, AccessControlType.Allow);
            //    dSecurity.ModifyAccessRule(AccessControlModification.RemoveAll, AccessRule2, out ok);
            //    dSecurity.ModifyAccessRule(AccessControlModification.Add, AccessRule2, out ok);
            //    dInfo.SetAccessControl(dSecurity);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        //数据库帐户
        public static bool addDBFtpDirectorySecurity(string DBPathDir, string DBName)
        {
            try
            {
                string SQLFtpUser = ConfigurationManager.AppSettings["SQLFtpUser"].Trim();
                if (SQLFtpUser != "")
                {
                    SQLFtpUser = " " + SQLFtpUser + ":F";
                }

                string tmpRar = "\"" + DBPathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F " + DBName + ":F" + SQLFtpUser;
                Process Process = new System.Diagnostics.Process();              //声明一个程序类，启动外部程序压缩站点
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
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
        #endregion

        #region 删除目录权限
        /// <summary>
        /// 设置目录权限
        /// </summary>
        /// <param name="PathDir">路径</param>
        /// <param name="SiteName">站点名</param>
        /// <returns></returns>
        public static bool DelDirectorySecurity(string PathDir, string SiteName)
        {
            try
            {
                string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F";
                Process Process = new System.Diagnostics.Process();              //声明一个程序类，启动外部程序压缩站点
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
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
            //try
            //{
            //    bool ok;
            //    DirectoryInfo dInfo = new DirectoryInfo(PathDir);
            //    DirectorySecurity dSecurity = dInfo.GetAccessControl();
            //    InheritanceFlags iFlags = new InheritanceFlags();
            //    iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            //    FileSystemAccessRule AccessRule2 = new FileSystemAccessRule(SiteName, FileSystemRights.FullControl, iFlags, PropagationFlags.InheritOnly, AccessControlType.Allow);
            //    dSecurity.ModifyAccessRule(AccessControlModification.RemoveAll, AccessRule2, out ok);
            //    dInfo.SetAccessControl(dSecurity);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        #endregion

        #region 创建应用程序池
        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="appPoolName">应用程序池名</param>
        /// <returns></returns>
        static bool CreateAppPool(string appPoolName)
        {
            try
            {
                DirectoryEntry newpool;
                DirectoryEntry apppools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");
                newpool.Properties["MaxProcesses"].Value = 1;//Web 园
                newpool.Properties["PeriodicRestartPrivateMemory"].Value = 102400;//回收工作进程
                //newpool.Properties["PeriodicRestartRequests"].Value = 35000;//回收工作进程请求数  
                newpool.Properties["PeriodicRestartTime"].Value = 120;
                newpool.Properties["RapidFailProtection"].Value = false;
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
        #endregion

        #region 修改站点状态
        /// <summary>
        /// 站点状态
        /// </summary>
        /// <param name="Siteid">站点标认符</param>
        /// <param name="Status">站点状态代码</param>
        /// <returns>成功/失败</returns>
        public static bool ChangeWebSiteStatus(string Siteid, string Status)
        {
            if (Siteid == "0")
                return false;
            try
            {
                DirectoryEntry siteStatus = new DirectoryEntry("IIS://localhost/W3SVC/" + Siteid);
                switch (Status)
                {
                    case "0":
                        siteStatus.Invoke("Start", new object[] { });//运行
                        break;
                    case "1":
                        siteStatus.Invoke("Stop", new object[] { });//停止
                        break;
                    case "2":

                        break;
                    case "3":
                        siteStatus.Invoke("Pause", new object[] { });//暂停
                        break;

                }
                siteStatus.Invoke("SetInfo");
                siteStatus.CommitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 删除磁盘配额
        /// <summary>
        /// 删除磁盘配额
        /// </summary>
        /// <param name="Path">磁盘盘符</param>
        /// <param name="UsersName">站点名</param>
        /// <returns>成功/失败</returns>
        public static bool RemoveDiskQuotas(string Path, string UsersName)
        {
            try
            {
                DiskQuotaControlClass diskQuotaControl = new DiskQuotaControlClass();
                diskQuotaControl.Initialize(Path, true);
                DIDiskQuotaUser dskuser = null;
                dskuser = diskQuotaControl.FindUser(System.Net.Dns.GetHostName() + "\\" + UsersName);
                diskQuotaControl.DeleteUser(dskuser);
                dskuser = null;
                return true;
            }
            catch
            {
                return true;
            }

        }
        #endregion

        #region 删除IIS帐号
        /// <summary>
        /// 删除IIS帐号
        /// </summary>
        /// <param name="Username">站点名</param>
        /// <returns></returns>
        public static bool DelIISUser(string Username)
        {
            try
            {
                DirectoryEntry obComputer = new DirectoryEntry("WinNT://" + Environment.MachineName);//获得计算机实例
                DirectoryEntry obUser = obComputer.Children.Find(Username, "User");//找到用户
                obComputer.Children.Remove(obUser);//删除用户
                obUser.Close();
                obComputer.Close();
                return true;
            }
            catch
            {
                return true;
            }
        }

        #endregion

        #region 删除FTP
        /// <summary>
        /// 删除FTP
        /// </summary>
        /// <param name="webSiteName">站点名</param>
        /// <returns></returns>
        public static bool DeleteFtpVDir(string webSiteName)
        {
            try
            {
                DirectoryEntry root = new DirectoryEntry("IIS://localhost/MSFTPSVC/1/root");
                root.Invoke("Delete", "IIsFtpVirtualDir", webSiteName);
                root.Invoke("SetInfo");
                root.CommitChanges();
                return true;
            }
            catch
            {
                return true;
            }
        }
        #endregion

        #region 删除站点
        ///<summary>  删除站点  </summary>
        ///<param   name="WebSiteName">站点名</param>   
        ///<returns>成功或失败信息!</returns>   
        ///
        public static bool DeleteWebSite(string WebSiteName)
        {

            try
            {
                string SiteID = GetSiteID(WebSiteName);
                if (SiteID == null)
                {
                    return true;
                }
                DirectoryEntry deRoot = new DirectoryEntry("IIS://localhost/W3SVC");
                try
                {
                    DirectoryEntry deVDir = new DirectoryEntry();
                    deRoot.RefreshCache();
                    deVDir = deRoot.Children.Find(SiteID, "IIsWebServer");
                    deRoot.Children.Remove(deVDir);
                    deRoot.CommitChanges();
                    try
                    {
                        deVDir.Children.Find(SiteID, "IIsWebServer");
                        return false;
                    }
                    catch
                    {
                        return true;
                    }

                }
                catch
                {
                    return false;

                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 确认网站是否相同
        ///<summary> 确认网站是否可以创建</summary>
        ///<param name="bindStr">站点名</param>
        ///<returns>真为可以创建，假为不可以创建</returns>
        public static bool EnsureNewSiteEnavaible(string bindStr)
        {
            Application.DoEvents();
            string entPath = "IIS://localhost/w3svc";
            DirectoryEntry root = new DirectoryEntry(entPath);
            foreach (DirectoryEntry child in root.Children)
            {
                Thread.Sleep(1);
                if (child.SchemaClassName == "IIsWebServer")
                {
                    if (child.Properties["ServerComment"].Value != null)
                    {
                        if (child.Properties["ServerComment"].Value.ToString() == bindStr)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region 开设数据库方法
        /// <summary>
        /// 开设数据库方法
        /// </summary>
        /// <param name="DBType">数据库类型（mssql，mysql）</param>
        /// <param name="DBName">数据库名</param>
        /// <param name="DBUserName">登录名</param>
        /// <param name="DBPassWord">登录密码</param>
        /// <param name="DBSize">数据库大小</param>
        /// <returns>成功/失败</returns>
        public static bool CreateDataBases(string DBName, string DBUserName, string DBPassWord)
        {
            #region 开设MYSQL数据库
            //开通MYSQL
            string strSql_1 = "select * from db where db = '" + DBName + "' ";
            string strSql_2 = "select * from user where user = '" + DBUserName + "'";
            MySqlConnection myconn = new MySqlConnection("server=127.0.0.1;port=" + frmMain.MysqlDBPort + ";user id='" + frmMain.MysqlDBUser + "';password='" + frmMain.MysqlDBPassWord + "';database=mysql;pooling=false;Charset=utf8");
            try
            {
                myconn.Open();
                //检查数据库是否存在
                MySqlDataReader reader1 = new MySqlCommand(strSql_1, myconn).ExecuteReader();
                bool execute_1 = reader1.HasRows;
                reader1.Close();
                //检查数据库登录名是否存在
                MySqlDataReader reader2 = new MySqlCommand(strSql_2, myconn).ExecuteReader();
                bool execute_2 = reader2.HasRows;
                reader2.Close();
                string strSql_3 = "Create database `" + DBName + "`;";
                strSql_3 = strSql_3 + "GRANT USAGE ON *.* TO '" + DBUserName + "'@'%' IDENTIFIED BY '" + DBPassWord + "' WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0;";
                strSql_3 = strSql_3 + "GRANT ALL PRIVILEGES ON `" + DBName + "`.* TO '" + DBUserName + "'@'%' WITH GRANT OPTION;";

                if (!execute_1 && !execute_2)
                {
                    //如果数据库名不存在，则执行创建命令
                    try
                    {
                        MySqlDataReader reader3 = new MySqlCommand(strSql_3, myconn).ExecuteReader();
                        myconn.Close();
                        return true;
                    }
                    catch (Exception ex2)
                    {
                        WriteLog(ex2.ToString());
                        return false;
                    }
                }
                else
                {
                    //数据库存在删除数据库重新建立                            
                    if (!DeleteMySqlDataBase(DBName, DBUserName))
                        return false;
                    try
                    {
                        MySqlDataReader reader3 = new MySqlCommand(strSql_3, myconn).ExecuteReader();
                        myconn.Close();
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        WriteLog(ex1.ToString());
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
            #endregion
        }

        public static bool CreateMsSQL(string DBName, string DBUserName, string DBPassWord, string mssqldbsize)
        {
            if (!System.IO.Directory.Exists(frmMain.MssqlDBPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(frmMain.MssqlDBPath);
                }
                catch { }
            }

            string strSQL1 = "CREATE DATABASE \"" + DBName + "\" ";
            strSQL1 = strSQL1 + "ON ";
            strSQL1 = strSQL1 + "(NAME = \"" + DBName + "_Data\",FILENAME = '" + frmMain.MssqlDBPath + "\\" + DBName + "_Data.mdf',SIZE = 3MB,MAXSIZE = " + mssqldbsize + "MB,FILEGROWTH = 1MB) ";
            strSQL1 = strSQL1 + "LOG ON ";
            strSQL1 = strSQL1 + "(NAME ='" + DBName + "_Log',FILENAME = '" + frmMain.MssqlDBPath + "\\" + DBName + "_Log.ldf',SIZE = 1MB,MAXSIZE = " + mssqldbsize + "MB,FILEGROWTH = 1MB) ";
            strSQL1 = strSQL1 + "EXEC sp_addlogin '" + DBUserName + "', '" + DBPassWord + "', '" + DBName + "' ";

            string strSQL2 = "USE \"" + DBName + "\" ";
            strSQL2 = strSQL2 + "EXEC sp_changedbowner '" + DBUserName + "'";
            strSQL2 += " ALTER DATABASE [" + DBName + "] SET RECOVERY SIMPLE";
            SqlConnection conn = new SqlConnection("server=127.0.0.1," + frmMain.MssqlDBPort + ";uid=" + frmMain.MssqlDBUser + ";pwd=" + frmMain.MssqlDBPassWord + ";database=master");
            try
            {
                conn.Open();
                int execute1 = new SqlCommand(strSQL1, conn).ExecuteNonQuery();
                //如果开设数据库及用户成功，则执行给用户赋予权限
                if (execute1 == -1)
                {
                    int execute2 = new SqlCommand(strSQL2, conn).ExecuteNonQuery();
                }
                conn.Close();
                return true;

            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
        }

        #endregion

        #region 删除MY数据库及帐户方法
        /// <summary>
        /// 删除My数据库及帐户方法
        /// </summary>       
        /// <param name="DBName">数据库名</param>
        /// <param name="DBUserName">登录用户名</param>
        /// <returns>返回成功/失败</returns>
        public static bool DeleteMySqlDataBase(string DBName, string DBUserName)
        {

            //----删除MYSQL
            string strSql_1 = "SHOW DATABASES LIKE '" + DBName + "' ";
            string strSql_2 = " select * from user where user = '" + DBUserName + "'";
            MySqlConnection myconn = new MySqlConnection("server=127.0.0.1;port=" + frmMain.MysqlDBPort + ";user id=" + frmMain.MysqlDBUser + "; password=" + frmMain.MysqlDBPassWord + "; database=mysql; pooling=false ;Charset=utf8");
            try
            {
                myconn.Open();
                //检查数据库是否存在
                MySqlDataReader reader1 = new MySqlCommand(strSql_1, myconn).ExecuteReader();
                bool execute_1 = reader1.HasRows;
                reader1.Close();
                //检查数据库登录名是否存在
                MySqlDataReader reader2 = new MySqlCommand(strSql_2, myconn).ExecuteReader();
                bool execute_2 = reader2.HasRows;
                reader2.Close();
                if (execute_1 && execute_2)
                {
                    //如果数据库名存在，则执行删除命令
                    string strSql_3 = "DROP USER '" + DBUserName + "'@'%';";
                    strSql_3 = strSql_3 + "DROP DATABASE IF EXISTS `" + DBName + "`;";
                    try
                    {
                        MySqlDataReader reader3 = new MySqlCommand(strSql_3, myconn).ExecuteReader();
                        myconn.Close();
                        return true;
                    }
                    catch
                    {
                        return true;
                    }
                }
                else
                {
                    myconn.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 删除Ms数据库及帐户方法
        /// </summary>       
        /// <param name="DBName">数据库名</param>
        /// <param name="DBUserName">登录用户名</param>
        /// <returns>返回成功/失败</returns>
        public static bool DeleteMsSqlDataBase(string DBName, string DBUserName)
        {

            //先删除数据库，再执行删除登录用户
            //string strSQL1 = "drop database " + strDataBase;
            //string strSQL2 = "EXEC sp_droplogin '" + strUserName + "'";
            string strSQL1 = "ALTER DATABASE [" + DBName + "] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE USE [master] DROP DATABASE [" + DBName + "] execute sp_droplogin '" + DBUserName + "'";
            SqlConnection conn = new SqlConnection("server=127.0.0.1," + frmMain.MssqlDBPort + ";uid=" + frmMain.MssqlDBUser + ";pwd=" + frmMain.MssqlDBPassWord + ";database=master");
            try
            {
                conn.Open();
                int execute1 = new SqlCommand(strSQL1, conn).ExecuteNonQuery();
                //如果删除数据库成功，则执行删除用户
                //if (execute1 == -1)
                //{
                //    int execute2 = new SqlCommand(strSQL2, conn).ExecuteNonQuery();
                //}
                conn.Close();
                return true;

            }
            catch
            {
                return false;
            }
        }


        #endregion

        #region 修改IIS帐号密码
        /// <summary>
        /// 修改IIS帐号密码
        /// </summary>
        /// <param name="Username">帐户名</param>
        /// <param name="Userpassword">密码</param>
        /// <returns>成功/失败</returns>
        public static bool ChangeUserPassword(string Username, string Userpassword)
        {
            try
            {
                DirectoryEntry obComputer = new DirectoryEntry("WinNT://" + Environment.MachineName);
                DirectoryEntry obUser = obComputer.Children.Find(Username, "User");
                obUser.Invoke("SetPassword", Userpassword);
                obUser.CommitChanges();
                obUser.Close();
                obComputer.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ModifyWebSite(string SiteID, string OStr, string SStr)
        {
            if (SiteID == "" || SiteID == "0")
            {
                return false;
            }
            try
            {
                DirectoryEntry site = new DirectoryEntry("IIS://localhost/W3SVC/" + SiteID + "/root");
                site.Invoke("Put", OStr, SStr);
                site.CommitChanges();
                site.Close();
                site = new DirectoryEntry("IIS://localhost/W3SVC/" + SiteID + "/root");
                //site.Invoke("Put", OStr, SStr);
                site.Properties[OStr].Value = SStr;
                site.CommitChanges();
                site.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 修改数据库密码

        /// <summary>
        /// 修改MYSQL数据库密码
        /// </summary>
        /// <param name="MysqlDBName"></param>
        /// <param name="MysqlDBUser"></param>
        /// <param name="MysqlDBPassWord"></param>
        /// <returns></returns>
        public static bool ChangeMySQLPassword(string MysqlDBName, string MysqlDBUser, string MysqlDBPassWord)
        {
            MySqlConnection myConn = null;
            try
            {
                string SQL = "UPDATE `user` SET `password` = PASSWORD('" + MysqlDBPassWord + "') WHERE `user` = '" + MysqlDBUser + "';FLUSH PRIVILEGES;";

                myConn = new MySqlConnection("server=127.0.0.1;port=" + frmMain.MysqlDBPort + ";user id='" + frmMain.MysqlDBUser + "';password='" + frmMain.MysqlDBPassWord + "';database=mysql;pooling=false;Charset=utf8");

                MySqlCommand myCommand = new MySqlCommand();
                myConn.Open();
                myCommand.Connection = myConn;
                myCommand.CommandText = SQL;
                int reault = myCommand.ExecuteNonQuery();
                myConn.Close();
                if (reault > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
            finally
            {
                try
                {
                    myConn.Close();
                    myConn = null;
                }
                catch
                { }
            }

        }

        public static bool ChangeMsSQLPassword(string MssqlDBName, string MssqlDBUser, string MssqlDBPassWord)
        {
            SqlConnection conn = new SqlConnection("server=127.0.0.1," + frmMain.MssqlDBPort + ";uid=" + frmMain.MssqlDBUser + ";pwd=" + frmMain.MssqlDBPassWord + ";database=master");
            string strSQL1 = "EXEC sp_password NULL, '" + MssqlDBPassWord + "', '" + MssqlDBUser + "'";
            try
            {
                conn.Open();
                int execute1 = new SqlCommand(strSQL1, conn).ExecuteNonQuery();
                if (execute1 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                try
                {
                    conn.Close();
                    conn = null;
                }
                catch
                { }
            }
        }

        #endregion

        public static bool ModifyWebSite(string SiteID, string OStr, string[] SStr)
        {

            if (SiteID == "" || SiteID == "0")
            {
                return false;
            }
            try
            {
                DirectoryEntry site = new DirectoryEntry("IIS://localhost/W3SVC/" + SiteID + "/root");
                //site.Invoke("Put", OStr, SStr);
                site.Properties[OStr].Value = SStr;
                site.CommitChanges();
                site.Close();
                //site = new DirectoryEntry("IIS://localhost/W3SVC/" + SiteID + "/root");
                ////site.Invoke("Put", OStr, SStr);
                //site.Properties[OStr].Value = SStr;
                //site.CommitChanges();
                //site.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool EditWebSite(string SiteID, string OStr, string SStr)
        {

            if (SiteID == "" || SiteID == "0")
            {
                return false;
            }
            try
            {
                DirectoryEntry site = new DirectoryEntry("IIS://localhost/W3SVC/" + SiteID);
                //site.Invoke("Put", OStr, SStr);
                site.Properties[OStr].Value = SStr;
                site.CommitChanges();
                site.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 重启iis

        public static bool IISReset()
        {
            try
            {
                String s = "iiscnfg /save";
                s += ",iisreset";
                string Message = ExecuteCmd(s.Split(','));
                if (Message.IndexOf("成功") != -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

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
                    p.StandardInput.WriteLine(cmd[i].ToString());
            }
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }

        public static void WriteLog(string theStr)
        {
            WriteTxt(Application.StartupPath + "\\errlog.txt", theStr, true, Encoding.UTF8);
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

    }
}
