using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class Auth
    {
        [Key]
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength]
        public byte[]? PasswordSalt { get; set; }
        [MaxLength]
        public byte[]? PasswordHash { get; set; }
    }
}
