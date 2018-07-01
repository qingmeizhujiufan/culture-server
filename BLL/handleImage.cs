using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class handleImage
    {
        //获取附件信息
        public DataTable queryDetail(string id)
        {
            string str = @"select   id,
                                    fileName,
                                    fileType,
                                    fileSize,
                                    filePath,
                                    CONVERT(varchar(19), create_time, 120) as create_time
                               from dbo.c_file";
            str = string.Format(str);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //保存附件信息
        public bool save(dynamic d)
        {
            string str = @"insert into dbo.c_file (fileName, fileType, fileSize, filePath)
                                values ('{0}', '{1}', '{2}', '{3}')";
            str = string.Format(str, d.fileName, d.fileType, d.fileSize, d.filePath);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
