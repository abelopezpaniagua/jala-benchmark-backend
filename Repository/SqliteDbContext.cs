using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;

namespace Repository
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        
        public SqliteDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Define properties of database entities
            DefineProductModel(modelBuilder);
        }

        private void DefineProductModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.Code)
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(65)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasColumnType("text")
                .IsRequired(false);
        }
    }
}
