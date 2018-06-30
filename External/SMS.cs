using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Data;

namespace External
{
    /// <summary>
    /// 发送短信类
    /// </summary>
    public class SMS
    {
        /// <summary>
        /// 每次只发一条短信, 客户端管理系统，给学员发送短信
        /// </summary>
        /// <param name="strPhone"></param>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string SendMsg(string strPhone, string strContent, string strSendUserID, string strReciverID, int leftMsgCount)
        {
            string strUID = CommonTool.DeEncode.Decode(CommonTool.ConfigInfo.msgOwerName);
            string strPwd = CommonTool.DeEncode.Decode(CommonTool.ConfigInfo.msgOwerPwd);
            string strurl = "http://utf8.sms.webchinese.cn/?Uid={2}&Key={3}&smsMob={0}&smsText={1}";
            strurl = string.Format(strurl, strPhone, strContent, strUID, strPwd);

            string strRtn = "1";
            int intIsTest = 1;
            if (CommonTool.ConfigInfo.useMsg == "1" && leftMsgCount > 0)
            {
                strRtn = CommonTool.Common.GetHtmlFromUrl(strurl);
                intIsTest = 0;
            }
            else
            {
                strRtn = "1";
                intIsTest = 1;
            }

            int wordCount = 32;
            int itemCount = 1;
            wordCount = strContent.Length;
            if(wordCount <= 32)
            {
                itemCount = 1;
            }
            else if (wordCount <= 64)
            {
                itemCount = 2;
            }
            else if (wordCount <= 96)
            {
                itemCount = 3;
            }
            else
            {
                itemCount = (int)Math.Ceiling(wordCount/32.00);
            }

            //真实的发送短信，并且发送成功了
            if (intIsTest == 0 && !string.IsNullOrEmpty(strRtn) && CommonTool.Common.IsNumber(strRtn) && Convert.ToInt32(strRtn) > 0)
            {
                leftMsgCount=leftMsgCount - itemCount;
            }

            DBHelper.DataModal modal = new DBHelper.DataModal();
            modal.Type = DBHelper.ExecuteType.Insert;
            modal.TableName = "dbo.SendMsgRecord";
            modal.ListFieldItem.Add(new DBHelper.FieldItem("phones", strPhone));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("content", strContent));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("returnState", strRtn));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("sendUserId", strSendUserID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("reciverID", strReciverID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgOwerName", strUID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgOwerPwd", strPwd));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("isTest", intIsTest.ToString(), CommonTool.JsonValueType.Number));

            //短信字数和短信逻辑条数
            modal.ListFieldItem.Add(new DBHelper.FieldItem("wordCount",wordCount.ToString() , CommonTool.JsonValueType.Number));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("itemCount", itemCount.ToString(), CommonTool.JsonValueType.Number));

            modal.ListFieldItem.Add(new DBHelper.FieldItem("carSchoolID", CommonTool.ClientInfo.CarSchoolID));
            //modal.ListFieldItem.Add(new DBHelper.FieldItem("clientID", CommonTool.ClientInfo.CpuID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("userID", CommonTool.UserInfo.UserID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("userName", CommonTool.UserInfo.UserName));

            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgLeft", leftMsgCount.ToString(), CommonTool.JsonValueType.Number));
            
            int intRtn = modal.Execute();

            return strRtn;
        }

        public static string SendMsg(string strPhone, string strContent, string sysName)
        {
            string strUID = CommonTool.DeEncode.Decode(CommonTool.Common.GetAppSetting("msgOwerName"));
            string strPwd = CommonTool.DeEncode.Decode(CommonTool.Common.GetAppSetting("msgOwerPwd"));
            string strurl = "http://utf8.sms.webchinese.cn/?Uid={2}&Key={3}&smsMob={0}&smsText={1}";
            strurl = string.Format(strurl, strPhone, strContent, strUID, strPwd);

            string strRtn = "1";
            int intIsTest = 1;
            if (CommonTool.Common.GetAppSetting("useMsg") == "1")
            {
                strRtn = CommonTool.Common.GetHtmlFromUrl(strurl);
                intIsTest = 0;
            }
            else
            {
                strRtn = "1";
                intIsTest = 1;
            }

            DBHelper.DataModal modal = new DBHelper.DataModal();
            modal.Type = DBHelper.ExecuteType.Insert;
            modal.TableName = "dbo.SendMsgRecord_Web";
            modal.ListFieldItem.Add(new DBHelper.FieldItem("phones", strPhone));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("content", strContent));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("returnState", strRtn));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgOwerName", strUID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgOwerPwd", strPwd));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("isTest", intIsTest.ToString(), CommonTool.JsonValueType.Number));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("appname", sysName));

            int intRtn = modal.Execute();

            return strRtn;
        }

        public static string SendMsg(string strPhone, string strContent, string strIP, string sysName)
        {
            string strUID = CommonTool.DeEncode.Decode(CommonTool.Common.GetAppSetting("msgOwerName"));
            string strPwd = CommonTool.DeEncode.Decode(CommonTool.Common.GetAppSetting("msgOwerPwd"));
            string strurl = "http://utf8.sms.webchinese.cn/?Uid={2}&Key={3}&smsMob={0}&smsText={1}";
            strurl = string.Format(strurl, strPhone, strContent, strUID, strPwd);

            string strRtn = "1";
            int intIsTest = 1;
            if (CommonTool.Common.GetAppSetting("useMsg") == "1")
            {
                strRtn = CommonTool.Common.GetHtmlFromUrl(strurl);
                intIsTest = 0;
            }
            else
            {
                strRtn = "1";
                intIsTest = 1;
            }

            DBHelper.DataModal modal = new DBHelper.DataModal();
            modal.Type = DBHelper.ExecuteType.Insert;
            modal.TableName = "dbo.SendMsgRecord_Web";
            modal.ListFieldItem.Add(new DBHelper.FieldItem("phones", strPhone));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("content", strContent));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("returnState", strRtn));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgOwerName", strUID));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("msgOwerPwd", strPwd));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("sendUserIp", strIP));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("isTest", intIsTest.ToString(), CommonTool.JsonValueType.Number));
            modal.ListFieldItem.Add(new DBHelper.FieldItem("appname", sysName));

            int intRtn = modal.Execute();

            return strRtn;
        }
    }
}
