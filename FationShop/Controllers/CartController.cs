using DemoVNPay.Others;
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

        //[HttpGet]
        //public ActionResult Payment()
        //{
        //    var cart = Session[CartSession];
        //    var list = new List<CartItem>();
        //    if (cart != null)
        //    {
        //        list = (List<CartItem>)cart;
        //    }
        //    return View(list);
        //}


        public ActionResult PaymentCod(FormCollection form)
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
            new Mail().sendMail(form["email"], "Đơn hàng mới từ Now Food", content);

            //Clear session
            Session.Remove(CartSession);
            if (Session[CartSession] == null)
            {
                ViewBag.OrderSuccess = "Đặt đơn hàng thành công ! Thông tin đơn hàng đã được gửi về email của bạn.";
            }
            return RedirectToAction("Payment", "Cart");
        }

        public ActionResult Payment(FormCollection form)
        {
            if (Session["UserID"] == null)
            {
                return Redirect("/dang-nhap");
            }
            else
            {
                if (form["1"] == "cod")
                {
                    return View();
                }
                else
                {
                    string url = ConfigurationManager.AppSettings["Url"];
                    string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
                    string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
                    string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

                    PayLib pay = new PayLib();

                    long price = Convert.ToInt32(form["price"]);

                    long newPrice = price * 100;

                    pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
                    pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                    pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                    pay.AddRequestData("vnp_Amount", newPrice.ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                    pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                    pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                    pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
                    pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                    pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
                    pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                    pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                    pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

                    string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

                    return Redirect(paymentUrl);
                }
            }
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Clear session
                        Session.Remove(CartSession);
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
    }


}