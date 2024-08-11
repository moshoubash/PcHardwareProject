using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "seller")]
    public class SellerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;
        public SellerController(UserManager<ApplicationUser> userManager, MyDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Settings()
        {
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
            return View(applicationUser);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string currentPassword, string newPassword) {
            var user = await userManager.GetUserAsync(User);
            await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return Redirect("/Seller/Settings");
        }
    }
}
