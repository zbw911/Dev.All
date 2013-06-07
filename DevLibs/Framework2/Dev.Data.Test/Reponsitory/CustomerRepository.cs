namespace Infrastructure.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Dev.Data;
    using Dev.Data.Infras;
    using Dev.Data.Test.Domain;

    public interface ICustomerRepository : IRepository
    {
        #region Public Methods and Operators

        Customer FindByName(string firstname, string lastname);

        IList<Customer> NewlySubscribed();

        #endregion
    }

    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        //public CustomerRepository()
        //{
        //}

        #region Constructors and Destructors

        public CustomerRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public CustomerRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        #endregion

        #region Public Methods and Operators

        public Customer FindByName(string firstname, string lastname)
        {
            return this.GetQuery<Customer>().FirstOrDefault(c => c.Firstname == firstname && c.Lastname == lastname);
        }

        public IList<Customer> NewlySubscribed()
        {
            DateTime lastMonth = DateTime.Now.Date.AddMonths(-1);

            return this.GetQuery<Customer>().Where(c => c.Inserted >= lastMonth).ToList();
        }

        #endregion
    }
}