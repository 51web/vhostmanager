using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**  ��Ҫ���ڶ�IIS�Ŀ���

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
                MessageBox.Show("����IIS�ɹ�!");
            }
            else
            {
                MessageBox.Show("����IISʧ��!");
            }
        }
    }
}