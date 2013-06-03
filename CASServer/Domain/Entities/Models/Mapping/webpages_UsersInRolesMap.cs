using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Entities.Models.Mapping
{
    public class webpages_UsersInRolesMap : EntityTypeConfiguration<webpages_UsersInRoles>
    {
        public webpages_UsersInRolesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.RoleId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("webpages_UsersInRoles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");

            // Relationships
            this.HasRequired(t => t.UserExtend)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.UserProfile)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.webpages_Roles)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
