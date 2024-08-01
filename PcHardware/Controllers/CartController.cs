using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PcHardware.Models;
using PcHardware.Repositories.Cart;
using PcHardware.Services;

namespace PcHardware.Controllers
{
    [Authorize(Roles = "client")]
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MyDbContext dbContext;
        public CartController(UserManager<ApplicationUser> _userManager, ICartRepository cartRepository, MyDbContext dbContext)
        {
            this._userManager = _userManager;
            this.cartRepository = cartRepository;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<ActionResult> AddCartItem(int ProductId, int Quantity)
        {
            var targetUser = await _userManager.GetUserAsync(User);
            var targetCart = cartRepository.GetCartByUserId(targetUser.Id);

            var cartItem = new CartItem { 
                Quantity = Quantity,
                CartId = targetCart.Id,
                ProductId = ProductId
            };

            bool isInCart = cartRepository.IsInCart(cartItem);

            // add quantity
            if (isInCart) {
                var targetCartItem = dbContext.CartItems.Where(ci=>ci.ProductId==ProductId && ci.CartId == cartItem.CartId).FirstOrDefault();
                targetCartItem.Quantity= targetCartItem.Quantity + Quantity;
                dbContext.SaveChanges();
                return Redirect($"/Product/Details/{ProductId}");
            }
            else {
                cartRepository.AddCartItem(cartItem);
                return Redirect($"/Product/Details/{ProductId}");
            }
            
        }

        [HttpPost]
        public ActionResult RemoveCartItem(int ProductId, int CartId)
        {
            cartRepository.RemoveCartItem(ProductId, CartId);
            return Redirect("/Cart/Items");
        }

        public async Task<ActionResult> Items()
        {
            var targetUser = await _userManager.GetUserAsync(User);
            var targetCart = cartRepository.GetCartByUserId(targetUser.Id);
            List<CartItem> list = (from CI in dbContext.CartItems 
                                   where CI.CartId == targetCart.Id 
                                   select CI).ToList();
            return View(list);
        }
    }
}
