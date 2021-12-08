using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FationShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.DTO.UserModel user)
        {
            if (ModelState.IsValid)
            {
                bool isLogin = Models.DAO.AccountDAO.checkLogin(user.Username, user.Password);
                if (isLogin)
                {
                    var url = Request.QueryString["ReturnUrl"];
                    Session["Username"] = user.Username;
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    if(url != null)
                    {
                        return Redirect(url);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
                }
            }
            return View();
        }
    }
}