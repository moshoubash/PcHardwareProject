using PcHardware.Models;

namespace PcHardware.Repositories.Review
{
    public interface IReviewRepository
    {
        List<Models.Review> GetProductReviews(int Id);
    }
}
