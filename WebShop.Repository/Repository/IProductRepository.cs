using Dapper;
using Repository.Models;
using System.Data;
using WebShop.Repository.Models;

namespace Repository.Repository;

public interface IProductRepository : IRepository<Product>
{
     Task UpdateStockQuantity(int productId, int quantity);
     Task<IEnumerable<OrderItem>> GetAllProductsFromOrder(int orderId);
}
public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly IDbConnection _connectionString;
    private IDbTransaction _transaction;

    public ProductRepository(IDbConnection connectionString, IDbTransaction transaction) : base(connectionString, transaction)
    {
        _connectionString = connectionString;
        _transaction = transaction;
    }

    public async Task UpdateStockQuantity(int productId, int quantity)
    {
        var tableName = "Products";

        var sql = $"UPDATE {tableName} SET Stock = @Quantity WHERE Id = @ProductId";
        await _connectionString.ExecuteAsync(sql, new { Quantity = quantity, ProductId = productId }, transaction: _transaction);
    }

    public async Task<IEnumerable<OrderItem>> GetAllProductsFromOrder(int orderId)
    {
        var tableName = "OrderItems";
        var sql = $"SELECT * FROM {tableName} oi JOIN Products p ON oi.ProductId = p.Id WHERE oi.OrderId = @OrderId";
        return await _connectionString.QueryAsync<OrderItem>(sql, new { OrderId = orderId }, transaction: _transaction);
    }
}
