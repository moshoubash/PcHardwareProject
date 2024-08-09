using PcHardware.Models;
namespace PcHardware.Repositories.Order
{
    public interface IOrderRepository
    {
        public List<Models.Order> GetUserOrders(string UserId);
        public List<Models.Order> GetOrders();
        public Models.Order GetOrderById(int Id);
        public void DeleteOrder(int Id);
    }
}
