namespace WebShop.Repository.Notifications.Factory
{
    public class SMSNotificationFactory : INotificationObserverFactory
    {
        public INotificationObserver CreateNotificationObserver()
        {
            return new SMSNotification();
        }
    }
}
