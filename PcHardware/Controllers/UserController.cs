using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Order;
using PcHardware.Models;
using Microsoft.EntityFrameworkCore;
using PcHardware.Services;
using PcHardware.Repositories.User;

namespace PcHardware.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;
        private readonly IUserRepository userRepository;
        public UserController(MyDbContext dbContext, IUserRepository userRepository, IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
        {
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.userRepository = userRepository;
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

        [HttpPost]
        public async Task<ActionResult> CreateReview(int ProductId, int Rating, string Comment) {
            var user = await userManager.GetUserAsync(User);

            var review = new Review { 
                ProductId = ProductId,
                Rating = Rating,
                Comment = Comment,
                Date = DateTime.Now,
                UserId = user.Id
            };

            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();

            return Redirect($"/Product/Details/{ProductId}");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Manage() {
            return View(userRepository.GetUsers());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Details(string Id) {
            return View(userRepository.GetUserById(Id));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(ApplicationUser user)
        {
            user.CreatedAt = DateTime.Now;
            user.UserName = user.Email;
            await userRepository.CreateUser(user);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(string Id) {
            return View(userRepository.GetUserById(Id));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(ApplicationUser user)
        {
            userRepository.EditUser(user);
            return RedirectToAction("Manage");
        }

        public ActionResult Delete(string Id) {
            userRepository.DeleteUser(Id);
            return RedirectToAction("Manage");
        }
    }
}
