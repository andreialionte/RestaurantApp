namespace RestaurantApp.DTOs
{
    public class OrderDto
    {
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public List<int>? SelectedItemIds { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? StripeToken { get; set; }
    }
}