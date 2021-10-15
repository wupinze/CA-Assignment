using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace ShoppingCartWebApp.Models
{
    public class PurchasesItem
    {
        public Product product { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public List<string> ActivationCode { get; set; }

        public PurchasesItem()
        {
            this.ActivationCode = new List<string>();
        }

        //public PurchasesItem(Product product, DateTime PurchaseDate, int quantity, List<string> ActivationCode)
        //{
        //    this.product = product;
        //    this.Quantity = quantity;
        //    this.PurchaseDate = PurchaseDate;
        //    this.ActivationCode = ActivationCode;
        //}

    }
}
