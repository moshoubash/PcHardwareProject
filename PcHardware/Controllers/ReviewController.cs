using Microsoft.AspNetCore.Mvc;
using PcHardware.Repositories.Review;

namespace PcHardware.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository reviewRepository;
        public ReviewController(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public IActionResult Manage()
        {
            return View(reviewRepository.GetReviews());
        }

        public ActionResult Delete(int Id) {
            reviewRepository.DeleteReview(Id);
            return RedirectToAction("Manage");
        }
    }
}
