using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace CommonTool
{
    public class WXOperate
    {
        /// <summary>
        /// 获取openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetOpenID(string code)
        {
            string strRtn = string.Empty;

            if (string.IsNullOrEmpty(code))
            {
                return strRtn;
            }

            string urlForOpenID = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            urlForOpenID = string.Format(urlForOpenID, CommonTool.WXParam.APP_ID, CommonTool.WXParam.APP_SECRET, code);
            string strContent = CommonTool.Common.GetHtmlFromUrl(urlForOpenID);
            Dictionary<string, string> dic = CommonTool.JsonHelper.GetParms2(strContent);
            if (dic.Keys.Contains("openid"))
            {
                strRtn = dic["openid"].ToString();
            }
            //test
            CommonTool.WriteLog.Write("urlForOpenID: " + urlForOpenID);
            CommonTool.WriteLog.Write(strRtn);
            return strRtn;

        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetOAuthInfo(string code)
        {
            Dictionary<string, string> strRtn = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(code))
            {
                return strRtn;
            }

            string urlForOpenID = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            urlForOpenID = string.Format(urlForOpenID, CommonTool.WXParam.APP_ID, CommonTool.WXParam.APP_SECRET, code);
            string strContent = CommonTool.Common.GetHtmlFromUrl(urlForOpenID);
            strRtn = CommonTool.JsonHelper.GetParms2(strContent);

            return strRtn;

        }   

        public static Dictionary<string, string> GetUserInfo(string code)
        {
            Dictionary<string, string> dicRtn = new Dictionary<string, string>();

            string strUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
            Dictionary<string, string> dic_oauth_info = GetOAuthInfo(code);
            CommonTool.WriteLog.Write("dic_oauth_info ===== " + dic_oauth_info);
            if (dic_oauth_info.Count > 0 && dic_oauth_info.Keys.Contains("openid") && dic_oauth_info.Keys.Contains("access_token"))
            {
                strUrl = string.Format(strUrl, dic_oauth_info["access_token"].ToString(), dic_oauth_info["openid"].ToString());
                string strContent = Common.GetHtmlFromUrl(strUrl);
                CommonTool.WriteLog.Write("strContent===== " + strContent);
                dicRtn = JsonHelper.GetParms2(strContent);
            }

            return dicRtn;
        }

        public static WxUser GetWxUser(string code)
        {
            WxUser user = new WxUser();
            Dictionary<string, string> dic = WXOperate.GetUserInfo(code);
            if (dic != null && dic.Count > 0)
            {
                user.openid = dic["openid"];
                user.nickname = dic["nickname"];
                user.sex = dic["sex"];
                user.province = dic["province"];
                user.city = dic["city"];           
                user.country = dic["country"];
                user.headimgurl = dic["headimgurl"];
                user.unionid = dic["unionid"];
            }
            return user;
        }
    }
}
