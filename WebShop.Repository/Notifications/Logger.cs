using Microsoft.Extensions.Logging;
using Repository.Models;

namespace WebShop.Repository.Notifications;

//Factory Method Pattern - ProductLogger implementing INotificationObserver
public class Logger : INotificationObserver
{
    private readonly ILogger<Logger> _logger;

    public Logger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Logger>();
    }

    public Task Update<T>(T Entity)
    {
        if (Entity is Product product)
        {
            Console.WriteLine($"Notification: New product added - {product.Name}");
            return Task.CompletedTask;

        }
        else if (Entity is Order order)
        {
            Console.WriteLine($"Notification: New order added - {order.Id}");
            return Task.CompletedTask;
        }
        else if (Entity is Customer customer)
        {
            Console.WriteLine($"Notification: New customer created - {customer.FirstName} {customer.LastName}");
            return Task.CompletedTask;
        }
        else
        {
            throw new ArgumentException("Invalid entity type");
        }
    }
}
