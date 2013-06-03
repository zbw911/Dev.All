using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class UserExtendMap : EntityTypeConfiguration<UserExtend>
    {
        public UserExtendMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Uid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("UserExtend");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Uid).HasColumnName("Uid");
        }
    }
}
