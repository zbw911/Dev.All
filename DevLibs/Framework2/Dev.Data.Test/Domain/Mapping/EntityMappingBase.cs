namespace Infrastructure.Tests.Data.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Dev.Data.Test.Domain;

    public abstract class EntityMappingBase<T> : EntityTypeConfiguration<T>
        where T : Entity
    {
        #region Constructors and Destructors

        public EntityMappingBase()
        {
            this.HasKey(x => x.Id);
        }

        #endregion
    }
}