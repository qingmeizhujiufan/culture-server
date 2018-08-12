using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleAd
    {
        //获取广告列表
        public DataTable queryList()
        {
            string str = @"select   id,
                                    adCover,
                                    adTitle,
                                    adLink,
                                    adsense,
                                    state,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_ad ";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取广告详情
        public DataTable queryDetail(string id)
        {
            string str = @"select   id,
                                    adCover,
                                    adTitle,
                                    adLink,
                                    adsense,
                                    state,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_ad 
                                where id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取广告详情通过广告位
        public DataTable queryAdsense(string adsense)
        {
            string str = @"select   id,
                                    adCover,
                                    adTitle,
                                    adLink,
                                    adsense,
                                    state,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                                from dbo.c_ad 
                                where adsense='{0}' and state=1";
            str = string.Format(str, adsense);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存广告
        public bool saveAP(dynamic d)
        {
            string str = string.Empty;
            int flag = 0;
            string id = d.id;
            if (string.IsNullOrEmpty(id))
            {
                str = @"insert into dbo.c_ad (adCover, adTitle, adLink, 
                                    adsense, state)
                                values ('{0}', '{1}', '{2}', '{3}', 0)";
                str = string.Format(str, d.adCover, d.adTitle, d.adLink, d.adsense);
            }
            else
            {
                str = @"update dbo.c_ad set 
                                adCover='{1}',
                                adTitle='{2}', 
                                adLink='{3}',
                                adsense='{4}'
                                where id='{0}'";
                str = string.Format(str, d.id, d.adCover, d.adTitle, d.adLink, d.adsense);
            }

            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除广告
        public bool delete(string id)
        {
            string str = @"delete dbo.c_ad where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //审核广告   return   0: 未找到广告； 1: 审核成功； 2: 审核失败; -1: 不允许审核;
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
                    str = @"update dbo.c_ad set 
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
