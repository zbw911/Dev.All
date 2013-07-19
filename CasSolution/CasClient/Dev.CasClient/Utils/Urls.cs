// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月13日 16:42
//  
//  修改于：2013年07月19日 9:35
//  文件名：CasClient/Dev.CasClient/Urls.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web;
using Dev.Comm.Web;

namespace Dev.CasClient.Utils
{
    internal static class Urls
    {
        #region Class Methods

        /// <summary>
        /// </summary>
        /// <param name="handedReturl"> </param>
        /// <param name="returnUrl"> 返回参数，默认是 returnUrl,如果 </param>
        /// <param name="ticket"> </param>
        /// <returns> </returns>
        public static string BuildServiceUrl(string handedReturl, string returnUrl = "returnUrl",
                                             string ticket = "ticket")
        {
            return HttpServerInfo.RebuildUrl(returnUrl, HttpUtility.UrlEncode(handedReturl), ticket);
        }

        /// <summary>
        ///   返回地址,return->reffer-> /
        /// </summary>
        /// <param name="returnUrl"> </param>
        /// <returns> </returns>
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

        #endregion
    }
}