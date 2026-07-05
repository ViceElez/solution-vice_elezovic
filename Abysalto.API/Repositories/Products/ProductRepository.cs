using Abysalto.API.DTOs;
using System.Net;
using Abysalto.API.Models.Products;


namespace Abysalto.API.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(HttpClient httpClient, ILogger<ProductRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("products?limit=0");
            response.EnsureSuccessStatusCode();

            var productsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();

            return productsWrapper?.Products ?? new List<ProductDto>();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            var response = await _httpClient.GetAsync($"products/{productId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Product not found");
                return null;
            }
            response.EnsureSuccessStatusCode();

            var product = await response.Content.ReadFromJsonAsync<Product>();

            return product ?? null;
        }

        public async Task<List<ProductDto>> GetProductsByCategory(string category)
        {
            var response = await _httpClient.GetAsync($"products/category/{category}");
            response.EnsureSuccessStatusCode();

            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();

            return filteredProductsWrapper?.Products ?? new List<ProductDto>();
        }

        public async Task<List<ProductDto>> GetProductsByName(string search)
        {
            var response = await _httpClient.GetAsync($"products/search?q={search}");
            response.EnsureSuccessStatusCode();

            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();

            return filteredProductsWrapper?.Products ?? new List<ProductDto>();
        }
    }
}
