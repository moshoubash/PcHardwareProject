using PcHardware.Models;

namespace PcHardware.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetProducts();
        public void CreateProduct(Product product);
        public void EditProduct(Product product);
        public void DeleteProduct(int Id);
        public Product GetProductById(int Id);
    }
}
