using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS363Lab.Models;

namespace CS363Lab.Controllers
{
    public class StoresController : Controller
    {
        private DataRepository data = new DataRepository();
        private StoreModels storemodel = new StoreModels();
        //
        // GET: /Store/

        public ActionResult Index()
        {
            return View(storemodel.GetAllStores());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Store store)
        {
            aspnet_User user = data.GetCurrentUser();
            data.AddStore(store, user);
            return View("CreateSuccess", store);
        }

        [Authorize]
        public ActionResult Update(int id)
        {
            Store store = storemodel.GetStore(id);
            return View(store);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Update(Store store)
        {
            storemodel.UpdateStore(store);
            return View(store);
        }


        [Authorize]
        public ActionResult AddProducts(int id)
        {
            if (storemodel.IsValidStore(id, data.GetCurrentUser())) return View(new Product { StoreID = id });
            else return View("ProductAddError");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddProducts(Product product)
        {
            if (storemodel.IsValidStore(product.StoreID, data.GetCurrentUser())) {
                data.AddProduct(product);
                return View("AddProductSuccess", product);
            }
            else return View("ProductAddError");
        }

    }
}
