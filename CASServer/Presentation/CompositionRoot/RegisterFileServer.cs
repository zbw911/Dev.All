using Dev.Framework.FileServer;
using Dev.Framework.FileServer.Config;
using Dev.Framework.FileServer.DocFile;
using Dev.Framework.FileServer.ImageFile;
using Dev.Framework.FileServer.ShareImpl;
using Dev.Web.CompositionRootBase;
 
using Ninject;

namespace CompositionRoot
{
    class RegisterFileServer : IRegister
    {
        public void Register()
        {


            ReadConfig x = new ReadConfig();

            //公用方法 
            this.Kernel.Bind<IKey>().To<ShareFileKey>();
            this.Kernel.Bind<IUploadFile>().To<ShareUploadFile>();

            //文档类型
            this.Kernel.Bind<IDocFile>().To<DocFileUploader>();
            //图片类型
            this.Kernel.Bind<IImageFile>().To<ImageUploader>();
        }

        public IKernel Kernel { get; set; }
    }
}
