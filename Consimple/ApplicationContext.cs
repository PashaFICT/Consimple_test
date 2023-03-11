using Consimple.Model;
using Consimple.Model.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Consimple.Model.Client;

namespace Consimple
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ClientDto> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RoleDto> Roles { get; set; }
        public DbSet<PurchaseDto> Purchases { get; set; }
        public DbSet<ProductDto> Products { get; set; }
        public DbSet<PurchasesProductsDto> PurchasesProducts { get; set; }
        public DbSet<CategoryDto> Categories { get; set; }
        //public DbSet<ClientDto> clients { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientDto>().ToTable("Clients");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<RoleDto>().ToTable("UsersRole");
            modelBuilder.Entity<PurchaseDto>().ToTable("Purchases"); 
            modelBuilder.Entity<ProductDto>().ToTable("Products");
            modelBuilder.Entity<PurchasesProductsDto>().ToTable("Purchases_Products");
            modelBuilder.Entity<CategoryDto>().ToTable("Categories");
        }
    }
}
