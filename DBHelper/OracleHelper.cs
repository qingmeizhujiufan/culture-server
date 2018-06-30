using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

using ConfigHelper;


namespace DBHelper
{
    /// <summary>
    /// Oracle帮助类具体实现
    /// </summary>
    class OracleHelper : ISqlHelper
    {
        #region 实现接口行为部分

        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            return 0;
        }

        public string ConnectionString 
        {
            get
            {
                return ConfigHelper.ConfigHelper.DBConfigModal.CurrentConnectionString;
            }
        }

        public DataSet GetDataSet(string strSql)
        {
            DataSet ds = new DataSet();
            using (this.conn = this.GetOleDbConnection())
            {
                OleDbCommand cmd = new OleDbCommand(strSql, this.conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                try
                {
                    this.conn.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    CommonTool.WriteLog.Write(ex.Message);
                    throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    da.Dispose();
                    cmd.Dispose();
                }
            }
            return ds;
            
        }

        public DataTable GetDataTable(string strSql)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public int ExecuteSql(string strSql)
        {
            int intRtn = -1;

            return intRtn;
        }

        public string GetDataItemString(string strSql)
        {
            string strRtn = string.Empty;

            return strRtn;
        }

        public double GetDataItemDouble(string strSql)
        {
            double dblRtn = 0.00;


            return dblRtn;
        }

        public DateTime GetDataItemDateTime(string strSql)
        {
            DateTime dtmRtn = new DateTime();


            return dtmRtn;
        }

        public bool Exist(string strTableName, string strFieldName, string strValue)
        {
            bool bRtn = true;

            return bRtn;
        }

        public int Update(string tableName, string id, string idValue, string field, string fieldValue)
        {
            int intTag = 0;


            return intTag;
        }

        public int Update(string tableName, string id, string idValue, string field, int fieldValue)
        {
            int intTag = 0;

            return intTag;
        }

        public int Delete(string tableName, string id, string idValue)
        {
            int intTag = 0;

            return intTag;
        }
        #endregion 

        #region 类内部数据和方法

        protected OleDbConnection conn = null;

        protected OleDbConnection GetOleDbConnection()
        {
            this.conn = new OleDbConnection(this.ConnectionString);
            return this.conn;
        }

        #endregion 
    }
}
