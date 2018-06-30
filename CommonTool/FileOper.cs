using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonTool
{
    public class FileOper
    {
        /// <summary>
        /// 打开文件，读取文件中内容（读取整个文件内容）
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string OpenFileRead(string strPath)
        {
            string strRtn = string.Empty;
            if (File.Exists(strPath))
            {
                StreamReader sr = new StreamReader(strPath);
                strRtn = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            else
            {
                WriteLog.Write("文件不存在");
            }
            return strRtn;
        }
    }
}
