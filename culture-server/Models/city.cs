using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class city
    {
        public string id { get; set; }
        public string cityName { get; set; }
        public int cultureTotal { get; set; }
        public int artTotal { get; set; }
        public int newsTotal { get; set; }
        public string create_time { get; set; }
    }
}