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
        private DataRepository data = new DataRepository();
        //
        // GET: /Products/

        //Shows products for a particular store
        public ActionResult Index(int id)
        {
            //if (storemodel.IsValidStore(id, data.GetCurrentUser())) return View("StoreOwnerIndex", storemodel.GetStore(id));
            var products = storemodel.GetStoreProducts(id);
            if (products.Count() == 0) return View("NoProducts");
            else return View(products);
        }

    }
}
