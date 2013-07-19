// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年07月19日 10:19
//  
//  修改于：2013年07月19日 13:26
//  文件名：CasClient/Dev.CasClient/WebConfigUtils.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Configuration;
using System.Web.Configuration;

namespace Dev.CasClient.Utils
{
    internal static class WebConfigUtils
    {
        #region Class Methods

        /// <summary>
        ///   取得Webconfig中默认的LoginUrl
        /// </summary>
        /// <returns> </returns>
        public static string FormsLoginUrl()
        {
            var authSection =
                (AuthenticationSection) ConfigurationManager.GetSection("system.web/authentication");

            var loginurl = authSection.Forms.LoginUrl;

            return loginurl;
        }

        #endregion
    }
}