// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年07月18日 9:29
//  
//  修改于：2013年07月19日 9:35
//  文件名：CasClient/Dev.CasClient/CasClientModule.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using Dev.CasClient.User;
using Dev.CasClient.Utils;

namespace Dev.CasClient
{
    internal class CasClientModule : IHttpModule
    {
        #region Readonly & Static Fields

        private static bool _routeInited = false;
        private readonly CasClient _casClient = new CasClient();

        #endregion

        #region Instance Methods

        private void Checksession(HttpApplication app)
        {
            return;
            var context = HttpContext.Current;
            //var request = context.Request;

            //var a = app.Session;


            var cn = context.CurrentNotification;

            var path = context.Request.Path;
            var session = context.Session;


            //Write(path + "->" + cn.ToString() + ":Session" + (session == null ? "null" : "SESSION") + "<br/>");
        }

        private void HandlerAll()
        {
            var context = HttpContext.Current;
            var request = context.Request;

            ////context.Session["a"] = 1;
            //if (request.Path.ToLower() == Configuration.CasClientConfiguration.Config.LocalCheckPath.ToLower())
            //{
            //    Write("here over<br/>");
            //    context.ApplicationInstance.CompleteRequest();
            //    return;
            //}
            //return;
            //只有在请求路径一是设置路径的时候才进行处理
            if (request.Path.ToLower() == Configuration.CasClientConfiguration.Config.LocalLoginPath.ToLower())
            {
                #region Login

                var returnUrlParm = Dev.CasClient.Configuration.CasClientConfiguration.Config.ReturnUrlParm;
                var ticketName = Dev.CasClient.Configuration.CasClientConfiguration.Config.TicketName;

                var localParam = Dev.CasClient.Configuration.CasClientConfiguration.Config.LocalParam;


                var returnUrl = Dev.Comm.Web.DevRequest.GetString(returnUrlParm);
                var ticket = Dev.Comm.Web.DevRequest.GetString(ticketName);
                var local = Dev.Comm.Web.DevRequest.Get(localParam, false);

                var handedReturl = Urls.GetReturnUrl(returnUrl);
                string strRedirectUrl, strUserName, strErrorText;

                //去除增加returl,同时删除ticket参数
                var strService = Urls.BuildServiceUrl(handedReturl, returnUrlParm, ticketName);
                // HttpServerInfo.RebuildUrl("returnUrl", HttpUtility.UrlEncode(handedReturl), "ticket");

                if (local)
                {
                    strService = Dev.Comm.Web.DevRequest.GetString("service");
                }

                if (this._casClient.Login(ticket, strService, out strRedirectUrl, out strUserName, out strErrorText))
                {
                    if (CasClient.LoginSucess != null)
                    {
                        CasClient.LoginSucess();
                    }

                    if (local)
                    {
                        context.Response.ContentType = "text/html;charset=UTF-8";

                        //context.Response.Headers.Add("ContentType", "text/html");
                        context.Response.Write("OK");
                        context.ApplicationInstance.CompleteRequest();
                        return;
                    }

                    if (string.IsNullOrEmpty(strRedirectUrl))
                    {
                        //对于 .Html 结尾的页面加一个 随机数，用以清除浏览器缓存，这样可以在一定程度上解决页面反复刷新的问题，
                        //这仅是一个补丁，不是最终解决方案，最终方案，应该是使用 Ajax方式，读取当前用户状态，然后显示于页面

                        //var jumpurl = handedReturl;

                        //var uncachekey = "uncache=" + System.DateTime.Now.Ticks;

                        //if (jumpurl.IndexOf('?') > 0)
                        //    jumpurl = jumpurl.TrimEnd('&') + "&" + uncachekey;
                        //else
                        //    jumpurl = jumpurl + "?" + uncachekey;


                        context.Response.Redirect(handedReturl);
                    }
                    else
                        context.Response.Redirect(strRedirectUrl);
                }
                else
                {
                    if (local)
                    {
                        context.Response.Write(strErrorText);
                        context.ApplicationInstance.CompleteRequest();
                        return;
                    }

                    context.Response.Write(strErrorText);
                }


                context.ApplicationInstance.CompleteRequest();
                return;

                #endregion
            }
            else if (request.Path.ToLower() == Configuration.CasClientConfiguration.Config.LocalCheckPath.ToLower())
            {
                #region LocalCheck

                string responsestr;
                if (UserAuthenticate.UserAuthenticateManager.Provider.GetUserIsAuthenticated())
                {
                    var username = UserInfo.GetCurrentUserName();
                    responsestr = Dev.Comm.JsonConvert.ToJsonStrDyn(new { state = true, username = username });
                }
                else
                {
                    responsestr = Dev.Comm.JsonConvert.ToJsonStrDyn(new { state = false });
                }

                //context.Response.ContentType = "application/json";

                context.Response.Write(responsestr);

                context.ApplicationInstance.CompleteRequest();
                return;

                #endregion
            }
            else if (request.Path.ToLower() == Configuration.CasClientConfiguration.Config.LocalLogOffPath.ToLower())
            {
                #region LocalLoginOut

                var localParam = Dev.CasClient.Configuration.CasClientConfiguration.Config.LocalParam;
                var local = Dev.Comm.Web.DevRequest.Get<bool>(localParam, false);

                string handedReturl;
                var ret = this._casClient.LoginOut(out handedReturl);


                if (context.Session == null)
                    return;

                if (local)
                    context.Response.Write("Local");
                else
                    context.Response.Redirect(handedReturl);

                context.ApplicationInstance.CompleteRequest();
                return;

                #endregion
            }
        }

