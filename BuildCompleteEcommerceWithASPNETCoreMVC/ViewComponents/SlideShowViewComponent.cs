using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.ViewComponents
{
    [ViewComponent(Name = "SlideShow")]
    public class SlideShowViewComponent : ViewComponent
    {
        private DatabaseContext db;
        public SlideShowViewComponent(DatabaseContext _db) 
        {
            this.db = _db; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<SlideShow> slideShows = db.SlideShows.Where(s => s.Status).ToList();

            return View("Index", slideShows);
        }
    }
}
