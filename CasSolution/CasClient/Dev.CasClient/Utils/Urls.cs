using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.CasClient.Utils
{
    using System.Web;

    using Dev.Comm.Web;

    public static class Urls
    {
        /// <summary>
        /// 返回地址,return->reffer-> /
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static string GetReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return HttpServerInfo.WapperFullUrl(returnUrl);

            }

            var reffer = HttpServerInfo.GetUrlReferrer();


            if (!string.IsNullOrWhiteSpace(reffer))
            {
                return HttpServerInfo.WapperFullUrl(reffer);
            }

            return HttpServerInfo.BaseUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handedReturl"></param>
        /// <param name="returnUrl">返回参数，默认是 returnUrl,如果</param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static string BuildServiceUrl(string handedReturl, string returnUrl = "returnUrl", string ticket = "ticket")
        {
            return HttpServerInfo.RebuildUrl(returnUrl, HttpUtility.UrlEncode(handedReturl), ticket);
        }

    }
}
