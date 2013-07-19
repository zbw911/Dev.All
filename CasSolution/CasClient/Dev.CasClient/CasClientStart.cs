// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年07月18日 9:43
//  
//  修改于：2013年07月19日 9:35
//  文件名：CasClient/Dev.CasClient/CasClientStart.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using Dev.CasClient;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof (CasClientStart), "Start")]

namespace Dev.CasClient
{
    /// <summary>
    ///   CAS 自动启动
    /// </summary>
    public static class CasClientStart
    {
        #region Class Methods

        /// <summary>
        ///   Starts the application
        /// </summary>
        public static void Start()
        {
            var wantReg = typeof (CasClientModule).FullName;
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                var websermodules = configuration.GetSection("system.webServer");

                var xml = websermodules.SectionInformation.GetRawXml();

                var xmlFile = XDocument.Load(new StringReader(xml));
                var query = from c in xmlFile.Descendants("modules").Descendants("add") select c;

                foreach (var band in query)
                {
                    var attr = band.Attribute("type");

                    var strType = attr.Value.Split(',').First();

                    if (strType.ToLower() == wantReg.ToLower())
                        return;
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
                        return;
                }
            }

            //如果没有在Web.config 中声明，就使用动态注册
            DynamicModuleUtility.RegisterModule(typeof (CasClientModule));
        }

        #endregion
    }
}