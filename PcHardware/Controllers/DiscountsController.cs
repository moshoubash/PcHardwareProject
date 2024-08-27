using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Models;
using PcHardware.Repositories.Discount;
using PcHardware.Services;
using Stripe;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "admin, seller")]
    public class DiscountsController : Controller
    {
        private readonly IDiscountRepository discountRepository;
        private readonly MyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public DiscountsController(IDiscountRepository discountRepository, MyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.discountRepository = discountRepository;
            this.dbContext = dbContext;
            this.userManager = userManager;

        }

        [HttpGet]
        public IActionResult Manage()
        {
            return View(discountRepository.GetDiscounts());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Models.Discount discount) {

            var user = await userManager.GetUserAsync(User);
            discountRepository.CreateDiscount(discount);

            var activity = new Activity
            {
                Type = $"New Discount added {discount.Name}",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Delete(int Id)
        {
            var user = await userManager.GetUserAsync(User);
            discountRepository.DeleteDiscount(Id);

            var activity = new Activity
            {
                Type = $"Discount {Id} deleted",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return View(discountRepository.GetDiscountById(Id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Models.Discount discount)
        {
            var user = await userManager.GetUserAsync(User);
            discountRepository.EditDiscount(discount.Id, discount);

            var activity = new Activity
            {
                Type = $"Discount {discount.Id} was edited",
                Time = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        
    }
}
