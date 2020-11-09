using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/photo")]
    public class PhotoController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        private IHostingEnvironment iHostingEnvironment;

        public PhotoController(DatabaseContext _db, IHostingEnvironment _iHostingEnvironment)
        {
            db = _db;
            iHostingEnvironment = _iHostingEnvironment;
        }

        [Route("index/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.Product = db.Products.FirstOrDefault(p => p.Id == id);
            ViewBag.Photos = db.Photos.Where(p => p.ProductId == id).ToList();

            return View();
        }

        [HttpGet]
        [Route("add/{id}")]
        public IActionResult Add(int id)
        {
            ViewBag.Product = db.Products.FirstOrDefault(p => p.Id == id);

            var photo = new Photo
            {
                ProductId = id
            };

            return View("Add",photo);
        }

        [HttpPost]
        [Route("add/{productId}")]
        public IActionResult Add(int productId, Photo photo, IFormFile formFile)
        {
            var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + formFile.FileName;
            var path =
                Path.Combine(this.iHostingEnvironment.WebRootPath, "product", fileName);

            var stream = new FileStream(path, FileMode.Create);

            formFile.CopyToAsync(stream);

            photo.Name = fileName;

            db.Photos.Add(photo);
            db.SaveChanges();

            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }

        [HttpGet]
        [Route("delete/{id}/{productId}")]
        public IActionResult Delete(int id,int productId)
        {
            try
            {
                var photo = db.Photos.FirstOrDefault(p => p.Id == id);

                if (photo.Name != "no-image.jpg")
                {
                    var path = Path.Combine(this.iHostingEnvironment.WebRootPath, "product", photo.Name);

                    if (!System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                db.Photos.Remove(photo);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }          

            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }

        [HttpGet]
        [Route("edit/{id}/{productId}")]
        public IActionResult Edit(int id, int productId)
        {
            ViewBag.Product = db.Products.FirstOrDefault(p => p.Id == productId);
            var photo = db.Photos.FirstOrDefault(p => p.Id == id);
            return View("Edit", photo);
        }

        [HttpPost]
        [Route("edit/{id}/{productId}")]
        public IActionResult Edit(int id, int productId, Photo photo, IFormFile formFile)
        {
            try
            {
                var currPhoto = db.Photos.FirstOrDefault(p => p.Id == photo.Id);
                var currNamePhoto = currPhoto.Name;

                if (formFile != null && !string.IsNullOrEmpty(formFile.FileName))
                {
                    if (currNamePhoto != "no-image.jpg")
                    {
                        var pathDelete = Path.Combine(this.iHostingEnvironment.WebRootPath, "product", currNamePhoto);

                        if (!System.IO.File.Exists(pathDelete))
                        {
                            System.IO.File.Delete(pathDelete);
                        }
                    }

                    var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + formFile.FileName;

                    var pathUpdate =
                        Path.Combine(this.iHostingEnvironment.WebRootPath, "product", fileName);

                    var stream = new FileStream(pathUpdate, FileMode.Create);

                    formFile.CopyToAsync(stream);

                    currPhoto.Name = fileName;
                }

                currPhoto.Status = photo.Status;

                db.SaveChanges();

                TempData["success"] = "Successfull!!!";

                return RedirectToAction("index", "photo", new { area = "admin", id = productId });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;

                return RedirectToAction("edit", "photo", new { area = "admin", id = id, productId = productId    });
            }            
        }

        [HttpGet]
        [Route("SetFeatured/{id}/{productId}")]
        public IActionResult SetFeatured(int id, int productId)
        {
            var product = db.Products.SingleOrDefault(p => p.Id == productId);
            product.Photos.ToList().ForEach(p =>
            {
                p.Featured = false;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            });

            var photo = db.Photos.FirstOrDefault(p => p.Id == id);
            photo.Featured = true;
            db.SaveChanges();

            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }
    }
}
