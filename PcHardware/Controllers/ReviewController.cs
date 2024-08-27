using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Review;
using PcHardware.Models;
using System.Data;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository reviewRepository;
        private readonly MyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public ReviewController(IReviewRepository reviewRepository, UserManager<ApplicationUser> userManager, MyDbContext dbContext)
        {
            this.reviewRepository = reviewRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public IActionResult Manage()
        {
            return View(reviewRepository.GetReviews());
        }

        public async Task<ActionResult> Delete(int Id) {
            reviewRepository.DeleteReview(Id);

            var user = await userManager.GetUserAsync(User);
            var activity = new Activity { 
                Type = $"Review {Id} deleted",
                Time = DateTime.Now,
                UserId = user.Id
            };
            
            dbContext.Activities.Add(activity);
            dbContext.SaveChanges();

            return RedirectToAction("Manage");
        }
    }
}
