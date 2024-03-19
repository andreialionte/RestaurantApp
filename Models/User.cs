using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
