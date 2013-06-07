// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月16日 16:42
// 
// 修改于：2013年02月16日 16:43
// 文件名：ProjectionsExtensionMethods.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
namespace Dev.Crosscutting.Adapter
{
    using System.Collections.Generic;

    using Dev.Crosscutting.Adapter.Adapter;
          
    /// <summary>
    /// 这个方法来自于http://microsoftnlayerapp.codeplex.com/
    /// 对原有方法进行了部分改进，将 ：Entity 改为 object ,
    /// 当然，副作用也是显而易见的，
    /// 在所有的“类”实例后面都会加上这个扩展方法，
    /// 但原方法有更强类型化，两者取其一吧，对于EF DB first 中，如果有一个使用基类型反而是一个比较好的方法。
    ///  add by zbw911 2013-2-16
    /// </summary>
    public static class ProjectionsExtensionMethods
    {
        /// <summary>
        /// Project a type using a DTO，
        /// 这个只用于单个实例，由于为了通用性，就使用了object ,但同时也会出现一个问题，对于任何 “类”实例的都会出现这个扩展了
        /// </summary>
        /// <typeparam name="TProjection">The dto projection</typeparam>
        /// <param name="entity">The source entity to project</param>
        /// <returns>The projected type</returns>
        public static TProjection ProjectedAs<TProjection>(this object item)
            where TProjection : class,new()
        {
            var adapter = TypeAdapterFactory.CreateAdapter();
            return adapter.Adapt<TProjection>(item);
        }

        /// <summary>
        /// projected a enumerable collection of items,
        /// 适用于集全类型
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <param name="items">the collection of entity items</param>
        /// <returns>Projected collection</returns>
        public static List<TProjection> ProjectedAsCollection<TProjection>(this IEnumerable<object> items)
            where TProjection : class,new()
        {

            var adapter = TypeAdapterFactory.CreateAdapter();

            return adapter.Adapt<List<TProjection>>(items);
        }
    }




}
