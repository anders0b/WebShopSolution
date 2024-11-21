using Dapper;
using Repository.Models;
using Repository.Repository;
using System.Data;
using System.Data.Common;

namespace WebShop.Repository.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task AddProductsToOrder(int orderId, List<int> productIds);
    Task AddCustomerToOrder(int orderId, int customerId);
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
    public override async Task<int> Add(Order entity)
    {
        var orderParams = new
        {
            entity.OrderDate,
            entity.IsShipped,
        };

        var sql = @"INSERT INTO Orders (OrderDate, IsShipped) VALUES (@OrderDate, @IsShipped); SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var orderId = await _connectionString.ExecuteScalarAsync<int>(sql, orderParams, _transaction);

        return orderId;
    }
    public async Task AddCustomerToOrder(int orderId, int customerId)
    {
        var sql = "UPDATE Orders SET CustomerId = @CustomerId WHERE Id = @OrderId";

        var rowsAffected = await _connectionString.ExecuteAsync(sql, new { CustomerId = customerId, OrderId = orderId }, _transaction);
    }

    public async Task AddProductsToOrder(int orderId, List<int> productIds)
    {
        var tableName = "OrderItems";

        var sql = $"INSERT INTO {tableName} (OrderId, ProductId) VALUES (@OrderId, @ProductId);";

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
