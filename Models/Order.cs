namespace Restaurant_Application.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string Status { get; set; } = string.Empty;

        // Navigation property for related OrderDetail entities
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
