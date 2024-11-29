using Dapper;
using Repository.Models;
using Repository.Repository;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using WebShop.Repository.Models;

namespace WebShop.Repository.Repository;

public interface IOrderRepository : IRepository<Order>
{
    Task AddProductsToOrder(int orderId, List<OrderItem> products);
    Task DeleteAllProductsFromOrder(int orderId);
    Task AddCustomerToOrder(int orderId, int customerId);
    Task RemoveCustomerFromOrder(int orderId);
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
    public override async Task Update(Order entity)
    {
        var orderParams = new
        {
            entity.OrderDate,
            entity.IsShipped,
            entity.Id
        };

        var sql = @"UPDATE Orders SET OrderDate = @OrderDate, IsShipped = @IsShipped WHERE Id = @Id";

        await _connectionString.ExecuteAsync(sql, orderParams, _transaction);
    }
    public async Task AddCustomerToOrder(int orderId, int customerId)
    {
        var sql = "UPDATE Orders SET CustomerId = @CustomerId WHERE Id = @OrderId";

        await _connectionString.ExecuteAsync(sql, new { OrderId = orderId, CustomerId = customerId }, _transaction);
    }
    public async Task RemoveCustomerFromOrder(int orderId)
    {
        var sql = "UPDATE Orders SET CustomerId = NULL WHERE Id = @OrderId";

        await _connectionString.ExecuteAsync(sql, new { OrderId = orderId }, _transaction);
    }

    public async Task AddProductsToOrder(int orderId, List<OrderItem> orderItems)
    {
        var tableName = "OrderItems";

        var existingProductsSql = $"SELECT ProductId, Quantity FROM {tableName} WHERE OrderId = @OrderId AND ProductId IN @ProductIds";

        var existingProducts = await _connectionString.QueryAsync<OrderItem>(existingProductsSql, new { OrderId = orderId, ProductIds = orderItems.Select(p => p.ProductId).ToArray() }, transaction: _transaction);

        var productsToInsert = new List<OrderItem>();
        var productsToUpdate = new List<OrderItem>();

        foreach(var product in orderItems)
        {
            var existingProduct = existingProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct == default)
            {
                productsToInsert.Add(product);
            }
            else
            {
                productsToUpdate.Add(product);
            }
        }
        if(productsToUpdate.Any())
        {
            var updateSql = $"UPDATE {tableName} SET Quantity = @Quantity WHERE OrderId = @OrderId AND ProductId = @ProductId";
            await _connectionString.ExecuteAsync(updateSql, productsToUpdate, transaction: _transaction);
        }
        if (productsToInsert.Any())
        {
            var insertSql = $"INSERT INTO {tableName} (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity);";
            await _connectionString.ExecuteAsync(insertSql, productsToInsert.Select(p => new { OrderId = orderId, p.ProductId, p.Quantity }), transaction: _transaction);
        }

    }

    public async Task DeleteAllProductsFromOrder(int orderId)
    {
        var tableName = "OrderItems";
        var sql = $"DELETE FROM {tableName} WHERE OrderId = @OrderId";
        await _connectionString.ExecuteAsync(sql, new { OrderId = orderId }, transaction: _transaction);
    }

        public async Task UpdateOrderStatus(int orderId, bool isShipped)
    {
        var tableName = "Orders";

        var sql = $"UPDATE {tableName} SET IsShipped = @IsShipped WHERE Id = @OrderId";
        await _connectionString.ExecuteAsync(sql, new { IsShipped = isShipped, OrderId = orderId }, transaction: _transaction);
    }

}
