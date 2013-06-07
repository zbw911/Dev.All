// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月02日 17:15
// 
// 修改于：2013年02月05日 17:32
// 文件名：NinjectControllerActivator.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Web.CompositionRootBase.Tools
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Ninject;

    /// <summary>
    ///     用于MVC的 Activator
    /// </summary>
    public class NinjectControllerActivator : IControllerActivator
    {
        #region Constructors and Destructors

        public NinjectControllerActivator(IKernel kernel)
        {
            this.Kernel = kernel;
        }

        #endregion

        #region Public Properties

        public IKernel Kernel { get; private set; }

        #endregion

        #region Public Methods and Operators

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            var type = Kernel.Get(controllerType);
          
            return (IController)this.Kernel.TryGet(controllerType);
        }

        #endregion
    }
}