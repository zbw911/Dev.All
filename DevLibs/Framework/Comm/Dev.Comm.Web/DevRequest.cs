// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：DevRequest.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.IO;
using System.Web;
using System.Web.Routing;
using Dev.Comm.Utils;

namespace Dev.Comm.Web
{
    /// <summary>
    /// Request操作类
    /// </summary>
    public class DevRequest
    {
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }


        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] SearchEngine =
                {
                    "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom",
                    "yisou", "iask", "soso", "gougou", "zhongsou"
                };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        #region GetString

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName, false);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return "";

            if (sqlSafeCheck && !Security.IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.QueryString[strName];
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            return GetFormString(strName, false);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
                return "";

            if (sqlSafeCheck && !Security.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.Form[strName];
        }

        public static string GetRouteDataString(string strName, bool sqlSafeCheck)
        {
            RouteData rotedata = HttpContext.Current.Request.RequestContext.RouteData;
            if (rotedata != null)
            {
                if (rotedata.Values.ContainsKey(strName))
                {
                    string data = rotedata.Values[strName].ToString();

                    if (sqlSafeCheck && !Security.IsSafeSqlString(data))
                        throw new UnSafeRequestException("unsafe " + data);

                    return data;
                }
            }

            return "";
        }

        public static string GetRouteDataString(string strName)
        {
            return GetRouteDataString(strName, false);
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            return GetString(strName, false);
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            if (!"".Equals(GetQueryString(strName)))
                return GetQueryString(strName, sqlSafeCheck);


            if (!"".Equals(GetFormString(strName)))
                return GetFormString(strName, sqlSafeCheck);

            if (!"".Equals(GetRouteDataString(strName)))
                return GetRouteDataString(strName, sqlSafeCheck);

            return "";
        }

        #endregion

        #region GetInit

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName)
        {
            return TypeConverter.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }


        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return TypeConverter.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return TypeConverter.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }


        public static int GetRouteDataInt(string strName, int defValue)
        {
            RouteData rotedata = HttpContext.Current.Request.RequestContext.RouteData;
            if (rotedata != null)
            {
                if (rotedata.Values.ContainsKey(strName))
                {
                    string data = rotedata.Values[strName].ToString();
                    return TypeConverter.StrToInt(data, defValue);
                }
            }

            return defValue;
        }

        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) != defValue)
                return GetQueryInt(strName, defValue);
            if (GetFormInt(strName, defValue) != defValue)
                return GetFormInt(strName, defValue);
            if (GetRouteDataInt(strName, defValue) != defValue)
                return GetRouteDataInt(strName, defValue);

            return defValue;
        }

        #endregion

        #region Float

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return TypeConverter.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return TypeConverter.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        public static float GetRouteDataFloat(string strName, float defValue)
        {
            RouteData rotedata = HttpContext.Current.Request.RequestContext.RouteData;
            if (rotedata != null)
            {
                if (rotedata.Values.ContainsKey(strName))
                {
                    string data = rotedata.Values[strName].ToString();
                    return TypeConverter.StrToFloat(data, defValue);
                }
            }

            return defValue;
        }

        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) != defValue)
                return GetQueryFloat(strName, defValue);

            if (GetFormFloat(strName, defValue) != defValue)
                return GetFormFloat(strName, defValue);
            if (GetRouteDataFloat(strName, defValue) != defValue)
                return GetRouteDataFloat(strName, defValue);

            return defValue;
        }

        #endregion

        /// <summary>
        /// 取得类型
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(string strName, T defValue)
        {
            var val = GetString(strName);
            if (val == "")
                return defValue;

            return TypeConverter.ConvertType(val, defValue);
        }

        /// <summary>
        /// Reqest 文件处理
        /// </summary>
        public static class File
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static Stream GetFilesStream(string name)
            {
                return Files[name].InputStream;
            }

            /// <summary>
            /// 保存
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public static Stream GetFilesStream(int index)
            {
                return Files[index].InputStream;
            }


            /// <summary>
            /// 保存
            /// </summary>
            /// <param name="name"></param>
            /// <param name="filename"></param>
            public static void SavaAs(string name, string filename)
            {
                Files[name].SaveAs(filename);
            }

            /// <summary>
            /// 保存
            /// </summary>
            /// <param name="index"></param>
            /// <param name="filename"></param>
            public static void SavaAs(int index, string filename)
            {
                Files[index].SaveAs(filename);
            }


            /// <summary>
            /// Mime类型
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static string ContentType(string name)
            {
                return
                    Files[name].ContentType;
            }

            /// <summary>
            /// Mime类型
            /// </summary>
            /// <param name="index"> </param>
            /// <returns></returns>
            public static string ContentType(int index)
            {
                return
                    Files[index].ContentType;
            }

            private static HttpFileCollection Files
            {
                get { return HttpContext.Current.Request.Files; }
            }
        }
    }
}