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

**  主要用于数据移置

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
            openFileDialog1.Filter = "数据库(*.mdb)|*.mdb";
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
                    MessageBox.Show("选择的路径错误");
                }
                string Temp = "";
                Temp = ImportSite();
                MessageBox.Show(Temp);
                MessageBox.Show("请重启本管理工具!");
            }
            catch
            {
                MessageBox.Show("选择的路径错误");
            }

        }

        private string ImportSite()
        {
            //老
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
                    Temp = "导入记录成功!";
                }
                else
                {
                    Temp = "导入记录成功!其中" + strTemp;
                }
            }
            catch
            {
                Temp = "导入记录失败!";
            }
            return Temp;
        }


        private string insert(string sitename, string siteid, string sql)
        {
            string temp = "";
            try
            {
                //新
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
                    temp = "站点:" + sitename + "已存在!";
                }
                OleConn.Close();
            }
            catch (Exception ex)
            {
                temp = "站点:" + sitename + "导入失败" + ex.ToString();
            }
            return temp;
        }


    }
}