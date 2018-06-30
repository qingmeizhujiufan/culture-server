using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Case
    {
        //获取案例列表
        public DataTable GgetCaseList()
        {
            string str = @"select id,
                                  cover_img,
                                  title,
                                  detail_img,
                                  create_time
                           from dbo.wxop_case
                           order by create_time desc";
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取案例详情
        public DataTable GetCaseDetail(string id)
        {
            string str = @"select id,
                                  cover_img,
                                  title,
                                  detail_img,
                                  create_time
                           from dbo.wxop_case
                           where id = '{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //新增新闻
        public bool AddCase(string cover_img, string title, string detail_img)
        {
            string str = @"insert into dbo.wxop_case (cover_img, title, detail_img)
                                values ('{0}', '{1}', '{2}')";
            str = string.Format(str, cover_img, title, detail_img);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //更新新增
        public bool EditCase(string cover_img, string title, string detail_img, string id)
        {
            string str = @"update dbo.wxop_case set cover_img='{0}', title='{1}', detail_img='{2}' where id='{3}'";
            str = string.Format(str, cover_img, title, detail_img, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除新闻
        public bool DelCase(string caseId)
        {
            string str = @"delete from dbo.wxop_case where id='{0}'";
            str = string.Format(str, caseId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
