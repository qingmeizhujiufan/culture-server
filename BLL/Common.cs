using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using External;
using Com.Alipay;

namespace BLL
{
    public class Common
    {
        //插入阅读情况
        public void insertRead(string viewId)
        {
            try
            {
                string str = @"insert into dbo.c_read (viewId) values('{0}')";
                str = string.Format(str, viewId);
                DBHelper.SqlHelper.ExecuteSql(str);
            }
            catch (Exception e)
            {

            }
        }

        //获取首页文化展示轮播图详情
        public DataTable queryHomeCulutreDetail()
        {
            string str = @"select  slider_1,
	                               slider_2,
	                               slider_3,
                                   slider_4,
	                               slider_5,
	                               slider_6,
                                   slider_7,
	                               slider_8,
	                               slider_9
                            from dbo.c_home_culture";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //首页文化展示轮播图保存
        public bool saveHomeSlider(dynamic d)
        {
            string str = @"update dbo.c_home_culture set 
                                slider_1='{1}',
                                slider_2='{2}', 
                                slider_3='{3}',
                                slider_4='{4}',
                                slider_5='{5}', 
                                slider_6='{6}',
                                slider_7='{7}',
                                slider_8='{8}', 
                                slider_9='{9}'
                                where id={0}";
            str = string.Format(str, 1, d.slider_1, d.slider_2, d.slider_3, d.slider_4, d.slider_5, d.slider_6, d.slider_7, d.slider_8, d.slider_9);

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取背景音乐
        public DataTable queryMusic()
        {
            string str = @"select  bgMusic
                            from dbo.c_music";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //背景音乐保存
        public bool saveMusic(dynamic d)
        {
            string str = @"update dbo.c_music set 
                                bgMusic='{1}'
                                where id={0}";
            str = string.Format(str, 1, d.bgMusic);

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //总量统计
        public DataTable getWebTotal()
        {
            string str = @"select COUNT(*) from dbo.c_culture
                            union all
                            select COUNT(*) from dbo.c_art
                            union all
                            select COUNT(*) from dbo.c_taste
                            union all
                            select COUNT(*) from dbo.c_video";
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //文化，艺术品，新闻浏览分布统计
        public DataTable countCAN()
        {
            string str = @"select COUNT(*) from dbo.c_read where viewId in(select id from dbo.c_culture)
                            union all
                            select COUNT(*) from dbo.c_read where viewId in(select id from dbo.c_art)
                            union all
                            select COUNT(*) from dbo.c_read where viewId in(select id from dbo.c_news)
                            union all
                            select COUNT(*) from dbo.c_read where viewId in(select id from dbo.c_video)";
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }
    }
}
