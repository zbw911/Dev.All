using System;
using System.IO;
using Application.MainBoundedContext.UserModule;
using WebMatrix.WebData;

namespace CASServer.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;


    using CASServer.Models;

    using Dev.CasServer;
    using Dev.CasServer.Authenticator;

    public class CasController : Controller
    {
        #region Fields

        private static string strJsSDK = null;

        private readonly ICasAuthenticator casAuthenticator;
        private readonly CasServer _casServer;

        private readonly CasServer casServer;
        private readonly IUserService _userService;

        private readonly IUserValidate userValidate;

        #endregion

        #region Constructors and Destructors

        public CasController(IUserValidate UserValidate, ICasAuthenticator CasAuthenticator, CasServer CasServer, IUserService userService)
        {
            if (UserValidate == null) throw new ArgumentNullException("UserValidate");
            if (CasAuthenticator == null) throw new ArgumentNullException("CasAuthenticator");
            this.casAuthenticator = CasAuthenticator;

            this.userValidate = UserValidate;

            this.casServer = CasServer;
            _userService = userService;
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Login(string service)
        {
            this.ViewBag.service = service;
            //如果 serivce 不为空是,有必要返回
            if (!string.IsNullOrEmpty(service))
                this.ViewBag.ReturnUrl = Request.RawUrl;

            //Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            string returl;
            bool redrect = this.casServer.HandlePageLoad(service, out returl);



            if (redrect)
            {

                return this.Redirect(returl);
            }

            return this.View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string service)
        {
            this.ViewBag.service = service;

            //if (!WebSecurity.IsConfirmed(model.UserName))
            //{

            //    return RedirectToAction("EmailActivation", "Account", new { email = model.UserName, type = 1 });
            //}


            string redirectUrl;
            // check if this is a CAS request and handle it
            if (ModelState.IsValid && this.casServer.HandlePageLogin(
                service, model.UserName, model.Password, model.RememberMe, out redirectUrl))
            {
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    // if not, do it the FormsAuthentication way
                    //FormsAuthentication.RedirectFromLoginPage(model.UserName, model.RememberMe);

                    return Redirect(Dev.CasServer.Configuration.CasServerConfiguration.Config.DefaultUrl);
                }
                else
                {
                    return this.Redirect(redirectUrl);
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            //ViewBag.ErrorMessage = "提供的用户名或密码不正确。";
            ModelState.AddModelError("", "提供的用户名或密码不正确。");
            return View(model);
        }

        public ActionResult Logout(string service)
        {
            this.casServer.HandleLogoutRequest();
            if (string.IsNullOrEmpty(service))
            {
                return this.RedirectToAction("login");
            }

            return this.Redirect(service);
        }

        public ActionResult ServiceValidate(string service, string ticket)
        {
            string strResponse = this.casServer.HandleServiceValidateRequest(service, ticket);
            return this.Content(strResponse);
        }

        public ActionResult Validate(string service, string ticket)
        {
            string strResponse = this.casServer.HandleValidateRequest(service, ticket);
            return this.Content(strResponse);
        }


        public ActionResult JsSDK()
        {

            return JavaScript(Js());


        }

        private string Js()
        {
            //if (strJsSDK == null)
            //{
                string content;
                ViewEngineResult view = null;

                view = ViewEngines.Engines.FindPartialView(ControllerContext, "JsSDK");
                using (var writer = new StringWriter())
                {
                    ViewData["basurl"] = Dev.Comm.Web.HttpServerInfo.BaseUrl;
                    var context = new ViewContext(ControllerContext, view.View, ViewData, TempData, writer);
                    view.View.Render(context, writer);

                    writer.Flush();
                    content = writer.ToString();
                }

                strJsSDK = content;
            //}
            return strJsSDK;
        }

        #endregion
    }
}