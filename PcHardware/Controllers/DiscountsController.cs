using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Repositories.Discount;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "admin, seller")]
    public class DiscountsController : Controller
    {
        private readonly IDiscountRepository discountRepository;
        public DiscountsController(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        [HttpGet]
        public IActionResult Manage()
        {
            return View(discountRepository.GetDiscounts());
        }

        [HttpPost]
        public ActionResult Create(Discount discount) {
            discountRepository.CreateDiscount(discount);
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(int Id)
        {
            discountRepository.DeleteDiscount(Id);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return View(discountRepository.GetDiscountById(Id));
        }

        [HttpPost]
        public ActionResult Edit(Discount discount)
        {
            discountRepository.EditDiscount(discount.Id, discount);
            return RedirectToAction("Manage");
        }
    }
}
