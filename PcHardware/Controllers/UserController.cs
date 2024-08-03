using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Order;
using PcHardware.Models;
using Microsoft.EntityFrameworkCore;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;

        public UserController(MyDbContext dbContext, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<ActionResult> Orders()
        {
            var user = await userManager.GetUserAsync(User);
            return View(orderRepository.GetUserOrders(user.Id));
        }

        [HttpGet]
        public async Task<ActionResult> Settings() {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Settings(ApplicationUser applicationUser)
        {
            var user = await userManager.GetUserAsync(User);
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            user.Email = applicationUser.Email;
            user.DateOfBirth = applicationUser.DateOfBirth;
            user.PhoneNumber = applicationUser.PhoneNumber;
            dbContext.SaveChanges();
            return Redirect("/User/Settings");
        }
    }
}
