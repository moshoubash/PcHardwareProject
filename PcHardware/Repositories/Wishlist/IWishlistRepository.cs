namespace PcHardware.Repositories.Wishlist
{
    public interface IWishlistRepository
    {
        public List<Models.Wishlist> GetWishlistItems(string UserId);
        public void AddItem(Models.Wishlist wishlist);
        public void DeleteItem(string UserId, int Id);
    }
}
