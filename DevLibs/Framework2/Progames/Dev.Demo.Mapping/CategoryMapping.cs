namespace Dev.Demo.Mapping
{
    using Dev.Demo.Entities.NewsAgg;

    public class CategoryMapping : EntityMappingBase<Category>
    {
        public CategoryMapping()
        {
            this.Property(x => x.Name).IsRequired();

            this.HasMany(x => x.Items).WithRequired(y => y.Category);

            this.ToTable("Category");
        }
    }
}