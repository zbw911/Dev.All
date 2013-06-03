using System.Data.Entity;
using Dev.Data;
using Domain.MainBoundedContext.UserProfile.Repository;

namespace Domain.MainBoundedContext.UserExtend.Repository
{
    public class UserExtendRepository : GenericRepository<Entities.Models.UserExtend>, IUserExtendRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public UserExtendRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserExtendRepository(DbContext context)
            : base(context)
        {
        }



    }
}