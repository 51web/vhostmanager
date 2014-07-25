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

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**    ��Ҫ���ڶ�IIS6.0�Ŀ���

*********************************************************************************/


namespace VhostManage
{
    public class IISControl
    {
        #region ����վ��
        /// <summary>
        /// ����վ��
        /// </summary>
        /// <param name="WebSiteName">վ������</param>
        /// <param name="Password">FTP����</param>
        /// <param name="PathToRoot">�����ַ</param>
        /// <param name="BindString"></param>
        /// <param name="MaxBandwidth">�������</param>
        /// <param name="MaxConnections">���������</param>
        /// <param name="DefaultDoc">Ĭ���ĵ�</param>
        /// <param name="ScriptMapsLst">�ű�ӳ��</param>
        /// <param name="IPList">ip�б�</param>
        /// <param name="Ipsecurity">ip����</param>
        /// <param name="WebStatus">վ��״̬</param>
        /// <param name="HttpRedirect">�ض���URL</param>
        /// <returns>�Ƿ�ɹ�</returns>   
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
                ID = ran.Next(10000000, 99999999);//������վ��ʶ��
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
                //����             
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
                site.Properties["ServerBindings"].Value = BindString;       //������
                site.Properties["ScriptMaps"].Value = ScriptMapsLst;        //�ű�ӳ��  
                site.Properties["ServerState"].Value = 2;                   //վ��״̬
                site.Properties["DefaultDoc"].Value = DefaultDoc;           //Ĭ���ĵ�����
                site.Properties["AnonymousUserName"].Value = WebSiteName;
                site.Properties["AnonymousUserPass"].Value = Password;
                site.Properties["AspEnableParentPaths"].Value = true;       //��·��
                site.Properties["ServerAutoStart"].Value = WebState;        //����״̬
                site.Properties["ServerSize"].Value = 1;
                site.Properties["MaxBandwidth"].Value = MaxBandwidth;       //�������
                site.Properties["MaxConnections"].Value = MaxConnections;   //���������
                site.Properties["AppPoolId"].Value = strApp;                //ָ��Ӧ�ó����

                //����Ӧ�ó��������Ŀ¼
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
                    // ��ȡĬ����Ϊ
                    bool bGrantByDefault = (bool)typ.InvokeMember("GrantByDefault", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty, null, IPSecurity, null);
                    // ����Ĭ����Ϊ
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

