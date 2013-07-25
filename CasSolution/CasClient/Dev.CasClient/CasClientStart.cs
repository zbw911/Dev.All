// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年07月18日 9:43
//  
//  修改于：2013年07月19日 9:35
//  文件名：CasClient/Dev.CasClient/CasClientStart.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Routing;
using System.Xml.Linq;
using Dev.CasClient;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(CasClientStart), "Start")]

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

            //return;
            ////MachineKeySection section = (MachineKeySection)WebConfigurationManager.GetSection("system.web/machineKey");

            ////var key = section.DecryptionKey;
            //if (Utils.WebConfigUtils.CheckHttpModule<CasClientModule>())
            //{
            //    return;
            //}

            //如果没有在Web.config 中声明，就使用动态注册
            DynamicModuleUtility.RegisterModule(typeof(CasClientModule));



        }

        #endregion
    }


}