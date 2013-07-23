using System;
using System.Collections.Generic;
using Dev.CasClient.UserAuthenticate;

namespace Dev.CasClient.User
{
    /// <summary>
    /// </summary>
    public class UserInfo
    {
        private const string CookUserNameKey = "____UserName____";

        #region Class Methods

        /// <summary>
        ///   检测昵称是否存在
        /// </summary>
        /// <param name="nickname"> </param>
        /// <returns> </returns>
        public static bool CheckNick(string nickname)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/CheckNick?nickname=";

            url += nickname;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<bool>(result);
        }

        /// <summary>
        ///   取得用户的注册时间方法，null表示用户不存在
        /// </summary>
        /// <param name="uid"> </param>
        /// <returns> </returns>
        public static DateTime? GetRegDateTime(decimal uid)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetRegDateTime?uid=";

            url += uid;

            var result = Dev.Comm.Net.Http.GetUrl(url);
            var time = Dev.Comm.JsonConvert.ToJsonObject<DateTime?>(result);

            return time;
        }

        /// <summary>
        ///   根据用户UID取得用户信息
        /// </summary>
        /// <param name="uid"> </param>
        /// <returns> </returns>
        public static UserProfileModel GetUserInfo(decimal uid)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfo?uid=";

            url += uid;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<UserProfileModel>(result);
        }


        /// <summary>
        ///   根据用户昵称取得用户信息
        /// </summary>
        /// <param name="nickname"> </param>
        /// <returns> </returns>
        public static UserProfileModel GetUserInfoByNickname(string nickname)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoByNickname?nickname=" + nickname;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<UserProfileModel>(result);
        }


        /// <summary>
        ///   根据用户名取得用户信息
        /// </summary>
        /// <param name="username"> </param>
        /// <returns> </returns>
        public static UserProfileModel GetUserInfoByUserName(string username)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoByUserName?username=";

            url += username;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<UserProfileModel>(result);
        }

        /// <summary>
        ///   根据用户UID取得用户信息
        /// </summary>
        /// <param name="uids"> </param>
        /// <returns> </returns>
        public static List<UserProfileModel> GetUserInfoList(decimal[] uids)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoList?uids=";

            url += string.Join("&uids=", uids);

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<List<UserProfileModel>>(result);
        }

        /// <summary>
        ///   根据用户昵称批量取得用户信息
        /// </summary>
        /// <param name="nicknames"> </param>
        /// <returns> </returns>
        public static List<UserProfileModel> GetUserInfoListByNickNames(string[] nicknames)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoListByNickNames?nicknames=";

            url += string.Join("&nicknames=", nicknames);

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<List<UserProfileModel>>(result);
        }


        /// <summary>
        /// 当前的用户名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserName()
        {
            if (Dev.Comm.Web.CookieFun.IsExistCookies(CookUserNameKey))
            {
                var name = Dev.Comm.Web.CookieFun.GetCookie(CookUserNameKey);

                return Dev.Comm.Security.FormBase64Encrypt(name);
            }

            return "";
            //Dev.Comm.Web.CookieFun.GetCookie
        }




        internal static void SetCurrentUserName(string userName)
        {
            var value = Dev.Comm.Security.ToBase64Encrypt(userName);
            Dev.Comm.Web.CookieFun.SetCookie(CookUserNameKey, value);
        }

        /// <summary>
        /// 用户是还是已经通过登录验证
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated
        {
            get { return UserAuthenticateManager.Provider.GetUserIsAuthenticated(); }
        }

        #endregion
    }
}