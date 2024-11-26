
namespace WebShop.Repository.Notifications;

//Factory Method Pattern - EmailNotificationFactory implementing INotificationObserverFactory

public class EmailNotificationFactory : INotificationObserverFactory
{
    public INotificationObserver CreateNotificationObserver()
    {
        return new EmailNotification();
    }
}
