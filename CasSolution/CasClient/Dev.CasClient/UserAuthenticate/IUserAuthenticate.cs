// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 14:14
// 
// 修改于：2013年02月19日 15:10
// 文件名：IUserAuthenticate.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasClient.UserAuthenticate
{
    /// <summary>
    ///   用户验证方法
    /// </summary>
    public interface IUserAuthenticate
    {
        #region Instance Methods

        /// <summary>
        ///   登出
        /// </summary>
        void CurUserLoginOut();

        /// <summary>
        ///   是否已经验证通过
        /// </summary>
        /// <returns> </returns>
        bool GetUserIsAuthenticated();

        /// <summary>
        ///   标识用户登录
        /// </summary>
        /// <param name="strUserName"> </param>
        /// <param name="extDatas"> </param>
        void SignUserLogin(string strUserName, System.Collections.Generic.Dictionary<string, string> extDatas);

        #endregion
    }
}