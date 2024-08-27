using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHardware.Repositories.Order;
using PcHardware.Services;
using PcHardware.Models;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;

        public OrderController(IOrderRepository orderRepository, MyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.orderRepository = orderRepository;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public ActionResult Manage()
        {
            var orders = orderRepository.GetOrders();
            return View(orders);
        }

        [HttpGet]
        public ActionResult Details(int Id) {
            return View(orderRepository.GetOrderById(Id));
        }

        public async Task<ActionResult> Delete(int Id) {
            var user = await userManager.GetUserAsync(User);
            
            orderRepository.DeleteOrder(Id);

            var activity = new Activity
            {
                Type = $"Order {Id} has been deleted",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return Redirect("/Order/Manage");
        }

        [HttpPost]
        public async Task<ActionResult> EditStatus(int OrderId, string Status)
        {
            var user = await userManager.GetUserAsync(User);
            var order = dbContext.Orders.Find(OrderId);

            if (order != null)
            {
                order.Status = Status;
                dbContext.SaveChanges();
            }
            
            var activity = new Activity
            {
                Type = $"Order {OrderId} Status changes to {Status}",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();


            return RedirectToAction("Manage");
        }
    }
}
