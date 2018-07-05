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
                                    adCover,
                                    adTitle,
                                    adLink,
                                    state,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_user ";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取用户详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   id,
                                    adCover,
                                    adTitle,
                                    adLink,
                                    state,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
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
        public bool delete(string id)
        {
            string str = @"update dbo.c_user set isDelete=1 where id='{0}'";
            str = string.Format(str, id);
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

            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

    }
}
