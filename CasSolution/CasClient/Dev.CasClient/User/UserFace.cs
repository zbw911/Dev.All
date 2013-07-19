using System;

namespace Dev.CasClient.User
{
    /// <summary>
    ///   取得用户头像
    /// </summary>
    public class UserFace
    {
        #region Class Methods

        public static string Get(decimal uid, int type)
        {
            if (type > 4 || type < 1)
                throw new Exception("只能在1至4之间");

            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl +
                      "/Avatar/AvataUrl?type=" + type + "&uid=" + uid;

            return (url);
        }

        /// <summary>
        ///   通过用户ID取得头像地址
        /// </summary>
        /// <param name="userid"> </param>
        /// <param name="type"> </param>
        /// <returns> </returns>
        /// <exception cref="Exception"></exception>
        public static string GetByUserid(int userid, int type)
        {
            if (type > 4 || type < 1)
                throw new Exception("只能在1至4之间");

            return Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl +
                   "/Avatar/AvataUrlByUserid?type=" + type + "&userid=" + userid;
        }

        #endregion
    }
}