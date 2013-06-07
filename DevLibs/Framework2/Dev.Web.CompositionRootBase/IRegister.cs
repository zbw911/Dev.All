// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月02日 17:15
// 
// 修改于：2013年02月05日 17:32
// 文件名：IRegister.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Web.CompositionRootBase
{
    using Ninject;

    public interface IRegister
    {
        #region Public Properties

        IKernel Kernel { get; set; }

        #endregion

        #region Public Methods and Operators

        void Register();

        #endregion
    }
}