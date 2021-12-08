using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FationShop.Areas.Admin.Models.DAO
{
    public class AccountDAO
    {
        private static Framework.FashionShopEntities db = new Framework.FashionShopEntities();
        public static bool checkLogin(string Username, string Password)
        {
            int count = db.Accounts.Count(x => x.Username == Username && x.Password == Password);
            return count == 1;
        }
    }
}