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
            return true;
        }
    }
}