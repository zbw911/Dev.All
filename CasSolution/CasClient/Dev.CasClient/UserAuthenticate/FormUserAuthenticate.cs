// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 14:15
// 
// 修改于：2013年02月19日 15:10
// 文件名：FormUserAuthenticate.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;

namespace Dev.CasClient.UserAuthenticate
{
    using System.Web;
    using System.Web.Security;

    /// <summary>
    /// 
    /// </summary>
    public class FormUserAuthenticate : IUserAuthenticate
    {
        #region Public Methods and Operators

        /// <summary>
        ///     登出
        /// </summary>
        public virtual void CurUserLoginOut()
        {
            HttpContext context = HttpContext.Current;

            // Necessary for ASP.NET MVC Support.
            if (context.User.Identity.IsAuthenticated)
            {
                ClearAuthCookie();
            }
        }

        /// <summary>
        ///     是否已经验证通过
        /// </summary>
        /// <returns></returns>
        public virtual bool GetUserIsAuthenticated()
        {
            HttpContext context = HttpContext.Current;

            bool result = (context.User != null && context.User.Identity.IsAuthenticated);

            return result;
        }

        public virtual void SignUserLogin(string strUserName, Dictionary<string, string> extDatas)
        {
            SetAuthCookie(strUserName);
        }



        #endregion

        #region Methods

        private static void ClearAuthCookie()
        {
            FormsAuthentication.SignOut();
        }

        private static void SetAuthCookie(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, false);
        }

        #endregion
    }
}