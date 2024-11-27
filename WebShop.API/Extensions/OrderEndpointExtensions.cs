﻿using Repository.Models;
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
        return Results.NotFound();
    }
    public static async Task<IResult> GetAllOrders(IOrderService orderService)
    {
        var orders = await orderService.GetAllOrders();
        return Results.Ok(orders);
    }
    public static async Task<IResult> GetOrderById(IOrderService orderService, int id)
    {
        if(id != 0)
        {
            var order = await orderService.GetOrderById(id);
            return order is not null ? Results.Ok(order) : Results.NotFound();
        }
        return Results.NotFound();
    }
    public static async Task<IResult> AddProductsToOrder(IOrderService orderService, int orderId, List<int> productIds)
    {
        if (orderId != 0 || productIds.Count != 0)
        {
            await orderService.AddProductsToOrder(orderId, productIds);
            return Results.Ok($"Products added to order {orderId}");
        }
        return Results.NotFound();
    }
    public static async Task<IResult> RemoveOrder(IOrderService orderService, int id)
    {
        if (id != 0)
        {
            await orderService.RemoveOrder(id);
            return Results.Ok($"Removed order {id}");
        }
        return Results.NotFound();
    }
    public static async Task<IResult> UpdateOrder(IOrderService orderService, Order order)
    {
        if (order != null)
        {
            await orderService.UpdateOrder(order);
            return Results.Ok($"Update order {order}");
        }
        return Results.NotFound();
    }
    public static async Task<IResult> UpdateOrderStatus(IOrderService orderService, int id, bool isShipped)
    {
        if (id != 0)
        {
            await orderService.UpdateOrderStatus(id, isShipped);
            return Results.Ok($"Shipping status is now: {isShipped}");
        }
        return Results.NotFound();
    }

}
