using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FationShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
              new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Cart List",
                url: "them-gio-hang",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );
            routes.MapRoute(
                name: "Cart Index",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );
            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );
            routes.MapRoute(
                name: "Blog",
                url: "tin-tuc",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Payment",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "PaymentConfirm",
                url: "PaymentConfirm",
                defaults: new { controller = "Cart", action = "PaymentConfirm", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Blog Detail",
                url: "{tin-tuc}/{link}/{blogId}",
                defaults: new { controller = "Blog", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Product List",
                url: "{san-pham}",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Product Detail",
                url: "{san-pham}/{metatitle}-{id}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "FationShop.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] {"FationShop.Controllers"}
            );
        }
    }
}
