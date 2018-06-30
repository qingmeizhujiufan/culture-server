using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace ConfigHelper
{
    /// <summary>
    /// 数据库配置数据模型
    /// </summary>
    public class DBConfigModal
    {
        public static string strFileName = "DBHelper.txt";

        public List<DBConnectionStringNode> DBConnectionNodes
        {
            get;
            set;
        }
        public string CurrentConnectionString
        {
            get;
            set;
        }
        public DBType CurrentDbType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 数据库连接字符串节点模型
    /// </summary>
    public class DBConnectionStringNode
    {
        public string Name
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
        public bool IsCurrentUse
        {
            get;
            set;
        }
        public DBType DbType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        SqlServer = 1,
        Oracle,
        MySql,
        Access
    }
}
