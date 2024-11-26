using Repository.Models;

namespace WebShop.Repository.Notifications
{
    // Gränssnitt för notifieringsobservatörer enligt Observer Pattern
    public interface INotificationObserver
    {
        Task Update(Product product); // Metod som kallas när en ny produkt läggs till
    }
}
