using System;
using System.Collections.Generic;
using System.Text;

/********************************************************************************

** 作者： 金德文

** 创始时间：2010-03-19

** 修改人：金德文

** 修改时间：2012-04-09

** 描述：

**  随机密码

*********************************************************************************/
namespace VhostManage
{
    class RandomPassword
    {

        /// <summary>
        /// 获取随机密码 必须包含数字字母和特殊符号
        /// </summary>
        /// <param name="passwordLen"></param>
        /// <returns></returns>
        public static string GetRandomPassword(int passwordLen)
        {
            string randomChars = "abcdefghijklmnopqrstuvwxyzABCDEMGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            //必须包含小写字母
            randomChars = "abcdefghijklmnopqrstuvwxyz";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];
            //必须包含大写字母
            randomChars = "ABCDEMGHIJKLMNOPQRSTUVWXYZ";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];
            //必须包含数字
            randomChars = "0123456789";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];
            //必须包含特殊字符
            randomChars = "!@#$%^&*()";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];

            return password;
        }

    }

}
