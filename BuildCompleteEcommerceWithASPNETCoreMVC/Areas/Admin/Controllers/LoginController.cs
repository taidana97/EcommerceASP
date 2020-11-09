using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using BuildCompleteEcommerceWithASPNETCoreMVC.Security;
using Microsoft.AspNetCore.Mvc;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller    
    {
        private DatabaseContext db = new DatabaseContext();
        private SecurityManager securityManager = new SecurityManager();

        public LoginController(DatabaseContext _db)
        {
            db = _db;
        }
        
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process(string username, string password)
        {
            var account = processLogin(username, password);

            if (account!=null)
            {
                securityManager.SignIn(this.HttpContext, account);
                return RedirectToAction("Index","dashboard",new { area = "admin" });
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }            
        }

        private Account processLogin(string username, string password)
        {
            var account = db.Accounts.SingleOrDefault(a => a.Username.Equals(username));
        
            if (account!=null)
            {
                var role = account.RoleAccounts.FirstOrDefault();

                if (role.RoleId == 1 && BCrypt.Net.BCrypt.Verify(password,account.Password))
                {
                    return account;
                }
            }
            return null;
        }

        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext);
            return RedirectToAction("index","login", new { area = "admin" });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var username = user.Value;
            var account = db.Accounts.SingleOrDefault(a => a.Username.Equals(username));
            
            return View("Profile",account);
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var currAccount = db.Accounts.SingleOrDefault(a =>
                a.Id==account.Id
            );

            if (!string.IsNullOrEmpty(account.Password))
            {
                currAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            currAccount.Username = account.Username;
            currAccount.Email = account.Email;
            currAccount.FullName = account.FullName;
            currAccount.Status = account.Status;
            db.SaveChanges();
            ViewBag.msg = "Done";
            return View("Profile");
        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {

            return View("AccessDenied");
        }
    }
}
