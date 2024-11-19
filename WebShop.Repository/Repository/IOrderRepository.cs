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
        private readonly IDbTransaction _transaction;
        public OrderRepository(IDbConnection connectionString, IDbTransaction transaction) : base(connectionString, transaction)
        {
            _connectionString = connectionString;
            _transaction = transaction;
        }
    }
}
