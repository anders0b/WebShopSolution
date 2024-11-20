﻿using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Services.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrders();
    Task AddProductsToOrder(int orderId, List<int> productIds);
    Task<Order> GetOrderById(int id);
    Task RemoveOrder(int id);
    Task UpdateOrder(Order order);
    Task UpdateOrderStatus(int id, bool isShipped);
}
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddProductsToOrder(int orderId, List<int> productIds)
    {
        if (orderId == 0 || productIds.Count == 0)
        {
            throw new Exception("Order Id and Products cannot be null");
        }
        await _unitOfWork.Orders.AddProductsToOrder(orderId, productIds);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _unitOfWork.Orders.GetAll();
    }

    public async Task<Order> GetOrderById(int id)
    {
        var order = await _unitOfWork.Orders.GetById(id);
        var orderProducts = await _unitOfWork.Products.GetAllProductsFromOrder(id);
        var customer = await _unitOfWork.Customers.GetCustomerFromOrder(id);
        order.Customer = customer;
        order.Products = (List<Product>)orderProducts;
        return order;
    }

    public async Task RemoveOrder(int id)
    {
        await _unitOfWork.Orders.Remove(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateOrder(Order order)
    {
        await _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateOrderStatus(int id, bool isShipped)
    {
        await _unitOfWork.Orders.UpdateOrderStatus(id, isShipped);
        await _unitOfWork.SaveChangesAsync();
    }

}
