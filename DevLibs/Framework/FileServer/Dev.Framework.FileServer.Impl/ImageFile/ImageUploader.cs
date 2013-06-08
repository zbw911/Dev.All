// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月18日 10:44
// 
// 修改于：2013年02月18日 18:24
// 文件名：ImageUploader.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using Dev.Comm;

namespace Dev.Framework.FileServer.ImageFile
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageUploader : IImageFile
    {
        private readonly IUploadFile _curFileDisposer;
        private readonly IKey _curKeyDisposer;

        /// <summary>
        /// 设置当前的文件处理器
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="key"> </param>
        public ImageUploader(IUploadFile uploadFile, IKey key)
        {
            this._curFileDisposer = uploadFile;
            this._curFileDisposer.SetCurrentKey(key);
            this._curKeyDisposer = key;
        }

        #region IImageFile Members

        public string SaveImageFile(byte[] bytefile, string fileName, int width = 0, int height = 0)
        {
            return this.SaveImageFile(new MemoryStream(bytefile), fileName, width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytefile"></param>
        /// <param name="fileName"></param>
        /// <param name="sizes"></param>
        /// <returns></returns>
        public string SaveImageFile(byte[] bytefile, string fileName, ImagesSize[] sizes)
        {
            return this.SaveImageFile(new MemoryStream(bytefile), fileName, sizes);
        }

        public string SaveImageFile(Stream stream, string fileName, int width = 0, int height = 0)
        {
            return this.SaveImageFile(stream, fileName, new ImagesSize[] { new ImagesSize(width, height) });
        }


        public string SaveImageFile(Stream stream, string fileName, ImagesSize[] sizes)
        {
            if (sizes != null)
                foreach (var imagesSize in sizes)
                {
                    if (!this.checkWidthHeight(imagesSize.Width, imagesSize.Height))
                    {
                        throw new ArgumentException("图片大小参数错误");
                    }

                }
            //实际判断文件类型忽略文件名
            string ext = FileUtil.CheckImageExt(stream);

            if (fileName == null)
                throw new Exception("上传文件非图片类型");

            fileName = "x" + ext;

            string fileKey = this._curKeyDisposer.CreateFileKey(fileName);

            string filename = this._curFileDisposer.SaveFile(stream, fileKey);


            //this.CurKeyDisposer.CreateFileKey(filename,
            foreach (var imagesSize in sizes)
            {
                var width = imagesSize.Width;
                var height = imagesSize.Height;
                if (this.needThumb(width, height))
                {
                    //缩略后的图像数据
                    Stream thumbObj = ImageHelper.GenThumbnail(stream, width, height);
                    //保存
                    string thumbfilename = this._curFileDisposer.UpdateFile(thumbObj, fileKey, "-", width, "_", height);

                    //filename = thumbfilename;
                }
            }

            return filename;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileKey"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void UpdateImageFile(Stream stream, string fileKey, int width = 0, int height = 0)
        {
            if (!this.checkWidthHeight(width, height))
            {
                return;
            }
            //实际判断文件类型忽略文件名
            //fileName = FileUtil.CheckImageExt(stream);
            //fileName = "x" + fileName;

            string filename = this._curFileDisposer.UpdateFile(stream, fileKey);

            if (this.needThumb(width, height))
            {
                //缩略后的图像数据
                Stream thumbObj = ImageHelper.GenThumbnail(stream, width, height);
                //保存
                string thumbfilename = this._curFileDisposer.UpdateFile(thumbObj, fileKey, "-", width, "_", height);

                //filename = thumbfilename;
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytefile"></param>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void UpdateImageFile(byte[] bytefile, string fileName, int width = 0, int height = 0)
        {
            this.UpdateImageFile(new MemoryStream(bytefile), fileName, width, height);
        }


        public string GetImageUrl(string fileKey, int width = 0, int height = 0)
        {
            if (this.needThumb(width, height))
            {
                return this._curKeyDisposer.GetFileUrl(fileKey, "-", width, "_", height);
            }

            return this._curKeyDisposer.GetFileUrl(fileKey);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private bool checkWidthHeight(int width, int height)
        {
            if (width == 0 ^ height == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否要缩略
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private bool needThumb(int width, int height)
        {
            return width > 0 && height > 0;
        }





    }
}