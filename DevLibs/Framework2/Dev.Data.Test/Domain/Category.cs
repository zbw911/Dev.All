namespace Dev.Data.Test.Domain
{
    using System.Collections.Generic;

    public class Category : Entity
    {
        #region Constructors and Destructors

        public Category()
        {
            this.Products = new List<Product>();
        }

        #endregion

        #region Public Properties

        public virtual string Name { get; set; }

        public virtual IList<Product> Products { get; set; }

        #endregion
    }
}