using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Models.ViewModels;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ProductController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            
            ViewBag.Products = db.Products.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var productViewModel = new ProductViewModel();

            productViewModel.Product = new Product();
            productViewModel.Categories = new List<SelectListItem>();

            var categories = db.Categories.ToList();
            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };

                if (category.InverseParents != null && category.InverseParents.Count > 0)
                {
                    foreach (var subCategory in category.InverseParents)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Text = subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Group = group
                        };

                        productViewModel.Categories.Add(selectListItem);
                    }
                }
                
            }

            return View("Add", productViewModel);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(ProductViewModel productViewModel)
        {
            db.Products.Add(productViewModel.Product);          

            db.SaveChanges();

            var defaultPhoto = new Photo
            {
                Name = "no-image.jpg",
                Status = true,
                ProductId = productViewModel.Product.Id,
                Featured = true,
            };
            db.Photos.Add(defaultPhoto);
            db.SaveChanges();

            return RedirectToAction("index", "product", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = db.Products.FirstOrDefault(c => c.Id == id);

                db.Photos.Remove(product.Photos.SingleOrDefault(p=>p.ProductId==id));
                db.SaveChanges();

                db.Products.Remove(product);
                db.SaveChanges();
                TempData["success"] = "Successfuly!!!";
            }
            catch (Exception ex)
            {   
                TempData["error"] = ex.Message;
            }            

            return RedirectToAction("index", "product", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var productViewModel = new ProductViewModel();

            productViewModel.Product = db.Products.FirstOrDefault(p => p.Id == id);
            productViewModel.Categories = new List<SelectListItem>();

            var categories = db.Categories.ToList();
            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };

                if (category.InverseParents != null && category.InverseParents.Count > 0)
                {
                    foreach (var subCategory in category.InverseParents)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Text = subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Group = group
                        };

                        productViewModel.Categories.Add(selectListItem);
                    }
                }

            }

            return View("Edit", productViewModel);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, ProductViewModel productViewModel)
        {
            //var currProduct = db.Products.SingleOrDefault(p => p.Id == id);

            //currProduct.Name = productViewModel.Product.Name;
            //currProduct.Status = productViewModel.Product.Status;
            //currProduct.Description = productViewModel.Product.Description;
            //currProduct.Details = productViewModel.Product.Details;
            //currProduct.Price = productViewModel.Product.Price;
            //currProduct.Quantity = productViewModel.Product.Quantity;
            //currProduct.CategoryId = productViewModel.Product.CategoryId;            
            //currProduct.Featured = productViewModel.Product.Featured;    
            
            var product = (Product)productViewModel.Product;
            db.Entry(product).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("index", "product", new { area = "admin" });
        }
    }
}
