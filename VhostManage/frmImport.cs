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

**  ��Ҫ������������

*********************************************************************************/
namespace VhostManage
{
    public partial class frmImport : Form
    {
        public frmImport()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "d:\\";
            openFileDialog1.Filter = "���ݿ�(*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "site.mdb";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOldDBPath.Text = openFileDialog1.FileName;
            }

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(txtOldDBPath.Text))
                {
                    txtOldDBPath.Text = "";
                    MessageBox.Show("ѡ���·������");
                }
                string Temp = "";
                Temp = ImportSite();
                MessageBox.Show(Temp);
                MessageBox.Show("��������������!");
            }
            catch
            {
                MessageBox.Show("ѡ���·������");
            }

        }

        private string ImportSite()
        {
            //��
            string Temp = "";
            string strTemp = "";
            string sql = "";
            try
            {
                OleDbConnection oldOleConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtOldDBPath.Text);
                OleDbCommand oldOleCmd = new OleDbCommand();
                oldOleCmd.Connection = oldOleConn;
                OleDbDataReader OleReader = null;
                oldOleConn.Open();
                oldOleCmd.CommandText = "select * from site where sitename<>''";
                OleReader = oldOleCmd.ExecuteReader();
                if (OleReader.HasRows)
                {
                    while (OleReader.Read())
                    {
                        sql = "insert into site(siteid,sitename,sitepassword,sitedir,bindings,defaultdoc,diskquota,maxbandwidth,maxconnections,mysqldbname,mysqldbuser,mysqldbpassword) " +
                                 "values('" + OleReader["siteid"].ToString() + "','"
                                 + OleReader["sitename"].ToString() + "','"
                                 + OleReader["sitepassword"].ToString() + "','"
                                 + OleReader["sitedir"].ToString() + "','"
                                 + OleReader["bindings"].ToString() + "','"
                                 + OleReader["defaultdoc"].ToString() + "','"
                                 + OleReader["diskquota"].ToString() + "','"
                                 + OleReader["maxbandwidth"].ToString() + "','"
                                 + OleReader["maxconnections"].ToString() + "','"
                                 + OleReader["mysqldbname"].ToString() + "','"
                                 + OleReader["mysqldbname"].ToString() + "','"
                                 + OleReader["mysqldbpassword"].ToString() + "')";
                        strTemp += insert(OleReader["sitename"].ToString(), OleReader["siteid"].ToString(), sql);
                    }
                }
                OleReader.Close();
                oldOleConn.Close();
                if (strTemp == "")
                {
                    Temp = "�����¼�ɹ�!";
                }
                else
                {
                    Temp = "�����¼�ɹ�!����" + strTemp;
                }
            }
            catch
            {
                Temp = "�����¼ʧ��!";
            }
            return Temp;
        }


        private string insert(string sitename, string siteid, string sql)
        {
            string temp = "";
            try
            {
                //��
                OleDbConnection OleConn = ConnClass.DataConn();
                OleDbCommand OleCmd = new OleDbCommand();
                OleCmd.Connection = OleConn;
                OleConn.Open();
                OleCmd.CommandText = "select * from site where siteid = '" + siteid + "' and sitename = '" + sitename + "'";
                OleDbDataReader OleReader = null;
                OleReader = OleCmd.ExecuteReader();
                if (!OleReader.HasRows)
                {
                    OleReader.Close();
                    OleReader = null;
                    OleCmd.CommandText = sql;
                    OleCmd.ExecuteNonQuery();
                }
                else
                {
                    temp = "վ��:" + sitename + "�Ѵ���!";
                }
                OleConn.Close();
            }
            catch (Exception ex)
            {
                temp = "վ��:" + sitename + "����ʧ��" + ex.ToString();
            }
            return temp;
        }


    }
}