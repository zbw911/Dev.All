// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:16
// 
// 修改于：2013年02月05日 17:31
// 文件名：ParameterRebinder .cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Infras.Extensions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
    /// </summary>
    public class ParameterRebinder : ExpressionVisitor
    {
        #region Fields

        /// <summary>
        ///     The map.
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterRebinder" /> class.
        /// </summary>
        /// <param name="map">
        ///     The map.
        /// </param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The replace parameters.
        /// </summary>
        /// <param name="map">
        ///     The map.
        /// </param>
        /// <param name="exp">
        ///     The exp.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        public static Expression ReplaceParameters(
            Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The visit parameter.
        /// </summary>
        /// <param name="p">
        ///     The p.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (this.map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }

        #endregion
    }
}