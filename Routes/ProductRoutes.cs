using Restaurant_Application.Models;
using Microsoft.EntityFrameworkCore;
using Restaurant_Application.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Restaurant_Application.Routes
{
    public static class ProductRoutes
    {
        public static void MapProductRoutes(this IEndpointRouteBuilder app)
        {
            var Group = app.MapGroup("/api/products");

            Group.MapGet("/", async (ApplicationDbContext db, [FromQuery] int PageSize = 20, [FromQuery] int Page = 1) =>
            {
                try
                {
                    var Size = (Page - 1) * PageSize;
                    var Products = await db.Products
                    .OrderBy(o => o.Id)
                    .Skip(Size)
                    .Take(PageSize)
                    .ToListAsync();


                    var Response = new
                    {
                        TotalOrders = await db.Products.CountAsync(),
                        PageNumber = Page,
                        PageSize = PageSize,
                        Data = Products
                    };
                    return Results.Ok(Response);
                }
                catch (Exception ex)
                {
                    return Results.Conflict("Error retrieving products" + ex);
                }
            }).WithTags("Products");
            Group.MapGet("/{id}", async (ApplicationDbContext db, int id) =>
                {
                    try
                    {
                        var Product = await db.Products.FirstOrDefaultAsync(c => c.Id == id);
                        if (Product == null)
                        {
                            return Results.NotFound($"Product with id {id} not found");
                        }

                        return Results.Ok(Product);
                    }
                    catch (System.Exception)
                    {
                        return Results.Problem("Error retrieving the product");
                    }
                }).WithTags("Products");

            Group.MapPost("/", async (ApplicationDbContext db, [FromBody] Product product) =>
            {
                try
                {
                    db.Products.Add(product);
                    await db.SaveChangesAsync();

                    return Results.Ok(product);
                }
                catch (Exception ex)
                {
                    return Results.Conflict(ex);
                }
            }).WithTags("Products").RequireAuthorization("AdminOnly");

            Group.MapPut("/{id}", async (ApplicationDbContext db, int id, [FromBody] Product product) =>
                    {
                        try
                        {
                            var ExistingProduct = await db.Products.FirstOrDefaultAsync(c => c.Id == id);
                            if (ExistingProduct == null)
                            {
                                return Results.NotFound();
                            }

                            ExistingProduct.Name = product.Name;
                            ExistingProduct.Price = product.Price;
                            ExistingProduct.Description = product.Description;
                            ExistingProduct.ImageUrl = product.ImageUrl;

                            await db.SaveChangesAsync();
                            return Results.Ok(ExistingProduct);
                        }
                        catch (Exception ex)
                        {
                            return Results.Conflict(ex);
                        }
                    }).WithTags("Products").RequireAuthorization("AdminOnly");

            Group.MapDelete("/{id}", async (int id, ApplicationDbContext db) =>
            {
                try
                {
                    var ProductToDelete = await db.Products.FirstOrDefaultAsync(c => c.Id == id);
                    if (ProductToDelete == null)
                    {
                        return Results.NotFound();
                    }

                    db.Products.Remove(ProductToDelete);
                    await db.SaveChangesAsync();

                    return Results.Ok(ProductToDelete);

                }
                catch (Exception ex)
                {
                    return Results.Conflict(ex);
                }
            }).WithTags("Products").RequireAuthorization("AdminOnly");
        }
    }
}
