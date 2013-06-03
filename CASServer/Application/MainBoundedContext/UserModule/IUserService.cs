using CASServer.Models;

namespace Application.MainBoundedContext.UserModule
{
    using System.Collections.Generic;

    using Application.Dto;
    using Application.Dto.User;

    public interface IUserService
    {
        /// <summary>
        /// 昵称
        /// </summary>
        /// <returns></returns>
        bool UserNickExist(string nickname);

        /// <summary>
        /// 要用用户id 取得 Uid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        decimal GetUidByUserId(int userid);
        /// <summary>
        /// 根据用户编号读取用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserProfileModel GetUserProfile(decimal uid);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        decimal InserOrUpdateExtUid(int userid);
        /// <summary>
        /// 是否已经激活
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsConfirmedByToken(string token);
        /// <summary>
        /// 取得用户昵称
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string GetNickNameByUserName(string email);
        /// <summary>
        /// 取得确认token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string GetTokenByUserName(string email);
        /// <summary>
        /// 找回密码类的方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseState GetPassWord(GetPwdModel model);
        /// <summary>
        /// 根据token找到用户名
        /// </summary>
        /// <param name="restToken"></param>
        /// <returns></returns>
        string GetUserEmailByRestToken(string restToken);
        /// <summary>
        /// 通过 token重置密码
        /// </summary>
        /// <param name="token"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        bool ResetPasswordByEmailToken(string token, string NewPassword);

        /// <summary>
        /// 重置手机
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <param name="username"> </param>
        /// <returns></returns>


        BaseState ResetPasswordByPhoneToken(string token, string newPassword, string username);

        BaseState CheckPhoneToken(string username, string token);
        /// <summary>
        /// 在使用
        /// </summary>
        /// <param name="token"></param>
        void ConfirmEmail(string token);
        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="key"></param>
        void UpdateUserAvatar(int Userid, string key);
        /// <summary>
        /// 用户头像
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        string GetUserAvatar(int UserId);
        /// <summary>
        /// 根据用户Uid 取得 头像key
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns></returns>
        string GetUserAvatarByUid(decimal Uid);

        /// <summary>
        /// 通过确认token取得用户userid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        int GetUserIdByConfirmToken(string token);

        /// <summary>
        /// 根据用户编号读取用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserProfileModel GetUserProfileByCache(decimal uid);

        List<UserProfileModel> GetUserProfiles(decimal[] uids);

        UserProfileModel GetUserProfileByNickName(string nickname);
        List<UserProfileModel> GetUserProfileByNickNames(string[] nicknames);

        UserProfileModel GetUserProfileByUserName(string username);
        BaseState ChangeNick(int userid, string nickname);
        BaseState ChangeSex(int userid, int sex);

        System.DateTime? GetRegDateTime(decimal uid);
    }
}
