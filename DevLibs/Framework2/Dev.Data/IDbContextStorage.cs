// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:27
// 
// 修改于：2013年02月05日 17:28
// 文件名：IDbContextStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data
{
    #region

    using System.Collections.Generic;
    using System.Data.Entity;

    #endregion

    /// <summary>
    ///     Stores object context
    /// </summary>
    public interface IDbContextStorage
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets all db contexts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DbContext> GetAllDbContexts();

        /// <summary>
        ///     Gets the db context for key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        DbContext GetDbContextForKey(string key);

        /// <summary>
        ///     Sets the db context for key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="objectContext">The object context.</param>
        void SetDbContextForKey(string key, DbContext objectContext);

        #endregion
    }
}