using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.ViewComponents
{
    [ViewComponent(Name = "LatestProducts")]
    public class LatestProductsViewComponent : ViewComponent
    {
        private DatabaseContext db;
        public LatestProductsViewComponent(DatabaseContext _db) 
        {
            this.db = _db; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = db.Products.OrderByDescending(p => p.Id)
                .Where(p => p.Status).Take(6).ToList();

            return View("Index", products);
        }
    }
}
