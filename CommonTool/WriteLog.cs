using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace CommonTool
{
    public class WriteLog
    {
        //错误日志文件全路径
        public static string ErrorFolder = CommonTool.PathHelper.GetCurrentBasePath() + @"ErrorLog";
        public static string ErrorFilePath = CommonTool.PathHelper.GetCurrentBasePath() + @"ErrorLog/{0}.txt";

        //向日志文件写日志
        public static void Write(string strErrorMsg)
        {
            //判断文件夹是否存在
            if (!Directory.Exists(ErrorFolder))
            {
                Directory.CreateDirectory(ErrorFolder);
            }
            //首先判断文件是否存在，如果不存在则创建该文件
            string fileName = DateTime.Now.ToString("yyyy-MM-dd");
            ErrorFilePath = string.Format(ErrorFilePath, fileName);
            if (!File.Exists(ErrorFilePath))
            {
                File.CreateText(ErrorFilePath).Dispose();
            }

            StreamWriter sw = File.AppendText(ErrorFilePath);
            sw.WriteLine(String.Format("时间:{0}   错误信息:{1}", DateTime.Now.ToString(), strErrorMsg));
            sw.Close();
            sw.Dispose();
        }
    }
}
