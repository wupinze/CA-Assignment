using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartWebApp.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            carts = new List<Cart>();
            purHistories = new List<PurchaseHistory>();
        }

        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        //[Required]
        //public string Name { get; set; }

        [Required]
        public byte[] PassHash { get; set; }


        public virtual ICollection<Cart> carts { get; set; }

        public virtual ICollection<PurchaseHistory> purHistories { get; set; }
    }
}
