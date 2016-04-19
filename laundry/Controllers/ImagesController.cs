using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using laundry.ViewModels;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Drawing2D;
using laundry.Models.DB;
using laundry.Security;

namespace laundry.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ImagesController : Controller
    {
       
        private LundryDbContext db = new LundryDbContext();
        // GET: Images
        public ActionResult Index()
        {
            return View(db.tbl_images.ToList());
        }

        public ActionResult Create()
        {
           return View();
        }


        //TODO:Reduce the image sizes
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }


        //TODO: upload images to database and into site upload directory
        [HttpPost]
        public ActionResult BatchUpload()
        {
            bool isSavedSuccessfully = true;
            int count = 0;
            string msg = "";

            string fileName = "";
            string fileExtension = "";
            string filePath = "";
            string fileNewName = "";

            //  here is obtain strong  
            //int albumId = string.IsNullOrEmpty(Request.Params["hidAlbumId"])  
            //    0 : int.Parse(Request.Params["hidAlbumId"]);

            tbl_images ItmImg = new tbl_images();
            try
            {
                string directoryPath = Server.MapPath("~/uploads/images");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                foreach (string f in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[f];

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = file.FileName;
                        fileExtension = Path.GetExtension(fileName);
                        fileNewName = Guid.NewGuid().ToString() + fileExtension;
                        filePath = Path.Combine(directoryPath, fileNewName);
                        file.SaveAs(filePath);

                        Stream strm = file.InputStream;
                        string path_Thumb = System.IO.Path.Combine(Server.MapPath("~/uploads/Thumbs"), fileNewName);
                        ItmImg.imgL = "/uploads/Images/" + fileNewName; //path to large images
                        ItmImg.imgS = "/uploads/Thumbs/"+ fileNewName; // path to thumbnail images
                        GenerateThumbnails(0.5, strm, path_Thumb); //here reducing the image by 50%

                        db.tbl_images.Add(ItmImg);
                        db.SaveChanges();
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isSavedSuccessfully = false;
            }

            return Json(new
            {
                Result = isSavedSuccessfully,
                Count = count,
                Message = msg
            });
        }

        public JsonResult getItmImages()
        {
            var getImgLst = db.tbl_images.Select(x => x.imgL);
            return Json(new {getImgLst}, JsonRequestBehavior.AllowGet);
        }
    }
}