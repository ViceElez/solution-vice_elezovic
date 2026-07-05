using Abysalto.API.DTOs.Products;
using Abysalto.API.Models.Products;

namespace Abysalto.API.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task<List<ProductDto>> GetProductsByCategory(string category);
        Task<List<ProductDto>> GetProductsByName(string search);
    }
}
