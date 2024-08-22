using Microsoft.AspNetCore.Identity;
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MyDbContext dbContext;
        public UserRepository(MyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        async Task IUserRepository.CreateUser(ApplicationUser user)
        {
            await userManager.CreateAsync(user, "User1234@");
            dbContext.SaveChanges();
        }

        void IUserRepository.DeleteUser(string Id)
        {
            var targetUser = dbContext.Users.FirstOrDefault(u => u.Id == Id);
            
            // user data remove
            dbContext.Wishlists.ToList().RemoveAll(w => w.UserId == targetUser.Id);
            dbContext.Reviews.ToList().RemoveAll(r => r.UserId == targetUser.Id);
            dbContext.Orders.ToList().RemoveAll(o => o.UserId == targetUser.Id);
            dbContext.Addresses.ToList().RemoveAll(a => a.UserId == targetUser.Id);
            dbContext.Carts.Remove(dbContext.Carts.FirstOrDefault(c => c.UserId == targetUser.Id));

            // remove user
            dbContext.Users.Remove(targetUser);
            dbContext.SaveChanges();
        }

        void IUserRepository.EditUser(ApplicationUser user)
        {
            var targetUser = dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            targetUser.FirstName = user.FirstName;
            targetUser.LastName = user.LastName;
            targetUser.Email = user.Email;
            targetUser.PhoneNumber= user.PhoneNumber;
            targetUser.DateOfBirth = user.DateOfBirth;
            dbContext.SaveChanges();
        }

        ApplicationUser IUserRepository.GetUserById(string Id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id == Id);
        }

        List<ApplicationUser> IUserRepository.GetUsers()
        {
            return dbContext.Users.ToList();
        }
    }
}
