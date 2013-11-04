// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月05日 12:31
// 
// 修改于：2013年02月05日 17:32
// 文件名：RegistersHelper.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using Ninject.Web.Common;

namespace Dev.Web.CompositionRootBase
{
    using System.Data.Entity;

    using Dev.Data.Infras;

    using Ninject;

    //public static class RegistersHelper
    //{
    //    #region Public Methods and Operators

    //    /// <summary>
    //    ///     使用连接名进行的注入，要在 Manager中进行初始化Context,这个Context将是存于 ContextStorage中的，就要注意线程安全，特别是使用并发方法的时候，
    //    /// </summary>
    //    /// <typeparam name="TI"></typeparam>
    //    /// <typeparam name="TImp"></typeparam>
    //    /// <param name="kernel"></param>
    //    /// <param name="connectionStringName"></param>
    //    public static void RegContextWith<TI, TImp>(IKernel kernel, string connectionStringName)
    //        where TImp : TI, IRepository
    //    {
    //        kernel.Bind<TI>().To<TImp>().WithConstructorArgument("connectionStringName", connectionStringName);
    //    }

    //    /// <summary>
    //    ///     使用这个注入，直接注入的是的 XXXXXRepository，这个对像是理论上线程的安全的
    //    /// </summary>
    //    /// <typeparam name="TI"></typeparam>
    //    /// <typeparam name="TImp"></typeparam>
    //    /// <param name="kernel"></param>
    //    /// <param name="dbContext"></param>
    //    public static void RegContextWith<TI, TImp>(IKernel kernel, DbContext dbContext) where TImp : TI, IRepository
    //    {
    //        kernel.Bind<TI>().To<TImp>().WithConstructorArgument("context", dbContext);
    //    }

    //    #endregion
    //}

    /// <summary>
    /// 绑定基本方法
    /// </summary>
    public abstract class RegisterContextBase : IRegister
    {
        #region Public Properties

        public abstract IKernel Kernel { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     使用连接名进行的注入，要在 Manager中进行初始化Context,这个Context将是存于 ContextStorage中的，就要注意线程安全，特别是使用并发方法的时候，
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        /// <param name="connectionStringName"></param>
        protected void RegContextWith<TI, TImp>(string connectionStringName) where TImp : TI, IRepository
        {
            this.Kernel.Bind<TI>().To<TImp>().InRequestScope()/*.InRequestScope()*/.WithConstructorArgument("connectionStringName", connectionStringName);
        }


        /// <summary>
        ///     使用默认连接名“DefaultConnection”进行的注入，要在 Manager中进行初始化Context,这个Context将是存于 ContextStorage中的，就要注意线程安全，特别是使用并发方法的时候，
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        protected void RegContextWith<TI, TImp>() where TImp : TI, IRepository
        {
            RegContextWith<TI, TImp>("DefaultConnection");
        }

        /// <summary>
        ///     使用这个注入，直接注入的是的 XXXXXRepository，这个对像是理论上线程的安全的
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        /// <param name="dbContext"></param>
        protected void RegContextWith<TI, TImp>(DbContext dbContext) where TImp : TI, IRepository
        {
            this.Kernel.Bind<TI>().To<TImp>().InRequestScope().WithConstructorArgument("context", dbContext);
        }


        /// <summary>
        /// 绑定 Service 
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        protected void RegServiceWith<TI, TImp>() where TImp : TI
        {
            this.Kernel.Bind<TI>().To<TImp>().InRequestScope();
        }

        public abstract void Register();

        #endregion
    }
}