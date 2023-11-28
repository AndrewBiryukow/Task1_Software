using Microsoft.EntityFrameworkCore;
using TradingCompanyDbApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Contexts
{
    public class TradeDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<UserDTO> Users { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<ProductDTO> Products { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<OrderDTO> Orders { get; set; }

        public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options)
        {
        }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDTO>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderDTO>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<UserDTO>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }
        

    }
}
