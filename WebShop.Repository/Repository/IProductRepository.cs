using Dapper;
using Repository.Models;
using System.Data;
using System.Transactions;
using WebShop.Repository;

namespace Repository.Repository;

public interface IProductRepository : IRepository<Product>
{
     Task UpdateStockQuantity(int productId, int quantity);
}
public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly IDbConnection _connectionString;
    private readonly IDbTransaction _transaction;

    public ProductRepository(IDbConnection connectionString) : base(connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task UpdateStockQuantity(int productId, int quantity)
    {
        var tableName = "Products";
        using (var connection = _connectionString)
        {
            var sql = $"UPDATE {tableName} SET StockQuantity = StockQuantity - @Quantity WHERE Id = @ProductId";
            await connection.ExecuteAsync(sql, new { ProductId = productId, Quantity = quantity }, _transaction);
        }
    }
}
