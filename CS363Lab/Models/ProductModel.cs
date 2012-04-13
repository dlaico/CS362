using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS363Lab.Models
{
    public class ProductModel
    {
        private WebStoreDataDataContext webdata = new WebStoreDataDataContext();

        public Product GetProduct(int productid)
        {
            var product = from p in webdata.Products
                           where p.ProductID == productid
                           select p;
            return product.First();
        }

        public bool IsProductOwner(int productid, aspnet_User user)
        {
            var product = GetProduct(productid);
            if (product.Store.UserID == user.UserId) return true;
            else return false;
        }

        public Product UpdateProduct(Product product)
        {
            var updateProduct = GetProduct(product.ProductID);
            updateProduct.ProductDescription = product.ProductDescription;
            updateProduct.ProductName = product.ProductName;
            updateProduct.ProductPrice = product.ProductPrice;
            updateProduct.ProductQuantity = product.ProductQuantity;
            webdata.SubmitChanges();
            return updateProduct;
        }
    }
}