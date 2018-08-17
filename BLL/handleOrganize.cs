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
        public DataTable getAllOrganizeInfo(string userId)
        {
            int type =  new Common().queyrUserType(userId);
            string auth_str = string.Empty;
            if (type == 1) auth_str = @" where 1=1";
            else if (type == 2) auth_str = @" where a.id='" + userId + "' or a.pId='" + userId + "'";
            else auth_str = @" where a.id='" + userId + "'";
            string str = @"select	a.id,
                                    a.pId,
                                    a.cityId,
                                    c.cityName,
                                    a.userName,
                                    a.realName,
                                    a.type,
                                    a.typeName,
                                    CONVERT(varchar(19), a.update_time, 120) as update_time,
                                    CONVERT(varchar(19), a.create_time, 120) as create_time
                                from dbo.c_admin a
                                left join dbo.c_city c
                                on a.cityId = c.id";
            str += auth_str;
            
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
                string pId = d.pId;
                int type = new Common().queyrUserType(pId);
                if (type == 1) type = 2;
                else if (type == 2) type = 3;
                else return false;
                str = @"insert into dbo.c_admin (pId, cityId, userName, userPwd, type, typeName, realName)
                                values ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}')";
                str = string.Format(str, d.pId, d.cityId, d.userName, d.userPwd, type, d.typeName, d.realName);
            }
            else
            {
                str = @"update dbo.c_admin set 
                                cityId='{1}', 
                                userName='{2}',
                                realName='{3}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.cityId, d.userName);
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
