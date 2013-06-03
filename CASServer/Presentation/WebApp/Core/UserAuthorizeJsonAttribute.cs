using System.ComponentModel;
using System.Web.Mvc;
using Application.Dto;

namespace CASServer.Core
{
    public class UserAuthorizeJsonAttribute : ActionFilterAttribute
    {
        [DefaultValue(-1000)]
        public int UnAuthorizeState { get; set; }
        [DefaultValue("未登录")]
        public string UnAuthorizeMessage { get; set; }


        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    if (filterContext.Result is JsonResult && !filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        var json = new JsonResult
        //                       {
        //                           Data = new BaseState(-1, "未登录"),
        //                           JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //                       };

        //        filterContext.Result = json;
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult && !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var json = new JsonResult
                               {
                                   Data = new BaseState(-1, "未登录"),
                                   JsonRequestBehavior = JsonRequestBehavior.AllowGet
                               };

                filterContext.Result = json;
            }
            base.OnActionExecuted(filterContext);
        }



    }
}