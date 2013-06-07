namespace Dev.Demo.CompositionRoot
{
    using Dev.Data;
    using Dev.Data.Configuration;
    using Dev.Data.ContextStorage;
    using Dev.Demo.Application.MainBoundedContext.AdminModule;
    using Dev.Demo.Application.MainBoundedContext.TestModule;
    using Dev.Demo.Entities2.Models;
    using Dev.Demo.Style2.Repository.Repository;
    using Dev.Web.CompositionRootBase;

    using Ninject;

    public class Registers : RegisterContextBase, IRegister
    {
        private const string DefaultConnection = "DefaultConnection";

        private const string SysManagerContext = "SysManagerContext1";
        public override IKernel Kernel { get; set; }

        public override void Register()
        {

            Kernel.Bind<IDbContextStorage>().To<WebDbContextStorage>();

            //var name = Dev.Comm.Core.AssemblyManager.GetAssemblyFileName("Dev.Demo.Mapping");


            CommonConfig.Instance()
                     .ConfigureDbContextStorage(Kernel.Get<IDbContextStorage>())
                     .ConfigureData<SysManagerContext>(SysManagerContext)
                     //.ConfigureData(DefaultConnection, new[] { name }, false)
                     ;


            // Repository
            RegContextWith<IAdminRepository, AdminRepository>(DefaultConnection);
            RegContextWith<IAdmintypeRepository, AdmintypeRepository>(DefaultConnection);



            //Service 
            RegServiceWith<ITestService, TestService>();
            RegServiceWith<IAdminService, AdminService>();
        }


    }
}