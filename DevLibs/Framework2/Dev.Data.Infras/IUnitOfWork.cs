// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月04日 12:25
// 
// 修改于：2013年02月05日 17:31
// 文件名：IUnitOfWork.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************


using System.Data.Entity.Core.Objects;

namespace Dev.Data.Infras
{
    using System;
    using System.Data;

    /// <summary>
    ///     The UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether is in transaction.
        /// </summary>
        bool IsInTransaction { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The begin transaction.
        /// </summary>
        IUnitOfWork BeginTransaction();

        /// <summary>
        ///     The begin transaction.
        /// </summary>
        /// <param name="isolationLevel">
        ///     The isolation level.
        /// </param>
        IUnitOfWork BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        ///     The commit transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        ///     The roll back transaction.
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        ///     The save changes.
        /// </summary>
        int SaveChanges();

        /// <summary>
        ///     The save changes.
        /// </summary>
        /// <param name="saveOptions">
        ///     The save options.
        /// </param>
        int SaveChanges(SaveOptions saveOptions);

        #endregion
    }
}