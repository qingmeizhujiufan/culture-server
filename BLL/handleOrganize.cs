using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleOrganize
    {
        //获取所有组织信息
        public DataTable getAllOrganizeInfo()
        {
            string str = @"select   id,
                                    pId,
                                    userName,
                                    type,
                                    typeName,
                                    CONVERT(varchar(19), update_time, 120) as update_time,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                               from dbo.c_admin";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }
    }
}
