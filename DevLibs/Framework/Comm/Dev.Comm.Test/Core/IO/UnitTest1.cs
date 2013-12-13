using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Comm.Test.Core.IO
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var filepath = @"d:\aaa.txt";

            //if (!File.Exists(filepath))
            //{
            //    File.Create(filepath);
            //}




            Dev.Comm.FileUtil.DeleteFile(filepath);

            if (File.Exists(filepath))
                Assert.Fail("");

        }
    }
}
