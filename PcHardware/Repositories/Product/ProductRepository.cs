using PcHardware.Services;
using PcHardware.Models;

namespace PcHardware.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly MyDbContext dbContext;

        public ProductRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        void IProductRepository.CreateProduct(Product product)
        {
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
        }

        void IProductRepository.DeleteProduct(int Id)
        {
            var targetProduct = dbContext.Products.Find(Id);
            if (targetProduct != null)
            {
                dbContext.Products.Remove(targetProduct);
                dbContext.SaveChanges();
            }
        }

        void IProductRepository.EditProduct(Product product)
        {
            dbContext.Products.Update(product);
            dbContext.SaveChanges();
        }

        Product IProductRepository.GetProductById(int Id)
        {
            return dbContext.Products.Find(Id);
        }

        List<Product> IProductRepository.GetProducts()
        {
            return dbContext.Products.ToList();
        }
    }
}
