// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:27
// 
// 修改于：2013年02月05日 17:28
// 文件名：DbContextBuilder.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity.Core.Objects;

namespace Dev.Data
{
    #region

    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Reflection;

    #endregion

    public interface IDbContextBuilder<T>
        where T : DbContext
    {
        #region Public Methods and Operators

        T BuildDbContext();

        #endregion
    }

    public sealed class DbContextBuilder<T> : DbModelBuilder, IDbContextBuilder<T>
        where T : DbContext
    {
        #region Fields

        private readonly ConnectionStringSettings _cnStringSettings;

        private readonly DbProviderFactory _factory;

        private readonly bool _lazyLoadingEnabled;

        private readonly bool _recreateDatabaseIfExists;

        #endregion

        #region Constructors and Destructors

        public DbContextBuilder(
            string connectionStringName,
            string[] mappingAssemblies,
            bool recreateDatabaseIfExists,
            bool lazyLoadingEnabled)
        {
            this.Conventions.Remove<IncludeMetadataConvention>();

            this._cnStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            this._factory = DbProviderFactories.GetFactory(this._cnStringSettings.ProviderName);
            this._recreateDatabaseIfExists = recreateDatabaseIfExists;
            this._lazyLoadingEnabled = lazyLoadingEnabled;

            this.AddConfigurations(mappingAssemblies);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a new <see cref="ObjectContext" />.
        /// </summary>
        /// <param name="lazyLoadingEnabled">
        ///     if set to <c>true</c> [lazy loading enabled].
        /// </param>
        /// <param name="recreateDatabaseIfExist">
        ///     if set to <c>true</c> [recreate database if exist].
        /// </param>
        /// <returns></returns>
        public T BuildDbContext()
        {
            DbConnection cn = this._factory.CreateConnection();
            cn.ConnectionString = this._cnStringSettings.ConnectionString;

            DbModel dbModel = this.Build(cn);

            var ctx = dbModel.Compile().CreateObjectContext<ObjectContext>(cn);
            ctx.ContextOptions.LazyLoadingEnabled = this._lazyLoadingEnabled;

            if (!ctx.DatabaseExists())
            {
                ctx.CreateDatabase();
            }
            else if (this._recreateDatabaseIfExists)
            {
                ctx.DeleteDatabase();
                ctx.CreateDatabase();
            }

            return (T)new DbContext(ctx, true);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Ensures the assembly name is qualified
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private static string MakeLoadReadyAssemblyName(string assemblyName)
        {
            return (assemblyName.ToLower().IndexOf(".dll") == -1) ? assemblyName.Trim() + ".dll" : assemblyName.Trim();
        }

        /// <summary>
        ///     Adds mapping classes contained in provided assemblies and register entities as well
        /// </summary>
        /// <param name="mappingAssemblies"></param>
        private void AddConfigurations(string[] mappingAssemblies)
        {
            if (mappingAssemblies == null || mappingAssemblies.Length == 0)
            {
                throw new ArgumentNullException("mappingAssemblies", "You must specify at least one mapping assembly");
            }

            bool hasMappingClass = false;
            foreach (string mappingAssembly in mappingAssemblies)
            {
                Assembly asm = Assembly.LoadFrom(MakeLoadReadyAssemblyName(mappingAssembly));

                foreach (Type type in asm.GetTypes())
                {
                    if (!type.IsAbstract)
                    {
                        if (type.BaseType.IsGenericType && this.IsMappingClass(type.BaseType))
                        {
                            hasMappingClass = true;

                            // http://areaofinterest.wordpress.com/2010/12/08/dynamically-load-entity-configurations-in-ef-codefirst-ctp5/
                            dynamic configurationInstance = Activator.CreateInstance(type);
                            this.Configurations.Add(configurationInstance);
                        }
                    }
                }
            }

            if (!hasMappingClass)
            {
                throw new ArgumentException("No mapping class found!");
            }
        }

        /// <summary>
        ///     Determines whether a type is a subclass of entity mapping type
        /// </summary>
        /// <param name="mappingType">Type of the mapping.</param>
        /// <returns>
        ///     <c>true</c> if it is mapping class; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMappingClass(Type mappingType)
        {
            Type baseType = typeof(EntityTypeConfiguration<>);
            if (mappingType.GetGenericTypeDefinition() == baseType)
            {
                return true;
            }
            if ((mappingType.BaseType != null) && !mappingType.BaseType.IsAbstract && mappingType.BaseType.IsGenericType)
            {
                return this.IsMappingClass(mappingType.BaseType);
            }
            return false;
        }

        #endregion
    }
}