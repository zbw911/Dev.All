using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Dev.Data.ContextStorage
{
    /// <summary>
    ///    支持多线程模型
    /// </summary>
    public class ThreadDbContextStorage : IDbContextStorage
    {
        [ThreadStatic]
        private static SimpleDbContextStorage storage;

        #region Constructors and Destructors
        /// <summary>
        /// 初始化 上下文存储
        /// </summary>
        /// <param name="app"></param>
        public ThreadDbContextStorage(/*HttpApplication app*/)
        {
            //app.EndRequest += (sender, args) =>
            //    {
            //        DbContextManager.CloseAllDbContexts();
            //        HttpContext.Current.Items.Remove(StorageKey);

            //        Dev.Log.Loger.Debug("Release Conn");

            //    };
        }



        ~ThreadDbContextStorage()
        {
            DbContextManager.CloseAllDbContexts();
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


            if (storage == null)
            {
                storage = new SimpleDbContextStorage();

            }
            return storage;
        }

        #endregion
    }
}
