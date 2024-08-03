using PcHardware.Services;

namespace PcHardware.Repositories.Order
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyDbContext dbContext;
        public OrderRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        List<Models.Order> IOrderRepository.GetUserOrders(string UserId)
        {
            return dbContext.Orders.Where(o => o.UserId == UserId).ToList();
        }
    }
}
