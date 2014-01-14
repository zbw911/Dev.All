using System;
using System.IO;
using System.Linq;
using System.Threading;
using Dev.Data.Configuration;
using Dev.Data.ContextStorage;
using Dev.Data.Test.Domain;
using Dev.Demo.Entities2.Models;
using Infrastructure.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Data.Test
{
    [TestClass]
    public class UnitTestMutiThread
    {
        private CustomerRepository customerRepository;
        [TestInitialize]
        public void Init()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            //DbContextManager.InitStorage(new SimpleDbContextStorage());

            CommonConfig.Instance()
                .ConfigureDbContextStorage(new ThreadDbContextStorage())
                .ConfigureData<MyDbContext>("DefaultConnection")
                .ConfigureData<SysManagerContext>("DefaultConnection1");

            //config.ConfigureData<MyDbContext>("DefaultConnection");

            this.customerRepository = new CustomerRepository("DefaultConnection");

        }
        [TestMethod]
        public void TestMethod1()
        {

            for (int i = 0; i < 10; i++)
            {

                InThread(i);
            }


            Thread.Sleep(20 * 1000);

        }


        private void InThread(int i)
        {
            var t = new Thread(() =>
            {

                Console.WriteLine("start thread " + "=>" + i);
                //for (int i = 0; i < 10; i++)
                //{
                //    this.customerRepository.Add(
                //        new Customer { Firstname = "zbw911", Inserted = DateTime.Now, Lastname = "null" });
                //}

                try
                {
                    int list = this.customerRepository.GetQuery<Customer>().Count();
                    //this.customerRepository.UnitOfWork.SaveChanges();
                    Console.WriteLine(i + "=>" + list);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }




            });

            t.IsBackground = true;

            t.Start();
        }
    }
}
