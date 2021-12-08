using FationShop.Areas.Admin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FationShop.Controllers
{
    public class AboutController : Controller
    {
        private FashionShopEntities db = new FashionShopEntities();
        // GET: About
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 1800)]
        public ActionResult Index()
        {
            return View(db.Abouts.Where(x => x.Status == true).First());
        }
    }
}