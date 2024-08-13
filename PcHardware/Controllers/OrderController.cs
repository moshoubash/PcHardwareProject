using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Order;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
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
    }
}
