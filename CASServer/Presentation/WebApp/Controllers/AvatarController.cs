using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Dto;
using Application.MainBoundedContext.UserModule;
using Dev.Comm;
using Dev.Comm.Web;
using Dev.Comm.Web.Mvc.Filter;
using Dev.Framework.FileServer;
 
using WebMatrix.WebData;

namespace CASServer.Controllers
{
    [ActionAllowCrossSiteJson]
    public class AvatarController : Controller
    {
        private readonly IImageFile _imagefile;
        private readonly IUserService _userService;

        public AvatarController(IImageFile imagefile, IUserService userService)
        {
            _imagefile = imagefile;
            _userService = userService;
        }

        #region Flash Avtar

        public ActionResult FlashIndex()
        {
            return View();
        }

        [AllowAnonymous]
        [JsonpFilter]
        public ActionResult FlashJson()
        {
            string content = string.Empty;
            content = ViewEngine("FlashIndex", null);
            return Json(content, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult FlashFaceUpload()
        {
            string uid = WebSecurity.CurrentUserId.ToString();
            if (DevRequest.GetString("Filename") != "" && DevRequest.GetString("Upload") != "")
            {
                //string uid = DecodeUid(DevRequest.GetString("input")).Split(',')[0];
                return Content(UploadTempAvatar(uid));
            }
            if (DevRequest.GetString("avatar1") != "" && DevRequest.GetString("avatar2") != "" &&
                DevRequest.GetString("avatar3") != "")
            {
                //string uid = DecodeUid(DevRequest.GetString("input")).Split(',')[0];
                CreateDir(uid);
                if (!(SaveAvatar("avatar1", uid) /* && SaveAvatar("avatar2", uid) &&SaveAvatar("avatar3", uid)*/))
                {
                    //File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                    return Content("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
                }
                //File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                return Content("<?xml version=\"1.0\" ?><root><face success=\"1\"/></root>");
            }

            return Content("");
        }


        private void CreateDir(string uid)
        {
            string avatarDir = string.Format("/images/upload/avatars/{0}",
                                             uid);
            if (!Directory.Exists(Server.MapPath(avatarDir)))
                Directory.CreateDirectory(Server.MapPath(avatarDir));
        }

        #region

        private string UploadTempAvatar(string uid)
        {
            string filename = uid + ".jpg";

            string root = HttpServerInfo.BaseUrl;

            string uploadUrl = root + "/images/upload/avatars";
            string uploadDir = Server.MapPath("/images/upload/avatars");
            if (!Directory.Exists(uploadDir + "/temp"))
                Directory.CreateDirectory(uploadDir + "/temp");

            filename = "/temp/" + filename;
            if (Request.Files.Count > 0)
            {
                Request.Files[0].SaveAs(uploadDir + filename);
            }

            string serverfile = HttpServerInfo.BaseUrl + "/avatarImage/temp/" + filename;

            return uploadUrl + filename;
        }

        private byte[] FlashDataDecode(string s)
        {
            var r = new byte[s.Length/2];
            int l = s.Length;
            for (int i = 0; i < l; i = i + 2)
            {
                int k1 = (s[i]) - 48;
                k1 -= k1 > 9 ? 7 : 0;
                int k2 = (s[i + 1]) - 48;
                k2 -= k2 > 9 ? 7 : 0;
                r[i/2] = (byte) (k1 << 4 | k2);
            }
            return r;
        }

        private bool SaveAvatar(string avatar, string uid)
        {
            byte[] b = FlashDataDecode(Request[avatar]);
            //if (b.Length == 0)
            //    return false;
            //string size = "";
            //if (avatar == "avatar1")
            //    size = "large";
            //else if (avatar == "avatar2")
            //    size = "medium";
            //else
            //    size = "small";


            //string avatarFileName = string.Format("/images/upload/avatars/{0}/{1}.jpg",
            //    uid, size);
            //FileStream fs = new FileStream(Server.MapPath(avatarFileName), FileMode.Create);
            //fs.Write(b, 0, b.Length);
            //fs.Close();

            string key = "";

           
            key = _imagefile.SaveImageFile(b, "headFileName", new[]
                                                                  {
                                                                      new ImagesSize
                                                                          {
                                                                              Height = 180,
                                                                              Width = 180
                                                                          },
                                                                      new ImagesSize
                                                                          {
                                                                              Height = 75,
                                                                              Width = 75
                                                                          }, new ImagesSize
                                                                                 {
                                                                                     Height = 50,
                                                                                     Width = 50
                                                                                 },
                                                                      new ImagesSize
                                                                          {
                                                                              Height = 25,
                                                                              Width = 25
                                                                          },
                                                                  });

            //    cutedstream.Close();
            //}


            _userService.UpdateUserAvatar(WebSecurity.CurrentUserId, key);

            return true;
        }

        #endregion

        #endregion

        [Authorize]
        public ActionResult Test()
        {
            return View();
        }


        public ActionResult Js(string Id)
        {
            return JavaScript("show('" + Id + "')");
        }

        #region Html Avatar

        //
        // GET: /Avatar/

        public ActionResult Index()
        {
            return View();
        }


        [JsonpFilter]
        public ActionResult Json()
        {
            string content = string.Empty;
            content = ViewEngine("index");
            return Json(content, JsonRequestBehavior.AllowGet);
        }


        private string ViewEngine(string viewName, string layout = "_Layout4Js")
        {
            string content;
            ViewEngineResult view = null;
            if (!string.IsNullOrEmpty(layout))
                view = ViewEngines.Engines.FindView(ControllerContext, viewName, layout);
            else
                view = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
            using (var writer = new StringWriter())
            {
                var context = new ViewContext(ControllerContext, view.View, ViewData, TempData, writer);
                view.View.Render(context, writer);

                writer.Flush();
                content = writer.ToString();
            }
            return content;
        }

        //[AllowCrossSiteJson]
        [ActionAllowCrossSiteJson]
        //[JsonpFilter]
        [Authorize]
        [HttpPost]
        public ActionResult UploadHead(HttpPostedFileBase head) //命名和上传控件name 一样
        {
            BaseState state = null;
            try
            {
                if ((head == null))
                {
                    state = (new BaseState(-1, "无上传文件"));
                }
                else
                {
                    var supportedTypes = new[] {"jpg", "jpeg", "png", "gif", "bmp"};
                    string fileExt = Path.GetExtension(head.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        state = (new BaseState(-1, "文件类型不正确"));
                    }
                    else if (head.ContentLength > 1024*1000*10)
                    {
                        state = (new BaseState(-2, "文件太大"));
                    }
                    else
                    {
                        var r = new Random();
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + "." + fileExt;
                        string filepath = Path.Combine(Server.MapPath("~/avatarImage/temp"), filename);
                        head.SaveAs(filepath);

                        string serverfile = HttpServerInfo.BaseUrl + "/avatarImage/temp/" + filename;

                        state = new BaseState(0, serverfile);
                    }
                }
                string jsonstr = JsonConvert.ToJsonStr(state);
                string script =
                    string.Format(
                        "<script type='text/javascript'> if( top.fileuploadcallback ){{ top.fileuploadcallback({0});}}else{{window.alert('不在的图片回调方法');}}</script>",
                        jsonstr);
                return Content(script);
            }
            catch (Exception)
            {
                throw;
                return Json(new {msg = -3});
            }
        }


        //[HttpPost]
        [ValidateInput(false)]
        [JsonpFilter]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveHead(int x, int y, int width, int height, string headFileName)
        {
            if (!WebSecurity.IsAuthenticated)
            {
                return Content(ModifiyScript(new BaseState(-1, "用户还未登录")));
            }

            var model = new UploadImageModel();
            model.headFileName = Request["headFileName"];
            model.x = Convert.ToInt32(Request["x"]);
            model.y = Convert.ToInt32(Request["y"]);
            model.width = Convert.ToInt32(Request["width"]);
            model.height = Convert.ToInt32(Request["height"]);

            string filepath = Path.Combine(Server.MapPath("~/avatarImage/temp"), model.headFileName);
            string fileExt = Path.GetExtension(filepath);

            string key = "";

            using (MemoryStream cutedstream = CutAvatar(filepath, model.x, model.y, model.width, model.height, 75L, 180)
                )
            {
                key = _imagefile.SaveImageFile(cutedstream, model.headFileName, new[]
                                                                                    {
                                                                                        new ImagesSize
                                                                                            {
                                                                                                Height = 180,
                                                                                                Width = 180
                                                                                            },
                                                                                        new ImagesSize
                                                                                            {
                                                                                                Height = 75,
                                                                                                Width = 75
                                                                                            }, new ImagesSize
                                                                                                   {
                                                                                                       Height = 50,
                                                                                                       Width = 50
                                                                                                   },
                                                                                        new ImagesSize
                                                                                            {
                                                                                                Height = 25,
                                                                                                Width = 25
                                                                                            },
                                                                                    });

                cutedstream.Close();
            }

            _userService.UpdateUserAvatar(WebSecurity.CurrentUserId, key);


            //Dev.Comm.FileUtil.DeleteFile(filepath);

            var state = new BaseState(0, key);

            string script = ModifiyScript(state);
            return Content(script);
            return Json(new BaseState(0, key), JsonRequestBehavior.AllowGet);
        }

        private static string ModifiyScript(BaseState state)
        {
            string jsonstr = JsonConvert.ToJsonStr(state);
            string script =
                string.Format(
                    "<script type='text/javascript'> if( top.fileupladmodifycallback ){{ top.fileupladmodifycallback({0});}}else{{window.alert('不在的图片回调方法');}}</script>",
                    jsonstr);
            return script;
        }


        [JsonpFilter]
        public ActionResult CurrentUserAvataUrl(int type = 4)
        {
            if (!WebSecurity.IsAuthenticated)
                throw new Exception("未登录的操作");
            int userid = WebSecurity.CurrentUserId;
            string key = _userService.GetUserAvatar(userid);
            int size = GetSize(type);
            string url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = GetDefaultFace(type);
            }
            else
                url = _imagefile.GetImageUrl(key, size, size);

            return Json(url, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AvataUrlByUserid(int userid, int type = 4)
        {
            string key = _userService.GetUserAvatar(userid);
            int size = GetSize(type);
            string url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = GetDefaultFace(type);
            }
            else
                url = _imagefile.GetImageUrl(key, size, size);

            return Redirect(url);
        }

        [ActionAllowCrossSiteJson]
        [JsonpFilter]
        public ActionResult GetAvataUrlByUserid(int userid, int type = 4)
        {
            string key = _userService.GetUserAvatar(userid);
            int size = GetSize(type);
            string url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = GetDefaultFace(type);
            }
            else
                url = _imagefile.GetImageUrl(key, size, size);

            return Json(url);
        }


        [ActionAllowCrossSiteJson]
        [JsonpFilter]
        public ActionResult GetAvataUrlByUid(int uid, int type = 4)
        {
            int size = GetSize(type);

            string key = _userService.GetUserAvatarByUid(uid);
            string url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = GetDefaultFace(type);
            }
            else
                url = _imagefile.GetImageUrl(key, size, size);

            return Json(url);
        }


