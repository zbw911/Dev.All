using System;
using Ninject;
using Ninject.Modules;

namespace Dev.Web.CompositionRootBase.Tools
{
    /// <summary>
    /// Lazy《》绑定支持
    /// </summary>
    public class LazyBinding : NinjectModule
    {
        public override void Load()
        {
            this.Bind(typeof(Lazy<>))
                .ToMethod(
                    context =>
                    ((ILazyLoader)Activator.CreateInstance(typeof(LazyLoader<>).MakeGenericType(context.GenericArguments),
                                                           new object[] { context.Kernel })).Loader);
        }

        public interface ILazyLoader
        {
            object Loader { get; }
        }

        public class LazyLoader<T> : ILazyLoader
        {
            private readonly IKernel _kernel;
            private static readonly Func<IKernel, Lazy<T>> _lazyLoader = k => new Lazy<T>(() => k.Get<T>());

            public LazyLoader(IKernel kernel)
            {
                if (kernel == null)
                    throw new ArgumentNullException("kernel");

                this._kernel = kernel;
            }

            public object Loader { get { return _lazyLoader(this._kernel); } }
        }
    }
}