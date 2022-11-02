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
        public DbSet<UserLikes> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserLikes>().HasKey(k => new { k.SourceUserId, k.LikedUserId });
            builder.Entity<UserLikes>().HasOne
            (s => s.SourceUser).WithMany(l => l.LikedUsers);
            builder.Entity<UserLikes>()
            .HasOne(s=>s .SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s=> s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserLikes>()
            .HasOne(s=>s.LikedUser)
            .WithMany(l=>l.LikedByUsers)
            .HasForeignKey(s=>s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);

            

            
            
        }

    }


}