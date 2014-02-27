using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;

namespace Dev.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Dev.Data.ContextStorage;
    using Dev.Data.Infras;

    using Infrastructure.Tests.Data;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     TableClassMapTest 的摘要说明
    /// </summary>
    [TestClass]
    public class TableClassMapTest
    {
        #region Fields

        private ICustomerRepository customerRepository;

        private IRepository repository;

        #endregion

        #region Public Properties

        /// <summary>
        ///     获取或设置测试上下文，该上下文提供
        ///     有关当前测试运行及其功能的信息。
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void SetUp()
        {
            Console.WriteLine(this.TestContext.TestRunDirectory);
            Console.WriteLine(this.TestContext.TestDir);
            Console.WriteLine(this.TestContext.TestDeploymentDir);
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            DbContextManager.InitStorage(new SimpleDbContextStorage());
            DbContextManager.Init("DefaultDb", new[] { "Dev.Data.Test" }, true);

            this.customerRepository = new CustomerRepository("DefaultConnection");
            this.repository = new GenericRepository("DefaultConnection");
        }

        [TestCleanup]
        public void TearDown()
        {
            DbContextManager.CloseAllDbContexts();
        }

        [TestMethod]
        public void Test_Code_DbContext()
        {
            //customerRepository.FindByName("", "");
            //var dbcontext = DbContextManager.Current;
            IList<Type> list = GetDbContextGetGenericType(new MyDbContext("DefaultDb"));
            Assert.IsTrue(list.Count > 0);

            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void Test_Create_DbContext()
        {
            this.customerRepository.FindByName("", "");
            DbContext dbcontext = DbContextManager.Current;
            IList<Type> list = GetDbContextGetGenericType(dbcontext);

            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void Test_DbContext2ObjectContext()
        {
            DbContext dbcontext = DbContextManager.Current;
            IEnumerable<string> list = GetObjectTables(dbcontext);
            Assert.IsTrue(list.Count() > 0);

            Console.WriteLine(list.Count());

            foreach (string entityType in list)
            {
                Console.WriteLine(entityType);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     取得dbcontext中的 DbSet 中的类型，Added by zbw911
        ///     2012-12-22
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static IList<Type> GetDbContextGetGenericType(DbContext context)
        {
            Console.WriteLine(((IObjectContextAdapter)context).ObjectContext.DefaultContainerName);
            var listtype = new List<Type>();
            Type type = context.GetType();

            PropertyInfo[] listpropertyies = type.GetProperties();

            foreach (PropertyInfo property in listpropertyies)
            {
                Type t = property.PropertyType;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    Type[] args = t.GetGenericArguments();
                    listtype.AddRange(args);
                }
            }

            return listtype;
        }

        private static IEnumerable<string> GetObjectTables(DbContext context)
        {
            ObjectContext objcontext = ((IObjectContextAdapter)context).ObjectContext;

            //Console.WriteLine(objcontext.DefaultContainerName);

            ReadOnlyCollection<EntityType> entities = objcontext.MetadataWorkspace.GetItems<EntityType>(
                DataSpace.CSpace);

            foreach (EntityType entityType in entities)
            {
                Console.WriteLine(entityType.Name);
            }

            return entities.Select(x => x.Name);
        }

        #endregion
    }
}