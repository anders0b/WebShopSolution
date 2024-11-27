namespace WebShop.Repository.Notifications
{
    // Gränssnitt för notifieringsobservatörer enligt Observer Pattern
    public interface INotificationObserver
    {
        Task Update<T>(T entity); // Metod som kallas när en nytt objekt läggs till
    }
}
