using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool
{
    public class LogerInfo
    {
        public LogerInfo()
        {
            this.ClientInfo = new MClientInfo();
            this.UserInfo = new MUserInfo();
            this.RightInfo = new MRightInfo();
            this.ConfigInfo = new MConfigInfo();
            this.CoachInfo = new MCoachInfo();
            this.ProxyInfo = new MProxyInfo();
        }

        public MClientInfo ClientInfo;

        public MUserInfo UserInfo;

        public MRightInfo RightInfo;

        public MConfigInfo ConfigInfo;

        public MCoachInfo CoachInfo;

        public MProxyInfo ProxyInfo;
    }

    #region 内部类
    /// <summary>
    /// 客户端基本信息(驾校信息，客户机信息，授权信息)
    /// </summary>
    public class MClientInfo
    {
        //驾校表
        public string CarSchoolID;
        public string CarSchoolName;
        public string CarSchoolBoss;
        public string CarSchoolBossTel;
        public string CarSchoolCity;
        public string CarSchoolRegion;
        public string CarSchoolAddress;
        public int MsgAvaliableCount = 0;

        //授权表（暂时不用该数据）
        public DateTime DtmStart;
        public DateTime DtmEnd;
        public int LeftDays;
        public string IsTry;
    }

    /// <summary>
    /// 用户基本信息(驾校)
    /// </summary>
    public class MUserInfo
    {
        //用户表
        public string UserID;
        public string UserName;
        public string RealName;
        public string Sex;
        public string Occupation;
        public string Tel;
        public string QQ;
        public string Author;
        public string IsFrozen;
        public string Pwd;

        //用户登录记录表主键
        public string LoginRecordID = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 权限信息（驾校）
    /// </summary>
    public class MRightInfo
    {
        //授权表
        public Dictionary<string, string> DicFunctionCodeName;

        public string DataRight;
    }

    /// <summary>
    /// 配置信息（驾校）
    /// </summary>
    public class MConfigInfo
    {
        //参数表
        public string useMsg;
        public string msgOwerName;
        public string msgOwerPwd;
        public int exam1LocalDely;
        public int exam1NoLocalDely;
        public int exam2Dely;
        public int exam3Dely;
        public int exam4Dely;
        public string boy;
        public string girl;
        public string sysName;
        public string functionShowType;

        //练车相关参数
        public string useExperNight;
        public int experMoringStart;
        public int experMoringEnd;
        public int experAfterStart;
        public int experAfterEnd;
        public int experNightStart;
        public int experNightEnd;
        public string experMode;

        //后续增加
        public double getMoney;
        public double saveMoney;
        public double marketPrice;
        public int defaultPeriod;
        public string useDistribute;
    }

    public class MCoachInfo
    {
        //教练表
        public string CoachID;
        public string UserName;
        public string RealName;
        public string Sex;
        public string Occupation;
        public string Tel;
        public string QQ;
        public string Author;
        public string IsFrozen;
        public string Pwd;

        public string UserID; //所属用户

        //用户登录记录表主键
        public string LoginRecordID = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 代理人员表
    /// </summary>
    public class MProxyInfo
    {
        public string UserID;
        public string Name;
        public string UserName;
        public string Pwd;
        public string Phone;
        public string IP;
    }

    #endregion 
}
