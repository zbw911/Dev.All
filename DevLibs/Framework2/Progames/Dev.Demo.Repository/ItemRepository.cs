namespace Dev.Demo.Repository
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Dev.Data;
    using Dev.Demo.Entities.NewsAgg;

    public class ItemRepository : GenericRepository, IItemRepository
    {
        // public ItemRepository()
        //    : this(DependencyResolver.Current.GetService<MyDbContext>())
        //{
        //}

        // public ItemRepository(MyDbContext context)
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
        public ItemRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ItemRepository(DbContext context)
            : base(context)
        {
        }

        

        public bool DeleteItem(Item item)
        {
            this.Delete(item);

            this.UnitOfWork.SaveChanges();

            return true;
        }

        public IEnumerable<Item> GetByCategory(int categoryId)
        {
            IEnumerable<Item> items = this.GetItems();

            return items.Where(x => x.Category.Id == categoryId);
        }

        public Item GetById(int id)
        {
            return this.GetItems().First(x => x.Id == id);
        }

        public IEnumerable<Item> GetMostViews(int numOfPage)
        {
            return this.GetItems().OrderByDescending(x => x.ItemContent.NumOfView).Take(numOfPage);
        }

        public IEnumerable<Item> GetNewestItem(int numOfPage)
        {
            return this.GetItems().OrderByDescending(x => x.CreatedDate).Take(numOfPage);
        }

        public bool IncreaseNumOfView(int id)
        {
            Item item = this.GetById(id);

            if (item != null)
            {
                item.ItemContent.NumOfView = item.ItemContent.NumOfView + 1;

                return this.SaveItem(item);
            }

            return false;
        }

        public bool SaveItem(Item item)
        {
            if (item.Id > 0)
            {
                this.Update(item);
            }
            else
            {
                this.Add(item);
            }

            this.UnitOfWork.SaveChanges();

            return true;
        }

        public IEnumerable<Item> SeachByTitle(string titleSearchText, int index, int numOfpage, out int numOfRecords)
        {
            IEnumerable<Item> items = this.GetItems();

            numOfRecords = items.Count();

            items = items.Where(x => x.ItemContent.Title.Contains(titleSearchText));

            return items.Skip((index - 1) * numOfpage).Take(numOfpage);
        }

        #endregion

        #region Methods

        private IEnumerable<Item> GetItems()
        {
            var items = this.GetQuery<Item>().Include(x => x.ItemContent).ToList();

            //TODO: will remove query here, i dont know why include ItemContent didn't work
            foreach (var item in items)
            {
                item.ItemContent = this.GetQuery<ItemContent>().First(x => x.Id == item.ItemContentId);
            }

            return items;
        }

        #endregion
    }
}