using System;
using System.Collections.Generic;

namespace Restaurant_Application.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }

        // Navigation property for related OrderDetail entities
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
