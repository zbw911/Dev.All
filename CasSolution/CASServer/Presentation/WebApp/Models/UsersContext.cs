// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/AccountModels.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity;

namespace CASServer.Models
{
    public class UsersContext : DbContext
    {
        #region C'tors

        public UsersContext()
            : base("DefaultConnection")
        {
        }

        #endregion

        #region Instance Properties

        public DbSet<UserExtend> UserExtends { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        #endregion

        #region Instance Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserExtend>().Property(p => p.Uid).HasPrecision(18, 0).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}