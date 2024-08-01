using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;

namespace PcHardware.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Checkout()
        {
            List<CartItem> list = new List<CartItem>();
            return View(list);
        }
    }
}
