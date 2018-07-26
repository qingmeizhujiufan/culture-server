using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class message
    {
        public string id { set; get; }
        public string receiver { set; get; }
        public string messageTitle { set; get; }
        public string messageContent { set; get; }
        public int type { set; get; }
        public string create_time { set; get; }
    }
}