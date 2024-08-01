using PcHardware.Models;

namespace PcHardware.Repositories.Cart
{
    public interface ICartRepository
    {
        public void AddCartItem(CartItem cartItem);
        public void RemoveCartItem(int ProductId, int CartId);
        public Models.Cart GetCartByUserId(string UserId);
        public bool IsInCart(CartItem cartItem);
    }
}
