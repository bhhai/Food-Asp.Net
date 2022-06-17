using FationShop.Areas.Admin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace FationShop.Controllers
{
    public class ProductController : Controller
    {
        private FashionShopEntities db = new FashionShopEntities();
        // GET: Product
        public ActionResult Index(string sortOrder, string searchString, int? page, int pageLimit = 6)
        {
            ViewBag.PriceSort = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.searchString = searchString;

            var products = db.Products.AsQueryable();

            switch(sortOrder)
            {
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
            }

            //db.Products.Where(x => x.Status == true && x.Name.Contains(searchString) || searchString == null).ToList().ToPagedList(page ?? 1, pageLimit)
            return View(db.Products.Where(x => x.Status == true && x.Name.Contains(searchString) || searchString == null).ToList().ToPagedList(page ?? 1, pageLimit));
        }
        public ActionResult Detail(int id)
        {
            var product = db.Products.Find(id);
            ViewBag.RelatedProduct = db.Products.Where(x => x.ID != id && x.CategoryID == product.CategoryID).Take(4).ToList();
            return View(product);
        }

    }
}