using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleCulture
    {
        //获取文化列表
        public DataTable queryList(int pageNumber, int pageSize, string conditionText, string cityId)
        {
            string conditionCity = string.IsNullOrEmpty(cityId) ? @" and 1 = 1" : @" and n.cityId = '{3}'";
            string str = @"DECLARE @Start INT
                            DECLARE @End INT
                            SELECT @Start = {0}, @End = {1};

                            ;WITH CulturePage AS
                            (select   n.id,
                                    n.cityId,
                                    c.cityName,
                                    cultureType,
                                    cultureTitle,
                                    cultureCover,
                                    cultureAuthor,
                                    cultureBrief,
                                    cultureContent,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_culture_like where n.id = cultureId) as collectNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time,
									ROW_NUMBER() OVER (ORDER BY a.create_time desc) AS RowNumber
                                from dbo.c_culture n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where state = 1 and cultureTitle like '%{2}%'";
            str += conditionCity;
            str += @")
                            select id, cityId, cityName, cultureType, cultureTitle, cultureCover, cultureAuthor, cultureBrief, cultureContent, state, readNum, collectNum, updator, updatorName, update_time,creator, creatorName, typeName, create_time from CulturePage
                            where RowNumber > @Start AND RowNumber <= @End
                            ORDER BY create_time desc";
            str = string.Format(str, (pageNumber - 1) * pageSize, pageNumber * pageSize, conditionText, cityId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取管理文化列表
        public DataTable queryListByAdmin()
        {
            string str = @"select   n.id,
                                    n.cityId,
                                    c.cityName,
                                    cultureType,
                                    cultureTitle,
                                    cultureCover,
                                    cultureAuthor,
                                    cultureBrief,
                                    cultureContent,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_culture_like where n.id = cultureId) as collectNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_culture n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取文化详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   n.id,
                                    n.cityId,
                                    c.cityName,
                                    cultureType,
                                    cultureTitle,
                                    cultureCover,
                                    cultureAuthor,
                                    cultureBrief,
                                    cultureContent,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_culture_like where n.id = cultureId) as collectNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_culture n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where n.id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存文化
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_culture (cultureTitle, cultureCover, cultureBrief, cultureContent, cityId, state, creator)
                                values ('{0}', '{1}', '{2}', '{3}', '{4}', 0, '{5}')";
                str = string.Format(str, d.cultureTitle, d.cultureCover, d.cultureBrief, d.cultureContent, d.cityId, d.creator);
            }
            else
            {
                str = @"update dbo.c_culture set 
                                cultureTitle='{1}', 
                                cultureCover='{2}', 
                                cultureBrief='{3}', 
                                cultureContent='{4}',
                                cityId='{5}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.cultureTitle, d.cultureCover, d.cultureBrief, d.cultureContent, d.cityId);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除文化
        public bool delete(dynamic d)
        {
            string id = d.id;
            string str = @"delete dbo.c_culture where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除用户收藏的文化
        public bool delete2(dynamic d)
        {
            string id = d.id;
            string str = @"delete dbo.c_culture_like where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //审核文化   return   0: 未找到文化； 1: 审核成功； 2: 审核失败; -1: 不允许审核;
        public int review(dynamic d)
        {
            string str = string.Empty;
            string id = d.id;
            DataTable dt = queryDetail(id);
            if (dt.Rows.Count == 1)
            {
                int state = Convert.ToInt32(dt.Rows[0]["state"].ToString());
                if (state == 0)
                {
                    str = @"update dbo.c_culture set 
                                state=1
                                where id='{0}'";
                    str = string.Format(str, d.id);
                    int flag = DBHelper.SqlHelper.ExecuteSql(str);
                    if (flag > 0) return 1;
                    else return 2;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }

        //收藏文化
        public bool collect(dynamic d)
        {
            string str = @"declare @id uniqueidentifier, @userId uniqueidentifier, @isExist int

                                set @id = '{0}';

                                set @userId = '{1}';

                                set @isExist = (select COUNT(id) from dbo.c_culture_like where cultureId = @id and userId = @userId);

                                if @isExist > 0

                                     delete from dbo.c_culture_like where cultureId = @id and userId = @userId

                                else

                                     begin

                                          insert into dbo.c_culture_like (cultureId, userId) values(@id, @userId)

                                     end";
            str = string.Format(str, d.cultureId, d.userId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取用户收藏文化
        public DataTable queryUserCollectCulture(string userId)
        {
            string str = @"select   l.id,
                                    n.cityId,
                                    c.cityName,
                                    cultureType,
                                    cultureTitle,
                                    cultureCover,
                                    cultureAuthor,
                                    cultureBrief,
                                    cultureContent,
                                    state,
                                    (select COUNT(id) from dbo.c_culture_like where n.id = cultureId) as likeNum,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_culture_like l
                                left join dbo.c_culture n
                                on l.cultureId = n.id
                                left join dbo.c_city c
                                on n.cityId = c.id
                                where l.userId = '{0}'";
            str = string.Format(str, userId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            //添加一新列，其值为默认值
            DataColumn dc = new DataColumn("collectNum", typeof(int));
            dc.DefaultValue = 1;
            DataColumn dc2 = new DataColumn("creatorName", typeof(string));
            DataColumn dc3 = new DataColumn("typeName", typeof(string));

            dt.Columns.Add(dc);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);

            return dt;
        }

        //查询评论信息
        public DataTable queryCommentList(string cultureId)
        {
            string str = @"select  c.id,
	                               c.pId,
	                               c.cultureId,
                                   c.userId,
                                   u.avatar,
                                   u.userName,
                                   c.comment,
	                               CONVERT(varchar(19), c.create_time, 120) as create_time
                            from dbo.c_culture_comment c
                            left join dbo.c_user u
                            on c.userId = u.id
                            where c.cultureId = '{0}'
                            order by c.create_time desc";
            str = string.Format(str, cultureId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //查询管理评论信息
        public DataTable queryAdminCommentList()
        {
            string str = @"select  c.id,
	                               c.pId,
	                               c.cultureId,
                                   c.userId,
                                   u.avatar,
                                   u.userName,
                                   c.comment,
	                               CONVERT(varchar(19), c.create_time, 120) as create_time
                            from dbo.c_culture_comment c
                            left join dbo.c_user u
                            on c.userId = u.id
                            order by c.create_time desc";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存评论
        public bool add(dynamic d)
        {
            string pId = d.pId;
            string str = string.Empty;
            if (string.IsNullOrEmpty(pId))
            {
                str = @"insert into dbo.c_culture_comment (pId, cultureId, userId, comment)
                                values (NULL, '{0}', '{1}', '{2}')";
                str = string.Format(str, d.cultureId, d.userId, d.comment);
            }
            else
            {
                str = @"insert into dbo.c_culture_comment (pId, cultureId, userId, comment)
                                values ('{0}', '{1}', '{2}', '{3}')";
                str = string.Format(str, d.pId, d.cultureId, d.userId, d.comment);
            }

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //评论批量删除
        public bool deleteComment(string ids)
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
            string str = "delete dbo.c_culture_comment where id in(" + ids_str + ")";

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
