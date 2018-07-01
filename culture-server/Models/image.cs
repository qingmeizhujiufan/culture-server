using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace culture_server.Models
{
    public class image
    {
        public string id { set; get; }
        public string fileName { set; get; }
        public string fileType { set; get; }
        public int fileSize { set; get; }
        public string filePath { set; get; }
        public string create_time { set; get; }
    }
}