using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CommonTool
{
    public class WXParam
    {
        //appip
        public static string APP_ID
        {
            get 
            {
                return Common.GetAppSetting("appID");
            }
        }
        //appSecret
        public static string APP_SECRET
        {
            get
            {
                return Common.GetAppSetting("appSecret");
            }
            
        }
        //商户id
        public static string MCH_ID
        {
            get
            {
                return Common.GetAppSetting("mchID");
            }

        }
        //商户密钥
        public static string API_KEY
        {
            get
            {
                return Common.GetAppSetting("apiKey");
            }
        }
        //支付回调地址
        public static string NOTIFY_URL
        {
            get
            {
                return Common.GetAppSetting("notifyUrl");
            }
        }
        //支付方式
        public static string TRADE_TYPE
        {
            get
            {
                return Common.GetAppSetting("tradeType");
            }
        }
     
        //保存获取openid
        public static string OpenID
        {
            get 
            {
                return HttpContext.Current.Session["openid"].ToString();
            }
            set 
            {
                if (Common.GetAppSetting("isTest") == "1")
                {
                    HttpContext.Current.Session["openid"] = Common.GetAppSetting("openID");
                }
                else
                {
                    HttpContext.Current.Session["openid"] = value;
                }
            }
        }

        //获取微信用户信息
        public static WxUser WxUser
        {
            get
            {
                return WXOperate.GetWxUser(OpenID);
            }
        }
    }

    public class TicketModal
    {
        public static string ticket
        {
            get;
            set;
        }

        public static DateTime lastTime
        {
            get;
            set;
        }
    }

    public class WxUser
    {
        //用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        public string subscribe;
        //用户的标识，对当前公众号唯一
        public string openid;
        //用户的昵称
        public string nickname;
        //用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        public string sex;
        //用户的语言，简体中文为zh_CN
        public string language;
        //用户所在城市
        public string city;
        //用户所在省份
        public string province;
        //用户所在国家
        public string country;
        //头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        public string headimgurl;
        //用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        public string subscribe_time;
    }
}
