using System;
using System.Collections.Generic;
using System.Web.Http;
using Dev.CasServer.Authenticator;
using Dev.Comm.Web.Mvc.Filter;
using Dev.Framework.FileServer;

using WebMatrix.WebData;
using Application.Dto.User;
using Application.MainBoundedContext.UserModule;

namespace CASServer.ApiControllers
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
    }






    public class UserController : ApiController
    {
        private ICasAuthenticator _casAuthenticator;
        private readonly IUserService _userService;
        private readonly IKey _key;


        public UserController(IUserService userService, IKey key)
        {

            this._userService = userService;
            _key = key;
            this._casAuthenticator = new FormsCasAuthenticator();
        }

        [WebApiAllowCrossSiteJson]
        public UserInfo Get()
        {

            return new UserInfo
            {
                UserId = WebSecurity.CurrentUserId,
                UserName = WebSecurity.CurrentUserName
            };
        }

        [WebApiAllowCrossSiteJson]
        public UserProfileModel GetUserInfo([FromUri]decimal uid)
        {
            return this._userService.GetUserProfileByCache(uid);
        }

        public List<UserProfileModel> GetUserInfoList([FromUri] decimal[] uids)
        {
            return this._userService.GetUserProfiles(uids);
        }

        public UserProfileModel GetUserInfoByNickname([FromUri]string nickname)
        {
            return this._userService.GetUserProfileByNickName(nickname);
        }

        public UserProfileModel GetUserInfoByUserName([FromUri]string username)
        {
            return this._userService.GetUserProfileByUserName(username);
        }

        public List<UserProfileModel> GetUserInfoListByNickNames([FromUri] string[] nicknames)
        {
            return this._userService.GetUserProfileByNickNames(nicknames);
        }

        //[WebApiAllowCrossSiteJson]
        //public UserProfileModel GetUserInfo(decimal[] uid)
        //{
        //    return userService.GetUserProfileByCache(uid);
        //}
        [WebApiAllowCrossSiteJson]
        [HttpGet, HttpPost]
        public bool CheckNick([FromUri]string nickname)
        {
            return _userService.UserNickExist(nickname);
        }

        [HttpGet, HttpPost]
        public DateTime? GetRegDateTime([FromUri]decimal uid)
        {
            return this._userService.GetRegDateTime(uid);
        }


        public string AvatarLoader()
        {

            return "";
        }

    }
}
