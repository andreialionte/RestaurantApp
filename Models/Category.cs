using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestaurantApp.Models
{
    public class Category
    {
        //NEW
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(30)]
        public string? Name { get; set; }
        [JsonIgnore]
        public ICollection<Item>? Items { get; set; }
    }
}
