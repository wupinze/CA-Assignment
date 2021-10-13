using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartWebApp.Models
{
    public class Session
    {
        public Session()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
