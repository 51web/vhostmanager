using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace VhostManage
{
    class ConnClass
    {
        public static OleDbConnection DataConn()
        {
            Read.Ini ini = new Read.Ini();
            //检查数据库配置文件是否存在
            string dbpath = ini.IniReadValue("SysConfig", "DBPath");
            //string dbpath = Application.StartupPath.ToString();
            //dbpath += @"\DataBase";
            //dbpath += @"\site.mdb";
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbpath);
        }
    }
}
