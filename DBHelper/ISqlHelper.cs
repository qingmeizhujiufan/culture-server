using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DBHelper
{
    /// <summary>
    /// 为数据处理层提供统一接口
    /// </summary>
    public interface ISqlHelper
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        string ConnectionString
        {
            get;
        }
        /// <summary>
         /// 执行多条SQL语句，实现数据库事务。
         /// </summary>
         /// <param name="SQLStringList">多条SQL语句</param>        
        int ExecuteSqlTran(List<String> SQLStringList);
        /// <summary>
        /// 根据传入的sql语句获取数据集
        /// </summary>
        /// <param name="strSql">传入的sql语句</param>
        /// <returns>数据集</returns>
        DataSet GetDataSet(string strSql);

        /// <summary>
        /// 根据传入的sql语句获取DataTable
        /// </summary>
        /// <param name="strSql">传入的sql语句</param>
        /// <returns>DataTable</returns>
        DataTable GetDataTable(string strSql);

        /// <summary>
        /// 执行sql命令，主要完成 增，删，改 功能
        /// </summary>
        /// <param name="strSql">传入的sql语句</param>
        /// <returns>受影响的行数</returns>
        int ExecuteSql(string strSql);

        /// <summary>
        /// 获取第一行第一列单元格的数据（字符串类型）
        /// </summary>
        /// <param name="strSql">传入的sql语句</param>
        /// <returns></returns>
        string GetDataItemString(string strSql);

        /// <summary>
        /// 获取第一行第一列单元格的数据（数字类型）
        /// </summary>
        /// <param name="strSql">传入的sql语句</param>
        /// <returns></returns>
        double GetDataItemDouble(string strSql);

        /// <summary>
        /// 获取第一行第一列单元格的数据（日期时间类型）
        /// </summary>
        /// <param name="strSql">传入的sql语句</param>
        /// <returns></returns>
        DateTime GetDataItemDateTime(string strSql);

        /// <summary>
        /// 判断表中是否存在某个信息
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strFieldName"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        bool Exist(string strTableName, string strFieldName, string strValue);

        /// <summary>
        /// 修改某个表中某个字段的值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <param name="idValue"></param>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        int Update(string tableName, string id, string idValue, string field, string fieldValue);
        int Update(string tableName, string id, string idValue, string field, int fieldValue);

        /// <summary>
        /// 删除某个表中的某条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <param name="idValue"></param>
        /// <returns></returns>
        int Delete(string tableName, string id, string idValue);

        //函数和存储过程的接口待处理

    }
}
