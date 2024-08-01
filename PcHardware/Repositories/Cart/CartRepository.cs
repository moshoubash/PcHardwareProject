
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Repositories.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly MyDbContext dbContext;
        public CartRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        void ICartRepository.AddCartItem(CartItem cartItem)
        {
            dbContext.CartItems.Add(cartItem);
            dbContext.SaveChanges();
        }

        Models.Cart ICartRepository.GetCartByUserId(string UserId)
        {
            return dbContext.Carts.Where(c=>c.UserId== UserId).FirstOrDefault();
        }

        bool ICartRepository.IsInCart(CartItem cartItem)
        {
            var targetCartItem = dbContext.CartItems.Where(ci => ci.CartId == cartItem.CartId && ci.ProductId == cartItem.ProductId).FirstOrDefault();
            return (targetCartItem != null) ? true : false;
        }

        void ICartRepository.RemoveCartItem(int ProductId, int CartId)
        {
            var targetCartItem = dbContext.CartItems.Where(ci => ci.ProductId == ProductId && ci.CartId == CartId).FirstOrDefault();
            if (targetCartItem != null) {
                dbContext.CartItems.Remove(targetCartItem);
                dbContext.SaveChanges();
            }
        }
    }
}
