using FationShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace FationShop.Controllers
{
    public class CartController : Controller
    {
        private Areas.Admin.Framework.FashionShopEntities db = new Areas.Admin.Framework.FashionShopEntities();
        private string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult Update(FormCollection form)
        {
            try
            {
                string[] quantity = form.GetValues("quantity");
                var list = (List<CartItem>)Session[CartSession];
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Quantity = Convert.ToInt32(quantity[i]);
                }
                Session[CartSession] = list;
            }
            catch
            {

            }
            return Redirect("/gio-hang");
        }

        public ActionResult Delete(int productID)
        {
            var cart = (List<CartItem>)Session[CartSession];

            foreach (var item in cart)
            {
                if (item.Product.ID == productID)
                {
                    cart.Remove(item);
                    break;
                }
            }

            Session[CartSession] = cart;
            return Redirect("Index");
        }
        public ActionResult AddItem(int productID, int quantity)
        {
            if(Session["UserID"] == null)
            {
                return Redirect("/dang-nhap");
            }
            var product = db.Products.Find(productID);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //Tạo mới giỏ hàng
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gan vao session
                Session[CartSession] = list;
            }
            else
            {
                //Tạo mới giỏ hàng
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);

                //Lưu list item vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(FormCollection form)
        {
            if (Session[CartSession] != null)
            {
                var listCart = (List<CartItem>)Session[CartSession];
                Areas.Admin.Framework.Order order = new Areas.Admin.Framework.Order()
                {
                    CreatedDate = DateTime.Now,
                    ShipName = form["shipName"],
                    ShipAddress = form["address"],
                    ShipEmail = form["email"],
                    ShipPhoneNumber = form["phoneNumber"]
                };
                db.Orders.Add(order);
                db.SaveChanges();
                var total = 0;
                foreach (var cart in listCart)
                {
                    Areas.Admin.Framework.OrderDetail orderDetail = new Areas.Admin.Framework.OrderDetail()
                    {
                        OrderID = order.ID,
                        ProductID = cart.Product.ID,
                        Quantity = cart.Quantity,
                        Price = cart.Product.Price,
                    };
                    var money = orderDetail.Price * orderDetail.Quantity;
                    total += (int)money;
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/newOrder.html"));
                content = content.Replace("{{CustomerName}}", form["shipName"]);
                content = content.Replace("{{Address}}", form["address"]);
                content = content.Replace("{{Email}}", form["email"]);
                content = content.Replace("{{Phone}}", form["phoneNumber"]);
                content = content.Replace("{{Time}}", order.CreatedDate.Value.ToString("dd/MM/yyyy"));
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new Mail().sendMail(form["email"], "Đơn hàng mới từ TMT Fashion", content);

                //Clear session
                Session.Remove(CartSession);
                if(Session[CartSession] == null)
                {
                    ViewBag.OrderSuccess = "Đặt đơn hàng thành công ! Thông tin đơn hàng đã được gửi về email của bạn.";
                }
            }
            else
            {

            }


            return View();
        }
    }


}