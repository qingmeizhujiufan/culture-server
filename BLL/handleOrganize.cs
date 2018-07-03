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

        //保存管理员信息
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_admin (pId, userName, userPwd, type, typeName)
                                values ('{0}', '{1}', '{2}', 1, '{3}')";
                str = string.Format(str, d.pId, d.userName, d.userPwd, d.typeName);
            }
            else
            {
                str = @"update dbo.c_admin set 
                                newsTitle='{1}', 
                                newsCover='{2}', 
                                newsBrief='{3}', 
                                newsContent='{4}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.newsTitle, d.newsCover, d.newsBrief, d.newsContent);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除管理员
        public bool delete(string id)
        {
            string str = @"delete dbo.c_admin where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
