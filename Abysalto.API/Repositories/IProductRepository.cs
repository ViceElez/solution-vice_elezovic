using Abysalto.API.Models;

namespace Abysalto.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<List<Product>> GetProductsByCategoryAndPrice(string category, decimal price);
        Task<List<Product>> GetProductsByName(string search);
    }
}
