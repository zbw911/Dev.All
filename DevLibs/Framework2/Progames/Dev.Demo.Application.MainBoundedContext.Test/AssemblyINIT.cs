using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Demo.Application.MainBoundedContext.Test
{
    using System.Configuration;
    using System.IO;
    using System.Windows.Forms;

    using Dev.Comm.Core;
    using Dev.Crosscutting.Adapter.Adapter;
    using Dev.Crosscutting.Adapter.NetFramework.Adapter;
    using Dev.Data;
    using Dev.Data.Configuration;
    using Dev.Data.ContextStorage;
    using Dev.Data.Infras;
    using Dev.Demo.Entities2.Models;
    using Dev.Demo.EntityDtoProfile;
    using Dev.Demo.Style2.Repository.Repository;

    using Ninject;

    /// <summary>
    /// UnitTest2 的摘要说明
    /// </summary>
    [TestClass]
    public class AssemblyINIT
    {

        public AssemblyINIT()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;


        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        //[AssemblyInitialize]
        //public static void TestMethod1_1()
        //{
        //    int a = 1;
        //    var b = a;
        //    Console.WriteLine(b);
        //}

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            //UserProfie u = new UserProfie();
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            const string DefaultConnection = "SystemMangerConnection";

            Kernel = new StandardKernel();

            Kernel.Bind<IDbContextStorage>().To<SimpleDbContextStorage>();



            CommonConfig.Instance()
                     .ConfigureDbContextStorage(Kernel.Get<IDbContextStorage>())
                     .ConfigureData<SysManagerContext>(DefaultConnection)
                //.ConfigureData(DefaultConnection, new[] { name }, false)
                     ;


            // Repositorys
            RegContextWith<IAdminRepository, AdminRepository>(DefaultConnection);
            RegContextWith<IAdmintypeRepository, AdmintypeRepository>(DefaultConnection);





            var typeAdapterFactory = new AutomapperTypeAdapterFactory();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);

        }


        /// <summary>
        ///     使用连接名进行的注入，要在 Manager中进行初始化Context,这个Context将是存于 ContextStorage中的，就要注意线程安全，特别是使用并发方法的时候，
        /// </summary>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TImp"></typeparam>
        /// <param name="connectionStringName"></param>
        public static void RegContextWith<TI, TImp>(string connectionStringName) where TImp : TI, IRepository
        {
            Kernel.Bind<TI>().To<TImp>().WithConstructorArgument("connectionStringName", connectionStringName);
        }



        public static IKernel Kernel { get; set; }
    }
}
