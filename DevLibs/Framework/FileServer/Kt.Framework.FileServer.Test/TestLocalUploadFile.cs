using System;
using System.IO;
using Dev.Framework.FileServer.Config;
using Dev.Framework.FileServer.LocalUploaderFileImpl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Framework.FileServer.Test
{
    [TestClass]
    public class TestLocalUploadFile
    {
        [TestMethod]
        public void TestMethod1()
        {
            var filepath =
                @"C:\Users\Administrator\Source\Repos\Dev.All\DevLibs\Framework\FileServer\Kt.Framework.FileServer.Test\TestLocalUploadFile.cs";
            var x = new ReadConfig("TestLocalUploadFile.config");
            IKey key = new LocalFileKey();
            IUploadFile upload = new LocalUploadFile(key);

            var filekey = key.CreateFileKey(filepath);


            Console.WriteLine(filekey);


            var s = key.GetFileSavePath(filekey);


            Console.WriteLine(s);


            Console.WriteLine(Dev.Comm.JsonConvert.ToJsonStr(s));


            var uploadedkey = upload.SaveFile(File.OpenRead(filepath), filekey);

            //upload.SaveFile(File.OpenRead(".."), filekey);

           



        }
    }
}
