using Repository.Models;
using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        
    }
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly IDbConnection _connectionString;
        public OrderRepository(IDbConnection connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
