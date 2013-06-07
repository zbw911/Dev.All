using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.Demo.ViewModels
{
    /// <summary>
    /// 用户权限组模型
    /// </summary>
    public class PurviewGroup
    {
        public string GroupName { get; set; }
        public List<KeyValuePair<string, string>> PurviewKeyValues { get; set; }
    }
}
