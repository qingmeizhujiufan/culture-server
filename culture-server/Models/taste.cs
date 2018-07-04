﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class taste
    {
        public string id { get; set; }
        public image tasteCover { get; set; }
        public string tasteBrief { get; set; }
        public int collectNum { get; set; }
        public int commentNum { get; set; }
        public int state { get; set; }
        public string updator { get; set; }
        public string update_time { get; set; }
        public string creator { get; set; }
        public string create_time { get; set; }
    }
}