﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleTaste
    {
        //获取兴趣圈图片列表
        public DataTable queryList()
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
	                               CONVERT(varchar(19), a.create_time, 120) as create_time
                            from dbo.c_taste a
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
	                               CONVERT(varchar(19), a.create_time, 120) as create_time
                            from dbo.c_taste a
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
                str = @"insert into dbo.c_taste (newsTitle, newsCover, newsBrief, newsContent, state, creator)
                                values ('{0}', '{1}', '{2}', '{3}', 0, '{4}')";
                str = string.Format(str, d.newsTitle, d.newsCover, d.newsBrief, d.newsContent, d.creator);
            }
            else
            {
                str = @"update dbo.c_taste set 
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
	                                   CONVERT(varchar(19), a.create_time, 120) as create_time
                                from dbo.c_taste a
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
	                                   CONVERT(varchar(19), a.create_time, 120) as create_time
                                from dbo.c_taste a
                                order by commentNum desc";
            }
            
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }
    }
}