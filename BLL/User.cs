using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BLL
{
    public class User
    {

        private Common commonBll = null;
        public User()
        {
            this.commonBll = new Common();
        }

        //获取用户列表信息
        public DataTable GetUserList()
        {
            string strSql = @"select    id,
                                        openid,
                                        ISNULL(name, '') as name,
                                        ISNULL(sex, '') as sex,
                                        ISNULL(birth, '') as birth,
                                        ISNULL(village, '') as village,
                                        ISNULL(telephone, '') as telephone,
                                        CONVERT(varchar(19) , create_time, 120 ) as create_time
                                        from dbo.wxop_user
                                        order by create_time desc";
            strSql = string.Format(strSql);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //获取用户基本信息
        public DataTable GetUserBaseInfo(string openid)
        {
            string strSql = @"select    id,
                                        openid,
                                        ISNULL(name, '') as name,
                                        ISNULL(sex, '') as sex,
                                        ISNULL(birth, '') as birth,
                                        ISNULL(village, '') as village,
                                        ISNULL(telephone, '') as telephone
                                        from dbo.wxop_user
                                        where openid = '{0}'";
            strSql = string.Format(strSql, openid);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //更新用户信息
        public bool UpdateUserInfo(string openID, string fieldName, string fieldValue)
        {
            bool bRtn = false;

            if (string.IsNullOrEmpty(openID) || string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                bRtn = false;
                return bRtn;
            }

            string strSql = "update dbo.wxop_user set {0} = '{1}' where openid = '{2}'";
            strSql = string.Format(strSql, fieldName, fieldValue, openID);
            int tag = DBHelper.SqlHelper.ExecuteSql(strSql);
            if (tag > 0)
            {
                bRtn = true;
            }

            return bRtn;
        }
        

        //获取用户地址列表
        public DataTable GetUserAddress(string openid)
        {
            string strSql = @"select    a.id,
                                        a.userId,
                                        ISNULL(a.receiver, '') as receiver,
                                        ISNULL(a.telephone, '') as telephone,
                                        ISNULL(a.province, '') as province,
                                        ISNULL(a.city, '') as city,
                                        ISNULL(a.county, '') as county,
                                        ISNULL(a.area, '') as area,
                                        ISNULL(a.isDefault, 0) as isDefault
                                        from dbo.wxop_address a
                                        left join dbo.wxop_user u
                                        on a.userId = u.id
                                        where u.openid = '{0}'";
            strSql = string.Format(strSql, openid);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //选择默认地址
        public bool ChooseDefault(string user_id, string address_id)
        {
            string strSql = @"update dbo.wxop_address set isDefault=0 
	                          where userId = '{0}';
                              update dbo.wxop_address set isDefault=1 
                              where userId = '{0}' 
                              and id = '{1}'";
            strSql = string.Format(strSql, user_id, address_id);
            int flag = DBHelper.SqlHelper.ExecuteSql(strSql);
            return flag > 0 ? true : false;
        }

        //新增新地址
        public bool addNewAddress(string userId, string receiver, string telephone, string province, string city, string county, string area)
        {
            string str = @"insert into dbo.wxop_address (userId, receiver, telephone, province, city, county, area) 
                            values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
            str = string.Format(str, userId, receiver, telephone, province, city, county, area);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //更新地址
        public bool modifyAddress(string id, string userId, string receiver, string telephone, string province, string city, string county, string area)
        {
            string str = @"update dbo.wxop_address 
                                        set 
                                            userId='{1}', 
                                            receiver='{2}', 
                                            telephone='{3}', 
                                            province='{4}', 
                                            city='{5}', 
                                            county='{6}', 
                                            area='{7}'
                                        where id='{0}'";
            str = string.Format(str, id, userId, receiver, telephone, province, city, county, area);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取地址信息
        public DataTable getAddressInfo(string id)
        {
            string str = @"select * from dbo.wxop_address where id = '{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //删除地址
        public bool delAddress(string id)
        {
            string str = @"delete from dbo.wxop_address where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //获取默认地址
        public DataTable getUserDefaultAddress(string openId)
        {
            string str = @"select  a.id,
	                               a.userId,
	                               a.receiver,
	                               a.telephone,
	                               a.province,
	                               a.city,
	                               a.county,
	                               a.area
                            from dbo.wxop_address as a
                            left join dbo.wxop_user as u
                            on a.userId = u.id
                            where u.openid = '{0}' and a.isDefault=1";
            str = string.Format(str, openId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }
    }
}
