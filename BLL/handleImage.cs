using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleImage
    {
        //获取附件列表
        public DataTable queryList(string ids)
        {
            string ids_str = "";
            string[] ids_arr = ids.Split(',');
            for (int i = 0; i < ids_arr.Length; i++)
            {
                ids = "'" + ids_arr[i] + "'";  //在每个元素前后加上我们想要的格式，效果例如：
                if (i < ids_arr.Length - 1)  //根据数组元素的个数来判断应该加多少个逗号
                {
                    ids += ",";
                }
                ids_str += ids;
            }
            string str = @"select   id,
                                    fileName,
                                    fileType,
                                    fileSize,
                                    filePath,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                               from dbo.c_file
                               where id in(" + ids_str + ")";
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //获取附件信息
        public DataTable queryDetail(string id)
        {
            string str = @"select   id,
                                    fileName,
                                    fileType,
                                    fileSize,
                                    filePath,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                               from dbo.c_file
                               where id='{0}'";
            str = string.Format(str, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存附件信息
        public bool save(dynamic d)
        {
            string str = @"insert into dbo.c_file (id, fileName, fileType, fileSize, filePath)
                                values ('{0}', '{1}', '{2}', '{3}', '{4}')";
            str = string.Format(str, d.id, d.fileName, d.fileType, d.fileSize, d.filePath);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
