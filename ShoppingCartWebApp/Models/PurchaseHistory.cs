using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartWebApp.Models
{
    public class PurchaseHistory
    {
        public PurchaseHistory()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string PurchaseDate { get; set; }

        public string ActivationCode { get; set; }

        public virtual User user { get; set; }
        public virtual Product product { get; set; }

    }
}
