using Dev.Data.Infras.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Repository.Specification
{
    using System.Linq.Expressions;

    using Dev.Demo.Entities.UserAgg;

    public class UserByNameSpecification : Specification<User>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Specification{TEntity}" /> class.
        /// </summary>
        /// <param name="predicate">
        ///     The predicate.
        /// </param>
        public UserByNameSpecification(Expression<Func<User, bool>> predicate)
            : base(predicate)
        {
        }

        public UserByNameSpecification(string name)
            : base(x => x.DisplayName.IndexOf(name) >= 0)
        {

        }
    }
}
