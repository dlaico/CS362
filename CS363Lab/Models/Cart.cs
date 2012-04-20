using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS363Lab.Models
{
    public class Cart
    {
        private List<CartLine> cartLines = new List<CartLine>();
        public IList<CartLine> lines { get { return cartLines.AsReadOnly(); } }
        public double total { get; private set; }

        public void AddToCart(Product product, int quantity)
        {
            var line = cartLines.FirstOrDefault(x => x.product.ProductID == product.ProductID);
            if (line == null) cartLines.Add(new CartLine { product = product, quantity = quantity });
            else line.quantity = (line.quantity + quantity) > product.ProductQuantity ? line.quantity + quantity : product.ProductQuantity;
        }

        public void Empty()
        {
            cartLines.Clear();
        }

        public bool isEmpty() { return cartLines.Count() == 0; }

        public void RemoveLine(Product product)
        {
            cartLines.RemoveAll(x => x.product.ProductID == product.ProductID);
        }
    }

    public class CartLine
    {
        public Product product { get; set; }
        public int quantity { get; set; }
        public ProductPromotion promotion { get; set; }
        public int subtotal { get; set; }
    }
}