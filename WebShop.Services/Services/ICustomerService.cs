using Microsoft.Extensions.Logging;
using Repository.Models;
using WebShop.Repository.Notifications.Factory;
using WebShop.Repository.Repository;

namespace WebShop.Services.Services;

public interface ICustomerService
{
    Task<Customer> GetCustomerById(int id);
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task AddCustomer(Customer customer);
    Task RemoveCustomer(int id);
    Task UpdateCustomer(Customer customer);
    Task UpdateCustomerEmail(int id, string email);
    Task UpdateCustomerPhone(int id, string phone);
    Task<Customer> GetCustomerFromOrder(int orderId);
}
public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerFactory _loggerFactory;
    public CustomerService(IUnitOfWork unitOfWork, ILoggerFactory loggerfactory)
    {
        _unitOfWork = unitOfWork;
        _loggerFactory = loggerfactory;
    }

    public async Task AddCustomer(Customer customer)
    {
        await _unitOfWork.AttachObserver(new EmailNotificationFactory());
        await _unitOfWork.AttachObserver(new SMSNotificationFactory());
        await _unitOfWork.AttachObserver(new LoggerFactory(_loggerFactory));
        if (customer.Email == null || customer.Phone == null)
        {
            throw new Exception("Email and Phone cannot be null");
        }
        await _unitOfWork.Customers.Add(customer);
        await _unitOfWork.SaveChangesAsync();
        await _unitOfWork.NotifyCustomerAdded(customer);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        return await _unitOfWork.Customers.GetAll();
    }

    public async Task<Customer> GetCustomerById(int id)
    {
        return await _unitOfWork.Customers.GetById(id);
    }

    public async Task<Customer> GetCustomerFromOrder(int orderId)
    {
        return await _unitOfWork.Customers.GetCustomerFromOrder(orderId);
    }

    public async Task RemoveCustomer(int id)
    {
        await _unitOfWork.Customers.Remove(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomer(Customer customer)
    {
        await _unitOfWork.Customers.Update(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomerEmail(int id, string email)
    {
        await _unitOfWork.Customers.UpdateCustomerEmail(id, email);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomerPhone(int id, string phone)
    {
        await _unitOfWork.Customers.UpdateCustomerPhone(id, phone);
        await _unitOfWork.SaveChangesAsync();
    }
}
