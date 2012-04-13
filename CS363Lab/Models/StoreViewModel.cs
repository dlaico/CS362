using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS363Lab.Models
{
    public class StoreViewModel
    {
        public Store store { get; set; }
        public IQueryable<Product> products { get; set; }
    }
}