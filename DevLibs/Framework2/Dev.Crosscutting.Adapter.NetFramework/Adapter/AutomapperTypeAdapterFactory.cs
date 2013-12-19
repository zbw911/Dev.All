// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年02月16日 13:52
// 
// 修改于：2013年02月16日 13:55
// 文件名：AutomapperTypeAdapterFactory.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using Dev.Comm;

namespace Dev.Crosscutting.Adapter.NetFramework.Adapter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;
    using Dev.Crosscutting.Adapter.Adapter;

    public class AutomapperTypeAdapterFactory : ITypeAdapterFactory
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {

            var loadedlibs = AssemblyManager.LoadAllAssemblys();

            //scan all assemblies finding Automapper Profile
            IEnumerable<Type> profiles = loadedlibs
                         .SelectMany(a => a.GetTypes())
                         .Where(t => t.BaseType == typeof(Profile));



            Mapper.Initialize(
                cfg =>
                {
                    foreach (Type item in profiles)
                    {
                        if (item.FullName != "AutoMapper.SelfProfiler`2")
                        {
                            var profile = Activator.CreateInstance(item) as Profile;
                            if (profile == null)
                            {
                                throw new ArgumentNullException("profile");
                            }

                            cfg.AddProfile(profile);
                        }
                    }
                });
        }

        #endregion

        #region Public Methods and Operators

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}