// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：HttpServerInfo.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Dev.Comm.Web
{
    using System.Collections.Generic;

    public class HttpServerInfo
    {
        /// <summary>
        /// 系统根路径
        /// </summary>
        public static string RELATIVE_ROOT_PATH = HttpContext.Current.Server.MapPath("~");

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string BaseUrl
        {
            get { return GetRequestType() + "://" + GetHost() + (GetPort() == 80 ? "" : ":" + GetPort()) + ""; }
        }

        /// <summary>
        /// 当前系统网点地址
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            //HttpContext.Current.Request.
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 如源地址 : http://www.google.com/aaaa.html?fffffff.com
        /// 返回: http://www.google.com/aaaa.html
        /// </summary>
        /// <returns></returns>
        public static string GetLeftPath()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
        }

        /// <summary>
        /// 全地址包括？后的
        /// </summary>
        /// <returns></returns>
        public static string FullRequestPath()
        {
            return BaseUrl + HttpContext.Current.Request.Url.PathAndQuery;
        }


        /// <summary>
        /// 重建
        /// </summary>
        /// <param name="AddParms"></param>
        /// <param name="RemoveKeys"></param>
        /// <returns></returns>
        public static string RebuildUrl(NameValueCollection AddParms, string[] RemoveKeys)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query);

            Uri u = HttpContext.Current.Request.Url;

            string url = GetRequestType() + "://" + u.Authority + "" + u.AbsolutePath;

            return RebuildUrlParms(AddParms, RemoveKeys, queryString, url);
        }


        /// <summary>
        /// 重建
        /// </summary>
        /// <param name="AddParms"></param>
        /// <param name="RemoveKeys"></param>
        /// <returns></returns>
        public static string RebuildUrl(string addkey, string addvalue, string removeKey)
        {

            NameValueCollection queryString = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query);
            //NameValueCollection queryString = HttpContext.Current.Request.QueryString;

            Uri u = HttpContext.Current.Request.Url;

            string url = GetRequestType() + "://" + u.Authority + "" + u.AbsolutePath;


            NameValueCollection AddParms = new NameValueCollection { { addkey, addvalue } };
            var RemoveKeys = new[] { removeKey };
            return RebuildUrlParms(AddParms, RemoveKeys, queryString, url);
        }

        /// <summary>
        /// 重建URL
        /// </summary>
        /// <param name="addParms"></param>
        /// <param name="removeKeys"></param>
        /// <param name="queryString"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string RebuildUrlParms(
         NameValueCollection addParms, IEnumerable<string> removeKeys, NameValueCollection queryString, string url)
        {

            url += "?";


            if (removeKeys != null)
            {

                queryString = new NameValueCollection(queryString);
                foreach (var removeKey in removeKeys)
                {
                    string key = removeKey;
                    var localkeys = queryString.AllKeys.FirstOrDefault(x => x.ToLower() == key.ToLower());

                    queryString.Remove(localkeys);
                }
            }

            if (queryString != null)
            {
                foreach (var item in queryString.AllKeys)
                {
                    string val = queryString[item];

                    var parmkey = "";
                    if (
                        !string.IsNullOrEmpty(
                            parmkey = addParms.AllKeys.FirstOrDefault(x => x.ToLower() == item.ToLower())))
                    {
                        val = addParms[parmkey];
                    }

                    url += item + "=" + val + "&";
                }


                // 1,2,3,4,5,6
                //         5,6,7

                var queryResult = from p in addParms.AllKeys where queryString.AllKeys.All(x => x.ToLower() != p.ToLower()) select p;

                foreach (var key in queryResult)
                {
                    url += key + "=" + addParms[key] + "&";
                }
            }
            url = url.TrimEnd('&').TrimEnd('?');

            return url;
        }

        /// <summary>
        /// 增加参数
        /// </summary>
        /// <param name="sk"></param>
        /// <param name="sv"></param>
        /// <returns></returns>
        public static string AddParmToUrl(string sk, string sv)
        {
            NameValueCollection queryString = HttpContext.Current.Request.QueryString;

            Uri u = HttpContext.Current.Request.Url;

            string url = GetRequestType() + "://" + u.Authority + "" + u.AbsolutePath;

            var AddParms = new NameValueCollection { { sk, sv } };

            return RebuildUrlParms(AddParms, null, queryString, url);

        }


        /// <summary>
        /// 从URL中移除一个参数
        /// </summary>
        /// <param name="sk"></param>
        /// <returns></returns>
        public static string RemoveParmToUrl(string sk)
        {
            NameValueCollection queryString = HttpContext.Current.Request.QueryString;

            Uri u = HttpContext.Current.Request.Url;

            string url = GetRequestType() + "://" + u.Authority + "" + u.AbsolutePath;


            return RebuildUrlParms(null, new[] { sk }, queryString, url);
        }


        /// <summary>
        /// 请求端口
        /// </summary>
        /// <returns></returns>
        public static int GetPort()
        {
            return HttpContext.Current.Request.Url.Port;
        }

        /// <summary>
        /// 请求类型
        /// </summary>
        /// <returns></returns>
        public static string GetRequestType()
        {
            return HttpContext.Current.Request.Url.Scheme;
        }

        /// <summary>
        /// Agent
        /// </summary>
        /// <returns></returns>
        public static string GetRequestAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;


            if (HttpContext.Current.Request.UrlReferrer != null)
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();


            if (retVal == null)
                return "";

            return retVal;
        }

        public static string GetCurrAbsolutePath()
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            return url;
        }

        /// <summary>
        /// 将本地URL,包装包完整URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string WapperFullUrl(string url)
        {
            url = string.IsNullOrEmpty(url) ? "" : url;
            return url.IndexOf(HttpServerInfo.BaseUrl, System.StringComparison.Ordinal) == 0 ? url : HttpServerInfo.BaseUrl + url;
        }
    } 
}