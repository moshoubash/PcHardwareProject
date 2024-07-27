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
            
            // RELATIONS

            // PRODUCT
            builder.Entity<Product>().HasOne(p => p.Manufacturer).WithMany(m => m.Products).HasForeignKey(m => m.ManufacturerId);
            builder.Entity<Product>().HasOne(p => p.Category).WithMany(m => m.Products).HasForeignKey(m => m.CategoryId);

            // APPLICATION USER
            builder.Entity<Address>().HasOne(a => a.User).WithMany(u => u.Addresses).HasForeignKey(a => a.UserId);

            // CART
            builder.Entity<Cart>().HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey<Cart>(c => c.UserId);

            // CART ITEMS
            builder.Entity<CartItem>().HasOne(ci => ci.Cart).WithMany(c => c.CartItems).HasForeignKey(ci => ci.CartId);
            builder.Entity<CartItem>().HasOne(ci => ci.Product).WithMany(p => p.CartItems).HasForeignKey(ci => ci.ProductId);

            // INVENTORY
            builder.Entity<Inventory>().HasOne(i => i.Product).WithMany(p => p.Inventories).HasForeignKey(i => i.ProductId);
            builder.Entity<Inventory>().HasOne(i => i.Warehouse).WithMany(w => w.Inventories).HasForeignKey(w => w.WarehouseId);

            // REVIEW
            builder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId);
            builder.Entity<Review>().HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId);

            // WISHLIST
            builder.Entity<Wishlist>().HasOne(w => w.User).WithMany(u => u.Wishlists).HasForeignKey(w => w.UserId);
            builder.Entity<Wishlist>().HasOne(w => w.Product).WithMany(p => p.Wishlists).HasForeignKey(w => w.ProductId);

            // ORDER
            builder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);

            // ORDER ITEM
            builder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId);
            builder.Entity<OrderItem>().HasOne(oi => oi.Product).WithMany(p => p.OrderItems).HasForeignKey(oi => oi.ProductId);

            // Product Specification
            builder.Entity<ProductSpecification>().HasOne(ps => ps.Product).WithMany(p => p.ProductSpecifications).HasForeignKey(ps => ps.ProductId);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Manufacturer> Manufactureres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
    }
}
