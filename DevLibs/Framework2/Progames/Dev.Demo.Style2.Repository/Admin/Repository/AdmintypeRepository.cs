namespace Dev.Demo.Style2.Repository.Repository
{
    using System.Data.Entity;

    using Dev.Data;
    using Dev.Demo.Entities2.Models;

    public class AdmintypeRepository : GenericRepository<Admintype>,IAdmintypeRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public AdmintypeRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AdmintypeRepository(DbContext context)
            : base(context)
        {
        }
    }
}