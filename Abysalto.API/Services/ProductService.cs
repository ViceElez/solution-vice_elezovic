using Abysalto.API.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Abysalto.API.Models.Products;
using Abysalto.API.Repositories.Products;

namespace Abysalto.API.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IMemoryCache memoryCache, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private static MemoryCacheEntryOptions DefaultCacheOptions() =>
        new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        public async Task<List<ProductDto>> GetAllProducts()
        {
            if (!_memoryCache.TryGetValue("allProducts", out List<ProductDto>? allProducts) || allProducts == null)
            {
                allProducts = await _productRepository.GetAllProducts();
                _logger.LogInformation("Caching products for 5 minutes");
                _memoryCache.Set("allProducts", allProducts, DefaultCacheOptions());
            }
            else
            {
                _logger.LogInformation("Cache hit: all products");
            }

            var productsWithShorterDescrption= new List<ProductDto>();
            foreach (var product in allProducts) 
            {
                productsWithShorterDescrption.Add(new ProductDto
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Category = product.Category,
                    Description = !String.IsNullOrWhiteSpace(product.Description) && product.Description.Length > 100
                                ? product.Description.Substring(0, 100) + "..."
                                : product.Description,
                    ImageUrl = product.ImageUrl
                });
            }
            return productsWithShorterDescrption;
        }

        public async Task<Product?> GetProductById(int productId)
        {
            if (productId <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}. Product ID must be greater than zero.", productId);
                throw new ArgumentException("Product ID must be greater than zero.");
            }

            if (_memoryCache.TryGetValue($"product_{productId}", out Product? cachedProduct))
            {
                _logger.LogInformation("Returning cached product with id: {productId}", productId);
                return cachedProduct;
            }

            var product = await _productRepository.GetProductById(productId);

            if (product != null)
            {
                _logger.LogInformation("Caching product with id:{productId} for 5 minutes", productId);
                _memoryCache.Set($"product_{productId}", product, DefaultCacheOptions());
            }

            return product;
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAndPrice(string? category, decimal? minPrice, decimal? maxPrice)
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

            List<ProductDto> products;
            if (string.IsNullOrWhiteSpace(category))
            {
                if (!_memoryCache.TryGetValue("allProducts", out List<ProductDto>? allProducts) || allProducts == null)
                {
                    products = await _productRepository.GetAllProducts();
                    _logger.LogInformation("Caching products for 5 minutes");
                    _memoryCache.Set("allProducts", products, DefaultCacheOptions());
                }
                else
                {
                    _logger.LogInformation("Cache hit: all products");
                    products = allProducts;
                }
            }
            else
            {
                if (!_memoryCache.TryGetValue($"category_{category}", out List<ProductDto>? categoryProducts) || categoryProducts == null)
                {
                    products = await _productRepository.GetProductsByCategory(category);
                    _logger.LogInformation("Caching products for category: {Category} for 5 minutes", category);
                    _memoryCache.Set($"category_{category}", products, DefaultCacheOptions());
                }
                else
                {
                    _logger.LogInformation("Returning cached products for category: {Category}", category);
                    products = categoryProducts;
                }
            }

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();

            _logger.LogInformation("Retrieved {ProductCount} products for category '{Category}' with price range {MinPrice} - {MaxPrice}.", products.Count, category, minPrice, maxPrice);

            return products;
        }

        public async Task<List<ProductDto>> GetProductsByName(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                _logger.LogWarning("Search term cannot be null or whitespace.");
                throw new ArgumentException("Product name cannot be null or whitespace.", nameof(search));
            }

            if (_memoryCache.TryGetValue($"search_{search}", out List<ProductDto>? cachedProducts) && cachedProducts != null)
            {
                _logger.LogInformation("Returning cached products for search: {Search}", search);
                return cachedProducts;
            }

            var products = await _productRepository.GetProductsByName(search);
            _logger.LogInformation("Caching products for search: {Search} for 5 minutes", search);
            _memoryCache.Set($"search_{search}", products, DefaultCacheOptions());

            return products;
        }
    }
}
