namespace Dev.Data.Test.Domain.Mapping
{
    using Infrastructure.Tests.Data.Domain.Mapping;

    public class CustomerMapping : EntityMappingBase<Customer>
    {
        #region Constructors and Destructors

        public CustomerMapping()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Firstname).IsRequired().HasMaxLength(25);
            this.Property(x => x.Lastname).IsRequired().HasMaxLength(25);
            this.Property(x => x.Inserted);

            this.HasMany(x => x.Orders).WithRequired(y => y.Customer);

            this.ToTable("Customer");
        }

        #endregion
    }
}