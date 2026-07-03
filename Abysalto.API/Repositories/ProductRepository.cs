using Abysalto.API.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Abysalto.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public ProductRepository(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            if(_memoryCache.TryGetValue("allProducts", out List<Product> cachedProducts))
            {
                Console.WriteLine(cachedProducts.Count);
                return cachedProducts;
            }
            var response = await _httpClient.GetAsync("products?limit=0");
            response.EnsureSuccessStatusCode();
            var productsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Console.WriteLine("nije");
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            _memoryCache.Set("allProducts", productsWrapper?.Products ?? new List<Product>(), cacheEntryOptions);
            return productsWrapper?.Products ?? new List<Product>();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            if(_memoryCache.TryGetValue($"product_{productId}", out Product cachedProduct))
            {
                Console.WriteLine("pogodeno");
                return cachedProduct;
            }
            var response = await _httpClient.GetAsync($"products/{productId}");
            response.EnsureSuccessStatusCode();
            var product=await response.Content.ReadFromJsonAsync<Product>();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            Console.WriteLine("nije");
            _memoryCache.Set($"product_{productId}", product ?? new Product(), cacheEntryOptions);
            return product ?? new Product();
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            if(_memoryCache.TryGetValue($"category_{category}", out List<Product> cachedProducts))
            {
                Console.WriteLine("pogodeno");
                return cachedProducts;
            }
            var response = await _httpClient.GetAsync($"products/category/{category}");
            response.EnsureSuccessStatusCode();
            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            Console.WriteLine("nije");
            _memoryCache.Set($"category_{category}", filteredProductsWrapper?.Products ?? new List<Product>(), cacheEntryOptions);
            return filteredProductsWrapper?.Products ?? new List<Product>();
        }

        public async Task<List<Product>> GetProductsByName(string search)
        {
            if(_memoryCache.TryGetValue($"search_{search}", out List<Product> cachedProducts))
            {
                Console.WriteLine("pogodeno");
                return cachedProducts;
            }
            var response =await _httpClient.GetAsync($"products/search?q={search}");
            response.EnsureSuccessStatusCode();
            var filteredProductsWrapper = await response.Content.ReadFromJsonAsync<ProductResponse>();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            Console.WriteLine("nije");
            _memoryCache.Set($"search_{search}", filteredProductsWrapper?.Products ?? new List<Product>(), cacheEntryOptions);
            return filteredProductsWrapper?.Products ?? new List<Product>();
        }
    }
}
