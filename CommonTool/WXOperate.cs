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
            CommonTool.WriteLog.Write(strRtn);
            return strRtn;

        }

        /// <summary>
        /// 获取统一订单的package参数
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static string GetPackage(Dictionary<string, string> dic)
        {
            string strRtn = "prepay_id=";
            if (null == dic)
            {
                return "";
            }

            //签名所需参数
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("appid={0}&", CommonTool.WXParam.APP_ID);
            sb.AppendFormat("body={0}&", dic["body"]);
            sb.AppendFormat("mch_id={0}&", CommonTool.WXParam.MCH_ID);
            sb.AppendFormat("nonce_str={0}&", dic["noncestr"]);
            sb.AppendFormat("notify_url={0}&", CommonTool.WXParam.NOTIFY_URL);
            sb.AppendFormat("openid={0}&", dic["openid"]);
            sb.AppendFormat("out_trade_no={0}&", dic["out_trade_no"]);
            sb.AppendFormat("spbill_create_ip={0}&", dic["spbill_ip"]);
            sb.AppendFormat("total_fee={0}&", dic["totalMoney"]);
            sb.AppendFormat("trade_type={0}&", CommonTool.WXParam.TRADE_TYPE);
            sb.AppendFormat("key={0}", CommonTool.WXParam.API_KEY);

            //md5加密获取签名
            string sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString(), "MD5").ToUpper();

            CommonTool.WriteLog.Write(sign);

            //获取package所需参数
            StringBuilder sbXmlParam = new StringBuilder();
            sbXmlParam.Append("<xml>");
            sbXmlParam.AppendFormat("<appid>{0}</appid>", CommonTool.WXParam.APP_ID);
            sbXmlParam.AppendFormat("<body>{0}</body>", dic["body"]);
            sbXmlParam.AppendFormat("<mch_id>{0}</mch_id>", CommonTool.WXParam.MCH_ID);
            sbXmlParam.AppendFormat("<nonce_str>{0}</nonce_str>", dic["noncestr"]);
            sbXmlParam.AppendFormat("<notify_url>{0}</notify_url>", CommonTool.WXParam.NOTIFY_URL);
            sbXmlParam.AppendFormat("<openid>{0}</openid>", dic["openid"]);
            sbXmlParam.AppendFormat("<out_trade_no>{0}</out_trade_no>", dic["out_trade_no"]);
            sbXmlParam.AppendFormat("<spbill_create_ip>{0}</spbill_create_ip>", dic["spbill_ip"]);
            sbXmlParam.AppendFormat("<total_fee>{0}</total_fee>", dic["totalMoney"]);
            sbXmlParam.AppendFormat("<trade_type>{0}</trade_type>", CommonTool.WXParam.TRADE_TYPE);
            sbXmlParam.AppendFormat("<sign>{0}</sign>", sign);
            sbXmlParam.Append("</xml>");

            //获取package
            string strContent = CommonTool.HttpWeb.HttpPost("https://api.mch.weixin.qq.com/pay/unifiedorder", sbXmlParam.ToString());

            //test
            CommonTool.WriteLog.Write(sbXmlParam.ToString());

            //解析其中的prepay_id字段
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(strContent);
            XmlNode xmlNode1 = xmldoc.SelectSingleNode("/xml/prepay_id");
            if (xmlNode1 != null)
            {
                strRtn = strRtn + xmlNode1.InnerText;
            }

            return strRtn;

        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetPaySign(Dictionary<string, string> dic)
        {
            string strRtn = string.Empty;

            //支付签名所需参数
            StringBuilder sbSign = new StringBuilder();
            sbSign.AppendFormat("appId={0}&", CommonTool.WXParam.APP_ID);
            sbSign.AppendFormat("nonceStr={0}&", dic["noncestr"]);
            sbSign.AppendFormat("package={0}&", dic["package"]);
            sbSign.AppendFormat("signType={0}&", "MD5");
            sbSign.AppendFormat("timeStamp={0}&", dic["timestamp"]);
            sbSign.AppendFormat("key={0}", CommonTool.WXParam.API_KEY);

            //MD5加密获取签名
            strRtn = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sbSign.ToString(), "MD5").ToUpper();

            return strRtn;
        }


        /// <summary>
        /// 返回微信支付所需的参数
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static string WxPayFor(string strJson)
        {
            string strRtn = "{\"appId\":\"{0}\",\"timeStamp\":\"{1}\",\"nonceStr\":\"{2}\",\"package\":\"{3}\",\"paySign\":\"{4}\",\"signType\":\"{5}\"}";

            string strAppID = CommonTool.WXParam.APP_ID;
            string strRandom = CommonTool.Common.GetRandom(32);
            string strTimestamp = CommonTool.Common.GetTimeStamp();
            string strPackage = string.Empty;
            string strSign = string.Empty;

            Dictionary<string, string> dic = CommonTool.JsonHelper.GetParms2(strJson);
            dic.Add("noncestr", strRandom);
            dic.Add("timestamp", strTimestamp);
            dic.Add("spbill_ip", HttpContext.Current.Request.UserHostAddress);
            strPackage = GetPackage(dic);
            dic.Add("package", strPackage);
            strSign = GetPaySign(dic);

            strRtn = strRtn.
                Replace("{0}", strAppID).
                Replace("{1}", strTimestamp).
                Replace("{2}", strRandom).
                Replace("{3}", strPackage).
                Replace("{4}", strSign).
                Replace("{5}", "MD5");

            return strRtn;
        }

        /// <summary>
        /// 获取JS_JDK微信认证所需参数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string WxConfig(string url)
        {
            string strRtn = "{\"appId\":\"{0}\",\"timeStamp\":\"{1}\",\"nonceStr\":\"{2}\",\"paySign\":\"{3}\"}";

            string strAppID = CommonTool.WXParam.APP_ID;
            string strTicket = GetTicket();
            string strNoncestr = CommonTool.Common.GetRandom(16);
            string strTimestamp = CommonTool.Common.GetTimeStamp();
            string string1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", strTicket, strNoncestr, strTimestamp, url);

            //SHA1加密获取签名
            string strSign = CommonTool.Common.getSHA1(string1);

            strRtn = strRtn.
                Replace("{0}", strAppID).
                Replace("{1}", strTimestamp).
                Replace("{2}", strNoncestr).
                Replace("{3}", strSign);
            return strRtn;

        }


        /// <summary>
        /// 获取订单交易状态
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetTradeState(string orderID)
        {
            string strRtn = string.Empty;

            string strAppID = CommonTool.WXParam.APP_ID;
            string strMchID = CommonTool.WXParam.MCH_ID;
            string strNoncestr = CommonTool.Common.GetRandom(32);
            string strKey = CommonTool.WXParam.API_KEY;
            string strSigm = string.Empty;

            //签名参数
            StringBuilder sbSign = new StringBuilder();
            sbSign.AppendFormat("appid={0}&", strAppID);
            sbSign.AppendFormat("mch_id={0}&", strMchID);
            sbSign.AppendFormat("nonce_str={0}&", strNoncestr);
            sbSign.AppendFormat("out_trade_no={0}&", orderID);
            sbSign.AppendFormat("key={0}", strKey);

            //MD5加密获取签名
            string strSign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sbSign.ToString(), "MD5").ToUpper();

            //获取顶单所需参数
            StringBuilder sbXmlParam = new StringBuilder();
            sbXmlParam.Append("<xml>");
            sbXmlParam.AppendFormat("<appid>{0}</appid>", strAppID);
            sbXmlParam.AppendFormat("<mch_id>{0}</mch_id>", strMchID);
            sbXmlParam.AppendFormat("<out_trade_no>{0}</out_trade_no>", orderID);
            sbXmlParam.AppendFormat("<nonce_str>{0}</nonce_str>", strNoncestr);
            sbXmlParam.AppendFormat("<sign>{0}</sign>", strSign);
            sbXmlParam.Append("</xml>");

            //获取订单内容
            string strContent = CommonTool.HttpWeb.HttpPost("https://api.mch.weixin.qq.com/pay/orderquery", sbXmlParam.ToString());

            //提取订单中的交易状态
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(strContent);
            XmlNode xmlNode1 = xmldoc.SelectSingleNode("/xml/trade_state");
            if (xmlNode1 != null)
            {
                strRtn = xmlNode1.InnerText;
            }

            return strRtn;
        }



        /// <summary>
        /// 获取全局的Access_Token
        /// </summary>
        /// <returns></returns>
        public static string GetNewToken()
        {
            string strRtn = string.Empty;

            string appid = CommonTool.WXParam.APP_ID;
            string secret = CommonTool.WXParam.APP_SECRET;

            string UrlForAccess_Token = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            UrlForAccess_Token = string.Format(UrlForAccess_Token, appid, secret);
            //CommonTool.WriteLog.Write(UrlForAccess_Token);
            string strAccess_Token = CommonTool.Common.GetHtmlFromUrl(UrlForAccess_Token);
            CommonTool.WriteLog.Write("strAccess_Token   234243======" + strAccess_Token);
            Dictionary<string, string> dic2 = CommonTool.JsonHelper.GetParms2(strAccess_Token);
            if (dic2.Keys.Contains("access_token"))
            {
                strRtn = dic2["access_token"].ToString();
            }

            return strRtn;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <param name="strToken"></param>
        /// <returns></returns>
        public static string GetTicket()
        {
            string strRtn = string.Empty;
            string strToken = string.Empty;

            //如果ticket还没有生成，或者据上次生成时间超过6000秒，重新生成
            if (string.IsNullOrEmpty(TicketModal.ticket) || ((DateTime.Now - DateTime.Now.AddMinutes(-1)).TotalSeconds) > 6000)
            {
                string urlForApi_ticket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
                strToken = GetNewToken();
                urlForApi_ticket = string.Format(urlForApi_ticket, strToken);
                string strApi_ticket = CommonTool.Common.GetHtmlFromUrl(urlForApi_ticket);
                Dictionary<string, string> dic = CommonTool.JsonHelper.GetParms2(strApi_ticket);
                if (dic.Keys.Contains("ticket"))
                {
                    //获取新的ticket并赋值到TicketModal中
                    strRtn = dic["ticket"].ToString();
                    TicketModal.ticket = strRtn;
                    TicketModal.lastTime = DateTime.Now;
                }
            }
            else
            {
                strRtn = TicketModal.ticket;
            }

            return strRtn;
        }

        public static Dictionary<string, string> GetUserInfoByOpenID(string openid)
        {
            Dictionary<string, string> dicRtn = new Dictionary<string, string>();

            string strUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            string strAccessToken = GetNewToken();
            CommonTool.WriteLog.Write("strAccessToken===== " + strAccessToken);
            strUrl = string.Format(strUrl, strAccessToken, openid);
            string strContent =Common.GetHtmlFromUrl(strUrl);
            CommonTool.WriteLog.Write("strContent===== " + strContent);
            dicRtn = JsonHelper.GetParms2(strContent);
            return dicRtn;
        }

        public static WxUser GetWxUser(string openid)
        {
            WxUser user = new WxUser();
            Dictionary<string, string> dic = WXOperate.GetUserInfoByOpenID(openid);
            if (dic != null && dic.Count > 0)
            {
                user.subscribe = dic.Keys.Contains("subscribe") ? dic["subscribe"] : "0";
                user.openid = dic["openid"];
                user.nickname = dic["nickname"];               
                user.sex = dic["sex"];
                user.language = dic["language"];
                user.city = dic["city"];
                user.province = dic["province"];
                user.country = dic["country"];
                user.headimgurl = dic["headimgurl"];
                user.subscribe_time = dic["subscribe_time"];
            }
            return user;
        }

        public static bool IsWXUser(string openid)
        {
            Dictionary<string, string> dic = WXOperate.GetUserInfoByOpenID(openid);
            if (dic.Keys.Contains("subscribe"))
            {
                string tag = dic["subscribe"];
                if (tag == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }



    }
}
