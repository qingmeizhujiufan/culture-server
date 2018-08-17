using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class organize
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string cityId { get; set; }
        public string cityName { get; set; }
        public string userName { get; set; }
        public string realName { get; set; }
        public int type { get; set; }
        public string typeName { get; set; }
        public string update_time { get; set; }
        public string create_time { get; set; }
    }
}