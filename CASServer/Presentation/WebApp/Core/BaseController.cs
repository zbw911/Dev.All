﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dev.Comm.Web;
using Dev.Comm.Web.Mvc.Filter;

namespace CASServer.Core
{
    using ShareSession;

    public enum SessionName
    {
        验证码
    }

    public class Messager
    {
        public List<string> MessageList { get; set; }

        public string redirectto { get; set; }

        public string to_title { get; set; }

        public int time { get; set; }

        public string return_msg { get; set; }

        public string tips { get; set; }
    }

    [ErrorFilter]
    public class BaseController : Controller
    {
        public T SessionGet<T>(SessionName session)
        {
            return SessionOperateBase.Get<T>(session.GetType().FullName + session.ToString());
        }

        public void SessionSet(SessionName session, object value)
        {
            SessionOperateBase.Set(session.GetType().FullName + session.ToString(), value);
        }

        public void SessionRemove(SessionName session)
        {
            SessionOperateBase.Remove(session.GetType().FullName + session.ToString());
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="messagelist"></param>
        /// <param name="redirectto"></param>
        /// <param name="to_title"></param>
        /// <param name="time"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        protected ActionResult Message(List<string> messagelist, string redirectto = "", string to_title = "跳转", int time = 3, string return_msg = "")
        {
            if (redirectto == "") redirectto = HttpServerInfo.GetUrlReferrer();
            var model = new Messager
            {
                MessageList = messagelist,
                redirectto = redirectto,
                to_title = to_title,
                time = time,
                return_msg = return_msg

            };
            return View("_Messager", model);
        }




        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="strmessage"></param>
        /// <param name="redirectto"></param>
        /// <param name="to_title"></param>
        /// <param name="time"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        protected ActionResult Message(string strmessage, string redirectto = "", string to_title = "跳转", int time = 3, string return_msg = "")
        {
            return this.Message(new List<string> { strmessage }, redirectto, to_title, time, return_msg);
        }
    }
}