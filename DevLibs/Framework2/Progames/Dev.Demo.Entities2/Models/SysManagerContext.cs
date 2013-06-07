using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Dev.Demo.Entities2.Models.Mapping;

namespace Dev.Demo.Entities2.Models
{
    public partial class SysManagerContext : DbContext
    {
        static SysManagerContext()
        {
            Database.SetInitializer<SysManagerContext>(null);
        }

        //public SysManagerContext()
        //    : base("Name=SysManagerContext")
        //{
        //}

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Admintype> Admintypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AdminMap());
            modelBuilder.Configurations.Add(new AdmintypeMap());
        }
    }
}
