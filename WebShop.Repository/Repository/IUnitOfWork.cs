using Repository.Models;
using Repository.Repository;
using System.Data;
using WebShop.Repository.Notifications;
using WebShop.Repository.Notifications.Factory;

namespace WebShop.Repository.Repository;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task SaveChangesAsync();
    Task AttachObserver(INotificationObserverFactory factory);
    Task DetachObserver(INotificationObserver observer);
    Task NotifyProductAdded(Product product);
    Task NotifyCustomerAdded(Customer customer);
    Task NotifyOrderAdded(Order order);
}
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;
    public ICustomerRepository Customers{ get; }
    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }

    private readonly EntitySubject<object> _entitySubject;

    //Konstruktor används för tillfället av Observer pattern
    public UnitOfWork(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
        Customers = new CustomerRepository(connection, transaction);
        Products = new ProductRepository(connection, transaction);
        Orders = new OrderRepository(connection, transaction);

        // Om inget ProductSubject injiceras, skapa ett nytt
        _entitySubject = new EntitySubject<object>();

        // Registrera standardobservatörer
        _entitySubject.Attach(new EmailNotificationFactory());
    }
    public async Task AttachObserver(INotificationObserverFactory factory)
    {
        await _entitySubject.Attach(factory);
    }
    public async Task DetachObserver(INotificationObserver observer)
    {
        await _entitySubject.Detach(observer);
    }

    public async Task NotifyProductAdded(Product product)
    {
        await _entitySubject.Notify(product);
    }
    public async Task NotifyCustomerAdded(Customer customer)
    {
        await _entitySubject.Notify(customer);
    }
    public async Task NotifyOrderAdded(Order order)
    {
        await _entitySubject.Notify(order);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }

    public async Task SaveChangesAsync()
    {
        if(_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }
        try
        {
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
        }
        finally
        {
            _connection.Close();
        }
    }
}

