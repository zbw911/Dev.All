namespace Dev.Data.Test.Domain
{
    public class OrderLine : Entity
    {
        #region Public Properties

        public virtual Order Order { get; set; }

        // for information on why we want this 'extra' property, see:
        // http://stuartgatenby.com/ef/2011/03/05/entity-framework-relationship-mapping-best-of-both-worlds-ef4-1ctp5-code-only-fluent-api/
        public virtual int OrderId { get; set; }

        public virtual double Price { get; set; }

        public virtual Product Product { get; set; }

        // for information on why we want this 'extra' property, see:
        // http://stuartgatenby.com/ef/2011/03/05/entity-framework-relationship-mapping-best-of-both-worlds-ef4-1ctp5-code-only-fluent-api/
        public virtual int ProductId { get; set; }

        public virtual int Quantity { get; set; }

        #endregion
    }
}