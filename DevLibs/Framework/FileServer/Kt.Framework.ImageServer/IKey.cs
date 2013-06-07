// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：IKey.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using Dev.Framework.FileServer.Config;

namespace Dev.Framework.FileServer
{
    public class KeyInfo
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string unqid { get; set; }

        /// <summary>
        /// 当前的时间
        /// </summary>
        public DateTime now { get; set; }

        /// <summary>
        /// 生成的路径信息
        /// </summary>
        public string dirname { get; set; }

        public string extname { get; set; }
        public string savefilename { get; set; }
    }

    /// <summary>
    /// 文件的绝对路径
    /// </summary>
    public class FileSaveInfo
    {
        public Server FileServer { get; set; }

        /// <summary>
        /// 生成的路径信息
        /// </summary>
        public string dirname { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string extname { get; set; }

        /// <summary>
        /// 保存原始文件名
        /// </summary>
        public string savefilename { get; set; }
    }

    public interface IKey
    {
        ///// <summary>
        ///// 生成KEY返回相关信息
        ///// </summary>
        ///// <returns></returns>
        //KeyInfo CreateFileInfo(string fileName, params object[] param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        string CreateFileKey(string fileName, params object[] param);

        //string CreateFileKey(int serverId, DateTime now, string savefilename, params object[] param);

        /// <summary>
        /// 通过URLKEY生成URL
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        string GetFileUrl(string fileKey, params object[] param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        FileSaveInfo GetFileSavePath(string fileKey, object[] param);
    }
}