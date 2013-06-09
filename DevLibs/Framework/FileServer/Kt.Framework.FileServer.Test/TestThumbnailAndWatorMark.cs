using System.Drawing;
using System.IO;
using Dev.FileServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev.Framework.FileServer.Test
{
    [TestClass]
    public class TestThumbnailAndWatorMark
    {
        [TestMethod]
        public void MyTestMethodThumbnail()
        {
            string basepath = @"C:\Users\Administrator\Desktop\P\";

            string mark = basepath + "mark.png";
            string imageDir = basepath + "animatedsample.gif";

            string outdir = basepath + "animatedsamplesmall.gif";
            string outdir2 = basepath + "animatedsamplesmall2.gif";
            string outdir3 = basepath + "animatedsamplesmall3.gif";
            string outdir4 = basepath + "animatedsamplesmall4.gif";

            var obj = ImageServer.ImageFile.Thumbnail(File.OpenRead(imageDir), 400, 400);

            Save(obj, outdir);

            obj = ImageServer.ImageFile.Thumbnail(File.OpenRead(imageDir), 300, 300);
            Save(obj, outdir2);

            obj = ImageServer.ImageFile.Thumbnail(File.OpenRead(imageDir), 100, 100);
            Save(obj, outdir3);
        }


        [TestMethod]
        public void MyTestMethodWatorMark()
        {
            string basepath = @"C:\Users\Administrator\Desktop\P\";
            string mark = basepath + "mark.png";
            string imageDir = basepath + "animatedsample.gif";

            string outdir = basepath + "animatedsamplesmallWatorMar.gif";
            string outdir2 = basepath + "animatedsamplesmall2WatorMar.gif";
            string outdir3 = basepath + "animatedsamplesmall3WatorMar.gif";
            string outdir4 = basepath + "animatedsamplesmall4WatorMar.gif";

            var obj = ImageServer.ImageFile.Watermark(File.OpenRead(imageDir), mark);

            Save(obj, outdir);

            obj = ImageServer.ImageFile.Thumbnail(File.OpenRead(outdir), 300, 300);
            Save(obj, outdir2);

            obj = ImageServer.ImageFile.Thumbnail(File.OpenRead(outdir), 100, 100);
            Save(obj, outdir3);
        }


        public void Save(Stream stream, string path)
        {
            var image = Image.FromStream(stream);

            image.Save(path);

        }
    }
}