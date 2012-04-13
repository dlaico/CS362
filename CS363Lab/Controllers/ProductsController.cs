using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS363Lab.Models;

namespace CS363Lab.Controllers
{
    public class ProductsController : Controller
    {
        private StoreModels storemodel = new StoreModels();
        private ProductModel productmodel = new ProductModel();
        private DataRepository data = new DataRepository();
        //
        // GET: /Products/

        //Shows products for a particular store
        public ActionResult Index(int id)
        {
            if (data.GetCurrentUser() != null)
                if (storemodel.IsValidStore(id, data.GetCurrentUser())) 
                    return View("StoreOwnerIndex", new StoreViewModel { products = storemodel.GetStoreProducts(id), store = storemodel.GetStore(id) });
            var products = storemodel.GetStoreProducts(id);
            if (products.Count() == 0) return View("NoProducts");
            else return View(products);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            if (productmodel.IsProductOwner(id, data.GetCurrentUser()))
            {
                var product = productmodel.GetProduct(id);
                return View(product);
            }
            else return View("ProductAddError");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (productmodel.IsProductOwner(product.ProductID, data.GetCurrentUser()))
            {
                return View(productmodel.UpdateProduct(product));
            }
            else return View("ProductAddError");
        }
    }
}
