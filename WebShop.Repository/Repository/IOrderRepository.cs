using Dapper;
using Repository.Models;
using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task AddProductsToOrder(int orderId, List<int> productIds);
    Task UpdateOrderStatus(int orderId, bool isShipped);
}
public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly IDbConnection _connectionString;
    private IDbTransaction _transaction;
    public OrderRepository(IDbConnection connectionString, IDbTransaction transaction) : base(connectionString, transaction)
    {
        _connectionString = connectionString;
        _transaction = transaction;
    }
    public async Task AddProductsToOrder(int orderId, List<int> productIds)
    {
        var tableName = "OrderItems";

        var sql = $"INSERT INTO {tableName} (OrderId, CustomerId) VALUES (@OrderId, @CustomerId);";

        var orderItems = productIds.Select(productId => new { OrderId = orderId, ProductId = productId });

        await _connectionString.ExecuteAsync(sql, orderItems, transaction: _transaction);
    }

    public async Task UpdateOrderStatus(int orderId, bool isShipped)
    {
        var tableName = "Orders";

        var sql = $"UPDATE {tableName} SET IsShipped = @IsShipped WHERE Id = @OrderId";
        await _connectionString.ExecuteAsync(sql, new { IsShipped = isShipped, OrderId = orderId }, transaction: _transaction);
    }

}
