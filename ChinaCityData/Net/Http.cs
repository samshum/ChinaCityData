using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChinaCityData
{
    internal class Http
    {
        private static HttpRequest web = new HttpRequest();

        public static string Get(string url) { 
            return Get(url, null);
        }
        public static string Get(string url, string refererUri) { 
            web.setReferer = refererUri;
            return web.Get(url);
        }

        public static string Post(string url, string postData, string refererUri)
        {
            web.setReferer = refererUri;
            web.setContentType = "application/x-www-form-urlencoded; charset=utf-8";
            return web.Get(url, "Post", postData, Encoding.UTF8);
        }
    }
}
