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
            Id = new Guid();
            
        }

        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] PassHash { get; set; }

    }
}
