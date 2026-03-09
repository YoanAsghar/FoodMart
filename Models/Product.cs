using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Restaurant_Application.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
    }
}
