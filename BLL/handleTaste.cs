using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleTaste
    {
        //获取兴趣圈图片列表
        public DataTable queryList(string userId, int pageNumber, int pageSize)
        {
            string str = @" DECLARE @Start INT
                            DECLARE @End INT
                            SELECT @Start = {1}, @End = {2};

                            ;WITH TastePage AS
                            (
		                            select a.id,
                                            a.tasteCover,
                                            a.tasteBrief,
                                            (select COUNT(id)
					                            from dbo.c_taste_like as b
					                            where a.id = b.tasteId and ";
            str += (userId != null && userId != "") ? @"b.userId = '{0}'" : @"b.userId = NULL";
            str += @"  ) as isLike,
                       (select COUNT(id)
                            from dbo.c_taste_like b
                            where a.id = b.tasteId	
                       ) as likeNum,	  
                       (select COUNT(id)
                            from dbo.c_taste_comment c
                            where a.id = c.tasteId	
                       ) as commentNum,
                       a.state,
                       a.updator,
                       CONVERT(varchar(19), a.update_time, 120) as update_time,
                       a.creator,
                       u.avatar,
                       u.nickName as creatorName,
                       CONVERT(varchar(19), a.create_time, 120) as create_time,
                       ROW_NUMBER() OVER (ORDER BY a.create_time desc) AS RowNumber
                from dbo.c_taste a
                left join dbo.c_user u
                on a.creator = u.id
                where a.state=1
        )
        select id, tasteCover, tasteBrief, isLike, likeNum, commentNum, state, updator, update_time,creator, avatar, creatorName, create_time from TastePage
        where RowNumber > @Start AND RowNumber <= @End
        ORDER BY create_time desc";
            str = string.Format(str, userId, (pageNumber - 1) * pageSize, pageNumber * pageSize);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取指定状态图片数量
        public int queryTotal(int type)
        {
            string str = @"select COUNT(id) as total
                                from dbo.c_taste
                                where state={0}";
            str = string.Format(str, type);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            int total = Convert.ToInt32(dt.Rows[0]["total"].ToString());

            return total;
        }

        //获取管理兴趣圈图片列表
        public DataTable queryListByAdmin()
        {
            string str = @"select  a.id,
	                               a.tasteCover,
	                               a.tasteBrief,
	                               (select COUNT(id)
			                            from dbo.c_taste_like b
			                            where a.id = b.tasteId	
	                               ) as likeNum,	  
	                               (select COUNT(id)
			                            from dbo.c_taste_comment c
			                            where a.id = c.tasteId	
	                               ) as commentNum,
	                               a.state,
	                               a.updator,
	                               CONVERT(varchar(19), a.update_time, 120) as update_time,
	                               a.creator,
	                               u.avatar,
	                               u.nickName as creatorName,
	                               CONVERT(varchar(19), a.create_time, 120) as create_time
                            from dbo.c_taste a
                            left join dbo.c_user u
                            on a.creator = u.id
                            order by a.create_time desc";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取兴趣圈图片详情
        public DataTable queryDetail(string id)
        {
            string str = @"select  a.id,
	                               a.tasteCover,
	                               a.tasteBrief,
	                               (select COUNT(id)
			                            from dbo.c_taste_like b
			                            where a.id = b.tasteId	
	                               ) as likeNum,	  
	                               (select COUNT(id)
			                            from dbo.c_taste_comment c
			                            where a.id = c.tasteId	
	                               ) as commentNum,
	                               a.state,
	                               a.updator,
	                               CONVERT(varchar(19), a.update_time, 120) as update_time,
	                               a.creator,
                                   u.avatar,
	                               u.nickName as creatorName,
	                               CONVERT(varchar(19), a.create_time, 120) as create_time
                            from dbo.c_taste a
                            left join dbo.c_user u
                            on a.creator = u.id
                            where a.id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存兴趣圈图片
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_taste (tasteCover, tasteBrief, state, creator)
                                values ('{0}', '{1}', 0, '{2}')";
                str = string.Format(str, d.tasteCover, d.tasteBrief, d.creator);
            }
            else
            {
                str = @"update dbo.c_taste set 
                                tasteCover='{1}', 
                                tasteBrief='{2}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.tasteCover, d.tasteBrief);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除兴趣圈图片
        public bool delete(string id)
        {
            string str = @"delete dbo.c_taste where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //审核兴趣圈图片   return   0: 未找到兴趣圈图片； 1: 审核成功； 2: 审核失败; -1: 不允许审核;
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
                    str = @"update dbo.c_taste set 
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

        //收藏图片
        public bool collect(dynamic d)
        {
            string str = @"declare @id uniqueidentifier, @userId uniqueidentifier, @isExist int

                                set @id = '{0}';

                                set @userId = '{1}';

                                set @isExist = (select COUNT(id) from dbo.c_taste_like where tasteId = @id and userId = @userId);

                                if @isExist > 0

                                     delete from dbo.c_taste_like where  tasteId = @id and userId = @userId

                                else

                                     begin

                                          insert into dbo.c_taste_like (tasteId, userId) values(@id, @userId)

                                     end";
            str = string.Format(str, d.tasteId, d.userId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取TOP 10 喜欢: like |  评论: comment
        public DataTable queryRankingListTop10(string type)
        {
            string str = string.Empty;
            if(type == "like"){
                str = @"select top 10  a.id,
	                                   a.tasteCover,
	                                   a.tasteBrief,
	                                   (select COUNT(id)
			                                from dbo.c_taste_like b
			                                where a.id = b.tasteId	
	                                   ) as likeNum,	  
	                                   (select COUNT(id)
			                                from dbo.c_taste_comment c
			                                where a.id = c.tasteId	
	                                   ) as commentNum,
	                                   a.state,
	                                   a.updator,
	                                   CONVERT(varchar(19), a.update_time, 120) as update_time,
	                                   a.creator,
                                       u.avatar,
	                                   u.nickName as creatorName,
	                                   CONVERT(varchar(19), a.create_time, 120) as create_time
                                from dbo.c_taste a
                                left join dbo.c_user u
                                on a.creator = u.id
                                order by likeNum desc";
            }
            else if (type == "comment")
            {
                str = @"select top 10  a.id,
	                                   a.tasteCover,
	                                   a.tasteBrief,
	                                   (select COUNT(id)
			                                from dbo.c_taste_like b
			                                where a.id = b.tasteId	
	                                   ) as likeNum,	  
	                                   (select COUNT(id)
			                                from dbo.c_taste_comment c
			                                where a.id = c.tasteId	
	                                   ) as commentNum,
                                       a.state,
	                                   a.updator,
	                                   CONVERT(varchar(19), a.update_time, 120) as update_time,
	                                   a.creator,
                                       u.avatar,
	                                   u.nickName as creatorName,
	                                   CONVERT(varchar(19), a.create_time, 120) as create_time
                                from dbo.c_taste a
                                left join dbo.c_user u
                                on a.creator = u.id
                                order by commentNum desc";
            }
            
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //查询评论信息
        public DataTable queryCommentList(string tasteId)
        {
            string str = @"select  c.id,
	                               c.pId,
	                               c.tasteId,
                                   c.userId,
                                   u.avatar,
                                   u.userName,
                                   c.comment,
	                               CONVERT(varchar(19), c.create_time, 120) as create_time
                            from dbo.c_taste_comment c
                            left join dbo.c_user u
                            on c.userId = u.id
                            where c.tasteId = '{0}'
                            order by c.create_time desc";
            str = string.Format(str, tasteId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //新增评论
        public bool add(dynamic d)
        {
            string pId = d.pId;
            string str = string.Empty;
            if (string.IsNullOrEmpty(pId))
            {
                str = @"insert into dbo.c_taste_comment (pId, tasteId, userId, comment)
                                values (NULL, '{0}', '{1}', '{2}')";
                str = string.Format(str, d.tasteId, d.userId, d.comment);
            }
            else
            {
                str = @"insert into dbo.c_taste_comment (pId, tasteId, userId, comment)
                                values ('{0}', '{1}', '{2}', '{3}')";
                str = string.Format(str, d.pId, d.tasteId, d.userId, d.comment);
            }         

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
