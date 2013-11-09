using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    class MyCustom
    {
        //public virtual string Firstname { get; set; }

        //public virtual DateTime Inserted { get; set; }

        public virtual string Lastname { get; set; }

        public int Id { get; set; }
    }
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
            var x = this.customerRepository.ExecuteSqlCommand("update  Customer set lastname={0}", new[] { "a" });



            Console.WriteLine(x);


            var list = this.customerRepository.SqlQuery(typeof(MyCustom), "select * from Customer", new[] { "" });



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


            var list = this.customerRepository.SqlQuery<mytype>("select * from Customer").ToList();



            foreach (mytype custom in list)
            {
                Console.WriteLine(custom.Id + "->" + custom.Lastname);
            }


        }


        [TestMethod]
        public void SQLAdd()
        {
            this.customerRepository.ExecuteSqlCommand("insert into Customer(Firstname,Inserted,Lastname) values({0},{1},{2})", "sqlInsert",
                                                 DateTime.Now, "sql'");

            Assert.AreEqual(1, this.customerRepository.Count<Customer>(x => x.Firstname == "sqlInsert"));

            var count = this.customerRepository.SqlQuery<int>("select count(*) as c from  Customer where Firstname= {0}", "sqlInsert");

            Assert.AreEqual(1, count.First());
            //clean
            this.customerRepository.Delete<Customer>(x => x.Firstname == "sqlInsert");
            this.customerRepository.UnitOfWork.SaveChanges();
            //new Customer { Firstname = "zbw911", Inserted = DateTime.Now, Lastname = "null" });
        }

        [TestMethod]
        public void CleanSqlInsert()
        {
            this.customerRepository.Delete<Customer>(x => x.Firstname == "sqlInsert");
            this.customerRepository.UnitOfWork.SaveChanges();
        }
        [TestMethod]
        public void GetSet()
        {
            var type = Type.GetType("Dev.Data.Test.Domain.Customer");

            dynamic query = this.customerRepository.GetQuery(type);


            foreach (var id in query)
            {
                Console.WriteLine(id.Id);
            }

            Order s;


        }

        class customorder
        {
            public virtual int CustomerId { get; set; }

            public virtual DateTime OrderDate { get; set; }

            public virtual string Firstname { get; set; }

            public virtual DateTime Inserted { get; set; }

            public virtual string Lastname { get; set; }
        }

        [TestMethod]
        public void JoinTest()
        {
            var count = this.customerRepository.SqlQuery<customorder>("select * from  Customer join [order] on   Customer.id = [order].CustomerId ");

            foreach (var i in count)
            {
                Console.WriteLine(i.CustomerId);
                Console.WriteLine(i.Inserted);
            }

        }
        class customorder2
        {
            public Order order { get; set; }
            public Customer Customer { get; set; }
        }

        [TestMethod]
        public void JoinTest2()
        {
            throw new NotSupportedException();
            var count = this.customerRepository.SqlQuery<customorder2>("select * from  Customer join [order] on   Customer.id = [order].CustomerId ");
            // this is Error
            foreach (var i in count)
            {
                Console.WriteLine(i.order.CustomerId);
                Console.WriteLine(i.Customer.Inserted);
            }

        }


        [TestMethod]
        public void MyTestMethod()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic["1"] = "1";

            Console.WriteLine(dic["1"]);
        }

        [TestMethod]
        public void MyTestMethod2()
        {
            string[] a = new[] { "1", "2" };

            var b = a.Select(x => Convert.ToInt16(x));

            foreach (var intse in b)
            {
                Console.WriteLine(intse);
            }
        }




    }
}