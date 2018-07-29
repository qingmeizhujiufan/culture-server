using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleVideo
    {
        //获取视频列表
        public DataTable queryList(int pageNumber, int pageSize, string conditionText)
        {
            string str = @"DECLARE @Start INT
                            DECLARE @End INT
                            SELECT @Start = {0}, @End = {1};

                            ;WITH VideoPage AS
                            (select   n.id,
                                    videoFile,
                                    videoTitle,
                                    videoSubTitle,
                                    videoCover,
                                    videoBrief,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time,
									ROW_NUMBER() OVER (ORDER BY a.create_time desc) AS RowNumber
                                from dbo.c_video n
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where state = 1 and videoTitle like '%{2}%'
                            )
                            select id, videoFile, videoTitle, videoSubTitle, videoCover, videoBrief, state, readNum, updator, updatorName, update_time,creator, creatorName, typeName, create_time from VideoPage
                            where RowNumber > @Start AND RowNumber <= @End
                            ORDER BY create_time desc";
            str = string.Format(str, (pageNumber - 1) * pageSize, pageNumber * pageSize, conditionText);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取管理视频列表
        public DataTable queryListByAdmin()
        {
            string str = @"select   n.id,
                                    videoFile,
                                    videoTitle,
                                    videoSubTitle,
                                    videoCover,
                                    videoBrief,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_video n
                                left join dbo.c_admin a
                                on n.creator = a.id";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取视频详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   n.id,
                                    videoFile,
                                    videoTitle,
                                    videoSubTitle,
                                    videoCover,
                                    videoBrief,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_video n
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where n.id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存视频
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_video (videoFile, videoTitle, videoCover, videoBrief, state, creator)
                                values ('{0}', '{1}', '{2}', '{3}', 0, '{4}')";
                str = string.Format(str, d.videoFile, d.videoTitle, d.videoCover, d.videoBrief, d.creator);
            }
            else
            {
                str = @"update dbo.c_video set 
                                videoFile='{1}',
                                videoTitle='{2}', 
                                videoCover='{3}',  
                                videoBrief='{4}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.videoFile, d.videoTitle, d.videoCover, d.videoBrief);
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

        //审核视频   return   0: 未找到视频； 1: 审核成功； 2: 审核失败; -1: 不允许审核;
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
                    str = @"update dbo.c_video set 
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

        //收藏艺术品
        public bool collect(dynamic d)
        {
            string str = @"declare @id uniqueidentifier, @userId uniqueidentifier, @isExist int

                                set @id = '{0}';

                                set @userId = '{1}';

                                set @isExist = (select COUNT(id) from dbo.c_art_like where artId = @id and userId = @userId);

                                if @isExist > 0

                                     delete from dbo.c_art_like where artId = @id and userId = @userId

                                else

                                     begin

                                          insert into dbo.c_art_like (artId, userId) values(@id, @userId)

                                     end";
            str = string.Format(str, d.artId, d.userId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //是否设置艺术品为推荐
        public bool settingRecommend(dynamic d)
        {
            string str = @"update dbo.c_art set 
                                isRecommend= {1},
                                update_time='{2}'
                                where id='{0}'";
            str = string.Format(str, d.id, d.isRecommend, System.DateTime.Now);

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取Top3 推荐
        public DataTable queryRecommendTop3()
        {
            string str = @"select top 3   n.id,
                                    n.cityId,
                                    c.cityName,
                                    artType,
                                    artTitle,
                                    artCover,
                                    artAuthor,
                                    ISNULL(artMoney, 0) artMoney,
                                    buyUrl,
                                    artBrief,
                                    artContent,
                                    state,
                                    (select COUNT(*) from dbo.c_art_like l where n.id = l.artId) as isCollect,
                                    ISNULL(isRecommend, 0) isRecommend,
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
                                where n.state = 1 and n.isRecommend = 1
                                order by n.update_time desc";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取用户收藏艺术品
        public DataTable queryUserCollectArt(string userId)
        {
            string str = @"select   l.id,
                                    n.cityId,
                                    c.cityName,
                                    artType,
                                    artTitle,
                                    artCover,
                                    artAuthor,
                                    ISNULL(artMoney, 0) artMoney,
                                    buyUrl,
                                    artBrief,
                                    artContent,
                                    state,
                                    ISNULL(isRecommend, 0) isRecommend,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_art_like l
                                left join dbo.c_art n
                                on l.artId = n.id
                                left join dbo.c_city c
                                on n.cityId = c.id
                                where l.userId = '{0}'";
            str = string.Format(str, userId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            //添加一新列，其值为默认值
            DataColumn dc = new DataColumn("isCollect", typeof(int));
            dc.DefaultValue = 1;
            DataColumn dc2 = new DataColumn("creator", typeof(string));
            DataColumn dc3 = new DataColumn("creatorName", typeof(string));

            dt.Columns.Add(dc);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);

            return dt;
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
