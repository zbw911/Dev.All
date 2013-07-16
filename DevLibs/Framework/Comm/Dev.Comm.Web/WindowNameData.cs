using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Comm.Web
{
    /// <summary>
    /// 生成 为 window.name跨域的数据
    /// </summary>
    public class WindowNameData
    {
        public static string StringData(string data)
        {
            return GenStr(data);
        }


        public static string StringData<T>(T data)
        {
            var str = Dev.Comm.JsonConvert.ToJsonStr(data);
            return GenStr(str);
        }


        private static string GenStr(string data)
        {
            return string.Format(@"<script>window.name = '{0}';</script>", data);
        }




    }
}
