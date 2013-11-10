using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dev.Comm.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class DictionaryUtil
    {
        /// <summary>
        /// 将返回的参数转化为Dic类型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IDictionary StringArrayToDic(params string[] param)
        {
            IDictionary data = new Hashtable();
            for (int index = 0; index < param.Length; index++)
            {
                var s = param[index];

                data.Add(index, s);
            }

            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IDictionary CollectionToDic(ICollection collection)
        {

            IDictionary data = new Hashtable();
            //for (int index = 0; index < collection.Count; index++)
            //{
            //    var s = collection [index];

            //    data.Add(index, s);
            //}

            var index = 0;

            foreach (var c in collection)
            {
                data[index] = c;// Add(index, c);
            }

            return data;
        }

        internal static object GetPropertyValue(object obj, string property)
        {
            return TypeDescriptor.GetProperties(obj)[property].GetValue(obj);
        }

        /// <summary>
        /// 转化为Dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor property in properties)
            {
                result.Add(property.Name, property.GetValue(obj));
            }
            return result;
        }
    }
}
