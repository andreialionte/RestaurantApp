using RestaurantApp.Models;

namespace RestaurantApp.DTOs
{
    public class ItemDto
    {
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
