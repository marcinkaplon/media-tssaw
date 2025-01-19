using System;
using Microsoft.EntityFrameworkCore;
using PostAPI.Models;

namespace PostAPI.Data
{
    public class PostDbContext : DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().HasKey(p => p.Id);
        }
    }
}

