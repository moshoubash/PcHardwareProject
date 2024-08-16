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

        [HttpGet]
        public IActionResult CreateShipment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment(ShippingRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _fedExService.CreateShipment(model);
                    if (response.Status == "Error")
                    {
                        ModelState.AddModelError("", response.Message);
                        return View(model);
                    }

                    ViewBag.TrackingNumber = response.TrackingNumber;
                    return View("ShipmentSuccess");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);

        }

        public ActionResult ShipmentSuccess() {
            return View();
        }
    }
}
