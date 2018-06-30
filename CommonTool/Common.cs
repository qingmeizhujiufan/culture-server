using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonTool
{
    public class Common
    {
        public static bool IsNumber(string strNumber)
        {
            bool bRtn = true;
            try
            {
                Convert.ToDouble(strNumber);
            }
            catch (Exception ex)
            {
                bRtn = false;
            }

            return bRtn;
        }

        public static bool IsDateTime(string strDate)
        {
            bool bRtn = true;
            try
            {
                Convert.ToDateTime(strDate);
            }
            catch (Exception ex)
            {
                bRtn = false;
            }

            return bRtn;
        }

        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int GetStringByteLength(string str)
        {
            if (str.Equals(string.Empty))
            {
                return 0;
            }
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;
        }

        public static string GetAppSetting(string key)
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            else
            {
                return "";
            }
        }

        public static string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.UTF8);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return strRet;
        }

        
        /// <summary>
        /// 获取地址位数的随机数
        /// </summary>
        /// <param name="count">生成随机数的位数</param>
        /// <returns></returns>
        public static string GetRandom(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   
            System.Random random = new Random();
            for (int i = 0; i < count; i++) //产生32位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            //       this.radom.Value = checkCode;
            return checkCode;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static  string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        //获取签名加密sha1
        public static string getSHA1(string str)
        {
            byte[] cleanBytes = System.Text.Encoding.Default.GetBytes(str);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static bool IsTelphone(string str)
        {
            bool bRtn = false;

            string phone = str.Trim();//手机号  
            if (!string.IsNullOrEmpty(phone))
            {
                try
                {
                    //num = Convert.ToInt32(txtTEL.Text.Trim().ToString());  
                    Convert.ToInt64(phone);
                }
                catch (Exception ex)
                {
                    bRtn = false;
                    return bRtn;
                }

                if (phone.Length != 11)
                {
                    bRtn = false;
                    return bRtn;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^((0?1[358]\d{9})|((0(10|2[1-3]|[3-9]\d{2}))?[1-9]\d{6,7}))$"))
                {
                    bRtn = false;
                    return bRtn;
                }

                bRtn = true;
                return bRtn;
            }
            else
            {
                bRtn = false;
                return bRtn;
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strData">merc_name=555&merc_address=555&merc_phone=555&merc_username=555&merc_userpwd=555</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDicByPostData(string strData)
        {
            string strRtn = string.Empty;
            string  strClientData = System.Web.HttpUtility.UrlDecode(strData, System.Text.Encoding.UTF8);
            string[] aryKeyValue = strClientData.Split(new char[] { '&' });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (aryKeyValue == null && aryKeyValue.Length <= 0)
            {
                strRtn = "";
            }
            else
            {
                for (int i = 0; i < aryKeyValue.Length; i++)
                {
                    string[] item = aryKeyValue[i].Split(new char[] { '=' });
                    if (item != null && item.Length >= 2)
                    {
                        dic.Add(item[0], item[1]);
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="preFix"></param>
        /// <returns></returns>
        public static string CreateOrderNo(string preFix)
        {
            string strRtn = string.Empty;
            Random rnd = new Random();
            strRtn = System.DateTime.Now.ToString().Replace("-", "").Replace("/", "").Replace(" ", "").Replace(":", "") + rnd.Next(10, 99);
            strRtn = preFix + strRtn;

            return strRtn;
        }
    }

    public class ServerRtnInfo
    {
        private string info;
        public string State { get; set; }
        public string Msg {get; set; }

        public ServerRtnInfo()
        {
            this.info = "{'state':'{0}', 'msg':'{1}'}";
        }

        public ServerRtnInfo(string state, string msg)
        {
            this.info = "{'state':'{0}', 'msg':'{1}'}";
            this.State = state;
            this.Msg = msg;
        }

        protected string Info
        {
            get
            {
                return this.info.Replace("{0}", this.State).Replace("{1}", this.Msg);
            }
        }

        public string ToString()
        {
            return this.Info;
        }
    }

}
