using Microsoft.AspNetCore.Mvc;
using Abysalto.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Abysalto.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsByCategoryAndPrice([FromQuery] string? category, [FromQuery] decimal? minPrice,[FromQuery] decimal? maxPrice)
        {
            var products = await _productService.GetProductsByCategoryAndPrice(category,minPrice,maxPrice);
            if (products == null || !products.Any())
                return NotFound();
            return Ok(products);
        }

        [HttpGet("search/{search}")]
        public async Task<IActionResult> GetProductsByName(string search)
        {
            var products = await _productService.GetProductsByName(search);
            if (products == null)
                return NotFound();
            return Ok(products);
        }
    }
}
