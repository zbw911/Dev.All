using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCAS.jasig.web.flow
{
    public class AbstractAction
    {
        protected Event result(string p)
        {
            throw new NotImplementedException();
        }

        protected Event error()
        {
            throw new NotImplementedException();
        }

        protected Event success()
        {
            throw new NotImplementedException();
        }
    }
}
