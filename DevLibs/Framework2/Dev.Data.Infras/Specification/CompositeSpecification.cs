// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:16
// 
// 修改于：2013年02月05日 17:31
// 文件名：CompositeSpecification.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Infras.Specification
{
    using System.Linq;

    /// <summary>
    ///     http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CompositeSpecification<TEntity> : ISpecification<TEntity>
    {
        #region Fields

        /// <summary>
        ///     The _left side.
        /// </summary>
        protected readonly Specification<TEntity> _leftSide;

        /// <summary>
        ///     The _right side.
        /// </summary>
        protected readonly Specification<TEntity> _rightSide;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="leftSide">
        ///     The left side.
        /// </param>
        /// <param name="rightSide">
        ///     The right side.
        /// </param>
        public CompositeSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
        {
            this._leftSide = leftSide;
            this._rightSide = rightSide;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The satisfying entities from.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        public abstract IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);

        /// <summary>
        ///     The satisfying entity from.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        public abstract TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);

        #endregion
    }
}