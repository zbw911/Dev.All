// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:41
// 
// 修改于：2013年01月30日 18:06
// 文件名：SimpleDbContextStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.ContextStorage
{
    using System.Collections.Generic;
    using System.Data.Entity;

    /// <summary>
    /// 使用 Dictionary 存储上下文
    /// </summary>
    public class SimpleDbContextStorage : IDbContextStorage
    {
        #region Fields

        private readonly Dictionary<string, DbContext> _storage = new Dictionary<string, DbContext>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns all the values of the internal dictionary of db contexts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DbContext> GetAllDbContexts()
        {
            return this._storage.Values;
        }

        /// <summary>
        ///     Returns the db context associated with the specified key or
        ///     null if the specified key is not found.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public DbContext GetDbContextForKey(string key)
        {
            DbContext context;
            if (!this._storage.TryGetValue(key, out context))
            {
                return null;
            }
            return context;
        }

        /// <summary>
        ///     Stores the db context into a dictionary using the specified key.
        ///     If an object context already exists by the specified key,
        ///     it gets overwritten by the new object context passed in.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="context">The object context.</param>
        public void SetDbContextForKey(string key, DbContext context)
        {
            this._storage.Add(key, context);
        }

        #endregion
    }
}