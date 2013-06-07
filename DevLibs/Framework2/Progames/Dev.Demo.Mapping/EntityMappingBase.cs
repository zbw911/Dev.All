namespace Dev.Demo.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Dev.Demo.Entities;

    public abstract class EntityMappingBase<T> : EntityTypeConfiguration<T> where T : Entity
    {
        protected EntityMappingBase()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.CreatedBy).IsRequired();
            this.Property(x => x.ModifiedDate).IsOptional();
        }
    }
}