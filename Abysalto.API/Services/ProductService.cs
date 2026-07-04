using Abysalto.API.Repositories;  
using Abysalto.API.Models;

namespace Abysalto.API.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            if (productId <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}. Product ID must be greater than zero.", productId);
                throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));
            }
            return await _productRepository.GetProductById(productId);
        }

        public async Task<List<Product>> GetProductsByCategoryAndPrice(string? category, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue && minPrice.Value < 0)
            {
                _logger.LogWarning("Invalid min price: {MinPrice}. Min price must be greater than or equal to zero.", minPrice);
                throw new ArgumentException("Min price must be greater than or equal to zero.");
            }

            if (maxPrice.HasValue && maxPrice.Value < 0)
            {
                _logger.LogWarning("Invalid max price: {MaxPrice}. Max price must be greater than or equal to zero.", maxPrice);
                throw new ArgumentException("Max price must be greater than or equal to zero.");
            }

            if (minPrice.HasValue && maxPrice.HasValue && maxPrice.Value < minPrice.Value)
            {
                _logger.LogWarning("Invalid price range: minPrice {MinPrice} is greater than maxPrice {MaxPrice}.", minPrice, maxPrice);
                throw new ArgumentException("Max price must be greater than or equal to min price.");
            }

            var products = string.IsNullOrWhiteSpace(category)
                ? await _productRepository.GetAllProducts()
                : await _productRepository.GetProductsByCategory(category);

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();

            _logger.LogInformation("Retrieved {ProductCount} products for category '{Category}' with price range {MinPrice} - {MaxPrice}.", products.Count, category, minPrice, maxPrice);

            return products;
        }

        public async Task<List<Product>> GetProductsByName(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                _logger.LogWarning("Search term cannot be null or whitespace.");
                throw new ArgumentException("Product name cannot be null or whitespace.", nameof(search));
            }

            return await _productRepository.GetProductsByName(search);

        }
    }
}
