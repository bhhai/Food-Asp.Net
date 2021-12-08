using FationShop.Areas.Admin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FationShop.Controllers
{
    public class BlogController : Controller
    {
        private FashionShopEntities db = new FashionShopEntities();
        // GET: Blog
        [OutputCache(Location = OutputCacheLocation.Server, Duration = 1800)]
        public ActionResult Index()
        {
            return View(db.Blogs.Where(x => x.Status == true).ToList());
        }
        [OutputCache(Duration = 1800, VaryByParam = "blogId")]
        public ActionResult Detail(int blogId)
        {
            var blog = db.Blogs.Find(blogId);
            ViewBag.RelatedBlogs = db.Blogs.Where(x => x.ID != blogId).Take(2).ToList();
            return View(blog);
        }
    }
}