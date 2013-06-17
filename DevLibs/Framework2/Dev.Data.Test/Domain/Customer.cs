namespace Dev.Data.Test.Domain
{
    using System;
    using System.Collections.Generic;

    public class Customer : Entity
    {
        #region Constructors and Destructors

        public Customer()
        {
            this.Orders = new List<Order>();
        }

        #endregion

        #region Public Properties

        public virtual string Firstname { get; set; }

        public virtual DateTime Inserted { get; set; }

        public virtual string Lastname { get; set; }

        public virtual IList<Order> Orders { get; set; }

        #endregion
    }
}