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

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/slideshow")]
    public class SlideShowController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        private IHostingEnvironment iHostingEnvironment;

        public SlideShowController(DatabaseContext _db, IHostingEnvironment _iHostingEnvironment)
        {
            db = _db;
            iHostingEnvironment = _iHostingEnvironment;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.SlideShows = db.SlideShows.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var slideShow = new SlideShow();
            return View("Add", slideShow);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(SlideShow slideShow, IFormFile photo)
        {
            var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + photo.FileName;
            var path = 
                Path.Combine(this.iHostingEnvironment.WebRootPath, "slideshows", fileName);

            var stream = new FileStream(path, FileMode.Create);

            photo.CopyToAsync(stream);

            slideShow.Name = fileName;
            
            db.SlideShows.Add(slideShow);
            db.SaveChanges();

            return RedirectToAction("Index", "SlideShow", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var slideShow = db.SlideShows.FirstOrDefault(c => c.Id == id);
            db.SlideShows.Remove(slideShow);
            db.SaveChanges();

            return RedirectToAction("Index", "SlideShow", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var slideShow = db.SlideShows.FirstOrDefault(s => s.Id == id);
            return View("Edit", slideShow);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, SlideShow slideShow, IFormFile photo)
        {
            var currSlideShow = db.SlideShows.FirstOrDefault(c => c.Id == slideShow.Id);

            if (photo !=null && !string.IsNullOrEmpty(photo.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + photo.FileName;
                var path =
                    Path.Combine(this.iHostingEnvironment.WebRootPath, "slideshows", fileName);

                var stream = new FileStream(path, FileMode.Create);

                photo.CopyToAsync(stream);

                slideShow.Name = fileName;

                currSlideShow.Name = fileName;
            }

            currSlideShow.Status = slideShow.Status;
            currSlideShow.Title = slideShow.Title; 
            currSlideShow.Description = slideShow.Description; 

            db.SaveChanges();

            return RedirectToAction("Index", "SlideShow", new { area = "admin" });
        }
    }
}
