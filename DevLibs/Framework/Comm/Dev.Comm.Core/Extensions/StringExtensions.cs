using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Comm.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        public static T AS<T>(this string source)
        {

            return AS<T>(source, default(T));
        }


        public static T AS<T>(this string source, T defaultvalue)
        {
            T result = TypeConverter.ConvertType(source, defaultvalue);
            return result;
        }
        public static int AsInt(this string source, int defaultvalue)
        {
            return AS<int>(source, defaultvalue);
        }

        public static int AsInt(this string source)
        {
            return AS<int>(source);
        }

        public static float AsFloat(this string source, float defaultvalue)
        {
            return AS<float>(source, defaultvalue);
        }

        public static float AsFloat(this string source)
        {
            return AS<float>(source);
        }
    }
}
