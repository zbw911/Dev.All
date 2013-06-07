// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：ShareUploadFile.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.IO;
using Dev.Comm;
using Dev.Comm.NetFile;
using Dev.Framework.FileServer.Config;

namespace Dev.Framework.FileServer.ShareImpl
{
    public class ShareUploadFile : IUploadFile
    {
        private IKey CurrentKey;

        #region IUploadFile Members

        /// <summary>
        /// 生成KEY方案
        /// </summary>
        /// <param name="Key"></param>
        public void SetCurrentKey(IKey Key)
        {
            this.CurrentKey = Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytefile"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SaveFile(byte[] bytefile, string fileKey, params object[] param)
        {
            FileSaveInfo fileInfo = this.CurrentKey.GetFileSavePath(fileKey, param);

            Server server = fileInfo.FileServer;

            var filehelper = new FileHelper
                                 {
                                     hostIp = server.hostip,
                                     password = server.password,
                                     username = server.username,
                                     startdirname = server.startdirname
                                 };

            filehelper.WriteFile(fileInfo.dirname, fileInfo.savefilename, bytefile);

            return fileKey;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public string SaveFile(Stream stream, string fileKey, params object[] param)
        {
            stream.Seek(0, SeekOrigin.Begin);
            return this.SaveFile(FileUtil.StreamToBytes(stream), fileKey, param);
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="bytefile"></param>
        /// <param name="fileKey"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string UpdateFile(byte[] bytefile, string fileKey, params object[] param)
        {
            FileSaveInfo fileSaveInfo = this.CurrentKey.GetFileSavePath(fileKey, param);

            var filehelper = new FileHelper
                                 {
                                     hostIp = fileSaveInfo.FileServer.hostip,
                                     password = fileSaveInfo.FileServer.password,
                                     username = fileSaveInfo.FileServer.username,
                                     startdirname = fileSaveInfo.FileServer.startdirname
                                 };

            filehelper.UpdateFile(fileSaveInfo.dirname, fileSaveInfo.savefilename, bytefile);

            return fileKey;
        }


        public string UpdateFile(Stream stream, string fileKey, params object[] param)
        {
            stream.Seek(0, SeekOrigin.Begin);
            return this.UpdateFile(FileUtil.StreamToBytes(stream), fileKey, param);
        }

        #endregion
    }
}