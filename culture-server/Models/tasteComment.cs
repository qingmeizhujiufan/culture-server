using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class tasteComment
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string tasteId { get; set; }
        public string userId { get; set; }
        public string headimgurl { get; set; }
        public string nickname { get; set; }
        public string comment { get; set; }
        public string create_time { get; set; }
    }
}