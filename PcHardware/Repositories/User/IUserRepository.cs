using PcHardware.Models;

namespace PcHardware.Repositories.User
{
    public interface IUserRepository
    {
        List<ApplicationUser> GetUsers();
        ApplicationUser GetUserById(string Id);
        void EditUser(ApplicationUser user);
        Task CreateUser(ApplicationUser user);
        void DeleteUser(string Id);
    }
}
