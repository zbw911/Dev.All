using System.Data.Entity;
using Dev.Data;

namespace Domain.MainBoundedContext.webpages_Membership.Repository
{
    public class WebpagesMembershipRepository : GenericRepository<Entities.Models.webpages_Membership>, IWebpagesMembershipRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public WebpagesMembershipRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public WebpagesMembershipRepository(DbContext context)
            : base(context)
        {
        }



    }
}