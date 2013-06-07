// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月16日 13:52
// 
// 修改于：2013年02月16日 16:43
// 文件名：ITypeAdapterFactory.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
namespace Dev.Crosscutting.Adapter.Adapter
{
    /// <summary>
    /// Base contract for adapter factory
    /// </summary>
    public interface  ITypeAdapterFactory
    {
        /// <summary>
        /// Create a type adater
        /// </summary>
        /// <returns>The created ITypeAdapter</returns>
        ITypeAdapter Create();
    }
}
