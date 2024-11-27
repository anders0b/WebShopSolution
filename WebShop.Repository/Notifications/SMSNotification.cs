using Repository.Models;

namespace WebShop.Repository.Notifications;

public class SMSNotification : INotificationObserver
{
    public Task Update<T>(T Entity)
    {
        if(Entity is Product product)
        {
            Console.WriteLine($"SMS Notification: New product added - {product.Name}");
            return Task.CompletedTask;
        }
        else if(Entity is Order order)
        {
            Console.WriteLine($"SMS Notification: New order added - {order.Id}");
            return Task.CompletedTask;
        }
        else if(Entity is Customer customer)
        {
            Console.WriteLine($"SMS Notification: New customer created - {customer.FirstName} {customer.LastName}");
            return Task.CompletedTask;
        }
        else
        {
            throw new ArgumentException("Invalid entity type");
        }
    }
}
