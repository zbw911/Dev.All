namespace Dev.Data.Test.Domain.Mapping
{
    using Infrastructure.Tests.Data.Domain.Mapping;

    public class OrderMapping : EntityMappingBase<Order>
    {
        #region Constructors and Destructors

        public OrderMapping()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.OrderDate);
            this.Property(x => x.CustomerId);

            this.HasRequired(x => x.Customer).WithMany(y => y.Orders).HasForeignKey(o => o.CustomerId);

            this.ToTable("Order");
        }

        #endregion
    }
}