namespace Dev.Data.Test.Domain.Mapping
{
    using Infrastructure.Tests.Data.Domain.Mapping;

    public class OrderLineMapping : EntityMappingBase<OrderLine>
    {
        #region Constructors and Destructors

        public OrderLineMapping()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Price);
            this.Property(x => x.Quantity);
            this.Property(x => x.ProductId);
            this.Property(x => x.OrderId);

            this.HasRequired(x => x.Order).WithMany(o => o.OrderLines).HasForeignKey(ol => ol.OrderId);

            this.HasRequired(x => x.Product).WithMany(p => p.OrderLines).HasForeignKey(ol => ol.ProductId);

            this.ToTable("OrderLine");
        }

        #endregion
    }
}