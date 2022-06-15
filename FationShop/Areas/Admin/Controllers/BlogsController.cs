using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FationShop.Areas.Admin.Framework;
using PagedList;

namespace FationShop.Areas.Admin.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private FashionShopEntities db = new FashionShopEntities();

        // GET: Admin/Blogs
        public ActionResult Index(string searchString, int? page, int pageLimit = 10)
        {
            //Lưu search string vào viewbag
            ViewBag.searchString = searchString;
            return View(db.Blogs.Where(x => x.Name.StartsWith(searchString) || searchString == null).ToList().ToPagedList(page ?? 1, pageLimit));
        }

        // GET: Admin/Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog)
        {
            string fileName = Path.GetFileNameWithoutExtension(blog.ImageFile1.FileName);
            string extension = Path.GetExtension(blog.ImageFile1.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            blog.Avartar = "/Areas/Admin/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Areas/Admin/Image/"), fileName);
            blog.ImageFile1.SaveAs(fileName);


            db.Blogs.Add(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {

                string fileName = Path.GetFileNameWithoutExtension(blog.ImageFile1.FileName);
                string extension = Path.GetExtension(blog.ImageFile1.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                blog.Avartar = "/Areas/Admin/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Areas/Admin/Image/"), fileName);
                blog.ImageFile1.SaveAs(fileName);

                //string fileName2 = Path.GetFileNameWithoutExtension(blog.ImageFile2.FileName);
                //string extension2 = Path.GetExtension(blog.ImageFile2.FileName);
                //fileName2 = fileName2 + DateTime.Now.ToString("yymmssfff") + extension2;
                //blog.Images = "/Areas/Admin/Image/" + fileName2;
                //fileName2 = Path.Combine(Server.MapPath("~/Areas/Admin/Image/"), fileName2);
                //if (fileName2.Length > 0)
                //{
                //    blog.ImageFile2.SaveAs(fileName2);
                //}

                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
