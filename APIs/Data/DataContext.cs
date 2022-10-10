using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIs.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> User { get; set; }
    }
}