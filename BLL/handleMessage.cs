using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleMessage
    {
        //获取用户消息列表
        public DataTable queryList(string userId)
        {
            string str = @"select   id,
                                    receiver,
                                    messageTitle,
                                    messageContent,
                                    type,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_message
                                where receiver = '{0}'";
            str = string.Format(str, userId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        } 

        //删除消息
        public bool delete(dynamic d)
        {
            string str = @"delete dbo.c_message where id='{0}'";
            str = string.Format(str, d.id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
