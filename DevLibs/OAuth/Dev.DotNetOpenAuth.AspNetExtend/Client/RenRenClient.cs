using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetOpenAuth.AspNet.Clients;

namespace Dev.DotNetOpenAuth.AspNetExtend.Client
{
    /// <summary>
    /// 人人客户端
    /// http://wiki.dev.renren.com/wiki/Authentication
    /// </summary>
    public class RenRenClient : OAuth2Client
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        public RenRenClient(string appId, string appSecret)
            : base("RenRen")
        {
        }


        /// <summary>
        /// Gets the full url pointing to the login page for this client. The url should include the specified return url so that when the login completes, user is redirected back to that url.
        /// </summary>
        /// <param name="returnUrl">The return URL. 
        ///             </param>
        /// <returns>
        /// An absolute URL. 
        /// </returns>
        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Given the access token, gets the logged-in user's data. The returned dictionary must include two keys 'id', and 'username'.
        /// </summary>
        /// <param name="accessToken">The access token of the current user. 
        ///             </param>
        /// <returns>
        /// A dictionary contains key-value pairs of user data 
        /// </returns>
        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Queries the access token from the specified authorization code.
        /// </summary>
        /// <param name="returnUrl">The return URL. 
        ///             </param><param name="authorizationCode">The authorization code. 
        ///             </param>
        /// <returns>
        /// The access token 
        /// </returns>
        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            throw new NotImplementedException();
        }
    }
}
