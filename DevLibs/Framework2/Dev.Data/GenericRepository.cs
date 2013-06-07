// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:27
// 
// 修改于：2013年02月05日 17:28
// 文件名：GenericRepository.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Data.Objects;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using Dev.Data.Infras;
    using Dev.Data.Infras.Specification;

    #endregion

    /// <summary>
    ///     Generic repository
    /// </summary>
    public class GenericRepository : IRepository
    {
        #region Fields

        private readonly string _connectionStringName;

        private readonly PluralizationService _pluralizer =
            PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        private DbContext _context;

        private IUnitOfWork unitOfWork;

        #endregion

        ///// <summary>
        /////    不再使用的方法
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
        {
            this._connectionStringName = connectionStringName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GenericRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = context;
        }

        public GenericRepository(ObjectContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = new DbContext(context, true);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 当前的UOW
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (this.unitOfWork == null)
                {
                    this.unitOfWork = new UnitOfWork(this.DbContext);
                }
                return this.unitOfWork;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// 这个方法用于维护 DB Context的自动选择的一个方法，
        ///
        /// </summary>
        private DbContext DbContext
        {
            get
            {
                if (this._context == null)
                {
                    if (this._connectionStringName == string.Empty)
                    {
                        this._context = DbContextManager.Current;
                    }
                    else
                    {
                        this._context = DbContextManager.CurrentFor(this._connectionStringName);
                    }
                }
                return this._context;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.DbContext.Set<TEntity>().Attach(entity);
        }

        public int Count<TEntity>() where TEntity : class
        {
            return this.GetQuery<TEntity>().Count();
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return this.GetQuery<TEntity>().Count(criteria);
        }

        public int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(this.GetQuery<TEntity>()).Count();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            IEnumerable<TEntity> records = Find(criteria);

            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }

        public void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            IEnumerable<TEntity> records = Find(criteria);
            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return this.GetQuery<TEntity>().Where(criteria);
        }

        public IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(this.GetQuery<TEntity>()).AsEnumerable();
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return this.GetQuery<TEntity>().Where(criteria).FirstOrDefault();
        }

        public TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(this.GetQuery<TEntity>());
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return this.GetQuery<TEntity>().First(predicate);
        }

        public TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(this.GetQuery<TEntity>()).First();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(
            Expression<Func<TEntity, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return
                    this.GetQuery<TEntity>()
                        .OrderBy(orderBy)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .AsEnumerable();
            }
            return
                this.GetQuery<TEntity>()
                    .OrderByDescending(orderBy)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(
            Expression<Func<TEntity, bool>> criteria,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return
                    GetQuery(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return
                GetQuery(criteria)
                    .OrderByDescending(orderBy)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(
            ISpecification<TEntity> specification,
            Expression<Func<TEntity, TOrderBy>> orderBy,
            int pageIndex,
            int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return
                    specification.SatisfyingEntitiesFrom(this.GetQuery<TEntity>())
                                 .OrderBy(orderBy)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .AsEnumerable();
            }
            return
                specification.SatisfyingEntitiesFrom(this.GetQuery<TEntity>())
                             .OrderByDescending(orderBy)
                             .Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .AsEnumerable();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return this.GetQuery<TEntity>().AsEnumerable();
        }

        public TEntity GetByKey<TEntity>(object keyValue) where TEntity : class
        {
            EntityKey key = this.GetEntityKey<TEntity>(keyValue);

            object originalItem;
            if (((IObjectContextAdapter)this.DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (TEntity)originalItem;
            }
            return default(TEntity);
        }

        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            /* 
             * From CTP4, I could always safely call this to return an IQueryable on DbContext 
             * then performed any with it without any problem:
             */
            // return DbContext.Set<TEntity>();

            /*
             * but with 4.1 release, when I call GetQuery<TEntity>().AsEnumerable(), there is an exception:
             * ... System.ObjectDisposedException : The ObjectContext instance has been disposed and can no longer be used for operations that require a connection.
             */

            // here is a work around: 
            // - cast DbContext to IObjectContextAdapter then get ObjectContext from it
            // - call CreateQuery<TEntity>(entityName) method on the ObjectContext
            // - perform querying on the returning IQueryable, and it works!

            string entityName = this.GetEntityName<TEntity>();
            return ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateQuery<TEntity>(entityName);

            //下面的方法也应该是可以的
            return this.DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            
            return this.GetQuery<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(this.GetQuery<TEntity>());
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class
        {
            this.Add(entity);
            this.DbContext.SaveChanges();
            return entity;
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return this.GetQuery<TEntity>().Single<TEntity>(criteria);
        }

        public TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(this.GetQuery<TEntity>());
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            string fqen = this.GetEntityName<TEntity>();

            object originalItem;
            EntityKey key = ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateEntityKey(fqen, entity);
            if (((IObjectContextAdapter)this.DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                ((IObjectContextAdapter)this.DbContext).ObjectContext.ApplyCurrentValues(key.EntitySetName, entity);
            }

            this.DbContext.Set<TEntity>().AddOrUpdate(entity);
        }

        #endregion

        #region Methods

        private EntityKey GetEntityKey<TEntity>(object keyValue) where TEntity : class
        {
            string entitySetName = this.GetEntityName<TEntity>();
            ObjectSet<TEntity> objectSet =
                ((IObjectContextAdapter)this.DbContext).ObjectContext.CreateObjectSet<TEntity>();
            string keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var entityKey = new EntityKey(entitySetName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });
            return entityKey;
        }

        private string GetEntityName<TEntity>() where TEntity : class
        {
            return string.Format(
                "{0}.{1}",
                ((IObjectContextAdapter)this.DbContext).ObjectContext.DefaultContainerName,
                this._pluralizer.Pluralize(typeof(TEntity).Name));
        }

        #endregion
    }
}