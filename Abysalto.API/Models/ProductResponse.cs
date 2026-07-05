using Abysalto.API.DTOs.Products;

namespace Abysalto.API.DTOs
{
    public class ProductResponse
    {
        public List<ProductDto> Products { get; set; } = new();
    }
}
