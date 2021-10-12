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

    }
}
