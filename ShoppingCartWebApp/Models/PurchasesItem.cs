using System;
using System.Collections.Generic;

namespace ShoppingCartWebApp.Models
{
    public class PurchasesItem
    {
        public Product product { get; set; }
        public string PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public List<string> ActivationCode { get; set; }

        public PurchasesItem()
        {
            this.ActivationCode = new List<string>();
        }

    }
}