        public ActionResult AvataUrl(decimal uid, int type = 4)
        {
            int size = GetSize(type);

            string key = _userService.GetUserAvatarByUid(uid);
            string url = "";
            if (string.IsNullOrEmpty(key))
            {
                url = GetDefaultFace(type);
            }
            else
                url = _imagefile.GetImageUrl(key, size, size);

            return Redirect(url);
        }


        private string GetDefaultFace(int type)
        {
            string url = HttpServerInfo.BaseUrl;
            switch (type)
            {
                case 4:
                    url += "/avatarImage/180/default.gif";
                    break;
                case 3:
                    url += "/avatarImage/75/default.gif";

                    break;
                case 2:

                    url += "/avatarImage/50/default.gif";

                    break;
                case 1:

                    url += "/avatarImage/25/default.gif";

                    break;


                default:
                    throw new Exception("Error type");
            }
            return url;
        }

        private static int GetSize(int type)
        {
            int size;
            switch (type)
            {
                case 4:
                    size = 180;
                    break;
                case 3:
                    size = 75;
                    break;
                case 2:
                    size = 50;
                    break;
                case 1:
                    size = 25;
                    break;


                default:
                    throw new Exception("Error type");
            }
            return size;
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        private MemoryStream CutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, int t)
        {
            Image original = Image.FromFile(imgSrc);

            var img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (Graphics gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) ||
                    original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            Encoder myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var stream = new MemoryStream();
            img.Save(stream, myImageCodecInfo, myEncoderParameters);

            return stream;
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        private MemoryStream CutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, string SavePath,
                                       int t)
        {
            Image original = Image.FromFile(imgSrc);

            var img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (Graphics gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) ||
                    original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            Encoder myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var stream = new MemoryStream();
            img.Save(stream, myImageCodecInfo, myEncoderParameters);
            img.Dispose();
            return stream;
        }

        //根据长宽自适应 按原图比例缩放 
        private static Size GetThumbnailSize(Image original, int desiredWidth, int desiredHeight)
        {
            double widthScale = (double) desiredWidth/original.Width;
            double heightScale = (double) desiredHeight/original.Height;
            double scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
                       {
                           Width = (int) (scale*original.Width),
                           Height = (int) (scale*original.Height)
                       };
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        #endregion
    }

    public class UploadImageModel
    {
        public string headFileName { get; set; }

        public int x { get; set; }


        public int y { get; set; }


        public int width { get; set; }


        public int height { get; set; }
    }
}