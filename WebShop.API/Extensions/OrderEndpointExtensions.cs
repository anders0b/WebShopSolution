using Repository.Models;
using WebShop.Services.DTO;
using WebShop.Services.Services;

namespace WebShop.API.Extensions;

public static class OrderEndpointExtensions
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/orders").WithDisplayName("Order Management");
        group.MapPost("", CreateOrder).WithSummary("Create order");
        group.MapGet("", GetAllOrders).WithSummary("Get all orders");
        group.MapGet("{id}", GetOrderById).WithSummary("Get order by id");
        group.MapPost("{orderId}", AddProductsToOrder).WithSummary("Add products to order");
        group.MapDelete("{Id}", RemoveOrder).WithSummary("Remove order");
        group.MapPut("", UpdateOrder).WithSummary("Update order");
        group.MapPatch("{orderId}", AddCustomerToOrder).WithSummary("Add customer to order");
        group.MapDelete("{orderId}/customer", RemoveCustomerFromOrder).WithSummary("Remove customer from order");
        group.MapDelete("{orderId}/products", RemoveProductsFromOrder).WithSummary("Remove products from order");
        group.MapPatch("{id}/{isShipped}", UpdateOrderStatus).WithSummary("Update shipping status");
        return app;
    }
    public static async Task<IResult> CreateOrder(IOrderService orderService, Order order)
    {
        if (order != null)
        {
            await orderService.CreateOrder(order);
            return Results.Ok($"Order {order.Id} created");
        }
        return Results.Problem();
    }
    public static async Task<IResult> GetAllOrders(IOrderService orderService)
    {
        var orders = await orderService.GetAllOrders();
        return Results.Ok(orders);
    }
    public static async Task<IResult> GetOrderById(IOrderService orderService, int id)
    {
        if (id != 0)
        {
            var existingOrder = await orderService.GetOrderById(id);
            if (existingOrder != null)
            {
                return Results.Ok(existingOrder);
            }
        }
        return Results.BadRequest();
    }
    public static async Task<IResult> AddProductsToOrder(IOrderService orderService, int orderId, List<AddProductsToOrderRequest> products)
    {
        var existingOrder = await orderService.GetOrderById(orderId);
        if (existingOrder.Id != 0)
        {
            await orderService.AddProductsToOrder(orderId, products);
            return Results.Ok($"Products added to order {orderId}");
        }
        return Results.BadRequest();
    }
    public static async Task<IResult> RemoveProductsFromOrder(IOrderService orderService, int orderId)
    {
        var existingOrder = await orderService.GetOrderById(orderId);
        if (existingOrder.Id != 0)
        {
            await orderService.RemoveProductsFromOrder(orderId);
            return Results.Ok($"Removed products from order {orderId}");
        }
        return Results.BadRequest("Please enter a valid Id");
    }
        public static async Task<IResult> RemoveOrder(IOrderService orderService, int id)
    {
        var existingOrder = await orderService.GetOrderById(id);
        if (existingOrder.Id != 0)
        {
            await orderService.RemoveOrder(id);
            return Results.Ok($"Removed order {id}");
        }
        return Results.BadRequest();
    }
    public static async Task<IResult> UpdateOrder(IOrderService orderService, Order order)
    {
        var existingOrder = await orderService.GetOrderById(order.Id);
        if (existingOrder.Id != 0)
        { 
            await orderService.UpdateOrder(order);
            return Results.Ok($"Update order {order}");
        }
        return Results.BadRequest("Please enter a valid order ID");
    }
    public static async Task<IResult> AddCustomerToOrder(IOrderService orderService, int orderId, int customerId)
    {
        if (orderId != 0)
        {
            await orderService.AddCustomerToOrder(orderId, customerId);
            return Results.Ok($"Added customer {customerId} to order {orderId}");
        }
        return Results.BadRequest("Please enter a valid order ID");
    }
    public static async Task<IResult> RemoveCustomerFromOrder(IOrderService orderService, int orderId)
    {
        if (orderId != 0)
        {
            await orderService.RemoveCustomerFromOrder(orderId);
            return Results.Ok($"Removed customer from order {orderId}");
        }
        return Results.BadRequest("Please enter a valid order ID");
    }

    public static async Task<IResult> UpdateOrderStatus(IOrderService orderService, int id, bool isShipped)
    {
        var existingOrder = await orderService.GetOrderById(id);
        if (existingOrder.Id != 0)
        {
            await orderService.UpdateOrderStatus(id, isShipped);
            return Results.Ok($"Shipping status is now: {isShipped}");
        }
        return Results.BadRequest();
    }

}
