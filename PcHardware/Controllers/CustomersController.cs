using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Customers;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller, admin")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        public CustomersController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public ActionResult Manage()
        {
            return View(customerRepository.GetCustomers());
        }

        public ActionResult UserOrders(string Id)
        {
            return View(customerRepository.GetCustomerOrders(Id));
        }

        public ActionResult ChatBox() {
            return View();
        }
    }
}
