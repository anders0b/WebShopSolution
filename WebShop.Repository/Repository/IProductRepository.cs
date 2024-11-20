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
    private IDbTransaction _transaction;

    public ProductRepository(IDbConnection connectionString, IDbTransaction transaction) : base(connectionString, transaction)
    {
        _connectionString = connectionString;
        _transaction = transaction;
    }

    public async Task UpdateStockQuantity(int productId, int quantity)
    {
        var tableName = "Products";

        var sql = $"UPDATE {tableName} SET Stock = Stock - @Quantity WHERE Id = @ProductId";
        await _connectionString.ExecuteAsync(sql, new { Quantity = quantity, ProductId = productId }, transaction: _transaction);
    }
}
