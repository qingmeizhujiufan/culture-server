using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class video
    {
        public string id { set; get; }
        public image videoFile { set; get; }
        public string videoTitle { set; get; }
        public string videoSubTitle { set; get; }
        public image videoCover { set; get; }
        public string videoAuthor { set; get; }
        public string videoBrief { set; get; }
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