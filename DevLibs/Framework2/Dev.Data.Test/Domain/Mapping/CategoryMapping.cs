namespace Dev.Data.Test.Domain.Mapping
{
    using Infrastructure.Tests.Data.Domain.Mapping;

    public class CategoryMapping : EntityMappingBase<Category>
    {
        #region Constructors and Destructors

        public CategoryMapping()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).HasColumnName("Category Name");

            //HasMany(x => x.Products)
            //   .WithMany(y => y.Categories)
            //   .Map(m =>
            //   {
            //       m.ToTable("ProductsInCategories");
            //   });

            //HasMany(x => x.Products)
            //   .WithMany(y => y.Categories)               
            //   .Map(m =>
            //   {
            //       m.ToTable("ProductsInCategories");
            //       m.MapLeftKey("CategoryId"); // optional, to specify/override the column named ProductId (instead of auto-generated Product_Id) for a many-to-many relationship
            //       m.MapRightKey("ProductId"); // optional, to explicitly specify the column named "CategoryId" instead of auto-generated Category_Id for a many-to-many relationship
            //   });

            this.ToTable("Category");
        }

        #endregion
    }
}