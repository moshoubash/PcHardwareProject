﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Order;
using PcHardware.Models;
using Microsoft.EntityFrameworkCore;
using PcHardware.Services;
using PcHardware.Repositories.User;
using Stripe.Climate;

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

            dbContext.Activities.Add(new Activity { 
                Type = $"User {user.Id} settings updated",
                Time = DateTime.Now,
                UserId = user.Id
            });
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

            var notify = new Notification { Type = "New Review", Description = $"User {user.Id} create review on product {ProductId} : {(review.Comment.Length < 100? review.Comment : review.Comment.Substring(0, 100))}", Time = DateTime.Now, IsRead = false };
            dbContext.Notifications.Add(notify);
            dbContext.SaveChanges();

            dbContext.Activities.Add(new Activity
            {
                Type = $"User {user.Id} make new review on product {ProductId}",
                Time = DateTime.Now,
                UserId = user.Id
            });
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

            dbContext.Activities.Add(new Activity
            {
                Type = $"New User added {user.Id}",
                Time = DateTime.Now,
                UserId = user.Id
            });
            dbContext.SaveChanges();

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

            dbContext.Activities.Add(new Activity
            {
                Type = $"User {user.Id} Information updated",
                Time = DateTime.Now,
                UserId = user.Id
            });
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Delete(string Id) {
            var user = await userManager.GetUserAsync(User);
            userRepository.DeleteUser(Id);

            dbContext.Activities.Add(new Activity
            {
                Type = $"User {Id} deleted",
                Time = DateTime.Now,
                UserId = user.Id
            });
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }
    }
}
