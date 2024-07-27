using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PcHardware.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;
namespace PcHardware.Services
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> dbContext) : base(dbContext)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var seller = new IdentityRole("seller");
            seller.NormalizedName = "seller";

            var client = new IdentityRole("client");
            client.NormalizedName = "client";

            var WarehouseManager = new IdentityRole("WarehouseManager");
            WarehouseManager.NormalizedName = "WarehouseManager";

            builder.Entity<IdentityRole>().HasData(admin, seller, client, WarehouseManager);
        }
    }
}
