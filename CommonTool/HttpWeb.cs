using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CommonTool
{
    public class HttpWeb
    {
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();

            return retString;

        }

        public static string HttpPost(string Url, string postDataStr, ref CookieContainer cookie)
        {
            string retString = string.Empty;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                if (cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = cookie;
                }

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (myStreamReader != null) myStreamReader.Close();
                if (myResponseStream != null) myResponseStream.Close();
            }

            return retString;

        }

        public static string HttpGet(string Url, string postDataStr)
        {

            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            HttpWebResponse response = null;
            string retString = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();

                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (myStreamReader != null) myStreamReader.Close();
                if (myResponseStream != null) myResponseStream.Close();
            }

            return retString;

        }

        public static string HttpGet(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            string retString = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                if (cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = cookie;
                }

                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (myStreamReader != null) myStreamReader.Close();
                if (myResponseStream != null) myResponseStream.Close();
            }

            return retString;

        }

        /// <summary>
        /// 返回JSon数据
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <param name="Url">要提交的URL</param>
        /// <returns>返回的JSON处理字符串</returns>
        public static string GetResponseData(string Url, string JSONData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //声明一个HttpWebRequest请求
            request.Timeout = 90000;
            //设置连接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }

        /// <summary>
        /// 批量上传图片
        /// </summary>
        /// <param name="srcurl">服务器路径</param>
        /// <param name="imagesPath">图片文件夹路径</param>
        /// <param name="files">图片名称</param>
        public static void UpLoadFiles(string srcurl, string imagesPath, List<string> files)
        {
            int i = 1;
            foreach (string imageName in files)
            {
                System.Threading.Thread.Sleep(500);
                UpLoadFile(srcurl, imagesPath, imageName);
                Console.WriteLine((i++).ToString());
            }
        }

        /// <summary>
        /// 上传单张图片
        /// </summary>
        /// <param name="srcurl">服务器路径</param>
        /// <param name="imagesPath">图片文件夹路径</param>
        /// <param name="fileName">图片名称</param>
        public static void UpLoadFile(string srcurl, string imagesPath, string fileName)
        {
            string url = null;
            string strImageFullPath = imagesPath + @"/" + fileName;
            //+  加号特殊处理
            if (fileName.Contains("+"))
            {
                url = srcurl + "name=" + fileName.Replace("+", "%2B");
            }
            else
            {
                url = srcurl + "name=" + fileName;
            }
            FileStream fs = new FileStream(strImageFullPath, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "image/jpeg";
            request.Method = "POST";
            Encoding encoding = Encoding.UTF8;
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), encoding);
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
        }

    }
}
