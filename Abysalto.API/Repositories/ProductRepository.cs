using Abysalto.API.Models;

namespace Abysalto.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public Product GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProductsByCategoryAndPrice(string category, float price)
        {
            throw new NotImplementedException();
        }
    }
}
