using Abysalto.API.DTOs;
using System.Net;
using Abysalto.API.Models.Products;
using Abysalto.API.DTOs.Products;
using Microsoft.Extensions.Caching.Memory;

namespace Abysalto.API.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(HttpClient httpClient, IMemoryCache memoryCache, ILogger<ProductRepository> logger)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private static MemoryCacheEntryOptions DefaultCacheOptions() =>
            new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        public async Task<List<ProductDto>> GetAllProducts()
        {
            if (_memoryCache.TryGetValue("allProducts", out List<ProductDto>? cachedProducts) && cachedProducts != null)
            {
                _logger.LogInformation("Cache hit: all products");
                return cachedProducts;
            }

            var response = await _httpClient.GetAsync("products?limit=0");
            response.EnsureSuccessStatusCode();

            var productsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            var products = productsWrapper?.Products ?? new List<ProductDto>();

            if (products.Count > 0)
            {
                _logger.LogInformation("Caching products for 5 minutes");
                _memoryCache.Set("allProducts", products, DefaultCacheOptions());
            }

            return products;
        }

        public async Task<Product?> GetProductById(int productId)
        {
            if (_memoryCache.TryGetValue($"product_{productId}", out Product? cachedProduct) && cachedProduct != null)
            {
                _logger.LogInformation("Returning cached product with id: {productId}", productId);
                return cachedProduct;
            }

            var response = await _httpClient.GetAsync($"products/{productId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Product not found");
                return null;
            }
            response.EnsureSuccessStatusCode();

            var product = await response.Content.ReadFromJsonAsync<Product>();
            if (product != null)
            {
                _logger.LogInformation("Caching product with id:{productId} for 5 minutes", productId);
                _memoryCache.Set($"product_{productId}", product, DefaultCacheOptions());
            }

            return product ?? null;
        }

        public async Task<List<ProductDto>> GetProductsByCategory(string category)
        {
            if (_memoryCache.TryGetValue($"category_{category}", out List<ProductDto>? cachedProducts) && cachedProducts != null)
            {
                _logger.LogInformation("Returning cached products for category: {Category}", category);
                return cachedProducts;
            }

            var response = await _httpClient.GetAsync($"products/category/{category}");
            response.EnsureSuccessStatusCode();

            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            var products = filteredProductsWrapper?.Products ?? new List<ProductDto>();

            if (products.Count > 0)
            {
                _logger.LogInformation("Caching products for category: {Category} for 5 minutes", category);
                _memoryCache.Set($"category_{category}", products, DefaultCacheOptions());
            }

            return products;
        }

        public async Task<List<ProductDto>> GetProductsByName(string search)
        {
            if (_memoryCache.TryGetValue($"search_{search}", out List<ProductDto>? cachedProducts) && cachedProducts != null)
            {
                _logger.LogInformation("Returning cached products for search: {Search}", search);
                return cachedProducts;
            }

            var response = await _httpClient.GetAsync($"products/search?q={search}");
            response.EnsureSuccessStatusCode();

            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            var products = filteredProductsWrapper?.Products ?? new List<ProductDto>();

            if (products.Count > 0)
            {
                _logger.LogInformation("Caching products for search: {Search} for 5 minutes", search);
                _memoryCache.Set($"search_{search}", products, DefaultCacheOptions());
            }

            return products;
        }
    }
}
