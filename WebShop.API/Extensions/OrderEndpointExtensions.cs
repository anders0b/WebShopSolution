using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions;

public static class OrderEndpointExtensions
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/orders").WithDisplayName("Order Management");


        group.MapGet("", async (IOrderService orderService) =>
        {
            var orders = await orderService.GetAllOrders();
            return Results.Ok(orders);
        });

        group.MapGet("{id}", async (IOrderService orderService, int id) =>
        {
            if (id != 0)
            {
                var order = await orderService.GetOrderById(id);
                return order is not null ? Results.Ok(order) : Results.NotFound();
            }
            return Results.Problem();
        });
        group.MapPost("", async (IOrderService orderService, int orderId, List<int> productIds) =>
        {
            if (orderId != 0 || productIds.Count != 0)
            {
                await orderService.AddProductsToOrder(orderId, productIds);
                return Results.Ok($"Products added to order {orderId}");
            }
            return Results.Problem();
        });
        group.MapDelete("{Id}", async (IOrderService orderService, int id) =>
        {
            if(id != 0)
            {
                await orderService.RemoveOrder(id);
                return Results.Ok($"Removed order {id}");
            }
            return Results.NotFound();
        });

        group.MapPut("", async (IOrderService orderService, Order order) =>
        {
            if (order != null)
            {
                await orderService.UpdateOrder(order);
                return Results.Ok($"Update order {order}");
            }
            return Results.NotFound();
        });

        group.MapPatch("{id}/{isShipped}", async (IOrderService orderService, int id, bool isShipped) =>
        {
            if(id != 0)
            {
                await orderService.UpdateOrderStatus(id, isShipped);
                return Results.Ok($"Shipping status is now: {isShipped}");
            }
            return Results.NotFound();
        });


        return app;
    }
}
