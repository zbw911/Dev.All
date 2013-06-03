using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using Application.Dto;
using Application.MainBoundedContext;
using Application.MainBoundedContext.UserModule;
using CASServer.Core;
using CASServer.Filters;
using CASServer.Models;
using Dev.CasServer.Configuration;
using Dev.Comm;
using Dev.Comm.Web.Mvc.Filter;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace CASServer.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #region 中转URL

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return RedirectToAction("Login", "CAS");
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            return new HttpUnauthorizedResult();
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            return RedirectToAction("Logout", "CAS");
        }

        #endregion

        #region 注册

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            string code = (SessionGet<string>(SessionName.验证码) ?? "").ToLower();
            SessionRemove(SessionName.验证码);

            if (model.Validcode.ToLower() != code)
            {
                ModelState.AddModelError("Validcode", "验证码不正确");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (_userService.UserNickExist(model.NickName))
                {
                    ModelState.AddModelError("NickName", "昵称已经被使用");
                    return View(model);
                }

                // 尝试注册用户
                try
                {
                    string token = WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                                                                    new
                                                                        {
                                                                            Email = model.UserName,
                                                                            model.NickName,
                                                                            model.Sex,
                                                                            model.Province,
                                                                            model.City,
                                                                            model.ProvinceName,
                                                                            model.CityName
                                                                        }, true);

                    int userid = WebSecurity.GetUserId(model.UserName);

                    decimal uid = _userService.InserOrUpdateExtUid(userid);


                    SystemMessagerManager.SendValidateMail(model.UserName, model.NickName, "邮件激活",
                                                           SystemMessagerManager.ActMessage(model.UserName,
                                                                                            model.NickName, token));


                    return RedirectToAction("EmailActivation", new { email = model.UserName });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Activation(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("token can't null");
            }

            //if (WebSecurity.IsConfirmed(email))


            if (_userService.IsConfirmedByToken(token))
                return Message("此帐户已经激活", "/CAS/Login");

            if (WebSecurity.ConfirmAccount(accountConfirmationToken: token))
            {
                this._userService.ConfirmEmail(token);
                return Message("激活成功", "/CAS/Login");
            }

            return Content("激活失败");
        }

        [AllowAnonymous]
        public ActionResult EmailActivation(string email, int type = 0)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("email can't null");
            }

            ViewBag.type = type;
            //if (WebSecurity.IsConfirmed(email))


            if (WebSecurity.IsConfirmed(email))
            {
                return Message("此帐户已经激活", "/CAS/Login");
            }


            return View(model: email);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResendToken(string email)
        {
            if (WebSecurity.IsConfirmed(email))
            {
                return Json(false);
            }

            string NickName = _userService.GetNickNameByUserName(email);

            string token = _userService.GetTokenByUserName(email);

            SystemMessagerManager.SendValidateMail(email, NickName, "邮件激活",
                                                   SystemMessagerManager.ActMessage(email, NickName, token));

            return Json(true);
        }


        [AllowAnonymous]
        public bool Check(string username)
        {
            return WebSecurity.UserExists(username);
        }

        [AllowAnonymous]
        public bool CheckNick(string nickname)
        {
            return _userService.UserNickExist(nickname);
        }

        #endregion

        #region 验证码
        /// <summary>
        ///     验证码，当然要匿名了啊
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Code()
        {
            ////生成验证码
            var validateCode = new ValidateCode();
            string code = validateCode.CreateValidateCode(4, 0);
            SessionSet(SessionName.验证码, code);
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        #endregion



        #region 外部登录模块

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // 只有在当前登录用户是所有者时才取消关联帐户
            if (ownerAccount == User.Identity.Name)
            {
                // 使用事务来防止用户删除其上次使用的登录凭据
                using (
                    var scope = new TransactionScope(TransactionScopeOption.Required,
                                                     new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        #region  Manage
        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "已更改你的密码。"
                    : message == ManageMessageId.SetPasswordSuccess
                          ? "已设置你的密码。"
                          : message == ManageMessageId.RemoveLoginSuccess
                                ? "已删除外部登录。"
                                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // 在某些失败方案中，ChangePassword 将引发异常，而不是返回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword,
                                                                             model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                    }
                }
            }
            else
            {
                // 用户没有本地密码，因此将删除由于缺少
                // OldPassword 字段而导致的所有验证错误
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        #endregion


        #region ChangePWD

        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess
                   ? "已更改你的密码。"
                   : message == ManageMessageId.SetPasswordSuccess
                         ? "已设置你的密码。"
                         : message == ManageMessageId.RemoveLoginSuccess
                               ? "已删除外部登录。"
                               : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("ChangePassword", new { @in = Dev.Comm.Web.DevRequest.GetInt("in", 0) });
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // 在某些失败方案中，ChangePassword 将引发异常，而不是返回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword,
                                                                             model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess, @in = Dev.Comm.Web.DevRequest.GetInt("in", 0) });
                    }
                    else
                    {
                        ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                    }
                }
            }
            else
            {
                // 用户没有本地密码，因此将删除由于缺少
                // OldPassword 字段而导致的所有验证错误
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        #endregion


        #region Binding
        public ActionResult Binding()
        {
            return View();
        }
        #endregion

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result =
                OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToCas(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // 如果当前用户已登录，则添加新帐户
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                _userService.InserOrUpdateExtUid(WebSecurity.GetUserId(User.Identity.Name));
                return RedirectToCas(returnUrl);
            }
            else
            {
                // 该用户是新用户，因此将要求该用户提供所需的成员名称
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation",
                            new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated ||
                !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // 将新用户插入到数据库
                using (var db = new UsersContext())
                {
                    UserProfile user =
                        db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // 检查用户是否已存在
                    if (user == null)
                    {
                        // 将名称插入到配置文件表
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        _userService.InserOrUpdateExtUid(WebSecurity.GetUserId(model.UserName));
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToCas(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "用户名已存在。请输入其他用户名。");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            var externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                                       {
                                           Provider = account.Provider,
                                           ProviderDisplayName = clientData.DisplayName,
                                           ProviderUserId = account.ProviderUserId,
                                       });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 ||
                                       OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #endregion

        #region 找回密码

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetPwd()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetPwd(GetPwdModel model)
        {
            string code = (SessionGet<string>(SessionName.验证码) ?? "").ToLower();
            SessionRemove(SessionName.验证码);

            if (model.Validcode.ToLower() != code)
            {
                ModelState.AddModelError("Validcode", "验证码不正确");
                return View(model);
            }

            // 用户没有本地密码，因此将删除由于缺少
            // OldPassword 字段而导致的所有验证错误
            ModelState state = ModelState["GetPwdType"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                BaseState bs = _userService.GetPassWord(model);

                if (bs.ErrorCode == 0)
                {
                    if (model.GetPwdType == 0)
                        return View("_GetPwdMailSucess", model: model.UserName);
                    else
                        return View("_GetPwdNext", model: bs.ErrorMessage);
                }
                else
                {
                    if (bs.ErrorCode == -3)
                        return Message("此用户还未激活，激活后继续", Url.Action("EmailActivation", new { email = model.UserName }));

                    ModelState.AddModelError("", "" + bs.ErrorMessage);
                }
            }


            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResendSms(string username)
        {
            BaseState bs = _userService.GetPassWord(new GetPwdModel
                                                        {
                                                            UserName = username,
                                                            GetPwdType = 1
                                                        });

            return Json(bs, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPwdByPhone(string username, string token)
        {
            ViewBag.username = username;
            ViewBag.token = token;

            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPwdByPhone(string username, string token, LocalPasswordModel model)
        {
            ViewBag.username = username;
            ViewBag.token = token;
            // 用户没有本地密码，因此将删除由于缺少
            // OldPassword 字段而导致的所有验证错误
            ModelState state = ModelState["OldPassword"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (string.IsNullOrEmpty(username))
                ModelState.AddModelError("", "username can't null ");
            if (string.IsNullOrEmpty(token))
                ModelState.AddModelError("", "token can't null");


            if (ModelState.IsValid)
            {
                BaseState bs = _userService.ResetPasswordByPhoneToken(token, model.NewPassword, username);

                if (bs.ErrorCode == 0)
                {
                    return Message("重置成功", Url.Action("Login", "Cas"));
                }
                else
                {
                    ModelState.AddModelError("", bs.ErrorMessage);
                }
            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Checktoken(string username, string token)
        {
            BaseState bs = _userService.CheckPhoneToken(username, token);

            return Json(bs, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RestPwd(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Message("不正确定找回密码链接");
            }

            string email = _userService.GetUserEmailByRestToken(restToken: token);

            if (string.IsNullOrEmpty(email))
                return Message("找回密码链接已经过期", Url.Action("getpwd"));

            ViewBag.Email = email;
            ViewBag.token = token;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RestPwd(string token, LocalPasswordModel model)
        {
            // 用户没有本地密码，因此将删除由于缺少
            // OldPassword 字段而导致的所有验证错误
            ModelState state = ModelState["OldPassword"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                bool isok =
                    _userService.ResetPasswordByEmailToken(token, NewPassword: model.NewPassword);

                if (isok)
                    return Message("重置密码成功", Url.Action("login", "cas"));
                else
                    return Message("重置密码失败", Url.Action("RestPwd", new { token }));
            }
            return View(model);
        }

        #endregion

        #region 帮助程序

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private ActionResult RedirectToCas(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = CasServerConfiguration.Config.DefaultUrl;
                }
                return RedirectToAction("Login", "CAS", new { service = returnUrl });
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入其他电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion

        #region 修改NickName
        [AllowAnonymous, JsonpFilter, UserAuthorizeJson, ActionAllowCrossSiteJson]
        public ActionResult ChangeNick(string nickname)
        {
            BaseState bs = this._userService.ChangeNick(userid: WebSecurity.CurrentUserId, nickname: nickname);
            return Json(bs);
        }


        #endregion

        #region 修改 Sex
        [AllowAnonymous, JsonpFilter, UserAuthorizeJson, ActionAllowCrossSiteJson]
        public ActionResult ChangeSex(int sex)
        {
            BaseState bs = this._userService.ChangeSex(userid: WebSecurity.CurrentUserId, sex: sex);
            return Json(bs);
        }
        #endregion

        [AllowAnonymous, JsonpFilter, UserAuthorizeJson, ActionAllowCrossSiteJson]
        public ActionResult A()
        {
            return Json("json", JsonRequestBehavior.AllowGet);
        }


    }
}