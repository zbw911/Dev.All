// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：DocFileUploader.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.IO;

namespace Dev.Framework.FileServer.DocFile
{
    public class DocFileUploader : IDocFile
    {
        private readonly IUploadFile CurFileDisposer;
        private readonly IKey CurKeyDisposer;

        /// <summary>
        /// 设置当前的文件处理器
        /// </summary>
        /// <param name="IUploadFile"></param>
        public DocFileUploader(IUploadFile IUploadFile, IKey Key)
        {
            this.CurFileDisposer = IUploadFile;
            this.CurFileDisposer.SetCurrentKey(Key);
            this.CurKeyDisposer = Key;
        }

        #region IDocFile Members

        public string Save(Stream stream, string fileName)
        {
            string fileKey = this.CurKeyDisposer.CreateFileKey(fileName);

            this.CurFileDisposer.SaveFile(stream, fileKey);

            return fileKey;
        }

        public string Update(Stream stream, string fileKey)
        {
            this.CurFileDisposer.UpdateFile(stream, fileKey);

            return fileKey;
        }


        public string GetDocUrl(string fileKey)
        {
            return this.CurKeyDisposer.GetFileUrl(fileKey);
        }

        #endregion
    }
}