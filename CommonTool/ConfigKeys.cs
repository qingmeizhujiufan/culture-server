using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace CommonTool
{
    /// <summary>
    /// 客户端基本信息(驾校信息，客户机信息，授权信息)
    /// </summary>
    public class ClientInfo
    {
        //驾校表
        public static string CarSchoolID { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolID; } }
        public static string CarSchoolName { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolName; } }
        public static string CarSchoolBoss { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolBoss; } }
        public static string CarSchoolBossTel { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolBossTel; } }
        public static string CarSchoolCity { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolCity; } }
        public static string CarSchoolRegion { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolRegion; } }
        public static string CarSchoolAddress { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.CarSchoolAddress; } }
        public static int MsgAvaliableCount { get { return HttpContext.Current.Session["logerInfo"] == null ? 0 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ClientInfo.MsgAvaliableCount; } }

        //授权表（暂时不用该数据）
        public static DateTime DtmStart;
        public static DateTime DtmEnd;
        public static int LeftDays;
        public static string IsTry;
    }

    /// <summary>
    /// 用户基本信息(驾校)
    /// </summary>
    public class UserInfo
    {
        //用户表
        public static string UserID { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.UserID; } }
        public static string UserName { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.UserName; } }
        public static string RealName { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.RealName; } }
        public static string Sex { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.Sex; } }
        public static string Occupation { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.Occupation; } }
        public static string Tel { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.Tel; } }
        public static string QQ { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.QQ; } }
        public static string Author { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.Author; } }
        public static string IsFrozen { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.IsFrozen; } }
        public static string Pwd { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.Pwd; } }

        //用户登录记录表主键
        public static string LoginRecordID { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).UserInfo.LoginRecordID; } }
    }

    /// <summary>
    /// 权限信息（驾校）
    /// </summary>
    public class RightInfo
    {
        //授权表
        public static Dictionary<string, string> DicFunctionCodeName { get { return HttpContext.Current.Session["logerInfo"] == null ? new Dictionary<string, string>() : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).RightInfo.DicFunctionCodeName; } }

        public static string DataRight { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).RightInfo.DataRight; } }

        public static string GetDataRightCondtion(string strField)
        {
            //数据权限控制
            string strDataRight = " (2=2) ";
            if (!string.IsNullOrEmpty(CommonTool.RightInfo.DataRight))
            {
                strDataRight = string.Format(" {1} in ({0})", CommonTool.RightInfo.DataRight, strField);
            }
            return strDataRight;
        }
        
        public static bool ExistFunction(string strCode)
        {
            bool bRtn = false;
            if(DicFunctionCodeName.Keys.Contains(strCode))
            {
                bRtn = true;
            }
            else
            {
                bRtn = false;
            }

            return bRtn;
        }

    }

    /// <summary>
    /// 配置信息（驾校）
    /// </summary>
    public class ConfigInfo
    {
        //参数表
        public static string useMsg { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.useMsg; } }
        public static string msgOwerName { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.msgOwerName; } }
        public static string msgOwerPwd { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.msgOwerPwd; } }
        public static int exam1LocalDely { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.exam1LocalDely; } }
        public static int exam1NoLocalDely { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.exam1NoLocalDely; } }
        public static int exam2Dely { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.exam2Dely; } }
        public static int exam3Dely { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.exam3Dely; } }
        public static int exam4Dely { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.exam4Dely; } }
        public static string boy { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.boy; } }
        public static string girl { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.girl; } }
        public static string sysName { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.sysName; } }
        public static string functionShowType { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.functionShowType; } }

        //练车相关参数
        public static string useExperNight { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.useExperNight; } }
        public static int experMoringStart { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experMoringStart; } }
        public static int experMoringEnd { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experMoringEnd; } }
        public static int experAfterStart { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experAfterStart; } }
        public static int experAfterEnd { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experAfterEnd; } }
        public static int experNightStart { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experNightStart; } }
        public static int experNightEnd { get { return HttpContext.Current.Session["logerInfo"] == null ? 1 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experNightEnd; } }
        public static string experMode { get { return HttpContext.Current.Session["logerInfo"] == null ? "" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.experMode; } }

        //后续增加
        public static double getMoney { get { return HttpContext.Current.Session["logerInfo"] == null ? 50 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.getMoney; } }
        public static double saveMoney { get { return HttpContext.Current.Session["logerInfo"] == null ? 50 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.saveMoney; } }
        public static double marketPrice { get { return HttpContext.Current.Session["logerInfo"] == null ? 3800 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.marketPrice; } }
        public static int defaultPeriod { get { return HttpContext.Current.Session["logerInfo"] == null ? 36 : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.defaultPeriod; } }
        public static string useDistribute { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ConfigInfo.useDistribute; } }
    }

    public class CoachInfo
    {
        //教练表
        public static string CoachID { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.CoachID; } }
        public static string UserName { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.UserName; } }
        public static string RealName { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.RealName; } }
        public static string Sex { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.Sex; } }
        public static string Occupation { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.Occupation; } }
        public static string Tel { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.Tel; } }
        public static string QQ { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.QQ; } }
        public static string Author { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.Author; } }
        public static string IsFrozen { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.IsFrozen; } }
        public static string Pwd { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.Pwd; } }

        public static string UserID { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.UserID; } }

        //用户登录记录表主键
        public static string LoginRecordID { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).CoachInfo.LoginRecordID; } }
    }

    public class ProxyInfo
    {
        //代理表
        public static string UserID { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ProxyInfo.UserID; } }
        public static string UserName { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ProxyInfo.UserName; } }
        public static string Name { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ProxyInfo.Name; } }
        public static string Pwd { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ProxyInfo.Pwd; } }
        public static string Phone { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ProxyInfo.Phone; } }
        public static string IP { get { return HttpContext.Current.Session["logerInfo"] == null ? "0" : ((LogerInfo)HttpContext.Current.Session["logerInfo"]).ProxyInfo.IP; } }
    }
}
