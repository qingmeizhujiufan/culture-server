using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class culture
    {
        public string id { set; get; }
        public string cityId { set; get; }
        public string cityName { set; get; }
        public string cultureType { set; get; }
        public string cultureTitle { set; get; }
        public image cultureCover { set; get; }
        public string cultureAuthor { set; get; }
        public string cultureBrief { set; get; }
        public string cultureContent { set; get; }
        public int state { set; get; }
        public int isCollect { set; get; }
        public string updator { set; get; }
        public string updatorName { set; get; }
        public string update_time { set; get; }
        public string creator { set; get; }
        public string creatorName { set; get; }
        public string typeName { set; get; }
        public string create_time { set; get; }
    }
}