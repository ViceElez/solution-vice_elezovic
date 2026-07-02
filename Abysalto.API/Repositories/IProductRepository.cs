using Abysalto.API.Models;

namespace Abysalto.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<List<Product>> GetProductsByCategory(string category);
        Task<List<Product>> GetProductsByName(string search);
    }
}
