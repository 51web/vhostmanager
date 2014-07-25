using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace VhostManage
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            Mutex m = new Mutex(true, "网站管理助手", out   createdNew);
            if (!createdNew)
            {
                MessageBox.Show("程序已运行!请不要重复执行!", "警告!", MessageBoxButtons.OK);
                GC.KeepAlive(m);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            GC.KeepAlive(m);
        }
    }
}