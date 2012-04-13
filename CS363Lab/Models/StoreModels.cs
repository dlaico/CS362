using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS363Lab.Models
{
    public class StoreModels
    {

        private WebStoreDataDataContext webdata = new WebStoreDataDataContext();

        public bool IsValidStore(int storeid, aspnet_User user)
        {
            var store = GetStore(storeid);
            if (store.UserID == user.UserId) return true;
            else return false;
        }

        public IQueryable<Product> GetStoreProducts(int storeid)
        {
            var products = from p in webdata.Products
                           where p.StoreID == storeid
                           select p;
            return products;
        }

        public Store GetStore(int storeid)
        {
            var stores = from s in webdata.Stores
                           where s.StoreID == storeid
                           select s;
            return stores.First();
        }

        public Store UpdateStore(Store store)
        {
            Store updatedStore = GetStore(store.StoreID);
            updatedStore.StoreAddress = store.StoreAddress;
            updatedStore.StoreCity = store.StoreCity;
            updatedStore.StoreEmail = store.StoreEmail;
            updatedStore.StoreName = store.StoreName;
            updatedStore.StorePhone = store.StorePhone;
            updatedStore.StoreState = store.StoreState;
            updatedStore.StoreZip = store.StoreZip;
            webdata.SubmitChanges();
            return updatedStore;
        }
    }
}