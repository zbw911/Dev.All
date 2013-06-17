using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Data.Test.Reponsitory
{
    using System.Data.Entity;

    using Dev.Demo.Entities2.Models;
    using Dev.Data.Infras;

    interface IAdminRepository : IRepository, IRepository<Admin>
    {
        bool AddAdmin(int Typeid, string Uname, string Tname, string email);
    }

    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public AdminRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AdminRepository(DbContext context)
            : base(context)
        {
        }



        public bool AddAdmin(int Typeid, string Uname, string Tname, string email)
        {
            Add(new Admin { Typeid = Typeid, Uname = Uname, Tname = Tname, Email = email });
            this.UnitOfWork.SaveChanges();
            return true;
        }
    }
}
