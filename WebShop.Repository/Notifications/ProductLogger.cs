using Microsoft.Extensions.Logging;
using Repository.Models;

namespace WebShop.Repository.Notifications;

public class ProductLogger : INotificationObserver
{
    private readonly ILogger<ProductLogger> _logger;
    public ProductLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ProductLogger>();
    }
    public Task Update(Product product)
    {
        _logger.LogInformation($"Product added: {product.Name}");
        return Task.CompletedTask;
    }
}
