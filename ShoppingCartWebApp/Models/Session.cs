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
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
