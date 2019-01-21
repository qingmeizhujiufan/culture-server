using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class user
    {
        public string id { set; get; }
        public string openid { set; get; }
        public string nickname { set; get; }
        public string sex { set; get; }
        public string province { set; get; }
        public string city { set; get; }
        public string country { set; get; }
        public string headimgurl { set; get; }
        public string unionid { set; get; }
        public int state { set; get; }
        public int isDelete { set; get; }
        public string create_time { set; get; }
    }
}