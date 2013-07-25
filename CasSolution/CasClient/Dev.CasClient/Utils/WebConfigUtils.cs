// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年07月19日 10:19
//  
//  修改于：2013年07月19日 13:26
//  文件名：CasClient/Dev.CasClient/WebConfigUtils.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;

namespace Dev.CasClient.Utils
{
    internal static class WebConfigUtils
    {
        #region Class Methods

        /// <summary>
        ///   取得Webconfig中默认的LoginUrl
        /// </summary>
        /// <returns> </returns>
        public static string FormsLoginUrl()
        {
            var authSection =
                (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");


            if (authSection.Mode == AuthenticationMode.Forms)
            {
                var loginurl = authSection.Forms.LoginUrl;

                return loginurl;
            }

            return null;


        }

        /// <summary>
        /// 取得 web.config中配置的HttpModule
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webconfigpath"></param>
        /// <returns></returns>
        public static bool CheckHttpModule<T>(string webconfigpath = "~") where T : IHttpModule
        {
            var wantReg = typeof(T).FullName;
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                var websermodules = configuration.GetSection("system.webServer");

                var xml = websermodules.SectionInformation.GetRawXml();

                if (string.IsNullOrEmpty(xml))
                    return false;


                var xmlFile = XDocument.Load(new StringReader(xml));
                var query = from c in xmlFile.Descendants("modules").Descendants("add") select c;

                foreach (var band in query)
                {
                    var attr = band.Attribute("type");

                    var strType = attr.Value.Split(',').First();

                    if (strType.ToLower() == wantReg.ToLower())
                        return true;
                }
            }
            else
            {
                object o = configuration.GetSection("system.web/httpModules");
                var section = o as HttpModulesSection;
                var models = section.Modules;

                foreach (HttpModuleAction model in models)
                {
                    if (model.Type.Split(',').First() == wantReg)
                        return true;
                }
            }

            return false;
        }


        //public static bool CheckHttpHandler<T>(string webconfigpath = "~") where T : IHttpHandler
        //{
        //    var wantReg = typeof(T).FullName;
        //    var configuration = WebConfigurationManager.OpenWebConfiguration("~");
        //    if (HttpRuntime.UsingIntegratedPipeline)
        //    {
        //        var websermodules = configuration.GetSection("system.webServer");

        //        var xml = websermodules.SectionInformation.GetRawXml();

        //        var xmlFile = XDocument.Load(new StringReader(xml));
        //        var query = from c in xmlFile.Descendants("handlers").Descendants("add") select c;

        //        foreach (var band in query)
        //        {
        //            var attr = band.Attribute("type");

        //            var strType = attr.Value.Split(',').First();

        //            if (strType.ToLower() == wantReg.ToLower())
        //                return true;
        //        }
        //    }
        //    else
        //    {
        //        object o = configuration.GetSection("system.web/httpHandlers");
        //        var section = o as HttpHandlersSection;
        //        var models = section.Handlers;

        //        foreach (HttpHandlerAction model in models)
        //        {
        //            if (model.Type.Split(',').First() == wantReg)
        //                return true;
        //        }
        //    }

        //    return false;
        //}

        #endregion
    }
}