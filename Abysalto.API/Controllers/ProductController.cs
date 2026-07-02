using Microsoft.AspNetCore.Mvc;
using Abysalto.API.Models;
using Abysalto.API.Services;
using System.Diagnostics.Contracts;

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
    }
}
