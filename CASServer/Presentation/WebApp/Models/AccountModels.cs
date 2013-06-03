using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CASServer.Models
{
    public class UsersContext : DbContext
    {

        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserExtend> UserExtends { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserExtend>().Property(p => p.Uid).HasPrecision(18, 0).IsRequired();

            base.OnModelCreating(modelBuilder);
        }


    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string NickName { get; set; }

        

        public int? Sex { get; set; }

        public int? Province { get; set; }

        public int? City { get; set; }

    }


    [Table("UserExtend")]
    public class UserExtend
    {
        [Key]
        public int UserId { get; set; }
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal Uid { get; set; }
    }


}
