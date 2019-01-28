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
                                    ISNULL(artMoney, 0) artMoney,
                                    buyUrl,
                                    artBrief,
                                    artContent,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_art_like where n.id = artId) as collectNum,
                                    ISNULL(isRecommend, 0) isRecommend,
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
                            select id, cityId, cityName, artType, artTitle, artCover, artAuthor, artMoney, buyUrl, artBrief, artContent, state, readNum, collectNum, isRecommend, updator, updatorName, update_time,creator, creatorName, typeName, create_time from ArtPage
                            where RowNumber > @Start AND RowNumber <= @End
                            ORDER BY create_time desc";
            str = string.Format(str, (pageNumber - 1) * pageSize, pageNumber * pageSize, conditionText, cityId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取管理艺术品列表
        public DataTable queryListByAdmin(string userId)
        {
            int type = new Common().queyrUserType(userId);
            string auth_str = string.Empty;
            if (type == 1) auth_str = @" where 1=1";
            else if (type == 2) auth_str = @" where n.creator='" + userId + "' or n.creator in (select id from dbo.c_admin where pId='" + userId + "')";
            else auth_str = @" where a.id='" + userId + "'";
            string str = @"select   n.id,
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
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_art_like where n.id = artId) as collectNum,
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
                                on n.creator = a.id";
            str += auth_str;
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
                                    ISNULL(artMoney, 0) artMoney,
                                    buyUrl,
                                    artBrief,
                                    artContent,
                                    state,
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_art_like where n.id = artId) as collectNum,
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
                str = @"insert into dbo.c_art (artTitle, artCover, artBrief, artContent, cityId, state, creator, artMoney, buyUrl)
                                values ('{0}', '{1}', '{2}', '{3}', '{4}', 0, '{5}', {6}, '{7}')";
                str = string.Format(str, d.artTitle, d.artCover, d.artBrief, d.artContent, d.cityId, d.creator, d.artMoney, d.buyUrl);
            }
            else
            {
                str = @"update dbo.c_art set 
                                artTitle='{1}', 
                                artCover='{2}', 
                                artBrief='{3}', 
                                artContent='{4}',
                                cityId='{5}',
                                artMoney={6},
                                buyUrl='{7}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.artTitle, d.artCover, d.artBrief, d.artContent, d.cityId, d.artMoney, d.buyUrl);
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

        //删除用户收藏的艺术品
        public bool delete2(dynamic d)
        {
            string artId = d.artId;
            string userId = d.userId;
            string str = @"delete dbo.c_art_like where artId='{0}' and userId='{1}'";
            str = string.Format(str, artId, userId);
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

        //收藏艺术品
        public bool collect(dynamic d)
        {
            string str = @"declare @id uniqueidentifier, @userId uniqueidentifier, @isExist int

                                set @artId = '{0}';

                                set @userId = '{1}';

                                set @isExist = (select COUNT(id) from dbo.c_art_like where artId = @artId and userId = @userId);

                                if @isExist > 0

                                     delete from dbo.c_art_like where artId = @artId and userId = @userId

                                else

                                     begin

                                          insert into dbo.c_art_like (artId, userId) values(@artId, @userId)

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
                                    (select COUNT(id) from dbo.c_read where n.id = viewId) as readNum,
                                    (select COUNT(*) from dbo.c_art_like where n.id = artId) as collectNum,
                                    ISNULL(isRecommend, 0) isRecommend,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.nickname as creatorName,
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
            string str = @"select   n.id,
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
            DataColumn dc = new DataColumn("collectNum", typeof(int));
            dc.DefaultValue = 1;
            DataColumn dc2 = new DataColumn("creator", typeof(string));
            DataColumn dc3 = new DataColumn("creatorName", typeof(string));
            DataColumn dc4 = new DataColumn("readNum", typeof(int));
            dc4.DefaultValue = 1;

            dt.Columns.Add(dc);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);

            return dt;
        }

        //查询评论信息
        public DataTable queryCommentList(string artId)
        {
            string str = @"select  c.id,
	                               c.pId,
	                               c.artId,
                                   c.userId,
                                   u.headimgurl,
                                   u.nickname,
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

        //查询管理评论信息
        public DataTable queryAdminCommentList(string userId)
        {
            int type = new Common().queyrUserType(userId);
            string auth_str = string.Empty;
            if (type == 1) auth_str = @" where 1=1";
            else if (type == 2) auth_str = @" where c.artId in(
								select id 
								from dbo.c_art cu
								where cu.creator='" + userId + "'or cu.creator in(select id from dbo.c_admin where pId='" + userId + "'))";
            else auth_str = @" where c.artId in(
								select id 
								from dbo.c_art cu
								where cu.creator='" + userId + "')";
            string str = @"select  c.id,
	                               c.pId,
	                               c.artId,
                                   c.userId,
                                   u.headimgurl,
                                   u.nickname,
                                   c.comment,
	                               CONVERT(varchar(19), c.create_time, 120) as create_time
                            from dbo.c_art_comment c
                            left join dbo.c_user u
                            on c.userId = u.id";
            str += auth_str;
            str += " order by c.create_time desc";
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
            string str = "delete dbo.c_art_comment where id in(" + ids_str + ")";

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
