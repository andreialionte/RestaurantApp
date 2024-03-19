using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("UserId")]
        public int? CustomerId { get; set; }
        public User? User { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        [MaxLength(20)]
        public string? Status { get; set; }
        public DateTime? OrderDate { get; set; }
        [ForeignKey("PaymentId")]
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
