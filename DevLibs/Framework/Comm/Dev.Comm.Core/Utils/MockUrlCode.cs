// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月07日 14:25
//  
//  修改于：2013年09月17日 11:32
//  文件名：Dev.Libs/Dev.Comm.Core/MockUrlCode.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Comm.Utils
{
    /// <summary>
    ///   不使用　System.Web.进行ＵＲＬ编码　，　added by zbw911
    /// </summary>
    public class MockUrlCode
    {
        ///// <summary>
        ///// Url编码
        ///// </summary>
        ///// <param name="str">字符串</param>
        ///// <returns></returns>
        //public static string UrlEncode(string str, Encoding Encode)
        //{
        //    string result = string.Empty;
        //    const string keys = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        string str4 = str.Substring(i, 1);
        //        if (keys.Contains(str4))
        //        {
        //            result = result + str4;
        //        }
        //        else
        //        {
        //            result = Encode.GetBytes(str4).Aggregate(result, (current, n) => current + "%" + n.ToString("X"));
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        ///   默认源为gb2312
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static string UrlEncode(string str)
        {
            return System.Uri.EscapeDataString(str);
            //return UrlEncode(str, Encoding.GetEncoding("gb2312"));
        }


        public static string UrlDecode(string str)
        {
            return System.Uri.UnescapeDataString(str);
        }

        //public static string UrlDecode(string str)
        //{
        //    return UrlDecode(str, Encoding.GetEncoding("gb2312"));
        //}
    }
}