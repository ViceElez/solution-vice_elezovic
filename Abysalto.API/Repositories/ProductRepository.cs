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

        public Task<Product?> GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductsByCategoryAndPrice(string category, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
