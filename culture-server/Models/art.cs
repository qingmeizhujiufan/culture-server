using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class art
    {
        public string id { set; get; }
        public string cityId { set; get; }
        public string cityName { set; get; }
        public string artType { set; get; }
        public string artTitle { set; get; }
        public List<image> artCover { set; get; }
        public string artAuthor { set; get; }
        public Single artMoney { set; get; }
        public string buyUrl { set; get; }
        public string artBrief { set; get; }
        public List<image> artContent { set; get; }
        public int state { set; get; }
        public int isCollect { set; get; }
        public int isRecommend { set; get; }
        public string updator { set; get; }
        public string updatorName { set; get; }
        public string update_time { set; get; }
        public string creator { set; get; }
        public string creatorName { set; get; }
        public string typeName { set; get; }
        public string create_time { set; get; }
    }
}