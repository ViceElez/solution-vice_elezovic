using Abysalto.API.Models;

namespace Abysalto.API.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int productId);
        IEnumerable<Product> GetProductsByCategoryAndPrice(string category, float price);
        Product GetProductByName(string name);
    }
}
