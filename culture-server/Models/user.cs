using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class user
    {
        public string id { set; get; }
        public image avatar { set; get; }
        public string sex { set; get; }
        public string userName { set; get; }
        public string nickName { set; get; }
        public string telephone { set; get; }  
        public int state { set; get; }
        public int isDelete { set; get; }
        public string create_time { set; get; }
    }
}