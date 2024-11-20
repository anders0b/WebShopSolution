using WebShop.Services.Services;

namespace WebShop.API.Extensions;

public static class OrderEndpointExtensions
{
    public static IEndPointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/orders").WithDisplayName("Order Management");

        group.MapGet("", async (IOrderService orderService) =>
        {
            var orders = await orderService.GetAllOrders();
            return Results.Ok(orders);
        });
        return app;
    }
}
