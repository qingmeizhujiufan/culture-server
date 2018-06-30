using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool
{
    public abstract class ReceiveModel
    {

        /// 

        /// 接收方帐号（收到的OpenID）
        /// 

        public string ToUserName { get; set; }
        /// 

        /// 发送方帐号（一个OpenID）
        /// 

        public string FromUserName { get; set; }
        /// 

        /// 消息创建时间 （整型）
        /// 

        public string CreateTime { get; set; }
        /// 

        /// 消息类型
        /// 

        public string MsgType { get; set; }

        /// 

        /// 当前实体的XML字符串
        /// 

        public string Xml { get; set; }
    }

    /// 

    /// 接收普通消息-文本消息
    /// 

    public class Receive_Text : ReceiveModel
    {

        /// 

        ///  文本消息内容
        /// 

        public string Content { get; set; }

        /// 

        ///  消息id，64位整型
        /// 

        public string MsgId { get; set; }
    }

    public class Xml
    {
        #region 文本xml
        /// <summary>
        /// ToUserName:用户ID（OpenID）
        /// FromUserName：开发者
        /// CreateTime：时间
        /// Content：内容
        /// </summary>
        public static string TextMsg
        {
            get
            {
                return @"
                <xml>
                <ToUserName><![CDATA[{0}]]></ToUserName>
                <FromUserName><![CDATA[{1}]]></FromUserName> 
                <CreateTime>{2}</CreateTime>
                <MsgType><![CDATA[text]]></MsgType>
                <Content><![CDATA[{3}]]></Content>
                </xml>
                ";
            }
        }
        #endregion
    }
}
