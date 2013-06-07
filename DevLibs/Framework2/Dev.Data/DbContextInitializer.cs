// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:27
// 
// 修改于：2013年02月05日 17:28
// 文件名：DbContextInitializer.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data
{
    #region

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class DbContextInitializer
    {
        #region Static Fields

        private static readonly object _syncLock = new object();

        private static DbContextInitializer _instance;

        #endregion

        #region Fields

        private bool _isInitialized;

        #endregion

        #region Constructors and Destructors

        protected DbContextInitializer()
        {
        }

        #endregion

        #region Public Methods and Operators

        public static DbContextInitializer Instance()
        {
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DbContextInitializer();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        ///     This is the method which should be given the call to intialize the DbContext; e.g.,
        ///     DbContextInitializer.Instance().InitializeDbContextOnce(() => InitializeDbContext());
        ///     where InitializeDbContext() is a method which calls DbContextManager.Init()
        /// </summary>
        /// <param name="initMethod"></param>
        public void InitializeDbContextOnce(Action initMethod)
        {
            lock (_syncLock)
            {
                if (!this._isInitialized)
                {
                    initMethod();
                    this._isInitialized = true;
                }
            }
        }

        #endregion
    }
}