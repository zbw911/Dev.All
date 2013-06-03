using Application.CasHander;
using Dev.CasServer;
using Dev.CasServer.Authenticator;
using Domain.MainBoundedContext.UserExtend.Repository;
using Domain.MainBoundedContext.webpages_Membership.Repository;

namespace CompositionRoot
{
    using Application.MainBoundedContext.UserModule;

    using Dev.Data;
    using Dev.Data.Configuration;
    using Dev.Data.ContextStorage;
    using Dev.Web.CompositionRootBase;

    using Domain.Entities.Models;
    using Domain.MainBoundedContext.UserProfile.Repository;

    using Ninject;

    public class Registers : RegisterContextBase, IRegister
    {
        private const string DefaultConnection = "DefaultConnection";

        //private const string SysManagerContext = "SysManagerContext1";
        public override IKernel Kernel { get; set; }

        public override void Register()
        {

            this.Kernel.Bind<IDbContextStorage>().To<WebDbContextStorage>();

            //var name = Dev.Comm.Core.AssemblyManager.GetAssemblyFileName("Dev.Demo.Mapping");


            CommonConfig.Instance()
                .ConfigureDbContextStorage(this.Kernel.Get<IDbContextStorage>())
                .ConfigureData<passportContext>(DefaultConnection);




            // Repository
            this.RegContextWith<IUserProfileRepository, UserProfileRepository>(DefaultConnection);
            this.RegContextWith<IUserExtendRepository, UserExtendRepository>(DefaultConnection);
            this.RegContextWith<IWebpagesMembershipRepository, WebpagesMembershipRepository>(DefaultConnection);
             



            ////Service 
            this.RegServiceWith<IUserService, UserService>();





            //CAS
            RegServiceWith<ICasAuthenticator, FormsCasAuthenticator>();
            RegServiceWith<IUserValidate, UserValidateFake>();
        }


    }
}