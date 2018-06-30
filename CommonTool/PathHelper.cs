using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool
{
    /// <summary>
    /// 处理服务端路径问题
    /// </summary>
    public class PathHelper
    {
        public PathHelper()
        {
        }

        public static string GetCurrentBasePath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
