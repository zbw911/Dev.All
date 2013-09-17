// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月07日 14:25
//  
//  修改于：2013年09月17日 11:33
//  文件名：Dev.Libs/Dev.Comm.Core/AssemblyManager.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Comm.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///   程序集管理
    /// </summary>
    public class AssemblyManager
    {
        #region Public Methods and Operators

        public static Assembly GetAssembly(string assemblyName)
        {
            foreach (var item in GetDomainAssemblies())
            {
                if (CompareAssembly(item, assemblyName))
                {
                    return item;
                }
            }
            return null;
        }

        public static string GetAssemblyFileName(string assemblyName)
        {
            var assembly = GetAssembly(assemblyName);

            if (assembly != null) return (assembly.CodeBase);

            return null;
        }


        /// <summary>
        ///   取得所有与当前域相关的 Assembly
        /// </summary>
        /// <returns> </returns>
        public static List<Assembly> LoadAllAssemblys()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var loadedPaths = loadedAssemblies.Where(x => !x.IsDynamic).Select(a => a.Location);

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad =
                referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            toLoad.ForEach(
                path => loadedAssemblies.Add(Assembly.LoadFrom(path)));

            return loadedAssemblies;
        }


        public static IEnumerable<Assembly> GetDomainAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public static IEnumerable<object> GetTypeInstances(Type type)
        {
            return GetTypes(type).Select(it => Activator.CreateInstance(it));
        }

        public static IEnumerable<T> GetTypeInstances<T>() where T : class
        {
            return GetTypes(typeof (T)).Select(it => Activator.CreateInstance(it) as T);
        }

        public static IEnumerable<Type> GetTypes()
        {
            var types = new List<Type>();
            foreach (var assembly in GetDomainAssemblies())
            {
                //var asstypes = TryGetAssembly(assembly);

                types.AddRange(assembly.GetTypes());
            }
            return types;
        }


        private static IEnumerable<Type> TryGetAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetTypes(Type type)
        {
            var types = new List<Type>();

            types.AddRange(GetTypes().Where(it => type.IsAssignableFrom(it) && !it.IsInterface && !it.IsAbstract));

            return types.Distinct();
        }

        public static IEnumerable<Type> GetTypes<T>()
        {
            return GetTypes(typeof (T));
        }

        public static IEnumerable<Type> GetTypes(string fileName)
        {
            var assembly = GetAssembly(fileName);
            if (assembly != null)
            {
                return assembly.GetTypes().Where(it => !it.IsInterface && !it.IsAbstract);
            }
            return new Type[0];
        }

        #endregion

        #region Methods

        /// <summary>
        ///   比较程序集
        /// </summary>
        /// <param name="assembly"> </param>
        /// <param name="assemblyName"> </param>
        /// <returns> </returns>
        private static bool CompareAssembly(Assembly assembly, string assemblyName)
        {
            return !assembly.IsDynamic && !assembly.GlobalAssemblyCache
                   && assembly.GetName().Name
                          .EqualsOrNullEmpty(assemblyName, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion
    }

    internal static class strExt
    {
        #region Public Methods and Operators

        public static bool EqualsOrNullEmpty(this string str1, string str2, StringComparison comparisonType)
        {
            return String.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
        }

        #endregion
    }
}