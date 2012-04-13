using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CS363Lab.Models
{
    public class DataRepository
    {
        private WebStoreDataDataContext webstoredata = new WebStoreDataDataContext();

        public aspnet_User GetCurrentUser()
        {
            try
            {
                MembershipUser currentUser = Membership.GetUser();
                var user = from users in webstoredata.aspnet_Users
                           where users.UserName.Equals(currentUser.UserName)
                           select users;
                return (aspnet_User)user.First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int AddStore(Store store, aspnet_User user)
        {
            store.UserID = user.UserId;
            webstoredata.Stores.InsertOnSubmit(store);
            webstoredata.SubmitChanges();
            return store.StoreID;
        }

        public int AddProduct(Product product)
        {
            webstoredata.Products.InsertOnSubmit(product);
            webstoredata.SubmitChanges();
            return product.ProductID;
        }

        public IQueryable<Store> GetAllStores()
        {
            var store = from data in webstoredata.Stores
                    select data;
            return store;
        }

        public IQueryable<Product> GetProductsByStore(int storeid)
        {
            var product = from data in webstoredata.Products
                          where data.ProductID == storeid
                          select data;
            return product;
        }
    }
}