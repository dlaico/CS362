using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS363Lab.Models
{
    public class StoreModels
    {

        private WebStoreDataDataContext webdata = new WebStoreDataDataContext();

        //TODO need to implement
        public bool IsValidStore(int storeid, aspnet_User user)
        {
            if(webdata.Stores.Any( x => x.StoreID == storeid)) return true;
            else return false;
        }

        public IQueryable<Product> GetStoreProducts(int storeid)
        {
            var products = from p in webdata.Products
                           where p.StoreID == storeid
                           select p;
            return products;
        }
    }
}