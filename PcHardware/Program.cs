using Microsoft.EntityFrameworkCore;
using PcHardware.Services;
using Microsoft.AspNetCore.Identity;
using PcHardware.Models;
using PcHardware.Repositories;
using PcHardware.Repositories.Category;
using PcHardware.Repositories.Cart;
using PcHardware.Repositories.Wishlist;
using PcHardware.Repositories.Order;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Stripe;
using PcHardware.Repositories.Manufacturer;
using PcHardware.Repositories.Discount;
using PcHardware.Repositories.Address;
using PcHardware.Repositories.Customers;
using PcHardware.Repositories.Warehouse;
using PcHardware.Repositories.Review;
using PcHardware.Repositories.User;
using PcHardware.Repositories.Metatag;
using QuestPDF.Infrastructure;
using PcHardware.Repositories.Activity;

namespace PcHardware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MyDbContext>(op => op.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<MyDbContext>();

            builder.Services.AddMvc(op => op.EnableEndpointRouting = false);

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IWishlistRepository, WishlilstRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMetatagRepository, MetatagRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

            builder.Services.AddScoped<OrderReportService>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            QuestPDF.Settings.License = LicenseType.Community;

            var app = builder.Build();

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseMvcWithDefaultRoute();
            app.UseAuthorization();

            app.MapRazorPages();
            app.Run();
        }
    }
}
