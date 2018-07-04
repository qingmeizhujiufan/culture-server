using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class ad
    {
        public string id { set; get; }
        public image adCover { set; get; }
        public string adTitle { set; get; }
        public string adLink { set; get; }
        public int state { set; get; }
        public string create_time { set; get; }
    }
}