// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:16
// 
// 修改于：2013年02月05日 17:31
// 文件名：ISpecification.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Infras.Specification
{
    using System.Linq;

    /// <summary>
    ///     In simple terms, a specification is a small piece of logic which is independent and give an answer
    ///     to the question “does this match ?”. With Specification, we isolate the logic that do the selection
    ///     into a reusable business component that can be passed around easily from the entity we are selecting.
    /// </summary>
    /// <see cref="http://en.wikipedia.org/wiki/Specification_pattern" />
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<TEntity>
    {
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
        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);

        /// <summary>
        ///     The satisfying entity from.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);

        #endregion
    }
}