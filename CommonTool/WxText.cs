using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CommonTool
{
    public class WxText
    {
        public static string GetWxTextXml(string StrXml)
        {
            string result = string.Empty;
            //加载xml
            XmlDocument textXml = new XmlDocument();
            textXml.LoadXml(StrXml);
            XmlElement xmlElement = textXml.DocumentElement;
            //转成model对象
            Receive_Text model = new Receive_Text()
            {
                ToUserName = xmlElement.SelectSingleNode("ToUserName").InnerText,
                FromUserName = xmlElement.SelectSingleNode("FromUserName").InnerText,
                CreateTime = xmlElement.SelectSingleNode("CreateTime").InnerText,
                MsgType = xmlElement.SelectSingleNode("MsgType").InnerText,
                Content = xmlElement.SelectSingleNode("Content").InnerText,
                MsgId = xmlElement.SelectSingleNode("MsgId").InnerText
            };
            //数据组织拼接返回xml
            //if (model.Content == "你好！")
            //{
            //    //返回的xml
            //    result = string.Format(Xml.TextMsg, model.FromUserName, model.ToUserName, DateTime.Now.Ticks, "你好！接口测试通过了！恭喜你！");
            //}
            if(model.MsgType == "event")
            {
                //string strSql = @"select SUM(id) as num from dbo.cz_user";
                //strSql = string.Format(strSql);
                //string number = 
                result = string.Format(Xml.TextMsg, model.FromUserName, model.ToUserName, DateTime.Now.Ticks, "您是第10001位会员用户，热烈欢迎！！！");
            }
            else
            {
                result = string.Format(Xml.TextMsg, model.FromUserName, model.ToUserName, DateTime.Now.Ticks, "欢迎您使用悦想商城平台~");
            }
            return result;
        }
    }
}
