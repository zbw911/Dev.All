using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCAS.jasig;
using NCAS.jasig.authentication.principal;

namespace NCAS.Test.Mock
{
    class NinectInit
    {
        public static Ninject.IKernel Kernel;

        public static void init()
        {
            Kernel = new Ninject.StandardKernel();

            Kernel.Bind<CentralAuthenticationService>().To<CentralAuthenticationServiceImpl>();

            Kernel.Bind<Credentials>().To<UsernamePasswordCredentials>();
        }
    }
}
