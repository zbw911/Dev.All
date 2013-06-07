// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:41
// 
// 修改于：2013年01月30日 18:06
// 文件名：WebDbContextStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.ContextStorage
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web;

    /// <summary>
    ///     The web db context storage.
    /// </summary>
    public class WebDbContextStorage : IDbContextStorage
    {
        #region Constants

        private const string STORAGE_KEY = "HttpContextObjectContextStorageKey";

        #endregion

        #region Constructors and Destructors

        public WebDbContextStorage(HttpApplication app)
        {
            app.EndRequest += (sender, args) =>
                {
                    DbContextManager.CloseAllDbContexts();
                    HttpContext.Current.Items.Remove(STORAGE_KEY);
                };
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<DbContext> GetAllDbContexts()
        {
            SimpleDbContextStorage storage = this.GetSimpleDbContextStorage();
            return storage.GetAllDbContexts();
        }

        public DbContext GetDbContextForKey(string key)
        {
            SimpleDbContextStorage storage = this.GetSimpleDbContextStorage();
            return storage.GetDbContextForKey(key);
        }

        public void SetDbContextForKey(string factoryKey, DbContext context)
        {
            SimpleDbContextStorage storage = this.GetSimpleDbContextStorage();
            storage.SetDbContextForKey(factoryKey, context);
        }

        #endregion

        #region Methods

        private SimpleDbContextStorage GetSimpleDbContextStorage()
        {
            HttpContext context = HttpContext.Current;
            var storage = context.Items[STORAGE_KEY] as SimpleDbContextStorage;
            if (storage == null)
            {
                storage = new SimpleDbContextStorage();
                context.Items[STORAGE_KEY] = storage;
            }
            return storage;
        }

        #endregion
    }
}