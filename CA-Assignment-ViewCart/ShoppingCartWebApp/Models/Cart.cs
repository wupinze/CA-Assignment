using System;
using System.Collections.Generic;

namespace ShoppingCartWebApp.Models
{
    public class Cart
    {
        public Cart()
        {
            Id = new Guid();
        }


        public Guid Id { get; set; }


        public virtual User user { get; set; }
        public virtual Product product { get; set; }



    }
}