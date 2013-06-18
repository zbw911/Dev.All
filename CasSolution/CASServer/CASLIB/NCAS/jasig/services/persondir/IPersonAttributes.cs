using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dev.CasServer.principal;

namespace Dev.CasServer.jasig.services.persondir
{
    public interface IPersonAttributes : Principal
    {
        /**
         * @return The immutable Map of all attributes for the person.
         */
        Dictionary<String, List<Object>> getAttributes();

        /**
         * The value for the attribute, null if no value exists or the first value is null, if there are multiple values
         * the first is returned.
         * 
         * @param name The name of the attribute to get the value for
         * @return The first value for the attribute
         */
        Object getAttributeValue(String name);

        /**
         * All values of the attribute, null if no values exist.
         * 
         * @param name The name of the attribute to get the values for
         * @return All values for the attribute
         */
        List<Object> getAttributeValues(String name);
    }

}
