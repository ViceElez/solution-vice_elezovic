using Abysalto.API.Models;

namespace Abysalto.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        public ProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("products?limit=0");
            response.EnsureSuccessStatusCode();
            var productsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            return productsWrapper?.Products ?? new List<Product>();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            var response = await _httpClient.GetAsync($"products/{productId}");
            response.EnsureSuccessStatusCode();
            var product=await response.Content.ReadFromJsonAsync<Product>();
            return product ?? new Product();
        }

        public async Task<List<Product>> GetProductsByCategoryAndPrice(string category, decimal price)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetProductsByName(string search)
        {
            var response =await _httpClient.GetAsync($"products/search?q={search}");
            response.EnsureSuccessStatusCode();
            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            return filteredProductsWrapper?.Products ?? new List<Product>();
        }
    }
}
