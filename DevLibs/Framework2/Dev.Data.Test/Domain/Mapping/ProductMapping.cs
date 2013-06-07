namespace Dev.Data.Test.Domain.Mapping
{
    using Infrastructure.Tests.Data.Domain.Mapping;

    public class ProductMapping : EntityMappingBase<Product>
    {
        #region Constructors and Destructors

        public ProductMapping()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name);
            this.Property(x => x.Description);
            this.Property(x => x.Price);

            // many to many relationship
            this.HasMany(x => x.Categories).WithMany(y => y.Products).Map(
                m =>
                    {
                        m.ToTable("ProductsInCategories");
                        m.MapLeftKey("ProductId");
                        // optional, to specify/override the column named ProductId (instead of auto-generated Product_Id) for a many-to-many relationship
                        m.MapRightKey("CategoryId");
                        // optional, to explicitly specify the column named "CategoryId" instead of auto-generated Category_Id for a many-to-many relationship
                    });

            this.ToTable("Product");
        }

        #endregion
    }
}