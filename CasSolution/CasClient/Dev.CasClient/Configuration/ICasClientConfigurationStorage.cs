// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月19日 15:01
// 
// 修改于：2013年02月19日 15:10
// 文件名：ICasClientConfigurationStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;

namespace Dev.CasClient.Configuration
{
    /// <summary>
    ///   配置文件存储接口， zbw911
    /// </summary>
    public interface ICasClientConfigurationStorage
    {
        #region Instance Methods

        /// <summary>
        ///   取得
        /// </summary>
        /// <returns> </returns>
        CasClientConfiguration Get();

        /// <summary>
        ///   根据名称取得
        /// </summary>
        /// <param name="configname"> </param>
        /// <returns> </returns>
        CasClientConfiguration Get(string configname);

        /// <summary>
        ///   保存
        /// </summary>
        /// <param name="config"> </param>
        /// <returns> </returns>
        void Save(CasClientConfiguration config);

        /// <summary>
        /// </summary>
        /// <param name="config"> </param>
        /// <param name="configname"> </param>
        void Save(CasClientConfiguration config, string configname);

        #endregion

        #region Event Declarations

        /// <summary>
        ///   配置改变
        /// </summary>
        event EventHandler<EventArgs> ConfigChangedEvent;

        #endregion
    }
}