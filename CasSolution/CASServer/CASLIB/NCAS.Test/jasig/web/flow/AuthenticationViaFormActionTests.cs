using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCAS.Test.Mock;
using NCAS.jasig;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.web.flow;

namespace NCAS.Test.jasig.web.flow
{
    [TestClass]
    public class AuthenticationViaFormActionTests
    {
        private AuthenticationViaFormAction _authenticationViaFormAction;
        private CentralAuthenticationService CentralAuthenticationService = new CentralAuthenticationServiceImpl();
        [TestInitialize]
        public void init()
        {
            //MockHttp.FakeHttpContext("lt=loginticket");

            //HttpContext.Current.Session["loginTicket"] = "loginticket";
            //HttpContext.Current.Request.Form["lt"] = "loginticket";


            MockHttp.FakeHttpContext("");

            _authenticationViaFormAction.setCentralAuthenticationService(CentralAuthenticationService);

            //HttpContext.Current.Session["loginTicket"] = "";
        }

        [TestMethod]
        public void TestMethod1()
        {
            Credentials credentials = new NCAS.jasig.authentication.principal.UsernamePasswordCredentials();
            ((UsernamePasswordCredentials)credentials).setPassword("test");
            ((UsernamePasswordCredentials)credentials).setUsername("test");

            _authenticationViaFormAction.submit(HttpContext.Current, credentials, null);
        }
    }
}
