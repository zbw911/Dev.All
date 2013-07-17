using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dev.Comm
{
    /// <summary>
    /// http://stackoverflow.com/questions/7717281/expandoobject-to-static-object-and-back-again-spanning-the-two-domains
    /// </summary>
    class ObjectDynamicConvert
    {
        public static ExpandoObject ToExpando(object staticObject)
        {
            System.Dynamic.ExpandoObject expando = new ExpandoObject();
            var dict = expando as IDictionary<string, object>;
            PropertyInfo[] properties = staticObject.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                dict[property.Name] = property.GetValue(staticObject, null);
            }

            return expando;
        }
    }
}
