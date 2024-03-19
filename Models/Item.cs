using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [MaxLength(50)]
        public string? ItemName { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        /*        [NotMapped] //excludem dim db
                public IFormFile? PhotoFile { get; set; }*/
        [MaxLength]
        public string? PhotoUrl { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
