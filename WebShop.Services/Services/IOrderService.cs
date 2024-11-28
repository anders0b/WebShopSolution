using Microsoft.Extensions.Logging;
using Repository.Models;
using WebShop.Repository.Models;
using WebShop.Repository.Notifications.Factory;
using WebShop.Repository.Repository;
using WebShop.Services.DTO;

namespace WebShop.Services.Services;

public interface IOrderService
{
    Task CreateOrder(Order order);
    Task<IEnumerable<Order>> GetAllOrders();
    Task AddProductsToOrder(int orderId, List<AddProductsToOrderRequest> products);
    Task<Order> GetOrderById(int id);
    Task RemoveOrder(int id);
    Task UpdateOrder(Order order);
    Task UpdateOrderStatus(int id, bool isShipped);
    Task AddCustomerToOrder(int orderId, int customerId);
}
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerFactory _loggerFactory;
    public OrderService(IUnitOfWork unitOfWork, ILoggerFactory loggerfactory)
    {
        _unitOfWork = unitOfWork;
        _loggerFactory = loggerfactory;
    }

    public async Task AddCustomerToOrder(int orderId, int customerId)
    {
        var existingOrder = await _unitOfWork.Orders.GetById(orderId);
        var existingCustomer = await _unitOfWork.Customers.GetById(customerId);
        if (existingOrder != null && existingCustomer != null)
        {
            await _unitOfWork.Orders.AddCustomerToOrder(orderId, customerId);
            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Order or Customer does not exist");
        }
    }

    public async Task AddProductsToOrder(int orderId, List<AddProductsToOrderRequest> products)
    {
        var existingOrder = await _unitOfWork.Orders.GetById(orderId);
        var orderItems = new List<OrderItem>();
        {
            if (existingOrder != null)
            {
                foreach (var product in products)
                {
                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.ProductId,
                        Quantity = product.Quantity
                    });
                }
                await _unitOfWork.Orders.AddProductsToOrder(orderId, orderItems);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }

    public async Task CreateOrder(Order order)
    {
        await _unitOfWork.AttachObserver(new EmailNotificationFactory());
        await _unitOfWork.AttachObserver(new LoggerFactory(_loggerFactory));
        if (order != null)
        {
            await _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.NotifyOrderAdded(order);
        }
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _unitOfWork.Orders.GetAll();
    }

    public async Task<Order> GetOrderById(int id)
    {
        var order = await _unitOfWork.Orders.GetById(id);
        if (order != null)
        {
            return order;
        }
        else
        {
            throw new Exception("Order does not exist");
        }
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
