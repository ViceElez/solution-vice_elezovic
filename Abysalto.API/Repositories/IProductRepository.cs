using Abysalto.API.Models;
using Abysalto.API.DTOs;

namespace Abysalto.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<List<ProductDto>> GetProductsByCategory(string category);
        Task<List<ProductDto>> GetProductsByName(string search);
    }
}
