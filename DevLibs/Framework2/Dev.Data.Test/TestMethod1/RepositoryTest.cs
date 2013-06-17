namespace Infrastructure.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using Dev.Data;
    using Dev.Data.ContextStorage;
    using Dev.Data.Infras;
    using Dev.Data.Infras.Specification;
    using Dev.Data.Test.Domain;
    using Dev.Data.Test.Specification;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     This class is used to test the _lab_ version of repository implementation
    /// </summary>
    [TestClass]
    public class RepositoryTest
    {
        #region Fields

        private ICustomerRepository customerRepository;

        private IRepository repository;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

            DbContextManager.InitStorage(new SimpleDbContextStorage());
            DbContextManager.Init("DefaultConnection", new[] { "Dev.Data.Test" }, true);

            this.customerRepository = new CustomerRepository("DefaultConnection");
            this.repository = new GenericRepository("DefaultConnection");
        }

        [TestCleanup]
        public void TearDown()
        {
            DbContextManager.CloseAllDbContexts();
        }

        [TestMethod]
        public void Test()
        {
            DoAction(() => this.CreateCustomer());
            DoAction(() => this.CreateProducts());
            DoAction(() => this.AddOrders());
            DoAction(() => this.FindOneCustomer());
            DoAction(() => this.FindByKey());
            DoAction(() => this.FindManyOrdersForJohnDoe());
            DoAction(() => this.FindNewlySubscribed());
            DoAction(() => this.FindOrderWithInclude());
            DoAction(() => this.FindBySpecification());
            DoAction(() => this.FindByCompositeSpecification());
            DoAction(() => this.FindByConcretSpecification());
            DoAction(() => this.FindByConcretCompositeSpecification());
            DoAction(() => this.FindByChainOfSpecifications());
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

        private void AddOrders()
        {
            Customer c = this.customerRepository.FindByName("John", "Doe");

            var winXP = this.repository.FindOne<Product>(x => x.Name == "Windows XP Professional");
            var winSeven = this.repository.FindOne<Product>(x => x.Name == "Windows Seven Professional");

            var o = new Order
                        {
                            OrderDate = DateTime.Now,
                            Customer = c,
                            OrderLines =
                                new List<OrderLine>
                                    {
                                        new OrderLine { Price = 200, Product = winXP, Quantity = 1 },
                                        new OrderLine
                                            {
                                                Price = 699.99,
                                                Product = winSeven,
                                                Quantity = 5
                                            }
                                    }
                        };

            this.repository.Add(o);
            this.repository.UnitOfWork.SaveChanges();
            Console.Write("Saved one order");
        }

        private void CreateCustomer()
        {
            var c = new Customer { Firstname = "John", Lastname = "Doe", Inserted = DateTime.Now };
            this.customerRepository.Add(c);

            this.customerRepository.UnitOfWork.SaveChanges();
        }

        private void CreateProducts()
        {
            var osCategory = new Category { Name = "Operating System" };
            var msProductCategory = new Category { Name = "MS Product" };

            this.repository.Add(osCategory);
            this.repository.Add(msProductCategory);

            var p1 = new Product { Name = "Windows Seven Professional", Price = 100 };
            p1.Categories.Add(osCategory);
            p1.Categories.Add(msProductCategory);
            this.repository.Add(p1);

            var p2 = new Product { Name = "Windows XP Professional", Price = 20 };
            p2.Categories.Add(osCategory);
            p2.Categories.Add(msProductCategory);
            this.repository.Add(p2);

            var p3 = new Product { Name = "Windows Seven Home", Price = 80 };
            p3.Categories.Add(osCategory);
            p3.Categories.Add(msProductCategory);
            this.repository.Add(p3);

            var p4 = new Product { Name = "Windows Seven Ultimate", Price = 110 };
            p4.Categories.Add(osCategory);
            p4.Categories.Add(msProductCategory);
            this.repository.Add(p4);

            var p5 = new Product { Name = "Windows Seven Premium", Price = 150 };
            p5.Categories.Add(osCategory);
            p5.Categories.Add(msProductCategory);
            this.repository.Add(p5);

            this.repository.UnitOfWork.SaveChanges();

            Console.Write("Saved five Products in 2 Category");
        }

        private void FindByChainOfSpecifications()
        {
            IEnumerable<Product> products =
                this.repository.Find(
                    new ProductOnSaleSpecification().And(new ProductByNameSpecification("Windows XP Professional")));
            Assert.AreEqual(1, products.Count());
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

        private void FindByKey()
        {
            Customer c = this.customerRepository.FindByName("John", "Doe");

            var customer = this.customerRepository.GetByKey<Customer>(c.Id);

            Console.Write("Found Customer by its PK: {0}", customer != null);
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
            Customer c = this.customerRepository.FindByName("John", "Doe");
            IEnumerable<Order> orders = this.repository.Find<Order>(x => x.Customer.Id == c.Id);

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

        private void FindOrderWithInclude()
        {
            Customer c = this.customerRepository.FindByName("John", "Doe");
            List<Order> orders = this.repository.Find<Order>(x => x.Customer.Id == c.Id).ToList();
            Console.Write("Found {0} Orders with {1} OrderLines", orders.Count(), orders.ToList()[0].OrderLines.Count);
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

        #endregion
    }
}