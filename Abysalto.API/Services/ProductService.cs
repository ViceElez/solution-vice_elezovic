using Abysalto.API.Repositories;  
using Abysalto.API.Models;

namespace Abysalto.API.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));
            }

            return await _productRepository.GetProductById(productId);
        }

        public async Task<List<Product>> GetProductsByName(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new ArgumentException("Product name cannot be null or whitespace.", nameof(search));
            }

            return await _productRepository.GetProductsByName(search);

        }
    }
}
