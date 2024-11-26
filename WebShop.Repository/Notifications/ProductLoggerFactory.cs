using Microsoft.Extensions.Logging;

namespace WebShop.Repository.Notifications;

//Factory Method Pattern - Implementing INotificationObserver

public class ProductLoggerFactory : INotificationObserverFactory
{
    private readonly ILoggerFactory _logger;
    public ProductLoggerFactory(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory;
    }

    public INotificationObserver CreateNotificationObserver()
    {
        return new ProductLogger(_logger);
    }
}
