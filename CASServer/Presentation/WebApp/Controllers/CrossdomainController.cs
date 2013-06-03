using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASServer.Controllers
{
    public class CrossdomainController : Controller
    {
        //
        // GET: /Crossdomain/

        public ActionResult Index()
        {

            var xml = @"<?xml version=""1.0""?>
<cross-domain-policy>
  <allow-access-from domain=""*"" />
</cross-domain-policy>";




            return new ContentResult
                       {
                           Content = xml,
                           ContentType = "text/xml"
                       };
            //return View();            //return View();
        }

    }
}
