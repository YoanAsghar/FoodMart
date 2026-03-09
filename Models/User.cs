using Microsoft.AspNetCore.Identity;

namespace Restaurant_Application.Models;

public class User : IdentityUser
{
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = string.Empty;
}
