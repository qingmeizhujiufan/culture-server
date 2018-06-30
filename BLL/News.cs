using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class News
    {
        //获取新闻详情
        public DataTable GetNewsDetail(string newsId)
        {
            string str = @"select     id,
                                      news_type,
                                      news_title, 
                                      news_cover, 
                                      news_brief, 
                                      news_content, 
                                      CONVERT(varchar(19), create_time, 120) as create_time
                               from dbo.wxop_news 
                               where id='{0}'";
            str = string.Format(str, newsId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //新增新闻
        public bool AddNews(string news_type, string news_cover, string news_title, string news_brief, string news_content)
        {
            string str = @"insert into dbo.wxop_news (news_type, news_cover, news_title, news_brief, news_content)
                                values ('{0}', '{1}', '{2}', '{3}', '{4}')";
            str = string.Format(str, news_type, news_cover, news_title, news_brief, news_content);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //更新新增
        public bool EditNews(string news_type, string news_cover, string news_title, string news_brief, string news_content, string id)
        {
            string str = @"update dbo.wxop_news set news_type='{0}', news_cover='{1}', news_title='{2}', news_brief='{3}', news_content='{4}' where id='{5}'";
            str = string.Format(str, news_type, news_cover, news_title, news_brief, news_content, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除新闻
        public bool DelNews(string newsId)
        {
            string str = @"delete from dbo.wxop_news where id='{0}'";
            str = string.Format(str, newsId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
