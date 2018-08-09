using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleCity
    {
        //获取城市列表
        public DataTable queryList()
        {
            string str = @"select   id,
                                    cityName,
                                    (select COUNT(id) from dbo.c_culture as culture where culture.cityId = city.id) as cultureTotal,
                                    (select COUNT(id) from dbo.c_art as art where art.cityId = city.id) as artTotal,
                                    (select COUNT(id) from dbo.c_news as news where news.cityId = city.id) as newsTotal,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_city as city";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存城市
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_city (cityName)
                                values ('{0}')";
                str = string.Format(str, d.cityName);
            }
            else
            {
                str = @"update dbo.c_city set 
                                cityName='{1}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.cityName);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除城市
        public bool delete(string id)
        {
            string str = @"delete dbo.c_city where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
