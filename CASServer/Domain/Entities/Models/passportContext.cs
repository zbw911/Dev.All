using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Domain.Entities.Models.Mapping;

namespace Domain.Entities.Models
{
    public partial class passportContext : DbContext
    {
        static passportContext()
        {
            Database.SetInitializer<passportContext>(null);
        }

       public passportContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<UserExtend> UserExtends { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }
        public DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new UserExtendMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
            modelBuilder.Configurations.Add(new webpages_MembershipMap());
            modelBuilder.Configurations.Add(new webpages_OAuthMembershipMap());
            modelBuilder.Configurations.Add(new webpages_RolesMap());
            modelBuilder.Configurations.Add(new webpages_UsersInRolesMap());
        }
    }
}
