using PcHardware.Models;
namespace PcHardware.Repositories.Order
{
    public interface IOrderRepository
    {
        public List<Models.Order> GetUserOrders(string UserId);
    }
}
