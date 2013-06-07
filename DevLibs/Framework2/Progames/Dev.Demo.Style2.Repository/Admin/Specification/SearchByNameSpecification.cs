using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Style2.Repository.Specification
{
    using System.Linq.Expressions;

    using Dev.Data.Infras.Specification;
    using Dev.Demo.Entities2.Models;

    public class SearchByNameSpecification : Specification<Admin>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Specification{TEntity}" /> class.
        /// </summary>
        /// <param name="predicate">
        ///     The predicate.
        /// </param>
        public SearchByNameSpecification(string username)
            : base(x => x.Tname.IndexOf(username) > 0 || x.Userid.IndexOf(username) > 0)
        {
        }
    }
}
