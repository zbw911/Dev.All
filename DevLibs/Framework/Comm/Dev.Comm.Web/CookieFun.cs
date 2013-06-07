// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：CookieFun.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;
using System.Web;

namespace Dev.Comm.Web
{
    public class CookieFun
    {
        /// <summary>
        /// 设置 cookies
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string value, TimeSpan? timespan = null, string domain = "", bool crossDomainCookie = false)
        {
            var cookies = new HttpCookie(cookieName, value);

            if (!string.IsNullOrEmpty(domain) && domain != "localhost")
                cookies.Domain = domain;
            cookies.Path = "/";



            if (timespan != null)
                cookies.Expires = DateTime.Now.Add((TimeSpan)timespan);

            if (crossDomainCookie)
                HttpContext.Current.Response.AddHeader("P3P",
                                    "CP=\"CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR\"");
            HttpContext.Current.Response.AppendCookie(cookies); //.a.Cookies.Add(cookies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="timespan"></param>
        /// <param name="crossDomainCookie"></param>
        public static void SetCookie(string cookieName, string value, TimeSpan? timespan = null, bool crossDomainCookie = false)
        {
            SetCookie(cookieName, value, timespan, null, crossDomainCookie);
        }

        public static void RemoveCookie(string cookieName, bool crossDomainCookie = false)
        {
            SetCookie(cookieName, "", new TimeSpan(365 * 24, 0, 0), crossDomainCookie);
        }


        public static void Clear()
        {
            HttpContext.Current.Response.Cookies.Clear();
        }

        public static string GetCookie(string cookieName)
        {
            if (!IsExistCookies(cookieName)) return null;

            return HttpContext.Current.Request.Cookies[cookieName].Value;
        }

        ///**
        // * 是否存在
        // * @param unknown_type $cookieName
        // */
        public static bool IsExistCookies(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName] != null;
        }
    }
}