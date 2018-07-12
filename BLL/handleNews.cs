using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleNews
    {
        //获取新闻列表
        public DataTable getNewsList()
        {
            string str = @"select   n.id,
                                    newsType,
                                    newsTitle,
                                    newsCover,
                                    newsAuthor,
                                    newsBrief,
                                    newsContent,
                                    state,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_news n
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where state = 1";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取管理新闻列表
        public DataTable queryListByAdmin()
        {
            string str = @"select   n.id,
                                    newsType,
                                    newsTitle,
                                    newsCover,
                                    newsAuthor,
                                    newsBrief,
                                    newsContent,
                                    state,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_news n
                                left join dbo.c_admin a
                                on n.creator = a.id";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取新闻详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   n.id,
                                    newsType,
                                    newsTitle,
                                    newsCover,
                                    newsAuthor,
                                    newsBrief,
                                    newsContent,
                                    state,
                                    updator,
                                    updatorName,
                                    CONVERT(varchar(19), n.update_time, 120) as update_time,
                                    creator,
                                    a.userName as creatorName,
                                    a.typeName,
                                    CONVERT(varchar(19), n.create_time, 120) as create_time
                                from dbo.c_news n
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where n.id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存新闻
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_news (newsTitle, newsCover, newsBrief, newsContent, state, creator)
                                values ('{0}', '{1}', '{2}', '{3}', 0, '{4}')";
                str = string.Format(str, d.newsTitle, d.newsCover, d.newsBrief, d.newsContent, d.creator);
            }
            else
            {
                str = @"update dbo.c_news set 
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

        //删除新闻
        public bool delete(string id)
        {
            string str = @"delete dbo.c_news where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //审核新闻   return   0: 未找到新闻； 1: 审核成功； 2: 审核失败; -1: 不允许审核;
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
                    str = @"update dbo.c_news set 
                                state=1
                                where id='{0}'";
                    str = string.Format(str, d.id);
                    int flag = DBHelper.SqlHelper.ExecuteSql(str);
                    if (flag > 0) return 1;
                    else return 2;
                }
                else {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
