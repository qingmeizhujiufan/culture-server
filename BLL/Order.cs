using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BLL
{
    public class Order
    {
        //获取订单列表信息
        public DataTable GetOrderList()
        {
            string strSql = @"select    o.id,
                                        ISNULL(o.orderNo, '') as orderNo,
                                        o.productId,
                                        p.name as productName,
                                        p.coverAttaches,
                                        p.price,
                                        o.userId,
                                        ISNULL(o.userName, '') as userName,
                                        ISNULL(o.telephone, '') as telephone,
                                        a.id,
                                        o.addressId,
                                        a.province,
                                        a.city,
                                        a.county,
                                        a.area,
                                        CONVERT(varchar(19) , o.installDate, 120 ) as installDate,
                                        ISNULL(o.installSize, '') as installSize,
                                        o.installNum,
                                        o.payMoney,
                                        o.state,
                                        CONVERT(varchar(19) , o.create_time, 120 ) as create_time
                                        from dbo.wxop_order o
                                        left join dbo.wxop_address a
                                        on o.addressId = a.id
                                        left join dbo.wxop_product p
                                        on o.productId = p.id
                                        order by o.create_time desc";
            strSql = string.Format(strSql);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //获取用户订单列表信息
        public DataTable GetUserOrderList(string userId){
            string strSql = @"select    o.id,
                                        ISNULL(o.orderNo, '') as orderNo,
                                        o.productId,
                                        p.name as productName,
                                        p.coverAttaches,
                                        CAST(ISNULL(p.price, 0) as decimal(18,2)) as price,
                                        o.userId,
                                        ISNULL(o.userName, '') as userName,
                                        ISNULL(o.telephone, '') as telephone,
                                        a.province,
                                        a.city,
                                        a.county,
                                        a.area,
                                        CONVERT(varchar(19) , o.installDate, 120 ) as installDate,
                                        ISNULL(o.installSize, '') as installSize,
                                        o.payMoney,
                                        o.state,
                                        CONVERT(varchar(19) , o.create_time, 120 ) as create_time
                                        from dbo.wxop_order o
                                        left join dbo.wxop_address a                                     
                                        on o.addressId = a.id
                                        left join dbo.wxop_product p
                                        on o.productId = p.id
                                        where o.userId = '{0}'
                                        order by o.create_time desc";
            strSql = string.Format(strSql, userId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //获取订单基本信息
        public DataTable GetOrderBaseInfo(string orderId)
        {
            string strSql = @"select    o.id,
                                        ISNULL(o.orderNo, '') as orderNo,
                                        o.productId,
                                        p.name as productName,
                                        p.coverAttaches,
                                        CAST(ISNULL(p.price, 0) as decimal(18,2)) as price,
                                        o.userId,
                                        ISNULL(o.userName, '') as userName,
                                        ISNULL(o.telephone, '') as telephone,
                                        a.province,
                                        a.city,
                                        a.county,
                                        a.area,
                                        CONVERT(varchar(19) , o.installDate, 120 ) as installDate,
                                        ISNULL(o.installSize, '') as installSize,
                                        ISNULL(o.installNum, 0) as installNum,
                                        CAST(ISNULL(o.payMoney, 0) as decimal(18,2)) as payMoney,
                                        o.state,
                                        CONVERT(varchar(19) , o.create_time, 120 ) as create_time
                                        from dbo.wxop_order o
                                        left join dbo.wxop_address a                                     
                                        on o.addressId = a.id
                                        left join dbo.wxop_product p
                                        on o.productId = p.id
                                        where o.id = '{0}'";
            strSql = string.Format(strSql, orderId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        /// <summary>
        /// 获取所有订单
        /// </summary>     
        /// <returns></returns>
        public DataTable GetAllOrder()
        {
            DataTable dt = new DataTable();
            string strSql = @"select    id,
                                        openid,
                                        telephone,
                                        amount,
                                        CAST(ISNULL(paymoney, 0) as decimal(18,2)) as paymoney,
                                        area,
                                        company,
                                        state,
                                        create_time                              
                                from dbo.cz_payorder
                                order by create_time asc ";
            strSql = string.Format(strSql);
            dt = DBHelper.SqlHelper.GetDataTable(strSql);

            return dt;
        }

        //提交订单
        public string SubmitOrder(string productId, string userId, string userName, string telephone, string addressId, string installDate, float installSize, int installNum, float payMoney)
        {
            string id = Guid.NewGuid().ToString();
            string orderNo = CommonTool.Common.CreateOrderNo("fx");
            string str = @"insert into dbo.wxop_order 
	                            (
                                    id,
                                    orderNo,
		                            productId, 
		                            userId, 
		                            userName, 
		                            telephone, 
		                            addressId, 
		                            installDate, 
		                            installSize, 
		                            installNum, 
		                            payMoney,
                                    state
	                            )
	                            values 
	                            (
		                            '{0}', 
		                            '{1}', 
		                            '{2}', 
		                            '{3}', 
		                            '{4}', 
		                            '{5}', 
		                            '{6}', 
		                            '{7}', 
		                            {8},
                                    {9},
                                    {10},
                                    0
	                            )";
            str = string.Format(str, id, orderNo, productId, userId, userName, telephone, addressId, installDate, installSize, installNum, payMoney);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);
            if (flag > 0)
                return id;
            else
                return "";
        }

        //检查订单是否存在
        public bool checkOrderIsExist(string orderId)
        {
            string str = @"select id from dbo.wxop_order where id='{0}'";

            str = string.Format(str, orderId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt.Rows.Count > 0 ? true : false;
        }

        //会写订单编号及订单状态
        public int PayOrder(string orderId)
        {       
            string str = @"update dbo.wxop_order set state=1 where id='{0}'";
            str = string.Format(str, orderId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag;
        }

        //后台完成订单
        public int CompleteOrder(string orderId)
        {
            string str = @"update dbo.wxop_order set state=2 where id='{0}'";
            str = string.Format(str, orderId);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag;
        }

        //取消订单
        public int DelOrder(string orderId)
        {
            string str = string.Empty;
            int flag = -1;
            str = @"select id, state from dbo.wxop_order where id='{0}'";
            str = string.Format(str, orderId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            if (dt.Rows.Count > 0)
            {
                int state = Convert.ToInt32(dt.Rows[0]["state"].ToString());
                if (state != 0)
                {
                    flag = 40003;
                    return flag;
                }
            }
            str = @"update dbo.wxop_order set state=-1 where id='{0}'";
            str = string.Format(str, orderId);
            flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag;
        }
    }
}
