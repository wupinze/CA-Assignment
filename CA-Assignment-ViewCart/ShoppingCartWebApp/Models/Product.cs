using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartWebApp.Models
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();
            PurHistories = new List<PurchaseHistory>();
            carts = new List<Cart>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ProductName { get; set;}


        [Required]
        [MaxLength(200)]
        public string Description { get; set;}

        [Required]
        public float Price { get; set;}

        [Required]
        public string imageUrl { get; set;} 

        public virtual ICollection<PurchaseHistory> PurHistories { get; set;}

        public virtual ICollection<Cart> carts { get; set; }
    }
}
