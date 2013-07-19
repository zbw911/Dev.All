// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 14:20
// 
// 修改于：2013年02月19日 15:10
// 文件名：UserAuthenticateManager.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasClient.UserAuthenticate
{
    /// <summary>
    ///   provider模式
    /// </summary>
    public class UserAuthenticateManager
    {
        #region Readonly & Static Fields

        private static IUserAuthenticate userAuthenticate;

        #endregion

        #region Class Properties

        public static IUserAuthenticate Provider
        {
            get
            {
                if (userAuthenticate == null)
                {
                    userAuthenticate = new FormUserAuthenticate();
                }

                return userAuthenticate;
            }
            set { userAuthenticate = value; }
        }

        #endregion
    }
}