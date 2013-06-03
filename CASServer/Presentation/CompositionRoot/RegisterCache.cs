using Dev.Framework.Cache;
using Dev.Framework.Cache.Impl;
using Dev.Web.CompositionRootBase;
 
using Ninject;

namespace CompositionRoot
{
    class RegisterCache : IRegister
    {

        public void Register()
        {  //暂时先使用 httpruntime cache
            this.Kernel.Bind<ICacheState>().To<HttpRuntimeCache>();
            this.Kernel.Bind<ICacheWraper>().To<CacheWraper>();
        }

        public IKernel Kernel { get; set; }
    }
}
