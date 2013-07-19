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
using System.Web;
using Dev.CasClient.Utils;

namespace Dev.CasClient
{
    internal class CasClientModule : IHttpModule
    {
        #region Readonly & Static Fields

        private readonly CasClient casClient = new CasClient();

        #endregion

        #region Instance Methods

        private void Write(string w)
        {
            HttpContext.Current.Response.Write(w);
        }

        #endregion

        #region Event Handling

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;
            if (request.Path == Configuration.CasClientConfiguration.Config.LocalLogOffPath)
            {
                var strRedirectUrl = "";
                this.casClient.LoginOut(out strRedirectUrl);

                context.Response.Redirect(strRedirectUrl);
                context.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;


            //只有在请求路径一是设置路径的时候才进行处理
            if (request.Path == Configuration.CasClientConfiguration.Config.LocalLoginPath)
            {
                string returnUrl = Dev.Comm.Web.DevRequest.GetString("returnUrl");
                string ticket = Dev.Comm.Web.DevRequest.GetString("ticket");


                var handedReturl = Urls.GetReturnUrl(returnUrl);
                string strRedirectUrl, strUserName, strErrorText;

                //去除增加returl,同时删除ticket参数
                var strService = Urls.BuildServiceUrl(handedReturl);
                // HttpServerInfo.RebuildUrl("returnUrl", HttpUtility.UrlEncode(handedReturl), "ticket");

                if (this.casClient.Login(ticket, strService, out strRedirectUrl, out strUserName, out strErrorText))
                {
                    if (string.IsNullOrEmpty(strRedirectUrl))
                        context.Response.Redirect(handedReturl);
                    else
                        context.Response.Redirect(strRedirectUrl);
                }


                context.ApplicationInstance.CompleteRequest();
                return;
            }
            else if (request.Path == Configuration.CasClientConfiguration.Config.LocalLogOffPath)
            {
                var strRedirectUrl = "";
                //casClient.LoginOut(out strRedirectUrl);

                //context.Response.Redirect(strRedirectUrl);
                //context.ApplicationInstance.CompleteRequest();
                //return;
            }


            //Uri u = new Uri(localLoginPath, UriKind.Absolute);
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var end = 0;
        }

        #endregion

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.BeginRequest += this.OnBeginRequest;
            context.AuthenticateRequest += this.OnAuthenticateRequest;
            context.EndRequest += this.OnEndRequest;
        }


        public void Dispose()
        {
        }

        #endregion
    }
}