﻿using Microsoft.EntityFrameworkCore;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Repositories.Configurations;

namespace RealEstate.DAL.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<HistoryEntity> Histories { get; set; }
        public DbSet<RealEstateEntity> RealEstates { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ReviewEntityConfiguration());
        }
    }
}
