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

        void IOrderRepository.DeleteOrder(int Id)
        {
            var targetOrder = dbContext.Orders.Where(o => o.Id == Id).FirstOrDefault();
            if (targetOrder != null) {
                dbContext.Orders.Remove(targetOrder);
                dbContext.SaveChanges();
            }
        }

        Models.Order IOrderRepository.GetOrderById(int Id)
        {
            var order = (from o in dbContext.Orders where o.Id == Id select o).FirstOrDefault();
            return order;
        }

        List<Models.Order> IOrderRepository.GetOrders()
        {
            return dbContext.Orders.ToList() ;
        }

        List<Models.Order> IOrderRepository.GetUserOrders(string UserId)
        {
            return dbContext.Orders.Where(o => o.UserId == UserId).ToList();
        }
    }
}
