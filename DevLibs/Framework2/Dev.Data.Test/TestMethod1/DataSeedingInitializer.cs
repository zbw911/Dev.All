namespace Infrastructure.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Dev.Data.Test;
    using Dev.Data.Test.Domain;

    /// <summary>
    ///     Seeding data
    /// </summary>
    public class DataSeedingInitializer : DropCreateDatabaseAlways<MyDbContext>
    {
        #region Methods

        protected override void Seed(MyDbContext context)
        {
            this.CreateCustomer(context);
            this.CreateProducts(context);
            this.AddOrders(context);
        }

        private void AddOrders(MyDbContext context)
        {
            Customer c = context.Customers.Where(x => x.Firstname == "John" && x.Lastname == "Doe").SingleOrDefault();

            Product winXP = context.Products.Where(x => x.Name == "Windows XP Professional").SingleOrDefault();
            Product winSeven = context.Products.Where(x => x.Name == "Windows Seven Professional").SingleOrDefault();

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

            context.Orders.Add(o);
            context.SaveChanges();
            Console.WriteLine("Saved one order");
        }

        private void CreateCustomer(MyDbContext context)
        {
            var c = new Customer { Firstname = "John", Lastname = "Doe", Inserted = DateTime.Now };
            context.Customers.Add(c);
            context.SaveChanges();
        }

        private void CreateProducts(MyDbContext context)
        {
            var osCategory = new Category { Name = "Operating System" };
            var msProductCategory = new Category { Name = "MS Product" };

            context.Categories.Add(osCategory);
            context.Categories.Add(msProductCategory);

            var p1 = new Product { Name = "Windows Seven Professional", Price = 100 };
            p1.Categories.Add(osCategory);
            p1.Categories.Add(msProductCategory);
            context.Products.Add(p1);

            var p2 = new Product { Name = "Windows XP Professional", Price = 20 };
            p2.Categories.Add(osCategory);
            p2.Categories.Add(msProductCategory);
            context.Products.Add(p2);

            var p3 = new Product { Name = "Windows Seven Home", Price = 80 };
            p3.Categories.Add(osCategory);
            p3.Categories.Add(msProductCategory);
            context.Products.Add(p3);

            var p4 = new Product { Name = "Windows Seven Ultimate", Price = 110 };
            p4.Categories.Add(osCategory);
            p4.Categories.Add(msProductCategory);
            context.Products.Add(p4);

            var p5 = new Product { Name = "Windows Seven Premium", Price = 150 };
            p5.Categories.Add(osCategory);
            p5.Categories.Add(msProductCategory);
            context.Products.Add(p5);

            context.SaveChanges();
            Console.WriteLine("Saved five Products in 2 Category");
        }

        #endregion
    }
}