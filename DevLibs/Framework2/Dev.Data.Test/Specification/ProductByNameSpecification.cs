namespace Dev.Data.Test.Specification
{
    using Dev.Data.Infras.Specification;
    using Dev.Data.Test.Domain;

    public class ProductByNameSpecification : Specification<Product>
    {
        #region Constructors and Destructors

        public ProductByNameSpecification(string nameToMatch)
            : base(p => p.Name == nameToMatch)
        {
        }

        #endregion
    }
}