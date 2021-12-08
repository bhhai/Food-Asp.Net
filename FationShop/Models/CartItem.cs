using FationShop.Areas.Admin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FationShop.Models
{
    [Serializable]
    public class CartItem
    {
        private Areas.Admin.Framework.FashionShopEntities db = new Areas.Admin.Framework.FashionShopEntities();
        public Product Product { set; get; }
        public int Quantity { set; get; }

        public void CartUpdate(int id, int _quantity)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.Quantity = _quantity;
            }
        }
    }
}