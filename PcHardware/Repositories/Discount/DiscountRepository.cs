
using PcHardware.Services;

namespace PcHardware.Repositories.Discount
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly MyDbContext dbContext;
        public DiscountRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void IDiscountRepository.CreateDiscount(Models.Discount discount)
        {
            dbContext.Discounts.Add(discount);
            dbContext.SaveChanges();
        }

        void IDiscountRepository.DeleteDiscount(int Id)
        {
            var dis = dbContext.Discounts.Where(d => d.Id == Id).FirstOrDefault();
            dbContext.Discounts.Remove(dis);
            dbContext.SaveChanges();
        }

        void IDiscountRepository.EditDiscount(int Id, Models.Discount discount)
        {
            var dis = dbContext.Discounts.Where(d => d.Id == Id).FirstOrDefault();
            dis.StartDate = discount.StartDate;
            dis.EndDate = discount.EndDate;
            dis.DiscountPercentage = discount.DiscountPercentage;
            dis.IsActive = discount.IsActive;
            dis.Name = discount.Name;
            dbContext.SaveChanges();
        }

        Models.Discount IDiscountRepository.GetDiscountById(int Id)
        {
            return dbContext.Discounts.Where(d => d.Id == Id).FirstOrDefault();
        }

        List<Models.Discount> IDiscountRepository.GetDiscounts()
        {
            return dbContext.Discounts.ToList();
        }
    }
}
