using System;
using System.Linq;
using System.Web;
using Dev.CasServer;
using Domain.MainBoundedContext.UserExtend.Repository;
using Domain.MainBoundedContext.UserProfile.Repository;
using Domain.MainBoundedContext.webpages_Membership.Repository;
using WebMatrix.WebData;

namespace Application.CasHander
{
    /// <summary>
    /// 一个假的用户登录判断，这个随后应该是去业务中进行判断的
    /// </summary>
    public class UserValidateFake : IUserValidate
    {
        private IUserProfileRepository _userProfileRepository;
        private IUserExtendRepository _userExtendRepository;
        private IWebpagesMembershipRepository _webpagesMembershipRepository;
        public UserValidateFake(IUserProfileRepository userProfileRepository, IUserExtendRepository userExtendRepository, IWebpagesMembershipRepository webpagesMembershipRepository)
        {
            this._userExtendRepository = userExtendRepository;
            this._userProfileRepository = userProfileRepository;
            this._webpagesMembershipRepository = webpagesMembershipRepository;
        }



        public bool PerformAuthentication(string strUserName, string strPassWord, bool rembers, out string ErrorMsg)
        {
            HttpContext.Current.Response.AddHeader("P3P",
                              "CP=\"CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR\"");
            ErrorMsg = "";
            //WebSecurity.ConfirmAccount()
            if (WebSecurity.Login(strUserName, strPassWord, rembers))
            {
                return true;
            }

            return false;
        }

        public object GetExtendProperty(string strUserName)
        {
            var userid = WebSecurity.GetUserId(strUserName);

            var userExtend = this._userExtendRepository.Single(x => x.UserId == userid);

            var uid = userExtend.Uid;

            var profile = this._userProfileRepository.Single(x => x.UserId == userid);


            var obj = new
            {
                Uid = uid,
                profile.Sex,
                profile.NickName,
                profile.City,
                profile.Province,
                profile.UserName,
                profile.Email,
                profile.Avator,
                profile.CityName,
                profile.ProvinceName,
                profile.UserId
            };

            return obj;
        }
    }
}
