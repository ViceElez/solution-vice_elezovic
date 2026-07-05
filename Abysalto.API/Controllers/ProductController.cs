using Microsoft.AspNetCore.Mvc;
using Abysalto.API.Services;

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
            try
            {
                var product = await _productService.GetProductById(productId);
                if (product == null) 
                { 
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsByCategoryAndPrice([FromQuery] string? category, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAndPrice(category, minPrice, maxPrice);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search/{search}")]
        public async Task<IActionResult> GetProductsByName(string search)
        {
            try
            {
                var products = await _productService.GetProductsByName(search);
                return Ok(products);

            }
            catch (Exception ex)
            {
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
