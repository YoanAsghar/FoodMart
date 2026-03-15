using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Restaurant_Application.Models;
using Microsoft.EntityFrameworkCore;
using Restaurant_Application.Data;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Application.Routes
{
    public static class AuthRoutes
    {
        public static string GenerateToken(User user, IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT SecretKey is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static void MapAuthRoutes(this IEndpointRouteBuilder app)
        {
            var AuthRoutes = app.MapGroup("/api/auth");
            var passwordHash = new PasswordHasher<User>();


            AuthRoutes.MapPost("/register", async ([FromBody] User user, ApplicationDbContext db, IConfiguration config) =>
                {
                    try
                    {
                        User? CheckIfUserExists = await db.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == user.Email.ToLower());
                        if (CheckIfUserExists != null)
                        {
                            return Results.BadRequest("User already exists");
                        }
                        var isFirst = !await db.Users.AnyAsync();
                        user.Role = isFirst ? "Admin" : "User";

                        user.RegisterDate = DateTime.UtcNow;
                        user.PasswordHash = passwordHash.HashPassword(user, user.PasswordHash);

                        db.Users.Add(user);
                        await db.SaveChangesAsync();

                        return Results.Ok(new { token = GenerateToken(user, config) });
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex);
                    }
                }).WithTags("Auth");

            AuthRoutes.MapPost("/login", async ([FromBody] User user, ApplicationDbContext db, IConfiguration config) =>
            {
                try
                {
                    User? CheckIfUserExists = await db.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == user.Email.ToLower());

                    if (CheckIfUserExists == null)
                    {
                        return Results.BadRequest("Invalid credentials");
                    }

                    var result = passwordHash.VerifyHashedPassword(CheckIfUserExists, CheckIfUserExists.PasswordHash, user.PasswordHash);

                    if (result == PasswordVerificationResult.Failed)
                    {
                        return Results.BadRequest("Invalid credentials");
                    }
                    return Results.Ok(new { token = GenerateToken(CheckIfUserExists, config) });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex);
                }
            }).WithTags("Auth");

            AuthRoutes.MapGet("/verify", (HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

                return Results.Ok(new { userId, email, role });
            }).RequireAuthorization().WithTags("Auth");
        }

    }

}