        #region ����Ftp����Ŀ¼
        /// <summary>
        /// ����Ftp����Ŀ¼
        /// </summary>
        /// <param name="webSiteName">վ����</param>
        /// <param name="VDir">·��</param>
        /// <param name="status">վ��״̬������ = 0 ֹͣ = 1��</param>
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
                site.Properties["AccessRead"][0] = strStatus;//��Ȩ��
                site.Properties["AccessWrite"][0] = strStatus;//дȨ��
                site.Properties["DontLog"][0] = false;//true����¼��־
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
                return true;//�����ɹ�
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                //ftp�Ѵ���
                return false;
            }

        }
        #endregion

        #region ����IISϵͳ�ʺ�
        /// <summary>
        /// ����IISϵͳ�ʺ�
        /// </summary>
        /// <param name="Username">�ʻ���</param>
        /// <param name="Userpassword">����</param>
        /// <returns>�ɹ�/ʧ��</returns>
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
                const int ADS_UF_DEFAULT = 0x0201;                              //Ĭ��
                //const int ADS_UF_SCRIPT = 0x0001;                             //��ִ�е�¼�ű�
                //const int ADS_UF_ACCOUNTDISABLE = 0x0002;               		//�����ʻ�
                //const int ADS_UF_HOMEDIR_REQUIRED = 0x0008;	                //�ʻ���Ҫ��Ŀ¼
                //const int ADS_UF_LOCKOUT = 0x0010;		                    //�����ʻ�
                //const int ADS_UF_PASSWD_NOTREQD = 0x0020;	                    //�ʻ�����Ҫ����
                const int ADS_UF_PASSWD_CANT_CHANGE = 0x0040;	                //�û����ܸ�������
                //const int ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x0080;	//��������ı�����
                const int ADS_UF_DONT_EXPIRE_PASSWD = 0x10000;	                //�ʻ�������������
                //const int ADS_UF_SMARTCARD_REQUIRED = 0x40000;	            //��¼��Ҫʹ�����ܿ�
                //const int ADS_UF_PASSWORD_EXPIRED = 0x800000;		            //�����ѹ���
                NewUser.Properties["UserFlags"].Value = ADS_UF_DEFAULT + ADS_UF_PASSWD_CANT_CHANGE + ADS_UF_DONT_EXPIRE_PASSWD;//�����û����ܸ�������/�ʻ�������������
                NewUser.Invoke("Put", new object[] { "Description", "������������ִ���" });
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

        #region ����û��������
        /// <summary>
        /// ����û��������
        /// </summary>
        /// <param name="Path">�����̷�</param>
        /// <param name="UsersName">վ����</param>
        /// <param name="DiskQuota"></param>
        /// <returns>�ɹ�/ʧ��</returns>
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
                //Console.WriteLine("�û���:" + dskuser.LogonName);
                //Console.WriteLine("�ܴ�С" + dskuser.QuotaLimitText);
                //Console.WriteLine("����ȼ�" + dskuser.QuotaThresholdText);
                //Console.WriteLine("״̬" + dskuser.AccountStatus);
                //Console.WriteLine("�Ѿ�ʹ��" + dskuser.QuotaUsedText);            
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region ���´������
        /// <summary>
        /// ���´������
        /// </summary>
        /// <param name="Path">�����̷�</param>
        /// <param name="UsersName">վ����</param>
        /// <param name="DiskQuota">����С</param>
        /// <returns>�ɹ�/ʧ��</returns>
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

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="Siteid">վ���ʶ��</param>
        /// <param name="BindString">���ַ���</param>
        /// <returns>�ɹ�/ʧ��</returns>
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

        #region ����������
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="UserName">�û���</param>
        /// <param name="SitePath">·��d:\wwwroot\test</param>
        /// <returns>�ɹ�/ʧ��</returns>
        public static bool SetChown(string UserName, string SitePath)
        { //System.Diagnostics.Process Proc = System.Diagnostics.Process.Start("chown.exe", @"chown.exe -r -q vtest4 d:\wwwroot\vtest3\*");
            string TmpChown = " -r -q " + UserName + " " + SitePath + "\\*";
            try
            {
                System.Diagnostics.Process Process = new System.Diagnostics.Process();
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
                Process.StartInfo.FileName = "chown.exe";           //���ó�ʽ��            
                Process.StartInfo.Arguments = TmpChown; //�趨ִ�в���
                Process.StartInfo.UseShellExecute = false;   //�ر�Shell��ʹ��                      
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
                Process.StartInfo.FileName = "chown.exe";           //���ó�ʽ��            
                Process.StartInfo.Arguments = TmpChown; //�趨ִ�в���
                Process.StartInfo.UseShellExecute = false;   //�ر�Shell��ʹ��                      
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

        #region ���վ���ʶ��
        /// <summary>
        ///  ���վ���ʶ��
        /// </summary>
        /// <param name="WebSiteName">վ����</param>
        /// <returns>վ���ʶ��</returns>
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

        #region ���Ŀ¼Ȩ��
        /// <summary>
        /// ����Ŀ¼Ȩ��
        /// </summary>
        /// <param name="PathDir">·��</param>
        /// <param name="SiteName">վ����</param>
        /// <returns></returns>
        public static bool addDirectorySecurity(string PathDir, string SiteName)
        {
            try
            {
                //string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F " + SiteName + ":F";
                string tmpRar = string.Format("echo Y|cacls.exe \"{0}\" /C /T /G \"Administrators\":F \"SYSTEM\":F;icacls.exe \"{0}\" /grant iis_wpg:(oI)(ci)(R) {1}:(oi)(CI)(R);icacls.exe {0} /deny {1}:(oi)(ci)(wa,wea);icacls.exe \"{0}\" /grant {1}:(oi)(CI)(R,w,D)", PathDir, SiteName);
                ExecuteCmd(tmpRar.Split(';'));
                //Process Process = new System.Diagnostics.Process();              //����һ�������࣬�����ⲿ����ѹ��վ��
                //Process.StartInfo.WorkingDirectory = Application.StartupPath;
                //Process.StartInfo.FileName = "cacls.exe";                        //���ó�ʽ��   
                //Process.StartInfo.FileName = "cmd.exe";
                //Process.StartInfo.Arguments = tmpRar;                            //�趨ִ�в���
                //Process.StartInfo.UseShellExecute = false;                       //�ر�Shell��ʹ��                      
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

        //���ݿ��ʻ�
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
                Process Process = new System.Diagnostics.Process();              //����һ�������࣬�����ⲿ����ѹ��վ��
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
                Process.StartInfo.FileName = "cacls.exe";                          //���ó�ʽ��            
                Process.StartInfo.Arguments = tmpRar;                            //�趨ִ�в���
                Process.StartInfo.UseShellExecute = false;                       //�ر�Shell��ʹ��                      
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

        #region ɾ��Ŀ¼Ȩ��
        /// <summary>
        /// ����Ŀ¼Ȩ��
        /// </summary>
        /// <param name="PathDir">·��</param>
        /// <param name="SiteName">վ����</param>
        /// <returns></returns>
        public static bool DelDirectorySecurity(string PathDir, string SiteName)
        {
            try
            {
                string tmpRar = "\"" + PathDir + "\" /C /T /G Administrators:F System:F \"Network Service\":F";
                Process Process = new System.Diagnostics.Process();              //����һ�������࣬�����ⲿ����ѹ��վ��
                Process.StartInfo.WorkingDirectory = Application.StartupPath;
                Process.StartInfo.FileName = "cacls.exe";                          //���ó�ʽ��            
                Process.StartInfo.Arguments = tmpRar;                            //�趨ִ�в���
                Process.StartInfo.UseShellExecute = false;                       //�ر�Shell��ʹ��                      
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

        #region ����Ӧ�ó����
        /// <summary>
        /// ����Ӧ�ó����
        /// </summary>
        /// <param name="appPoolName">Ӧ�ó������</param>
        /// <returns></returns>
        static bool CreateAppPool(string appPoolName)
        {
            try
            {
                DirectoryEntry newpool;
                DirectoryEntry apppools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");
                newpool.Properties["MaxProcesses"].Value = 1;//Web ԰
                newpool.Properties["PeriodicRestartPrivateMemory"].Value = 102400;//���չ�������
                //newpool.Properties["PeriodicRestartRequests"].Value = 35000;//���չ�������������  
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

        #region �޸�վ��״̬
        /// <summary>
        /// վ��״̬
        /// </summary>
        /// <param name="Siteid">վ����Ϸ�</param>
        /// <param name="Status">վ��״̬����</param>
        /// <returns>�ɹ�/ʧ��</returns>
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
                        siteStatus.Invoke("Start", new object[] { });//����
                        break;
                    case "1":
                        siteStatus.Invoke("Stop", new object[] { });//ֹͣ
                        break;
                    case "2":

                        break;
                    case "3":
                        siteStatus.Invoke("Pause", new object[] { });//��ͣ
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

        #region ɾ���������
        /// <summary>
        /// ɾ���������
        /// </summary>
        /// <param name="Path">�����̷�</param>
        /// <param name="UsersName">վ����</param>
        /// <returns>�ɹ�/ʧ��</returns>
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

        #region ɾ��IIS�ʺ�
        /// <summary>
        /// ɾ��IIS�ʺ�
        /// </summary>
        /// <param name="Username">վ����</param>
        /// <returns></returns>
        public static bool DelIISUser(string Username)
        {
            try
            {
                DirectoryEntry obComputer = new DirectoryEntry("WinNT://" + Environment.MachineName);//��ü����ʵ��
                DirectoryEntry obUser = obComputer.Children.Find(Username, "User");//�ҵ��û�
                obComputer.Children.Remove(obUser);//ɾ���û�
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

        #region ɾ��FTP
        /// <summary>
        /// ɾ��FTP
        /// </summary>
        /// <param name="webSiteName">վ����</param>
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

        #region ɾ��վ��
        ///<summary>  ɾ��վ��  </summary>
        ///<param   name="WebSiteName">վ����</param>   
        ///<returns>�ɹ���ʧ����Ϣ!</returns>   
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

        #region ȷ����վ�Ƿ���ͬ
        ///<summary> ȷ����վ�Ƿ���Դ���</summary>
        ///<param name="bindStr">վ����</param>
        ///<returns>��Ϊ���Դ�������Ϊ�����Դ���</returns>
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

        #region �������ݿⷽ��
        /// <summary>
        /// �������ݿⷽ��
        /// </summary>
        /// <param name="DBType">���ݿ����ͣ�mssql��mysql��</param>
        /// <param name="DBName">���ݿ���</param>
        /// <param name="DBUserName">��¼��</param>
        /// <param name="DBPassWord">��¼����</param>
        /// <param name="DBSize">���ݿ��С</param>
        /// <returns>�ɹ�/ʧ��</returns>
        public static bool CreateDataBases(string DBName, string DBUserName, string DBPassWord)
        {
            #region ����MYSQL���ݿ�
            //��ͨMYSQL
            string strSql_1 = "select * from db where db = '" + DBName + "' ";
            string strSql_2 = "select * from user where user = '" + DBUserName + "'";
            MySqlConnection myconn = new MySqlConnection("server=127.0.0.1;port=" + frmMain.MysqlDBPort + ";user id='" + frmMain.MysqlDBUser + "';password='" + frmMain.MysqlDBPassWord + "';database=mysql;pooling=false;Charset=utf8");
            try
            {
                myconn.Open();
                //������ݿ��Ƿ����
                MySqlDataReader reader1 = new MySqlCommand(strSql_1, myconn).ExecuteReader();
                bool execute_1 = reader1.HasRows;
                reader1.Close();
                //������ݿ��¼���Ƿ����
                MySqlDataReader reader2 = new MySqlCommand(strSql_2, myconn).ExecuteReader();
                bool execute_2 = reader2.HasRows;
                reader2.Close();
                string strSql_3 = "Create database `" + DBName + "`;";
                strSql_3 = strSql_3 + "GRANT USAGE ON *.* TO '" + DBUserName + "'@'%' IDENTIFIED BY '" + DBPassWord + "' WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0;";
                strSql_3 = strSql_3 + "GRANT ALL PRIVILEGES ON `" + DBName + "`.* TO '" + DBUserName + "'@'%' WITH GRANT OPTION;";

                if (!execute_1 && !execute_2)
                {
                    //������ݿ��������ڣ���ִ�д�������
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
                    //���ݿ����ɾ�����ݿ����½���                            
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
                //����������ݿ⼰�û��ɹ�����ִ�и��û�����Ȩ��
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

        #region ɾ��MY���ݿ⼰�ʻ�����
        /// <summary>
        /// ɾ��My���ݿ⼰�ʻ�����
        /// </summary>       
        /// <param name="DBName">���ݿ���</param>
        /// <param name="DBUserName">��¼�û���</param>
        /// <returns>���سɹ�/ʧ��</returns>
        public static bool DeleteMySqlDataBase(string DBName, string DBUserName)
        {

            //----ɾ��MYSQL
            string strSql_1 = "SHOW DATABASES LIKE '" + DBName + "' ";
            string strSql_2 = " select * from user where user = '" + DBUserName + "'";
            MySqlConnection myconn = new MySqlConnection("server=127.0.0.1;port=" + frmMain.MysqlDBPort + ";user id=" + frmMain.MysqlDBUser + "; password=" + frmMain.MysqlDBPassWord + "; database=mysql; pooling=false ;Charset=utf8");
            try
            {
                myconn.Open();
                //������ݿ��Ƿ����
                MySqlDataReader reader1 = new MySqlCommand(strSql_1, myconn).ExecuteReader();
                bool execute_1 = reader1.HasRows;
                reader1.Close();
                //������ݿ��¼���Ƿ����
                MySqlDataReader reader2 = new MySqlCommand(strSql_2, myconn).ExecuteReader();
                bool execute_2 = reader2.HasRows;
                reader2.Close();
                if (execute_1 && execute_2)
                {
                    //������ݿ������ڣ���ִ��ɾ������
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
        /// ɾ��Ms���ݿ⼰�ʻ�����
        /// </summary>       
        /// <param name="DBName">���ݿ���</param>
        /// <param name="DBUserName">��¼�û���</param>
        /// <returns>���سɹ�/ʧ��</returns>
        public static bool DeleteMsSqlDataBase(string DBName, string DBUserName)
        {

            //��ɾ�����ݿ⣬��ִ��ɾ����¼�û�
            //string strSQL1 = "drop database " + strDataBase;
            //string strSQL2 = "EXEC sp_droplogin '" + strUserName + "'";
            string strSQL1 = "ALTER DATABASE [" + DBName + "] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE USE [master] DROP DATABASE [" + DBName + "] execute sp_droplogin '" + DBUserName + "'";
            SqlConnection conn = new SqlConnection("server=127.0.0.1," + frmMain.MssqlDBPort + ";uid=" + frmMain.MssqlDBUser + ";pwd=" + frmMain.MssqlDBPassWord + ";database=master");
            try
            {
                conn.Open();
                int execute1 = new SqlCommand(strSQL1, conn).ExecuteNonQuery();
                //���ɾ�����ݿ�ɹ�����ִ��ɾ���û�
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

        #region �޸�IIS�ʺ�����
        /// <summary>
        /// �޸�IIS�ʺ�����
        /// </summary>
        /// <param name="Username">�ʻ���</param>
        /// <param name="Userpassword">����</param>
        /// <returns>�ɹ�/ʧ��</returns>
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

        #region �޸����ݿ�����

        /// <summary>
        /// �޸�MYSQL���ݿ�����
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

        #region ����iis

        public static bool IISReset()
        {
            try
            {
                String s = "iiscnfg /save";
                s += ",iisreset";
                string Message = ExecuteCmd(s.Split(','));
                if (Message.IndexOf("�ɹ�") != -1)
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
        /// ���ı��ļ�
        /// </summary>
        /// <param name="strPath">·��</param>
        /// <param name="encod">����</param>
        /// <returns>����</returns>
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
        /// д�ı��ļ�
        /// </summary>
        /// <param name="strPath">·��</param>
        /// <param name="theStr">����</param>
        /// <param name="append">�Ƿ�׷��</param>
        /// <param name="encod">����</param>
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
