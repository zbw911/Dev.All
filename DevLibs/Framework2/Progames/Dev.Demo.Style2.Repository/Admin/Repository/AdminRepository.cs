namespace Dev.Demo.Style2.Repository.Repository
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Dev.Data;
    using Dev.Demo.Entities2.Models;
    using Dev.Demo.Style2.Repository.Specification;

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

        public List<Admin> GetSystemAdmin()
        {
            return this.Find(new SystemAdminSpecification()).ToList();

        }

        public List<Admin> GetSystemActiveAdmin()
        {

            

            return this.Find(new SystemAdminSpecification()
                .And(new ActiveAdminSpecification(System.DateTime.Now.AddDays(-1)))).ToList();

        }
    }
}