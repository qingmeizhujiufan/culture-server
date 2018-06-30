using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using ConfigHelper;


namespace DBHelper
{
    public class SqlHelper
    {
        private static ISqlHelper IHelper;
        static SqlHelper()
        {
            switch (ConfigHelper.ConfigHelper.DBConfigModal.CurrentDbType)
            {
                case DBType.SqlServer:
                    IHelper = new SqlServerHelper();
                    break;
                case DBType.Oracle:
                    IHelper = new OracleHelper();
                    break;
                case DBType.MySql:
                    IHelper = new MySqlHelper();
                    break;
                case DBType.Access:
                    IHelper = new AccessHelper();
                    break;
            }
        }
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            return IHelper.ExecuteSqlTran(SQLStringList);
        }
        public static string ConnectionString
        {
            get
            {
                return IHelper.ConnectionString;
            }
        }

        public static DataSet GetDataSet(string strSql)
        {
            return IHelper.GetDataSet(strSql);
        }

        public static DataTable GetDataTable(string strSql)
        {
            return IHelper.GetDataTable(strSql);
        }

        public static int ExecuteSql(string strSql)
        {
            return IHelper.ExecuteSql(strSql);
        }

        public static string GetDataItemString(string strSql)
        {
            return IHelper.GetDataItemString(strSql);
        }

        public static double GetDataItemDouble(string strSql)
        {
            return IHelper.GetDataItemDouble(strSql);
        }

        public static DateTime GetDataItemDateTime(string strSql)
        {
            return IHelper.GetDataItemDateTime(strSql);
        }

        public static bool Exist(string strTableName, string strFieldName, string strValue)
        {
            return IHelper.Exist(strTableName, strFieldName, strValue);
        }
    }
}
