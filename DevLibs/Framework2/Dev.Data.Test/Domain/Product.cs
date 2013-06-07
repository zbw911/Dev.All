namespace Dev.Data.Test.Domain
{
    using System.Collections.Generic;

    public class Product : Entity
    {
        #region Constructors and Destructors

        public Product()
        {
            this.Categories = new List<Category>();
            this.OrderLines = new List<OrderLine>();
        }

        #endregion

        #region Public Properties

        public virtual IList<Category> Categories { get; set; }

        public virtual string Description { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<OrderLine> OrderLines { get; set; }

        public virtual double Price { get; set; }

        #endregion
    }
}