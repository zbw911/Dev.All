using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dev.ClientWeb.Controllers
{
    [NoCache]
    //[CacheMaxAge]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "修改此模板以快速启动你的 ASP.NET MVC 应用程序。";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }


    }


    /// <summary>
    /// 加入 Cache-Control: no-cache
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            base.OnResultExecuted(filterContext);
        }
    }
    /// <summary>
    /// Http头加入 MaxAge={}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CacheMaxAgeAttribute : ActionFilterAttribute
    {

        public TimeSpan MaxAge { get; set; }



        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetMaxAge(MaxAge);

            //filterContext.HttpContext.Response.CacheControl = "private";
            base.OnResultExecuted(filterContext);
        }
    }
}
