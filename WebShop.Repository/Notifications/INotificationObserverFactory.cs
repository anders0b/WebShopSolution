namespace WebShop.Repository.Notifications;

public interface INotificationObserverFactory
{
    INotificationObserver CreateNotificationObserver();
}
