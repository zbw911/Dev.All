// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：Http.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Dev.Comm.Net
{

    //TODO:这个类太多重复代码，需要重构
    public class Http
    {
        //#region 获取主机名称

        ///// <summary>
        ///// 获取本地机器名称
        ///// </summary>
        ///// <returns>机器名称</returns>
        //public static string GetHostName()
        //{
        //    string hostName = "";

        //    hostName = Dns.GetHostName();
        //    return hostName;
        //}

        //#endregion

        //#region 获取本地主机IP

        ///// <summary>
        ///// 获取本地机器IP
        ///// </summary>
        ///// <returns>IP地址</returns>
        //public static string GetHostIp()
        //{
        //    string localIp = "";


        //    IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        //    for (int i = 0; i < addressList.Length; i++)
        //    {
        //        localIp += addressList[i].ToString();
        //    }
        //    return localIp;
        //}

        //#endregion

        #region 获取指定WEB页面

        /// <summary>
        /// 获取指定WEB页面
        /// </summary>
        /// <param name="strurl">URL</param>
        /// <returns>string</returns>
        public static string GetWebUrl(string strurl)
        {
            var MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = MyWebClient.DownloadData(strurl);
            //string pageHtml = Encoding.UTF8.GetString(pageData);
            string pageHtml = Encoding.Default.GetString(pageData);
            return pageHtml;
        }

        #endregion

        #region GET方法获取页面

        /// GET方法获取页面
        /// 函数名:GetUrl	
        /// 功能描述:GET方法获取页面	
        /// 处理流程:
        /// 算法描述:
        /// 作 者: 杨栋
        /// 日 期: 2006-11-19 12:00
        /// 修 改: 2007-01-29 17:00
        /// 修 改: 2008-08-06 15:30
        /// 日 期:
        /// 版 本:

        #region GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url)
        {
            string outcookieheader = "";
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        public static string GetUrl(String url, string mycharset, string meiyouyong)
        {
            string outcookieheader = "";
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", 0, "", "", "", false, false, mycharset);
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, int timeout)
        {
            string outcookieheader = "";
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", timeout, "", "", "");
        }


        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, out string outcookieheader)
        {
            return GetUrl(url, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader)
        {
            return GetUrl(url, cookieheader, out cookieheader, "", true, "", "", "", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, bool autoRedirect)
        {
            return GetUrl(url, cookieheader, out cookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType">请求类型 http https</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string headerUserAgent,
                                    string httpType)
        {
            return GetUrl(url, cookieheader, out outcookieheader, "", true, headerUserAgent, httpType, "", 0, "", "",
                          "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType">请求类型 http https</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string headerReferer, string cookieheader, out string outcookieheader,
                                    string headerUserAgent, string httpType)
        {
            return GetUrl(url, cookieheader, out outcookieheader, headerReferer, true, headerUserAgent, httpType, "",
                          0, "", "", "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType">请求类型 http https</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string headerReferer, string cookieheader, out string outcookieheader,
                                    string headerUserAgent, string httpType, string encoding)
        {
            //return GetUrl(url,cookieheader,out outcookieheader, Header_Referer, AutoRedirect, Header_UserAgent, http_type, encoding,0);
            return GetUrl(url, cookieheader, out outcookieheader, headerReferer, true, headerUserAgent, httpType,
                          encoding, 0, "", "", "");
        }


        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType">请求类型 http https</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer,
                                    bool autoRedirect, string headerUserAgent, string httpType, string encoding,
                                    int timeout, string mywebproxy, string networkCredentialName,
                                    string networkCredentialPassword)
        {
            return GetUrl(url, cookieheader, out outcookieheader, Header_Referer, autoRedirect, headerUserAgent,
                          httpType, encoding, timeout, mywebproxy, networkCredentialName, networkCredentialPassword,
                          false, false, "");
        }

        /// <summary>
        /// GET方法获取页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType">请求类型 http https</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <param name="httpExpect100Continue">HTTP100Continue</param>
        /// <param name="servicePointManagerExpect100Continue">服务100Continue</param>
        /// <param name="mycharset"> </param>
        /// <returns>返回被请求页面的内容</returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer,
                                    bool autoRedirect, string headerUserAgent, string httpType, string encoding,
                                    int timeout, string mywebproxy, string networkCredentialName,
                                    string networkCredentialPassword, bool httpExpect100Continue,
                                    bool servicePointManagerExpect100Continue, string mycharset)
        {
            ServicePointManager.Expect100Continue = servicePointManagerExpect100Continue == false ? true : false;

            outcookieheader = string.Empty;
            if ((httpType == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);

                req.ServicePoint.Expect100Continue = httpExpect100Continue == false ? true : false;
                req.Method = "GET";
                req.AllowAutoRedirect = autoRedirect;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (headerUserAgent.Length < 2)
                {
                    headerUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                    ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    var myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((networkCredentialName.Length > 0) || (networkCredentialPassword.Length > 0))
                {
                    var myCred = new NetworkCredential(networkCredentialName, networkCredentialPassword);
                    var myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache; //增加请求身份验证信息
                }
                req.UserAgent = headerUserAgent;
                req.Accept =
                    "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");

                //为请求加入cookies 
                var cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] lsCookies = cookieheader.Split(';');
                if (lsCookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    for (int i = 0; i < lsCookies.Length; i++)
                    {
                        int indexOfSeparater = lsCookies[i].IndexOf("="); //找到第一个=号的位置
                        if (indexOfSeparater == -1)
                        {
                            continue;
                        }
                        string cookieKey = lsCookies[i].Substring(0, indexOfSeparater);
                        string cookieValue = lsCookies[i].Substring(indexOfSeparater + 1);
                        cookieCon.Add(new Uri(url), new Cookie(cookieKey.Trim(), cookieValue));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream receiveStream = null;
                //try
                res = (HttpWebResponse)req.GetResponse();



                string resContentEncoding = res.ContentEncoding.ToLower();
                if (resContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    receiveStream = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (resContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    receiveStream = new DeflateStream(res.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    receiveStream = res.GetResponseStream();
                }

                //try
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url)); //获得cookie
                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");
                    //outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                }
                string encodeheader = res.ContentType;
                string encodestr = Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) &&
                    (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }


                if (string.IsNullOrEmpty(mycharset) == false)
                {
                    encodestr = mycharset;
                }

                Encoding encode = Encoding.GetEncoding(encodestr); //GetEncoding("utf-8");
                var sr = new StreamReader(receiveStream, encode);
                var read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    var str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }

            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region POST方法获取页面

        /// POST方法获取页面
        /// 函数名:PostUrl	
        /// 功能描述:POST方法获取页面	
        /// 处理流程:
        /// 算法描述:
        /// 作 者: 杨栋
        /// 日 期: 2006-11-19 12:00
        /// 修 改: 2007-01-29 17:00
        /// 修 改: 2008-08-06 15:00
        /// 日 期:
        /// 版 本:

        #region PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList)
        {
            string outcookieheader = "";
            return PostUrl(url, paramList, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">post方式请求页面</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输出cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader)
        {
            return PostUrl(url, paramList, cookieheader, out cookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">post方式请求页面</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="outcookieheader">输入cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, out string outcookieheader)
        {
            return PostUrl(url, paramList, "", out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, "", true, "", "", "", 0, "", "", "");
        }


        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string headerReferer)
        {
            return PostUrl(url, paramList, cookieheader, out cookieheader, headerReferer, true, "", "", "", 0, "", "",
                           "");
        }

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string headerReferer,
                                     bool autoRedirect)
        {
            return PostUrl(url, paramList, cookieheader, out cookieheader, headerReferer, autoRedirect, "", "", "", 0,
                           "", "", "");
        }

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader,
                                     string headerReferer)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, headerReferer, true, "", "", "", 0, "",
                           "", "");
        }

        /// <summary>
        /// post方式请求页面
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader,
                                     string headerReferer, bool autoRedirect)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, headerReferer, autoRedirect, "", "", "",
                           0, "", "", "");
        }

        /// <summary>
        /// post主函数
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType"> 请求类型 http https </param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader,
                                     string headerReferer, bool autoRedirect, string headerUserAgent, string httpType,
                                     string encoding, int timeout, string mywebproxy, string networkCredentialName,
                                     string networkCredentialPassword)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, headerReferer, autoRedirect,
                           headerUserAgent, httpType, encoding, timeout, mywebproxy, networkCredentialName,
                           networkCredentialPassword, false, false);
        }

        /// <summary>
        /// post主函数
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType"> 请求类型 http https </param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <param name="httpExpect100Continue">HTTP100Continue</param>
        /// <param name="servicePointManagerExpect100Continue">服务100Continue</param>
        /// <returns>返回被请求页面的内容</returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader,
                                     string headerReferer, bool autoRedirect, string headerUserAgent, string httpType,
                                     string encoding, int timeout, string mywebproxy, string networkCredentialName,
                                     string networkCredentialPassword, bool httpExpect100Continue,
                                     bool servicePointManagerExpect100Continue)
        {
            return PostUrl(url, paramList, cookieheader, out outcookieheader, headerReferer, autoRedirect,
                           headerUserAgent, httpType, encoding, timeout, mywebproxy, networkCredentialName,
                           networkCredentialPassword, false, false, null);
        }

        /// <summary>
        /// post主函数
        /// </summary>
        /// <param name="url">要请求的url</param>
        /// <param name="paramList">请求内容。格式: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="autoRedirect">是否自动跳转</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="httpType"> 请求类型 http https </param>
        /// <param name="encoding">编码方式</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <param name="httpExpect100Continue">HTTP100Continue</param>
        /// <param name="servicePointManagerExpect100Continue">服务100Continue</param>
        /// <param name="Headers">请求头参数</param>
        /// <returns></returns>
        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader,
                                     string headerReferer, bool autoRedirect, string headerUserAgent, string httpType,
                                     string encoding, int timeout, string mywebproxy, string networkCredentialName,
                                     string networkCredentialPassword, bool httpExpect100Continue,
                                     bool servicePointManagerExpect100Continue, string[] Headers)
        {
            ServicePointManager.Expect100Continue = servicePointManagerExpect100Continue == false ? true : false;

            outcookieheader = string.Empty;
            if ((httpType == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.ServicePoint.Expect100Continue = httpExpect100Continue == false ? true : false;
                req.Method = "POST";
                req.AllowAutoRedirect = autoRedirect;
                if (headerReferer.Length > 2)
                {
                    req.Referer = headerReferer;
                }
                if (headerUserAgent.Length < 2)
                {
                    headerUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                    ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    var myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((networkCredentialName.Length > 0) || (networkCredentialPassword.Length > 0))
                {
                    var myCred = new NetworkCredential(networkCredentialName, networkCredentialPassword);
                    var myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache; //增加请求身份验证信息
                }
                req.UserAgent = headerUserAgent;
                req.Accept =
                    "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                if (Headers != null && Headers.Length > 0)
                {
                    for (int hCount = 0; hCount < Headers.Length; hCount++)
                    {
                        req.Headers.Add(Headers[hCount]);
                    }
                }
                req.MaximumResponseHeadersLength = 1024; //石坤杰 09-03-28 17:00 新增加

                //为请求加入cookies 
                var cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    //////////////////////////////////
                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        int indexOfSeparater = ls_cookies[i].IndexOf("="); //找到第一个=号的位置
                        if (indexOfSeparater == -1)
                        {
                            continue;
                        }
                        string cookieKey = ls_cookies[i].Substring(0, indexOfSeparater);
                        string cookieValue = ls_cookies[i].Substring(indexOfSeparater + 1);
                        cookieCon.Add(new Uri(url), new Cookie(cookieKey.Trim(), cookieValue));
                    }
                    req.CookieContainer = cookieCon;
                }
                var urlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            urlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        urlEncoded.Append((paramList.Substring(i, j - i)));
                        urlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(urlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = null;
                    //try
                    newStream = req.GetRequestStream();
                    //
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                //取得返回的响应
                //res = (HttpWebResponse)req.GetResponse();
                Stream receiveStream = null;
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");
                }



                string resContentEncoding = res.ContentEncoding.ToLower();
                if (resContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    receiveStream = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (resContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    receiveStream = new DeflateStream(res.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    receiveStream = res.GetResponseStream();
                }


                string encodeheader = res.ContentType;
                string encodestr = Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) &&
                    (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }
                Encoding encode = Encoding.GetEncoding(encodestr); //GetEncoding("utf-8");
                var sr = new StreamReader(receiveStream, encode);
                var read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    var str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }

            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }


        #endregion



        #endregion

        //NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
        //CredentialCache myCache = new CredentialCache();
        //myCache.Add(new Uri(url), "Basic", myCred);

        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //req.Credentials = myCache;//增加请求身份验证信息
        //怎讲代理设置



        #region 获取图片

        /// GET方法获取页面
        /// 函数名:GetImage	
        /// 功能描述:GET方法获取图片	
        /// 处理流程:
        /// 算法描述:
        /// 作 者: 杨栋
        /// 日 期: 2006-11-21 09:00
        /// 修 改:2006-12-05 17:00
        /// 修 改: 2007-01-29 17:00 
        /// 修 改: 2008-08-06 16:00
        /// 日 期:
        /// 版 本:
        /// 

        #region GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer, string Header_UserAgent, string http_type, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword)

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url)
        {
            string cookieheader = "";
            return GetImage(url, cookieheader, out cookieheader, "", "", "http", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader)
        {
            return GetImage(url, cookieheader, out cookieheader, "", "", "http", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string headerReferer)
        {
            return GetImage(url, cookieheader, out outcookieheader, headerReferer, "", "http", 0, "", "", "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, string Header_Referer)
        {
            return GetImage(url, cookieheader, out cookieheader, Header_Referer, "", "http", 0, "", "", "");
        }


        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="Header_Referer">包头 Referer</param>
        /// <param name="Header_UserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http 或 https</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, string Header_Referer, string Header_UserAgent,
                                      string http_type)
        {
            return GetImage(url, cookieheader, out cookieheader, Header_Referer, Header_UserAgent, http_type, 0, "", "",
                            "");
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader)
        {
            //return GetImage(url, cookieheader, out outcookieheader, Header_Referer,  Header_UserAgent, http_type,  timeout);
            return GetImage(url, cookieheader, out outcookieheader, "", "", "", 0, "", "", "");
        }

        public static byte[] GetImage(string url, string cookieheader, out string outcookieheader, string headerReferer,
                                      string headerUserAgent, string httptype, int timeout, string mywebproxy,
                                      string NetworkCredentialName, string NetworkCredentialPassword,
                                      bool HttpExpect100Continue, bool ServicePointManagerExpect100Continue)
        {
            bool IsAcceptEncoding = true;
            return GetImage(url, cookieheader, out outcookieheader, headerReferer, headerUserAgent, IsAcceptEncoding,
                            httptype, timeout, mywebproxy, NetworkCredentialName, NetworkCredentialPassword,
                            HttpExpect100Continue, ServicePointManagerExpect100Continue);
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="http_type">请求类型 http 或 https</param>
        /// <param name="timeout">超时时间 ms  输入0则为默认时间</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="NetworkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="NetworkCredentialPassword">密码 身份验证密码</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string headerReferer,
                                      string headerUserAgent, string http_type, int timeout, string mywebproxy,
                                      string NetworkCredentialName, string NetworkCredentialPassword)
        {
            bool IsAcceptEncoding = true;
            return GetImage(url, cookieheader, out outcookieheader, headerReferer, headerUserAgent, IsAcceptEncoding,
                            http_type, timeout, mywebproxy, NetworkCredentialName, NetworkCredentialPassword);
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="isAcceptEncoding">包头 Accept-Encoding（为true时请求包头会包含该值）</param>
        /// <param name="httpType">请求类型 http 或 https</param>
        /// <param name="timeout">超时时间 ms  输入0则为默认时间</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string headerReferer,
                                      string headerUserAgent, bool isAcceptEncoding, string httpType, int timeout,
                                      string mywebproxy, string networkCredentialName, string networkCredentialPassword)
        {
            return GetImage(url, cookieheader, out outcookieheader, headerReferer, headerUserAgent, isAcceptEncoding,
                            httpType, timeout, mywebproxy, networkCredentialName, networkCredentialPassword, false,
                            false);
        }

        /// <summary>
        /// GET方法获取图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="cookieheader">输入cookie</param>
        /// <param name="outcookieheader">输出cookie</param>
        /// <param name="headerReferer">包头 Referer</param>
        /// <param name="headerUserAgent">包头 UserAgent</param>
        /// <param name="isAcceptEncoding">包头 Accept-Encoding（为true时请求包头会包含该值）</param>
        /// <param name="httpType">请求类型 http 或 https</param>
        /// <param name="timeout">超时时间 ms  输入0则为默认时间</param>
        /// <param name="mywebproxy">代理地址 例  "xx.xx.xx.xx:xx"</param>
        /// <param name="networkCredentialName">帐号 身份验证帐号 对于一些需要身份严重的地址有用</param>
        /// <param name="networkCredentialPassword">密码 身份验证密码</param>
        /// <param name="httpExpect100Continue">HTTP100Continue(默认为false。一次性向官方发送所有数据)</param>
        /// <param name="servicePointManagerExpect100Continue">服务100Continue(默认为false，一次性向官方所有数据)</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string headerReferer,
                                      string headerUserAgent, bool isAcceptEncoding, string httpType, int timeout,
                                      string mywebproxy, string networkCredentialName, string networkCredentialPassword,
                                      bool httpExpect100Continue, bool servicePointManagerExpect100Continue)
        {
            ServicePointManager.Expect100Continue = servicePointManagerExpect100Continue != false;

            outcookieheader = string.Empty;
            if ((httpType == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https 跳过证书
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.ServicePoint.Expect100Continue = httpExpect100Continue != false;
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                if (headerReferer.Length > 2)
                {
                    req.Referer = headerReferer;
                }
                if (headerUserAgent.Length < 2)
                {
                    headerUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                    ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");代理设置
                    var myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((networkCredentialName.Length > 0) || (networkCredentialPassword.Length > 0))
                {
                    var myCred = new NetworkCredential(networkCredentialName, networkCredentialPassword);
                    var myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache; //增加请求身份验证信息
                }
                req.UserAgent = headerUserAgent;
                req.Accept =
                    "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";

                if (isAcceptEncoding)
                {
                    req.Headers.Add("Accept-Encoding", "gzip, deflate");
                }

                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");

                //为请求加入cookies 
                var cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //取得cookies 集合
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //如果有一个或没有cookies 就采用下面的方法。
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //如果是多个cookie 就分别加入 cookies 容器。
                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        int indexOfSeparater = ls_cookies[i].IndexOf("="); //找到第一个=号的位置
                        if (indexOfSeparater == -1)
                        {
                            continue;
                        }
                        string cookieKey = ls_cookies[i].Substring(0, indexOfSeparater);
                        string cookieValue = ls_cookies[i].Substring(indexOfSeparater + 1);
                        cookieCon.Add(new Uri(url), new Cookie(cookieKey.Trim(), cookieValue));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream receiveStream = null;
                res = (HttpWebResponse)req.GetResponse();

                string resContentEncoding = res.ContentEncoding.ToLower();


                if (resContentEncoding.Contains("gzip"))
                {
                    receiveStream = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (resContentEncoding.Contains("deflate"))
                {
                    receiveStream = new DeflateStream(res.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    receiveStream = res.GetResponseStream();
                }
                //receiveStream = res.GetResponseStream();
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url)); //获得cookie
                if (outcookieheader.Length < 2)
                {
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");
                    //outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                }
                var ms = new MemoryStream();
                var buffer = new byte[1024];
                while (true)
                {
                    int sz = receiveStream.Read(buffer, 0, 1024);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                ms.Position = 0;
                byte[] image = ms.GetBuffer();
                return image;
            }

            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
        }

        #endregion

        #endregion

        #endregion

        static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
                                               SslPolicyErrors errors)
        {
            //   Always   accept   
            return true;
        }


    }
}