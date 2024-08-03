using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PcHardware.Models;
using PcHardware.Repositories.Cart;
using PcHardware.Services;
using System.Numerics;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "client")]
    public class PaymentController : Controller
    {
        private readonly MyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICartRepository cartRepository;

        public PaymentController(MyDbContext dbContext, UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Checkout()
        {
            var user = await userManager.GetUserAsync(User);
            
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Email = user.Email;
            ViewBag.Id = user.Id;

            ViewBag.Countries = new SelectList(dbContext.Countries, "Id", "Name");

            var userCart = cartRepository.GetCartByUserId(user.Id);
            var userCartItems = dbContext.CartItems.Where(ci => ci.CartId == userCart.Id).ToList();

            ViewBag.CartSize = userCartItems.Count;

            return View(userCartItems);
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(Address address, decimal TotalAmount)
        {
            var user = await userManager.GetUserAsync(User);

            // Add address to the database
            dbContext.Addresses.Add(address);
            dbContext.SaveChanges();

            var targetAddress = (from a in dbContext.Addresses where a.UserId == user.Id select a).FirstOrDefault();

            // create new order
            var order = new Order {
                OrderDate = DateTime.Now,
                TotalAmount = TotalAmount,
                Status = "Pending",
                AddressId = targetAddress.Id,
                UserId = targetAddress.UserId
            };
            
            // Add order to the database
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // convert eash cart item into order item
            var targetCart = dbContext.Carts.Where(c => c.UserId == user.Id).FirstOrDefault();
            var userCartItems = dbContext.CartItems.Where(ci => ci.CartId == targetCart.Id).ToList();

            foreach (var ci in userCartItems) {
                var price = dbContext.Products.Where(p => p.Id == ci.ProductId).FirstOrDefault().Price;
                var oi = new OrderItem { 
                    Quantity = ci.Quantity,
                    UnitPrice = price,
                    OrderId = order.Id,
                    ProductId = ci.ProductId
                };
                dbContext.OrderItems.Add(oi);
                dbContext.SaveChanges();
            }

            foreach (var ci in userCartItems) {
                dbContext.CartItems.Remove(ci);
                dbContext.SaveChanges();
            }

            return Redirect("/Index");
        }
    }
}
