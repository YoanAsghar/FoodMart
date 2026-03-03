using System.Collections.Generic;

namespace Restaurant_Application.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation property for related Product entities
        public List<Product> Products { get; set; } = new();
    }
}
