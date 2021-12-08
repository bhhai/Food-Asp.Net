using FationShop.Areas.Admin.Framework;
using FationShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FationShop.Controllers
{
    public class HomeController : Controller
    {
        private FashionShopEntities db = new FashionShopEntities();
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 1800)] //lưu cache vào server
        public ActionResult Index()
        {
            ViewBag.Slides = db.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
            ViewBag.FeatureCategory = db.Categories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
            ViewBag.FeatureProduct = db.Products.Where(x => x.Status == true).OrderByDescending(x => x.Price).Take(6).ToList();
            ViewBag.NewProduct = db.Products.Where(x => x.Status == true).OrderByDescending(x => x.ID).Take(6).ToList();
            ViewBag.Blog = db.Blogs.Where(x => x.Status == true).ToList();

            return View();
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            return PartialView(db.Menus.Where(x=>x.Status == true).OrderBy(x=>x.DisplayOrder).ToList());
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session["CartSession"];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }

    }
}