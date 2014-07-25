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
using System.IO;
using System.Globalization;

/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2012-04-09

** ������

**  ��Ҫ���ڶ�վ������޸�

*********************************************************************************/
namespace VhostManage
{
    public partial class frmIISReset : Form
    {
        public frmIISReset()
        {
            InitializeComponent();
        }

        private void frmIISReset_Load(object sender, EventArgs e)
        {
            ListSite();
        }

        private void ListSite()
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

        private void btnSiteReset_Click(object sender, EventArgs e)
        {
            string SiteName = this.lstSite.Text;
            if (SiteName.Trim() == "")
            {
                MessageBox.Show("��ѡ����Ҫ�޸���վ����!");
                this.lstSite.Focus();
            }
            else
            {
                if (iisReset(SiteName))
                {
                    MessageBox.Show("վ��:" + SiteName + "�޸��ɹ�!");

                }
                else
                {
                    MessageBox.Show("վ��:" + SiteName + "�޸�ʧ��!");
                }
            }


        }

        private bool iisReset(string SiteName)
        {
            try
            {
                if (!System.IO.Directory.Exists(frmMain.SitePath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(frmMain.SitePath);
                    }
                    catch { }
                }
                string siteid = "";
                string s_SiteName = "";
                string s_SitePass = "";
                string SiteDir = "";
                string strBinding = "";
                string s_MaxBandwidth = "";
                string s_MaxConnections = "";
                string s_DefaultDoc = "";
                string Script = "";
                //string ipls = "";
                string s_IPSecurity = "";
                string s_WebStatus = "0"; //WEB��ʼ״̬������ = 0 ֹͣ = 1 ��Ҫ��ת = 2��
                string s_FtpStatus = "0"; //FTP��ʼ״̬������ = 0 ֹͣ = 1��  
                string s_UrlForward = "";
                string s_DiskQuota = "0"; //�ռ��С
                string s_IPAddress = "";
                string strApp = "DefaultAppPool";
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                try
                {
                    OleCmd.CommandText = "select * from site where sitename='" + SiteName + "'";
                    OleConn.Open();
                    OleDbDataReader OleReader = null;
                    OleReader = OleCmd.ExecuteReader();
                    if (OleReader.HasRows)
                    {
                        OleReader.Read();
                        s_SiteName = OleReader["sitename"].ToString();
                        s_SitePass = OleReader["sitepassword"].ToString();
                        SiteDir = OleReader["sitedir"].ToString();
                        strBinding = OleReader["bindings"].ToString();
                        s_MaxBandwidth = OleReader["maxbandwidth"].ToString();
                        s_MaxConnections = OleReader["maxconnections"].ToString();
                        s_DiskQuota = OleReader["diskquota"].ToString();
                        s_DefaultDoc = OleReader["defaultdoc"].ToString();
                        Script = OleReader["script"].ToString();
                        s_WebStatus = OleReader["webstatus"].ToString();
                        s_FtpStatus = OleReader["ftpstatus"].ToString();
                        //s_IPSecurity = OleReader[""].ToString();
                        //s_WebStatus = OleReader[""].ToString();
                        //s_UrlForward = OleReader[""].ToString();
                        //strApp = OleReader[""].ToString();
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
                if (!txtIsMatch.SecurePassword(s_SitePass))
                {
                    if (MessageBox.Show("���벻���Ϲ淶,����Ҫ��14λ���ϵ����֡���ĸ�����������ϵ�����!�Ƿ�ʹ��ϵͳ������룿", "��ʾ��Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                    else
                    {
                        s_SitePass = RandomPassword.GetRandomPassword(12);
                        MessageBox.Show("վ��:" + s_SiteName + "�������Ϊ:" + s_SitePass);
                        try
                        {
                            OleCmd.CommandText = "update site set sitepassword = '" + s_SitePass + "' where sitename = '" + s_SiteName + "'";
                            OleConn.Open();
                            int ok = OleCmd.ExecuteNonQuery();
                            OleConn.Close();
                        }
                        catch { }
                    }
                }
                if (!Directory.Exists(SiteDir))
                {
                    Directory.CreateDirectory(SiteDir);
                }
                string[] ipaddress;
                ipaddress = Regex.Split(s_IPAddress, ",", RegexOptions.IgnoreCase);
                object[] ipls = new object[ipaddress.Length];
                for (int c = 0; c < ipaddress.Length; c++)
                {
                    Thread.Sleep(1);
                    ipls[c] = ipaddress[c].Replace("/", ",");
                }

                string Domain = strBinding;
                StringBuilder bing = new StringBuilder();
                string[] bds = Regex.Split(Domain, ",", RegexOptions.IgnoreCase);
                IdnMapping idn = new IdnMapping();
                foreach (string bd in bds)
                {
                    try
                    {
                        bing.Append(idn.GetAscii(bd)).Append(";");
                    }
                    catch { }
                }
                strBinding = ":80:" + bing.ToString().TrimEnd(';');
                strBinding = strBinding.Replace(";", ",");
                string[] s_Bindings = Regex.Split(strBinding.Replace(",", ",:80:"), ",", RegexOptions.IgnoreCase);
                //�ű�ӳ��
                if (Script == "")
                {
                    Script = "asp;php;net2;";
                }
                Script = Script.Substring(0, Script.Length - 1);
                string[] script = System.Text.RegularExpressions.Regex.Split(Script, ";");
                Script = "";
                foreach (string sc in script)
                {
                    switch (sc)
                    {
                        case "asp":
                            Script += frmMain.asp + ";";
                            break;
                        case "php":
                            Script += frmMain.php + ";";
                            break;
                        case "net1":
                            Script += frmMain.net1 + ";";
                            break;
                        case "net2":
                            Script += frmMain.net2 + ";";
                            break;
                        case "net4":
                            Script += frmMain.net4 + ";";
                            break;
                    }
                }
                Script = Script.Substring(0, Script.Length - 1);
                string[] s_Scriptmap = System.Text.RegularExpressions.Regex.Split(Script, ";");
                for (int a = 0; a < s_Scriptmap.Length; a++)
                {
                    Thread.Sleep(1);
                    s_Scriptmap[a] = s_Scriptmap[a].Trim();
                }

                IISControl.DeleteWebSite(s_SiteName);
                if (!IISControl.CreateWebSite(s_SiteName, s_SitePass, SiteDir, s_Bindings, s_MaxBandwidth, s_MaxConnections, s_DefaultDoc, s_Scriptmap, ipls, s_IPSecurity, s_WebStatus, s_UrlForward, strApp))
                {
                    return false;
                }
                IISControl.DeleteFtpVDir(s_SiteName);
                if (!IISControl.CreateFtpVDir(s_SiteName, SiteDir, s_FtpStatus))
                {
                    return false;
                }
                IISControl.RemoveDiskQuotas(SiteDir.Substring(0, 3), s_SiteName);
                IISControl.DelIISUser(s_SiteName);
                if (!IISControl.CreateIISUser(s_SiteName, s_SitePass))
                {
                    return false;
                }
                if (!IISControl.CreateDiskQuotas(SiteDir.Substring(0, 3), s_SiteName, s_DiskQuota))
                {
                    IISControl.SetChown1("Administrators", SiteDir);
                    IISControl.RemoveDiskQuotas(SiteDir.Substring(0, 3), s_SiteName);
                    return false;
                }
                if (IISControl.addDirectorySecurity(SiteDir, s_SiteName))
                {
                    //����״̬
                    IISControl.SetChown(s_SiteName, SiteDir);
                    siteid = IISControl.GetSiteID(s_SiteName);
                    if (s_WebStatus == "1")
                    {
                        IISControl.ChangeWebSiteStatus(siteid, s_WebStatus);
                    }
                }
                else
                {
                    IISControl.DeleteWebSite(s_SiteName);
                    IISControl.DeleteFtpVDir(s_SiteName);
                    IISControl.RemoveDiskQuotas(SiteDir.Substring(0, 3), s_SiteName);
                    IISControl.DelIISUser(s_SiteName);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        private void btnAllSiteReset_Click(object sender, EventArgs e)
        {
            string strMessage = "";
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
                        if (iisReset(OleReader["sitename"].ToString()))
                        {
                            strMessage += "վ��:" + OleReader["sitename"].ToString() + "�޸��ɹ�!\r\n";
                        }
                        else
                        {
                            strMessage += "վ��:" + OleReader["sitename"].ToString() + "�޸��ɹ�!\r\n";
                        }
                    }
                    OleReader.Close();
                }
                else
                {
                    strMessage = "û����Ҫ�޸���վ��!";
                }
                OleConn.Close();
                MessageBox.Show("�޸���Ϣ��\r\n" + strMessage);
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
                MessageBox.Show("�޸�ʧ��!");
            }


        }


    }
}