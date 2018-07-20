using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleArt
    {
        //获取艺术品列表
        public DataTable queryList(int pageNumber, int pageSize, string conditionText, string cityId)
        {
            string conditionCity = string.IsNullOrEmpty(cityId) ? @" and 1 = 1" : @" and n.cityId = '{3}'";
            string str = @"DECLARE @Start INT
                            DECLARE @End INT
                            SELECT @Start = {0}, @End = {1};

                            ;WITH ArtPage AS
                            (select   n.id,
                                    n.cityId,
                                    c.cityName,
                                    artType,
                                    artTitle,
                                    artCover,
                                    artAuthor,
                                    artBrief,
                                    artContent,
                                    state,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time,
									ROW_NUMBER() OVER (ORDER BY a.create_time desc) AS RowNumber
                                from dbo.c_art n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where state = 1 and artTitle like '%{2}%'";
            str += conditionCity;
            str += @")
                            select id, cityId, cityName, artType, artTitle, artCover, artAuthor, artBrief, artContent, state, updator, updatorName, update_time,creator, creatorName, typeName, create_time from ArtPage
                            where RowNumber > @Start AND RowNumber <= @End
                            ORDER BY create_time desc";
            str = string.Format(str, (pageNumber - 1) * pageSize, pageNumber * pageSize, conditionText, cityId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取管理艺术品列表
        public DataTable queryListByAdmin()
        {
            string str = @"select   n.id,
                                    n.cityId,
                                    c.cityName,
                                    artType,
                                    artTitle,
                                    artCover,
                                    artAuthor,
                                    artBrief,
                                    artContent,
                                    state,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_art n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取艺术品详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   n.id,
                                    n.cityId,
                                    c.cityName,
                                    artType,
                                    artTitle,
                                    artCover,
                                    artAuthor,
                                    artBrief,
                                    artContent,
                                    state,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_art n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where n.id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存艺术品
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_art (artTitle, artCover, artBrief, artContent, cityId, state, creator)
                                values ('{0}', '{1}', '{2}', '{3}', '{4}', 0, '{5}')";
                str = string.Format(str, d.artTitle, d.artCover, d.artBrief, d.artContent, d.cityId, d.creator);
            }
            else
            {
                str = @"update dbo.c_art set 
                                artTitle='{1}', 
                                artCover='{2}', 
                                artBrief='{3}', 
                                artContent='{4}',
                                cityId='{5}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.artTitle, d.artCover, d.artBrief, d.artContent, d.cityId);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除艺术品
        public bool delete(string id)
        {
            string str = @"delete dbo.c_art where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //审核艺术品   return   0: 未找到艺术品； 1: 审核成功； 2: 审核失败; -1: 不允许审核;
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
                    str = @"update dbo.c_art set 
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

        //查询评论信息
        public DataTable queryCommentList(string artId)
        {
            string str = @"select  c.id,
	                               c.pId,
	                               c.artId,
                                   c.userId,
                                   u.avatar,
                                   u.userName,
                                   c.comment,
	                               CONVERT(varchar(19), c.create_time, 120) as create_time
                            from dbo.c_art_comment c
                            left join dbo.c_user u
                            on c.userId = u.id
                            where c.artId = '{0}'
                            order by c.create_time desc";
            str = string.Format(str, artId);
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
                str = @"insert into dbo.c_art_comment (pId, artId, userId, comment)
                                values (NULL, '{0}', '{1}', '{2}')";
                str = string.Format(str, d.artId, d.userId, d.comment);
            }
            else
            {
                str = @"insert into dbo.c_art_comment (pId, artId, userId, comment)
                                values ('{0}', '{1}', '{2}', '{3}')";
                str = string.Format(str, d.pId, d.artId, d.userId, d.comment);
            }

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
