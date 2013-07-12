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

        internal const string StorageKey = "HttpContextObjectContextStorageKey";

        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// 初始化 上下文存储
        /// </summary>
        /// <param name="app"></param>
        public WebDbContextStorage(/*HttpApplication app*/)
        {
            //app.EndRequest += (sender, args) =>
            //    {
            //        DbContextManager.CloseAllDbContexts();
            //        HttpContext.Current.Items.Remove(StorageKey);

            //        Dev.Log.Loger.Debug("Release Conn");

            //    };
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
            var storage = context.Items[StorageKey] as SimpleDbContextStorage;
            if (storage == null)
            {
                storage = new SimpleDbContextStorage();
                context.Items[StorageKey] = storage;
            }
            return storage;
        }

        #endregion
    }



    /// <summary>
    /// 
    /// </summary>
    public class WebDbContextStorageRleaseModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, args) =>
            {
                DbContextManager.CloseAllDbContexts();
                HttpContext.Current.Items.Remove(WebDbContextStorage.StorageKey);

                //Dev.Log.Loger.Debug("Release Conn");

            };
        }

        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }
    }
}