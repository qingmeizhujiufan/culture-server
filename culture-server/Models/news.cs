using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class news
    {
        public string id { set; get; }
        public string newsType { set; get; }
        public string newsTitle { set; get; }
        public image newsCover { set; get; }
        public string newsAuthor { set; get; }
        public string newsBrief { set; get; }
        public string newsContent { set; get; }
        public int state { set; get; }
        public string updator { set; get; }
        public string updatorName { set; get; }
        public string update_time { set; get; }
        public string creator { set; get; }
        public string creatorName { set; get; }
        public string typeName { set; get; }
        public string create_time { set; get; }
    }
}