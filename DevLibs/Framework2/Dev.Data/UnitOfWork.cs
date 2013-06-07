// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:27
// 
// 修改于：2013年02月05日 17:28
// 文件名：UnitOfWork.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data
{
    #region

    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;

    using Dev.Data.Infras;

    #endregion

    internal class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly DbContext _dbContext;

        private bool _disposed;

        private DbTransaction _transaction;

        #endregion

        #region Constructors and Destructors

        public UnitOfWork(DbContext context)
        {
            this._dbContext = context;
        }

        #endregion

        #region Public Properties

        public bool IsInTransaction
        {
            get
            {
                return this._transaction != null;
            }
        }

        #endregion

        #region Public Methods and Operators

        public IUnitOfWork BeginTransaction()
        {
            this.BeginTransaction(IsolationLevel.ReadCommitted);
            return this;
        }

        public IUnitOfWork BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this._transaction != null)
            {
                throw new ApplicationException(
                    "Cannot begin a new transaction while an existing transaction is still running. "
                    + "Please commit or rollback the existing transaction before starting a new one.");
            }
            this.OpenConnection();
            this._transaction =
                ((IObjectContextAdapter)this._dbContext).ObjectContext.Connection.BeginTransaction(isolationLevel);
            return this;
        }

        public void CommitTransaction()
        {
            if (this._transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                ((IObjectContextAdapter)this._dbContext).ObjectContext.SaveChanges();
                this._transaction.Commit();
                this.ReleaseCurrentTransaction();
            }
            catch
            {
                this.RollBackTransaction();
                throw;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RollBackTransaction()
        {
            if (this._transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            if (this.IsInTransaction)
            {
                this._transaction.Rollback();
                this.ReleaseCurrentTransaction();
            }
        }

        public void SaveChanges()
        {
            if (this.IsInTransaction)
            {
                throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");
            }
            ((IObjectContextAdapter)this._dbContext).ObjectContext.SaveChanges();
        }

        public void SaveChanges(SaveOptions saveOptions)
        {
            if (this.IsInTransaction)
            {
                throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");
            }

            ((IObjectContextAdapter)this._dbContext).ObjectContext.SaveChanges(saveOptions);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
        }

        private void OpenConnection()
        {
            if (((IObjectContextAdapter)this._dbContext).ObjectContext.Connection.State != ConnectionState.Open)
            {
                ((IObjectContextAdapter)this._dbContext).ObjectContext.Connection.Open();
            }
        }

        /// <summary>
        ///     Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (this._transaction != null)
            {
                this._transaction.Dispose();
                this._transaction = null;
            }
        }

        #endregion
    }
}