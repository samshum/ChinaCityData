/*************************************************************************************
* 文 件 名： WebRequest.cs
* 创建时间： 2015-06-18
* 作    者： Sam Shum (s.sams ＠ msn.com)
* 说    明： 解决WebHttpRequest下载网页数据出现问题  
* 修改时间： 2015-06-19
* 修 改 人： Sam Shum
*************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ChinaCityData
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            //string Url = "http://localhost:5192/api/Cab/5";
            //string httpMethod = "PUT";
            //string httpContent = "{'Name': 'B90', 'Color': 'Green', 'Height': 1590, 'Width': 4500 }";
            //Encoding httpCode = Encoding.Default;
            //Console.WriteLine(Get(Url, httpMethod, httpContent, httpCode));

            setEncoding = Encoding.Default;
        }

        public HttpRequest(string url, string method) : base()
        {
            setUrl = url;
            setMethod = method;
        }

        /// <summary>
        /// 服务请求地址 http://www.aaa.com/
        /// </summary>
        public string setUrl { get; set; }

        /// <summary>
        /// 服务请求方法：GET/POST/PUT/DELETE
        /// </summary>
        public string setMethod { get; set; }

        /// <summary>
        /// Referer Uri
        /// </summary>
        public string setReferer { get; set; }

        /// <summary>
        /// 设置服务请求数据类型
        /// </summary>
        [DefaultValue("text/html")]
        public string setContentType { get; set; }

        /// <summary>
        /// 设置服务页面编码
        /// </summary>
        public Encoding setEncoding { get; set; }

        public delegate void DownloadStartDelegate(int httpStatusCode);
        public delegate void DownloadProcessDelegate(long totalLength, long DownloadedByte, float percent);
        public delegate void DownloadEndDelegate(long totalLength);

        public event DownloadStartDelegate DownloadStart;
        public event DownloadProcessDelegate DownloadProcess;
        public event DownloadEndDelegate DownloadEnd;

        public string Get()
        {
            return Get(setUrl);
        }

        public string Get(string setUrl)
        {
            return Get(setUrl, "GET", null, setEncoding);
        }

        public string Get(string setUrl, string httpContent)
        {
            return Get(setUrl, "POSE", httpContent, setEncoding);
        }

        public string Get(string Url, string httpMethod, string httpContent, Encoding httpCode)
        {
            if (string.IsNullOrWhiteSpace(Url) || string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new ArgumentException("Url or HttpMethod 参数不能为空！");
            }

            GC.Collect();
            StringBuilder content = new StringBuilder();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamWriter sw = null;
            try
            {
                ServicePointManager.DefaultConnectionLimit = 512;
                request = WebRequest.Create(Url) as HttpWebRequest;
                request.Method = httpMethod;
                request.AllowAutoRedirect = true;
                request.KeepAlive = false;
                request.Referer = setReferer;
                request.Accept = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
                request.ContentType = setContentType; //"application/octet-stream";
                request.Proxy = null;

                if (httpContent != null && !string.IsNullOrWhiteSpace(httpContent))
                {
                    using (sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.Write(httpContent);
                    }
                }

                WebResponse res = request.GetResponse();
                if (res != null)
                {
                    response = res as HttpWebResponse;
                    if (this.DownloadStart != null)
                        this.DownloadStart((int)response.StatusCode);

                    Stream stream = response.GetResponseStream();
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                        stream = new GZipStream(stream, CompressionMode.Decompress);

                    StreamReader sr = new StreamReader(stream);
                    content.Append(sr.ReadToEnd());
                    sr.Close();
                    sr.Dispose();
                    stream.Close();
                    stream.Dispose();
                    res.Close();
                    res.Dispose();
                }
            }
            catch (WebException webe) { 
                Console.WriteLine(webe.Message);
            }
            finally
            {
                if (request != null) request.Abort();
                if (response != null) response.Dispose();
                if (sw != null) sw.Dispose();
            }
            GC.SuppressFinalize(this);
            return content.ToString();

            /*
            Response.Write(Guid.NewGuid().ToString() + "<hr />");
            Response.Write(Guid.NewGuid().ToString("N") + "<hr />");

            Response.Write(GetHttpContent);
            Response.End();
            */
        }

    }
}