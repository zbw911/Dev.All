namespace Infrastructure.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using Dev.Data;
    using Dev.Data.Infras;
    using Dev.Data.Infras.Specification;
    using Dev.Data.Test;
    using Dev.Data.Test.Domain;
    using Dev.Data.Test.Specification;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UseMyDbContextTest
    {
        #region Fields

        private MyDbContext context;

        private ICustomerRepository customerRepository;

        private IRepository repository;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void GenerateDatabaseScriptTest()
        {
            string script = ((IObjectContextAdapter)this.context).ObjectContext.CreateDatabaseScript();
            // for debugging
            Console.WriteLine(script);
            Assert.IsTrue(!string.IsNullOrEmpty(script));
        }

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            Database.SetInitializer(new DataSeedingInitializer());
            this.context = new MyDbContext("DefaultDb");

            this.customerRepository = new CustomerRepository(this.context);
            this.repository = new GenericRepository(this.context);
        }

        [TestCleanup]
        public void TearDown()
        {
            if ((this.context != null)
                && (((IObjectContextAdapter)this.context).ObjectContext.Connection.State == ConnectionState.Open))
            {
                ((IObjectContextAdapter)this.context).ObjectContext.Connection.Close();
                this.context = null;
            }
        }

        [TestMethod]
        public void Test()
        {
            DoAction(() => this.FindOneCustomer());
            DoAction(() => this.FindCategoryWithInclude());
            DoAction(() => this.FindManyOrdersForJohnDoe());
            DoAction(() => this.FindNewlySubscribed());
            DoAction(() => this.FindBySpecification());
            DoAction(() => this.FindByCompositeSpecification());
            DoAction(() => this.FindByConcretSpecification());
            DoAction(() => this.FindByConcretCompositeSpecification());
            DoAction(() => this.UpdateProduct());
        }

        #endregion

        #region Methods

        private static void DoAction(Expression<Action> action)
        {
            Console.Write("Executing {0} ... ", action.Body);

            Action act = action.Compile();
            act.Invoke();

            Console.WriteLine();
        }

        private void FindByCompositeSpecification()
        {
            IEnumerable<Product> products =
                this.repository.Find(
                    new Specification<Product>(p => p.Price < 100).And(
                        new Specification<Product>(p => p.Name == "Windows XP Professional")));
            Assert.AreEqual(1, products.Count());
        }

        private void FindByConcretCompositeSpecification()
        {
            IEnumerable<Product> products =
                this.repository.Find(
                    new AndSpecification<Product>(
                        new ProductOnSaleSpecification(), new ProductByNameSpecification("Windows XP Professional")));
            Assert.AreEqual(1, products.Count());
        }

        private void FindByConcretSpecification()
        {
            var specification = new ProductOnSaleSpecification();
            IEnumerable<Product> productsOnSale = this.repository.Find(specification);
            Assert.AreEqual(2, productsOnSale.Count());
        }

        private void FindBySpecification()
        {
            var specification = new Specification<Product>(p => p.Price < 100);
            IEnumerable<Product> productsOnSale = this.repository.Find(specification);
            Assert.AreEqual(2, productsOnSale.Count());
        }

        private void FindCategoryWithInclude()
        {
            Category category =
                this.repository.GetQuery<Category>(x => x.Name == "Operating System")
                    .Include(c => c.Products)
                    .SingleOrDefault();
            Assert.IsNotNull(category);
            Assert.IsTrue(category.Products.Count > 0);
        }

        private void FindManyOrdersForJohnDoe()
        {
            Customer customer = this.customerRepository.FindByName("John", "Doe");
            IEnumerable<Order> orders = this.repository.Find<Order>(x => x.Customer.Id == customer.Id);

            Console.Write("Found {0} Orders with {1} OrderLines", orders.Count(), orders.ToList()[0].OrderLines.Count);
        }

        private void FindNewlySubscribed()
        {
            IList<Customer> newCustomers = this.customerRepository.NewlySubscribed();

            Console.Write("Found {0} new customers", newCustomers.Count);
        }

        private void FindOneCustomer()
        {
            var c = this.repository.FindOne<Customer>(x => x.Firstname == "John" && x.Lastname == "Doe");

            Console.Write("Found Customer: {0} {1}", c.Firstname, c.Lastname);
        }

        private void GetProductsWithPaging()
        {
            List<Product> output = this.repository.Get<Product, string>(x => x.Name, 0, 5).ToList();
            Assert.IsTrue(output[0].Name == "Windows Seven Home");
            Assert.IsTrue(output[1].Name == "Windows Seven Premium");
            Assert.IsTrue(output[2].Name == "Windows Seven Professional");
            Assert.IsTrue(output[3].Name == "Windows Seven Ultimate");
            Assert.IsTrue(output[4].Name == "Windows XP Professional");
        }

        private void UpdateProduct()
        {
            this.repository.UnitOfWork.BeginTransaction();

            var output = this.repository.FindOne<Product>(x => x.Name == "Windows XP Professional");
            Assert.IsNotNull(output);

            output.Name = "Windows XP Home";
            this.repository.Update(output);
            this.repository.UnitOfWork.CommitTransaction();

            var updated = this.repository.FindOne<Product>(x => x.Name == "Windows XP Home");
            Assert.IsNotNull(updated);
        }

        #endregion
    }
}