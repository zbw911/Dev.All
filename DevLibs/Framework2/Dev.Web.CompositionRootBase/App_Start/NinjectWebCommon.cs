// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月02日 17:15
// 
// 修改于：2013年02月05日 17:32
// 文件名：NinjectWebCommon.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using Dev.Web.CompositionRootBase.App_Start;

using WebActivator;

[assembly: PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Dev.Web.CompositionRootBase.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;

    using Dev.Comm.Core;
    using Dev.Comm.Web.Mvc.Formatter;
    using Dev.Crosscutting.Adapter.Adapter;
    using Dev.Crosscutting.Adapter.NetFramework.Adapter;
    using Dev.Web.CompositionRootBase.Tools;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        #region Static Fields

        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            //RegisterMapper(kernel);
            return kernel;
        }

        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            IEnumerable<IRegister> registers = AssemblyManager.GetTypeInstances<IRegister>();

            foreach (IRegister register in registers)
            {
                register.Kernel = kernel;
                register.Register();
            }

            //Jsonp Support
            GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter());

            //for web api
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            //for mvc controllers
            var controllerActivator = new NinjectControllerActivator(kernel);
            var controllerFactory = new DefaultControllerFactory(controllerActivator);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);


        }

        ///// <summary>
        ///// 设置映射,
        ///// </summary>
        ///// <param name="kernel"></param>
        //private static void RegisterMapper(IKernel kernel)
        //{

        //}



        #endregion
    }
}