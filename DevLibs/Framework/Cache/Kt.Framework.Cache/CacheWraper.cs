// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：CacheWraper.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;

namespace Dev.Framework.Cache
{


    /// <summary>
    /// 事实上的空
    /// </summary>
    [Serializable]
    public class FactNull
    {
    }

    /// <summary>
    /// 包装器
    /// </summary>
    public class CacheWraper : ICacheWraper
    {
        public ICacheState CacheState;

        public CacheWraper(ICacheState CacheState)
        {
            this.CacheState = CacheState;
        }

        #region ICacheWraper Members

        /// <summary>
        /// 绝对过期的智能方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(object key, DateTime absoluteExpiration, Func<T> GetDataFunc)
        {

            var cachekey = key.BuildFullKey<T>();

            var instance = this.CacheState.GetObjectByKey(cachekey);


            if (instance == null)
                instance = GetDataFunc();
            //取回的数据还是空的
            if (instance == null)
            {
                this.CacheState.PutObjectByKey(cachekey, new FactNull(), absoluteExpiration);
                return default(T);
            }

            if (instance is FactNull)
                return default(T);

            if (instance is T)
            {
                this.CacheState.Put(key, (T)instance, absoluteExpiration);
                return (T)instance;
            }

            throw new Exception("Error");
        }

        /// <summary>
        /// 绝对过期的智能方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="absoluteExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(DateTime absoluteExpiration, Func<T> GetDataFunc)
        {
            return this.SmartyGetPut(null, absoluteExpiration, GetDataFunc);
        }

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(object key, TimeSpan slidingExpiration, Func<T> GetDataFunc)
        {
            var instance = this.CacheState.Get<T>(key);
            if (instance != null) return instance;

            instance = GetDataFunc();
            if (instance == null) return instance;
            //放入缓存
            this.CacheState.Put(key, instance, slidingExpiration);
            return instance;
        }

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="slidingExpiration"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(TimeSpan slidingExpiration, Func<T> GetDataFunc)
        {
            return this.SmartyGetPut(null, slidingExpiration, GetDataFunc);
        }

        /// <summary>
        /// 永远不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(object key, Func<T> GetDataFunc)
        {


            var instance = this.CacheState.Get<T>(key);
            //if (instance != defaultT) return instance;

            //if(defaultT!= null)

            if (instance != null && instance.Equals(default(T)))
            {

            }


            var isclass = typeof(T).IsValueType;

            var equal = instance.Equals(default(T));

            instance = GetDataFunc();

            if (instance == null) return instance;

            //放入缓存
            this.CacheState.Put(key, instance);
            return instance;
        }

        /// <summary>
        /// 永远不过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="GetDataFunc"></param>
        /// <returns></returns>
        public T SmartyGetPut<T>(Func<T> GetDataFunc)
        {
            return this.SmartyGetPut(null, GetDataFunc);
        }

        #endregion
    }
}