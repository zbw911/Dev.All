namespace Dev.Data.Test.Domain
{
    using System;
    using System.Collections.Generic;

    public class Order : Entity
    {
        #region Constructors and Destructors

        public Order()
        {
            this.OrderLines = new List<OrderLine>();
        }

        #endregion

        #region Public Properties

        public virtual Customer Customer { get; set; }

        // for information on why we want this 'extra' property, see:
        // http://stuartgatenby.com/ef/2011/03/05/entity-framework-relationship-mapping-best-of-both-worlds-ef4-1ctp5-code-only-fluent-api/
        public virtual int CustomerId { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual IList<OrderLine> OrderLines { get; set; }

        #endregion
    }
}