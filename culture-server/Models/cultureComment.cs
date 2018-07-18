using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class cultureComment
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string cultureId { get; set; }
        public string userId { get; set; }
        public image avatar { get; set; }
        public string userName { get; set; }
        public string comment { get; set; }
        public string create_time { get; set; }
    }
}