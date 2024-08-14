using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MyDbContext dbContext;
        public CustomerRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        ApplicationUser ICustomerRepository.GetCustomer(string Id)
        {
            return dbContext.Users.Where(u => u.Id == Id).FirstOrDefault();
        }

        List<Models.Order> ICustomerRepository.GetCustomerOrders(string Id)
        {
            var orders = dbContext.Orders.Where(o => o.UserId == Id).ToList();
            return orders;
        }

        List<ApplicationUser> ICustomerRepository.GetCustomers()
        {
            var users = dbContext.Users.ToList();
            var orders = dbContext.Orders.ToList();
            var customers = new List<ApplicationUser>();
            foreach (var u in users) {
                foreach (var o in orders) {
                    if (o.UserId == u.Id) {
                        customers.Add(u);
                    }
                }
            }
            return customers;
        }
    }
}
