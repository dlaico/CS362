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
        //
        // GET: /Store/

        public ActionResult Index()
        {
            DataRepository data = new DataRepository();
            var stores = data.GetAllStores();
            return View(stores);
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
        public ActionResult AddProducts(int id)
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddProducts(Product product)
        {
            return View();
        }

    }
}
