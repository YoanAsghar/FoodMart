using System.Security.Claims;
using Restaurant_Application.Models;
using Microsoft.EntityFrameworkCore;
using Restaurant_Application.Data;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Application.Routes
{
    public static class OrderRoutes
    {
        public static void MapOrderRoutes(this IEndpointRouteBuilder app)
        {
            var OrderRoutes = app.MapGroup("/api/orders").WithTags("Orders");

            //Get all orders created 
            OrderRoutes.MapGet("/", async (ApplicationDbContext db, [FromQuery] int PageSize = 20, [FromQuery] int Page = 1) =>
            {
                try
                {
                    var skip = (Page - 1) * PageSize;


                    var Orders = await db.Orders
                    .OrderBy(o => o.OrderId)
                    .Skip(skip)
                    .Take(PageSize)
                    .ToListAsync();

                    var TotalOrders = await db.Orders.CountAsync();

                    var Response = new
                    {
                        TotalOrders = TotalOrders,
                        PagenNumber = Page,
                        PageSize = PageSize,
                        Data = Orders
                    };


                    return Results.Ok(Response);
                }
                catch (Exception ex)
                {
                    return Results.Conflict(ex);
                }
            }).RequireAuthorization("AdminOnly");

            OrderRoutes.MapGet("/me", async (ApplicationDbContext db, ClaimsPrincipal user, [FromQuery] string? status) =>
            {
                try
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Results.Unauthorized();
                    }

                    IQueryable<Order> ordersQuery = db.Orders.Where(u => u.UserId == userId);

                    if (!string.IsNullOrEmpty(status))
                    {
                        ordersQuery = ordersQuery.Where(o => o.Status.ToLower() == status.ToLower());
                    }

                    var orders = await ordersQuery
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ToListAsync();

                    return Results.Ok(orders);
                }
                catch (Exception ex)
                {
                    return Results.Conflict(ex);
                }

            }).RequireAuthorization();

            //Create a new order 
            OrderRoutes.MapPost("/{id}", async (ApplicationDbContext db, ClaimsPrincipal user, [FromQuery] string? status) =>
            {
                try
                {
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Conflict(ex);
                }
            }).RequireAuthorization();

            //Delete order
            OrderRoutes.MapDelete("/{id}", (ApplicationDbContext db, ClaimsPrincipal user) =>
            {
                try
                {
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }).RequireAuthorization("AdminOnly");
        }
    }
}
