using Microsoft.Extensions.Configuration;
using Repository.Models;

namespace Repository.Repository;

public interface IProductRepository : IRepository<Product>
{
     void UpdateStock(int productId, int quantity);
}
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IConfiguration configuration) : base(configuration)
    {
        
    }
    public void UpdateStock(int productId, int quantity)
    {
        throw new NotImplementedException();
    }
}
