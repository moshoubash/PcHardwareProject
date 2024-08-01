
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
