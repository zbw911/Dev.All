using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using CASServer.Models;

namespace CASServer.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            SimpleMembershipInitializer.Init();
        }

        public class SimpleMembershipInitializer
        {
            private static SimpleMembershipInitializer _initializer;
            private static object _initializerLock = new object();
            private static bool _isInitialized;

            public static void Init()
            {
                LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
            }
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(new MyInitializer());

                try
                {
                    using (var context = new UsersContext())
                    {
                        context.Database.CreateIfNotExists();
                        //if (context.Database.Exists())
                        //{
                        //    //    // Create the SimpleMembership database without Entity Framework migration schema
                        //    //    //((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        //}
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);


                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }



        public class MyInitializer : CreateDatabaseIfNotExists<UsersContext>
        {
            protected override void Seed(UsersContext context)
            {
                //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_UserProfile_Uid ON  UserProfile (uid)");
                //context.Database.ExecuteSqlCommand("ALTER TABLE USERPROFILE ALTER COLUMN [UID] DECIMAL(18,0)");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('UserExtend', RESEED, 10000)");
            }
        }
    }
}
