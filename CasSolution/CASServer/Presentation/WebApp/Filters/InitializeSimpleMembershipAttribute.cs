﻿// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/InitializeSimpleMembershipAttribute.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Data.Entity;
using System.Threading;
using System.Web.Mvc;
using CASServer.Models;
using WebMatrix.WebData;

namespace CASServer.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        #region Instance Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            SimpleMembershipInitializer.Init();
        }

        #endregion

        #region Nested type: MyInitializer

        public class MyInitializer : CreateDatabaseIfNotExists<UsersContext>
        {
            #region Instance Methods

            protected override void Seed(UsersContext context)
            {
                //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_UserProfile_Uid ON  UserProfile (uid)");
                //context.Database.ExecuteSqlCommand("ALTER TABLE USERPROFILE ALTER COLUMN [UID] DECIMAL(18,0)");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('UserExtend', RESEED, 10000)");
            }

            #endregion
        }

        #endregion

        #region Nested type: SimpleMembershipInitializer

        public class SimpleMembershipInitializer
        {
            #region Readonly & Static Fields

            private static SimpleMembershipInitializer _initializer;
            private static object _initializerLock = new object();
            private static bool _isInitialized;

            #endregion

            #region C'tors

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

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName",
                                                             autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588",
                        ex);
                }
            }

            #endregion

            #region Class Methods

            public static void Init()
            {
                LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
            }

            #endregion
        }

        #endregion
    }
}