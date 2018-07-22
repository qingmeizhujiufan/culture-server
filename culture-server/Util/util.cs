using culture_server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace culture_server.Util
{
    public class util
    {
        //返回list image对象
        public static List<image> generateListImage(string ids)
        {
            DataTable dt = new BLL.handleImage().queryList(ids);
            List<image> list = new List<image>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                image img = new image();
                img.id = dt.Rows[i]["id"].ToString();
                img.fileName = dt.Rows[i]["fileName"].ToString();
                img.fileType = dt.Rows[i]["fileType"].ToString();
                img.fileSize = Convert.ToInt32(dt.Rows[i]["fileSize"].ToString());
                img.filePath = dt.Rows[i]["filePath"].ToString();
                img.create_time = dt.Rows[i]["create_time"].ToString();

                list.Add(img);
            }

            return list;
        }

        //返回image对象
        public static image generateImage(string id)
        {
            DataTable dt = new BLL.handleImage().queryDetail(id);
            image img = new image();
            if (dt.Rows.Count == 1)
            {
                img.id = dt.Rows[0]["id"].ToString();
                img.fileName = dt.Rows[0]["fileName"].ToString();
                img.fileType = dt.Rows[0]["fileType"].ToString();
                img.fileSize = Convert.ToInt32(dt.Rows[0]["fileSize"].ToString());
                img.filePath = dt.Rows[0]["filePath"].ToString();
                img.create_time = dt.Rows[0]["create_time"].ToString();
            }

            return img;
        }     
    }
}