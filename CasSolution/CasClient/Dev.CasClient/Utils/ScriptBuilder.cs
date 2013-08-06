using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev.CasClient.Utils
{
    /// <summary>
    /// 生成常见的方法
    /// </summary>
    public static class ScriptBuilder
    {
        /// <summary>
        /// 生成 script type="text/javascript" src="@Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl/cas/jssdk" 
        /// </summary>
        /// <returns></returns>
        public static string BuildSdkJsReference(string version = "")
        {
            return string.Format(@"<script type=""text/javascript"" src=""{0}/cas/jssdk{1}""></script>",
                Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl, (string.IsNullOrEmpty(version) ? "" : "?v=" + version));
        }

        /// <summary>
        /// 生成 CasSdk.Init（。。。） 代码块
        /// </summary>
        /// <returns></returns>
        public static string BuildCasInit()
        {
            var initscript = string.Format(@"CasSdk.Init(
                '{0}',
                '{1}',
                '{2}'
                
            );"
                //,(User.UserInfo.IsAuthenticated ? "true" : "false")
                //, (User.UserInfo.IsAuthenticated ? User.UserInfo.GetCurrentUserName() : "")
              , Configuration.CasClientConfiguration.Config.LocalLoginPath
             , Configuration.CasClientConfiguration.Config.LocalLogOffPath
             , Configuration.CasClientConfiguration.Config.LocalCheckPath
                //, isLocal ? "true" : "false"
              );


            return initscript;
        }

        /// <summary>
        /// 创建$(function(){}),代码块
        /// </summary>
        /// <param name="inner"></param>
        /// <returns></returns>
        public static string BuildReady(string inner)
        {
            return string.Format(@"$(function () {
 {0}
}", inner);
        }
    }
}
