using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

using System.Drawing;

namespace CommonTool
{
    /// <summary>
    /// DeEncode 的摘要说明
    /// </summary>
    public class DeEncode
    {
        public DeEncode()
        {
        }

        const string KEY_64 = "ZHUJINGJ";//注意了，是8个字符，64位

        const string IV_64 = "ZHUJINGJ";
        public static string Encode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        public static string Decode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }

    }


    public class DeEncodeBase64
    {
        public DeEncodeBase64()
        {
        }

        //图片序列化为字符串
        public static string Encode(string picurl)
        {
            string strRtn = string.Empty;


            return strRtn;

        }

        //字符串反序列化为图片
        public static Image Decode(string data)
        {
            byte[] aryData = new Base64Decoder().GetDecoded(data);
            Image img = ImageHelper.BytesToImage(aryData);
            return img;
        }
    }
}

