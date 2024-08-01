using Microsoft.EntityFrameworkCore;
using PcHardware.Models;
using PcHardware.Services;

namespace PcHardware.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext dbContext;
        public CategoryRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void ICategoryRepository.CreateCategory(Models.Category category)
        {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
        }

        void ICategoryRepository.DeleteCategory(int Id)
        {
            dbContext.Categories.Remove(dbContext.Categories.FirstOrDefault(c=>c.Id == Id));
            dbContext.SaveChanges();
        }

        List<Models.Category> ICategoryRepository.GetCategories()
        {
            return dbContext.Categories.ToList();
        }

        async Task<Models.Category> ICategoryRepository.GetCategoryWithProductsAsync(int Id)
        {
            return await dbContext.Categories
                             .Include(c => c.Products)
                             .FirstOrDefaultAsync(c => c.Id == Id);
        }
    }
}
