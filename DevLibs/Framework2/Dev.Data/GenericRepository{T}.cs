// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 16:25
// 
// 修改于：2013年02月05日 17:28
// 文件名：GenericRepository{T}.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using Dev.Data.Infras;
    using Dev.Data.Infras.Specification;

    public class GenericRepository<T> : GenericRepository, IRepository<T>
        where T : class
    {
        ///// <summary>
        /////    下面的方法不再使用了，
        ///// </summary>
        //public GenericRepository()
        //    : this(string.Empty)
        //{
        //}

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public GenericRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GenericRepository(DbContext context)
            : base(context)
        {
        }


        //这个方法不再用了，没有什么实际意义
        //public GenericRepository(ObjectContext context)
        //    : base(context)
        //{
        //}

        #endregion

        #region Public Methods and Operators

        public void Add(T entity)
        {
            this.Add<T>(entity);
        }

        public void Attach(T entity)
        {
            this.Attach<T>(entity);
        }

        public int Count()
        {
            return this.Count<T>();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return Count<T>(criteria);
        }

        public int Count(ISpecification<T> criteria)
        {
            return Count<T>(criteria);
        }

        public void Delete(T entity)
        {
            Delete<T>(entity);
        }

        public void Delete(Expression<Func<T, bool>> criteria)
        {
            Delete<T>(criteria);
        }

        public void Delete(ISpecification<T> criteria)
        {
            Delete<T>(criteria);
        }

        public IEnumerable<T> Find(ISpecification<T> criteria)
        {
            return Find<T>(criteria);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> criteria)
        {
            return Find<T>(criteria);
        }

        public T FindOne(ISpecification<T> criteria)
        {
            return FindOne<T>(criteria);
        }

        public T FindOne(Expression<Func<T, bool>> criteria)
        {
            return FindOne<T>(criteria);
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return First<T>(predicate);
        }

        public T First(ISpecification<T> criteria)
        {
            return First<T>(criteria);
        }

        public IEnumerable<T> Get<TOrderBy>(
            Expression<Func<T, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            return Get<T, TOrderBy>(orderBy, pageIndex, pageSize, sortOrder);
        }

        public IEnumerable<T> Get<TOrderBy>(
            Expression<Func<T, bool>> criteria,
            Expression<Func<T, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            return Get<T, TOrderBy>(criteria, orderBy, pageIndex, pageSize, sortOrder);
        }

        public IEnumerable<T> Get<TOrderBy>(
            ISpecification<T> specification,
            Expression<Func<T, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            return Get<T, TOrderBy>(specification, orderBy, pageIndex, pageSize, sortOrder);
        }

        public IEnumerable<T> GetAll()
        {
            return this.GetAll<T>();
        }

        public T GetByKey(object keyValue)
        {
            return this.GetByKey<T>(keyValue);
        }

        public IQueryable<T> GetQuery()
        {
            return this.GetQuery<T>();
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return GetQuery<T>(predicate);
        }

        public IQueryable<T> GetQuery(ISpecification<T> criteria)
        {
            return GetQuery<T>(criteria);
        }

        public T Single(Expression<Func<T, bool>> criteria)
        {
            return Single<T>(criteria);
        }

        public T Single(ISpecification<T> criteria)
        {
            return Single<T>(criteria);
        }

        public void Update(T entity)
        {
            this.Update<T>(entity);
        }

        #endregion
    }
}