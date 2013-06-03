using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Dev.DotNetOpenAuth.AspNetExtend.Client;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using Microsoft.Web.WebPages.OAuth;
using CASServer.Models;

namespace CASServer
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // 若要允许此站点的用户使用他们在其他站点(例如 Microsoft、Facebook 和 Twitter)上拥有的帐户登录，
            // 必须更新此站点。有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();

            OAuthWebSecurity.RegisterClient(new SinaClient(appId: "1771985550", appSecret: "09ebbe5b995e25e55b5d36a8de4b48c3"), displayName: "新浪", extraData: new Dictionary<string, object>
                                                                                                                                                                 {
                                                                                                                                                                     {"class","sina"}
                                                                                                                                                                 });

            OAuthWebSecurity.RegisterClient(new QQClient(appId: "100392331", appSecret: "4a1d5fe59f5e7b6425d9b182b19e106e"), displayName: "QQ", extraData: new Dictionary<string, object>
                                                                                                                                                                 {
                                                                                                                                                                     {"class","qq"}
                                                                                                                                                                 });

        }
    }
}
