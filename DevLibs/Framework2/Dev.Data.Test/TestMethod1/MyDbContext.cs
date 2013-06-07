namespace Dev.Data.Test
{
    using System.Data.Entity;

    using Dev.Data.Test.Domain;
    using Dev.Data.Test.Domain.Mapping;

    public class MyDbContext : DbContext
    {
        #region Constructors and Destructors

        public MyDbContext()
        {
        }

        public MyDbContext(string connStringName)
            : base(connStringName)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #endregion

        #region Public Properties

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CustomerMapping());
            modelBuilder.Configurations.Add(new ProductMapping());
            modelBuilder.Configurations.Add(new CategoryMapping());
            modelBuilder.Configurations.Add(new OrderMapping());
            modelBuilder.Configurations.Add(new OrderLineMapping());
        }

        #endregion
    }
}