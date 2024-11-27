using Microsoft.Extensions.Logging;

namespace WebShop.Repository.Notifications.Factory;

//Factory Method Pattern - Implementing INotificationObserver

public class LoggerFactory : INotificationObserverFactory
{
    private readonly ILoggerFactory _logger;
    public LoggerFactory(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory;
    }

    public INotificationObserver CreateNotificationObserver()
    {
        return new Logger(_logger);
    }
}
