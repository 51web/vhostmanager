using System;
using System.Collections.Generic;
using System.Text;

/********************************************************************************

** ���ߣ� �����

** ��ʼʱ�䣺2010-03-19

** �޸��ˣ������

** �޸�ʱ�䣺2012-04-09

** ������

**  �������

*********************************************************************************/
namespace VhostManage
{
    class RandomPassword
    {

        /// <summary>
        /// ��ȡ������� �������������ĸ���������
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
            //�������Сд��ĸ
            randomChars = "abcdefghijklmnopqrstuvwxyz";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];
            //���������д��ĸ
            randomChars = "ABCDEMGHIJKLMNOPQRSTUVWXYZ";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];
            //�����������
            randomChars = "0123456789";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];
            //������������ַ�
            randomChars = "!@#$%^&*()";
            randomNum = random.Next(randomChars.Length);
            password += randomChars[randomNum];

            return password;
        }

    }

}
