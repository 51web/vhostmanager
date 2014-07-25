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

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**  ��Ҫ���ڶ�վ����й���

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
                        //�ű�script 
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
                        MessageBox.Show("δ�鵽������Ϊ" + strSiteName + "�ļ�¼");
                        this.Close();
                    }
                    OleReader = null;
                    OleConn.Close();
                    //�г�Ӧ�ó����
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
                        this.gpbxDatabase.Text = "��վ����������ݿ�";
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
                    MessageBox.Show("��ѯ����");
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

        //FTP����
        private void btnChangeFtpPass_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_SitePass = this.txtSitePassword.Text;
            if (!txtIsMatch.SecurePassword(s_SitePass))
            {
                MessageBox.Show("���벻���Ϲ淶,����Ҫ��14λ���ϵ����֡���ĸ������������!");
                txtSitePassword.Focus();
                return;
            }
            if (IISControl.ChangeUserPassword(s_SiteName, s_SitePass) && IISControl.ModifyWebSite(IISControl.GetSiteID(s_SiteName), "AnonymousUserPass", s_SitePass))
            {
                MessageBox.Show("�������óɹ�!");
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
                MessageBox.Show("��������ʧ��!");
            }
        }

        //�������
        private void btnChangeDiskQuota_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_DiskQuota = this.txtDiskQuota.Text;
            if (!txtIsMatch.DiskQuota(s_DiskQuota))
            {
                MessageBox.Show("�ռ��С����!");
                return;
            }
            if (IISControl.UpdateDiskQuotas(this.txtSiteDir.Text.Substring(0, 3), s_SiteName, s_DiskQuota))
            {
                MessageBox.Show("�ռ��С���óɹ�!");
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
                MessageBox.Show("�ռ��С����ʧ��!");
            }

        }

        //�������
        private void btnChangeMaxBandwidth_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_MaxBandwidth = txtMaxBandwidth.Text;
            if (!txtIsMatch.DiskQuota(s_MaxBandwidth))
            {
                MessageBox.Show("���������������!");
                return;
            }
            if (s_MaxBandwidth == "0")
                s_MaxBandwidth = "&HFFFFFFFF";
            else
                s_MaxBandwidth = (1024 * Convert.ToDouble(s_MaxBandwidth)).ToString();
            if (IISControl.EditWebSite(IISControl.GetSiteID(s_SiteName), "MaxBandwidth", s_MaxBandwidth))
            {
                MessageBox.Show("�����������óɹ�!");
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
                MessageBox.Show("������������ʧ��!");

            }

        }
        //�޸�IIS��
        private void btnChangeMaxConnections_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string s_MaxConnections = txtMaxConnections.Text;
            if (!txtIsMatch.DiskQuota(s_MaxConnections))
            {
                MessageBox.Show("IIS�������������!");
                return;
            }
            if (s_MaxConnections == "0")
                s_MaxConnections = "&HFFFFFFFF";
            if (IISControl.EditWebSite(IISControl.GetSiteID(s_SiteName), "MaxConnections", s_MaxConnections))
            {
                MessageBox.Show("IIS���������óɹ�!");
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
                MessageBox.Show("IIS����������ʧ��!");
            }
        }

        //������
        private void btnChangeBindings_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;
            string Domain = txtBindings.Text;
            Domain = Domain.Replace(" ", string.Empty);
            if (Domain.Replace("\r\n", "").Trim() == "")
            {
                MessageBox.Show("�����������");
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
                MessageBox.Show("����������,��������ֿո񼰿���!\r\n" + ex.ToString());
                txtBindings.Focus();
                return;
            }
            strBinding = ":80:" + bing.ToString().TrimEnd(';');
            strBinding = strBinding.Replace(";", "\r\n");
            string[] s_Bindings = Regex.Split(strBinding.Replace("\r\n", ",:80:"), ",", RegexOptions.IgnoreCase);
            if (IISControl.UpdateBindString(IISControl.GetSiteID(s_SiteName), s_Bindings))
            {
                MessageBox.Show("�������ɹ�!");
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
                MessageBox.Show("������ʧ��!");
            }

        }

        //Ĭ���ĵ�
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
                MessageBox.Show("������Ĭ���ĵ�!");
                txtDefaultDoc.Focus();
                return;
            }
            s_DefaultDoc = s_DefaultDoc.Trim(',');
            if (IISControl.EditWebSite(IISControl.GetSiteID(s_SiteName), "DefaultDoc", s_DefaultDoc))
            {
                MessageBox.Show("Ĭ����ҳ���óɹ�!");
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
                MessageBox.Show("Ĭ����ҳ����ʧ��!");
            }
        }

        private void btnChangeScript_Click(object sender, EventArgs e)
        {
            string s_SiteName = this.txtSiteName.Text;

            #region �ű�ӳ��
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

                MessageBox.Show("�ű�ӳ�����óɹ�!");
            }
            else
            {
                MessageBox.Show("�ű�ӳ������ʧ��!");
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
        /// �޸�MySQL����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ChangeMySQLDatabasePassword_Click(object sender, EventArgs e)
        {
            if (lbl_mysqldbname.Text.Trim() != "" || this.lbl_mysqldbuser.Text.Trim() != "")
            {
                if (this.txt_mysqldbpassword.Text.Trim() == "")
                {
                    MessageBox.Show("���������ݿ�����!");
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
                        MessageBox.Show("���ݿ������޸ĳɹ�!");
                    }
                    else
                    {
                        MessageBox.Show("���ݿ������޸�ʧ��!");
                    }
                }
            }
            else
            {
                MessageBox.Show("���ݿ������û�������Ϊ��!");
            }
        }

        private void btn_ChangeMsSQLDatabasePassword_Click(object sender, EventArgs e)
        {

            if (lbl_mysqldbname.Text.Trim() != "" || this.lbl_mysqldbuser.Text.Trim() != "")
            {
                if (this.txt_mysqldbpassword.Text.Trim() == "")
                {
                    MessageBox.Show("���������ݿ�����!");
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
                        MessageBox.Show("���ݿ������޸ĳɹ�!");
                    }
                    else
                    {
                        MessageBox.Show("���ݿ������޸�ʧ��!");
                    }
                }
            }
            else
            {
                MessageBox.Show("���ݿ������û�������Ϊ��!");
            }

        }

    }
}