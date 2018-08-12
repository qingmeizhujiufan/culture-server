using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleUser
    {
        //获取用户列表
        public DataTable queryList()
        {
            string str = @"select   id,
                                    avatar,
                                    sex,
                                    userName,
                                    nickName,
                                    telephone,
                                    ISNULL(state, 0) as state,
                                    ISNULL(isDelete, 0) as isDelete,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_user 
                                where ISNULL(isDelete, 0) <> 1";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取用户详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   id,
                                    avatar,
                                    sex,
                                    userName,
                                    nickName,
                                    telephone,
                                    ISNULL(state, 0) as state,
                                    ISNULL(isDelete, 0) as isDelete,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_user 
                                where id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存用户
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_user (adCover, adTitle, adLink, state)
                                values ('{0}', '{1}', '{2}', 0)";
                str = string.Format(str, d.adCover, d.adTitle, d.adLink);
            }
            else
            {
                str = @"update dbo.c_user set 
                                adCover='{1}',
                                adTitle='{2}', 
                                adLink='{3}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.adCover, d.adTitle, d.adLink);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除用户 逻辑删除
        public bool delete(string ids)
        {
            string ids_str = "";
            string[] ids_arr = ids.Split(',');
            for (int i = 0; i < ids_arr.Length; i++)
            {
                ids = "'" + ids_arr[i] + "'";  //在每个元素前后加上我们想要的格式，效果例如：
                if (i < ids_arr.Length - 1)  //根据数组元素的个数来判断应该加多少个逗号
                {
                    ids += ",";
                }
                ids_str += ids;
            }
            string str = "update dbo.c_user set isDelete=1 where id in(" + ids_str + ")";

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取最近用户注册统计数据
        public DataTable getNewlyRegisterUserData(string type)
        {
            string str = string.Empty;
            if (type == "threeday")
            {
                str = @";with a as
                            ( select create_time=convert(varchar(10),dateadd(day,-2,getdate()),120)
                             union all select create_time=convert(varchar(10),dateadd(day,-1,getdate()),120)
                             union all select create_time=convert(varchar(10),getdate(),120)
                            )
                            select id=ROW_NUMBER()OVER(ORDER BY t1.create_time),t1.create_time as countDate,COUNT(t2.create_time) as num
                            from a t1 left join dbo.c_user t2 on DATEDIFF(day, t1.create_time, t2.create_time) = 0
                            group by t1.create_time";
            }
            else if (type == "week")
            {
                str = @";with a as
                        ( 
                             select create_time=convert(varchar(10),dateadd(day,-6,getdate()),120)
                             union all select create_time=convert(varchar(10),dateadd(day,-5,getdate()),120)
                             union all select create_time=convert(varchar(10),dateadd(day,-4,getdate()),120)
                             union all select create_time=convert(varchar(10),dateadd(day,-3,getdate()),120)
                             union all select create_time=convert(varchar(10),dateadd(day,-2,getdate()),120)
                             union all select create_time=convert(varchar(10),dateadd(day,-1,getdate()),120)
                             union all select create_time=convert(varchar(10),getdate(),120)
                        )
                        select id=ROW_NUMBER()OVER(ORDER BY t1.create_time),t1.create_time as countDate,COUNT(t2.create_time) as num
                        from a t1 left join dbo.c_user t2 on DATEDIFF(day, t1.create_time, t2.create_time) = 0
                        group by t1.create_time";
            }
            else if (type == "month")
            {
                str = @"select  id=ROW_NUMBER()OVER(ORDER BY convert(varchar(10), create_time,120)),
	                            convert(varchar(10), create_time,120) as countDate, 
	                            count(id) as num
                        from dbo.c_user 
                        WHERE (DATEPART(yy, create_time) = DATEPART(yy, GETDATE())) 
                        AND ((DATEPART(mm, create_time) = DATEPART(mm, GETDATE())) 
                        OR (31-DATEPART(DD, create_time)+DATEPART(DD, GETDATE()))<=31)
                        group by convert(varchar(10), create_time,120)";
            }
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

    }
}
