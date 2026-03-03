namespace Restaurant_Application.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Foreign keys and navigation properties
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
