using Dapper;
using Repository.Models;
using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task UpdateOrderStatus(int orderId, bool isShipped);
    Task<IEnumerable<Product>> GetAllProductsFromOrder(int orderId);
    Task<Customer> GetCustomerFromOrderId(int orderId);
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
    public async Task UpdateOrderStatus(int orderId, bool isShipped)
    {
        var tableName = "Orders";

        var sql = $"UPDATE {tableName} SET IsShipped = @IsShipped WHERE Id = @OrderId";
        await _connectionString.ExecuteAsync(sql, new { IsShipped = isShipped, OrderId = orderId }, transaction: _transaction);
    }
    public async Task<IEnumerable<Product>> GetAllProductsFromOrder(int orderId)
    {
        var tableName = "OrderProducts";
        var sql = $"SELECT * FROM {tableName} WHERE OrderId = @OrderId";
        return await _connectionString.QueryAsync<Product>(sql, new { OrderId = orderId }, transaction: _transaction);
    }
    public async Task<Customer> GetCustomerFromOrderId(int orderId)
    {
        var tableName = "Orders";
        var sql = $"SELECT * FROM {tableName} WHERE Id = @OrderId";
        var order = await _connectionString.QueryFirstOrDefaultAsync<Order>(sql, new { OrderId = orderId }, transaction: _transaction);
        return await _connectionString.QueryFirstOrDefaultAsync<Customer>($"SELECT * FROM Customers WHERE Id = @CustomerId", new { CustomerId = order.Customer.Id }, transaction: _transaction) ?? default!;
    }
}
