using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonTool
{
    /// <summary>
    /// 此加密过程是不可逆的，只能进行加密操作，不能进行反向解密
    /// </summary>
    public class Author
    {

        public Author()
        {
        }

        public static string GetEnCode(string str)
        {
            string strRtn = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                strRtn += GetEnCode(str[i]);
            }

            return strRtn;
        }

        private static string GetEnCode(int c)
        {
            int rand1 = 3;
            int rand2 = 7;
            int rand3 = 11;

            char ct;


            string rtn = string.Empty;

            if (c == 58)
            {
                rtn = "AZa";
            }
            else if (c >= 65 && c <= 90)
            {
                ct = GetTransChar(65, 90, c, rand1);
                rtn += ct;
                ct = GetTransChar(65, 90, c, rand2);
                rtn += ct;
                ct = GetTransChar(65, 90, c, rand3);
                rtn += ct;
            }
            else if (c >= 97 && c <= 122)
            {
                ct = GetTransChar(97, 122, c, rand1);
                rtn += ct;
                ct = GetTransChar(97, 122, c, rand2);
                rtn += ct;
                ct = GetTransChar(97, 122, c, rand3);
                rtn += ct;
            }
            else if (c >= 48 && c <= 57)
            {
                ct = GetTransChar(48, 57, c, rand1);
                rtn += ct;
                ct = GetTransChar(48, 57, c, rand2);
                rtn += ct;
                ct = GetTransChar(48, 57, c, rand3);
                rtn += ct;
            }

            return rtn;

        }

        private static char GetTransChar(int min, int max, int charIn, int rand)
        {
            int ct = charIn + rand;
            if ((ct % max) >= min || (ct % max) == 0)
            {
                return (char)ct;
            }
            else
            {
                return (char)((ct % max) + min - 1);
            }
        }
    }

    public class AuthorDetial
    {
        public int leftDays ;
        public string message;
    }
}
