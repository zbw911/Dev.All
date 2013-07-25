using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dev.CasClient.UserAuthenticate;

namespace Dev.CasClientWebForm.App_Code
{
    public class MyUserAuthenticate : IUserAuthenticate
    {


        public void CurUserLoginOut()
        {

            if (HttpContext.Current.Session == null)
            {
                HttpContext.Current.Response.Write("null session");
                return;
            }
            HttpContext.Current.Session.Clear();

        }

        public bool GetUserIsAuthenticated()
        {
            var a = HttpContext.Current.Session["a"];
            return a != null;

        }

        public void SignUserLogin(string strUserName, Dictionary<string, string> extDatas)
        {
            HttpContext.Current.Session["a"] = strUserName;

        }

        ///// <summary>
        /////   登出
        ///// </summary>
        //public void CurUserLoginOut()
        //{
        //    HttpContext.Current.Session.Clear();
        //}

        ///// <summary>
        /////   是否已经验证通过
        ///// </summary>
        ///// <returns> </returns>
        //public bool GetUserIsAuthenticated()
        //{
        //    var a = HttpContext.Current.Session["a"];
        //    return a != null;
        //}

        ///// <summary>
        /////   标识用户登录
        ///// </summary>
        ///// <param name="strUserName"> </param>
        ///// <param name="extDatas"> </param>
        //public void SignUserLogin(string strUserName, Dictionary<string, string> extDatas)
        //{
        //    //throw new NotImplementedException();

        //    HttpContext.Current.Session["a"] = strUserName;
        //}
    }
}