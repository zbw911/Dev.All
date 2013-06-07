// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:16
// 
// 修改于：2013年02月05日 17:31
// 文件名：AndSpecification.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Infras.Specification
{
    using System.Linq;

    using Dev.Data.Infras.Extensions;

    /// <summary>
    ///     The and specification.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class AndSpecification<TEntity> : CompositeSpecification<TEntity>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AndSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="leftSide">
        ///     The left side.
        /// </param>
        /// <param name="rightSide">
        ///     The right side.
        /// </param>
        public AndSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
            : base(leftSide, rightSide)
        {
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
        public override IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(this._leftSide.Predicate.And(this._rightSide.Predicate));
        }

        /// <summary>
        ///     The satisfying entity from.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        public override TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            return this.SatisfyingEntitiesFrom(query).FirstOrDefault();
        }

        #endregion
    }
}