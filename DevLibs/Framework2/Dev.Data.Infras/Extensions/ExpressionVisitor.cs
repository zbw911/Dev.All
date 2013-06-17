// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:16
// 
// 修改于：2013年02月05日 17:31
// 文件名：ExpressionVisitor.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Infras.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    /// <summary>
    ///     This comes from Matt Warren's sample:
    ///     http://blogs.msdn.com/mattwar/archive/2007/07/31/linq-building-an-iqueryable-provider-part-ii.aspx.
    /// </summary>
    public abstract class ExpressionVisitor
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The visit.
        /// </summary>
        /// <param name="exp">
        ///     The exp.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public virtual Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return exp;
            }

            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return this.VisitBinary((BinaryExpression)exp);
                case ExpressionType.TypeIs:
                    return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    return this.VisitConditional((ConditionalExpression)exp);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)exp);
                case ExpressionType.Parameter:
                    return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.MemberAccess:
                    return this.VisitMemberAccess((MemberExpression)exp);
                case ExpressionType.Call:
                    return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New:
                    return this.VisitNew((NewExpression)exp);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The visit binary.
        /// </summary>
        /// <param name="b">
        ///     The b.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitBinary(BinaryExpression b)
        {
            Expression left = this.Visit(b.Left);
            Expression right = this.Visit(b.Right);
            Expression conversion = this.Visit(b.Conversion);
            if (left != b.Left || right != b.Right || conversion != b.Conversion)
            {
                if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                {
                    return Expression.Coalesce(left, right, conversion as LambdaExpression);
                }
                else
                {
                    return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
                }
            }

            return b;
        }

        /// <summary>
        ///     The visit binding.
        /// </summary>
        /// <param name="binding">
        ///     The binding.
        /// </param>
        /// <returns>
        ///     The <see cref="MemberBinding" />.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);
                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)binding);
                default:
                    throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
            }
        }

        /// <summary>
        ///     The visit binding list.
        /// </summary>
        /// <param name="original">
        ///     The original.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                MemberBinding b = this.VisitBinding(original[i]);
                if (list != null)
                {
                    list.Add(b);
                }
                else if (b != original[i])
                {
                    list = new List<MemberBinding>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(b);
                }
            }

            if (list != null)
            {
                return list;
            }

            return original;
        }

        /// <summary>
        ///     The visit conditional.
        /// </summary>
        /// <param name="c">
        ///     The c.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitConditional(ConditionalExpression c)
        {
            Expression test = this.Visit(c.Test);
            Expression ifTrue = this.Visit(c.IfTrue);
            Expression ifFalse = this.Visit(c.IfFalse);
            if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
            {
                return Expression.Condition(test, ifTrue, ifFalse);
            }

            return c;
        }

        /// <summary>
        ///     The visit constant.
        /// </summary>
        /// <param name="c">
        ///     The c.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitConstant(ConstantExpression c)
        {
            return c;
        }

        /// <summary>
        ///     The visit element initializer.
        /// </summary>
        /// <param name="initializer">
        ///     The initializer.
        /// </param>
        /// <returns>
        ///     The <see cref="ElementInit" />.
        /// </returns>
        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
            if (arguments != initializer.Arguments)
            {
                return Expression.ElementInit(initializer.AddMethod, arguments);
            }

            return initializer;
        }

        /// <summary>
        ///     The visit element initializer list.
        /// </summary>
        /// <param name="original">
        ///     The original.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                ElementInit init = this.VisitElementInitializer(original[i]);
                if (list != null)
                {
                    list.Add(init);
                }
                else if (init != original[i])
                {
                    list = new List<ElementInit>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(init);
                }
            }

            if (list != null)
            {
                return list;
            }

            return original;
        }

        /// <summary>
        ///     The visit expression list.
        /// </summary>
        /// <param name="original">
        ///     The original.
        /// </param>
        /// <returns>
        ///     The <see cref="ReadOnlyCollection" />.
        /// </returns>
        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> list = null;
            for (int i = 0, n = original.Count; i < n; i++)
            {
                Expression p = this.Visit(original[i]);
                if (list != null)
                {
                    list.Add(p);
                }
                else if (p != original[i])
                {
                    list = new List<Expression>(n);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(original[j]);
                    }

                    list.Add(p);
                }
            }

            if (list != null)
            {
                return list.AsReadOnly();
            }

            return original;
        }

        /// <summary>
        ///     The visit invocation.
        /// </summary>
        /// <param name="iv">
        ///     The iv.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(iv.Arguments);
            Expression expr = this.Visit(iv.Expression);
            if (args != iv.Arguments || expr != iv.Expression)
            {
                return Expression.Invoke(expr, args);
            }

            return iv;
        }

        /// <summary>
        ///     The visit lambda.
        /// </summary>
        /// <param name="lambda">
        ///     The lambda.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            Expression body = this.Visit(lambda.Body);
            if (body != lambda.Body)
            {
                return Expression.Lambda(lambda.Type, body, lambda.Parameters);
            }

            return lambda;
        }

        /// <summary>
        ///     The visit list init.
        /// </summary>
        /// <param name="init">
        ///     The init.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitListInit(ListInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
            if (n != init.NewExpression || initializers != init.Initializers)
            {
                return Expression.ListInit(n, initializers);
            }

            return init;
        }

        /// <summary>
        ///     The visit member access.
        /// </summary>
        /// <param name="m">
        ///     The m.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitMemberAccess(MemberExpression m)
        {
            Expression exp = this.Visit(m.Expression);
            if (exp != m.Expression)
            {
                return Expression.MakeMemberAccess(exp, m.Member);
            }

            return m;
        }

        /// <summary>
        ///     The visit member assignment.
        /// </summary>
        /// <param name="assignment">
        ///     The assignment.
        /// </param>
        /// <returns>
        ///     The <see cref="MemberAssignment" />.
        /// </returns>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Expression e = this.Visit(assignment.Expression);
            if (e != assignment.Expression)
            {
                return Expression.Bind(assignment.Member, e);
            }

            return assignment;
        }

        /// <summary>
        ///     The visit member init.
        /// </summary>
        /// <param name="init">
        ///     The init.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression);
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
            if (n != init.NewExpression || bindings != init.Bindings)
            {
                return Expression.MemberInit(n, bindings);
            }

            return init;
        }

        /// <summary>
        ///     The visit member list binding.
        /// </summary>
        /// <param name="binding">
        ///     The binding.
        /// </param>
        /// <returns>
        ///     The <see cref="MemberListBinding" />.
        /// </returns>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
            if (initializers != binding.Initializers)
            {
                return Expression.ListBind(binding.Member, initializers);
            }

            return binding;
        }

        /// <summary>
        ///     The visit member member binding.
        /// </summary>
        /// <param name="binding">
        ///     The binding.
        /// </param>
        /// <returns>
        ///     The <see cref="MemberMemberBinding" />.
        /// </returns>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
            if (bindings != binding.Bindings)
            {
                return Expression.MemberBind(binding.Member, bindings);
            }

            return binding;
        }

        /// <summary>
        ///     The visit method call.
        /// </summary>
        /// <param name="m">
        ///     The m.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitMethodCall(MethodCallExpression m)
        {
            Expression obj = this.Visit(m.Object);
            IEnumerable<Expression> args = this.VisitExpressionList(m.Arguments);
            if (obj != m.Object || args != m.Arguments)
            {
                return Expression.Call(obj, m.Method, args);
            }

            return m;
        }

        /// <summary>
        ///     The visit new.
        /// </summary>
        /// <param name="nex">
        ///     The nex.
        /// </param>
        /// <returns>
        ///     The <see cref="NewExpression" />.
        /// </returns>
        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
            if (args != nex.Arguments)
            {
                if (nex.Members != null)
                {
                    return Expression.New(nex.Constructor, args, nex.Members);
                }
                else
                {
                    return Expression.New(nex.Constructor, args);
                }
            }

            return nex;
        }

        /// <summary>
        ///     The visit new array.
        /// </summary>
        /// <param name="na">
        ///     The na.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitNewArray(NewArrayExpression na)
        {
            IEnumerable<Expression> exprs = this.VisitExpressionList(na.Expressions);
            if (exprs != na.Expressions)
            {
                if (na.NodeType == ExpressionType.NewArrayInit)
                {
                    return Expression.NewArrayInit(na.Type.GetElementType(), exprs);
                }
                else
                {
                    return Expression.NewArrayBounds(na.Type.GetElementType(), exprs);
                }
            }

            return na;
        }

        /// <summary>
        ///     The visit parameter.
        /// </summary>
        /// <param name="p">
        ///     The p.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        /// <summary>
        ///     The visit type is.
        /// </summary>
        /// <param name="b">
        ///     The b.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
        {
            Expression expr = this.Visit(b.Expression);
            if (expr != b.Expression)
            {
                return Expression.TypeIs(expr, b.TypeOperand);
            }

            return b;
        }

        /// <summary>
        ///     The visit unary.
        /// </summary>
        /// <param name="u">
        ///     The u.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        protected virtual Expression VisitUnary(UnaryExpression u)
        {
            Expression operand = this.Visit(u.Operand);
            if (operand != u.Operand)
            {
                return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
            }

            return u;
        }

        #endregion
    }
}