using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace CommonTool
{
    public class MD5Code
    {
        /// <summary>
        /// 对某一字符md5加密
        /// </summary>
        /// <param name="strPwd"></param>
        /// <param name="code">16与32位加密</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string strPwd, int code)
        {
            if (string.IsNullOrEmpty(strPwd))
            {
                return strPwd;
            }
            if (code == 16)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(strPwd, "MD5").ToLower().Substring(8, 16);
            }
            if (code == 32)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(strPwd, "MD5").ToLower();
            }
            return strPwd;
        }
    }
}
