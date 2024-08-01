



using PcHardware.Services;

namespace PcHardware.Repositories.Wishlist
{
    public class WishlilstRepository : IWishlistRepository
    {
        private readonly MyDbContext dbContext;
        public WishlilstRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void IWishlistRepository.AddItem(Models.Wishlist wishlist)
        {
            dbContext.Wishlists.Add(wishlist);
            dbContext.SaveChanges();
        }

        void IWishlistRepository.DeleteItem(string UserId,int Id)
        {
            var targetWishlistItem = (from w in dbContext.Wishlists where w.ProductId == Id && w.UserId == UserId select w).FirstOrDefault();
            if (targetWishlistItem != null) {
                dbContext.Wishlists.Remove(targetWishlistItem);
                dbContext.SaveChanges();
            }
        }

        List<Models.Wishlist> IWishlistRepository.GetWishlistItems(string UserId)
        {
            return dbContext.Wishlists.Where(w=>w.UserId==UserId).ToList();
        }

        bool IWishlistRepository.isInWishlist(string UserId, int ProductId)
        {
            var targetItem = dbContext.Wishlists.Where(w=>w.UserId==UserId && w.ProductId == ProductId).FirstOrDefault();
            return (targetItem == null) ? false : true;
        }
    }
}
