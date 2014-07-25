using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2011-12-19

** ������

**  �ı���֤

*********************************************************************************/

namespace VhostManage
{
    //�ı���֤��
    class txtIsMatch
    {
        //FTP�û�����֤
        internal static bool SiteName(string SiteName)
        {
            Regex obj_sitename = new Regex("^[a-z0-9]{1}[a-z0-9_-]{1,18}[a-z0-9]${1}");//ƥ����ĸ������^\\w+\\.[a-z0-9]+$
            if (!obj_sitename.IsMatch(SiteName))
                return false;
            else
                return true;
        }
        //FTP������֤
        internal static bool SitePassword(string SitePassword)
        {
            Regex obj_sitename = new Regex("[a-zA-Z0-9-_`!@#$%^?&*()+=|<>:;*.,]{6,16}");//ƥ����ĸ������^\\w+\\.[a-z0-9]+$
            if (!obj_sitename.IsMatch(SitePassword))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //��ȫ����
        internal static bool SecurePassword(string Password)
        {
            Regex rx = new Regex(@"(?=.{6,})(?=(.*\d){1,})(?=(.*\W){1,}){14,}", RegexOptions.Compiled);
            if (!rx.IsMatch(Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public static bool DiskQuota(string DiskQuota)
        {
            Regex obj_DiskQuota = new Regex("^([0-9]*)$");//����
            if (!obj_DiskQuota.IsMatch(DiskQuota))
                return false;
            else
                return true;
        }
    }
}
