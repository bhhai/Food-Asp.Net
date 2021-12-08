using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using Facebook;
using FationShop.Areas.Admin.Framework;

namespace FationShop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Mã xác nhận không chính xác.")] //captcha validation
        public ActionResult Register(Customer model)
        {
            if(ModelState.IsValid)
            {
                using(FashionShopEntities dbModel = new FashionShopEntities())
                {
                    if(dbModel.Customers.Any(x => x.Email == model.Email ))
                    {
                        ViewBag.ErrMsg = "Tài khoản này đã tồn tại!";
                        return View("Register", model);
                    }
                    

                    dbModel.Customers.Add(model);
                    dbModel.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.SuccessMsg = "Đăng ký thành công!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer model)
        {
            using (FashionShopEntities dbModel = new FashionShopEntities())
            {
                var userDetails = dbModel.Customers.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
                if(userDetails == null)
                {
                    ViewBag.Alert = "Tài khoản hoặc mật khẩu không chính xác, vui lòng kiểm tra lại.";
                    return View("Login", model);
                }
                else
                {
                    Session["UserID"] = userDetails.DisplayName;
                    return RedirectToAction("Index", "Home");
                }
            }
                
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                respone_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code, Customer model)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code,
            });
            var accessToken = result.access_token;
            if (!String.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                //Get the user's infomation from fb
                dynamic me = fb.Get("me?fields=first_name, middle_name, last_name, id, email");
                string email = me.email;
                string displayName = me.first_name + me.middle_name + me.last_name;

                using (FashionShopEntities dbModel = new FashionShopEntities())
                {
                    if (dbModel.Customers.Any(x => x.Email == model.Email))
                    {
                        ViewBag.ErrMsg = "Tài khoản này đã tồn tại!";
                        return View("Register", model);
                    }

                    model.Email = email;
                    model.DisplayName = displayName;

                    dbModel.Customers.Add(model);
                    dbModel.SaveChanges();
                }
            }
            return Redirect("/");
        }
        public ActionResult LogOut()
        {
            string emailUser = (string)Session["UserID"];
            ViewBag.EmailUser = emailUser;
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }
    }
}