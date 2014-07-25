using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 自动安装
{
    public partial class frmLoading : Form
    {
        public string txtMessage = "";
        public frmLoading()
        {
            InitializeComponent();
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            //textBox1.Text = txtMessage;
        }
    }
}