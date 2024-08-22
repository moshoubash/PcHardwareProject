using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHardware.Repositories.Order;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly MyDbContext dbContext;
        public OrderController(IOrderRepository orderRepository, MyDbContext dbContext)
        {
            this.orderRepository = orderRepository;
            this.dbContext = dbContext;
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

        public ActionResult Delete(int Id) {
            orderRepository.DeleteOrder(Id);
            return Redirect("/Order/Manage");
        }

        [HttpPost]
        public ActionResult EditStatus(int OrderId, string Status)
        {
            var order = dbContext.Orders.Find(OrderId);

            if (order != null)
            {
                order.Status = Status;
                dbContext.SaveChanges();
            }

            return RedirectToAction("Manage");
        }
    }
}
