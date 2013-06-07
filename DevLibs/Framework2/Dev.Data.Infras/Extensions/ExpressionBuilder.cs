// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月05日 16:01
// 
// 修改于：2013年02月05日 17:31
// 文件名：ExpressionBuilder.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Infras.Extensions
{
    using System;
    using System.Linq.Expressions;

    /*
     * 
     *  旧的方法 在 LINQ to Entities 不支持 LINQ 表达式节点类型“Invoke”。   出错， 改出新方法 实现 
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
     * */

    /// <summary>
    ///     用于创建起始的条件，简化一些操作了
    /// </summary>
    public static class PredicateBuilder
    {
        #region Public Methods and Operators

        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        #endregion
    }


    public static class OrderBy<Tout>
    {
        public static Expression<Func<T, Tout>> For<T>()
        {
            return null;
        }
    }

    public class ss
    {
        public string Str { get; set; }

        public int Id { get; set; }
    }

    public class test
    {
        void T()
        {
            var z = OrderBy<int>.For<ss>();
            z = x => x.Id;
        }
    }
}