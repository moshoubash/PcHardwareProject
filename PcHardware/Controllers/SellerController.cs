using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PcHardware.Models;
using PcHardware.Repositories;
using PcHardware.Repositories.Customers;
using PcHardware.Repositories.Order;
using PcHardware.Services;
using System.Runtime.CompilerServices;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class SellerController : Controller
    {
        private readonly IProductRepository ProductRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;
        public SellerController(UserManager<ApplicationUser> userManager, IOrderRepository orderRepository, ICustomerRepository customerRepository, MyDbContext dbContext, IProductRepository ProductRepository)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.ProductRepository = ProductRepository;
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
        }
        public ActionResult Dashboard()
        {
            ViewBag.ProductsNumber = ProductRepository.GetProducts().Count;
            ViewBag.CustomersNumber = customerRepository.GetCustomers().Count;
            ViewBag.OrdersNumber = orderRepository.GetOrders().Count;
            ViewBag.TotalSales = orderRepository.GetOrders().Sum(o => o.TotalAmount);

            ViewBag.PendingOrders = dbContext.Orders.Where(o => o.Status == "pending").ToList();
            // Get top three products
            var topThreeProducts = ProductRepository.TopThreeProducts();

            // Calculate total quantities for each product
            var productOneQuantities = dbContext.OrderItems
                .Where(oi => oi.ProductId == topThreeProducts[0].Id)
                .Sum(oi => oi.Quantity);

            var productTwoQuantities = dbContext.OrderItems
                .Where(oi => oi.ProductId == topThreeProducts[1].Id)
                .Sum(oi => oi.Quantity);

            var productThreeQuantities = dbContext.OrderItems
                .Where(oi => oi.ProductId == topThreeProducts[2].Id)
                .Sum(oi => oi.Quantity);

            // Prepare data for the chart
            var chartData = new
            {
                ProductNames = topThreeProducts.Select(p => p.Name).ToList(),
                Quantities = new List<int>
                {
                    productOneQuantities,
                    productTwoQuantities,
                    productThreeQuantities
                }
            };

            // Pass the data to the view
            return View(chartData);
        }

        [HttpGet]
        public async Task<ActionResult> Settings()
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Settings(ApplicationUser applicationUser)
        {
            var user = await userManager.GetUserAsync(User);
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            user.Email = applicationUser.Email;
            user.DateOfBirth = applicationUser.DateOfBirth;
            user.PhoneNumber = applicationUser.PhoneNumber;
            dbContext.SaveChanges();
            return View(applicationUser);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string currentPassword, string newPassword) {
            var user = await userManager.GetUserAsync(User);
            await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return Redirect("/Seller/Settings");
        }
    }
}
