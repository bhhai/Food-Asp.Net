using FationShop.Areas.Admin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FationShop.Controllers
{
    public class ContactController : Controller
    {
        private FashionShopEntities db = new FashionShopEntities();
        // GET: Contact
        public ActionResult Index()
        {
            var contact = db.Contacts.Where(x => x.Status == true).Take(1).ToList();
            return View(contact);
        }
    }
}