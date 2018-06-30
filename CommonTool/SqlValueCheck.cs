using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTool
{
    public class SqlValueCheck
    {
        public static string ChangeEscapecode(string value)
        {
            return value.Replace("'","''");
         }
    }
}
