using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class ShippingController : Controller
    {
        private readonly FedExService _fedExService;
        public ShippingController(FedExService _fedExService)
        {
            this._fedExService = _fedExService;
        }

        public async Task<IActionResult> GetRates()
        {
            var rates = await _fedExService.GetShippingRatesAsync();
            return View("Rates", rates);
        }

        public ActionResult ShipmentSuccess() {
            return View();
        }
    }
}
