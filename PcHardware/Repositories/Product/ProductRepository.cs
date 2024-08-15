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

        List<Product> IProductRepository.TopThreeProducts()
        {
            var topThreeProductIds = dbContext.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantitySold)
                .Take(3)
                .Select(g => g.ProductId) // Select only the ProductId
                .ToList();

            var topThreeProducts = dbContext.Products
                .Where(p => topThreeProductIds.Contains(p.Id))
                .ToList();

            return topThreeProducts;
        }
    }
}
