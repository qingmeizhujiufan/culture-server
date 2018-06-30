using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class DataModal
    {
        public class Delivery
        {
            public string delivery_address { get; set; }
            public string start_number { get; set; }
            public string start_price { get; set; }
            public string add_number { get; set; }
            public string add_price { get; set; }

        }
    }

    public enum GoodsState
    {
        下架 = 0,
        上架 = 1
    }

    public enum OrderState
    {
        待付款 = 0,
        已付款 = 1,
        待发货 = 2,
        已发货 = 3,
        已签收 = 4,
        待评论 = 5,
        已评论 = 6,
        已取消 = 7,
        付款失败 = 8

    }
}
