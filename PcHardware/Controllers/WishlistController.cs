using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using PcHardware.Models;
using PcHardware.Repositories.Wishlist;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "client")]
    public class WishlistController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWishlistRepository wishlistRepository;

        public WishlistController(UserManager<ApplicationUser> userManager, IWishlistRepository wishlistRepository)
        {
            this.userManager = userManager;
            this.wishlistRepository = wishlistRepository;
        }
        public async Task<ActionResult> Items()
        {
            var user = await userManager.GetUserAsync(User);
            return View(wishlistRepository.GetWishlistItems(user.Id));
        }
        
        [HttpPost]
        public async Task<ActionResult> AddItem(int ProductId)
        {
            var user = await userManager.GetUserAsync(User);

            bool isInWishlist = wishlistRepository.isInWishlist(user.Id, ProductId);
            if (isInWishlist == false) {

                var wishlistItem = new Wishlist
                {
                    UserId = user.Id,
                    ProductId = ProductId
                };

                wishlistRepository.AddItem(wishlistItem);

                return Redirect($"/Product/Details/{ProductId}");
            }
            else { 
                return Redirect($"/Product/Details/{ProductId}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveItem(int ProductId)
        {
            var user = await userManager.GetUserAsync(User);
            string UserId = user.Id;

            wishlistRepository.DeleteItem(UserId, ProductId);
            return RedirectToAction("Items");
        }
    }
}
