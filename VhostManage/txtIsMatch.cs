using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2011-12-19

** 描述：

**  文本验证

*********************************************************************************/

namespace VhostManage
{
    //文本验证类
    class txtIsMatch
    {
        //FTP用户名验证
        internal static bool SiteName(string SiteName)
        {
            Regex obj_sitename = new Regex("^[a-z0-9]{1}[a-z0-9_-]{1,18}[a-z0-9]${1}");//匹配字母及数字^\\w+\\.[a-z0-9]+$
            if (!obj_sitename.IsMatch(SiteName))
                return false;
            else
                return true;
        }
        //FTP密码验证
        internal static bool SitePassword(string SitePassword)
        {
            Regex obj_sitename = new Regex("[a-zA-Z0-9-_`!@#$%^?&*()+=|<>:;*.,]{6,16}");//匹配字母及数字^\\w+\\.[a-z0-9]+$
            if (!obj_sitename.IsMatch(SitePassword))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //安全密码
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
            Regex obj_DiskQuota = new Regex("^([0-9]*)$");//数字
            if (!obj_DiskQuota.IsMatch(DiskQuota))
                return false;
            else
                return true;
        }
    }
}
