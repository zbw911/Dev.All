// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：ShareFileKey.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using Dev.Comm;
using Dev.Framework.FileServer.Config;
using Dev.Framework.FileServer.HashServer;

namespace Dev.Framework.FileServer.LocalUploaderFileImpl
{
    /// <summary>
    /// 本站点文件实现方式
    /// </summary>
    public class LocalFileKey : IKey
    {
        #region IKey Members

        public string CreateFileKey(string fileName, params object[] param)
        {
            string unqid = Security.GetMD5(Guid.NewGuid().ToString());
            DateTime now = DateTime.Now;
            //var dirname = HashPath.DatePath(now) + @"\" + HashPath.GetUploadPath(unqid);
            string extname = Path.GetExtension(fileName);

            string paramInfo = "";
            foreach (var par in param)
            {
                paramInfo += par.ToString();
            }

            string savefilename = unqid + paramInfo + extname;

            Server server = HashFileServer.HashConfig(unqid);

            return server.id + "-" + now.ToString("yyyy-MM-dd") + "-" + savefilename;
        }

        /// <summary>
        /// 取得文件的绝对位置信息
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public FileSaveInfo GetFileSavePath(string fileKey, params object[] param)
        {
            //         0   1   2  3    4                                 
            // case 1: 2-2011-04-26-adf96d2c6be8dfade2a049f1ee4b8d7d.jpg
            // case 2: 2-2011-04-26-adf96d2c6be8dfade2a049f1ee4b8d7d-100_1000.jpg

            //兼容以前的数据，如果是 以 / 及 http://开头的 直接返回数据
            if (string.IsNullOrEmpty(fileKey))
            {
                throw new Exception("Error fileKey");
            }

            if (fileKey.StartsWith("/") || fileKey.ToLower().StartsWith("http://"))
            {
                throw new Exception("Error fileKey,Please Use FileKey");
            }

            string[] parts = SplitFileKey(fileKey);
            if (parts == null) return null;

            Server server = HashFileServer.GetServer(int.Parse(parts[0]));

            string paramInfo = "";
            foreach (var par in param)
            {
                paramInfo += par.ToString();
            }

            parts[4] += paramInfo;

            return new FileSaveInfo
                       {
                           FileServer = server,
                           Dirname = @"" + parts[1] + @"/" + parts[2] + @"/" + parts[3] + @"/" + GetUploadPath(parts[4]),
                           Extname = Path.GetExtension(fileKey),
                           Savefilename = parts[4] + Path.GetExtension(fileKey)
                       };
        }

        public string GetFileUrl(string fileKey, params object[] param)
        {
            //         0   1   2  3    4                                 
            // case 1: 2-2011-04-26-adf96d2c6be8dfade2a049f1ee4b8d7d.jpg
            // case 2: 2-2011-04-26-adf96d2c6be8dfade2a049f1ee4b8d7d-100_1000.jpg

            //兼容以前的数据，如果是 以 / 及 http://开头的 直接返回数据
            if (!string.IsNullOrWhiteSpace(fileKey))
            {
                if (fileKey.StartsWith("/") || fileKey.ToLower().StartsWith("http://"))
                {
                    return fileKey;
                }

                string[] parts = SplitFileKey(fileKey);
                if (parts == null || parts.Length < 5)
                    return fileKey;



                string paramInfo = "";
                foreach (var par in param)
                {
                    paramInfo += par.ToString();
                }

                parts[4] += paramInfo;

                Server server = HashFileServer.GetServer(int.Parse(parts[0]));

                string url = server.serverurl;

                string suburl = @"/" + parts[1] + @"/" + parts[2] + @"/" + parts[3] + @"/" + GetUploadPath(parts[4]) +
                                @"/" + parts[4] + Path.GetExtension(fileKey);
                return url + suburl;
            }
            else
            {
                return "";
            }
        }

        #endregion

        /// <summary>
        /// 拆分成部分
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string[] SplitFileKey(string key)
        {
            string[] parts = key.Split("-.".ToCharArray());
            return parts;
        }

        private static string GetUploadPath(string strin)
        {
            //36*36*36
            strin = strin.Replace("-", "");
            return strin.Substring(0, 2) + @"/" + strin.Substring(2, 1) + @"/" + strin.Substring(3, 1) + @"";
        }
    }
}