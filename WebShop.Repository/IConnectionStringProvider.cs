using Microsoft.Extensions.Configuration;

namespace WebShop.Repository
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string name);
    }
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;
        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name) ?? default!;
        }
    }
}
