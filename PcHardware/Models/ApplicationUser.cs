using Microsoft.AspNetCore.Identity;
namespace PcHardware.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateOfBirth { get; set; }

        public List<Address>? Addresses { get; set; }
        public Cart? Cart { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Wishlist>? Wishlists { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
