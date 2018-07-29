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
        public void insertRead(string viewId){
            try
            {
                string str = @"insert into dbo.c_read (viewId) values('{0}')";
                str = string.Format(str, viewId);
                DBHelper.SqlHelper.ExecuteSql(str);
            }catch(Exception e) {

            }       
        }
    }
}
