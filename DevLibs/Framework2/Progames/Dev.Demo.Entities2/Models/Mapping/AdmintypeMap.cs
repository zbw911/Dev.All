using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Dev.Demo.Entities2.Models.Mapping
{
    public class AdmintypeMap : EntityTypeConfiguration<Admintype>
    {
        public AdmintypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Typeid);

            // Properties
            this.Property(t => t.Typename)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Admintype");
            this.Property(t => t.Typeid).HasColumnName("Typeid");
            this.Property(t => t.Typename).HasColumnName("Typename");
            this.Property(t => t.System).HasColumnName("System");
            this.Property(t => t.Purviews).HasColumnName("Purviews");
        }
    }
}
