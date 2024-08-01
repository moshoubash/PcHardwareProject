namespace PcHardware.Repositories.Category
{
    public interface ICategoryRepository
    {
        public Task<Models.Category> GetCategoryWithProductsAsync(int Id);
        public void CreateCategory(Models.Category category);
        public void DeleteCategory(int Id);
        public List<Models.Category> GetCategories();
    }
}
