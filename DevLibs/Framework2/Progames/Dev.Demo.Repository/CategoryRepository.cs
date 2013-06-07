namespace Dev.Demo.Repository
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Dev.Data;
    using Dev.Demo.Entities.NewsAgg;

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        //public CategoryRepository()
        //    : this(DependencyResolver.Current.GetService<MyDbContext>())
        //{
        //}

        //public CategoryRepository(MyDbContext context)
        //    : base(context)
        //{
        //}

        #region Public Methods and Operators

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;" /> class.
        /// </summary>
        //public GenericRepository()
        //    : this(string.Empty)
        //{
        //}

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public CategoryRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        ///// <summary>
        /////     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        ///// </summary>
        ///// <param name="context">The context.</param>
        //public CategoryRepository(DbContext context)
        //    : base(context)
        //{
        //}



        public IEnumerable<Category> GetCategories()
        {
            return this.GetQuery().ToList();
        }

        public Category GetCategoryById(int id)
        {
            return this.FindOne(x => x.Id == id);
        }

        #endregion
    }
}