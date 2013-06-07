using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.Style2.Repository.Specification
{
    using System.Linq.Expressions;

    using Dev.Data.Infras.Specification;
    using Dev.Demo.Entities2.Models;

    public class SystemAdminSpecification : Specification<Admin>
    {
        public SystemAdminSpecification()
            : base(x => x.Typeid == 1)
        {
        }

    }
}
