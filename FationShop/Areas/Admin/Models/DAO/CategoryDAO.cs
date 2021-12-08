using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace FationShop.Areas.Admin.Models.DAO
{
    public class CategoryDAO
    {
        private static Framework.FashionShopEntities db = null;
        public CategoryDAO()
        {
            db = new Framework.FashionShopEntities();
        }
    }
}