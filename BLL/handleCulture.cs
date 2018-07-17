using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleCulture
    {
        //获取新闻列表
        public DataTable getNewsList(int pageNumber, int pageSize, string conditionText, string cityId)
        {
            string conditionCity = string.IsNullOrEmpty(cityId) ? @" and 1 = 1" : @" and n.cityId = '{3}'";
            string str = @"DECLARE @Start INT
                            DECLARE @End INT
                            SELECT @Start = {0}, @End = {1};

                            ;WITH NewsPage AS
                            (select   n.id,
                                    n.cityId,
                                    c.cityName,
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
                                    CONVERT(varchar(19), n.create_time, 120) as create_time,
									ROW_NUMBER() OVER (ORDER BY a.create_time desc) AS RowNumber
                                from dbo.c_news n
                                left join dbo.c_city c
                                on n.cityId = c.id
                                left join dbo.c_admin a
                                on n.creator = a.id
                                where state = 1 and newsTitle like '%{2}%'";
            str += conditionCity;
            str += @")
                            select id, cityId, cityName, newsType, newsTitle, newsCover, newsAuthor, newsBrief, newsContent, state, updator, updatorName, update_time,creator, creatorName, typeName, create_time from NewsPage
                            where RowNumber > @Start AND RowNumber <= @End
                            ORDER BY create_time desc";
            str = string.Format(str, (pageNumber - 1) * pageSize, pageNumber * pageSize, conditionText, cityId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取管理新闻列表
        public DataTable queryListByAdmin()
        {
            string str = @"select   n.id,
                                    n.cityId,
                                    c.cityName,
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
                                left join dbo.c_city c
                                on n.cityId = c.id
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
                                    n.cityId,
                                    c.cityName,
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
                                left join dbo.c_city c
                                on n.cityId = c.id
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
                                newsContent='{4}',
                                cityId='{5}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.newsTitle, d.newsCover, d.newsBrief, d.newsContent, d.cityId);
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
    }
}
