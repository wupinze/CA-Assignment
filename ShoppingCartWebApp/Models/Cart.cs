using System;
using System.Collections.Generic;

namespace ShoppingCartWebApp.Models
{
    public class Cart
    {
        public Cart()
        {
            Id = new Guid();
            products = new List<Product>();
        }


        public Guid Id { get; set;}


        public virtual User user { get; set;}
        public virtual string productId { get; set;}

        public virtual ICollection<Product> products { get; set;}

    }
}
