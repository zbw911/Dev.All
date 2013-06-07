using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Comm.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        ///  是否为本地Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlLocalToHost(string url)
        {
            return !String.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public static bool IsCurrentDomainUrl(string url)
        {
            return !String.IsNullOrEmpty(url) && url.ToLower().IndexOf(HttpServerInfo.BaseUrl.ToLower(), StringComparison.Ordinal) == 0;
        }


       

    }
}
