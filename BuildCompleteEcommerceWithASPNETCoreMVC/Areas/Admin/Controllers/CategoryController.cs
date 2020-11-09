using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public CategoryController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.categories = db.Categories.Where(c => c.Parent == null).ToList();
            //ViewBag.categories = db.Categories.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var category = new Category();
            return View("Add",category);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Category category)
        {
            category.Parent = null;
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index", "Category", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = db.Categories.FirstOrDefault(c => c.Id == id);
                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["success"] = "successfuly!!!";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
           
            return RedirectToAction("Index", "Category", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.FirstOrDefault(c => c.Id == id);           
            return View("Edit", category);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id,Category category)
        {
            var currCategory = db.Categories.FirstOrDefault(c => c.Id == id);

            currCategory.Name = category.Name;
            currCategory.Status = category.Status;

            db.SaveChanges();

            return RedirectToAction("Index", "Category", new { area = "admin" });
        }

        [HttpGet]
        [Route("addsubcategory/{id}")]
        public IActionResult AddSubCategory(int id)
        {
            var subCategory = new Category
            {
                ParentId = id
            };

            return View("AddSubCategory", subCategory);
        }

        [HttpPost]
        [Route("addsubcategory/{categoryId}")]
        public IActionResult AddSubCategory(int categoryId, Category subCategory)
        {

            db.Categories.Add(subCategory);
            db.SaveChanges();

            return RedirectToAction("Index", "Category", new { area = "admin" });
        }
    }
}
