using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartWebApp.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) 
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Cart> carts { get; set;}
        public DbSet<PurchaseHistory> purHistories { get; set;}
        public DbSet<Product> products { get; set;}

    }
}
