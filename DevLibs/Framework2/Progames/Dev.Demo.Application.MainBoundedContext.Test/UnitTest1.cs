using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Demo.Application.MainBoundedContext.Test
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web;

    using AutoMapper;

    using Dev.Demo.Application.MainBoundedContext.AdminModule;
    using Dev.Demo.Entities2.Models;
    using Dev.Demo.EntityDtoProfile;
    using Dev.Demo.ViewModels;

    using Ninject;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var AdminService = AssemblyINIT.Kernel.Get<AdminService>();

            var list = AdminService.GetAdminGroupList();
            Console.WriteLine(list.Count);
            Assert.IsTrue(list.Count > 0);

        }


        [TestMethod]
        public void Test_Login()
        {

            var adminService = AssemblyINIT.Kernel.Get<AdminService>();



            for (int i = 0; i < 1; i++)
            {


                const string Userid = "truename";
                adminService.DeleteAdminByUserId(Userid);

                var state = adminService.AddAdmin(new Entities2.Models.Admin
                                             {
                                                 Userid = "truename",
                                                 Tname = "truename",
                                                 Pwd = "truename",
                                                 Uname = "truename",
                                                 Email = "email",
                                                 Typeid = 14

                                             });

                Assert.IsTrue(state.ErrorCode == 0);

                AdminInfoDto ou;
                state = adminService.Login("truename", "truename", "ip", out ou);


                Assert.IsTrue(state.ErrorCode == 0);


                adminService.DeleteAdminByUserId(Userid);
            }
        }


        [TestMethod]
        public void CreateDbTest()
        {
            var connectionStringName = "DefaultConnection";
            SysManagerContext context1 = new SysManagerContext();
            context1.Database.Connection.ConnectionString =
                        ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;


            context1.Database.CreateIfNotExists();

            var script = ((IObjectContextAdapter)context1).ObjectContext.CreateDatabaseScript();

            Console.WriteLine(script);

        }



        [TestMethod]
        public void MyTestMethod()
        {
            // Create Uri
            Uri uriAddress = new Uri("http://www.contoso.com/index.htm/aource/a.html?东奔西走茜模压" + "#search");
            Console.WriteLine(uriAddress.Fragment);
            Console.WriteLine("Uri {0} the default port ", uriAddress.IsDefaultPort ? "uses" : "does not use");

            Console.WriteLine("The path of this Uri is {0}", uriAddress.GetLeftPart(UriPartial.Path));
            Console.WriteLine("Hash code {0}", uriAddress.GetHashCode());

        }

        [TestMethod]
        public void MyTestMethod2()
        {
            Uri baseUri = new Uri("http://www.contoso.com/");
            Uri myUri = new Uri(baseUri, "catalog/shownew.htm?date=today");

            Console.WriteLine(myUri.AbsolutePath);

        }


        [TestMethod]
        public void MyTestMethodUrl()
        {
            var queryString = HttpUtility.ParseQueryString("a=1&b=2&c=3");
            NameValueCollection AddParms = new NameValueCollection { { "d", "2" } };
            string[] RemoveKeys = new string[] { "b" };
            var r = RebuildUrl(AddParms, RemoveKeys, queryString, "www.google.com");
            Console.WriteLine(r);
        }


        private static string RebuildUrl(
          NameValueCollection addParms, IEnumerable<string> removeKeys, NameValueCollection queryString, string url)
        {

            url += "?";

            if (removeKeys != null)
            {

                foreach (var removeKey in removeKeys)
                {
                    string key = removeKey;
                    var localkeys = queryString.AllKeys.FirstOrDefault(x => x.ToLower() == key.ToLower());

                    queryString.Remove(localkeys);
                }
            }

            if (queryString != null)
            {
                foreach (var item in queryString.AllKeys)
                {
                    string val = queryString[item];

                    var parmkey = "";
                    if (
                        !string.IsNullOrEmpty(
                            parmkey = addParms.AllKeys.FirstOrDefault(x => x.ToLower() == item.ToLower())))
                    {
                        val = addParms[parmkey];
                    }

                    url += item + "=" + val + "&";
                }


                // 1,2,3,4,5,6
                //         5,6,7

                var queryResult = from p in addParms.AllKeys where queryString.AllKeys.All(x => x.ToLower() != p.ToLower()) select p;

                foreach (var key in queryResult)
                {
                    url += key + "=" + addParms[key] + "&";
                }
            }
            url = url.TrimEnd('&').TrimEnd('?');

            return url;
        }
    }
}