        /// <summary>
        ///   初始化与CAS登录相关的Route，这个功能在MVC 中基本没有太大的意义，但极有可能出现副作用，
        ///   但对于Winform而言，却是相当重要的，如果没有它，将会导致SEssion为空，或 无法找到页面的情况
        ///   之所以放在这里，是为了可以使它在MVC设置Route的后面执行。
        /// </summary>
        private void InitRoute()
        {
            if (_routeInited)
                return;
            _routeInited = true;
            var directory = HttpRuntime.AppDomainAppPath;
            var filename = "___Dev.CasClient.aspx";


            if (!File.Exists(directory + filename))
            {
                var filecontent = "<!-- 本文件为自动生成，用于承载SESSION，解决httpmodoule中为空的问题,生成于 " + System.DateTime.Now.ToString() +
                                  "-->";


                Dev.Comm.IO.IOUtility.SaveStringToFile(directory + filename, filecontent);
            }

            RouteTable.Routes.MapPageRoute("Configuration.CasClientConfiguration.Config.LocalLogOffPath",
                                           Dev.CasClient.Configuration.CasClientConfiguration.Config.LocalLogOffPath.
                                               TrimStart('/'), "~/" + filename, false);
            RouteTable.Routes.MapPageRoute("Configuration.CasClientConfiguration.Config.LocalLoginPath",
                                           Dev.CasClient.Configuration.CasClientConfiguration.Config.LocalLoginPath.
                                               TrimStart('/'), "~/" + filename, false);
            RouteTable.Routes.MapPageRoute("Configuration.CasClientConfiguration.Config.LocalCheckPath",
                                           Dev.CasClient.Configuration.CasClientConfiguration.Config.LocalCheckPath.
                                               TrimStart('/'), "~/" + filename, false);
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;

            this.Checksession((HttpApplication)sender);
        }

        private void OnAuthorizeRequest(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }


        private void OnBeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;

            this.Checksession((HttpApplication)sender);

            //else if (request.Path == Configuration.CasClientConfiguration.Config.LocalLogOffPath)
            //{
            //    bool local = Dev.Comm.Web.DevRequest.Get<bool>("local", false);

            //    string handedReturl;
            //    var ret = this.casClient.LoginOut(out handedReturl);

            //    if (local)
            //        context.Response.Write("Local");
            //    else
            //        context.Response.Redirect(handedReturl);

            //    context.ApplicationInstance.CompleteRequest();
            //    return;
            //}
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnLogRequest(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnMapRequestHandler(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostAuthorizeRequest(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostLogRequest(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostMapRequestHandler(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostReleaseRequestState(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostRequestHandlerExecute(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostResolveRequestCache(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPostUpdateRequestCache(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPreSendRequestContent(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnReleaseRequestState(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnResolveRequestCache(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void OnUpdateRequestCache(object sender, EventArgs e)
        {
            this.Checksession((HttpApplication)sender);
        }

        private void Write(string w)
        {
            HttpContext.Current.Response.Write(w);
        }

        #endregion

        #region Event Handling

        private void OnAcquireRequestState(object sender, EventArgs e)
        {
            //this.checksession((HttpApplication)sender);
            this.HandlerAll();
        }

        #endregion

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            this.InitRoute();

            //context.BeginRequest += this.OnBeginRequest;

            //context.PostUpdateRequestCache += this.OnPostUpdateRequestCache;
            //context.PostAuthorizeRequest += this.OnPostAuthorizeRequest;
            //context.AuthorizeRequest += this.OnAuthorizeRequest;
            //context.UpdateRequestCache += this.OnUpdateRequestCache;
            //context.ResolveRequestCache += this.OnResolveRequestCache;
            //context.ReleaseRequestState += this.OnReleaseRequestState;

            //context.PreRequestHandlerExecute += this.OnPreRequestHandlerExecute;
            //context.PostResolveRequestCache += this.OnPostResolveRequestCache;
            //context.PostRequestHandlerExecute += this.OnPostRequestHandlerExecute;
            //context.PostReleaseRequestState += this.OnPostReleaseRequestState;
            //context.PostMapRequestHandler += this.OnPostMapRequestHandler;
            //context.PostLogRequest += this.OnPostLogRequest;
            //context.PostAuthenticateRequest += this.OnPostAuthenticateRequest;
            //context.PostAcquireRequestState += this.OnPostAcquireRequestState;
            //context.LogRequest += this.OnLogRequest;
            //context.MapRequestHandler += this.OnMapRequestHandler;

            //context.AuthenticateRequest += this.OnAuthenticateRequest;
            context.AcquireRequestState += this.OnAcquireRequestState;
            //context.EndRequest += this.OnEndRequest;

            //context.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
            //context.PreSendRequestContent += this.OnPreSendRequestContent;
        }


        public void Dispose()
        {
        }

        #endregion
    }
}