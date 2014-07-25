using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**  主要用于对IIS的控制

*********************************************************************************/
namespace VhostManage
{
    public partial class frmIIS : Form
    {
        public frmIIS()
        {
            InitializeComponent();
        }

        private void btnIISRest_Click(object sender, EventArgs e)
        {
            if (IISControl.IISReset())
            {
                MessageBox.Show("重启IIS成功!");
            }
            else
            {
                MessageBox.Show("重启IIS失败!");
            }
        }
    }
}