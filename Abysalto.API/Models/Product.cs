namespace Abysalto.API.Models
{
    public class Product
    {
        int Id { get; set; }
        string Title { get; set; }
        float Price { get; set; }
        string Category { get; set; }
        string Description { get; set; }
        string ImageUrl { get; set; }
    }
}
