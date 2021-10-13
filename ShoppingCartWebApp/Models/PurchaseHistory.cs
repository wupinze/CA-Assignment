using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartWebApp.Models
{
    public class PurchaseHistory
    {
        public PurchaseHistory()
        {
            Id = new Guid();
        }

        public Guid Id { get; set;}

        [Required]
        public DateTime PurchaseDate { get; set;}

        public string ActivationCode { get; set;}

        public virtual Guid UserId { get; set; }
        public virtual Guid ProductId { get; set; }

    }
}
