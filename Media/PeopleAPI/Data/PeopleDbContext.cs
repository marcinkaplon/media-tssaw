using System;
using Microsoft.EntityFrameworkCore;
using PeopleAPI.Models;

namespace PeopleAPI.Data
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserFollows> UserFollows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFollows>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity
                    .HasOne(e => e.Follower)
                    .WithMany(e => e.Following)
                    .HasForeignKey(e => e.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.Followee)
                    .WithMany(e => e.Followers)
                    .HasForeignKey(e => e.FolloweeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

