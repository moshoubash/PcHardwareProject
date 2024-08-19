using PcHardware.Models;

namespace PcHardware.Repositories.Review
{
    public interface IReviewRepository
    {
        List<Models.Review> GetReviews();
        List<Models.Review> GetProductReviews(int Id);
        void DeleteReview(int Id);
    }
}
