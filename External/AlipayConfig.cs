using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace Com.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string partner = "";
        private static string seller_id = "";
        private static string private_key = "";
        private static string public_key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = "2088021925320907";
			
            //收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
            seller_id = "2088021925320907";

            //商户的私钥
            private_key = @"MIICeAIBADANBgkqhkiG9w0BAQEFAASCAmIwggJeAgEAAoGBAMFK0EQqZAZsWLfQ/XQm7yFDIHLkn5IYU5f9bLQK+ObfCcViWrUZHj7Uhc5z4gcUGT1y5SdIYHfucaw/T8lRlxp6vXCsIvkbVLlUfujbFZbf7fXXHrjCuF6Y2ehdYlNCBcDlssmN3hcq0xPOGwvFVsndCtPCico5eK3LYN8W5lPNAgMBAAECgYEAv8m+ulQhaWop4UGhqpVTInrbgISufCRdFWskZ8SXUWSu6kX74MMqiocOuVmsRP/BR4JwMzkSVw2lhpZPUV7+gFBuPlXGHH52FfMKJRkPPbE91haeJkeqSvnchzNU75m34ud3tA6Bgi+902X/JLbl7Gxx5hyCSFNyJxLXuMOL9jUCQQD4jR6XRE4HzmVyymacvZCA8PsFGhl1DmAqIFEEFInB4GwlFA4GclnAmnDweXBWDxJOkJruvLwHkBs+i6xVq/OnAkEAxxW/e18GA8No62i1ZNc6iTltre+ksZEONO8u16ffr5ozgCbx1S7JiA/018158qhLQcdmuaarCQXNJVeifiA7awJBAJy1HNxyjGAPmZbOVfPs8Pba+Ok2k2ZiMe9dgLb53AEI5iZahp0566yrsxlq95DK39f5BDYReNur2AGPPpClYXECQCUNKh7m13K52nv9d8z1tYI2evAlKoyVh4RYg2pFZv8CftU56kYXuBAnkBTdu/VqVt8+IiMpHqnGN6Dch3RbwVMCQQDZXqOsd4jKmu9YFvyedXRnRx/2fhayixdYozaVwstKhvf1loHtESfu7co5U38ICpg8S7YaBsvME+KsTx0pjK6H";

            //支付宝的公钥，无需修改该值
            public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Seller_id
        {
            get { return seller_id; }
            set { seller_id = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public static string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}