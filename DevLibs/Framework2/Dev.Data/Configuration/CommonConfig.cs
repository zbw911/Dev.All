// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月05日 13:47
// 
// 修改于：2013年02月05日 17:28
// 文件名：CommonConfig.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data.Configuration
{
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     用于配置的类
    /// </summary>
    public class CommonConfig
    {
        #region Static Fields

        private static CommonConfig _config;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     创建自身一个实例
        /// </summary>
        /// <returns></returns>
        public static CommonConfig Instance()
        {
            return _config ?? (_config = new CommonConfig());
        }

        /// <summary>
        ///     配置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionStringName"></param>
        /// <param name="createDbFileImmediately">是否立即创建不存在的数据库文件 </param>
        /// <returns></returns>
        public CommonConfig ConfigureData<T>(string connectionStringName, bool createDbFileImmediately = false) where T : DbContext
        {
            DbContextManager.Init<T>(connectionStringName, createDbFileImmediately);
            return this;
        }

        /// <summary>
        ///     配置数据,使用程序集加载
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="recreateDatabaseIfExist"></param>
        /// <param name="lazyLoadingEnabled"></param>
        /// <returns></returns>
        public CommonConfig ConfigureData(
            string connectionStringName,
            string[] mappingAssemblies,
            bool recreateDatabaseIfExist = false,
            bool lazyLoadingEnabled = true)
        {
            DbContextManager.Init(connectionStringName, mappingAssemblies, recreateDatabaseIfExist, lazyLoadingEnabled);
            return this;
        }

        /// <summary>
        ///     配置数据,使用程序集进行加载
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="assemblies"></param>
        /// <param name="recreateDatabaseIfExist"></param>
        /// <param name="lazyLoadingEnabled"></param>
        /// <returns></returns>
        public CommonConfig ConfigureData(
            string connectionStringName,
            Assembly[] assemblies,
            bool recreateDatabaseIfExist = false,
            bool lazyLoadingEnabled = true)
        {
            DbContextManager.Init(
                connectionStringName,
                assemblies.Select(assembly => assembly.CodeBase).ToArray(),
                recreateDatabaseIfExist,
                lazyLoadingEnabled);
            return this;
        }

        /// <summary>
        ///     存储模式
        /// </summary>
        /// <param name="dbContextStorage"></param>
        /// <returns></returns>
        public CommonConfig ConfigureDbContextStorage(IDbContextStorage dbContextStorage)
        {
            DbContextManager.InitStorage(dbContextStorage);
            return this;
        }

        #endregion
    }


}