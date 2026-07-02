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

        public async Task<List<Product>> GetProductsByCategoryAndPrice(string? category, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue && minPrice.Value < 0)
                throw new ArgumentException("Min price must be greater than or equal to zero.");

            if (maxPrice.HasValue && maxPrice.Value < 0)
                throw new ArgumentException("Max price must be greater than or equal to zero.");

            if (minPrice.HasValue && maxPrice.HasValue && maxPrice.Value < minPrice.Value)
                throw new ArgumentException("Max price must be greater than or equal to min price.");

            var products = string.IsNullOrWhiteSpace(category)
                ? await _productRepository.GetAllProducts()
                : await _productRepository.GetProductsByCategory(category);

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();

            return products;
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
