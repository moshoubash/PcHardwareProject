using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Repositories.Review
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MyDbContext dbContext;
        public ReviewRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void IReviewRepository.DeleteReview(int Id)
        {
            dbContext.Reviews.Remove(dbContext.Reviews.FirstOrDefault(r => r.Id == Id));
            dbContext.SaveChanges();
        }

        List<Models.Review> IReviewRepository.GetProductReviews(int Id)
        {
            var reviews = dbContext.Reviews.Where(r => r.ProductId == Id).ToList() ;
            return reviews ;
        }

        List<Models.Review> IReviewRepository.GetReviews()
        {
            return dbContext.Reviews.ToList();
        }
    }
}
