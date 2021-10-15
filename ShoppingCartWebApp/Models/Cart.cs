using System;
using System.Collections.Generic;

namespace ShoppingCartWebApp.Models
{
    public class Cart
    {
        public Cart()
        {
            Id = Guid.NewGuid().ToString();
        }


        public string Id { get; set;}


        public virtual User user { get; set;}
        public virtual Product product { get; set;}

        
    }
}
