// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月02日 17:15
// 
// 修改于：2013年02月05日 17:32
// 文件名：NinjectDependencyScope.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Web.CompositionRootBase.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;

    using Ninject;
    using Ninject.Syntax;

    /// <summary>
    ///     用于 WebApi的 注入依赖
    /// </summary>
    public class NinjectDependencyScope : IDependencyScope
    {
        #region Fields

        private IResolutionRoot resolver;

        #endregion

        #region Constructors and Destructors

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            var disposable = this.resolver as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            this.resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (this.resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return this.resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this.resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return this.resolver.GetAll(serviceType);
        }

        #endregion
    }


    /// <summary>
    /// This class is the resolver, but it is also the global scope
    /// so we derive from NinjectScope.
    /// </summary>
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        #region Fields

        private readonly IKernel kernel;

        #endregion

        #region Constructors and Destructors

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        #endregion

        #region Public Methods and Operators

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this.kernel.BeginBlock());
        }

        #endregion
    }
}