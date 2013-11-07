using System;
using System.IO;
using System.Linq;
using Dev.Data.Configuration;
using Dev.Data.ContextStorage;
using Dev.Data.Test.Domain;
using Dev.Demo.Entities2.Models;
using Infrastructure.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Data.Test
{
    [TestClass]
    public class UnitTestExeSql
    {
        private ICustomerRepository customerRepository;
        [TestInitialize]
        public void Init()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            //DbContextManager.InitStorage(new SimpleDbContextStorage());

            CommonConfig.Instance()
                        .ConfigureDbContextStorage(new SimpleDbContextStorage())
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
                this.customerRepository.Add(
                    new Customer { Firstname = "zbw911", Inserted = DateTime.Now, Lastname = "null" });
            }

            int list = this.customerRepository.GetQuery<Customer>().Count();
            this.customerRepository.UnitOfWork.SaveChanges();
            Console.WriteLine(list);


        }


        [TestMethod]
        public void ExeQueySqlTest()
        {
            var x = customerRepository.ExecuteSqlCommand("update  Customer set lastname={0}", new[] { "a" });



            Console.WriteLine(x);


            var list = customerRepository.SqlQuery(typeof(MyCustom), "select * from Customer", new[] { "" });



            foreach (MyCustom custom in list)
            {
                Console.WriteLine(custom.Id + "->" + custom.Lastname);
            }
        }

        class mytype
        {
            public int Id { get; set; }

            public string Lastname { get; set; }
        }
        [TestMethod]
        public void CustomTypefrotable()
        {


            var list = customerRepository.SqlQuery<mytype>("select * from Customer").ToList();



            foreach (mytype custom in list)
            {
                Console.WriteLine(custom.Id + "->" + custom.Lastname);
            }


        }


        [TestMethod]
        public void SQLAdd()
        {
            customerRepository.ExecuteSqlCommand("insert into Customer(Firstname,Inserted,Lastname) values({0},{1},{2})", "sqlInsert",
                                                 DateTime.Now, "sql'");

            Assert.AreEqual(1, customerRepository.Count<Customer>(x => x.Firstname == "sqlInsert"));

            var count = customerRepository.SqlQuery<int>("select count(*) as c from  Customer where Firstname= {0}", "sqlInsert");

            Assert.AreEqual(1, count.First());
            //clean
            customerRepository.Delete<Customer>(x => x.Firstname == "sqlInsert");
            customerRepository.UnitOfWork.SaveChanges();
            //new Customer { Firstname = "zbw911", Inserted = DateTime.Now, Lastname = "null" });
        }

        [TestMethod]
        public void CleanSqlInsert()
        {
            customerRepository.Delete<Customer>(x => x.Firstname == "sqlInsert");
            customerRepository.UnitOfWork.SaveChanges();
        }

    }

    class MyCustom
    {
        //public virtual string Firstname { get; set; }

        //public virtual DateTime Inserted { get; set; }

        public virtual string Lastname { get; set; }

        public int Id { get; set; }
    }

}
