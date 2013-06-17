namespace Dev.Data.Test.Specification
{
    using Dev.Data.Infras.Specification;
    using Dev.Data.Test.Domain;

    public class ProductOnSaleSpecification : Specification<Product>
    {
        #region Constructors and Destructors

        public ProductOnSaleSpecification()
            : base(p => p.Price < 100)
        {
        }

        #endregion
    }
}