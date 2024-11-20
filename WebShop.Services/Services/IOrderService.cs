using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Services.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrders();
    Task<Order> GetOrderById(int id);
    Task AddOrder(Order order);
    Task RemoveOrder(Order order);
    Task UpdateOrder(Order order);
    Task UpdateOrderStatus(int id, bool isShipped);
    Task<IEnumerable<Product>> GetAllProductsFromOrder(int orderId);
    Task<Customer> GetCustomerFromOrderId(int orderId);
}
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddOrder(Order order)
    {
        if (order.Customer.Id == 0 || order.Products.Count == 0)
        {
            throw new Exception("Customer and Products cannot be null");
        }
        await _unitOfWork.Orders.Add(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _unitOfWork.Orders.GetAll();
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await _unitOfWork.Orders.GetById(id);
    }

    public async Task RemoveOrder(Order order)
    {
        await _unitOfWork.Orders.Remove(order);
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
    public async Task<IEnumerable<Product>> GetAllProductsFromOrder(int orderId)
    {
        return await _unitOfWork.Orders.GetAllProductsFromOrder(orderId);
    }
    public async Task<Customer> GetCustomerFromOrderId(int orderId)
    {
        return await _unitOfWork.Orders.GetCustomerFromOrderId(orderId);
    }
}
