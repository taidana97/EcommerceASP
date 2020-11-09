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
    [Route("admin/customer")]
    public class CustomerController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public CustomerController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.Customers = db.Accounts.ToList();
            return View();
        }
    }
}
